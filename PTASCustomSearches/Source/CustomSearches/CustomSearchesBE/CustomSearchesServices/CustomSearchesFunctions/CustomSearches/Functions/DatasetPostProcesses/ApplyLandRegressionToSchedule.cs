namespace CustomSearchesFunctions.CustomSearches.Functions.DatasetPostProcesses
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services.DatasetPostProcesses;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ApplyLandRegressionToSchedule service.
    /// The service applies the land regression to schedule.
    /// </summary>
    public class ApplyLandRegressionToSchedule
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
        /// Initializes a new instance of the <see cref="ApplyLandRegressionToSchedule"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public ApplyLandRegressionToSchedule(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Applies the land regression to schedule.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ApplyLandRegressionToSchedule/{rScriptPostProcessId}/{exceptionPostProcessId}</url>
        /// <param name="req">The request.</param>
        /// <param name="rScriptPostProcessId" cref="int" in="path">The RScript post process id.</param>
        /// <param name="exceptionPostProcessId" cref="int" in="path">The exception post process id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200" cref="IdResult">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("ApplyLandRegressionToSchedule")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "API/CustomSearches/ApplyLandRegressionToSchedule/{rScriptPostProcessId}/{exceptionPostProcessId}")]
            HttpRequest req,
            int rScriptPostProcessId,
            int exceptionPostProcessId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ApplyLandRegressionToScheduleService service = new ApplyLandRegressionToScheduleService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        int jobId = await service.QueueApplyLandRegressionToScheduleAsync(
                            rScriptPostProcessId,
                            exceptionPostProcessId,
                            dbContext);

                        return new OkObjectResult(new IdResult(jobId));
                    }
                },
                req,
                log);
        }
    }
}
