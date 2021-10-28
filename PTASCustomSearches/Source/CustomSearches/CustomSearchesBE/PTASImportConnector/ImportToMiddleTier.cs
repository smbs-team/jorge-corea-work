// <copyright file="ImportToMiddleTier.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASImportConnector
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using ConnectorService;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASImportConnector.Exceptions;
    using PTASImportConnector.SDK;
    using PTASServicesCommon.TokenProvider;

    /// <summary>Entry point of the application.</summary>
    public class ImportToMiddleTier
    {
        private readonly Connector connector;
        private readonly DbModel databaseModel;
        private readonly BackendService backendService;
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportToMiddleTier"/> class.
        /// </summary>
        /// <param name="connector"> It has the necessary connector SDK functionality.</param>
        /// <param name="databaseModel"> It has the necessary database model functionality.</param>
        /// <param name="backendService"> It has the necessary back-end functionality.</param>
        /// <param name="logger"> It has the necessary logger functionality.</param>
        public ImportToMiddleTier(Connector connector, DbModel databaseModel, BackendService backendService, ILogger logger)
        {
            this.connector = connector;
            this.databaseModel = databaseModel;
            this.backendService = backendService;
            this.logger = logger;
        }

        // public ImportToMiddleTier(IConnector connector, IDbModel databaseModel, IBackendService backendService)
        // {
        //    this.connector = connector;
        //    this.databaseModel = databaseModel;
        //    this.backendService = backendService;
        // }

        /// <summary>Runs the backed to middle tier.</summary>
        /// <param name="sqlConnectionStringParameter"> Schedule for the function.</param>
        /// <param name="principalCredentials"> Credentials.</param>
        public void Run(string sqlConnectionStringParameter, ClientCredential principalCredentials)
        {
            try
            {
                var cnnstrBuilder = new SqlConnectionStringBuilder(sqlConnectionStringParameter);
                cnnstrBuilder.ConnectTimeout = 172800;
                var sqlConnectionString = cnnstrBuilder.ToString();
                bool isBulkInsert = true;

                // Initialize a new instance of the connector SDK
                this.connector.Init(sqlConnectionString, principalCredentials);

                if (!this.connector.IsRunning())
                {
                    var syncDate = DateTime.UtcNow;
                    this.logger.LogInformation("Setting the status of the connectors to running...");
                    this.connector.SetRunningStatus(syncDate, true);
                    this.logger.LogInformation($"ImportToMiddleTier function executed on: {DateTime.Now}");

                    var backendConnectionString = sqlConnectionStringParameter;
                    this.logger.LogInformation("Loading the structure of the middle-tier data-base...");

                    // Load the structure of middle tier database from XML
                    var databaseModel = this.databaseModel.GetDatabaseModel();

                    this.logger.LogInformation("Getting a new ticket to store related data...");

                    // Get a new ticket to store the data related to this ticket (group by ticket number)
                    var uploadTicket = this.connector.GetUploadTicketForBackend();

                    // Iterates the entities in the database model (entity = table)
                    foreach (EntityModel entity in databaseModel.Entities.Where(e => !string.IsNullOrWhiteSpace(e.BackendQuery)).OrderBy(e => e.SyncOrder))
                    {
                        this.logger.LogInformation("Moving " + entity.Name + " to middle-tier...");
                        this.logger.LogInformation("Getting the last import date for " + entity.Name + "...");
                        DateTime? lastDate = this.connector.GetLastImportEntityDate(entity.Name);
                        var uploadList = this.backendService.GetData(entity, backendConnectionString, sqlConnectionString, lastDate, this.connector, uploadTicket, isBulkInsert, this.logger, principalCredentials);

                        if (uploadList.Count == 0)
                        {
                            Console.WriteLine(entity.Name + " is empty.");
                            continue;
                        }

                        if (!isBulkInsert)
                        {
                            this.connector.AddUploadData(entity.Name, uploadList, uploadTicket);

                            // Invoke the method of the connector to process the data
                            this.connector.ProcessDataForTicket(uploadTicket, false, false);
                        }
                        else
                        {
                            this.backendService.BulkProcess(backendConnectionString, sqlConnectionString, entity, uploadTicket, uploadList, this.connector);

                            // this.backendService.DisableorRebuildIndex(sqlConnectionString, entity.Name, 2);
                        }

                        this.logger.LogInformation("Setting the last import date for " + entity.Name + "...");
                        this.connector.SetImportEntityDate(entity.Name, syncDate);
                        this.logger.LogInformation("Moving " + entity.Name + " completed.");
                    }

                    this.logger.LogInformation("Relate entities and area updates...");
                    this.connector.ProcessDataForTicket(uploadTicket, false, true);
                    this.logger.LogInformation("Setting the status of the connectors to not running...");
                    this.connector.SetRunningStatus(syncDate, false);
                    this.logger.LogInformation($"Importing completed on: " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error during the import process: " + ex.Message);
                throw new ImportConnectorException("Error during the import process: " + ex.Message, ex.InnerException);
            }
        }
    }
}
