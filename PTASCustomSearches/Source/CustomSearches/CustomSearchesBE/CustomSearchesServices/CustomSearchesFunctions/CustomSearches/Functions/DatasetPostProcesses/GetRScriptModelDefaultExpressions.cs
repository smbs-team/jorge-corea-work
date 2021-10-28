namespace CustomSearchesFunctions.CustomSearches.Functions.DatasetPostProcesses
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetRScriptModelDefaultExpressions service.
    /// The service gets the default expressions for an RScript Model.
    /// </summary>
    public class GetRScriptModelDefaultExpressions
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
        /// Initializes a new instance of the <see cref="GetRScriptModelDefaultExpressions"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetRScriptModelDefaultExpressions(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the default expressions for an RScript Model.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetRScriptModelDefaultExpressions/{rScriptModelId}</url>
        /// <param name="req">The request.</param>
        /// <param name="rScriptModelId" cref="int" in="path">The rscript model id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The default expressions for the RScriptModel.
        /// </returns>
        [FunctionName("GetRScriptModelDefaultExpressions")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "API/CustomSearches/GetRScriptModelDefaultExpressions/{rScriptModelId}")]
            HttpRequest req,
            int rScriptModelId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetRScriptModelDefaultExpressionsService service = new GetRScriptModelDefaultExpressionsService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetRScriptModelDefaultExpressionsResponse response =
                            await service.GetRScriptModelDefaultExpressionsAsync(rScriptModelId, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
