namespace PTASMapTileFunctions.Functions
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.GeoLocationProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Azure function class for the GetGeoLocation service.  This service returns feature coordinates based on feature data.
    /// </summary>
    public class GetGeoLocation
    {
        /// <summary>
        /// The geo location provider.
        /// </summary>
        private readonly GeoLocationProvider geoLocationProvider;

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly IServiceTokenProvider tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGeoLocation" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetGeoLocation(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            IServiceTokenProvider tokenProvider)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            this.dbContextFactory = dbContextFactory;
            this.tokenProvider = tokenProvider;
            this.geoLocationProvider = new GeoLocationProvider(dbContextFactory);
        }

        /// <summary>
        /// Runs the GetGeoLocation function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="pin">The parcel identifier.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested feature data.
        /// </returns>
        [FunctionName("GetGeoLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "geoLocation/{pin}")] HttpRequest req,
            string pin,
            ILogger log)
        {
            try
            {
                object featureData = await this.geoLocationProvider.GetParcelCoordinates(pin);
                if (featureData == null)
                {
                    return new NotFoundResult();
                }

                return new JsonResult(featureData);
            }
            catch (GeoLocationProviderException geoLocationProviderException)
            {
                return MapTileFunctionsExceptionHandler.HandleGeoLocationProviderException(geoLocationProviderException, req, log);
            }
            catch (System.Exception ex)
            {
                return MapTileFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
