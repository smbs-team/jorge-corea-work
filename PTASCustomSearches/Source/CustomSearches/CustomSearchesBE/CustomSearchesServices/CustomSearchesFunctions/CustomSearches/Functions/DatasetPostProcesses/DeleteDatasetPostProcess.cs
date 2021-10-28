namespace CustomSearchesFunctions.CustomSearches.Functions.InteractiveCharts
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Services.CustomSearches.InteractiveCharts;
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
    /// Azure function class for the DeleteDatasetPostProcess service.  The service deletes a dataset post process.
    /// </summary>
    public class DeleteDatasetPostProcess
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
        /// Initializes a new instance of the <see cref="DeleteDatasetPostProcess"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory/cloudStorageProvider/serviceContextFactory parameter is null.</exception>
        public DeleteDatasetPostProcess(
            IFactory<CustomSearchesDbContext> dbContextFactory,
            ICloudStorageProvider cloudStorageProvider,
            IServiceContextFactory serviceContextFactory)
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
        /// Deletes a dataset post process.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/DeleteDatasetPostProcess/{datasetPostProcessId}</url>
        /// <param name="req">The request.</param>
        /// <param name="datasetPostProcessId" cref="int" in="path">The dataset post process id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// Operation result.
        /// </returns>
        /// <response code="200">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        /// <response code="409" cref="ErrorResultModel">A conflict was found.</response>
        [FunctionName("DeleteDatasetPostProcess")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/DeleteDatasetPostProcess/{datasetPostProcessId}")] HttpRequest req, int datasetPostProcessId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    DeleteDatasetPostProcessService service = new DeleteDatasetPostProcessService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        CloudBlobContainer blobContainer = await this.cloudStorageProvider.GetCloudBlobContainer(GetDatasetFile.RScriptBlobContainerName, serviceContext.AppCredential);
                        await service.DeletePostProcessAndSecondariesWithLockAsync(datasetPostProcessId, dbContext, blobContainer, log);
                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
