namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using BaseWorkerLibrary.SqlServer.Model;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using PTASServicesCommon.DependencyInjection;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Payload;
    using PTASCRMHelpers;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    /// <summary>
    /// Dataset generation worker class
    /// </summary>
    public class DatasetGenerationProcessor : WorkerJobProcessor
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
                return nameof(CustomSearchesJobTypes.DatasetGenerationJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetGenerationProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="dynamicsODataHelper">The dynamics OData helper factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/logger parameter is null.</exception>
        public DatasetGenerationProcessor(IFactory<CustomSearchesDbContext> dbContextFactory, IFactory<GenericDynamicsHelper> dynamicsODataHelper, ILogger logger) : base(logger)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
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
            DatasetGenerationPayloadData payload = JsonConvert.DeserializeObject<DatasetGenerationPayloadData>(workerJob.JobPayload);
            ExecuteCustomSearchService service = new ExecuteCustomSearchService(this.ServiceContext);
            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                var warnings = await service.ExecuteCustomSearchAsync(
                    payload,
                    dbContext,
                    this.dynamicsODataHelperFactory.Create(),
                    (string message) => { this.LogMessage(workerJob, message); });

                DatasetGenerationJobResultData results = new DatasetGenerationJobResultData()
                {
                    Status = "Success",
                    Warnings = warnings
                };

                workerJob.JobResult = JsonConvert.SerializeObject(results);
            }

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            DatasetGenerationPayloadData payload = JsonConvert.DeserializeObject<DatasetGenerationPayloadData>(workerJob.JobPayload);
            return payload;
        }
    }
}
