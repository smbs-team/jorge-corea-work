namespace PTASMapTileServicesLibrary.TileFeatureDataProvider
{
    using System.Net.Http;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.MapServer;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Class that creates tile feature data providers.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.TileProvider.ITileProviderFactory" />
    public class TileFeatureDataProviderFactory : ITileFeatureDataProviderFactory
    {
        /// <summary>
        /// Creates the SQL Server tile feature data provider.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server DB context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <returns>A newly created SQL Server tile feature data provider.</returns>
        public ITileFeatureDataProvider CreateSqlServerTileFeatureDataProvider(
            IFactory<TileFeatureDataDbContext> dbContextFactory,
            ILogger logger,
            TelemetryClient telemetryClient)
        {
            return new SqlServerFeatureDataProvider(dbContextFactory, logger, telemetryClient);
        }

        /// <summary>
        /// Creates the BLOB tile feature data provider.
        /// </summary>
        /// <param name="cloudStorageProvider">The cloud storage provider.</param>
        /// <param name="blobTileConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="fallbackProvider">The fallback provider.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// A newly created BLOB tile feature data provider.
        /// </returns>
        public ITileFeatureDataProvider CreateBlobTileFeatureDataProvider(
            ICloudStorageProvider cloudStorageProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            ITileFeatureDataProvider fallbackProvider,
            ILogger logger)
        {
            return new BlobFeatureDataProvider(cloudStorageProvider, blobTileConfigurationProvider, fallbackProvider, logger);
        }
    }
}
