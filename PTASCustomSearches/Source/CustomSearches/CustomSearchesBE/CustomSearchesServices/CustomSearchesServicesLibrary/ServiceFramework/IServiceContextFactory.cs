namespace CustomSearchesServicesLibrary.ServiceFramework
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Auth;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using SignalRManager;

    /// <summary>
    /// The service context factory interface.
    /// </summary>
    public interface IServiceContextFactory
    {
        /// <summary>
        /// Gets the client credentials.
        /// </summary>
        ClientCredential AppCredential { get; }

        /// <summary>
        /// Gets the authorization token provider factory.
        /// </summary>
        IFactory<IAuthProvider> AuthTokenProviderFactory { get; }

        /// <summary>
        /// Gets the suffix for the job queue.
        /// </summary>
        string JobQueueSuffix { get; }

        /// <summary>
        /// Gets  the custom searches database context factory.
        /// </summary>
        IFactory<CustomSearchesDbContext> CustomSearchesDbContextFactory { get; }

        /// <summary>
        /// Gets the message publisher factory.
        /// </summary>
        public IMessagePublisherFactory MessagePublisherFactory { get; }

        /// <summary>
        /// Gets the cloud storage provider.
        /// </summary>
        public ICloudStorageProvider CloudStorageProvider { get; }

        /// <summary>
        /// Gets the premium cloud storage provider.
        /// </summary>
        public ICloudStorageProvider PremiumCloudStorageProvider { get; }

        /// <summary>
        /// Gets the telemetry client.
        /// </summary>
        public TelemetryClient TelemetryClient { get; }

        /// <summary>
        /// Validate the access token, returning the security principal in a result.
        /// </summary>
        /// <param name="request">The HTTP request containing the access token.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A result that contains the security principal.</returns>
        Task<IServiceContext> CreateFromHttpRequestAsync(HttpRequest request, ILogger logger);

        /// <summary>
        /// Creates service context from the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="runningInJobContext">Value indicating whether it is running in job context.</param>
        /// <returns>The service context.</returns>
        Task<IServiceContext> CreateFromUserIdAsync(Guid userId, bool runningInJobContext);

        /// <summary>
        /// Creates service context from the worker.
        /// </summary>
        /// <returns>The service context.</returns>
        IServiceContext CreateFromWorker();
    }
}