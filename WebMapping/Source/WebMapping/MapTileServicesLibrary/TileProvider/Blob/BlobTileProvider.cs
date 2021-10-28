namespace PTASMapTileServicesLibrary.TileProvider.Blob
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.Telemetry;

    /// <summary>
    /// Provides tiles served from blob storage.  If the tile is not found in the blob storage, BlobTileProvider supports
    /// a fall-back provider where the tile can be retrieved (and then cached to the blob storage).
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.Providers.ITileProvider" />
    public class BlobTileProvider : ITileProvider
    {
        /// <summary>
        /// The connection string parse error message.
        /// </summary>
        private const string ConnectionStringParseErrorMessage = "Error parsing storage connection string";

        /// <summary>
        /// The BLOB upload error message.
        /// </summary>
        private const string BlobUploadErrorMessage = "Error trying to upload tile to Blob.  Exception: ";

        /// <summary>
        /// The logger interface.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Provides information about blob storage configuration.
        /// </summary>
        private readonly IBlobTileConfigurationProvider tileBlobConfigurationProvider;

        /// <summary>
        /// The fall-back tile provider where tiles will be looked for if the blob tile does not find the tile.
        /// </summary>
        private readonly ITileProvider fallbackTileProvider;

        /// <summary>
        /// The provider for cloud storage containers.
        /// </summary>
        private readonly ICloudStorageProvider cloudStorageProvider;

        /// <summary>
        /// Type of the output for the generated tile.
        /// </summary>
        private readonly TileOutputType outputType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobTileProvider" /> class.
        /// </summary>
        /// <param name="fallbackTileProvider">Provider where tiles will be looked for if the blob tile does not find the tile.</param>
        /// <param name="tileBlobConfigurationProvider">The blob configuration provider.</param>
        /// <param name="outputType">Type of the output for the generated tile.</param>
        /// <param name="cloudStorageProvider">The azure storage provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <exception cref="System.ArgumentNullException">When fallbackTileProvider/blobConfigurationProvide/cloudStorageProvider/logger parameter is null.</exception>
        public BlobTileProvider(
            ITileProvider fallbackTileProvider,
            IBlobTileConfigurationProvider tileBlobConfigurationProvider,
            TileOutputType outputType,
            ICloudStorageProvider cloudStorageProvider,
            ILogger logger,
            TelemetryClient telemetryClient = null)
        {
            if (tileBlobConfigurationProvider == null)
            {
                throw new ArgumentNullException(nameof(tileBlobConfigurationProvider));
            }

            if (cloudStorageProvider == null)
            {
                throw new ArgumentNullException(nameof(cloudStorageProvider));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.tileBlobConfigurationProvider = tileBlobConfigurationProvider;
            this.fallbackTileProvider = fallbackTileProvider;
            this.cloudStorageProvider = cloudStorageProvider;
            this.logger = logger;
            this.telemetryClient = telemetryClient;
            this.outputType = outputType;
        }

        /// <summary>
        /// Get the bytes for a tile from blob storage or fall-back.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layerId">The layerId of the tile.</param>
        /// <returns>An array of bytes with the specified tile.  NULL if the tile was not found.</returns>
        /// <exception cref="TileProviderException">Thrown in case the blob storage can't be contacted or if the fall-provider fails.</exception>
        public async Task<byte[]> GetTile(int x, int y, int z, string layerId)
        {
            string tilePath = this.ResolveTilePath(x, y, z, layerId);

            try
            {
                byte[] tileBytes = null;
                bool saveTileToBlob = false;

                CloudBlobContainer container = null;
                try
                {
                    await TelemetryHelper.TrackPerformanceAsync(
                        "BlobTileProvider_GetContainer",
                        async () =>
                        {
                            container = await this.cloudStorageProvider.GetCloudBlobContainer(this.tileBlobConfigurationProvider.TileContainerName);
                            return null;
                        },
                        this.telemetryClient);
                }
                catch (System.FormatException formatException)
                {
                    string error = string.Format(ConnectionStringParseErrorMessage);
                    throw new TileProviderException(TileProviderExceptionCategory.InvalidConfiguration, this.GetType(), error, formatException);
                }

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(tilePath);

                if (await blockBlob.ExistsAsync())
                {
                    System.IO.MemoryStream tileStream = new MemoryStream();
                    await TelemetryHelper.TrackPerformanceAsync(
                        "BlobTileProvider_DownloadBlob",
                        async () =>
                        {
                            await blockBlob.DownloadToStreamAsync(tileStream);

                            tileStream.Position = 0;
                            tileBytes = new byte[tileStream.Length];
                            await tileStream.ReadAsync(tileBytes, 0, (int)tileStream.Length);

                            var metrics = new Dictionary<string, double>()
                            {
                                { "BlobTileProvider_DownloadBlob:Length", (double)tileStream.Length }
                            };

                            return metrics;
                        },
                        this.telemetryClient);
                }
                else if (this.fallbackTileProvider != null)
                {
                    tileBytes = await this.fallbackTileProvider.GetTile(x, y, z, layerId);
                    saveTileToBlob = true && tileBytes != null;
                }

                if (saveTileToBlob)
                {
                    try
                    {
                        await TelemetryHelper.TrackPerformanceAsync(
                            "BlobTileProvider_UploadBlob",
                            async () =>
                            {
                                await blockBlob.UploadFromByteArrayAsync(tileBytes, 0, tileBytes.Length);

                                var metrics = new Dictionary<string, double>()
                                {
                                    { "BlobTileProvider_UploadBlob:Length", (double)tileBytes.Length }
                                };

                                return metrics;
                            },
                            this.telemetryClient);
                    }
                    catch (StorageException ex)
                    {
                        // When we fail to upload to blob we just log the exception, but we don't re-throw since this is not a critical error.
                        this.logger.LogError(BlobUploadErrorMessage, ex);
                    }
                }

                return tileBytes;
            }
            catch (StorageException storageException)
            {
                string error = string.Format("Error trying to retrieve tile from Blob.  Can't connect to the path: {0}", tilePath);
                throw new TileProviderException(TileProviderExceptionCategory.CloudStorageError, this.GetType(), error, storageException);
            }
        }

        /// <summary>
        /// Resolves the tile URL.
        /// </summary>
        /// <param name="x">The x tile coordinate.</param>
        /// <param name="y">The y tile coordinate.</param>
        /// <param name="z">The z tile coordinate.</param>
        /// <param name="layerId">The layer id.</param>
        /// <returns>The URL where the tile can be retrieved.</returns>
        private string ResolveTilePath(int x, int y, int z, string layerId)
        {
            string fileExtension = this.outputType.ToString().ToLower();

            string resolvedMapName = string.Format(this.tileBlobConfigurationProvider.TilePathMask, layerId, z, y, x, fileExtension);
            return resolvedMapName;
        }
    }
}
