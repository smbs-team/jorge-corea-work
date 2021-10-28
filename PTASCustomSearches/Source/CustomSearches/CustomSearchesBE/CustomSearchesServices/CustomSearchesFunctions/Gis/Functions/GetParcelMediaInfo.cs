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
    /// Azure function class for the GetParcelMediaInfo service.  The service gets media information for a parcel.
    /// </summary>
    public class GetParcelMediaInfo
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
        /// Initializes a new instance of the <see cref="GetParcelMediaInfo"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If dbContextFactory parameter is null.</exception>
        public GetParcelMediaInfo(IFactory<GisDbContext> dbContextFactory, IServiceContextFactory serviceContextFactory)
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
        /// Gets the media info for a parcel.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/GIS/GetParcelMediaInfo/{parcelId}</url>
        /// <param name="req">The request.</param>
        /// <param name="parcelId" cref="string" in="path">The parcel id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The parcel media info.
        /// </returns>
        [FunctionName("GetParcelMediaInfo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/GIS/GetParcelMediaInfo/{parcelId}")] HttpRequest req, string parcelId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetParcelMediaInfoService(serviceContext);
                    using (GisDbContext dbContext = this.dbContextFactory.Create())
                    {
                        var response = await service.GetParcelMediaInfoAsync(dbContext, parcelId);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
