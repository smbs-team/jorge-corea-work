namespace CustomSearchesWorkerLibrary.DatasetProcessor
{
    using Microsoft.Extensions.Logging;
    using System;
    using BaseWorkerLibrary;
    using Newtonsoft.Json;
    using CustomSearchesEFLibrary.CustomSearches;
    using PTASServicesCommon.DependencyInjection;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesWorkerLibrary.Enumeration;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.CustomSearches;
    using Microsoft.EntityFrameworkCore;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesEFLibrary.WorkerJob.Model;
    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using CustomSearchesWorkerLibrary.DatasetProcessor.Model;

    /// <summary>
    /// Dataset post process execution worker class
    /// </summary>
    public class DatasetPostProcessExecutionProcessor : WorkerJobProcessor
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The notification payload.
        /// </summary>
        private PostProcessUserNotificationPayload NotificationPayload;

        /// <summary>
        /// Gets the type of the processor.
        /// </summary>
        public override string ProcessorType 
        { 
            get
            {
                return nameof(CustomSearchesJobTypes.DatasetPostProcessExecutionJobType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetPostProcessExecutionProcessor" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContextFactory/logger parameter is null.</exception>
        public DatasetPostProcessExecutionProcessor(IFactory<CustomSearchesDbContext> dbContextFactory, ILogger logger) : base(logger)
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
            DatasetPostProcessExecutionPayloadData payload = JsonConvert.DeserializeObject<DatasetPostProcessExecutionPayloadData>(workerJob.JobPayload);

            ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(this.ServiceContext);
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

                UserProject ownerProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

                DatasetPostProcess datasetPostProcess = null;
                if (payload.DatasetPostProcessId >= 0)
                {
                    datasetPostProcess = await dbContext.DatasetPostProcess.FirstOrDefaultAsync(dp => dp.DatasetPostProcessId == payload.DatasetPostProcessId);
                }
    
                this.NotificationPayload = new PostProcessUserNotificationPayload()
                {
                    DatasetId = payload.DatasetId,
                    PostProcessId = payload.DatasetPostProcessId,
                    PostProcessRole = datasetPostProcess?.PostProcessRole,
                    PostProcessType = datasetPostProcess?.PostProcessType,
                    ProjectId = ownerProject?.UserProjectId
                };

                try
                {
                    await DatasetHelper.GetAlterDatasetLockAsyncV2(
                        async (datasetState, datasetPostProcessState) =>
                        {
                            await service.ExecuteDatasetPostProcessAsync(payload, dbContext, (string message) => { this.LogMessage(workerJob, message); });
                            
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
                        this.ServiceContext,
                        skipAlterDatasetLock: payload.SingleRowExecutionData.IsSingleRowExecutionMode);
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

            payload.AdditionalData = string.Empty;
            workerJob.JobPayload = JsonConvert.SerializeObject(payload);

            return true;
        }

        /// <summary>
        /// Gets the SignalR notification payload.
        /// </summary>
        /// <param name="workerJob">The worker job queue.</param>
        /// <returns>The payload.</returns>
        public override object GetSinalRNotificationPayload(WorkerJobQueue workerJob)
        {
            DatasetPostProcessExecutionPayloadData payload = JsonConvert.DeserializeObject<DatasetPostProcessExecutionPayloadData>(workerJob.JobPayload);
            return payload;
        }

        /// <summary>
        /// Creates a user notification.
        /// </summary>
        /// <param name="workerJob">The worker job.</param>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="text">The text.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="errorMessage">The error message.</param>
        public override async Task CreateUserNotificationAsync(
            WorkerJobQueue workerJob,
            JobNotificationType notificationType,
            string text,
            string errorMessage = null)
        {
            await base.CreateUserNotificationAsync(workerJob, notificationType, text, errorMessage);
        }

        /// <summary>
        /// Gets the user notification payload.
        /// </summary>
        protected override object GetUserNotificationPayload()
        {
            return this.NotificationPayload;
        }
    }
}
