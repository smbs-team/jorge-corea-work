// <copyright file="SketchToJsonController.cs" company="King County">
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
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using PTASIlinxService.Classes.Exceptions;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Convert a sketch html into json.
    /// </summary>
    public class SketchToJsonController : ApiController
    {
        private readonly CloudBlobContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SketchToJsonController"/> class.
        /// </summary>
        /// <param name="cloudProvider">Cloud provider.</param>
        /// <param name="config">System configuration.</param>
        public SketchToJsonController(ICloudStorageProvider cloudProvider, IConfigParams config)
        {
            if (cloudProvider is null)
            {
                throw new ArgumentNullException(nameof(cloudProvider));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.container = cloudProvider.GetCloudBlobContainer(config.SketchContainer);
        }

        /// <summary>
        /// Deletes all related items to a blob.
        /// </summary>
        /// <param name="route">Route to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAsync(string route)
        {
            var name = Path.GetFileNameWithoutExtension(route);
            var x = this.container.ListBlobs(name, true, BlobListingDetails.None).ToList();
            _ = await this.DeleteBlobsAsync(x);
            return CreateResponse("Ok");
        }

        /// <summary>
        /// Finds an XML in the blob and converts it to JSON.
        /// </summary>
        /// <param name="route">File Route, example: file.xml | dir/file.xml.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        /// <exception cref="XMLToJSonException">When the process of converting to json fails.</exception>
        public async Task<HttpResponseMessage> GetAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException("message", nameof(route));
            }

            try
            {
                // first see if there's an already produced json file.
                string jsonInfo = await this.GetJsonFromFileAsync(route);
                if (string.IsNullOrEmpty(jsonInfo))
                {
                    // json not found, proceed to decode XML.
                    var xmlContents = await this.GetBlobText(XMLPath(route));
                    if (string.IsNullOrEmpty(xmlContents))
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }

                    var idx = xmlContents.IndexOf('<');
                    if (idx > 0)
                    {
                        xmlContents = xmlContents.Remove(0, idx);
                    }

                    var sketch = PTASMobileSketch.SketchFromVCADD.Read(xmlContents);
                    jsonInfo = JsonConvert.SerializeObject(sketch, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    await this.SaveJSONAndSVG(route, jsonInfo);
                }

                return CreateResponse(jsonInfo);
            }
            catch (Exception ex)
            {
                throw new XMLToJSonException(ex);
            }
        }

        /// <summary>
        /// Post new sketch.
        /// </summary>
        /// <param name="route">Route to post to.</param>
        /// <returns>Posted sketch.</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentException("Route missing.", nameof(route));
            }

            var content = await this.Request.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("No content.", nameof(content));
            }

            await this.SaveJSONAndSVG(route, content);

            HttpResponseMessage response = CreateResponse(content);
            return response;
        }

        /// <summary>
        /// Creates the tasks to delete a batch of blobs.
        /// </summary>
        /// <param name="files">Files to delete.</param>
        /// <returns>Tasks for deletion.</returns>
        protected virtual async Task<bool[]> DeleteBlobsAsync(IEnumerable<IListBlobItem> files)
            => await Task.WhenAll(files.Select(blob
                => blob is CloudBlob b ? b.DeleteIfExistsAsync() : Task.FromResult(false)));

        /// <summary>
        /// Attempt to get a text from the blob container.
        /// </summary>
        /// <param name="route">Route to get from.</param>
        /// <returns>Task string or null.</returns>
        protected virtual async Task<string> GetBlobText(string route)
        {
            var blob = this.container.GetBlockBlobReference(route);
            return blob.Exists() ? await blob.DownloadTextAsync() : null;
        }

        /// <summary>
        /// Attempts to get the json file directly.
        /// </summary>
        /// <param name="route">Route to look for.</param>
        /// <returns>File content if found.</returns>
        protected virtual async Task<string> GetJsonFromFileAsync(string route)
        {
            return await this.GetBlobText(JsonPath(route));
        }

        /// <summary>
        /// Save to json and svg.
        /// </summary>
        /// <param name="route">Route to save to.</param>
        /// <param name="content">Content to save.</param>
        /// <returns>Task.</returns>
        protected virtual async Task SaveJSONAndSVG(string route, string content)
        {
            var jsonBlob = this.container.GetBlockBlobReference(JsonPath(route));
            var svgBlob = this.container.GetBlockBlobReference(SVGPath(route));
            string svg = ConvertToSVG(content);
            await Task.WhenAll(svgBlob.UploadTextAsync(svg), jsonBlob.UploadTextAsync(content));
        }

        private static string ConvertToSVG(string content)
        {
            var sketch = (PTASMobileSketch.SketchControl)JsonConvert.DeserializeObject(
                content,
                typeof(PTASMobileSketch.SketchControl));
            return PTASMobileSketch.SketchToSVG.Write(sketch);
        }

        private static HttpResponseMessage CreateResponse(string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content),
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/json");
            return response;
        }

        private static string JsonPath(string route) => Path.ChangeExtension(route, ".json");

        private static string XMLPath(string route) => Path.ChangeExtension(route, ".xml");

        private static string SVGPath(string route) => Path.ChangeExtension(route, ".svg");
    }
}