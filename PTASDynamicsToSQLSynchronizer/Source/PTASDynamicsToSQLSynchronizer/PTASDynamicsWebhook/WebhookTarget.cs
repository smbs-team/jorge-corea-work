// <copyright file="WebhookTarget.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsWebhook
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Static class for webhook target function.
    /// </summary>
    public static class WebhookTarget
    {
        /// <summary>
        /// Function invoked by the webhook.
        /// </summary>
        /// <param name="req">Http request.</param>
        /// <param name="log">Tracer log.</param>
        /// <returns>Action Result.</returns>
        [FunctionName("WebhookTarget")]
        [return: ServiceBus("dynamicsqueue", Connection = "dynamicsqueueconnection")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(jsonContent))
            {
                return jsonContent;
            }
            else
            {
                throw new ArgumentException("Must have content", nameof(jsonContent));
            }
        }
    }
}
