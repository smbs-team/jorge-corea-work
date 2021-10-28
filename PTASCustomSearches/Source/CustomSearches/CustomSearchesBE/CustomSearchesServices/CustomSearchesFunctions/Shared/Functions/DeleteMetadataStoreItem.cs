namespace CustomSearchesFunctions.CustomSearches.Functions.CustomSearches
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using CustomSearchesServicesLibrary.Shared.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the DeleteMetadataStoreItem service.  The service deletes a metadata store item.
    /// </summary>
    public class DeleteMetadataStoreItem
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
        /// Initializes a new instance of the <see cref="DeleteMetadataStoreItem"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public DeleteMetadataStoreItem(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Deletes a metadata store item.
        /// </summary>
        /// <group>Shared</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Shared/DeleteMetadataStoreItem/{metadataStoreItemId}</url>
        /// <param name="req">The request.</param>
        /// <param name="metadataStoreItemId" cref="int" in="path">The metadata store item id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// Operation result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("DeleteMetadataStoreItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/Shared/DeleteMetadataStoreItem/{metadataStoreItemId}")] HttpRequest req, int metadataStoreItemId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    DeleteMetadataStoreItemService service = new DeleteMetadataStoreItemService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        await service.DeleteMetadataStoreItemAsync(metadataStoreItemId, dbContext);
                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
