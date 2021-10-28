// <copyright file="JsonStoreController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.ActionAttributes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller to store json in blobs.
    /// </summary>
    public class JsonStoreController : ApiController
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public const string NoRoot = "Cannot use the root directory.";

        /// <summary>
        /// Error message.
        /// </summary>
        public const string PayloadRequired = "Payload must have content.";

        /////// <summary>
        /////// Name of the processed container.
        /////// </summary>
        ////public const string ProcessedContainerName = "json-store-processed";

        /// <summary>
        /// Error message.
        /// </summary>
        public const string RouteRequired = "Route cannot be empty.";

        private readonly HttpClient client = new HttpClient();
        private readonly IConfigParams config;
        private readonly CloudBlobContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStoreController"/> class.
        /// </summary>
        /// <param name="provider">Cloud provider.</param>
        /// <param name="config">System configurations.</param>
        public JsonStoreController(ICloudStorageProvider provider, IConfigParams config)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.container = provider.GetCloudBlobContainer(config.JSONContainerName);
            ////this.processedContainer = provider.GetCloudBlobContainer(config.ProcessedJSONContainerName);
            this.config = config;
        }

        /// <summary>
        /// Delete from the json store.
        /// </summary>
        /// <param name="route">Route to delete from.</param>
        /// <param name="isDirectory">Confirm deletion of directory route.</param>
        /// <returns>Result of deletion.</returns>
        /// <remarks>
        /// The param isDirectory must be set to true if the route resolves
        /// to a directory and not a physical file.
        /// </remarks>
        [SwaggerResponse(HttpStatusCode.OK, "Result of deletion", typeof(DeleteBatchResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [BearerSecurityChecker("route")]
        public async Task<object> DeleteAsync(
          string route, bool isDirectory = false)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException(RouteRequired, nameof(route));
            }

            if (route.Split('/').Length < 2)
            {
                throw new ArgumentException(NoRoot, nameof(route));
            }

            try
            {
                IEnumerable<IListBlobItem> files = this.GetBlobsToDelete(route, isDirectory);
                bool[] result = await this.DeleteBlobsAsync(files);
                return new DeleteBatchResult
                {
                    Result = "ok",
                    Files = files.Select((f, idx) => new DeleteResult
                    {
                        Path = f.Uri.LocalPath,
                        Deleted = result[idx],
                    }),
                };
            }
            catch (Exception ex)
            {
                throw new BlobFailException("Delete", route, ex);
            }
        }

        /// <summary>
        /// Gets JSON or text content from a route.
        /// </summary>
        /// <param name="route">Route to get.</param>
        /// <returns>Json string fetched from the storage.</returns>
        /// <remarks>
        /// If the route resolves into file, it will return just the contents of the file.
        /// If not, it will return a JSON array with each item containing a route for the item and the item itself.
        /// </remarks>
        [SwaggerResponse(HttpStatusCode.OK, "JSON string stored", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [BearerSecurityChecker("route", true)]
        public async Task<HttpResponseMessage> GetAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException(RouteRequired, nameof(route));
            }

            try
            {
                string blobContent = await this.GetBlobContent(route);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(blobContent),
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
                return response;
            }
            catch (Exception ex)
            {
                throw new BlobFailException("Get", route, ex);
            }
        }

        /// <summary>
        /// Save a json file into the file blob.
        /// </summary>
        /// <returns>Status of creation.</returns>
        /// <param name="route">Route to look for.</param>
        /// <remarks>
        /// If the incoming route is A/B, it will create a file called A/B/B.
        /// Always the filename will be the same as the directory name.
        /// </remarks>
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [SwaggerResponse(HttpStatusCode.OK, "Status code OK.", typeof(StatusResponse))]
        [BearerSecurityChecker("route", true)]
        public async Task<StatusResponse> PostAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException(RouteRequired, nameof(route));
            }

            string[] parts = route.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string lastPart = parts.Last();
            string x = string.Join("/", parts);
            route = $"{x}/{lastPart}";

            var requestContent = await this.GetRequestContent();
            if (string.IsNullOrEmpty(requestContent))
            {
                throw new ArgumentException(PayloadRequired, "payload");
            }

            try
            {
                await this.SaveContent(route, requestContent);
                return new StatusResponse { Result = "ok" };
            }
            catch (Exception ex)
            {
                throw new BlobFailException("Post", route, ex);
            }
        }

        /// <summary>
        /// Another post.
        /// </summary>
        /// <param name="route">Route to send to server.</param>
        /// <returns>Nothing.</returns>
        [BearerSecurityChecker("route")]
        public async Task<object> PutAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException(nameof(route));
            }

            string[] toPostcontents;
            try
            {
                toPostcontents = (await BlobJsonGetter.GetAllContentsAsync(this.container, route))
                  .OrderBy(a => a.Route)
                  .Select((a, b) => a.Content)
                  .ToArray();
            }
            catch (GetAllContentsFailException ex)
            {
                throw new BlobFailException("Put", route, ex);
            }

            try
            {
                var results = this.MoveToDynamics(toPostcontents);
                return new
                {
                    files = toPostcontents,
                    results,
                };
            }
            catch (MoveToDynamicsFailException ex)
            {
                throw new DynamicsFailException("Apply json", route, ex);
            }
        }

        /// <summary>
        /// Creates the tasks to delete a batch of blobs.
        /// </summary>
        /// <param name="files">Files to delete.</param>
        /// <returns>Tasks for deletion.</returns>
        protected virtual async Task<bool[]> DeleteBlobsAsync(IEnumerable<IListBlobItem> files)
        {
            var tasks = files.Select(blob =>
            {
                if (blob is CloudBlob b)
                {
                    return b.DeleteIfExistsAsync();
                }

                return Task.FromResult(false);
            }).ToArray();
            var result = await Task.WhenAll(tasks);
            return result;
        }

        /// <summary>
        /// Attempts to get the content off of a blob.
        /// </summary>
        /// <param name="route">Route to get.</param>
        /// <returns>String with contents.</returns>
        protected virtual async Task<string> GetBlobContent(string route)
        {
            var blob = this.container.GetBlockBlobReference(route);
            var blobContent = blob.Exists() ? await blob.DownloadTextAsync() : await this.TryGetDirectoryAsync(route);
            return blobContent;
        }

        /// <summary>
        /// Get a list of blobs to delete from the storage.
        /// </summary>
        /// <param name="route">Route to fetch.</param>
        /// <param name="isDirectory">Confirm that this is a directory.</param>
        /// <returns>A list of blob items.</returns>
        protected virtual IEnumerable<IListBlobItem> GetBlobsToDelete(string route, bool isDirectory)
        {
            if (isDirectory)
            {
                IListBlobItem[] items = this.container.GetDirectoryReference(route).ListBlobs(true).ToArray();
                return items;
            }

            return new List<IListBlobItem>
      {
        this.container.GetBlockBlobReference(route),
      };
        }

        /// <summary>
        /// Get post content.
        /// </summary>
        /// <returns>Post content.</returns>
        protected virtual async Task<string> GetRequestContent()
        {
            return await this.Request.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Try to save content to blob.
        /// </summary>
        /// <param name="route">Route to save to.</param>
        /// <param name="requestContent">Content to save.</param>
        /// <returns>Task.</returns>
        protected virtual async Task SaveContent(string route, string requestContent)
        {
            CloudBlockBlob blobRef = this.container.GetBlockBlobReference(route);
            await blobRef.UploadTextAsync(requestContent);
        }

        private string ExtractController(string p)
        {
            var x = JsonConvert.DeserializeObject<SerializedWithControllerName>(p);
            if (x?.ControllerName != null)
            {
                return x.ControllerName + (x.ControllerName.EndsWith("s") ? string.Empty : "s");
            }

            throw new NoControllerException(p);
        }

        private IEnumerable<StatusResponse> MoveToDynamics(string[] toPostcontents)
        {
            var url = this.config.DynamicsApiURL;
            var method = new HttpMethod("POST");

            // move to dynamics
            var result = toPostcontents.Select(jsonStr =>
            {
                string controller = this.ExtractController(jsonStr);
                var itemPath = Path.Combine(url, controller);
                var response = this.client.SendAsync(new HttpRequestMessage(method, itemPath)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "application/json"),
                }).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new MoveToDynamicsFailException($"Error status code: {response.StatusCode}. {response.ReasonPhrase}. JSON: \n${jsonStr}", null);
                }

                var content = response.Content.ReadAsStringAsync().Result;
                return new StatusResponse { Error = false, Result = content };
            }).ToArray();
            return result;
        }

        /// <summary>
        /// If the route is a directory, then we concatenate all jsons into
        /// a json array with a relative path to it.
        /// </summary>
        /// <param name="route">Route to look for.</param>
        /// <returns>A Json array string.</returns>
        private async Task<string> TryGetDirectoryAsync(string route)
        {
            IEnumerable<JsonBlob> contents = await BlobJsonGetter.GetAllContentsAsync(this.container, route);
            var jsonsWithUrls = contents.Select((itm, idx)
                =>
            {
                return $"{{\"path\":\"{itm.Route}\", \"json\":{itm.Content}}}";
            });
            var withCommas = string.Join(",", jsonsWithUrls);
            var jsonArray = $"[{withCommas}]";
            return jsonArray;
        }
    }
}