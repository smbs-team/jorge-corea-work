namespace PTASMapTileServicesLibrary.TileProvider.Exception
{
    /// <summary>
    /// Different types of exceptions that tile feature data provider can generate.
    /// </summary>
    public enum TileFeatureDataProviderExceptionCategory
    {
        /// <summary>
        /// There was an error reading data from SQL Server.
        /// </summary>
        SqlServerError = 0,

        /// <summary>
        /// The provided dataset was invalid.
        /// </summary>
        DatasetNotFound = 1
    }
}
