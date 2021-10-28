namespace PTASMediaHelperClasses
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Auth;
    using Microsoft.Azure.Storage.File;

    /// <summary>
    /// Helper class to handle azure blobs and files.
    /// </summary>
    public class AzureStorageHelper
    {
        private CloudStorageAccount storageAccount;
        private CloudFileClient fileClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageHelper"/> class.
        /// Constructor for class.
        /// </summary>
        /// <param name="connectionStr">Received connection string.</param>
        public AzureStorageHelper(string connectionStr)
        {
            this.ConnectionStr = connectionStr;
        }

        /// <summary>
        /// Gets or sets local connection string copy.
        /// </summary>
        private string ConnectionStr
        {
            get; set;
        }

        /// <summary>
        /// Get a CloudFile instance with the specified name in the given share.
        /// </summary>
        /// <param name="shareName">Share name.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>A <see cref="Task{T}"/> object of type <see cref="CloudFile"/> that represents the asynchronous operation.</returns>
        public async Task<CloudFile> GetCloudFileAsync(string shareName, string fileName)
        {
            string path = SeparateFileAndPath(ref fileName);

            CloudFileClient client = this.GetCloudFileClient();
            CloudFileShare share = client.GetShareReference(shareName);
            await share.CreateIfNotExistsAsync();

            // change by WR to create directory if not exist.
            CloudFileDirectory rootDirectory = share.GetRootDirectoryReference();
            var currDirectory = rootDirectory;
            var parts = path.Split('/');
            foreach (string currPart in parts)
            {
                var r = currDirectory.GetDirectoryReference(currPart);
                await r.CreateIfNotExistsAsync();
                currDirectory = r;
            }

            return currDirectory.GetFileReference(fileName);
        }

        /// <summary>
        /// Checks if a file exists.
        /// </summary>
        /// <param name="shareName">Share name.</param>
        /// <param name="relativePath">Path to the item.</param>
        /// <returns>true if exists.</returns>
        public async Task<bool> FileExists(string shareName, string relativePath)
        {
            CloudFile cloudFile = await this.GetCloudFileAsync(shareName, relativePath);
            var exists = await cloudFile.ExistsAsync();
            return exists;
        }

        /// <summary>
        /// Gets the current cloud storage account.
        /// </summary>
        /// <returns>The account retrieved.</returns>
        public CloudStorageAccount GetStorageAccount()
        {
            return this.storageAccount ?? (this.storageAccount = CloudStorageAccount.Parse(this.ConnectionStr));
        }

        /// <summary>
        /// Gets a cloud from the cloud.
        /// </summary>
        /// <returns>The fetched file.</returns>
        public CloudFileClient GetCloudFileClient()
        {
            if (this.ConnectionStr.Contains("AccountKey"))
            {
                return this.fileClient ?? (this.fileClient = this.GetStorageAccount().CreateCloudFileClient());
            }

            string tokenCredential = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkhsQzBSMTJza3hOWjFXUXdtak9GXzZ0X3RERSIsImtpZCI6IkhsQzBSMTJza3hOWjFXUXdtak9GXzZ0X3RERSJ9.eyJhdWQiOiJodHRwczovL3B0YXNzdG9yYWdlLmZpbGUuY29yZS53aW5kb3dzLm5ldCIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0L2JhZTUwNTlhLTc2ZjAtNDlkNy05OTk2LTcyZGZlOTVkNjljNy8iLCJpYXQiOjE1ODA0OTkwNjgsIm5iZiI6MTU4MDQ5OTA2OCwiZXhwIjoxNTgwNTAyOTY4LCJhaW8iOiI0Mk5nWUpnZmNXV2F3K3hWVEVxL0MyZHFOcDY5Q0FBPSIsImFwcGlkIjoiNDA0YjliOGItNTMxNS00ZWYzLTg1OWQtMzczNTJhMTJiMmRhIiwiYXBwaWRhY3IiOiIxIiwiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvYmFlNTA1OWEtNzZmMC00OWQ3LTk5OTYtNzJkZmU5NWQ2OWM3LyIsIm9pZCI6ImU2NDA3MjhjLTQ3YzctNDM5MC05MGM1LWQyNTc3ZmY2ZDI4NiIsInN1YiI6ImU2NDA3MjhjLTQ3YzctNDM5MC05MGM1LWQyNTc3ZmY2ZDI4NiIsInRpZCI6ImJhZTUwNTlhLTc2ZjAtNDlkNy05OTk2LTcyZGZlOTVkNjljNyIsInV0aSI6IkVvdmV4cUVaNkV5SG1ENEdySzRGQUEiLCJ2ZXIiOiIxLjAifQ.kId0MqMGrL88oiHeB5HFaUehNw4doWb5i2DfwTZSp_sGblvuQ3aPpr5RQRSHBzVrua_yqXI3HbyqnUu37x6qeiqv0d-lcbVD4bPRZxgaLoTlJisDr4su4pGCH6NdvLt8EQgzYI1FG-BksY3i7_x77W-kVmPpRA17-cVOpE06XVux8DGLTnl6Fl6hnQMv8Sn-283wnFMN6yNJ30B0Ddt0yfY3vDIgJeY6dU0hLIIWVyvBQD5rOPQE4gF7r3y9sbvbDVK32irjUYejEw_sRYMPKoKFE-DDrfix9l-SbQr898u_4y_LppjIPKF3CJAl9fnNItc3tdh-mCozODsMbVcY6Q";
            var storageCredentials = new StorageCredentials(new TokenCredential(tokenCredential));

            CloudFileClient blobClient = new CloudFileClient(new Uri(this.ConnectionStr), storageCredentials);
            return blobClient;
        }

        private static string SeparateFileAndPath(ref string fileName)
        {
            var lastSlash = fileName.LastIndexOf("/");
            var path = fileName.Substring(0, lastSlash);
            fileName = fileName.Substring(lastSlash + 1).ToUpper();
            return path;
        }
    }
}