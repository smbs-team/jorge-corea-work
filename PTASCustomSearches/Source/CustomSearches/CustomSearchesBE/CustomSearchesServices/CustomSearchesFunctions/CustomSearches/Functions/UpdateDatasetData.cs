namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the UpdateDatasetData service.  The service updates the dataset data.
    /// </summary>
    public class UpdateDatasetData
    {
        /// <summary>
        /// The max number of rows.
        /// </summary>
        private const int UpdateBatchSize = 250;

        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the custom search grid data.";

        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string MaxRowsRequestBodyErrorMessage = "Exceeded the update batch size. Max size: {0}.";

        /// <summary>
        /// The empty batch request body error message.
        /// </summary>
        private const string EmptyBatchRequestBodyErrorMessage = "The update batch should have at least 1 row.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The SQL Server data db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDataDbContext> dataDbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetData"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="dataDbContextFactory">The SQL Server data db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public UpdateDatasetData(IFactory<CustomSearchesDbContext> dbContextFactory, IFactory<CustomSearchesDataDbContext> dataDbContextFactory, IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (dataDbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dataDbContextFactory));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.dbContextFactory = dbContextFactory;
            this.dataDbContextFactory = dataDbContextFactory;
        }

        /// <summary>
        /// Updates the dataset data.  This method supports a maximum of 250 rows to update at a time.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/UpdateDatasetData/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="includeUpdatedRows" cref="bool" in="query">Value indicating whether the results should include the updated rows.</param>
        /// <param name="clientId" cref="string" in="query">The client id.</param>
        /// <returns>
        /// The updated rows if returnUpdaterRows is true.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("UpdateDatasetData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/UpdateDatasetData/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    JObject[] rows;
                    try
                    {
                        rows = JsonConvert.DeserializeObject<JObject[]>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    if (rows.Length == 0)
                    {
                        throw new CustomSearchesRequestBodyException(EmptyBatchRequestBodyErrorMessage, null);
                    }

                    if (rows.Length > UpdateBatchSize)
                    {
                        throw new CustomSearchesRequestBodyException(string.Format(MaxRowsRequestBodyErrorMessage, UpdateBatchSize), null);
                    }

                    bool includeUpdatedRows = false;
                    string clientId = null;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("includeUpdatedRows") == true)
                        {
                            includeUpdatedRows = bool.Parse(req.Query["includeUpdatedRows"].ToString());
                        }

                        if (req.Query.ContainsKey("clientId") == true)
                        {
                            clientId = req.Query["clientId"].ToString();
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    UpdateDatasetDataService service = new UpdateDatasetDataService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserCustomSearchDataResponse result = await service.UpdateDatasetDataAsync(datasetId, rows, includeUpdatedRows, clientId, dbContext);
                        if (result == null)
                        {
                            return new OkResult();
                        }
                        else
                        {
                            return new OkObjectResult(result);
                        }
                    }
                },
                req,
                log);
        }
    }
}
