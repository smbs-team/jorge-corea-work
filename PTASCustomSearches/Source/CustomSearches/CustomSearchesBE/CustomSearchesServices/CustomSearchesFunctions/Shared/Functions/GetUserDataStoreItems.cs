namespace CustomSearchesFunctions.CustomSearches.Functions.CustomSearches
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Model;
    using CustomSearchesServicesLibrary.Shared.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetUserDataStoreItems service.  The service gets the user data store items.
    /// </summary>
    public class GetUserDataStoreItems
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserDataStoreItems"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetUserDataStoreItems(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Gets the user data store items.
        /// Only returns the items without an owner type.
        /// </summary>
        /// <group>Shared</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Shared/GetUserDataStoreItems/{storeType}</url>
        /// <param name="req">The request.</param>
        /// <param name="storeType">The store data type.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The collection of user data store items.
        /// </returns>
        /// <response code="200" cref="GetUserDataStoreItemsResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        [FunctionName("GetUserDataStoreItems")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Shared/GetUserDataStoreItems/{storeType}")] HttpRequest req, string storeType, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetUserDataStoreItemsService service = new GetUserDataStoreItemsService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserDataStoreItemsResponse response = await service.GetUserDataStoreItemsAsync(storeType, ownerType: null, ownerObjectId: null, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
