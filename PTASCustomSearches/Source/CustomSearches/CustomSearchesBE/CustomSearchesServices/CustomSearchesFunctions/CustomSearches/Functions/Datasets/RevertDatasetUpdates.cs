namespace CustomSearchesFunctions.CustomSearches.Functions.Datasets
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
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
    /// Azure function class for the RevertDatasetUpdates service.  The service revert data updates done by the user.
    /// </summary>
    public class RevertDatasetUpdates
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
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevertDatasetUpdates" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public RevertDatasetUpdates(
            IFactory<CustomSearchesDbContext> dbContextFactory,
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

            this.serviceContextFactory = serviceContextFactory;

            this.dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Reverts dataset data updates done by the user.  If rows are not specified, all rows are reverted.
        /// If rows are specified, a maximum of 250 rows can be received at a time.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/RevertDatasetUpdates/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="postBody" cref="RevertDatasetUpdatesData" in="body">Body containing the rows to revert.  If not sent all rows are reverted.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The custom search data.
        /// </returns>
        /// <response code="200" cref="GetUserCustomSearchDataResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("RevertDatasetUpdates")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/RevertDatasetUpdates/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    var body = await new StreamReader(req.Body).ReadToEndAsync();
                    RevertDatasetUpdatesData rowsToRevert = null;

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        try
                        {
                            rowsToRevert = JsonConvert.DeserializeObject<RevertDatasetUpdatesData>(body);
                        }
                        catch (Newtonsoft.Json.JsonException jsonSerializationException)
                        {
                            throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                        }
                    }

                    if (rowsToRevert == null || rowsToRevert.RowIds == null || rowsToRevert.RowIds.Length == 0)
                    {
                        rowsToRevert = null;
                    }

                    if (rowsToRevert != null && rowsToRevert.RowIds.Length > UpdateBatchSize)
                    {
                        throw new CustomSearchesRequestBodyException(string.Format(MaxRowsRequestBodyErrorMessage, UpdateBatchSize), null);
                    }

                    bool includeRevertedRows = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("includeRevertedRows") == true)
                        {
                            includeRevertedRows = bool.Parse(req.Query["includeRevertedRows"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new RevertDatasetUpdatesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserCustomSearchDataResponse result = await service.RevertDatasetUpdatesAsync(datasetId, rowsToRevert, includeRevertedRows, dbContext);
                        return new OkObjectResult(result);
                    }
                },
                req,
                log);
        }
    }
}
