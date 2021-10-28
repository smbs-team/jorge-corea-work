namespace PTASMapTileServicesLibrary.TileFeatureDataProvider
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.Extensions.Logging;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Interface that defines a contract for tile feature data provider creation.
    /// </summary>
    public interface ITileFeatureDataProviderFactory
    {
        /// <summary>
        /// Creates the SQL Server tile feature data provider.
        /// </summary>
        /// <param name="dbContextFactorty">The SQL Server DB context factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <returns>A newly created SQL Server tile feature data provider.</returns>
        ITileFeatureDataProvider CreateSqlServerTileFeatureDataProvider(
            IFactory<TileFeatureDataDbContext> dbContextFactorty,
            ILogger logger,
            TelemetryClient telemetryClient = null);

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
        ITileFeatureDataProvider CreateBlobTileFeatureDataProvider(
            ICloudStorageProvider cloudStorageProvider,
            IBlobTileConfigurationProvider blobTileConfigurationProvider,
            ITileFeatureDataProvider fallbackProvider,
            ILogger logger);
    }
}
