//-----------------------------------------------------------------------
// <copyright file="SignalRFunctions.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PTASNegotiateSRSemaW
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Extensions.SignalRService;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Azure function class for the SignalRFunctions service.  This service negotiates and notifies messages using Azure SignalR.
    /// </summary>
    public static class SignalRFunctions
    {
        /// <summary>
        /// Negotiates and returns the SignalR connection information.
        /// </summary>
        /// <param name="req">The Http request.</param>
        /// <param name="connectionInfo">The connection information.</param>
        /// <returns>The SignalR connection information.</returns>
        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "sema")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        /// <summary>
        /// Uses SignalR to send the received message to the respective user.
        /// </summary>
        /// <param name="req">The Http request.</param>
        /// <param name="questionR">The SignalR message.</param>
        /// <param name="log">The logger element.</param>
        /// <returns>A response with the respective message.</returns>
        [FunctionName("Notify")]
        public static async Task<IActionResult> Run(
                [HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "post",
                    Route = "notify")]
                HttpRequest req,
                [SignalR(HubName = "sema")]
                IAsyncCollector<SignalRMessage> questionR,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for SEMA SingalR.");

            try
            {
                string json = null;
                try
                {
                    json = await new StreamReader(req.Body).ReadToEndAsync();
                }
                catch (ArgumentException argEx)
                {
                    log.LogError(argEx, "Invalid argument level exception");
                    return new BadRequestObjectResult("Body value must not be null or empty.");
                }
                catch (ObjectDisposedException objEx)
                {
                    log.LogError(objEx, "Function level exception");
                    ObjectResult result = new ObjectResult("There was an error processing the body information.")
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    return result;
                }
                catch (InvalidOperationException invEx)
                {
                    log.LogError(invEx, "Function level exception");
                    ObjectResult result = new ObjectResult("There was an error processing the body information.")
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    return result;
                }

                dynamic obj = JsonConvert.DeserializeObject(json);

                var name = obj.name.ToString();
                var text = obj.text.ToString();
                var target = obj.target.ToString();

                var jObject = new JObject(obj);

                await questionR.AddAsync(
                    new SignalRMessage
                    {
                        Target = target,
                        Arguments = new[] { name, text }
                    });

                return new OkObjectResult($"Hello {name}, your message was '{text}' to target '{target}'");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Function level exception");
                ObjectResult result = new ObjectResult("There was an error processing the request.")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return result;
            }
        }
    }
}
