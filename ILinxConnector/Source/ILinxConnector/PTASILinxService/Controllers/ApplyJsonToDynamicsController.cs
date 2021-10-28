// <copyright file="ApplyJsonToDynamicsController.cs" company="King County">
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
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Json to dynamics applier serial version.
    /// </summary>
    public class ApplyJsonToDynamicsController : ApiController
    {
        private const string ControllerProperty = "\"controller\"";
        private readonly HttpClient client = new HttpClient();
        private readonly CloudBlobContainer inputContainer;
        private readonly CloudBlobContainer outputContainer;
        private readonly string url;
        private readonly NotificationSender notifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyJsonToDynamicsController"/> class.
        /// </summary>
        /// <param name="provider">Cloud provider.</param>
        /// <param name="config">Configuration params.</param>
        public ApplyJsonToDynamicsController(ICloudStorageProvider provider, IConfigParams config)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.inputContainer = provider.GetCloudBlobContainer(config.JSONContainerName);
            this.outputContainer = provider.GetCloudBlobContainer(config.ProcessedJSONContainerName);
            this.url = config.DynamicsApiURL;
            this.notifier = new NotificationSender(config);
        }

        /// <summary>
        /// Patches a single contact.
        /// </summary>
        /// <param name="route">route to the contact.</param>
        /// <param name="isUpdate">Update or create.</param>
        /// <returns>Result of the operation.</returns>
        /// <remarks>
        /// Route must be a list of guids separated by slash.
        /// If isUpdate is true it will send a patch request, else a post.
        /// </remarks>
        public async Task<StatusResponse> PatchAsync(string route, bool isUpdate)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException("Route should have a value.", nameof(route));
            }

            // note recursive set to false because in this case we only want to process the root element.
            IEnumerable<JsonBlob> contents = this.FilterContents(await this.GetContents(route, false));
            var itemToSend = contents.FirstOrDefault();
            try
            {
                if (itemToSend == null)
                {
                    throw new Exception($"Item not found {route}.");
                }

                await this.MoveItem(itemToSend, isUpdate ? "PATCH" : "POST");
                return new StatusResponse { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new StatusResponse { Error = true, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Post action. Will attempt to apply and then move the json store.
        /// </summary>
        /// <param name="route">Route to apply.</param>
        /// <returns>Nothing yet.</returns>
        public async Task<StatusResponse> PostAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException("Route should have a value.", nameof(route));
            }

            IEnumerable<JsonBlob> contents = (await this.GetContents(route, true)).ToList();

            try
            {
                // ordering by count of slashes will order them hierarchically.
                // The where clause checks that all segments are guids.
                // Whatever that does not comply, we ignore.
                JsonBlob[] sorted = this.FilterContents(contents);
                foreach (JsonBlob item in sorted)
                {
                    await this.MoveItem(item);
                }

                return new StatusResponse { Result = $"Processed {sorted.Count()} items." };
            }
            catch (Exception ex)
            {
                return new StatusResponse
                {
                    Result = "ERROR",
                    Error = true,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// Attempts to delete an item form the storage.
        /// </summary>
        /// <param name="route">Route to delete.</param>
        /// <returns>Void task.</returns>
        protected virtual async Task DeleteItem(string route)
        {
            var rf = this.inputContainer.GetBlockBlobReference(route.RemoveStartingSlash());
            if (await rf.ExistsAsync())
            {
                await rf.DeleteAsync();
            }
        }

        /// <summary>
        /// Get contents from the input container.
        /// </summary>
        /// <param name="route">Route to get.</param>
        /// <param name="recursive">Should we load all leaves.</param>
        /// <returns>List of json blobs loaded.</returns>
        protected virtual async Task<IEnumerable<JsonBlob>> GetContents(string route, bool recursive)
          => await BlobJsonGetter.GetAllContentsAsync(this.inputContainer, route, recursive);

        /// <summary>
        /// Move one item.
        /// </summary>
        /// <param name="item">item to move.</param>
        /// <param name="method">http method.</param>
        /// <returns>void task.</returns>
        protected virtual async Task MoveItem(JsonBlob item, string method = "POST")
        {
            try
            {
                await this.MoveToDynamics(item.Content, method);
                await this.MoveToBackup(item);
                await this.DeleteItem(item.Route);
            }
            catch (Exception ex)
            {
                if (ex is MoveToDynamicsFailException)
                {
                    this.notifier.SendNotification("Error on ApplyJsonToDynamics", ex.Message);
                }

                throw;
            }
        }

        /// <summary>
        /// Move item to backup storage.
        /// </summary>
        /// <param name="blob">Json blob to move.</param>
        /// <returns>Void task.</returns>
        protected virtual async Task MoveToBackup(JsonBlob blob)
          => await this.outputContainer
            .GetBlockBlobReference(blob.Route.RemoveStartingSlash())
            .UploadTextAsync(blob.Content);

        /// <summary>
        /// Attempt to apply json item to dynamics.
        /// </summary>
        /// <param name="jsonToApply">Json string to apply.</param>
        /// <param name="method">http method to apply.</param>
        /// <returns>Void task.</returns>
        protected virtual async Task MoveToDynamics(string jsonToApply, string method = "POST")
        {
            var httpMethod = new HttpMethod(method);
            string controller = this.ExtractController(jsonToApply);
            if (string.IsNullOrEmpty(controller))
            {
                return;
            }

            HttpResponseMessage response = await this.SendMessage(jsonToApply, controller, httpMethod);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode && !content.ToLower().Contains("preconditionfailed"))
            {
                var formatted = JValue.Parse(jsonToApply).ToString(Formatting.Indented);
                throw new MoveToDynamicsFailException($"Failed on controller: {controller} with json: <br><pre>{formatted}</pre>. <br>Response content: <br>{content}", null);
            }
        }

        private JsonBlob[] FilterContents(IEnumerable<JsonBlob> contents)
          => contents
                  .Where(a => a.Content.Contains(ControllerProperty))
                  .Where(a => a.Route.Split(new char[] { '/' }).All(itm => Guid.TryParse(itm, out Guid _)))
                  .OrderBy(a => a.Route.Count(c => c == '/'))
                  .ThenBy(a => this.WeightOf(a))
                  .ThenBy(a => a.Route)
                  .ToArray();

        private int WeightOf(JsonBlob a)
        {
            string yy = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedWithControllerName>(a.Content).ControllerName as string;
            switch (yy)
            {
                case "SEApplicationDetails":
                    return 10;
                default:
                    return 100;
            }
        }

        private string ExtractController(string controller)
                  => JsonConvert.DeserializeObject<SerializedWithControllerName>(controller)?.ControllerName
          ?? string.Empty;

        private async Task<HttpResponseMessage> SendMessage(string jsonToApply, string controller, HttpMethod method1)
        {
            var itemPath = Path.Combine(this.url, controller);
            HttpRequestMessage requestMessage = new HttpRequestMessage(method1, itemPath)
            {
                Content = new StringContent(jsonToApply, Encoding.UTF8, "application/json"),
            };

            string a1 = HttpRequestHeader.Authorization.ToString();
            IEnumerable<string> a2 = HttpContext.Current.Request.Headers.GetValues(a1);
            var authHeader = a2.FirstOrDefault();
            if (authHeader != null)
            {
                requestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), authHeader);
            }

            var response = await this.client.SendAsync(requestMessage);
            return response;
        }
    }
}
