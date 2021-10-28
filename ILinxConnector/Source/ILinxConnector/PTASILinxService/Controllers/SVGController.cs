// <copyright file="SVGController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Controller for SVG images.
    /// </summary>
    public class SVGController : ApiController
    {
        private readonly CloudBlobContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SVGController"/> class.
        /// </summary>
        /// <param name="cloudProvider">Cloud provider.</param>
        /// <param name="config">System configuration.</param>
        public SVGController(ICloudStorageProvider cloudProvider, IConfigParams config)
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
        /// Gets svg file from the storage using route as filename.
        /// </summary>
        /// <param name="route">Route to load.</param>
        /// <returns>SVG contents.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetSVGAsync(string route)
        {
            route = Path.ChangeExtension(route, "svg");
            var blob = this.container.GetBlockBlobReference(route);
            return CreateResponse(blob.Exists() ? await blob.DownloadTextAsync() : "{}", "text/xml");
        }

        /// <summary>
        /// Receives a json sketch file and converts it to SVG.
        /// </summary>
        /// <returns>Result of conversion.</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            string json = await this.Request.Content.ReadAsStringAsync();
            var sketch = (PTASMobileSketch.SketchControl)JsonConvert.DeserializeObject(json, typeof(PTASMobileSketch.SketchControl), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var svg = PTASMobileSketch.SketchToSVG.Write(sketch);
            return CreateResponse(svg, "image/svg+xml");
        }

        private static HttpResponseMessage CreateResponse(string content, string mediaType)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content),
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return response;
        }
    }
}