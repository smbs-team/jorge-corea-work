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
    /// Azure function class to dismiss the user notifications for jobs.
    /// </summary>
    public class DismissUserJobNotification
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DismissUserJobNotification"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public DismissUserJobNotification(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Dismisses the user notifications for jobs.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="notificationId">The notification identifier. If the value is -1, all notifications are dismissed.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <group>Jobs</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Jobs/DismissUserJobNotification/{notificationId}</url>
        [FunctionName("DismissUserJobNotification")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/Jobs/DismissUserJobNotification/{notificationId}")] HttpRequest req, int notificationId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new DismissUserJobNotificationService(serviceContext);
                    await service.DismissUserJobNotification(notificationId);
                    return new OkResult();
                },
                req,
                log);
        }
    }
}
