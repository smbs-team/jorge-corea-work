namespace CustomSearchesFunctions.CustomSearches.Functions.DatasetPostProcesses
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetDatasetPostProcesses service.
    /// The service gets the post-processes for a dataset.
    /// </summary>
    public class GetDatasetPostProcesses
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
        /// Initializes a new instance of the <see cref="GetDatasetPostProcesses"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetDatasetPostProcesses(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the dataset post processes.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetPostProcesses/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The user map.
        /// </returns>
        /// <response code="200" cref="GetDatasetPostProcessesResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetDatasetPostProcesses")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/CustomSearches/GetDatasetPostProcesses/{datasetId}")]
            HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetDatasetPostProcessesService service = new GetDatasetPostProcessesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetDatasetPostProcessesResponse response =
                            await service.GetDatasetPostProcessesAsync(datasetId, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
