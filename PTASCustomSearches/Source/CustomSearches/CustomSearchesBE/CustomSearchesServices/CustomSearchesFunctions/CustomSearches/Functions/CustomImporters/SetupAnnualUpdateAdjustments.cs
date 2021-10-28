namespace CustomSearchesFunctions.CustomSearches.Functions.CustomImporters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomImporters;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomImporters;
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
    /// Azure function class for the SetupAnnualUpdateAdjustments service.
    /// The service setups the annual update adjustments creating the required post processes.
    /// </summary>
    public class SetupAnnualUpdateAdjustments
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the setup annual update adjustments parameters.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupAnnualUpdateAdjustments"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public SetupAnnualUpdateAdjustments(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Setups the annual update adjustments creating the required post processes.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/SetupAnnualUpdateAdjustments/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="SetupAnnualUpdateAdjustmentsData" in="body">The setup annual update adjustments data.</param>
        /// <returns>
        /// The annual update adjustments post processes.
        /// </returns>
        /// <response code="200" cref="SetupAnnualUpdateAdjustmentsResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("SetupAnnualUpdateAdjustments")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "API/CustomSearches/SetupAnnualUpdateAdjustments/{datasetId}")]
            HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    SetupAnnualUpdateAdjustmentsData setupAnnualUpdateAdjustmentsData = null;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(body))
                        {
                            setupAnnualUpdateAdjustmentsData = new SetupAnnualUpdateAdjustmentsData();
                        }
                        else
                        {
                            setupAnnualUpdateAdjustmentsData = JsonConvert.DeserializeObject<SetupAnnualUpdateAdjustmentsData>(body);
                        }
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    SetupAnnualUpdateAdjustmentsService service = new SetupAnnualUpdateAdjustmentsService(setupAnnualUpdateAdjustmentsData, serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var response = await service.SetupAnnualUpdateAdjustmentsAsync(
                            datasetId,
                            dbContext,
                            log);

                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
