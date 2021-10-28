namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
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
    /// Azure function class for the DuplicateDataset service.  The service duplicates a dataset and all its child objects (including charts and DB tables).
    /// </summary>
    public class DuplicateDataset
    {
        /// <summary>
        /// The request body error message.
        /// </summary>
        private const string RequestBodyErrorMessage = "Cannot deserialize the request body that contains the dataset data.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataset"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public DuplicateDataset(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Duplicates a dataset.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/DuplicateDataset/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="postBody" cref="DuplicateDatasetData" in="body">The dataset data.</param>
        /// <param name="applyRowFilter" cref="bool" in="query">Value indicating whether the row filter should be used to define the rows to duplicate.</param>
        /// <param name="applyUserSelection" cref="bool" in="query">Value indicating whether the user selection should be used to define the rows to duplicate.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200" cref="ExecuteCustomSearchResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="405" cref="ErrorResultModel">Operation not supported.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("DuplicateDataset")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/DuplicateDataset/{datasetId}")] HttpRequest req, Guid datasetId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool applyRowFilter = false;
                    bool applyUserSelection = false;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("applyRowFilter") == true)
                        {
                            applyRowFilter = bool.Parse(req.Query["applyRowFilter"].ToString());
                        }

                        if (req.Query.ContainsKey("applyUserSelection") == true)
                        {
                            applyUserSelection = bool.Parse(req.Query["applyUserSelection"].ToString());
                        }
                    }

                    var body = await new StreamReader(req.Body).ReadToEndAsync();

                    DuplicateDatasetData duplicateDatasetData = null;

                    if (string.IsNullOrWhiteSpace(body))
                    {
                        throw new ArgumentNullException("body");
                    }

                    try
                    {
                        duplicateDatasetData = JsonConvert.DeserializeObject<DuplicateDatasetData>(body);
                    }
                    catch (Newtonsoft.Json.JsonException jsonSerializationException)
                    {
                        throw new CustomSearchesRequestBodyException(RequestBodyErrorMessage, jsonSerializationException);
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    DuplicateDatasetService service = new DuplicateDatasetService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        ExecuteCustomSearchResponse response = await service.DuplicateDatasetAsync(
                            datasetId,
                            newUserProject: null,
                            applyRowFilter,
                            applyUserSelection,
                            duplicateDatasetData,
                            duplicatePostProcess: false,
                            needsPostProcessExecution: false,
                            dbContext);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
