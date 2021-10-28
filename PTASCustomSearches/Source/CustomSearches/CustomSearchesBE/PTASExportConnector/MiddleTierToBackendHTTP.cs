// <copyright file="MiddleTierToBackendHTTP.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASExportConnector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASExportConnector.Exceptions;
    using PTASExportConnector.SDK;

    /// <summary>
    ///   <para>Moves the received data from the middle-tier to the back-end.</para>
    /// </summary>
    public class MiddleTierToBackendHTTP
    {
        private readonly Exporters exporter;
        private readonly Connector connector;
        private readonly DbService dbService;

        /// <summary>Initializes a new instance of the <see cref="MiddleTierToBackendHTTP"/> class.</summary>
        /// <param name="exporter">
        ///   <para>
        ///  It has the exporting functionality.</para>
        /// </param>
        /// <param name="connector">  It has the connector SDK functionality.</param>
        /// <param name="dbService">  It has database related functionality.</param>
        public MiddleTierToBackendHTTP(Exporters exporter, Connector connector, DbService dbService)
        {
            this.connector = connector;
            this.exporter = exporter;
            this.dbService = dbService;
        }

        /// <summary>Runs the synchronization process.</summary>
        /// <param name="sqlConnection">SQL Connection.</param>
        /// <param name="clientCredential">Used for credential data.</param>
        /// <param name="log">Used for show information.</param>
        public void Run(string sqlConnection, ClientCredential clientCredential, ILogger log)
        {
            // Init the code of Timer
            log.LogInformation($"Start MiddleTierToBackend: {DateTime.UtcNow}");

            string sqlConnectionString = sqlConnection;
            ClientCredential principalCredentials = clientCredential;

            this.connector.Init(sqlConnectionString, principalCredentials);

            log.LogTrace("Getting the device list...");
            var deviceList = this.connector.GetDeviceGuidList();

            if (deviceList.Count() == 0)
            {
                Console.WriteLine("There is nothing new to move back to the back-end.");
            }
            else
            {
                log.LogTrace($"Synchronizing {deviceList.Count()} device(s)");
                Console.WriteLine($" {deviceList.Count()} device(s)");

                List<Task> taskList = new List<Task>();
                foreach (Guid deviceGuid in deviceList)
                {
                    this.Execution(deviceGuid, log);
                }

                // Parallel.ForEach<Task>(taskList.ToArray(), t => t.Start());
                Task.WaitAll(taskList.ToArray());

                Console.WriteLine("Synchronization complete.");
                log.LogTrace("Cleaning the exported data...");
                this.dbService.CleanExportedData();
                Console.WriteLine("Cleaning the exported data complete.");
            }

            Console.WriteLine("Synchronization complete.");
            log.LogInformation("Synchronization complete.");
        }

        /// <summary>Start the execution of export.</summary>
        /// <param name="deviceGuid">GUID for execute.</param>
        public void Execution(Guid deviceGuid, ILogger log)
        {
            if (deviceGuid == Guid.Empty)
            {
                Console.WriteLine("Device GUID is empty.");
                return;
            }

            var currentEntity = string.Empty;

            log.LogInformation($"Synchronizing device {deviceGuid}...");
            try
            {
                var changesetIds = new List<string>();

                log.LogTrace("Getting the entity list...");
                var entityList = this.EntityListMethod();

                Console.WriteLine("Processing list by sync order...");
                foreach (var entity in entityList.OrderBy(e => e.SyncOrder))
                {
                    currentEntity = entity.Name;
                    Console.WriteLine($"Moving to back-end {entity.Name}");
                    var ids = this.exporter.ExportAsync(this.connector, deviceGuid, entity.Name);

                    foreach (var item in ids.Result)
                    {
                        if (!changesetIds.Contains(item))
                        {
                            changesetIds.Add(item);
                        }
                    }
                }

                Console.WriteLine("Processing completed.");
                log.LogInformation("Setting the change-set ids...");
                if (changesetIds.Count() != 0)
                {
                    this.connector.SetChangesetsExported(changesetIds); // VIENE 13 REVISAR
                }

                Console.WriteLine("Setting the change-set ids completed.");
            }
            catch (ExportConnectorException ex)
            {
                Console.WriteLine($"Error moving data related to device " + deviceGuid.ToString() + "\n" + "Entity: " + currentEntity + "\n" + "Error: " + ex.Message + "\n" + "Inner Message:" + ex.InnerException);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving data related to device " + deviceGuid.ToString() + "\n" + "Entity: " + currentEntity + "\n" + "Error: " + ex.Message + "\n" + "Inner Message: " + ex.InnerException);
                return;
            }
        }

        private List<Entity> EntityListMethod()
        {
            List<Entity> list = new List<Entity>();

            foreach (var entity in this.dbService.GetDatabaseModel().Entities)
            {
                if (!(entity.Name.Contains("_mb") || entity.Name.ToLower().Equals("mbusertask") || entity.Name.ToLower().Equals("user_filter")))
                {
                    Entity e = new Entity(entity.Name, entity.SyncOrder);
                    list.Add(e);
                }
            }

            return list;
        }
    }
}
