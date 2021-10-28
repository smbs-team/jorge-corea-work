namespace PTASServicesCommon.CloudStorage
{
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;
    using Microsoft.WindowsAzure.Storage.Table;

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
        /// <param name="credentials">The client credentials in case this call is made with Managed Identity permissions.</param>
        /// <returns>A blob container.</returns>
        Task<CloudBlobContainer> GetCloudBlobContainer(string containerName, ClientCredential credentials = null);

        /// <summary>
        /// Gets the cloud BLOB client.
        /// </summary>
        /// <param name="credentials">The client credentials in case this call is made with Managed Identity permissions.</param>
        /// <returns>The cloud blob client.</returns>
        Task<CloudBlobClient> GetCloudBlobClient(ClientCredential credentials = null);

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

        /// <summary>
        /// Gets the cloud table client.
        /// </summary>
        /// <returns>
        /// The table client.
        /// </returns>
        Task<CloudTableClient> GetCloudTableClient();

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The cloud table.</returns>
        Task<CloudTable> GetCloudTable(string tableName);
    }
}