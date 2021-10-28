namespace CustomSearchesFunctions.CustomSearches.Functions.Projects
{
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Services.Projects;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Azure function class for the ApplyModel service.  The service applies the user project model.
    /// </summary>
    public class ApplyModel
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyModel"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public ApplyModel(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Applies the user project model.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/ApplyModel/{projectId}</url>
        /// <param name="req">The request.</param>
        /// <param name="projectId" cref="int" in="path">The user project id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <response code="200" cref="ExecuteCustomSearchResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("ApplyModel")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/CustomSearches/ApplyModel/{projectId}")] HttpRequest req, int projectId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    ApplyModelService service = new ApplyModelService(serviceContext);
                    using (CustomSearchesDbContext dbContext = serviceContext.DbContextFactory.Create())
                    {
                        CloudBlobContainer blobContainer = await serviceContext.CloudStorageProvider.GetCloudBlobContainer(GetDatasetFile.RScriptBlobContainerName, serviceContext.AppCredential);
                        var result = await service.QueueApplyModelAsync(projectId, major: null, minor: null, dbContext, blobContainer);
                        return new OkObjectResult(result);
                    }
                },
                req,
                log);
        }
    }
}
