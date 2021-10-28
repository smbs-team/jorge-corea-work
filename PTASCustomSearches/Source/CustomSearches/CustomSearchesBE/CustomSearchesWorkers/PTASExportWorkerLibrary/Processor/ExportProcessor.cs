using BaseWorkerLibrary;
using Microsoft.Extensions.Logging;
using CustomSearchesEFLibrary.WorkerJob.Model;
using System;
using System.Threading.Tasks;
using PTASExportConnector.SDK;
using PTASExportConnector;
using Microsoft.EntityFrameworkCore;

namespace PTASExportWorkerLibrary.Processor
{
    public class ExportProcessor : WorkerJobProcessor
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="webApi">Web Api.</param>
        /// <param name="crmUri">Dynamics URI.</param>
        /// <param name="authUri">Auth URI.</param>
        /// <param name="clientID">ClientID.</param>
        /// <param name="clientSecret">ClientSecret.</param>
        public ExportProcessor(
            ILogger logger, string webApi, string crmUri, string authUri, string clientID, string clientSecret)
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
        }

        /// <inheritdoc/>
        public override string ProcessorType
        {
            get
            {
                return "ExportProcessor";
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
            this.LogMessage(workerJob, $"Starting job for Import Connector.  JobId: {workerJob.JobId}, Payload: {workerJob.JobPayload}.");

            using (var dbContext = this.ServiceContext.DataDbContextFactory.Create())
            using (var connection = dbContext.Database.GetDbConnection())
            {

                Connector connector = new Connector();
                DbService dbService = new DbService(connection.ConnectionString, this.ServiceContext.AppCredential);
                Odata odata = new Odata();
                Exporters exporters = new Exporters(dbService, odata, this.crmUri, this.authUri, this.clientId, this.clientSecret, connection.ConnectionString, this.webapi, this.ServiceContext.AppCredential);

                MiddleTierToBackendHTTP middleTierToBackendHTTP = new MiddleTierToBackendHTTP(exporters, connector, dbService);
                middleTierToBackendHTTP.Run(connection.ConnectionString, this.ServiceContext.AppCredential, this.logger);
            }

            this.logger.LogInformation($"Job {workerJob.JobId} finished!");

            return true;
        }
    }
}
