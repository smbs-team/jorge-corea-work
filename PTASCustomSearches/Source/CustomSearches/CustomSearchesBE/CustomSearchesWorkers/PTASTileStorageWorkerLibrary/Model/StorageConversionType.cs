namespace PTASTileStorageWorkerLibrary.Model
{
    /// <summary>
    /// Type of storage conversion jobs.
    /// </summary>
    public enum StorageConversionType
    {
        /// <summary>
        /// The SQL server to file job
        /// </summary>
        SqlServerToFile = 0,

        /// <summary>
        /// The File to File job
        /// </summary>
        FileToFile = 1,

        /// <summary>
        /// File is just copied.
        /// </summary>
        FilePassthrough = 2,

        /// <summary>
        /// A conversion from SQL Server to SQL Server
        /// </summary>
        SqlServerToSqlServer = 3,

        /// <summary>
        /// Convert ArcGis Pre-Tiled Vector Tile layer style and copy tiles to tile cache.
        /// </summary>
        ArcGisPreTiledLayerPassthrough = 4,

        /// <summary>
        /// A conversion from Fle to SQL Server
        /// </summary>
        FileToSqlServer = 5,
    }
}
