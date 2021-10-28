namespace PTASMapTileServicesLibrary.TileProvider.Exception
{
    /// <summary>
    /// Different types of exceptions that tile provider can generate.
    /// </summary>
    public enum TileProviderExceptionCategory
    {
        /// <summary>
        /// There was an error sending the request
        /// </summary>
        HttpRequestSendError = 0,

        /// <summary>
        /// The tile provider returned a server error
        /// </summary>
        ServerError = 1,

        /// <summary>
        /// The tile provider configuration is invalid
        /// </summary>
        InvalidConfiguration = 2,

        /// <summary>
        /// There is an error with the cloud storage
        /// </summary>
        CloudStorageError = 3,
    }
}
