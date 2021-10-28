namespace PTASMapTileFunctions.Functions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.GeoLocationProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetDatasetGeoLocation service.  This service returns feature coordinates for all parcels in a  dataset.
    /// </summary>
    public class GetDatasetGeoLocation
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
        /// Initializes a new instance of the <see cref="GetDatasetGeoLocation" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetDatasetGeoLocation(
            IFactory<TileFeatureDataDbContext> dbContextFactory)
        {
            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            this.dbContextFactory = dbContextFactory;
            this.geoLocationProvider = new GeoLocationProvider(dbContextFactory);
        }

        /// <summary>
        /// Runs the GetDatasetGeoLocation function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested feature data.
        /// </returns>
        [FunctionName("GetDatasetGeoLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "datasetGeoLocation/{datasetId}")] HttpRequest req,
            Guid datasetId,
            ILogger log)
        {
            try
            {
                string filterParameter = req.Query != null ? (string)req.Query["filter"] : null;

                object featureData = await this.geoLocationProvider.GetDatasetCoordinates(datasetId, filterParameter);
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
            catch (TileProviderException tileproviderException)
            {
                return MapTileFunctionsExceptionHandler.HandleTileProviderException(tileproviderException, req, log);
            }
            catch (TileFeatureDataProviderException tileFeatureDataProviderException)
            {
                return MapTileFunctionsExceptionHandler.HandleTileFeatureDataProviderException(tileFeatureDataProviderException, req, log);
            }
            catch (System.Exception ex)
            {
                return MapTileFunctionsExceptionHandler.HandleUntypedException(ex, req, log);
            }
        }
    }
}
