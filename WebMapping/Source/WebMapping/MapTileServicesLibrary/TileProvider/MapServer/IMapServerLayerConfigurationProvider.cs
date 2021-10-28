namespace PTASMapTileServicesLibrary.TileProvider.MapServer
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that defines the contract to provide configuration information about map server layers.
    /// </summary>
    public interface IMapServerLayerConfigurationProvider
    {
        /// <summary>
        /// Gets a value indicating whether this configuration is for a raster layer provider.
        /// </summary>
        bool IsRasterLayerProvider { get; }

        /// <summary>
        /// Gets the map server URL.
        /// </summary>
        string MapServerUrl { get; }

        /// <summary>
        /// Gets the MapServer map file for a layer identifier and the corresponding layers query
        /// that needs to be submitted to mapserver.
        /// </summary>
        /// <param name="layerId">The layer identifier.</param>
        /// <returns>The map name and the layers query.  Returns null if the layer is not valid.</returns>
        (string layersQuery, string mapFile) GetMapFileForLayerId(string layerId);
    }
}
