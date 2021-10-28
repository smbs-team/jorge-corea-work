namespace CustomSearchesFunctions.Gis.Functions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesFunctions.Exception;
    using CustomSearchesServicesLibrary.Gis.Services;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the DeleteUserMap service.  The service deletes the user map.
    /// </summary>
    public class DeleteUserMap
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
        /// Initializes a new instance of the <see cref="DeleteUserMap"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public DeleteUserMap(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Deletes the user map.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/v2/API/GIS/DeleteUserMap/{userMapId}</url>
        /// <param name="req">The request.</param>
        /// <param name="userMapId" cref="int" in="path">The user map id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [FunctionName("DeleteUserMap")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "API/GIS/DeleteUserMap/{userMapId}")] HttpRequest req, int userMapId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    DeleteUserMapService service = new DeleteUserMapService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        await service.DeleteUserMap(dbContext, userMapId);
                        return new OkResult();
                    }
                },
                req,
                log);
        }
    }
}
