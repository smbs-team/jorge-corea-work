namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using BaseWorkerLibrary.SqlServer.Model;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using PTASServicesCommon.DependencyInjection;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using PTASCRMHelpers;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    /// <summary>
    /// Dataset backend update worker class
    /// </summary>
    public class DatasetBackendUpdateProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The dynamics OData helper factory.
        /// </summary>
        private readonly IFactory<GenericDynamicsHelper> dynamicsODataHelperFactory;

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType 
        { 
            get
            {
                return nameof(CustomSearchesJobTypes.DatasetBackendUpdateJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetBackendUpdateProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/dynamicsODataHelper/logger parameter is null.</exception>
        public DatasetBackendUpdateProcessor(
            IFactory<CustomSearchesDbContext> dbContextFactory, 
            IFactory<GenericDynamicsHelper> dynamicsODataHelper,
            ILogger logger) : base(logger)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (dynamicsODataHelper == null)
            {
                throw new System.ArgumentNullException(nameof(dynamicsODataHelper));
            }

            this.dbContextFactory = dbContextFactory;
            this.dynamicsODataHelperFactory = dynamicsODataHelper;
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob"></param>
        /// <returns>True if the job completed successfully</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            DatasetBackendUpdatePayloadData payload = JsonConvert.DeserializeObject<DatasetBackendUpdatePayloadData>(workerJob.JobPayload);

            this.LogMessage(workerJob, $"Starting job for dataset backend update.  DatasetId: {payload.DatasetId}");

            ExecuteBaseBackendUpdateService service;
            if (payload.DatasetPostProcessId > 0)
            {
                service = new ExecutePostProcessBackendUpdateService(this.ServiceContext);
            }
            else
            {
                service = new ExecuteDatasetBackendUpdateService(this.ServiceContext);
            }

            await service.ExecuteBackendUpdateAsync(
                payload,
                this.dynamicsODataHelperFactory.Create(),
                (string message) => { this.LogMessage(workerJob, message); });

            this.LogMessage(workerJob, "Finished!...");

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            DatasetBackendUpdatePayloadData payload = JsonConvert.DeserializeObject<DatasetBackendUpdatePayloadData>(workerJob.JobPayload);
            return payload;
        }
    }
}
