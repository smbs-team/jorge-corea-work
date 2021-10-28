namespace PTASServicesCommon.CloudStorage
{
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;

    /// <summary>
    /// Interface that defines the contract to provide Cloud Storage containers.
    /// </summary>
    public interface ICloudStorageProvider
    {
        /// <summary>
        /// Gets the cloud storage account.
        /// </summary>
        /// <returns>A storage account.</returns>
        CloudStorageAccount GetCloudStorageAccount();

        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>A blob container.</returns>
        Task<CloudBlobContainer> GetCloudBlobContainer(string containerName);

        /// <summary>
        /// Gets the cloud BLOB client.
        /// </summary>
        /// <returns>The cloud blob client.</returns>
        Task<CloudBlobClient> GetCloudBlobClient();

        /// <summary>
        /// Gets the cloud FILE client.
        /// </summary>
        /// <returns>The file client.</returns>
        Task<CloudFileClient> GetCloudFileClient();

        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>The blob container.</returns>
        Task<CloudFileShare> GetCloudFileContainer(string containerName);
    }
}