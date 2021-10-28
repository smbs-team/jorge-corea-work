// <copyright file="MiddleTierToBackendHTTP.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASExportConnector.Exceptions;
    using PTASExportConnector.SDK;

    /// <summary>
    ///   <para>Moves the received data from the middle-tier to the back-end.</para>
    /// </summary>
    public class MiddleTierToBackendHTTP
    {
        private readonly IExporters exporter;
        private readonly IConnector connector;

        /// <summary>Initializes a new instance of the <see cref="MiddleTierToBackendHTTP"/> class.</summary>
        /// <param name="exporter">
        ///   <para>
        ///  It has the exporting functionality.</para>
        /// </param>
        /// <param name="connector">  It has the connector SDK functionality.</param>
        public MiddleTierToBackendHTTP(IExporters exporter, IConnector connector)
        {
            this.connector = connector;
            this.exporter = exporter;
        }

        /// <summary>Runs the synchronization process.</summary>
        /// <param name="req">The HTTP request.</param>
        /// <param name="log">Used for logging data.</param>
        /// <param name="deviceGuid">The device GUID.</param>
        /// <param name="exeContext">  Context of the function, containing useful data such as the directory path.</param>
        /// <returns>Completion message.</returns>
        [FunctionName("MiddleTierToBackendHTTP")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [ActivityTrigger] Guid deviceGuid,
            ExecutionContext exeContext)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (deviceGuid == Guid.Empty)
            {
                log.LogError("Device GUID is empty.");
                return new BadRequestResult();
            }

            this.connector.Init();

            var currentEntity = string.Empty;

            Console.WriteLine($"Synchronizing device {deviceGuid}...");
            log.LogTrace("Synchronizing device {deviceGuid}", deviceGuid);
            try
            {
                var changesetIds = new List<string>();

                log.LogTrace("Getting the entity list...");
                var entityList = this.exporter.GetEntityList(Directory.GetCurrentDirectory());

                log.LogTrace("Processing list by sync order...");
                foreach (var entity in entityList.OrderBy(e => e.SyncOrder))
                {
                    currentEntity = entity.Name;
                    Console.WriteLine($"Moving to back-end {0}", entity.Name);
                    log.LogTrace("Moving to back-end {name}", entity.Name);
                    var ids = this.exporter.Export(this.connector, deviceGuid, entity.Name);

                    foreach (var item in ids)
                    {
                        if (!changesetIds.Contains(item))
                        {
                            changesetIds.Add(item);
                        }
                    }
                }

                log.LogTrace("Processing completed.");
                log.LogTrace("Setting the change-set ids...");
                this.connector.SetChangesetsExported(changesetIds);
                log.LogTrace("Setting the change-set ids completed.");
            }
            catch (ExportConnectorException ex)
            {
                ObjectResult result = new ObjectResult($"Error moving data related to device " + deviceGuid.ToString() + "\n" + "Entity: " + currentEntity + "\n" + "Error: " + ex.Message + "\n" + "Inner Message:" + ex.InnerException);
                result.StatusCode = ex.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                ObjectResult result = new ObjectResult($"Error moving data related to device " + deviceGuid.ToString() + "\n" + "Entity: " + currentEntity + "\n" + "Error: " + ex.Message + "\n" + "Inner Message: " + ex.InnerException);
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }

            log.LogTrace("Synchronization complete.");
            return new OkObjectResult("Synchronization complete.");
        }
    }
}
