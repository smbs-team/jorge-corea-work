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
    /// Azure function class for the GetMetadataStoreItem service.  The service gets the metadata store items.
    /// </summary>
    public class GetMetadataStoreItem
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
        /// Initializes a new instance of the <see cref="GetMetadataStoreItem"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetMetadataStoreItem(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the metadata store items.
        /// </summary>
        /// <group>Shared</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Shared/GetMetadataStoreItem/{storeType}/{itemName}</url>
        /// <param name="req">The request.</param>
        /// <param name="storeType">The store data type.</param>
        /// <param name="itemName">The item name.</param>
        /// <param name="log">The log.</param>
        /// <param name="latestVersion" cref="bool" in="query">Value indicating whether the results should include only the latest version.</param>
        /// <returns>
        /// The collection of metadata store items.
        /// </returns>
        /// <response code="200" cref="GetMetadataStoreItemsResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        [FunctionName("GetMetadataStoreItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Shared/GetMetadataStoreItem/{storeType}/{itemName}")] HttpRequest req, string storeType, string itemName, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool latestVersion = true;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("latestVersion") == true)
                        {
                            latestVersion = bool.Parse(req.Query["latestVersion"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetMetadataStoreItemsService service = new GetMetadataStoreItemsService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetMetadataStoreItemsResponse response = await service.GetMetadataStoreItemAsync(storeType, itemName, latestVersion, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
