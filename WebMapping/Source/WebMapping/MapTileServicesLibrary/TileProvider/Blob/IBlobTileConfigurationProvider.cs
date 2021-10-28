namespace PTASMapTileServicesLibrary.TileProvider.Blob
{
    /// <summary>
    /// Interface that defines the configuration contract for a blob tile provider.
    /// </summary>
    public interface IBlobTileConfigurationProvider
    {
        /// <summary>
        /// Gets the path mask for a tile in azure blob storage.
        /// </summary>
        string TilePathMask { get; }

        /// <summary>
        /// Gets the name of the tile container.
        /// </summary>
        string TileContainerName { get; }
    }
}
