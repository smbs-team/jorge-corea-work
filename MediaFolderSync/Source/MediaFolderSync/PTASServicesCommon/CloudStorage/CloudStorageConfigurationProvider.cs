namespace PTASServicesCommon.CloudStorage
{
    /// <summary>
    /// Cloud storage configuration provider.
    /// </summary>
    /// <seealso cref="PTASServicesCommon.CloudStorage.ICloudStorageConfigurationProvider" />
    public class CloudStorageConfigurationProvider : ICloudStorageConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudStorageConfigurationProvider" /> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <exception cref="System.ArgumentNullException">When storageConnectionString is null.</exception>
        public CloudStorageConfigurationProvider(string storageConnectionString)
        {
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new System.ArgumentNullException(nameof(storageConnectionString));
            }

            this.StorageConnectionString = storageConnectionString;
        }

        /// <summary>
        /// Gets the connection string to the cloud storage account.
        /// </summary>
        public string StorageConnectionString { get; }
    }
}
