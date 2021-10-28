namespace CustomSearchesFunctions.Shared.Functions
{
    using System.Threading.Tasks;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Extensions.SignalRService;

    /// <summary>
    /// Serverless hub that manages to the UI client connection with the Azure SignalR service.
    /// </summary>
    public class UIClientHub : ServerlessHub
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIClientHub"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory is null.</exception>
        public UIClientHub(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the SignalR connection information for the UI client hub.
        /// </summary>
        /// <group>Hub</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Negotiate</url>
        /// <param name="req">The request.</param>
        /// <returns>The SignalR connection information.</returns>
        /// <response code="200" cref="SignalRConnectionInfo">The request succeeded.</response>
        [FunctionName("Negotiate")]
        public async Task<SignalRConnectionInfo> Negotiate([HttpTrigger(AuthorizationLevel.Anonymous, Route = "API/Negotiate")]HttpRequest req)
        {
            IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, logger: null);

            return this.Negotiate(serviceContext.AuthProvider.UserInfoData.Id.ToString(), null);
        }
    }
}
