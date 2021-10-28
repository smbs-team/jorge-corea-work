using BaseWorkerLibrary;
using Microsoft.Extensions.Logging;
using CustomSearchesEFLibrary.WorkerJob.Model;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using PTASDynamicsTranfer;
using System.Threading;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PTASDynamicsTransferWorkerLibrary.Model;
using PTASServicesCommon.CloudStorage;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PTASDynamicsTranferWorkerLibrary.Processor
{
    public class TransferProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger logger;

        private string webapi;
        private string crmUri;
        private string authUri;
        private string clientId;
        private string clientSecret;
        private string sqlConnectionString;
        private const string SyncBlobContainer = "entitiesmanifest";
        private readonly ICloudStorageProvider cloudStorageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="webApi">Web Api.</param>
        /// <param name="crmUri">Dynamics URI.</param>
        /// <param name="authUri">Auth URI.</param>
        /// <param name="clientID">ClientID.</param>
        /// <param name="clientSecret">ClientSecret.</param>
        public TransferProcessor(
            ILogger logger, string webApi, string crmUri, string authUri, string clientID, string clientSecret, ICloudStorageProvider cloudStorageProvider)
            : base(logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
            this.webapi = webApi;
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.clientId = clientID;
            this.clientSecret = clientSecret;
            this.cloudStorageProvider = cloudStorageProvider;
        }

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
        public override string ProcessorType
        {
            get
            {
                return "TransferProcessor";
            }
        }

        /// <inheritdoc/>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            return null;
        }

        /// <inheritdoc/>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            var payload = JsonConvert.DeserializeObject<TransferPayload>(workerJob.JobPayload);

            this.LogMessage(workerJob, $"Starting job for Import Connector.  JobId: {workerJob.JobId}, Payload: {workerJob.JobPayload}.");
            List<string> idList = new List<string>();

            string response = getEntities(payload.EntityNumber).GetAwaiter().GetResult();


            //1 = Use Bulk Insert
            int useBulkInsert = 1;
            int chunkSize = 2000;

            if (!string.IsNullOrEmpty(response))
            {
                idList = response.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            }
            else
            {
                throw new Exception("The Entities File doesn't exist in Blob Container");
            }

            using (var dbContext = this.ServiceContext.DataDbContextFactory.Create())
            using (var connection = dbContext.Database.GetDbConnection())
            {
                var builder = new SqlConnectionStringBuilder(connection.ConnectionString)
                {
                    ConnectTimeout = 172800 // 2 Days in seconds
                };
                string sqlConnectionString = builder.ConnectionString;

                foreach (string entity in idList)
                {
                    await Start.Run(chunkSize, sqlConnectionString, authUri, crmUri, entity, clientId, clientSecret, useBulkInsert, this.ServiceContext.AppCredential);
                }
            }

            this.logger.LogInformation($"Job {workerJob.JobId} finished!");

            return true;
        }

        private async Task<string> getEntities(int entityNumber)
        {
            string localFile = "./entitymanifest" + entityNumber + ".txt";
            ClientCredential principalCredentials = new ClientCredential(this.clientId, this.clientSecret);
            CloudBlobClient cloudBlobClient = await this.cloudStorageProvider.GetCloudBlobClient(principalCredentials);
            CloudBlobContainer blobContainer = cloudBlobClient.GetContainerReference(SyncBlobContainer);
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(localFile);

            if (await blockBlob.ExistsAsync())
            {
                var response = await blockBlob.DownloadTextAsync();
                return response;
            }
            return string.Empty;
        }
    }
}