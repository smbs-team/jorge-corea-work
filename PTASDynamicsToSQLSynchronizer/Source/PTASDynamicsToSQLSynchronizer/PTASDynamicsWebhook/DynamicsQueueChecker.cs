// <copyright file="DynamicsQueueChecker.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsWebhook
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    using PTASDynamicsDigesterLibrary;

    /// <summary>
    /// Function to check the queue for insertions.
    /// </summary>
    public static class DynamicsQueueChecker
    {
        private const string Sql = "sqlconnectionstring";
        private const string OrgId = "organizationId";
        private const string OrgName = "organizationName";

        /// <summary>
        /// Runs the function.
        /// </summary>
        /// <param name="myQueueItem">What we received in the queue.</param>
        /// <param name="log">Log.</param>
        /// <returns>A task.</returns>
        [FunctionName("DynamicsQueuChecker")]
        [return: ServiceBus("deltaprocessqueue", Connection = "dynamicsqueueconnection")]
        public static async Task<string> RunAsync([ServiceBusTrigger("dynamicsqueue", Connection = "dynamicsqueueconnection")] string myQueueItem, ILogger log)
        {
            string qItem = myQueueItem;
            string connectionStr = Environment.GetEnvironmentVariable(Sql, EnvironmentVariableTarget.Process);
            string organizationId = Environment.GetEnvironmentVariable(OrgId, EnvironmentVariableTarget.Process);
            string organizationName = Environment.GetEnvironmentVariable(OrgName, EnvironmentVariableTarget.Process);
            log.LogInformation("Queue trigger ");
            var (info, result) = await new DynamicsPayloadDigester(
                connectionStr,
                organizationId,
                organizationName)
                .DigestItem(qItem);
            if (info == null)
            {
                log.LogError(result);
                return null;
            }
            else
            {
                log.LogInformation($"Primary Entity {info.PrimaryEntityName}({info.PrimaryEntityId})");
                log.LogInformation(result.Substring(0, 40) + "...");
                return info.PrimaryEntityName;
            }
        }
    }
}