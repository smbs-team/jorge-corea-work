// <copyright file="Blob64FilesController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Table;

    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes;
    using PTASIlinxService.Classes.ActionAttributes;
    using PTASIlinxService.Classes.Entities;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller to return the bytes of a blob image.
    /// </summary>
    public class Blob64FilesController : ApiController
    {
        private readonly BlobFilesController blobController;

        /// <summary>
        /// Initializes a new instance of the <see cref="Blob64FilesController"/> class.
        /// </summary>
        /// <param name="provider">Cloud storage provider.</param>
        /// <param name="config">Configuration parameters.</param>
        public Blob64FilesController(ICloudStorageProvider provider, IConfigParams config)
        {
            this.blobController = new BlobFilesController(provider, config);
        }

        /// <summary>
        /// Gets a blob as a json obect.
        /// </summary>
        /// <param name="id">Full id of the blob to get.</param>
        /// <param name="contactId">Contact Id.</param>
        /// <returns>Json object with bytes and file name.</returns>
        /// <remarks>Please do not remove the contactId parameter, it is used in the security filter.</remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        [BearerSecurityChecker("contactId")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Contact Id parameter is used by security and is included for documentation.")]
        public async Task<BlobFileReturn> PostAsync(string id, string contactId)
        {
            var (memStream, rewired) = this.blobController.GetBlobStream(id);
            return await Task.FromResult(memStream == null
              ? null
              : new BlobFileReturn
              {
                  FileBytes = memStream.ToArray(),
                  FileName = rewired,
              });
        }
    }
}