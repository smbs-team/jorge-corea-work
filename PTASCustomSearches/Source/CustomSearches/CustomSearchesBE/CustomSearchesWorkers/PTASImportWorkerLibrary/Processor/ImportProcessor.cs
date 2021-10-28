using BaseWorkerLibrary;
using CustomSearchesEFLibrary.WorkerJob.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PTASImportConnector;
using PTASImportConnector.SDK;
using PTASImportWorkerLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PTASImportWorkerLibrary.Processor
{
    public class ImportProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="ImportProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ImportProcessor(
            ILogger logger)
            : base(logger)
        {
            
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
        }

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType
        {
            get
            {
                return "ImportProcessor";
            }
        }

        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            return null;
        }

        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            // var payload = JsonConvert.DeserializeObject<ImportPayload>(workerJob.JobPayload); // CHECK THE MODEL, NOT USED

            this.LogMessage(workerJob, $"Starting job for Import Connector.  JobId: {workerJob.JobId}, Payload: {workerJob.JobPayload}.");

            Connector connector = new Connector();
            DbModel dbModel = new DbModel();
            BackendService backendService = new BackendService();

            ImportToMiddleTier importToMiddleTier = new ImportToMiddleTier(connector, dbModel, backendService, this.logger);

            using (var dbContext = this.ServiceContext.DataDbContextFactory.Create())
            using (var connection = dbContext.Database.GetDbConnection())
            {
                importToMiddleTier.Run(connection.ConnectionString, this.ServiceContext.AppCredential);
            }

            this.logger.LogInformation($"Job {workerJob.JobId} finished!");

            return true;
        }
    }
}
