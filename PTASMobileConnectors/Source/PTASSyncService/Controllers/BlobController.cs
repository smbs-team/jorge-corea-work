using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using PTASSyncService.Utilities;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http.Features;
using Swashbuckle.AspNetCore.Annotations;

namespace PTASSyncService.Controllers
{
    /// <summary>
    /// Class to upload and download blobs.
    /// </summary>
    public class BlobController : Controller
    {
        /// <summary>
        /// App configuration settings.
        /// </summary>
        private readonly Settings Configuration;

        /// <summary>
        /// Form options
        /// </summary>
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">App configuration settings.</param>
        public BlobController(Settings config)
        {
            this.Configuration = config;
        }

        /// <summary>
        /// Verifies the services is up.
        /// </summary>
        /// <returns></returns>
        [Route("api/[controller]")]
        [HttpGet]
        [SwaggerOperation(OperationId = "BlobGet")]
        public string Get()
        {
            return "Blob controller: " + DateTime.UtcNow.ToString();
        }
        
        /// <summary>
        /// Downloads a blob from the repository.
        /// </summary>
        /// <param name="id">Token id.</param>
        /// <param name="filename">Name of the file to download.</param>
        /// <returns></returns>
        [Route("api/Blob/Download")]
        [HttpGet]
        [SwaggerOperation(OperationId = "Download")]
        public IActionResult Download([FromQuery]Guid id, [FromQuery]string filename)
        {
            string filePath = System.IO.Path.Combine(Configuration.documentPath, filename);
            if (!System.IO.File.Exists(filePath))
            {
                var result = Json("{error: File not found.");
                result.StatusCode = 404;

                return NotFound();
            }
            else
            {


                return new FileCallbackResult(new MediaTypeHeaderValue("application/octet-stream"), async (outputStream, _) =>
                    {
                        using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, true))
                            await sourceStream.CopyToAsync(outputStream);
                    }
                )
                {
                    FileDownloadName = filename
                };
            }
        }

        // 1. Disable the form value model binding here to take control of handling 
        //    potentially large files.
        // 2. Typically antiforgery tokens are sent in request body, but since we 
        //    do not want to read the request body early, the tokens are made to be 
        //    sent via headers. The antiforgery token filter first looks for tokens
        //    in the request header and then falls back to reading the body.
        /// <summary>
        /// Uploads a blob to the repository.
        /// </summary>
        /// <returns>Ok if successful.</returns>
        [HttpPost]
        [DisableFormValueModelBinding]
        [Route("api/Blob/Upload")]
        [SwaggerOperation(OperationId = "Upload")]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            try
            {
                var section = await reader.ReadNextSectionAsync();
                while (section != null)
                {
                    ContentDispositionHeaderValue contentDisposition;
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            var fileName = contentDisposition.FileName.Substring(1, contentDisposition.FileName.Length - 2);
                            targetFilePath = Path.Combine(Configuration.documentPath, fileName);
                            using (var targetStream = System.IO.File.Create(targetFilePath))
                            {
                                await section.Body.CopyToAsync(targetStream);
                            }
                        }
                        else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                        {
                            // Content-Disposition: form-data; name="key"
                            //
                            // value

                            // Do not limit the key name length here because the 
                            // multipart headers length limit is already in effect.
                            var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                            var encoding = GetEncoding(section);
                            using (var streamReader = new StreamReader(
                                section.Body,
                                encoding,
                                detectEncodingFromByteOrderMarks: true,
                                bufferSize: 1024,
                                leaveOpen: true))
                            {
                                // The value length limit is enforced by MultipartBodyLengthLimit
                                var value = await streamReader.ReadToEndAsync();
                                if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                                {
                                    value = String.Empty;
                                }
                                if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                                {
                                    throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                                }
                            }
                        }
                    }

                    // Drains any remaining section body that has not been consumed and
                    // reads the headers for the next section.
                    section = await reader.ReadNextSectionAsync();
                }
            }
            catch (Exception ex)
            {
                LogClass.ReportError("BlobController", "Upload", ex, Configuration);
            }

            return Ok();
        }

        /// <summary>
        /// Gets the encoding of the blob.
        /// </summary>
        /// <param name="section">Section of the multipart request.</param>
        /// <returns>Encoding type</returns>
        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
   
    
     }


    /// <summary>
    /// Class to handle multipart content.
    /// </summary>
    public class MultipartContent
    {
        public string ContentType { get; set; }

        public string FileName { get; set; }

        public Stream Stream { get; set; }
    }

    /// <summary>
    /// Class to handle multipart result.
    /// </summary>
    public class MultipartResult : Collection<MultipartContent>, IActionResult
    {
        private readonly System.Net.Http.MultipartContent content;

        public MultipartResult(string subtype = "byteranges", string boundary = null)
        {
            if (boundary == null)
            {
                this.content = new System.Net.Http.MultipartContent(subtype);
            }
            else
            {
                this.content = new System.Net.Http.MultipartContent(subtype, boundary);
            }
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var item in this)
            {
                if (item.Stream != null)
                {
                    var content = new StreamContent(item.Stream);

                    if (item.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);
                    }

                    if (item.FileName != null)
                    {
                        var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        contentDisposition.SetHttpFileName(item.FileName);
                        content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    }

                    this.content.Add(content);
                }
            }

            context.HttpContext.Response.ContentLength = content.Headers.ContentLength;
            context.HttpContext.Response.ContentType = content.Headers.ContentType.ToString();

            await content.CopyToAsync(context.HttpContext.Response.Body);
        }
    }

    /// <summary>
    /// Attribute used to disable model binding for the Upload action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var formValueProviderFactory = context.ValueProviderFactories
                .OfType<FormValueProviderFactory>()
                .FirstOrDefault();
            if (formValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(formValueProviderFactory);
            }

            var jqueryFormValueProviderFactory = context.ValueProviderFactories
                .OfType<JQueryFormValueProviderFactory>()
                .FirstOrDefault();
            if (jqueryFormValueProviderFactory != null)
            {
                context.ValueProviderFactories.Remove(jqueryFormValueProviderFactory);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }

    /// <summary>
    /// Helper to handle multipart requests.
    /// </summary>
    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(Microsoft.Net.Http.Headers.MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return "";
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data");
        }

        public static bool HasFileContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data");
        }
    }
}

