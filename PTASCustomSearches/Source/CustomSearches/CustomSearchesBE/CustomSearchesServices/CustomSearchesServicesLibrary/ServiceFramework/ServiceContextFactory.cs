namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesServicesLibrary.Auth;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using SignalRManager;

    /// <summary>
    /// The service context factory.
    /// </summary>
    public class ServiceContextFactory : IServiceContextFactory
    {
        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContextFactory" /> class.
        /// </summary>
        /// <param name="authTokenProviderFactory">The security token provider factory.</param>
        /// <param name="customSearchesDbContextFactory">The custom searches database context factory.</param>
        /// <param name="gisDbContextFactory">The gis database context factory.</param>
        /// <param name="customSearchesDataDbContextFactory">The custom searches data database context factory.</param>
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
        public ServiceContextFactory(
            IFactory<IAuthProvider> authTokenProviderFactory,
            IFactory<CustomSearchesDbContext> customSearchesDbContextFactory,
            IFactory<GisDbContext> gisDbContextFactory,
            IFactory<CustomSearchesDataDbContext> customSearchesDataDbContextFactory,
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
            this.AuthTokenProviderFactory = authTokenProviderFactory;
            this.CustomSearchesDbContextFactory = customSearchesDbContextFactory;
            this.GisDbContextFactory = gisDbContextFactory;
            this.CustomSearchesDataDbContextFactory = customSearchesDataDbContextFactory;
            this.WorkerJobDbContextFactory = workerJobDbContextFactory;
            this.MessagePublisherFactory = messagePublisherFactory;
            this.CloudStorageProvider = cloudStorageProvider;
            this.PremiumCloudStorageProvider = premiumCloudStorageProvider;
            this.AppCredential = appCredential;
            this.ExecutionFolder = executionFolder;
            this.JobQueueSuffix = jobQueueSuffix;
            this.SignalRHubEndpoint = signalRHubEndpoint;
            this.logger = logger;
            this.TelemetryClient = telemetryClient;
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
        /// Gets or sets the authorization token provider factory.
        /// </summary>
        public IFactory<IAuthProvider> AuthTokenProviderFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the custom searches db context factory.
        /// </summary>
        public IFactory<CustomSearchesDbContext> CustomSearchesDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the gis db context factory.
        /// </summary>
        public IFactory<GisDbContext> GisDbContextFactory { get; set; }

        /// <summary>
        /// Gets or sets the custom searches data db context factory.
        /// </summary>
        public IFactory<CustomSearchesDataDbContext> CustomSearchesDataDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the worker job db context factory.
        /// </summary>
        public IFactory<WorkerJobDbContext> WorkerJobDbContextFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the message publisher factory.
        /// </summary>
        public IMessagePublisherFactory MessagePublisherFactory { get; protected set; }

        /// <summary>
        /// Gets or sets execution folder.
        /// </summary>
        public string ExecutionFolder { get; protected set; }

        /// <summary>
        /// Gets or sets the job queue suffix.
        /// </summary>
        /// <value>
        /// The job queue suffix.
        /// </value>
        public string JobQueueSuffix { get; protected set; }

        /// <summary>
        /// Gets or sets the SignalR hub endpoint.
        /// </summary>
        public string SignalRHubEndpoint { get; protected set; }

        /// <summary>
        /// Gets or sets the cloud storage provider.
        /// </summary>
        public ICloudStorageProvider CloudStorageProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the premium cloud storage provider.
        /// </summary>
        public ICloudStorageProvider PremiumCloudStorageProvider { get; protected set; }

        /// <summary>
        /// Creates service context from the http request.
        /// </summary>
        /// <param name="request">The HTTP request containing the access token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>The service context.</returns>
        public async Task<IServiceContext> CreateFromHttpRequestAsync(HttpRequest request, ILogger logger)
        {
            IAuthProvider authTokenProvider = this.AuthTokenProviderFactory.Create();
            await authTokenProvider.InitFromHttpRequestAsync(request);

            // Each function has your own logger that is passed as a parameter in this method.
            ServiceContext serviceContext = new ServiceContext(
                authTokenProvider,
                this.CustomSearchesDbContextFactory,
                this.GisDbContextFactory,
                this.CustomSearchesDataDbContextFactory,
                this.WorkerJobDbContextFactory,
                this.MessagePublisherFactory,
                this.CloudStorageProvider,
                this.PremiumCloudStorageProvider,
                this.AppCredential,
                this.TelemetryClient,
                this.ExecutionFolder,
                this.JobQueueSuffix,
                this.SignalRHubEndpoint,
                logger);

            return serviceContext;
        }

        /// <summary>
        /// Creates service context from the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="runningInJobContext">Value indicating whether it is running in job context.</param>
        /// <returns>The service context.</returns>
        public async Task<IServiceContext> CreateFromUserIdAsync(Guid userId, bool runningInJobContext)
        {
            IAuthProvider authTokenProvider = this.AuthTokenProviderFactory.Create();
            await authTokenProvider.InitFromUserIdAsync(userId, runningInJobContext);

            ServiceContext serviceContext = new ServiceContext(
                authTokenProvider,
                this.CustomSearchesDbContextFactory,
                this.GisDbContextFactory,
                this.CustomSearchesDataDbContextFactory,
                this.WorkerJobDbContextFactory,
                this.MessagePublisherFactory,
                this.CloudStorageProvider,
                this.PremiumCloudStorageProvider,
                this.AppCredential,
                this.TelemetryClient,
                this.ExecutionFolder,
                this.JobQueueSuffix,
                this.SignalRHubEndpoint,
                this.logger);
            return serviceContext;
        }

        /// <summary>
        /// Creates service context from the worker.
        /// </summary>
        /// <returns>The service context.</returns>
        public IServiceContext CreateFromWorker()
        {
            IAuthProvider authTokenProvider = this.AuthTokenProviderFactory.Create();
            authTokenProvider.InitFromWorker();

            ServiceContext serviceContext = new ServiceContext(
                authTokenProvider,
                this.CustomSearchesDbContextFactory,
                this.GisDbContextFactory,
                this.CustomSearchesDataDbContextFactory,
                this.WorkerJobDbContextFactory,
                this.MessagePublisherFactory,
                this.CloudStorageProvider,
                this.PremiumCloudStorageProvider,
                this.AppCredential,
                this.TelemetryClient,
                this.ExecutionFolder,
                this.JobQueueSuffix,
                this.SignalRHubEndpoint,
                this.logger);
            return serviceContext;
        }
    }
}
