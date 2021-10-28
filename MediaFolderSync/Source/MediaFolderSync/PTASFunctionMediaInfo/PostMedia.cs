// <copyright file="PostMedia.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASFunctionMediaInfo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MediaInfo;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    using PTASImageManipulation;

    /// <summary>
    /// Posts a file to media storage.
    /// </summary>
    public static class PostMedia
    {
        /// <summary>
        /// Main function call.
        /// </summary>
        /// <param name="req">Http Request.</param>
        /// <param name="log">Output log.</param>
        /// <returns>Result of operation.</returns>
        [FunctionName("PostMedia")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                Stream readStream;
                string file = req.Query["fileUrl"];
                if (!string.IsNullOrEmpty(file))
                {
                    var s = await new HttpClient().GetAsync(file);
                    if (!s.IsSuccessStatusCode)
                    {
                        throw new PostMediaException(await s.Content.ReadAsStringAsync(), null);
                    }

                    readStream = await s.Content.ReadAsStreamAsync();
                }
                else
                {
                    if (!req.Form.Files.Any())
                    {
                        throw new PostMediaException("Requires at least one file to post.", null);
                    }

                    var requestBody = req.Form.Files[0];
                    readStream = requestBody.OpenReadStream();
                }

                string entityStr = req.Query["entityId"];
                string storageConnectionString = Environment.GetEnvironmentVariable("BlobStorage");
                string media_url = Environment.GetEnvironmentVariable("media-url");

                if (entityStr == null)
                {
                    throw new PostMediaException("Entity is required in the query.", null);
                }

                var fname = await MediaStorageHelper.SaveNewFile(storageConnectionString, entityStr, readStream);
                IEnumerable<(string fileName, Stream stream)> otherFiles = GetOtherFiles(entityStr, readStream).ToList();

                _ = await Task.WhenAll(otherFiles.Select(of => MediaStorageHelper.SaveNewFile(storageConnectionString, of.fileName, of.stream)));

                return new OkObjectResult(new
                {
                    fileName = fname,
                    imageDates = GetImageProperties(entityStr, readStream),
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private static ImageReturnValue[] GetImageProperties(string fileName, Stream readStream)
        {
            var resultData = new List<ImageReturnValue>(new ImageReturnValue[]
            {
                new ImageReturnValue
                {
                        Date = DateTime.Now.ToUniversalTime().ToString(),
                        Type = "Uploaded",
                },
            });

            string extension = Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();
            var found = ImageUtilities.Extensions.Any(ext => ext.extension.Equals(extension));
            if (found)
            {
                readStream.Position = 0;
                Image img = Image.FromStream(readStream, false, false);
                var itms = GetDateTypes();

                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                var ppp = img.PropertyItems.Select(itm =>
                                       {
                                           return new
                                           {
                                               value = encoding.GetString(itm.Value),
                                               description = itms
                                                   .TryGetValue(itm.Id, out string foundItm)
                                                       ? foundItm
                                                       : string.Empty,
                                               /* $"Id: {itm.Id}"*/
                                               itm,
                                           };
                                       }).ToList();
                var filteredProperties = ppp
                    .Where(itm => !string.IsNullOrEmpty(itm.description))
                    .Select(itm => new ImageReturnValue
                    {
                        Date = encoding.GetString(itm.itm.Value).Replace("\0", string.Empty),
                        Type = itm.description,
                    }).ToList();
                resultData.AddRange(filteredProperties);
            }

            return resultData.ToArray();
        }

        private static Dictionary<int, string> GetDateTypes()
        {
            return new Dictionary<int, string>
                {
                    { 0x0132, "Created" },
                    { 0x9003, "Generated" },
                    { 0xc71b, "Rendered" },
                    { 0x9004, "Stored" },
                };
        }

        private static IEnumerable<(string, Stream)> GetOtherFiles(string entityStr, Stream readStream)
        {
            var extension = Path.GetExtension(entityStr).ToLower().Replace(".", string.Empty);

            var fileName = Path.GetFileName(entityStr).ToLower();
            readStream.Position = 0;
            var nFileName = fileName.Replace("." + extension, string.Empty);

            if (extension == "svg")
            {
                yield return CreateStream(readStream, extension, nFileName, "small");
                yield return CreateStream(readStream, extension, nFileName, "med");
            }
            else
            {
                Image img = Image.FromStream(readStream, false, false);
                Stream stream = ImageUtilities.GetImageStream(img, 200, extension);
                if (stream != null)
                {
                    yield return ($"{nFileName}-small.{extension}", stream);
                    Stream medStream = ImageUtilities.GetImageStream(img, 768, extension);
                    if (medStream != null)
                    {
                        yield return ($"{nFileName}-med.{extension}", medStream);
                    }
                }
            }
        }

        private static (string, MemoryStream memStream) CreateStream(Stream readStream, string extension, string nFileName, string size)
        {
            var memStream = new MemoryStream();
            readStream.CopyTo(memStream);
            return ($"{nFileName}-{size}.{extension}", memStream);
        }

        private class ImageReturnValue
        {
            public string Date { get; internal set; }

            public string Type { get; internal set; }
        }
    }
}