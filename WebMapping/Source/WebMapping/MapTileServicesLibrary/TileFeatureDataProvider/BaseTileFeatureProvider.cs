namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using GeoAPI.Geometries;
    using Microsoft.Extensions.Logging;
    using NetTopologySuite.Geometries;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.Geography.Tile;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.EntityFramework;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;
    using PTASMapTileServicesLibrary.TileProvider.Exception;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Provides for feature data associated to tiles from blob.
    /// </summary>
    /// <seealso cref="PTASMapTileServicesLibrary.Providers.ITileProvider" />
    public abstract class BaseTileFeatureProvider
    {
        /// <summary>
        /// The default SQL query zoom level.  All tiles will be transformed to this level before looking for the information in the DB.
        /// </summary>
        protected const int DefaultSqlQueryZoomLevel = 14;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTileFeatureProvider" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">storageProvider or logger or fallbackProvider.</exception>
        public BaseTileFeatureProvider(ILogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the logger Interface.
        /// </summary>
        protected ILogger Logger { get; }

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
            TileExtent defaultTileExtent = TileConversionHelper.ChangeZoomLevel(tileExtent, BaseTileFeatureProvider.DefaultSqlQueryZoomLevel);

            FeatureDataResponse featureDataResponse = new FeatureDataResponse();
            featureDataResponse.LayerId = layerId;
            TileLocation extentLength = TileConversionHelper.GetTileExtentSize(defaultTileExtent);

            if (extentLength.X > 0 && extentLength.Y > 0)
            {
                int arrayLength = extentLength.X * extentLength.Y;
                Task[] taskArray = new Task[arrayLength];
                featureDataResponse.FeaturesDataCollections = new FeatureDataCollection[arrayLength];

                defaultTileExtent.Sort();
                for (int x = defaultTileExtent.Min.X; x <= defaultTileExtent.Max.X; x++)
                {
                    for (int y = defaultTileExtent.Min.Y; y <= defaultTileExtent.Max.Y; y++)
                    {
                        TileExtent queryTileExtent = new TileExtent(x, y, x, y, BaseTileFeatureProvider.DefaultSqlQueryZoomLevel);
                        TileLocation location = new TileLocation(x, y, BaseTileFeatureProvider.DefaultSqlQueryZoomLevel);
                        Extent queryExtent = TileConversionHelper.ExtentFromTileExtent(queryTileExtent);
                        int collectionIndex = (x - defaultTileExtent.Min.X) + ((y - defaultTileExtent.Min.Y) * extentLength.X);
                        featureDataResponse.FeaturesDataCollections[collectionIndex] = new FeatureDataCollection();
                        featureDataResponse.FeaturesDataCollections[collectionIndex].Location =
                            new TileLocation(x, y, BaseTileFeatureProvider.DefaultSqlQueryZoomLevel);

                        taskArray[collectionIndex] = this.LoadTileData(
                            featureDataResponse,
                            location,
                            collectionIndex,
                            queryExtent,
                            layerId,
                            columns,
                            datasetId,
                            filterDatasetId);
                    }
                }

                await Task.WhenAll(taskArray);
            }

            return featureDataResponse;
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
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        protected abstract Task LoadTileData(
            FeatureDataResponse featureDataResponse,
            TileLocation location,
            int collectionIndex,
            Extent queryExtent,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId);
    }
}