// <copyright file="CloudService.cs" company="King County">
// Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASSketchFileMigratorConsoleApp
{
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Blob;
    using PTASServicesCommon.CloudStorage;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    ///   <para>Provides a method that uploads a file to azure storage.</para>
    /// </summary>
    /// <seealso cref="PTASSketchFileMigratorConsoleApp.ICloudService" />
    public class CloudService : ICloudService
    {
        private readonly ISettingsManager settings;
        private readonly ICloudStorageProvider cloudStorageProvider;
        private readonly IServiceTokenProvider tokenProvider;
        private readonly ICloudStorageConfigurationProvider storageConfigurationProvider;

        /// <summary>Initializes a new instance of the <see cref="CloudService"/> class.</summary>
        /// <param name="settings">The settings.</param>
        public CloudService(ISettingsManager settings)
        {
            this.settings = settings;
            this.storageConfigurationProvider = new CloudStorageConfigurationProvider(this.settings.ReadSetting("ConnectionString"));
            this.tokenProvider = new AzureTokenProvider();
            this.cloudStorageProvider = new CloudStorageProvider(this.storageConfigurationProvider, this.tokenProvider);
        }

        /// <summary>Uploads a file to azure blob container.</summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="path"> The path and name of the file.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadFile(byte[] file, string containerName, string path)
        {
            CloudBlobContainer container = await this.cloudStorageProvider.GetCloudBlobContainer(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);
            await blockBlob.UploadFromByteArrayAsync(file, 0, file.Length);
        }
    }
}