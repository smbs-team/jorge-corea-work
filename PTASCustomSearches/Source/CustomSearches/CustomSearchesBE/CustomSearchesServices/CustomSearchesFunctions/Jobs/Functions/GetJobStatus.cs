namespace CustomSearchesFunctions.Jobs.Functions
{
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Jobs.Model;
    using CustomSearchesServicesLibrary.Jobs.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Azure function class to get the job status.
    /// </summary>
    public class GetJobStatus
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetJobStatus"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public GetJobStatus(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the job status.
        /// </summary>
        /// <group>Jobs</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Jobs/GetJobStatus/{jobId}</url>
        /// <param name="req">The request.</param>
        /// <param name="jobId" cref="int" in="path">The job id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The user info.
        /// </returns>
        [FunctionName("GetJobStatus")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Jobs/GetJobStatus/{jobId}")] HttpRequest req, int jobId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetJobStatusService service = new GetJobStatusService(serviceContext);
                    GetJobStatusResponse response = await service.GetJobStatus(jobId);
                    return new JsonResult(response);
                },
                req,
                log);
        }
    }
}
