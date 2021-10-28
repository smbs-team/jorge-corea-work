namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System;
    using System.Threading.Tasks;

    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.WorkerJob;

    using CustomSearchesServicesLibrary.Auth;

    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    using SignalRManager;

    /// <summary>
    /// The service context.
    /// </summary>
    public class ServiceContext : IServiceContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContext" /> class.
        /// </summary>
        /// <param name="authProvider">The authorization provider.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="gisDbContextFactory">The gis database context factory.</param>
        /// <param name="dataDbContextFactory">The data database context factory.</param>
        /// <param name="workerJobDbContextFactory">The worker job database context factory.</param>
        /// <param name="messagePublisherFactory">The message publisher factory.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="premiumCloudStorageProvider">The premium cloud storage provider.</param>
        /// <param name="appCredential">The application credentials.  Null if not needed.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="executionFolder">The execution folder.</param>
        /// <param name="jobQueueSuffix">The job queue suffix.</param>
        /// <param name="signalRHubEndpoint">The SignalR hub endpoint.</param>
        /// <param name="logger">The logger.</param>
        public ServiceContext(
            IAuthProvider authProvider,
            IFactory<CustomSearchesDbContext> dbContextFactory,
            IFactory<GisDbContext> gisDbContextFactory,
            IFactory<CustomSearchesDataDbContext> dataDbContextFactory,
            IFactory<WorkerJobDbContext> workerJobDbContextFactory,
            IMessagePublisherFactory messagePublisherFactory,
            ICloudStorageProvider cloudStorageProvider,
            ICloudStorageProvider premiumCloudStorageProvider,
            ClientCredential appCredential,
            TelemetryClient telemetryClient,
            string executionFolder,
            string jobQueueSuffix,
            string signalRHubEndpoint,
            ILogger logger)
        {
            this.AuthProvider = authProvider;
            this.DbContextFactory = dbContextFactory;
            this.GisDbContextFactory = gisDbContextFactory;
            this.DataDbContextFactory = dataDbContextFactory;
            this.WorkerJobDbContextFactory = workerJobDbContextFactory;
            this.ExecutionFolder = executionFolder;
            this.TemporaryFilesFolder = System.IO.Path.GetTempPath();
            this.AppCredential = appCredential;
            this.TelemetryClient = telemetryClient;
            this.JobQueueSuffix = jobQueueSuffix ?? string.Empty;
            this.SignalRHubEndpoint = signalRHubEndpoint;
            this.MessagePublisherFactory = messagePublisherFactory;
            this.CloudStorageProvider = cloudStorageProvider;
            this.PremiumCloudStorageProvider = premiumCloudStorageProvider;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets or sets the telemetry client.
        /// </summary>
        public TelemetryClient TelemetryClient { get; protected set; }

        /// <summary>
        /// Gets or sets the client credentials.
        /// </summary>
        public ClientCredential AppCredential { get; protected set; }

        /// <summary>
        /// Gets or sets the authorization provider.
        /// </summary>
        public IAuthProvider AuthProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the data database context factory.
        /// </summary>
        public IFactory<CustomSearchesDataDbContext> DataDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the worker job db context factory.
        /// </summary>
        public IFactory<WorkerJobDbContext> WorkerJobDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the database context factory.
        /// </summary>
        public IFactory<CustomSearchesDbContext> DbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the gis database context factory.
        /// </summary>
        public IFactory<GisDbContext> GisDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the message publisher factory.
        /// </summary>
        public IMessagePublisherFactory MessagePublisherFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the cloud storage provider.
        /// </summary>
        public ICloudStorageProvider CloudStorageProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the cloud storage provider.
        /// </summary>
        public ICloudStorageProvider PremiumCloudStorageProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the execution folder.
        /// </summary>
        public string ExecutionFolder { get; protected set; }

        /// <summary>
        /// Gets or sets the temporary files folder.
        /// </summary>
        public string TemporaryFilesFolder { get; protected set; }

        /// <summary>
        /// Gets or sets the suffix that will be appended to job queue names.
        /// </summary>
        public string JobQueueSuffix { get; protected set; }

        /// <summary>
        /// Gets or sets the SignalR hub endpoint.
        /// </summary>
        public string SignalRHubEndpoint { get; protected set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; protected set; }

        /// <summary>
        /// Sends a real time notification.
        /// </summary>
        /// <param name="hubName">The hub name.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="method">The method name.</param>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        /// <param name="arg4">The fourth argument.</param>
        /// <returns>The task.</returns>
        public async Task SendRealTimeNotificationAsync(string hubName, string userId, string method, object arg1, object arg2 = null, object arg3 = null, object arg4 = null)
        {
            try
            {
                await using (IMessagePublisher messagePublisher = await this.MessagePublisherFactory.CreateAsync(hubName))
                {
                    await messagePublisher.SendAsync(userId, method, arg1, arg2, arg3, arg4);
                }
            }
            catch
            {
                // Realtime notifications are optional and we don't want to crash due to a failure here.
            }
        }

        /// <summary>
        /// Adds worker job queue.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="jobType">The dataset id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="jobPayload">The job payload.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The jobid.</returns>
        public async Task<int> AddWorkerJobQueueAsync(string queueName, string jobType, Guid userId, object jobPayload, int timeout)
        {
            int workerJobId = await DbTransientRetryPolicy.AddWorkerJobQueueAsync(this, queueName + this.JobQueueSuffix, jobType, userId, jobPayload, timeout);
            await this.SendRealTimeNotificationAsync("WorkerHub", userId: null, "JobQueued", queueName + this.JobQueueSuffix);
            return workerJobId;
        }

        /// <summary>
        /// Waits until the specified job is completed to get the job result.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="jobSleepIntervalMs">The job sleep interval in milliseconds.</param>
        /// <returns>The job result.</returns>
        public async Task<string> WaitForJobResultAsync(int jobId, int jobSleepIntervalMs)
        {
            if (jobId <= 0)
            {
                return null;
            }

            string jobResult = null;
            object signalRNotificationPayload = null;
            await using (var hubConnection = new HubConnectionBuilder().WithUrl(this.SignalRHubEndpoint).Build())
            {
                hubConnection.On("JobProcessed", (string arg1, object arg2, object arg3, object arg4) =>
                {
                    if (jobId == int.Parse(arg2.ToString()))
                    {
                        signalRNotificationPayload = arg4.ToString();
                    }
                });

                while (string.IsNullOrWhiteSpace(jobResult))
                {
                    if (hubConnection.State == HubConnectionState.Disconnected)
                    {
                        try
                        {
                            await hubConnection.StartAsync();
                        }
                        catch (Exception)
                        {
                            // Real-time notifications are optional and we don't want to crash due to a failure here.
                        }
                    }

                    if (signalRNotificationPayload == null)
                    {
                        jobResult = await DbTransientRetryPolicy.GetWorkerJobResultAsync(this, jobId);
                    }
                    else
                    {
                        jobResult = signalRNotificationPayload.ToString();
                    }

                    await Task.Delay(jobSleepIntervalMs);
                }
            }

            return jobResult;
        }
    }
}