// <copyright file="BlobFilesController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System.IO;
    using System.Net;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using PTASIlinxService;
    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller for blob files.
    /// </summary>
    public class BlobFilesController : ApiController
    {
        private readonly CloudBlobContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobFilesController"/> class.
        /// </summary>
        /// <param name="provider">Cloud storage provider.</param>
        /// <param name="config">Configuration parameter provider.</param>
        public BlobFilesController(ICloudStorageProvider provider, IConfigParams config)
        {
            this.container = provider.GetCloudBlobContainer(config.BlobStorageContainer);
        }

        /// <summary>
        /// Attempt to delete a blob from storage.
        /// </summary>
        /// <param name="id">Id of the blob to delete.</param>
        /// <returns>Status of deletion.</returns>
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        public StatusResponse Delete(string id)
        {
            string rewired = id.RewireName();
            var blobRef = this.container.GetBlockBlobReference(rewired);
            if (blobRef.Exists())
            {
                blobRef.Delete();
                return new StatusResponse { Result = $"Deleted blob id {id}." };
            }

            throw new FileNotFoundException();
        }

        /// <summary>
        /// Gets the blob file for this item.
        /// </summary>
        /// <param name="id">Id of the item to find.</param>
        /// <returns>HTTP file result carrying the bytes and metadata of the blob.</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        public FileStreamResult Get(string id)
        {
            var (memStream, rewired) = this.GetBlobStream(id);
            if (memStream != null)
            {
                return new FileStreamResult(memStream, $"image/{Path.GetExtension(rewired)}".Replace(".", string.Empty));
            }

            throw new FileNotFoundException();
        }

        /// <summary>
        /// Gets the blob stream using id as key.
        /// </summary>
        /// <param name="id">Item to find.</param>
        /// <returns>Stream representing the item bytes.</returns>
        protected internal virtual (MemoryStream memStream, string rewired) GetBlobStream(string id)
        {
            string otherWired = id.RewireName();
            var blobRef = this.container.GetBlockBlobReference(otherWired);
            MemoryStream memStream = null;
            if (blobRef.Exists())
            {
                memStream = new MemoryStream();
                blobRef.DownloadToStream(memStream);
                memStream.Position = 0;
            }

            return (memStream, otherWired);
        }
    }
}