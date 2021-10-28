namespace CustomSearchesFunctions.CustomSearches.Functions.CustomSearches
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetCustomSearchParameters service.  The service gets the custom search parameters.
    /// </summary>
    public class GetCustomSearchParameters
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
        /// Initializes a new instance of the <see cref="GetCustomSearchParameters"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetCustomSearchParameters(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the custom search parameters.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetCustomSearchParameters/{customSearchDefinitionId}</url>
        /// <param name="req">The request.</param>
        /// <param name="customSearchDefinitionId" cref="int" in="path">The custom search definition id.</param>
        /// <param name="includeLookupValues" cref="bool" in="query">Value indicating whether the results should include the lookup values.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The collection of custom search parameters.
        /// </returns>
        /// <response code="200" cref="GetCustomSearchParametersResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetCustomSearchParameters")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetCustomSearchParameters/{customSearchDefinitionId}")] HttpRequest req, int customSearchDefinitionId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool includeLookupValues = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("includeLookupValues") == true)
                        {
                            includeLookupValues = bool.Parse(req.Query["includeLookupValues"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetCustomSearchParametersService service = new GetCustomSearchParametersService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetCustomSearchParametersResponse response = await service.GetCustomSearchParametersAsync(customSearchDefinitionId, includeLookupValues, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
