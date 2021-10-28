namespace CustomSearchesFunctions.Jobs.Functions
{
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Jobs.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Azure function class to get the user notifications for jobs.
    /// </summary>
    public class GetUserJobNotifications
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserJobNotifications"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public GetUserJobNotifications(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the user notifications for jobs.
        /// </summary>
        /// <group>Jobs</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/Jobs/GetUserJobNotifications</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="maxJobs" cref="int" in="query">Value indicating the max count of pending jobs to be retrieved.</param>
        /// <param name="maxNotifications" cref="int" in="query">Value indicating the max count of notifications to be retrieved.</param>
        /// <returns>
        /// The user notifications and jobs.
        /// </returns>
        [FunctionName("GetUserJobNotifications")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/Jobs/GetUserJobNotifications")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    int maxJobs = 50;
                    int maxNotifications = 50;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("maxJobs") == true)
                        {
                            maxJobs = int.Parse(req.Query["maxJobs"].ToString());
                        }

                        if (req.Query.ContainsKey("maxNotifications") == true)
                        {
                            maxNotifications = int.Parse(req.Query["maxNotifications"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetUserJobNotificationsService(serviceContext);
                    var response = await service.GetUserJobNotifications(maxJobs, maxNotifications);
                    return new JsonResult(response);
                },
                req,
                log);
        }
    }
}
