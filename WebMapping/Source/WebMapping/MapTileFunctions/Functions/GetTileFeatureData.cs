namespace PTASMapTileFunctions.Functions
{
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PTASMapTileFunctions.Exception;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Azure function class for the GetTileFeatureData service.  This service returns data related to tile features.
    /// </summary>
    public class GetTileFeatureData
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
        /// The cloud storage configuration provider.
        /// </summary>
        private readonly ICloudStorageConfigurationProvider cloudStorageConfigurationProvider;

        /// <summary>
        /// The BLOB tile configuration provider.
        /// </summary>
        private readonly IBlobTileConfigurationProvider blobTileConfigurationProvider;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly IServiceTokenProvider tokenProvider;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTileFeatureData" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="tileFeatureDataProviderFactory">The tile feature data provider factory.</param>
        /// <param name="cloudStorageConfigurationProvider">The cloud storage configuration provider.</param>
        /// <param name="blobTileConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">If sqlServerConfigurationProvider/tileFeatureDataProviderFactory parameter is null.</exception>
        public GetTileFeatureData(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            ITileFeatureDataProviderFactory tileFeatureDataProviderFactory,
            ICloudStorageConfigurationProvider cloudStorageConfigurationProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            IServiceTokenProvider tokenProvider,
            TelemetryClient telemetryClient)
        {
            if (tileFeatureDataProviderFactory == null)
            {
                throw new System.ArgumentNullException(nameof(tileFeatureDataProviderFactory));
            }

            if (dbContextFactory == null)
            {
                throw new System.ArgumentNullException(nameof(dbContextFactory));
            }

            if (cloudStorageConfigurationProvider == null)
            {
                throw new System.ArgumentNullException(nameof(cloudStorageConfigurationProvider));
            }

            if (blobTileConfigurationProvider == null)
            {
                throw new System.ArgumentNullException(nameof(blobTileConfigurationProvider));
            }

            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            this.tileFeatureDataProviderFactory = tileFeatureDataProviderFactory;
            this.dbContextFactory = dbContextFactory;
            this.cloudStorageConfigurationProvider = cloudStorageConfigurationProvider;
            this.blobTileConfigurationProvider = blobTileConfigurationProvider;
            this.tokenProvider = tokenProvider;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Runs the GetTileFeatureData function.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="z">The z.</param>
        /// <param name="minX">The minX coordinate in EPSG4326.</param>
        /// <param name="minY">The minY coordinate in EPSG4326.</param>
        /// <param name="maxX">The maxX coordinate in EPSG4326.</param>
        /// <param name="maxY">The maxY coordinate in EPSG4326.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        /// A JSON response containing the requested feature data.
        /// </returns>
        [FunctionName("GetTileFeatureData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tileFeatureData/{layerId}/{z}/{minX}/{minY}/{maxX}/{maxY}")] HttpRequest req,
            string layerId,
            int z,
            int minX,
            int minY,
            int maxX,
            int maxY,
            ILogger log)
        {
            string columnsParameter = req.Query != null ? (string)req.Query["select"] : null;
            string[] selectColumns = columnsParameter?.Split(',');
            string datasetIdParameter = req.Query != null ? (string)req.Query["datasetId"] : null;
            string filterDatasetIdParameter = req.Query != null ? (string)req.Query["filterDatasetId"] : null;

            ITileFeatureDataProvider fallbackProvider = this.GetSqlTileFeatureDataProvider(log);
            ITileFeatureDataProvider tileFeatureDataProvider = this.GetBlobTileFeatureDataProvider(fallbackProvider, log);

            try
            {
                TileExtent tileExtent = new TileExtent(minX, minY, maxX, maxY, z);
                object featureData = await tileFeatureDataProvider.GetTileFeatureData(
                    tileExtent,
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

        /// <summary>
        /// Gets the blob storage tile feature data provider.
        /// </summary>
        /// <param name="fallbackTileProvider">The fall-back tile provider.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A blob storage tile provider.</returns>
        protected virtual ITileFeatureDataProvider GetBlobTileFeatureDataProvider(ITileFeatureDataProvider fallbackTileProvider, ILogger logger)
        {
            return this.tileFeatureDataProviderFactory.CreateBlobTileFeatureDataProvider(
                this.GetCloudStorageProvider(),
                this.blobTileConfigurationProvider,
                fallbackTileProvider,
                logger);
        }

        /// <summary>
        /// Gets the cloud storage provider.
        /// </summary>
        /// <returns>An cloud storage provider.</returns>
        private ICloudStorageProvider GetCloudStorageProvider()
        {
            return new CloudStorageProvider(this.cloudStorageConfigurationProvider, this.tokenProvider);
        }
    }
}
