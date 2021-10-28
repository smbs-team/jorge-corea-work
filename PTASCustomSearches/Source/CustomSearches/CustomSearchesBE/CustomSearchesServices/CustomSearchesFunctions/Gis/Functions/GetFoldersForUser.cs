namespace CustomSearchesFunctions.Gis.Functions
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Gis.Services;
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
        private readonly IFactory<GisDbContext> dbContextFactory;

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
        public GetFoldersForUser(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// <group>GIS</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/GIS/GetFoldersForUser/{userId}/{folderItemType}</url>
        /// <param name="req">The request.</param>
        /// <param name="userId" cref="Guid" in="path">The user id.</param>
        /// <param name="folderItemType" cref="string" in="path">The folder item type.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The folder for user.
        /// </returns>
        [FunctionName("GetFoldersForUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/GIS/GetFoldersForUser/{userId}/{folderItemType}")] HttpRequest req, Guid userId, string folderItemType, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetFoldersForUserService service = new GetFoldersForUserService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetFoldersForUserResponse response = await service.GetFoldersForUser(dbContext, userId, folderItemType);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
