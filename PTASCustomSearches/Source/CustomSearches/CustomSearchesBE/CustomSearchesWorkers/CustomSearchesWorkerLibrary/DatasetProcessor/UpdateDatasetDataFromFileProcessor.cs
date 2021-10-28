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
    /// Dataset update data worker class
    /// </summary>
    public class UpdateDatasetDataFromFileProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType 
        { 
            get
            {
                return nameof(CustomSearchesJobTypes.UpdateDatasetDataFromFileJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetDataFromFileProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/logger parameter is null.</exception>
        public UpdateDatasetDataFromFileProcessor(
            IFactory<CustomSearchesDbContext> dbContextFactory, 
            ILogger logger) : base(logger)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="workerJob">Worker job to process.</param>
        /// <returns>True if the job completed successfully</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            var payload = JsonConvert.DeserializeObject<DatasetFileImportExportPayloadData>(workerJob.JobPayload);

            this.LogMessage(workerJob, $"Starting job for dataset data update from file.  DatasetId: {payload.DatasetId}");

            var service = new UpdateDatasetDataFromFileService(this.ServiceContext);
            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                await service.UpdateDatasetDataFromFileAsync(
                    payload,
                    dbContext,
                    workerJob.JobId,
                    (string message) => { this.LogMessage(workerJob, message); });
            }

            payload.DatasetData = string.Empty;
            workerJob.JobPayload = JsonConvert.SerializeObject(payload);

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
            var jobPayload = JsonConvert.DeserializeObject<DatasetFileImportExportPayloadData>(workerJob.JobPayload);

            DatasetFileImportExportSignalRPayloadData signalPayload = new DatasetFileImportExportSignalRPayloadData
            {
                DatasetId = jobPayload.DatasetId,
                FileType = jobPayload.FileType
            };

            return signalPayload;
        }
    }
}
