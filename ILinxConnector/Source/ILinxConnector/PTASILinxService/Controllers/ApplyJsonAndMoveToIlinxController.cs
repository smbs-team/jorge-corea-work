// <copyright file="ApplyJsonAndMoveToIlinxController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Web.Http;

    using ILinxSoapImport;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Controller ApplyJsonAndMoveToIlinxController.
    /// </summary>
    public class ApplyJsonAndMoveToIlinxController : ApiController
    {
        private readonly ApplyJsonToDynamicsController applyController;
        private readonly BlobMoveAbstractController moveController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyJsonAndMoveToIlinxController"/> class.
        /// </summary>
        /// <param name="provider">Cloud provider.</param>
        /// <param name="config">Sys configuration.</param>
        /// <param name="iLinx">ilinx manager.</param>
        public ApplyJsonAndMoveToIlinxController(ICloudStorageProvider provider, IConfigParams config, IILinxHelper iLinx)
        {
            this.applyController = new ApplyJsonToDynamicsController(provider, config);
            this.moveController = new BlobMoveController(provider, iLinx, config);
        }

        /// <summary>
        /// Post method.
        /// </summary>
        /// <param name="info">Info to be posted.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async System.Threading.Tasks.Task<IHttpActionResult> PostAsync([FromBody] ApplyJsonAndBlobMoveInfo info)
        {
            try
            {
                var applyResults = await this.applyController.PostAsync(info.Route);
                if (applyResults.Error)
                {
                    throw new Exception(applyResults.ErrorMessage);
                }

                MoveDocResults moveResults = await this.moveController.PostAsync(info);
                return this.Ok(new { applyResults, moveResults });
            }
            catch (Exception ex)
            {
                // please note: this is a call and forget, so this has no real effect except when debugging.
                return this.InternalServerError(ex);
            }
        }
    }
}