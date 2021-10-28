// <copyright file="DynamicsSyncWorker.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsSynchronizationToSQL.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BaseWorkerLibrary;

    using CustomSearchesEFLibrary.WorkerJob.Model;

    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using Newtonsoft.Json;

    using PTASDynamicsSyncLibrary;

    /// <summary>
    /// Worker processor for dynamics tranfer.
    /// </summary>
    public class DynamicsSyncWorker : WorkerJobProcessor
    {
        private readonly string organizationId;
        private readonly string organizationName;
        private readonly string crmUri;
        private readonly string authUri;
        private readonly string clientID;
        private readonly string connectionString;
        private readonly string clientSecret;
        private readonly ClientCredential principalCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsSyncWorker"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="authUri">AuthUri.</param>
        /// <param name="clientID">Client Id.</param>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="clientSecret">Client secret.</param>
        /// <param name="crmUri">Dynamics URI.</param>
        /// <param name="organizationId">Org Id.</param>
        /// <param name="organizationName">Org Name.</param>
        /// <param name="principalCredentials">Credentials.</param>
        public DynamicsSyncWorker(
            ILogger logger,
            string organizationId,
            string organizationName,
            string crmUri,
            string authUri,
            string clientID,
            string connectionString,
            string clientSecret,
            ClientCredential principalCredentials)
            : base(logger)
        {
            this.organizationId = organizationId;
            this.organizationName = organizationName;
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.clientID = clientID;
            this.connectionString = connectionString;
            this.clientSecret = clientSecret;
            this.principalCredentials = principalCredentials;
        }

        /// <inheritdoc/>
        public override string ProcessorType => "DynamicsSynchronizer";

        /// <inheritdoc/>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob) => null;

        /// <summary>
        /// Checks if the user id can be used to process the job.
        /// Throws an exception if the check does not pass.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public override void CheckUser(Guid userId)
        {
            // This processor does not need a user.
        }

        /// <inheritdoc/>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            string json = workerJob.JobPayload;
            var payload = JsonConvert.DeserializeObject<WorkerPayload>(json);
            var connectionString = string.IsNullOrEmpty(payload.ConnectionStringOverride) ? this.connectionString : payload.ConnectionStringOverride.Replace("_", " ");
            Dictionary<string, string> values = GetConfig(
                                        connectionString,
                                        this.organizationId,
                                        this.organizationName,
                                        this.crmUri,
                                        this.authUri,
                                        this.clientID,
                                        this.clientSecret);
            SyncConfig config = new SyncConfig(values);
            var t = new DynamicsUtility(config, this.principalCredentials);

            var tables = await t.GetAllTables();
            int numberOfAsyncTables = payload.NumberOfAsyncTables ?? 10;
            var groups = tables
                                .Select((v, i) => (value: v, group: i / numberOfAsyncTables))
                                .GroupBy(itm => itm.group)
                                .Select(gr => gr.Select(itm => itm.value).ToArray())
                                .ToArray();
            var i = 0;
            foreach (var group in groups)
            {
                try
                {
                    this.Logger.LogInformation($"Processing group {++i} of {groups.Length}");
                    var r = group.Select(s => t.Sync(
                        dbTable: s.Name,
                        entityName: s.Name,
                        timeoutMS: workerJob.TimeoutInSeconds * 1000,
                        batchCopy: payload.BatchInsert ?? false,
                        chunkSize: payload.ChunkSize ?? 50,
                        log: this.Logger,
                        waitWhenFailMS: payload.RetryWaitMS ?? 3000));
                    await Task.WhenAll(r);
                    this.Logger.LogInformation($"Done group {i} of {groups.Length}");
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex.Message);
                    return false;
                }
            }

            return true;
        }

        private static Dictionary<string, string> GetConfig(string connectionString, string organizationId, string organizationName, string dynamicsURL, string authUri, string clientId, string clientSecret)
        {
            return new Dictionary<string, string>()
            {
                { "connectionString",  connectionString },
                { "organizationId", organizationId },
                { "organizationName", organizationName },
                { "DynamicsURL", dynamicsURL },
                { "AuthUri", authUri },
                { "ClientId", clientId },
                { "ClientSecret", clientSecret },
            };
        }

        private class WorkerPayload
        {
            public int? ChunkSize { get; set; }

            public bool? BatchInsert { get; set; }

            public int? NumberOfAsyncTables { get; set; }

            public int? RetryWaitMS { get; set; }

            public string ConnectionStringOverride { get; set; }
        }
    }
}