namespace PTASMapTileServicesLibrary.TileFeatureDataProvider
{
    using System.Threading.Tasks;
    using PTASMapTileServicesLibrary.Geography.Data;
    using PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model;

    /// <summary>
    /// Interface that defines methods to provide map tiles from different sources.
    /// </summary>
    public interface ITileFeatureDataProvider
    {
        /// <summary>
        /// Gets the feature data for an extent.
        /// </summary>
        /// <param name="extent">The extent in EPSG4326.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="layerId">The layer id.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset id for the dataset where the data is going to be gathered from.
        /// If NULL the default live dataset will be used.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>
        /// The tile data.
        /// </returns>
        Task<FeatureDataResponse> GetTileFeatureData(
            Extent extent,
            int z,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId);

        /// <summary>
        /// Gets the feature data for a tile.
        /// </summary>
        /// <param name="tileExtent">The tile extent.</param>
        /// <param name="layerId">The layer id.</param>
        /// <param name="columns">The columns to be returned.</param>
        /// <param name="datasetId">The dataset id for the dataset where the data is going to be gathered from.
        /// If NULL the default live dataset will be used.</param>
        /// <param name="filterDatasetId">The dataset identifier for a join operation.
        /// Results will be returned if they are both present in the dataset and filter dataset.</param>
        /// <returns>
        /// The tile data.
        /// </returns>
        Task<FeatureDataResponse> GetTileFeatureData(
            TileExtent tileExtent,
            string layerId,
            string[] columns,
            string datasetId,
            string filterDatasetId);
    }
}
