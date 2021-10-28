// <copyright file="CloudStorageProvider.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASServicesCommon.CloudStorage
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Class that provides instances of cloud storage containers.
    /// </summary>
    public class CloudStorageProvider : ICloudStorageProvider
    {
        /// <summary>
        /// The storage configuration provider.
        /// </summary>
        private readonly ICloudStorageConfigurationProvider storageConfigurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudStorageProvider" /> class.
        /// </summary>
        /// <param name="storageConfigurationProvider">The storage configuration provider.</param>
        /// <exception cref="System.ArgumentNullException">When storageConfigurationProvider is null.</exception>
        public CloudStorageProvider(ICloudStorageConfigurationProvider storageConfigurationProvider)
        {
            this.storageConfigurationProvider =
                storageConfigurationProvider ?? throw new System.ArgumentNullException(nameof(storageConfigurationProvider));
        }

        /// <inheritdoc/>
        public CloudTable GetTableRef(string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConfigurationProvider.StorageConnectionString);
            var tempVal = storageAccount.CreateCloudTableClient();
            CloudTable cloudTable = tempVal.GetTableReference(tableName);
            cloudTable.CreateIfNotExists();
            return cloudTable;
        }

        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>The blob container.</returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public CloudBlobContainer GetCloudBlobContainer(string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConfigurationProvider.StorageConnectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container;
        }
    }
}