namespace CustomSearchesFunctions.CustomSearches.Functions.InteractiveCharts
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetGetInteractiveChart service.
    /// The service gets the information for an interactive chart.
    /// </summary>
    public class GetInteractiveChart
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
        /// Initializes a new instance of the <see cref="GetInteractiveChart"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetInteractiveChart(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets an interactive chart.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetInteractiveChart/{interactiveChartId}</url>
        /// <param name="req">The request.</param>
        /// <param name="interactiveChartId" cref="int" in="path">The interactive chart id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The interactive charts for a dataset.
        /// </returns>
        /// <response code="200" cref="GetInteractiveChartResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetInteractiveChart")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/CustomSearches/GetInteractiveChart/{interactiveChartId}")]
            HttpRequest req,
            int interactiveChartId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetInteractiveChartService service = new GetInteractiveChartService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetInteractiveChartResponse response =
                            await service.GetInteractiveChartAsync(interactiveChartId, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
