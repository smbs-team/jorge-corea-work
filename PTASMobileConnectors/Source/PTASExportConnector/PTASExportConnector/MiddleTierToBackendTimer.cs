// <copyright file="MiddleTierToBackendTimer.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using PTASExportConnector.SDK;

    /// <summary>
    ///   <para>Gets the necessary data to send.</para>
    /// </summary>
    public class MiddleTierToBackendTimer
    {
        private readonly IConnector connector;
        private readonly IDbService dbService;

        /// <summary>Initializes a new instance of the <see cref="MiddleTierToBackendTimer"/> class.</summary>
        /// <param name="connector">  It has the connector SDK functionality.</param>
        /// <param name="dbService">  It has database related functionality.</param>
        public MiddleTierToBackendTimer(IConnector connector, IDbService dbService)
        {
            this.connector = connector;
            this.dbService = dbService;
        }

        /// <summary>Check for changes every X minutes and calls another trigger to initiate the migration.</summary>
        /// <param name="myTimer">  Timer object.</param>
        /// <param name="log">  Used for logging.</param>
        /// <param name="context">
        ///   <para>
        ///  Used to pass data to another trigger.</para>
        /// </param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> representing the asynchronous operation.</returns>
        [FunctionName("MiddleTierToBackendTimer")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log, [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {context.CurrentUtcDateTime}");

            this.connector.Init();

            log.LogTrace("Getting the device list...");
            var deviceList = this.connector.GetDeviceGuidList();

            if (deviceList.Count() == 0)
            {
                log.LogTrace("There is nothing new to move back to the back-end.");
                Console.WriteLine("There is nothing new to move back to the back-end.");
            }
            else
            {
                log.LogTrace("Synchronizing {deviceList} device(s)", deviceList.Count());
                Console.WriteLine($" {deviceList.Count()} device(s)");

                List<Task> taskList = new List<Task>();
                foreach (Guid deviceGuid in deviceList)
                {
                    taskList.Add(context.CallActivityAsync<Task<IActionResult>>("MiddleTierToBackendHTTP", deviceGuid));
                }

                // Parallel.ForEach<Task>(taskList.ToArray(), t => t.Start());
                Task.WaitAll(taskList.ToArray());

                log.LogTrace("Synchronization complete.");
                log.LogTrace("Cleaning the exported data...");
                this.dbService.CleanExportedData();
                log.LogTrace("Cleaning the exported data complete.");
            }
        }
    }
}
