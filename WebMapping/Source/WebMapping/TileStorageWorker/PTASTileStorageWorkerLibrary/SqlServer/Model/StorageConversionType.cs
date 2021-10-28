namespace PTASTileStorageWorkerLibrary.SqlServer.Model
{
    /// <summary>
    /// Type of storage conversion jobs.
    /// </summary>
    public enum StorageConversionType
    {
        /// <summary>
        /// The SQL server to GPKG job
        /// </summary>
        SqlServerToGpkg = 0,

        /// <summary>
        /// The GDB to GPKG job
        /// </summary>
        GdbToGpkg = 1,

        /// <summary>
        /// File is just copied.
        /// </summary>
        BlobFilePassthrough = 2,

        /// <summary>
        /// A conversion from SQL Server to SQL Server
        /// </summary>
        SqlServerToSqlServer = 3
    }
}
