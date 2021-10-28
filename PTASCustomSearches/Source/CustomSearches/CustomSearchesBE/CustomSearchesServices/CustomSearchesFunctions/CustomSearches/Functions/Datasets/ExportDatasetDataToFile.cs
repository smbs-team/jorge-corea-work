namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Datasets;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Misc;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the ExportDatasetDataToFile service.  The service generates a CSV file for the dataset.
    /// </summary>
    public class ExportDatasetDataToFile
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
        /// Initializes a new instance of the <see cref="ExportDatasetDataToFile" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/serviceContextFactory parameter is null.</exception>
        public ExportDatasetDataToFile(
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
        /// Exports the dataset to a file.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ExportDatasetDataToFile/{datasetId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetId" cref="Guid" in="path">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <param name="format" cref="string" in="query">The format of the file: xlsx or csv.</param>
        /// <param name="usePostProcess" cref="bool" in="query">Value indicating whether the results should include the post process.</param>
        /// <param name="postProcessId" cref="int" in="query">Sets this value if usePostProcess is true and you want to use a specific post process.</param>
        /// <param name="includeSecondaryDatasets" cref="bool" in="query">Value indicating whether the export should include the secondary datasets.</param>
        /// <returns>
        /// The file content result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("ExportDatasetDataToFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/ExportDatasetDataToFile/{datasetId}")] HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool usePostProcess = true;
                    int? postProcessId = null;
                    bool includeSecondaryDatasets = true;

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ExportDatasetDataToFileService service = new ExportDatasetDataToFileService(serviceContext);
                    DatasetFileImportExportType format = DatasetFileImportExportType.XLSX;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("format") == true)
                        {
                            string formatParameter = req.Query["format"].ToString();
                            if (formatParameter.ToLower() == "csv")
                            {
                                format = DatasetFileImportExportType.CSV;
                                contentType = "text/csv";
                            }
                        }

                        if (req.Query.ContainsKey("usePostProcess") == true)
                        {
                            usePostProcess = bool.Parse(req.Query["usePostProcess"].ToString());
                        }

                        if (req.Query.ContainsKey("postProcessId") == true)
                        {
                            postProcessId = int.Parse(req.Query["postProcessId"].ToString());
                        }

                        if (req.Query.ContainsKey("includeSecondaryDatasets") == true)
                        {
                            includeSecondaryDatasets = bool.Parse(req.Query["includeSecondaryDatasets"].ToString());
                        }
                    }

                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        Ref<string> fileName = new Ref<string>();
                        var fileBytes = await service.ExportDatasetDataToFileAsync(
                            datasetId,
                            fileName,
                            format,
                            dbContext,
                            usePostProcess,
                            postProcessId,
                            includeSecondaryDatasets,
                            log);

                        return new FileContentResult(fileBytes, contentType)
                        {
                            FileDownloadName = fileName.Value
                        };
                    }
                },
                req,
                log);
        }
    }
}
