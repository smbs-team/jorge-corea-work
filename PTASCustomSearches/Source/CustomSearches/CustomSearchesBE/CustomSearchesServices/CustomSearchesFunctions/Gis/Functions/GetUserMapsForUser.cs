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
    /// Azure function class for the GetUserMap service.  The service gets the user maps for the user.
    /// </summary>
    public class GetUserMapsForUser
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
        /// Initializes a new instance of the <see cref="GetUserMapsForUser"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetUserMapsForUser(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the user maps for the user.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/GIS/GetUserMapsForUser/{userId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userId" cref="Guid" in="path">The user id.</param>
        /// <param name="log">The log.</param>
        /// <param name="folderType" cref="string" in="query">The folder type.</param>
        /// <returns>
        /// The user map.
        /// </returns>
        [FunctionName("GetUserMapsForUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/GIS/GetUserMapsForUser/{userId}")] HttpRequest req, Guid userId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    string folderType = null;

                    if (req.Query != null)
                    {
                        if (req.Query.ContainsKey("folderType") == true)
                        {
                            folderType = req.Query["folderType"].ToString();
                        }
                    }

                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetUserMapsForUserService service = new GetUserMapsForUserService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserMapsForUserResponse response = await service.GetUserMapsForUser(dbContext, userId, folderType);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
