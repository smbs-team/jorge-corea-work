namespace CustomSearchesFunctions.Jobs.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Jobs.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure function class to start async jobs.
    /// </summary>
    public class StartJob
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the job payload.";

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartJob"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dataDbContextFactory/serviceContextFactory parameter is null.</exception>
        public StartJob(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Starts a job.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="jobType">Type of the processor.</param>
        /// <param name="log">The log.</param>
        /// <param name="timeout" cref="int" in="query">Specifies a timeout for the job.</param>
        /// <returns>
        /// The user info.
        /// </returns>
        /// <group>Jobs</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/Jobs/StartJob</url>
        [FunctionName("StartJob")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/Jobs/StartJob/{QueueName}/{JobType}")] HttpRequest req,
            string queueName,
            string jobType,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    InputValidationHelper.AssertNotEmpty(queueName, nameof(queueName));
                    InputValidationHelper.AssertNotEmpty(jobType, nameof(jobType));

                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    object payload = null;
                    try
                    {
                        payload = JsonConvert.DeserializeObject(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    int timeout = WorkerJobTimeouts.StartJobTimeout;
                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("timeout") == true)
                        {
                            timeout = int.Parse(req.Query["timeout"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new StartJobService(serviceContext);
                    var jobId = new IdResult(await service.StartJobAsync(queueName, jobType, payload, timeout));
                    return new OkObjectResult(jobId);
                },
                req,
                log);
        }

        private object IdResult(int v)
        {
            throw new NotImplementedException();
        }
    }
}
