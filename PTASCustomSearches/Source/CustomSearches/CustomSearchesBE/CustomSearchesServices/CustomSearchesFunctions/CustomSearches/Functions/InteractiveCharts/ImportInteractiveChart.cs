namespace CustomSearchesFunctions.CustomSearches.Functions.InteractiveCharts
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.InteractiveCharts;
    using CustomSearchesServicesLibrary.CustomSearches.Services.InteractiveCharts;
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
    /// Azure function class for the ImportInteractiveChart service.  The service imports an interactive chart.
    /// </summary>
    public class ImportInteractiveChart
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the interactive chart data.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportInteractiveChart"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public ImportInteractiveChart(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Imports an interactive chart.
        /// The chart title is used as the key to identify the interactive chart. If no matching interactive chart is found, a new one is created.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ImportInteractiveChart</url>
        /// <param name="req">The request.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="InteractiveChartData" in="body">Post process to import.</param>
        /// <returns>
        /// The interactive chart id.
        /// </returns>
        /// <response code="200" cref="IdResult">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("ImportInteractiveChart")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ImportInteractiveChart")] HttpRequest req, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    InteractiveChartData chartData = null;
                    try
                    {
                        chartData = JsonConvert.DeserializeObject<InteractiveChartData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ImportInteractiveChartService service = new ImportInteractiveChartService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        IdResult id = await service.ImportInteractiveChartAsync(chartData, dbContext);
                        return new JsonResult(id);
                    }
                },
                req,
                log);
        }
    }
}
