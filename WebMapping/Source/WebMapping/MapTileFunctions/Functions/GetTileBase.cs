namespace PTASMapTileFunctions.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NetTopologySuite.Operation.Buffer;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.MapServer;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Base class for get tile azure functions.
    /// </summary>
    public class GetTileBase
    {
        /// <summary>
        /// The map server URL setting name.
        /// </summary>
        private const string MapServerUrlSettingName = "MapServerUrl";

        /// <summary>
        /// The cloud storage configuration provider.
        /// </summary>
        private readonly ICloudStorageConfigurationProvider cloudStorageConfigurationProvider;

        /// <summary>
        /// The BLOB tile configuration provider.
        /// </summary>
        private readonly IBlobTileConfigurationProvider blobTileConfigurationProvider;

        /// <summary>
        /// The tile provider factory.
        /// </summary>
        private readonly ITileProviderFactory tileProviderFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly IServiceTokenProvider tokenProvider;

        /// <summary>
        /// Type of the output for the generated tile.
        /// </summary>
        private readonly TileOutputType outputType;

        /// <summary>
        /// Type of the output for the generated tile.
        /// </summary>
        private readonly IFactory<TileFeatureDataDbContext> dbContextFactory;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTileBase" /> class.
        /// </summary>
        /// <param name="outputType">Type of the output for the generated tile.</param>
        /// <param name="cloudStorageConfigurationProvider">The cloud storage configuration provider.</param>
        /// <param name="blobTileConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="tileProviderFactory">The tile provider factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">If cloudStorageConfigurationProvider/blobTileConfigurationProvider/tileProviderFactory parameter is null.</exception>
        public GetTileBase(
            TileOutputType outputType,
            ICloudStorageConfigurationProvider cloudStorageConfigurationProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            ITileProviderFactory tileProviderFactory,
            IServiceTokenProvider tokenProvider,
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            TelemetryClient telemetryClient)
        {
            if (tileProviderFactory == null)
            {
                throw new System.ArgumentNullException(nameof(tileProviderFactory));
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

            this.outputType = outputType;
            this.cloudStorageConfigurationProvider = cloudStorageConfigurationProvider;
            this.blobTileConfigurationProvider = blobTileConfigurationProvider;
            this.tileProviderFactory = tileProviderFactory;
            this.tokenProvider = tokenProvider;
            this.dbContextFactory = dbContextFactory;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Gets the blob storage tile provider.
        /// </summary>
        /// <param name="fallbackTileProvider">The fall-back tile provider.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A blob storage tile provider.</returns>
        protected virtual ITileProvider GetBlobTileProvider(ITileProvider fallbackTileProvider, ILogger logger)
        {
            return this.tileProviderFactory.CreateBlobTileProvider(
                fallbackTileProvider,
                this.blobTileConfigurationProvider,
                this.outputType,
                this.GetCloudStorageProvider(),
                logger,
                this.telemetryClient);
        }

        /// <summary>
        /// Gets the map server tile provider.
        /// </summary>
        /// <param name="context">The execution context used to load configuration file.</param>
        /// <returns>
        /// A map server tile provider.
        /// </returns>
        protected virtual ITileProvider GetMapServerTileProvider(ExecutionContext context)
        {
            // In order to get the layers from the .config file, it is necessary to have the execution context.  It would be better to add
            // this to the startup configuration as a singleton.
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile("app.settings.json", optional: false, reloadOnChange: true)
                .Build();

            // MapServer provider configuration
            IConfigurationSection settingsSection = config.GetSection("Settings");
            string mapServerIp = config.GetValue<string>("MapServerIp");

            IMapServerLayerConfigurationProvider mapServerLayerConfigurationProvider = new MapServerLayerConfigurationProvider(
                settingsSection[GetTileBase.MapServerUrlSettingName],
                mapServerIp,
                this.dbContextFactory,
                this.outputType == TileOutputType.PNG);

            return this.tileProviderFactory.CreateMapServerTileProvider(
                mapServerLayerConfigurationProvider,
                this.outputType,
                this.telemetryClient);
        }

        /// <summary>
        /// Determines whether the layer is a pass through layer(fallback provider shouldn't be invoked).
        /// </summary>
        /// <param name="layerId">The layer identifier.</param>
        /// <returns>True if the layer is pass through (fallback provider shouldn't be invoked).</returns>
        protected bool IsPassThroughLayer(string layerId)
        {
            using (var dbContext = this.dbContextFactory.Create())
            {
                var layerSource =
                    (from ls in dbContext.LayerSource
                     where ls.GisLayerName.ToLower() == layerId.ToLower()
                     select ls).FirstOrDefault();

                if (layerSource != null)
                {
                    return layerSource.IsBlobPassThrough;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the BLOB tile configuration provider.
        /// </summary>
        /// <returns>The BLOB tile configuration provider.</returns>
        protected IBlobTileConfigurationProvider GetBlobTileConfigurationProvider()
        {
            return this.blobTileConfigurationProvider;
        }

        /// <summary>
        /// Gets the cloud storage provider.
        /// </summary>
        /// <returns>An cloud storage provider.</returns>
        protected ICloudStorageProvider GetCloudStorageProvider()
        {
            return new CloudStorageProvider(this.cloudStorageConfigurationProvider, this.tokenProvider);
        }
    }
}
