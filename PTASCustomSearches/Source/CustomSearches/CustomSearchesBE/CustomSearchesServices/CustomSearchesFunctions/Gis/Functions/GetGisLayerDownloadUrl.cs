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

    /// <summary>
    /// Azure function class for the GetGisLayerDownloadUrl service.  The service gets the GIS layer download URL.
    /// </summary>
    public class GetGisLayerDownloadUrl
    {
        /// <summary>
        /// The service context factory.
        /// </summary>
        private readonly IServiceContextFactory serviceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGisLayerDownloadUrl"/> class.
        /// </summary>
        /// <param name="serviceContextFactory">The service context factory.</param>
        /// <exception cref="System.ArgumentNullException">If serviceContextFactory parameter is null.</exception>
        public GetGisLayerDownloadUrl(IServiceContextFactory serviceContextFactory)
        {
            if (serviceContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(serviceContextFactory));
            }

            this.serviceContextFactory = serviceContextFactory;
        }

        /// <summary>
        /// Gets the GIS layer download URL.
        /// </summary>
        /// <group>GIS</group>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/v2/API/GIS/GetGisLayerDownloadUrl/{layerSourceId}</url>
        /// <param name="req">The request.</param>
        /// <param name="layerSourceId" cref="int" in="path">The layer source id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// The GIS layer download URL response.
        /// </returns>
        /// <response code="200" cref="GetGisLayerDownloadUrlResponse">The request succeeded.</response>
        /// <response code="400" cref="ErrorResultModel">Bad request.</response>
        /// <response code="404" cref="ErrorResultModel">Entity was not found.</response>
        [FunctionName("GetGisLayerDownloadUrl")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "API/GIS/GetGisLayerDownloadUrl/{layerSourceId}")] HttpRequest req, int layerSourceId, ILogger log)
        {
            return await CustomSearchesFunctionsExceptionHandler.GlobalExceptionHandler(
                async () =>
                {
                    IServiceContext serviceContext = await this.serviceContextFactory.CreateFromHttpRequestAsync(req, log);
                    var service = new GetGisLayerDownloadUrlService(serviceContext);
                    using (GisDbContext dbContext = serviceContext.GisDbContextFactory.Create())
                    {
                        var response = await service.GetGisLayerDownloadUrlAsync(dbContext, layerSourceId);
                        return new JsonResult(response);
                    }
                },
                req,
                log);
        }
    }
}
