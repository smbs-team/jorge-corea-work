namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASMapTileServicesLibrary.TileProvider;
    using PTASMapTileServicesLibrary.TileProvider.Blob;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.CloudStorage;

    /// <summary>
    /// Provides for feature data associated to tiles from blob.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.Providers.ITileProvider" />
    public class BlobFeatureDataProvider : BaseTileFeatureProvider, ITileFeatureDataProvider
    {
        /// <summary>
        /// The BLOB layer identifier suffix.
        /// </summary>
        private const string BlobLayerIdSuffix = "_FeatureData";

        /// <summary>
        /// The connection string parse error message.
        /// </summary>
        private const string ConnectionStringParseErrorMessage = "Error parsing storage connection string";

        /// <summary>
        /// The BLOB upload error message.
        /// </summary>
        private const string BlobUploadErrorMessage = "Error trying to upload tile to Blob.  Exception: ";

        /// <summary>
        /// Provides blob storage.
        /// </summary>
        private readonly ICloudStorageProvider cloudStorageProvider;

        /// <summary>
        /// Provides information about blob storage configuration.
        /// </summary>
        private readonly IBlobTileConfigurationProvider tileBlobConfigurationProvider;

        /// <summary>
        /// Provides that works in cases where the blob provider can't server the request.
        /// </summary>
        private readonly ITileFeatureDataProvider fallbackProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobFeatureDataProvider" /> class.
        /// </summary>
        /// <param name="storageProvider">The db context factory.</param>
        /// <param name="tileBlobConfigurationProvider">The BLOB tile configuration provider.</param>
        /// <param name="fallbackProvider">The fallback provider.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">storageProvider or logger or fallbackProvider.</exception>
        public BlobFeatureDataProvider(
            ICloudStorageProvider storageProvider,
            IBlobTileConfigurationProvider tileBlobConfigurationProvider,
            ITileFeatureDataProvider fallbackProvider,
            ILogger logger)
            : base(logger)
        {
            if (storageProvider == null)
            {
                throw new ArgumentNullException(nameof(storageProvider));
            }

            if (tileBlobConfigurationProvider == null)
            {
                throw new ArgumentNullException(nameof(tileBlobConfigurationProvider));
            }

            if (fallbackProvider == null)
            {
                throw new ArgumentNullException(nameof(fallbackProvider));
            }

            this.fallbackProvider = fallbackProvider;
            this.cloudStorageProvider = storageProvider;
            this.tileBlobConfigurationProvider = tileBlobConfigurationProvider;
        }

        /// <summary>
        /// Gets the feature data for a tile.
        /// </summary>
        /// <param name="extent">The extent of the tile in EPSG4326.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layerId">The layer id.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset id for the dataset where the data is going to be gathered from.
        /// If NULL the default live dataset will be used.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>
        /// The tile feature data.
        /// </returns>
        public async Task<FeatureDataResponse> GetTileFeatureData(
            Extent extent,
            int z,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId)
        {
            return await this.fallbackProvider.GetTileFeatureData(extent, z, layerId, columns, datasetId, filterDatasetId);
        }

        /// <summary>
        /// Gets the feature data for a tile.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <param name="layerId">The layer id.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>
        /// The tile data.
        /// </returns>
        public async virtual Task<FeatureDataResponse> GetTileFeatureData(
            TileExtent tileExtent,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId)
        {
            if (columns == null && datasetId == null)
            {
                return await base.GetTileFeatureData(tileExtent, layerId, columns, datasetId, filterDatasetId);
            }
            else
            {
                return await this.fallbackProvider.GetTileFeatureData(tileExtent, layerId, columns, datasetId, filterDatasetId);
            }
        }

        /// <summary>
        /// Loads the tile data asynchronously.
        /// </summary>
        /// <param name="featureDataResponse">The feature data response.</param>
        /// <param name="location">The location.</param>
        /// <param name="collectionIndex">Index of the collection.</param>
        /// <param name="queryExtent">The query extent in EPSG4326.</param>
        /// <param name="layerId">The layer identifier.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset identifier.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>The task.</returns>
        protected override async Task LoadTileData(
            FeatureDataResponse featureDataResponse,
            TileLocation location,
            int collectionIndex,
            Extent queryExtent,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId)
        {
            string tilePath = this.ResolveTilePath(location.X, location.Y, location.Z, layerId);

            try
            {
                IEnumerable<dynamic> featuresData = null;
                bool saveTileToBlob = false;

                CloudBlobContainer container = null;
                try
                {
                    container = await this.cloudStorageProvider.GetCloudBlobContainer(this.tileBlobConfigurationProvider.TileContainerName);
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
                    await blockBlob.DownloadToStreamAsync(tileStream);

                    tileStream.Position = 0;
                    var binaryData = new byte[tileStream.Length];
                    byte[] uncompressedData = null;

                    await tileStream.ReadAsync(binaryData, 0, (int)tileStream.Length);

                    using (var compressedStream = new MemoryStream(binaryData))
                    using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        uncompressedData = resultStream.ToArray();
                    }

                    string jsonData = Encoding.Unicode.GetString(uncompressedData);
                    featuresData = (ParcelFeatureData[])JsonConvert.DeserializeObject(jsonData, typeof(ParcelFeatureData[]));
                }
                else
                {
                    FeatureDataResponse fallbackResponse = await this.fallbackProvider.GetTileFeatureData(queryExtent, location.Z, layerId, null, null, null);
                    featuresData = fallbackResponse.FeaturesDataCollections[0].FeaturesData;
                    saveTileToBlob = true;
                }

                if (saveTileToBlob)
                {
                    try
                    {
                        string serializedFeatures = JsonConvert.SerializeObject(featuresData);
                        var binaryData = Encoding.Unicode.GetBytes(serializedFeatures);

                        MemoryStream ms = new MemoryStream();
                        using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                        {
                            zip.Write(binaryData, 0, binaryData.Length);
                        }

                        var compressedData = ms.ToArray();

                        await blockBlob.UploadFromByteArrayAsync(compressedData, 0, compressedData.Length);
                    }
                    catch (StorageException ex)
                    {
                        // When we fail to upload to blob we just log the exception, but we don't re-throw since this is not a critical error.
                        this.Logger.LogError(BlobUploadErrorMessage, ex);
                    }
                }

                featureDataResponse.FeaturesDataCollections[collectionIndex].FeaturesData = featuresData;
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
            string fileExtension = TileOutputType.JSON.ToString();

            string resolvedMapName = string.Format(this.tileBlobConfigurationProvider.TilePathMask, layerId + BlobFeatureDataProvider.BlobLayerIdSuffix, z, y, x, fileExtension);
            return resolvedMapName;
        }
    }
}