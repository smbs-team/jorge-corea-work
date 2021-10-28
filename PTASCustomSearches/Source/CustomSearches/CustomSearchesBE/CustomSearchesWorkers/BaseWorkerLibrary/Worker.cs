namespace BaseWorkerLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;

    using CustomSearchesWorkerLibrary.RScript;

    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.EntityFrameworkCore;

    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesEFLibrary.WorkerJob.Model;

    using CustomSearchesServicesLibrary.Jobs.Enumeration;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    using Newtonsoft.Json;

    using PTASServicesCommon.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base worker class.
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// The queue thread sleep interval in milliseconds.
        /// </summary>
        private const int QueueThreadSleepIntervalMs = 5;

        /// <summary>
        /// The queue thread sleep interval in milliseconds when SignalR is not connected.
        /// </summary>
        private const int QueueThreadSleepIntervalMsNoSignalR = 3000;

        /// <summary>
        /// The main thread sleep interval in milliseconds.
        /// </summary>
        private const int MainThreadSleepIntervalMs = 5000;

        /// <summary>
        /// The job retry limit.
        /// </summary>
        private const int JobRetryLimit = 3;

        /// <summary>
        /// The RScript database context.
        /// </summary>
        private readonly IFactory<WorkerJobDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The worker definition.
        /// </summary>
        private readonly WorkerQueueDefinition[] workerQueueDefinitions;

        /// <summary>
        /// The SignalR hub endpoint.
        /// </summary>
        private readonly string signalRHubEndpoint;

        /// <summary>
        /// The map server storage share path.
        /// </summary>
        private readonly string mapServerStorageSharePath;

        /// <summary>
        /// The job processors.
        /// </summary>
        private readonly Dictionary<string, IFactory<WorkerJobProcessor>> jobProcessors;

        /// <summary>
        /// The SignalR hub connection.
        /// </summary>
        private HubConnection hubConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <param name="workerQueueDefinitions">The queues definition.</param>
        /// <param name="jobProcessors">The job processors.</param>
        /// <param name="signalRHubEndpoint">The SignalR hub endpoint.</param>
        /// <param name="mapServerStorageSharePath">The map server storage share path.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">When the dbContext/logger parameter is null.</exception>
        public Worker(
            IFactory<WorkerJobDbContext> dbContextFactory,
            IServiceContextFactory serviceContextFactory,
            WorkerQueueDefinition[] workerQueueDefinitions,
            IFactory<WorkerJobProcessor>[] jobProcessors,
            string signalRHubEndpoint,
            string mapServerStorageSharePath,
            ILogger logger)
        {
            if (workerQueueDefinitions == null || workerQueueDefinitions.Length == 0)
            {
                throw new ArgumentNullException(nameof(workerQueueDefinitions));
            }

            if (jobProcessors == null || jobProcessors.Length == 0)
            {
                throw new ArgumentNullException(nameof(workerQueueDefinitions));
            }

            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            this.serviceContextFactory = serviceContextFactory ?? throw new ArgumentNullException(nameof(serviceContextFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.workerQueueDefinitions = workerQueueDefinitions;
            this.signalRHubEndpoint = signalRHubEndpoint ?? throw new ArgumentNullException(nameof(signalRHubEndpoint));
            this.mapServerStorageSharePath = mapServerStorageSharePath ?? throw new ArgumentNullException(nameof(mapServerStorageSharePath));
            this.jobProcessors = new Dictionary<string, IFactory<WorkerJobProcessor>>();

            foreach (var jobProcessor in jobProcessors)
            {
                WorkerJobProcessor tempProcessor = jobProcessor.Create();
                this.jobProcessors.Add(tempProcessor.ProcessorType, jobProcessor);
            }
        }

        /// <summary>
        /// Starts the listening of SignalR messages.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task RealTimeNotificationListen()
        {
            if (this.hubConnection == null)
            {
                this.hubConnection = new HubConnectionBuilder().WithUrl(this.signalRHubEndpoint).Build();
                this.hubConnection.On("JobQueued", (string arg1, object arg2, object arg3, object arg4) =>
                {
                    SignalRNotificationQueue.Notify(queueName: arg1);
                    this.logger.LogInformation($"Real time notification received in queue {arg1}.");
                });
            }

            if (this.hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await this.hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Exception connecting to SignalR: " + ex.GetBaseException().Message);
                }
            }
        }

        /// <summary>
        /// Starts the listening to all queues.
        /// </summary>
        public void Listen()
        {
            // To initialize the hub connection before being used by the queue threads.
            this.RealTimeNotificationListen().Wait();

            foreach (WorkerQueueDefinition queue in this.workerQueueDefinitions)
            {
                for (int i = 0; i < queue.MaxParallelJobs; i++)
                {
                    var queueThread = new Thread(this.QueueThread)
                    {
                        IsBackground = true,
                    };
                    queueThread.Start(queue);
                }
            }

            var folderProbe = new FolderProbe(this.mapServerStorageSharePath, this.logger);

            while (!WorkerJobCoordinator.ShouldClose)
            {
                folderProbe.CheckFolderAccess();

                // To reconnect the hub connection if required
                this.RealTimeNotificationListen().Wait();
                WorkerJobCoordinator.Coordinate(this.logger);
                Thread.Sleep(Worker.MainThreadSleepIntervalMs);
            }
        }

        /// <summary>
        /// Runs a worker job processor.
        /// </summary>
        /// <param name="jobType">The job type.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <param name="jobPayload">The job payload.</param>
        public void RunJob(string jobType, int timeoutInSeconds, string jobPayload)
        {
            WorkerJobQueue nextJob = new WorkerJobQueue()
            {
                JobType = jobType,
                QueueName = "Scheduled",
                UserId = Guid.Empty,
                JobPayload = jobPayload,
                TimeoutInSeconds = timeoutInSeconds,
                CreatedTimestamp = DateTime.UtcNow,
                StartedTimestamp = DateTime.UtcNow,
                KeepAliveTimestamp = DateTime.UtcNow,
            };

            using (WorkerJobDbContext dbContext = this.dbContextFactory.Create())
            {
                dbContext.WorkerJobQueue.Add(nextJob);
                dbContext.SaveChangesWithRetriesAsync().Wait();
            }

            this.logger.LogInformation($"Starting job {nextJob.JobId}!");

            this.ControlNextJobAsync(nextJob, threadId: -1).Wait();
        }

        private void QueueThread(object data)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            WorkerQueueDefinition queueDefinition = data as WorkerQueueDefinition;
            WorkerJobCoordinator.RegisterQueueThread(threadId, queueDefinition.QueueName, this.logger);

            this.logger.LogInformation($"Listening for jobs in queue {queueDefinition.QueueName + this.serviceContextFactory.JobQueueSuffix}...");

            Random random = new Random();
            while (!WorkerJobCoordinator.ShouldClose && WorkerJobCoordinator.IsThreadHealthy(threadId))
            {
                bool result = false;

                try
                {
                    result = this.ExecuteNextJobAsync(queueDefinition.QueueName, threadId).Result;
                }
                catch (System.Exception ex)
                {
                    this.logger.LogError(ex, $"Exception while processing jobs: {ex.GetBaseException().Message}");
                }

                if (!result)
                {
                    WorkerJobCoordinator.KeepAliveThread(threadId);

                    if (this.hubConnection.State == HubConnectionState.Connected)
                    {
                        TimeSpan ellapsedTime = default;
                        DateTime initialTime = DateTime.UtcNow;
                        double queueDefinitionPollIntervalM = queueDefinition.PollIntervalMs + (random.NextDouble() * queueDefinition.PollIntervalMs * 0.5);
                        while (ellapsedTime.TotalMilliseconds < queueDefinitionPollIntervalM)
                        {
                            if (SignalRNotificationQueue.WasNotified(queueDefinition.QueueName + this.serviceContextFactory.JobQueueSuffix))
                            {
                                break;
                            }

                            int maxQueueThreadSleepIntervalMs = Worker.QueueThreadSleepIntervalMs + (int)Math.Ceiling(Worker.QueueThreadSleepIntervalMs * 0.5);
                            int queueThreadSleepIntervalMs = random.Next(Worker.QueueThreadSleepIntervalMs, maxQueueThreadSleepIntervalMs);
                            Thread.Sleep(queueThreadSleepIntervalMs);

                            ellapsedTime = DateTime.UtcNow - initialTime;
                        }
                    }
                    else
                    {
                        Thread.Sleep(Worker.QueueThreadSleepIntervalMsNoSignalR);
                    }
                }
            }
        }

        /// <summary>
        /// Controls the next pending job in the queue.
        /// </summary>
        /// <param name="nextJob">The next job.</param>
        /// <param name="threadId">The thread id.</param>
        /// <returns>
        /// True if a job was executed.  False if there was no job.
        /// </returns>
        private async Task ControlNextJobAsync(WorkerJobQueue nextJob, int threadId)
        {
            bool exception = false;
            try
            {
                if (this.jobProcessors.ContainsKey(nextJob.JobType))
                {
                    WorkerJobProcessor processor = this.jobProcessors[nextJob.JobType].Create();

                    if (nextJob.UserId != Guid.Empty)
                    {
                        processor.ServiceContext = await this.serviceContextFactory.CreateFromUserIdAsync(nextJob.UserId, runningInJobContext: true);
                    }
                    else
                    {
                        processor.ServiceContext = this.serviceContextFactory.CreateFromWorker();
                    }

                    try
                    {
                        processor.CheckUser(nextJob.UserId);
                    }
                    catch (AuthorizationException ex)
                    {
                        await this.TrySaveFailedJobAsync(processor, nextJob, ex.GetBaseException().Message, ex, sendNotification: true);
                        throw;
                    }

                    DateTime startTime = DateTime.Now;
                    bool processed = false;
                    if (nextJob.RetryCount > Worker.JobRetryLimit)
                    {
                        string failedMessage = $"Too many retries: {nextJob.RetryCount}. The job should be requeued.";
                        await this.TrySaveFailedJobAsync(processor, nextJob, failedMessage, exception: null, sendNotification: false);
                    }
                    else
                    {
                        try
                        {
                            processed = await this.ProcessAvailableJob(nextJob, processor, threadId);
                        }
                        catch (Exception ex)
                        {
                            await this.TrySaveFailedJobAsync(processor, nextJob, ex.GetBaseException().Message, ex, sendNotification: true);

                            throw;
                        }
                    }

                    if (processed)
                    {
                        nextJob.ExecutionTime = (decimal)(System.DateTime.Now - startTime).TotalMilliseconds;

                        await this.SaveWorkerJobAsync(nextJob);

                        await this.SendRealTimeNotificationAsync(nextJob, processor, "Succeeded");
                    }
                    else
                    {
                        await this.SendRealTimeNotificationAsync(nextJob, processor, "Failed");
                    }
                }
                else
                {
                    string errorMessage = $"Can't find a process for job type: " + nextJob.JobType;
                    this.logger.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                exception = true;
                this.logger.LogError(ex, $"Job failed: " + ex.GetBaseException().Message);
            }

            if (!exception)
            {
                this.logger.LogInformation($"Job {nextJob.JobId} finished!");
            }
        }

        /// <summary>
        /// Processes the available job.
        /// </summary>
        /// <param name="job">The worker job queue.</param>
        /// <param name="processor">The worker job processor.</param>
        /// <param name="threadId">The thread id.</param>
        /// <returns>Task that yields a bool.</returns>
        private async Task<bool> ProcessAvailableJob(WorkerJobQueue job, WorkerJobProcessor processor, int threadId)
        {
            bool processed = false;
            bool exceptionHandled = false;
            var processAvailableJobThread = new Thread(() =>
            {
                try
                {
                    processed = processor.ProcessAvailableJob(job).Result;
                }
                catch (Exception ex)
                {
                    exceptionHandled = true;
                    string errorMessage = ex.GetBaseException().Message;
                    this.logger.LogError(ex, $"Exception while process job '{job.JobId}': {ex.GetBaseException().Message}");

                    processor.CreateUserNotificationAsync(
                        job,
                        JobNotificationType.Error,
                        "Job execution failed.",
                        errorMessage).Wait();

                    this.TrySaveFailedJobAsync(processor, job, ex.GetBaseException().Message, ex, sendNotification: false).Wait();
                }
            })
            {
                Name = processor.ProcessorType.ToString() + threadId.ToString(),

                IsBackground = true,
            };
            processAvailableJobThread.Start();
            WorkerJobCoordinator.RegisterJob(threadId, job.JobId, job.TimeoutInSeconds, this.logger);

            DateTime initialTime = DateTime.UtcNow;
            DateTime keepAliveInitialTime = DateTime.UtcNow;

            // If the worker is configured to only run a job then this loop also use the timeout of the job as an exit condition.
            DateTime expirationTime = threadId == -1 ? DateTime.UtcNow.AddSeconds(job.TimeoutInSeconds) : DateTime.MaxValue;

            while (processAvailableJobThread.IsAlive && expirationTime > DateTime.UtcNow)
            {
                WorkerJobCoordinator.KeepAliveThread(threadId);

                // We divide keep alive threshold by 3 to make sure SaveChangesWithRetriesAsync can fail at least a couple of times
                // before the job is considered unhealthy.
                if ((DateTime.UtcNow - keepAliveInitialTime).TotalSeconds > WorkerJobCoordinator.JobKeepAliveThreshold / 3.0d)
                {
                    keepAliveInitialTime = DateTime.UtcNow;

                    try
                    {
                        using (WorkerJobDbContext dbContext = this.dbContextFactory.Create())
                        {
                            var reloadedJob = await (from w in dbContext.WorkerJobQueue where w.JobId == job.JobId select w).FirstOrDefaultAsync();
                            reloadedJob.KeepAliveTimestamp = DateTime.UtcNow;
                            await dbContext.SaveChangesWithRetriesAsync();
                        }

                        WorkerJobCoordinator.KeepAliveJob(threadId);
                    }
                    catch (Exception)
                    {
                        // Keep alives are managed by the worker job coordinator.
                        // We don't care about exceptions saving the keep alive state.
                    }
                }

                await Task.Delay(300);
            }

            WorkerJobCoordinator.UnregisterJob(threadId, this.logger);

            if (string.IsNullOrWhiteSpace(job.JobResult))
            {
                if (processed)
                {
                    SucceededJobResult succeededJobResult = new SucceededJobResult
                    {
                        Status = "Success",
                    };

                    job.JobResult = JsonConvert.SerializeObject(succeededJobResult);
                }
                else if (processAvailableJobThread.IsAlive && expirationTime <= DateTime.UtcNow)
                {
                    FailedJobResult failedJobResult = new FailedJobResult
                    {
                        Status = "Failed",
                        Reason = "Execution time exceeded the specified timeout.",
                    };

                    job.JobResult = JsonConvert.SerializeObject(failedJobResult);
                }
            }

            // We just notified the user in case notification was created during exception handling.
            if (!exceptionHandled)
            {
                await this.NotifyUserJobResult(job, processor);
            }

            return processed;
        }

        /// <summary>
        /// Notifies the job result to the user.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="processor">The processor.</param>
        private async Task NotifyUserJobResult(WorkerJobQueue job, WorkerJobProcessor processor)
        {
            var jobResult = JsonHelper.DeserializeObject<FailedJobResult>(job.JobResult);
            if (jobResult == null || jobResult?.Status.ToLower() != "success")
            {
                string reason = jobResult?.Reason ?? "Job execution failed.";

                await processor.CreateUserNotificationAsync(
                    job,
                    JobNotificationType.Error,
                    reason);
            }
            else
            {
                await processor.CreateUserNotificationAsync(
                    job,
                    JobNotificationType.Notification,
                    "Job execution successful.");
            }
        }

        /// <summary>
        /// Executes the next pending job in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="threadId">The thread id.</param>
        /// <returns>
        /// True if a job was executed.  False if there was no job.
        /// </returns>
        private async Task<bool> ExecuteNextJobAsync(string queueName, int threadId)
        {
            WorkerJobQueue nextJob = null;
            bool foundJob = false;

            try
            {
                using WorkerJobDbContext dbContext = this.dbContextFactory.Create();
                nextJob = dbContext.PopNextWorkerJob(queueName + this.serviceContextFactory.JobQueueSuffix, WorkerJobCoordinator.TimeThreshold);
                foundJob = nextJob != null && nextJob.JobId >= 0;
            }
            catch (Exception ex)
            {
                if (DynamicSqlStatementHelper.ContainSqlException(ex))
                {
                    this.logger.LogError(ex, "Exception retrieving the next job:" + ex.GetBaseException().Message);
                    WorkerJobCoordinator.ReportCriticalException(ex, this.logger);
                }
                else
                {
                    throw;
                }
            }

            if (foundJob)
            {
                await this.ControlNextJobAsync(nextJob, threadId);
            }

            return foundJob;
        }

        /// <summary>
        /// Tries to save the failed job.
        /// </summary>
        /// <param name="processor">The worker job processor.</param>
        /// <param name="workerJob">The worker job.</param>
        /// <param name="reason">The reason of the fail.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="sendNotification">A value indicating whether a notification should be sent.</param>
        /// <returns>
        /// True if the job was saved, otherwise false.
        /// </returns>
        private async Task<bool> TrySaveFailedJobAsync(WorkerJobProcessor processor, WorkerJobQueue workerJob, string reason, Exception exception, bool sendNotification)
        {
            if (string.IsNullOrWhiteSpace(workerJob.JobResult))
            {
                FailedJobResult failedJobResult = new FailedJobResult
                {
                    Reason = reason,
                    Status = "Failed",
                    Exception = exception?.ToString(),
                };

                // If it is expression validation exception add aditional info
                CustomExpressionValidationException validationException = exception as CustomExpressionValidationException;
                if (exception is AggregateException aggregateException)
                {
                    validationException = aggregateException.InnerExceptions.OfType<CustomExpressionValidationException>().FirstOrDefault();
                }

                if (validationException != null)
                {
                    dynamic additionalInfo = new System.Dynamic.ExpandoObject();
                    additionalInfo.CustomExpressionValidationExceptionType = validationException.CustomExpressionValidationExceptionType.ToString();
                    additionalInfo.ValidationErrors = validationException.ValidationErrors;
                    failedJobResult.AdditionalInfo = additionalInfo;
                }

                workerJob.JobResult = JsonConvert.SerializeObject(failedJobResult);
            }

            try
            {
                await this.SaveWorkerJobAsync(workerJob);

                return true;
            }
            catch (Exception)
            {
                // Since we are failing the job it doesn't matter if the job is updated
                // If it isn't another worker thread will pick it up at some point.
            }
            finally
            {
                if (sendNotification)
                {
                    await this.SendRealTimeNotificationAsync(workerJob, processor, "Failed");
                }
            }

            return false;
        }

        /// <summary>
        /// Sends the realtime notification to client and worker hubs.
        /// </summary>
        /// <param name="workerJob">The worker job.</param>
        /// <param name="processor">The worker job processor.</param>
        /// <param name="status">The worker job status.</param>
        private async Task SendRealTimeNotificationAsync(WorkerJobQueue workerJob, WorkerJobProcessor processor, string status)
        {
            var signalRPayload = processor.GetSinalRNotificationPayload(workerJob);
            await processor.ServiceContext.SendRealTimeNotificationAsync(
                "UIClientHub",
                workerJob.UserId.ToString(),
                "JobProcessed",
                processor.ProcessorType,
                workerJob.JobId,
                status,
                signalRPayload);

            await processor.ServiceContext.SendRealTimeNotificationAsync(
                "WorkerHub",
                userId: null,
                "JobProcessed",
                processor.ProcessorType,
                workerJob.JobId,
                status,
                workerJob.JobResult);
        }

        /// <summary>
        /// Saves the worker job.
        /// </summary>
        /// <param name="workerJob">The worker job.</param>
        private async Task SaveWorkerJobAsync(WorkerJobQueue workerJob)
        {
            using WorkerJobDbContext dbContext = this.dbContextFactory.Create();
            var reloadedJob = await (from w in dbContext.WorkerJobQueue where w.JobId == workerJob.JobId select w).FirstOrDefaultAsync();
            reloadedJob.ExecutionTime = workerJob.ExecutionTime;
            reloadedJob.JobResult = workerJob.JobResult;
            reloadedJob.JobPayload = workerJob.JobPayload;

            await dbContext.SaveChangesWithRetriesAsync();
        }
    }
}