namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using PTASServicesCommon.DependencyInjection;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using Microsoft.EntityFrameworkCore;
    using CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    /// <summary>
    /// Worker job processor that applies the regression post process to the land schedule.
    /// </summary>
    public class ApplyLandRegressionToScheduleProcessor : WorkerJobProcessor
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
                return nameof(CustomSearchesJobTypes.ApplyLandRegressionToScheduleJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyLandRegressionToScheduleProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/logger parameter is null.</exception>
        public ApplyLandRegressionToScheduleProcessor(IFactory<CustomSearchesDbContext> dbContextFactory, ILogger logger) : base(logger)
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
        /// <param name="workerJob"></param>
        /// <returns>True if the job completed successfully</returns>
        public override async Task<bool> ProcessAvailableJob(WorkerJobQueue workerJob)
        {
            ApplyLandRegressionToSchedulePayloadData payload = JsonConvert.DeserializeObject<ApplyLandRegressionToSchedulePayloadData>(workerJob.JobPayload);
            ApplyLandRegressionToScheduleService service = new ApplyLandRegressionToScheduleService(this.ServiceContext);
            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                Dataset dataset = await dbContext.Dataset.FirstOrDefaultAsync(d => d.DatasetId == payload.DatasetId);
                if (dataset == null)
                {
                    string message = $"Dataset with id '{payload.DatasetId}' not found.";
                    CustomSearchesEntityNotFoundException exception = new CustomSearchesEntityNotFoundException(message, null);
                    FailedJobResult failedJobResult = new FailedJobResult
                    {
                        Status = "Failed",
                        Reason = message,
                        Exception = exception.ToString()
                    };

                    workerJob.JobResult = JsonConvert.SerializeObject(failedJobResult);
                    throw exception;
                }

                try
                {
                    await DatasetHelper.GetAlterDatasetLockAsyncV2(
                        async (datasetState, datasetPostProcessState) =>
                        {
                            await service.ApplyLandRegressionToScheduleAsync(
                            payload.RScriptPostProcessId,
                            payload.ExceptionPostProcessId,
                            dbContext,
                            this.Logger);

                            datasetPostProcessState = await DatasetHelper.CalculateDatasetPostProcessStateAsync(payload.DatasetId, this.ServiceContext);

                            return (datasetState, datasetPostProcessState);
                        },
                        dataset,
                        isRootLock: false,
                        dataset.DataSetState,
                        dataset.DataSetPostProcessState,
                        workerJob.UserId,
                        workerJob.JobId,
                        dbContext,
                        this.ServiceContext);
                }
                catch (CustomSearchesConflictException exception)
                {
                    FailedJobResult failedJobResult = new FailedJobResult
                    {
                        Status = "Failed",
                        Reason = exception.GetBaseException().Message,
                        Exception = exception.ToString()
                    };

                    workerJob.JobResult = JsonConvert.SerializeObject(failedJobResult);
                    throw; 
                }
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
            ApplyLandRegressionToSchedulePayloadData payload = JsonConvert.DeserializeObject<ApplyLandRegressionToSchedulePayloadData>(workerJob.JobPayload);
            return payload;
        }
    }
}
