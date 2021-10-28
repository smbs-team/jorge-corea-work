namespace CustomSearchesFunctions.CustomSearches.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetFoldersForUser service.  The service gets the folders for the user.
    /// </summary>
    public class GetFoldersForUser
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
        /// Initializes a new instance of the <see cref="GetFoldersForUser"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetFoldersForUser(IFactory<CustomSearchesDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the folders for the user.
        /// </summary>
        /// <group>CustomSearches</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/CustomSearches/GetDatasetFoldersForUser/{userId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userId" cref="Guid" in="path">The user id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The folder for user.
        /// </returns>
        /// <response code="200" cref="GetFoldersForUserResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="403" cref="ErrorResultModel">Not authorized request.</response>
        [FunctionName("GetDatasetFoldersForUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/CustomSearches/GetDatasetFoldersForUser/{userId}")] HttpRequest req, Guid userId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetFoldersForUserService service = new GetFoldersForUserService(serviceContext);
                    using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetFoldersForUserResponse response = await service.GetFoldersForUserAsync(dbContext, userId);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
