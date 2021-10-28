namespace PTASMapTileFunctions.Functions
{
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Azure function class for the GetTileFeatureData service.  This service returns data related to tile features.
    /// </summary>
    public class GetExtentFeatureData
    {
        /// <summary>
        /// The tile feature data provider factory.
        /// </summary>
        private readonly ITileFeatureDataProviderFactory tileFeatureDataProviderFactory;

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExtentFeatureData" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="tileFeatureDataProviderFactory">The tile feature data provider factory.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetExtentFeatureData(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            ITileFeatureDataProviderFactory tileFeatureDataProviderFactory,
            TelemetryClient telemetryClient = null)
        {
            if (tileFeatureDataProviderFactory == null)
            {
                throw new System.ArgumentNullException(nameof(tileFeatureDataProviderFactory));
            }

            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            this.tileFeatureDataProviderFactory = tileFeatureDataProviderFactory;
            this.dbContextFactory = dbContextFactory;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Runs the GetTileFeatureData function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="z">The z.</param>
        /// <param name="minX">The min x for the tile extent in EPSG4326.</param>
        /// <param name="minY">The min y for the tile extent in EPSG4326.</param>
        /// <param name="maxX">The max x for the tile extent in EPSG4326.</param>
        /// <param name="maxY">The max y for the tile extent in EPSG4326.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested feature data.
        /// </returns>
        [FunctionName("GetExtentFeatureData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "extentFeatureData/{layerId}/{z}/{minX}/{minY}/{maxX}/{maxY}")] HttpRequest req,
            string layerId,
            int z,
            double minX,
            double minY,
            double maxX,
            double maxY,
            ILogger log)
        {
            string columnsParameter = req.Query != null ? (string)req.Query["select"] : null;
            string[] selectColumns = columnsParameter?.Split(',');

            string datasetIdParameter = req.Query != null ? (string)req.Query["datasetId"] : null;
            string filterDatasetIdParameter = req.Query != null ? (string)req.Query["filterDatasetId"] : null;

            ITileFeatureDataProvider tileFeatureDataProvider = this.GetSqlTileFeatureDataProvider(log);

            try
            {
                object featureData = await tileFeatureDataProvider.GetTileFeatureData(
                    new Extent(minX, minY, maxX, maxY),
                    z,
                    layerId,
                    selectColumns,
                    datasetIdParameter,
                    filterDatasetIdParameter);

                return new JsonResult(featureData);
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

        /// <summary>
        /// Gets the SQL Server tile feature data provider.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <returns>A SQL Server feature data tile provider.</returns>
        public virtual ITileFeatureDataProvider GetSqlTileFeatureDataProvider(ILogger logger)
        {
            return this.tileFeatureDataProviderFactory.CreateSqlServerTileFeatureDataProvider(
                this.dbContextFactory,
                logger,
                this.telemetryClient);
        }
    }
}
