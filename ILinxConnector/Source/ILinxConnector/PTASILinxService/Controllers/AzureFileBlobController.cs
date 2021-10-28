// <copyright file="AzureFileBlobController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller for inserting blob. Only has a post command.
    /// </summary>
    public class AzureFileBlobController : ApiController
    {
        private readonly ICloudStorageProvider cloudProvider;
        private readonly IConfigParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFileBlobController"/> class.
        /// </summary>
        /// <param name="config">System configuration.</param>
        /// <param name="cloudProvider">Provider for cloud operations.</param>
        /// <param name="imageAnalizer">Who analyzes images.</param>
        public AzureFileBlobController(IConfigParams config, ICloudStorageProvider cloudProvider, IImageAnalysisHelper imageAnalizer)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.cloudProvider = cloudProvider ?? throw new ArgumentNullException(nameof(cloudProvider));
            this.ImageAnalizer = imageAnalizer ?? throw new ArgumentNullException(nameof(imageAnalizer));
        }

        private IImageAnalysisHelper ImageAnalizer { get; }

        /// <summary>
        /// Attempts to upload an image to the blob storage.
        /// </summary>
        /// <returns>Result of insertion.</returns>
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [SwaggerOperationFilter(typeof(AzureFileBlobControllerFilter))]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        [HttpPost]
        public async Task<AzureBlobResponse> PostAsync()
        {
            HttpFileCollectionBase files = this.GetFiles();
            NameValueCollection form = this.GetForm();

            var seniorApplicationId = form["seniorApplicationId"];
            var seniorApplicationDetailsId = form["seniorApplicationDetailsId"];
            var section = form["section"];
            var document = form["document"];
            var requestUri = this.GetRequestUri();
            var absoluteUri = this.GetAbsoluteUri(requestUri);
            var localPath = this.GetLocalPath(requestUri);
            var fileUris = absoluteUri.Replace(localPath, string.Empty);

            // note: default value is false.
            var checkImage = (form["checkImage"] ?? "false").ToLower() == "true";

            try
            {
                if (files.Count < 1)
                {
                    throw new WrongNumberOfFilesException(1, 0);
                }

                ////CheckNull(seniorApplicationId, nameof(seniorApplicationDetailsId));
                CheckNull(section, nameof(section));
                CheckNull(document, nameof(document));
                var tail = $"/{section}/{document}/";
                string route = string.IsNullOrEmpty(seniorApplicationDetailsId)
                  ? $"{seniorApplicationId}{tail}"
                  : $"{seniorApplicationId}/{seniorApplicationDetailsId}{tail}";
                CloudBlobContainer container = this.GetStorageContainer();
                IEnumerable<UploadFileResult> results =
                   (await this.SaveFiles(files, route, container, checkImage))
                   .Select(f => f.Error ? f : new UploadFileResult // note: convert url only if no error.
                   {
                       Error = f.Error,
                       ErrorMessage = f.ErrorMessage,
                       FileRoute = $"{fileUris}/v1.0/api/blob64files/?Id={f.FileRoute.Replace(@"/", ".")}",
                   }).ToArray();
                return new AzureBlobResponse
                {
                    Files = results,
                };
            }
            catch (Exception ex)
            {
                return new AzureBlobResponse
                {
                    Error = true,
                    ErrorMessage = ex.Message,
                    Files = new UploadFileResult[] { },
                };
            }
        }

        /// <summary>
        /// Get abs uri.
        /// </summary>
        /// <param name="requestUri">Original uri.</param>
        /// <returns>abs uri.</returns>
        protected virtual string GetAbsoluteUri(Uri requestUri)
          => requestUri.AbsoluteUri;

        /// <summary>
        /// Retrieve files from the request context.
        /// </summary>
        /// <returns>List of files.</returns>
        protected virtual HttpFileCollectionBase GetFiles() => new HttpFileCollectionWrapper(HttpContext.Current.Request.Files);

        /// <summary>
        /// Get form values.
        /// </summary>
        /// <returns>Name value collection with form results.</returns>
        protected virtual NameValueCollection GetForm() => HttpContext.Current.Request.Form;

        /// <summary>
        /// Returns local path.
        /// </summary>
        /// <param name="requestUri">original uri.</param>
        /// <returns>Local path.</returns>
        protected virtual string GetLocalPath(Uri requestUri) => requestUri.LocalPath;

        /// <summary>
        /// Gets an uri to current request.
        /// </summary>
        /// <returns>Uri formatted.</returns>
        protected virtual Uri GetRequestUri() => this.Request.RequestUri;

        /// <summary>
        /// Get a storage container for further processing.
        /// </summary>
        /// <returns>A created storage container from config.</returns>
        protected virtual CloudBlobContainer GetStorageContainer() => this.cloudProvider.GetCloudBlobContainer(this.config.BlobStorageContainer);

        /// <summary>
        /// Attempts ti save the files to the blob storage.
        /// </summary>
        /// <param name="files">Files to save.</param>
        /// <param name="route">Route to save files into.</param>
        /// <param name="container">Blob container to save to.</param>
        /// <param name="checkFiles">Do we need to check image first?.</param>
        /// <returns>URL of added files.</returns>
        protected virtual async Task<IEnumerable<UploadFileResult>> SaveFiles(HttpFileCollectionBase files, string route, CloudBlobContainer container, bool checkFiles)
        {
            var uploadResults = new List<UploadFileResult>();
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase postedFile = files[i];
                var fname = postedFile.FileName;

                string[] splitted = fname.Split('.');
                var butLast = splitted.ButLast();
                var extension = splitted.Last();
                fname = string.Join("_", butLast) + "." + extension;
                if ((extension?.ToLower() ?? string.Empty).Contains("pdf"))
                {
                    // PDF's require special treatment, must be separated into files and then saved.
                    var pdfFiles = postedFile.InputStream.GetPDFFiles(this.config.SautinLicense);
                    byte[][] vs = pdfFiles.Images.ToArray();
                    for (int j = 0; j < vs.Length; j++)
                    {
                        string newName = fname.Replace(".pdf", $"-pdf-page-{j}.png");
                        uploadResults.Add(await this.UploadBytes(
                          container,
                          checkFiles,
                          newName,
                          route + newName,
                          vs[j]));
                    }
                }
                else
                {
                    // None-pdf file, just upload with it's original name.
                    string fileRoute = route + fname;
                    Stream inputStream = postedFile.InputStream;
                    byte[] bytes = await StreamToBytes(inputStream);
                    uploadResults.Add(await this.UploadBytes(container, checkFiles, fname, fileRoute, bytes));
                }
            }

            return uploadResults;
        }

        private static void CheckNull(string field, string fieldName)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException($"{fieldName} input cannot be empty.", fieldName);
            }
        }

        private static UploadFileResult SaveBlob(CloudBlobContainer container, string fileRoute, byte[] bytes)
        {
            UploadFileResult itemToAdd;
            var blobRef = container.GetBlockBlobReference(fileRoute);
            blobRef.UploadFromStream(new MemoryStream(bytes));
            itemToAdd = new UploadFileResult
            {
                FileRoute = fileRoute,
            };
            return itemToAdd;
        }

        private static async Task<byte[]> StreamToBytes(Stream inputStream)
        {
            var m = new MemoryStream();
            await inputStream.CopyToAsync(m);
            var bytes = m.ToArray();
            return bytes;
        }

        private async Task<UploadFileResult> UploadBytes(
          CloudBlobContainer container,
          bool checkFiles,
          string fname,
          string fileRoute,
          byte[] bytes)
        {
            if (checkFiles)
            {
                var (isAcceptable, errorMessage) = await this.ImageAnalizer.ImageIsAcceptable(bytes);
                if (!isAcceptable)
                {
                    return new UploadFileResult
                    {
                        FileRoute = fname,
                        Error = true,
                        ErrorMessage = errorMessage,
                    };
                }
            }

            return SaveBlob(container, fileRoute, bytes);
        }
    }
}