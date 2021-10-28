namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ExecuteDatasetPostProcess service.  The service executes a dataset post process.
    /// </summary>
    public class ExecuteDatasetPostProcess
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the custom search parameters.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteDatasetPostProcess"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ExecuteDatasetPostProcess(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Executes a dataset post process.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ExecuteDatasetPostProcess/{datasetId}/{datasetPostProcessId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="datasetPostProcessId" cref="int" in="path">The dataset post process id. Use -1 to execute all the post processes.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="T:CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches.CustomSearchParameterValueData[]" in="body">Post process parameters.</param>
        /// <returns>
        /// A unique Id that can be used to get the database data.
        /// </returns>
        /// <response code="200" cref="IdResult">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("ExecuteDatasetPostProcess")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ExecuteDatasetPostProcess/{datasetId}/{datasetPostProcessId}")] HttpRequest req, Guid datasetId, int datasetPostProcessId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    CustomSearchParameterValueData[] parameterObjects = null;
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        try
                        {
                            parameterObjects = JsonConvert.DeserializeObject<CustomSearchParameterValueData[]>(body);
                        }
                        catch (Newtonsoft.Json.JsonException jsonSerializationException)
                        {
                            throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ExecuteDatasetPostProcessService service = new ExecuteDatasetPostProcessService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var idResult = await service.QueueExecuteDatasetPostProcessAsync(datasetId, datasetPostProcessId, major: null, minor: null, parameterObjects, dataStream: null, dbContext);

                        return new OkObjectResult(idResult);
                    }
                },
                req,
                log);
        }
    }
}
