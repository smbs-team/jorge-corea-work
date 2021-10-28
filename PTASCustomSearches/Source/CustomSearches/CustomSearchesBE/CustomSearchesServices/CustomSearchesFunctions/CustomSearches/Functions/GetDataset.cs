namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetDataset service.  The service gets the dataset.
    /// </summary>
    public class GetDataset
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
        /// Initializes a new instance of the <see cref="GetDataset"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/dataDbContextFactory/cloudStorageProvider parameter is null.</exception>
        public GetDataset(IFactory<CustomSearchesDbContext> dbContextFactory, ICloudStorageProvider cloudStorageProvider, IServiceContextFactory serviceContextFactory)
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
        /// Gets the dataset.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDataset/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="int" in="path">The dataset id.</param>
        /// <param name="includeDependencies" cref="bool" in="query">Value indicating whether should be included the dependencies.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The dataset.
        /// </returns>
        /// <response code="200" cref="GetDatasetResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetDataset")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetDataset/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            string includeDependenciesParameter = req.Query != null ? (string)req.Query["includeDependencies"] : null;
            bool includeDependencies = false;
            bool.TryParse(includeDependenciesParameter, out includeDependencies);

            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetDatasetService service = new GetDatasetService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetDatasetResponse response = await service.GetDatasetAsync(datasetId, includeDependencies, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
