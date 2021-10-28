namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetDatasetColumns service.  The service gets the dataset columns.
    /// </summary>
    public class GetDatasetColumns
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
        /// Initializes a new instance of the <see cref="GetDatasetColumns"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public GetDatasetColumns(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the dataset columns.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetColumns/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <param name="includeDependencies" cref="bool" in="query">Value indicating whether should be included the dependencies.</param>
        /// <returns>
        /// The dataset columns.
        /// </returns>
        /// <response code="200" cref="GetDatasetColumnsResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("GetDatasetColumns")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetDatasetColumns/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            string includeDependenciesParameter = req.Query != null ? (string)req.Query["includeDependencies"] : null;
            bool includeDependencies = false;
            bool.TryParse(includeDependenciesParameter, out includeDependencies);

            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool usePostProcess = true;
                    int? postProcessId = null;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("usePostProcess") == true)
                        {
                            usePostProcess = bool.Parse(req.Query["usePostProcess"].ToString());
                        }

                        if (req.Query.ContainsKey("postProcessId") == true)
                        {
                            postProcessId = int.Parse(req.Query["postProcessId"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetDatasetColumnsService service = new GetDatasetColumnsService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetDatasetColumnsResponse response = await service.GetDatasetColumnsAsync(datasetId, usePostProcess, postProcessId, includeDependencies, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
