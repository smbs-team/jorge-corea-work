namespace PTASServicesCommon.CloudStorage
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;
    using Microsoft.WindowsAzure.Storage.Table;
    using PTASServicesCommon.TokenProvider;

    /// <summary>
    /// Class that provides instances of cloud storage containers.
    /// </summary>
    public class CloudStorageProvider : ICloudStorageProvider
    {
        /// <summary>
        /// The endpoint to obtain tokens.
        /// </summary>
        private const string TokenEndpoint = "";

        /// <summary>
        /// The account key setting.
        /// </summary>
        private const string AccountKeySetting = "AccountKey";

        /// <summary>
        /// The storage configuration provider.
        /// </summary>
        private ICloudStorageConfigurationProvider storageConfigurationProvider;

        /// <summary>
        /// The token provider.
        /// </summary>
        private IServiceTokenProvider tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudStorageProvider" /> class.
        /// </summary>
        /// <param name="storageConfigurationProvider">The storage configuration provider.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <exception cref="System.ArgumentNullException">When storageConfigurationProvider is null.</exception>
        public CloudStorageProvider(ICloudStorageConfigurationProvider storageConfigurationProvider, IServiceTokenProvider tokenProvider)
        {
            if (storageConfigurationProvider == null)
            {
                throw new System.ArgumentNullException(nameof(storageConfigurationProvider));
            }

            if (tokenProvider == null)
            {
                throw new System.ArgumentNullException(nameof(tokenProvider));
            }

            this.tokenProvider = tokenProvider;
            this.storageConfigurationProvider = storageConfigurationProvider;
        }

        /// <summary>
        /// Gets the cloud storage account.
        /// </summary>
        /// <returns>The cloud storage account.</returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public CloudStorageAccount GetCloudStorageAccount()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConfigurationProvider.StorageConnectionString);
            return storageAccount;
        }

        /// <summary>
        /// Gets the cloud BLOB client.
        /// </summary>
        /// <param name="credentials">The client credentials in case this call is made with Managed Identity permissions.</param>
        /// <returns>The blob client.</returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public async Task<CloudBlobClient> GetCloudBlobClient(ClientCredential credentials = null)
        {
            string connectionString = this.storageConfigurationProvider.StorageConnectionString;
            if (connectionString.Contains(CloudStorageProvider.AccountKeySetting, StringComparison.OrdinalIgnoreCase))
            {
                CloudStorageAccount storageAccount;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    return storageAccount.CreateCloudBlobClient();
                }

                return null;
            }
            else
            {
                string tokenCredential = null;
                if (credentials == null)
                {
                    tokenCredential = await this.tokenProvider.GetAccessTokenAsync(AzureTokenProvider.StorageTokenEndpoint);
                }
                else
                {
                    tokenCredential = await this.tokenProvider.GetAccessTokenAsync(AzureTokenProvider.StorageTokenEndpoint, AzureTokenProvider.TenantId, credentials);
                }

                var storageCredentials = new StorageCredentials(new TokenCredential(tokenCredential));

                CloudBlobClient blobClient = new CloudBlobClient(new Uri(this.storageConfigurationProvider.StorageConnectionString), storageCredentials);
                return blobClient;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public async Task<CloudFileClient> GetCloudFileClient()
        {
            string connectionString = this.storageConfigurationProvider.StorageConnectionString;
            if (connectionString.Contains(CloudStorageProvider.AccountKeySetting, StringComparison.OrdinalIgnoreCase))
            {
                CloudStorageAccount storageAccount;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    return storageAccount.CreateCloudFileClient();
                }

                return null;
            }
            else
            {
                string tokenCredential = await this.tokenProvider.GetAccessTokenAsync(AzureTokenProvider.StorageTokenEndpoint);
                var storageCredentials = new StorageCredentials(new TokenCredential(tokenCredential));

                CloudFileClient fileClient = new CloudFileClient(new Uri(this.storageConfigurationProvider.StorageConnectionString), storageCredentials);
                return fileClient;
            }
        }

        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>The blob container.</returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public async Task<CloudFileShare> GetCloudFileContainer(string containerName)
        {
            var blobClient = await this.GetCloudFileClient();
            CloudFileShare container = blobClient.GetShareReference(containerName);
            return container;
        }

        /// <summary>
        /// Gets the cloud BLOB container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="credentials">The client credentials in case this call is made with Managed Identity permissions.</param>
        /// <returns>The blob container.</returns>
        /// <exception cref="System.FormatException">Thrown in case the connection string is invalid.</exception>
        public async Task<CloudBlobContainer> GetCloudBlobContainer(string containerName, ClientCredential credentials = null)
        {
            CloudBlobClient blobClient = await this.GetCloudBlobClient(credentials);
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            return container;
        }

        /// <summary>
        /// Gets the cloud table client.
        /// </summary>
        /// <returns>The table client.</returns>
        public async Task<CloudTableClient> GetCloudTableClient()
        {
            string connectionString = this.storageConfigurationProvider.StorageConnectionString;
            if (connectionString.Contains(CloudStorageProvider.AccountKeySetting, StringComparison.OrdinalIgnoreCase))
            {
                CloudStorageAccount storageAccount;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    return storageAccount.CreateCloudTableClient();
                }

                return null;
            }
            else
            {
                string tokenCredential = await this.tokenProvider.GetAccessTokenAsync(AzureTokenProvider.StorageTokenEndpoint);
                var storageCredentials = new StorageCredentials(new TokenCredential(tokenCredential));

                CloudTableClient blobClient = new CloudTableClient(
                    new Uri(this.storageConfigurationProvider.StorageConnectionString),
                    storageCredentials);
                return blobClient;
            }
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The cloud table.</returns>
        public async Task<CloudTable> GetCloudTable(string tableName)
        {
            CloudTableClient cloudTableClient = await this.GetCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference(tableName);
            return table;
        }
    }
}