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
    /// Azure function class for the GetDatasetColumnRangeValues service.  The service gets the dataset column range values.
    /// </summary>
    public class GetDatasetColumnRangeValues
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The SQL Server data db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDataDbContext> dataDbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetColumnRangeValues"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="dataDbContextFactory">The SQL Server data db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetDatasetColumnRangeValues(IFactory<CustomSearchesDbContext> dbContextFactory, IFactory<CustomSearchesDataDbContext> dataDbContextFactory, IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (dataDbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dataDbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.dbContextFactory = dbContextFactory;
            this.dataDbContextFactory = dataDbContextFactory;
        }

        /// <summary>
        /// Gets the dataset column range values.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetColumnRangeValues/{datasetId}/{columnName}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="columnName"  cref="string" in="path">The column name.</param>
        /// <param name="log">The log.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="ignoreEmpty" cref="bool" in="query">Value indicating whether should be ignored empty values.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <returns>
        /// The dataset column range values.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("GetDatasetColumnRangeValues")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetDatasetColumnRangeValues/{datasetId}/{columnName}")] HttpRequest req, Guid datasetId, string columnName, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool usePostProcess = true;
                    bool ignoreEmpty = true;
                    int? postProcessId = null;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("usePostProcess") == true)
                        {
                            usePostProcess = bool.Parse(req.Query["usePostProcess"].ToString());
                        }

                        if (req.Query.ContainsKey("ignoreEmpty") == true)
                        {
                            ignoreEmpty = bool.Parse(req.Query["ignoreEmpty"].ToString());
                        }

                        if (req.Query.ContainsKey("postProcessId") == true)
                        {
                            postProcessId = int.Parse(req.Query["postProcessId"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetDatasetColumnRangeValuesService service = new GetDatasetColumnRangeValuesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserCustomSearchDataResponse response = await service.GetDatasetColumnRangeValuesAsync(datasetId, usePostProcess, postProcessId, columnName, ignoreEmpty, dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
