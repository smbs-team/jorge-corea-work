// <copyright file="ICloudStorageConfigurationProvider.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASServicesCommon.CloudStorage
{
    /// <summary>
    /// Interface that defines the contract for cloud storage configuration.
    /// </summary>
    public interface ICloudStorageConfigurationProvider
    {
        /// <summary>
        /// Gets the connection string to the cloud storage account.
        /// </summary>
        string StorageConnectionString { get; }
    }
}
