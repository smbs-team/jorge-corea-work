namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesServicesLibrary.Auth;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using SignalRManager;

    /// <summary>
    /// The service context interface.
    /// </summary>
    public interface IServiceContext
    {
        /// <summary>
        /// Gets the client credentials.
        /// </summary>
        ClientCredential AppCredential { get; }

        /// <summary>
        /// Gets the authorization provider.
        /// </summary>
        IAuthProvider AuthProvider { get; }

        /// <summary>
        /// Gets the database context factory.
        /// </summary>
        IFactory<CustomSearchesDbContext> DbContextFactory { get; }

        /// <summary>
        /// Gets the gis database context factory.
        /// </summary>
        IFactory<GisDbContext> GisDbContextFactory { get; }

        /// <summary>
        /// Gets the data database context factory.
        /// </summary>
        IFactory<CustomSearchesDataDbContext> DataDbContextFactory { get; }

        /// <summary>
        /// Gets the worker job db context factory.
        /// </summary>
        IFactory<WorkerJobDbContext> WorkerJobDbContextFactory { get; }

        /// <summary>
        /// Gets the message publisher factory.
        /// </summary>
        IMessagePublisherFactory MessagePublisherFactory { get; }

        /// <summary>
        /// Gets the cloud storage provider.
        /// </summary>
        ICloudStorageProvider CloudStorageProvider { get; }

        /// <summary>
        /// Gets the premium cloud storage provider.
        /// </summary>
        ICloudStorageProvider PremiumCloudStorageProvider { get; }

        /// <summary>
        /// Gets the execution folder.
        /// </summary>
        string ExecutionFolder { get; }

        /// <summary>
        /// Gets the temporary files folder.
        /// </summary>
        string TemporaryFilesFolder { get; }

        /// <summary>
        /// Gets the suffix that will be appended to job queue names.
        /// </summary>
        string JobQueueSuffix { get; }

        /// <summary>
        /// Gets the SignalR hub endpoint.
        /// </summary>
        public string SignalRHubEndpoint { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Gets the telemetry client.
        /// </summary>
        TelemetryClient TelemetryClient { get; }

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
        public Task SendRealTimeNotificationAsync(string hubName, string userId, string method, object arg1, object arg2 = null, object arg3 = null, object arg4 = null);

        /// <summary>
        /// Adds worker job queue.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="jobType">The dataset id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="jobPayload">The job payload.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The jobid.</returns>
        public Task<int> AddWorkerJobQueueAsync(string queueName, string jobType, Guid userId, object jobPayload, int timeout);

        /// <summary>
        /// Waits until the specified job is completed to get the job result.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="jobSleepIntervalMs">The job sleep interval in milliseconds.</param>
        /// <returns>The job result.</returns>
        public Task<string> WaitForJobResultAsync(int jobId, int jobSleepIntervalMs = 5000);
    }
}