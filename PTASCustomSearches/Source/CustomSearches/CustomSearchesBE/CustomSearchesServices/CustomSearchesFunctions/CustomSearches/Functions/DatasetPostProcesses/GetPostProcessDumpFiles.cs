namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetPostProcessDumpFiles service.  The service gets the list of files in the blob for a post-process.
    /// </summary>
    public class GetPostProcessDumpFiles
    {
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
        /// Initializes a new instance of the <see cref="GetPostProcessDumpFiles"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/cloudStorageProvider/serviceContextFactory parameter is null.</exception>
        public GetPostProcessDumpFiles(IFactory<CustomSearchesDbContext> dbContextFactory, ICloudStorageProvider cloudStorageProvider, IServiceContextFactory serviceContextFactory)
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
        /// Gets the list of dump files for a failed post-process.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetPostProcessDumpFiles/{datasetPostProcessId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetPostProcessId" cref="int" in="path">The dataset post process id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The file content result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetPostProcessDumpFiles")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetPostProcessDumpFiles/{datasetPostProcessId}")] HttpRequest req, int datasetPostProcessId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetPostProcessDumpFilesService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        CloudBlobContainer blobContainer = await this.cloudStorageProvider.GetCloudBlobContainer(GetDatasetFile.RScriptBlobContainerName, serviceContext.AppCredential);
                        var files = await service.GetPostProcessDumpFilesAsync(datasetPostProcessId, dbContext, blobContainer);
                        return new ObjectResult(files);
                    }
                },
                req,
                log);
        }
    }
}
