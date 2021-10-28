namespace PTASMapTileFunctions.Exception
{
    /// <summary>
    /// Class containing the different error messages returned by the tile functions.
    /// </summary>
    public static class MapTileFunctionsErrorMessages
    {
        /// <summary>
        /// The unhandled exception error.
        /// </summary>
        public const string UnhandledExceptionError = "An unknown error has happened in the back-end.";

        /// <summary>
        /// The tile feature data sql exception error.
        /// </summary>
        public const string TileFeatureDataSqlException = "An error has happened while accessing tile feature data.";

        /// <summary>
        /// The dataset not found exception error.
        /// </summary>
        public const string DatasetNotFoundExceptionError = "The dataset was not found.";
    }
}
