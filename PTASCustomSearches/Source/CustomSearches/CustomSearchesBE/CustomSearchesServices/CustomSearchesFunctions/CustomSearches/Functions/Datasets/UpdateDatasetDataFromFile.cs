namespace CustomSearchesFunctions.CustomSearches.Functions.RScriptModel
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the UpdateDatasetDataFromFile service.  The service updates the contents of a dataset using the contents of the file.
    /// </summary>
    public class UpdateDatasetDataFromFile
    {
        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetDataFromFile" /> class.
        /// May return Conflict(409) depending on dataset state.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/cloudStorageProvider parameter is null.</exception>
        public UpdateDatasetDataFromFile(
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
        /// Updates a dataset from the contents of a file.  Only editable columns are updated.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/UpdateDatasetDataFromFile/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="file" cref="string" format="binary" in="body">The rscript file.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <response code="200" cref="IdResult">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("UpdateDatasetDataFromFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/UpdateDatasetDataFromFile/{datasetId}")] HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    if (req.ContentLength == 0)
                    {
                        throw new ArgumentNullException("body");
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new UpdateDatasetDataFromFileService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var idResult = await service.QueueUpdateDatasetDataFromFileAsync(datasetId, req.Body, DatasetFileImportExportType.XLSX, dbContext);

                        return new OkObjectResult(idResult);
                    }
                },
                req,
                log);
        }
    }
}
