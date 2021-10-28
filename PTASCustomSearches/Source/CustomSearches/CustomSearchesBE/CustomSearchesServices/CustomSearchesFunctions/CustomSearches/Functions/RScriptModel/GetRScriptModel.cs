namespace CustomSearchesFunctions.CustomSearches.Functions.RScriptModel
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScriptModel;
    using CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetRScriptModel service.  The service gets a rscript model.
    /// </summary>
    public class GetRScriptModel
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
        /// Initializes a new instance of the <see cref="GetRScriptModel"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetRScriptModel(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the rscript model.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetRScriptModel/{rscriptId}</url>
        /// <param name="req">The request.</param>
        /// <param name="rscriptId" cref="int" in="path">The rscript model id.</param>
        /// <param name="includeLookupValues" cref="bool" in="query">Value indicating whether the results should include the lookup values.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The rscript model.
        /// </returns>
        /// <response code="200" cref="GetRScriptModelResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetRScriptModel")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetRScriptModel/{rscriptId}")] HttpRequest req, int rscriptId, ILogger log)
        {
            string includeLookupValuesParameter = req.Query != null ? (string)req.Query["includeLookupValues"] : null;
            bool includeLookupValues = false;
            bool.TryParse(includeLookupValuesParameter, out includeLookupValues);

            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetRScriptModelService service = new GetRScriptModelService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetRScriptModelResponse response = await service.GetRScriptModelAsync(rscriptId, includeLookupValues, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}