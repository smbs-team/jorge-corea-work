namespace PTASMapTileServicesLibrary.TileProvider
{
    /// <summary>
    /// Different types output type for the map server layer.
    /// </summary>
    public enum TileOutputType
    {
        /// <summary>
        /// The output should be an MVT PBF file
        /// </summary>
        PBF = 0,

        /// <summary>
        /// The output should be a PNG file
        /// </summary>
        PNG = 1,

        /// <summary>
        /// The output should be a JSON file
        /// </summary>
        JSON = 2,

        /// <summary>
        /// The output should be a JPG file
        /// </summary>
        JPG = 3,
    }
}
