namespace CustomSearchesFunctions.Gis.Functions
{
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
    /// Azure function class for the GetUserMap service.  The service gets the user map.
    /// </summary>
    public class GetUserMap
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
        /// Initializes a new instance of the <see cref="GetUserMap"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetUserMap(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the user map.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/GIS/GetUserMap/{userMapId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userMapId" cref="int" in="path">The user map id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The user map.
        /// </returns>
        [FunctionName("GetUserMap")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/GIS/GetUserMap/{userMapId}")] HttpRequest req, int userMapId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    GetUserMapService service = new GetUserMapService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        GetUserMapResponse response = await service.GetUserMap(dbContext, userMapId);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
