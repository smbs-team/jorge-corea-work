namespace CustomSearchesFunctions.CustomSearches.Functions.Datasets
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.WorkerJob;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ExecuteDatasetBackendUpdate service.  The service queues a job that will perform a backend update when executed.
    /// </summary>
    public class ExecuteDatasetBackendUpdate
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The SQL Server worker job db context factory.
        /// </summary>
        private readonly IFactory<WorkerJobDbContext> workerJobDbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteDatasetBackendUpdate" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="workerJobDbContextFactory">The worker job database context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ExecuteDatasetBackendUpdate(
            IFactory<CustomSearchesDbContext> dbContextFactory,
            IFactory<WorkerJobDbContext> workerJobDbContextFactory,
            IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            if (workerJobDbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(workerJobDbContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.workerJobDbContextFactory = workerJobDbContextFactory;
            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Executes a dataset backend update.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ExecuteDatasetBackendUpdate/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The Id of the queued Job.  -1 if no job was queued (because there was nothing to update).
        /// </returns>
        /// <response code="200" cref="IdResult">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("ExecuteDatasetBackendUpdate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "API/CustomSearches/ExecuteDatasetBackendUpdate/{datasetId}")] HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new ExecuteDatasetBackendUpdateService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    using (WorkerJobDbContext workerJobDbContext = this.workerJobDbContextFactory.Create())
                    {
                        int jobId = await service.QueueExecuteDatasetBackendUpdateAsync(datasetId, dbContext, workerJobDbContext);

                        return new OkObjectResult(new IdResult(jobId));
                    }
                },
                req,
                log);
        }
    }
}
