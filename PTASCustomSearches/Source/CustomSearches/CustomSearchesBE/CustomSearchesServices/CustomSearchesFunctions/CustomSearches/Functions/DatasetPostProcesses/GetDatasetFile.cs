namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetDatasetFile service.  The service gets the dataset file.
    /// </summary>
    public class GetDatasetFile
    {
        /// <summary>
        /// RScript blob container name.
        /// </summary>
        public const string RScriptBlobContainerName = "rscript";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The cloud storage provider.
        /// </summary>
        private readonly ICloudStorageProvider cloudStorageProvider;

        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetFile"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/cloudStorageProvider/serviceContextFactory parameter is null.</exception>
        public GetDatasetFile(IFactory<CustomSearchesDbContext> dbContextFactory, ICloudStorageProvider cloudStorageProvider, IServiceContextFactory serviceContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (cloudStorageProvider == null)
            {
                throw new System.ArgumentNullException(nameof(cloudStorageProvider));
            }

            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
            this.dbContextFactory = dbContextFactory;
            this.cloudStorageProvider = cloudStorageProvider;
        }

        /// <summary>
        /// Gets the dataset post process file.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetFile/{datasetPostProcessId}/{fileName}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetPostProcessId" cref="int" in="path">The dataset post process id.</param>
        /// <param name="fileName" cref="string" in="path">The file name.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The file content result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetDatasetFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetDatasetFile/{datasetPostProcessId}/{fileName}")] HttpRequest req, int datasetPostProcessId, string fileName, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    bool checkFileInResults = true;
                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("checkFileInResults") == true)
                        {
                            checkFileInResults = bool.Parse(req.Query["checkFileInResults"].ToString());
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetDatasetFileService service = new GetDatasetFileService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        CloudBlobContainer blobContainer = await this.cloudStorageProvider.GetCloudBlobContainer(GetDatasetFile.RScriptBlobContainerName, serviceContext.AppCredential);
                        var fileBytes = await service.GetDatasetFileAsync(datasetPostProcessId, fileName, dbContext, blobContainer, checkFileInResults);

                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
                        contentType ??= "application/octet-stream";

                        return new FileContentResult(fileBytes, contentType)
                        {
                            FileDownloadName = fileName
                        };
                    }
                },
                req,
                log);
        }
    }
}
