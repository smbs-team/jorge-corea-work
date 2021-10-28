// <copyright file="ErrorReportingController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudCore.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Controller for error reporting.
    /// </summary>
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ErrorReportingController : ControllerBase
    {
        private readonly IConfigurationParams config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReportingController"/> class.
        /// </summary>
        /// <param name="config">Configuration values.</param>
        public ErrorReportingController(IConfigurationParams config)
        {
            this.config = config;
        }

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<object> PostAsync([FromBody] ErrorMessage message)
        {
            try
            {
                var t = new NotificationProvider(
                    this.config.ApimEndpoint,
                    this.config.NotificatioServiceResourceId,
                    this.config.ApimSubscriptionKey,
                    this.config.MagiclinkserviceClientId,
                    this.config.MagiclinkserviceClientSecret,
                    this.config.UndeliverableEmailRecipient);

                await t.SendNotification(
                    this.config.FromEmailAddress,
                    this.config.ToEmailAddress,
                    message.MessageSubject,
                    message.MessageBody);
                return new { result = "OK" };
            }
            catch (Exception ex)
            {
                return new
                {
                    result = "KO",
                    error = new
                    {
                        message = $"{ex.Message} {ex.InnerException?.Message}".Trim(),
                    },
                };
            }
        }
    }
}