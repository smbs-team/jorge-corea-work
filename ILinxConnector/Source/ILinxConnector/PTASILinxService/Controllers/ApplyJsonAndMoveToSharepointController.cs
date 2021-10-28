// <copyright file="ApplyJsonAndMoveToSharepointController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using PTASILinxConnectorHelperClasses.Models;

    using PTASIlinxService.Classes;

    using PTASLinxConnectorHelperClasses.Models;

    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// ApplyJsonAndMoveToSharepointController.
    /// </summary>
    public class ApplyJsonAndMoveToSharepointController : ApiController
    {
        private readonly ApplyJsonToDynamicsController applyController;
        private readonly BlobMoveAbstractController moveController;
        private readonly NotificationSender notifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyJsonAndMoveToSharepointController"/> class.
        /// </summary>
        /// <param name="provider">Cloud provider.</param>
        /// <param name="config">Sys config.</param>
        public ApplyJsonAndMoveToSharepointController(ICloudStorageProvider provider, IConfigParams config)
        {
            this.applyController = new ApplyJsonToDynamicsController(provider, config);
            this.moveController = new BlobMoveToSharepointController(provider, config);
            this.notifier = new NotificationSender(config);
        }

        /// <summary>
        /// Post.
        /// </summary>
        /// <param name="info">Data to post.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IHttpActionResult> PostAsync([FromBody] ApplyJsonAndBlobMoveInfo info)
        {
            return await this.ApplyJsonAndMoveToSharepoint(info);
        }

        private async System.Threading.Tasks.Task<IHttpActionResult> ApplyJsonAndMoveToSharepoint(ApplyJsonAndBlobMoveInfo info, int retry = 0)
        {
            try
            {
                var applyResults = await this.applyController.PostAsync(info.Route);

                if (applyResults.Error)
                {
                    throw new Exception(applyResults.ErrorMessage);
                }

                MoveDocResults moveResults = await this.moveController.PostAsync(info);
                if ((moveResults.Error || moveResults.InsertResults.Any(m => m.Error)) && retry < 3)
                {
                    await Task.Delay(TimeSpan.FromSeconds((retry + 1) * 30));
                    return await this.ApplyJsonAndMoveToSharepoint(info, ++retry);
                }
                else if (retry >= 3)
                {
                    // If errors on more than 3 retries send email with list of errors.
                    this.notifier.SendNotification("Apply Json And Move To Sharepoint Error - Move Doc Results", $"<h1>Global Error</h1><pre>{moveResults.Error}</pre><h2>Internal Errors</h2><pre>{(moveResults.InsertResults != null ? string.Join(";", moveResults.InsertResults.Select(m => m.Error).ToArray()) : string.Empty)}</pre><h3>Payload</h3><pre>{Newtonsoft.Json.JsonConvert.SerializeObject(info)}</pre>");
                }

                return this.Ok(new { applyResults, moveResults });
            }
            catch (Exception ex)
            {
                if (retry < 3)
                {
                    await Task.Delay(TimeSpan.FromSeconds((retry + 1) * 30));
                    return await this.ApplyJsonAndMoveToSharepoint(info, ++retry);
                }
                else
                {
                    // If errors on more than 3 retries send email with list of errors.
                    this.notifier.SendNotification("Apply Json And Move To Sharepoint Error", $"<h1>Error</h1><pre>{ex.Message}</pre><h2>Internal Error</h2><pre>{(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}</pre><h2>StackTrace</h2><pre>{ex.StackTrace}</pre><h3>Payload</h3><pre>{Newtonsoft.Json.JsonConvert.SerializeObject(info)}</pre>");

                    // please note: this is a call and forget, so this has no real effect except when debugging.
                    return this.InternalServerError(ex);
                }
            }
        }
    }
}