using BaseWorkerLibrary;
using CustomSearchesEFLibrary.WorkerJob.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using SyncDataBase;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncDatabaseWorkerLibrary.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using PTASServicesCommon.CloudStorage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SyncDatabaseWorkerLibrary.Processor
{
    public class SyncDBProcessor : WorkerJobProcessor
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
        private string BlobServer_URL;
        private string SyncServer_URL;
        private string PTASSketchServiceURL;
        private string PTASSAASTokenURL;
        private string[] paths = new string[6];
        private string[] names = new string[6];
        private const string SyncBlobContainer = "ptasmobiledbs";
        private int area;

        /// <summary>
        /// Global configuration.
        /// </summary>
        private IConfiguration configuration;
        /// <summary>
        /// The cloud storage provider.
        /// </summary>
        private readonly ICloudStorageProvider cloudStorageProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="webApi">Web Api.</param>
        /// <param name="crmUri">Dynamics URI.</param>
        /// <param name="authUri">Auth URI.</param>
        /// <param name="clientID">ClientID.</param>
        /// <param name="clientSecret">ClientSecret.</param>
        public SyncDBProcessor(
            ILogger logger, string webApi, string crmUri,
            string authUri, string clientID, string clientSecret,
            IConfiguration configuration,
            ICloudStorageProvider cloudStorageProvider)
            : base(logger)
        {

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (cloudStorageProvider == null)
            {
                throw new System.ArgumentNullException(nameof(cloudStorageProvider));
            }

            this.logger = logger;
            this.webapi = webApi;
            this.crmUri = crmUri;
            this.authUri = authUri;
            this.clientId = clientID;
            this.clientSecret = clientSecret;
            this.configuration = configuration;
            this.cloudStorageProvider = cloudStorageProvider;
        }

        /// <inheritdoc/>
        public override string ProcessorType
        {
            get
            {
                return "SyncDBProcessor";
            }
        }

        /// <inheritdoc/>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            return null;
        }

        /// <summary>
        /// Checks if the user id can be used to process the job.
        /// Throws an exception if the check does not pass.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public override void CheckUser(Guid userId)
        {
            // This processor does not require a user id.
        }

        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            var payload = JsonConvert.DeserializeObject<SyncPayload>(workerJob.JobPayload);
            this.BlobServer_URL = payload.BlobServer_URL;
            this.SyncServer_URL = payload.SyncServer_URL;
            this.PTASSketchServiceURL = payload.PTASSketchServiceURL;
            this.PTASSAASTokenURL = payload.PTASSAASTokenURL;

            this.LogMessage(workerJob, $"Starting job for Import Connector.  JobId: {workerJob.JobId}, Payload: {workerJob.JobPayload}.");
            this.area = payload.Area;

            // get paths for sprite tiling
            IConfigurationSection values = this.configuration.GetSection("Values");
            string path = values["SyncMobileStorageSharePath"];

            // verify if exist a previous file from sync
            names[0] = path + "\\PTASdb.sqlite";
            names[1] = path + "\\PTASdb.sqlite-shm";
            names[2] = path + "\\PTASdb.sqlite-wal";
            names[3] = path + "\\FileDownload.sqlite";
            names[4] = path + "\\FileDownload.sqlite-shm";
            names[5] = path + "\\FileDownload.sqlite-wal";


            paths[0] = path + "\\PTASdb" + this.area + ".sqlite";
            paths[1] = path + "\\PTASdb" + this.area + ".sqlite-shm";
            paths[2] = path + "\\PTASdb" + this.area + ".sqlite-wal";
            paths[3] = path + "\\FileDownload" + this.area + ".sqlite";
            paths[4] = path + "\\FileDownload" + this.area + ".sqlite-shm";
            paths[5] = path + "\\FileDownload" + this.area + ".sqlite-wal";
            filesExist();

            using (var dbContext = this.ServiceContext.DataDbContextFactory.Create())
            using (var connection = dbContext.Database.GetDbConnection())
            {
                SyncDB syncDB = new SyncDB(this.area, SyncServer_URL, BlobServer_URL, PTASSketchServiceURL, PTASSAASTokenURL, path);

                await syncDB.StartSyncAsync(this.ServiceContext.AppCredential, "syncapp@kingcounty.gov", clientSecret);
            }
            //UPLOAD TO BLOB
            await this.uploadFilesToBlobAsync();
            //DELETE FROM FILESHARE
            this.deleteFilesFromFileShare();

            this.logger.LogInformation($"Job {workerJob.JobId} finished!");

            return true;
        }

        private void filesExist()
        {
            // verify if exist a previous file from sync
            DateTime now = DateTime.UtcNow;

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    DateTime lastModifiedSqlite = File.GetLastWriteTimeUtc(path);
                    if ((lastModifiedSqlite - now).Days <= 1)
                    {
                        throw new FileExistException();
                    }
                }
            }
        }

        private void deleteFilesFromFileShare()
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private async Task uploadFilesToBlobAsync()
        {
            for (int i = 0; i < 3; i++) { // 3 BECAUSE WE DONT WANT UPLOAD FILESYNC FILES
                string virtualFolder = "./area" + this.area + "/";
                string localFile = Path.Combine(virtualFolder, names[i]);
                ClientCredential principalCredentials = new ClientCredential(this.clientId, this.clientSecret);
                CloudBlobClient cloudBlobClient = await this.cloudStorageProvider.GetCloudBlobClient(principalCredentials);
                CloudBlobContainer blobContainer = cloudBlobClient.GetContainerReference(SyncBlobContainer);
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(localFile);
            
                if (await blockBlob.ExistsAsync()) 
                {
                    await blockBlob.CreateSnapshotAsync();
                }
            
                using (var fileStream = File.OpenRead(paths[i]))
                {
                    if (File.Exists(paths[i])) 
                    {
                        await blockBlob.UploadFromStreamAsync(fileStream);
                    }
                }
            }
        }

        [Serializable]
        class FileExistException : Exception
        {
            public FileExistException()
                : base(String.Format("This file Was Created Less Than a Day Ago"))
            {

            }
        }
    }
}
