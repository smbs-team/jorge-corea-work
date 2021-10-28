namespace PTASMapTileServicesLibrary.TileProvider
{
    using System.Net.Http;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.MapServer;
    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Class that creates tile providers.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.TileProvider.ITileProviderFactory" />
    public class TileProviderFactory : ITileProviderFactory
    {
        /// <summary>
        /// Creates the BLOB tile provider.
        /// </summary>
        /// <param name="fallbackTileProvider">The fall-back tile provider.</param>
        /// <param name="tileBlobConfigurationProvider">The tile BLOB configuration provider.</param>
        /// <param name="outputType">Type of the output for the generated tile.</param>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <returns>A newly created blob tile provider.</returns>
        public ITileProvider CreateBlobTileProvider(
            ITileProvider fallbackTileProvider,
            IBlobTileConfigurationProvider tileBlobConfigurationProvider,
            TileOutputType outputType,
            ICloudStorageProvider cloudStorageProvider,
            ILogger logger,
            TelemetryClient telemetryClient)
        {
            return new BlobTileProvider(fallbackTileProvider, tileBlobConfigurationProvider, outputType, cloudStorageProvider, logger, telemetryClient);
        }

        /// <summary>
        /// Creates the map server tile provider.
        /// </summary>
        /// <param name="mapServerLayerConfigurationProvider">The map server layer configuration provider.</param>
        /// <param name="outputType">Type of the output for the generated tile.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        /// <returns>A newly created map server tile provider.</returns>
        public ITileProvider CreateMapServerTileProvider(
            IMapServerLayerConfigurationProvider mapServerLayerConfigurationProvider,
            TileOutputType outputType,
            TelemetryClient telemetryClient,
            HttpMessageHandler httpMessageHandler = null)
        {
            return new MapServerTileProvider(mapServerLayerConfigurationProvider, outputType, telemetryClient, httpMessageHandler);
        }
    }
}
