// <copyright file="ICloudStorageProvider.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASServicesCommon.CloudStorage
{
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Interface that defines the contract to provide Cloud Storage containers.
    /// </summary>
    public interface ICloudStorageProvider
    {
        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>A blob container.</returns>
        CloudBlobContainer GetCloudBlobContainer(string containerName);

        /// <summary>
        /// Gets a reference to a cloud table.
        /// </summary>
        /// <param name="tableName">name of the table.</param>
        /// <returns>refernce to the table if it exists.</returns>
        CloudTable GetTableRef(string tableName);
    }
}
