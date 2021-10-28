namespace CustomSearchesFunctions.CustomSearches.Functions.Datasets
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ClearRowFilter service.  The service clears
    /// the row filter in the dataset data.
    /// </summary>
    public class ClearRowFilter
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
        /// Initializes a new instance of the <see cref="ClearRowFilter"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ClearRowFilter(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Clears the row filter in the dataset data.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ClearRowFilter/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="preserveSelection" cref="bool" in="query">Value indicating whether the selection should be preserved.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("ClearRowFilter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ClearRowFilter/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool preserveSelection = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("preserveSelection") == true)
                        {
                            preserveSelection = bool.Parse(req.Query["preserveSelection"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ClearRowFilterService service = new ClearRowFilterService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        await service.ClearRowFilterAsync(datasetId, preserveSelection, dbContext);

                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
