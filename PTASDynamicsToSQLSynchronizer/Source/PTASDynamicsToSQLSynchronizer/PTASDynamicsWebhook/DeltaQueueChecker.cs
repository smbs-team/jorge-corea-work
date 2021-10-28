// <copyright file="DeltaQueueChecker.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsWebhook
{
    using System.Threading.Tasks;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    using PTASDynamicsSyncLibrary;

    /// <summary>
    ///  Check delta queue for n:m changes.
    /// </summary>
    public static class DeltaQueueChecker
    {
        /// <summary>
        /// Triggered when something enters the delta queue.
        /// </summary>
        /// <param name="primaryEntityName">Item to process.</param>
        /// <param name="log">Logger.</param>
        /// <returns>Async Task.</returns>
        [FunctionName("DeltaQueueChecker")]
        public static async Task Run([ServiceBusTrigger("deltaprocessqueue", Connection = "dynamicsqueueconnection")] string primaryEntityName, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {primaryEntityName}");
            try
            {
                // NOTE: keep useBatch in false to do inserts and deletes.
                await new DynamicsUtility(new DeltaConfig())
                    .SyncNM(
                        parentTable: primaryEntityName,
                        timeout: 0,
                        useBatch: false,
                        chunkSize: 50,
                        log: log);
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
            }
        }
    }
}