namespace PTASServicesCommon.CloudStorage
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;

    /// <summary>
    /// Provides for SAS (Shared Signature Access) string for blob access.
    /// </summary>
    /// <seealso cref="PTASServicesCommon.CloudStorage.ICloudStorageSharedSignatureProvider" />
    public class BlobSharedSignatureProvider : ICloudStorageSharedSignatureProvider
    {
        /// <summary>
        /// The ad hoc policy end time in hours.
        /// </summary>
        private const int AdHocPolicyEndInHours = 48;

        /// <summary>
        /// The ad hoc policy start time in minutes.
        /// </summary>
        private const int AdHocPolicyStartInMinutes = -5;

        /// <summary>
        /// The cloud storage provider.
        /// </summary>
        private ICloudStorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobSharedSignatureProvider"/> class.
        /// </summary>
        /// <param name="storageProvider">The cloud storage provider.</param>
        /// <exception cref="System.ArgumentNullException">When storageProvider is null.</exception>
        public BlobSharedSignatureProvider(ICloudStorageProvider storageProvider)
        {
            if (storageProvider == null)
            {
                throw new System.ArgumentNullException(nameof(storageProvider));
            }

            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// Gets the shared access signature.
        /// </summary>
        /// <param name="shareName">Name of the container to grant access to.</param>
        /// <param name="fileName">Name of the BLOB to grant access to.</param>
        /// <param name="requestedPermissions">The requested permissions.</param>
        /// <returns>
        /// The shared access signature.
        /// </returns>
        public async Task<string> GetSharedFileSignature(string shareName, string fileName, string requestedPermissions = "Read")
        {
            var permissions = SharedAccessFilePermissions.Read; // default to read permissions
            Enum.TryParse(requestedPermissions, out permissions);

            var blobClient = await this.storageProvider.GetCloudFileClient();
            var container = blobClient.GetShareReference(shareName);

            string sasToken = fileName != null ?
                this.GetFileSasToken(container, fileName, permissions) :
                this.GetFileShareSasToken(container, permissions);

            return sasToken;
        }

        /// <summary>
        /// Gets the shared access signature.
        /// </summary>
        /// <param name="containerName">Name of the container to grant access to.</param>
        /// <param name="blobName">Name of the BLOB to grant access to.</param>
        /// <param name="requestedPermissions">The requested permissions.</param>
        /// <returns>
        /// The shared access signature.
        /// </returns>
        public async Task<string> GetSharedSignature(string containerName, string blobName, string requestedPermissions = "Read")
        {
            var permissions = SharedAccessBlobPermissions.Read; // default to read permissions
            Enum.TryParse(requestedPermissions, out permissions);

            var blobClient = await this.storageProvider.GetCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);

            string sasToken = blobName != null ?
                this.GetBlobSasToken(container, blobName.ToString(), permissions) :
                this.GetContainerSasToken(container, permissions);

            return sasToken;
        }

        private SharedAccessFilePolicy CreateAdHocFileSasPolicy(SharedAccessFilePermissions permissions)
        {
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and
            // to construct a shared access policy that is saved to the container's shared access policies.
            return new SharedAccessFilePolicy()
            {
                // Set start time to five minutes before now to avoid clock skew.
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(BlobSharedSignatureProvider.AdHocPolicyStartInMinutes),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(BlobSharedSignatureProvider.AdHocPolicyEndInHours),
                Permissions = permissions,
            };
        }

        /// <summary>
        /// Creates an ad hoc SAS policy that will work for the next 48 hors.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns>A SAS policy.</returns>
        private SharedAccessBlobPolicy CreateAdHocSasPolicy(SharedAccessBlobPermissions permissions)
        {
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and
            // to construct a shared access policy that is saved to the container's shared access policies.
            return new SharedAccessBlobPolicy()
            {
                // Set start time to five minutes before now to avoid clock skew.
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(BlobSharedSignatureProvider.AdHocPolicyStartInMinutes),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(BlobSharedSignatureProvider.AdHocPolicyEndInHours),
                Permissions = permissions,
            };
        }

        private string GetBlobSasToken(CloudBlobContainer container, string blobName, SharedAccessBlobPermissions permissions, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                var adHocSas = this.CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSas);
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
            }

            return sasBlobToken;
        }

        /// <summary>
        /// Gets the container SAS token.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="storedPolicyName">Name of the stored policy.</param>
        /// <returns>A SAS token for the container.</returns>
        private string GetContainerSasToken(CloudBlobContainer container, SharedAccessBlobPermissions permissions, string storedPolicyName = null)
        {
            string sasContainerToken;

            // If no stored policy is specified, create a new access policy and define its constraints.
            if (storedPolicyName == null)
            {
                var adHocSas = this.CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(adHocSas, null);
            }
            else
            {
                // Generate the shared access signature on the container. In this case, all of the constraints for the
                // shared access signature are specified on the stored access policy, which is provided by name.
                // It is also possible to specify some constraints on an ad-hoc SAS and others on the stored access policy.
                // However, a constraint must be specified on one or the other; it cannot be specified on both.
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);
            }

            return sasContainerToken;
        }

        private string GetFileSasToken(CloudFileShare container, string fileName, SharedAccessFilePermissions permissions, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudFile fileRef = container.GetRootDirectoryReference().GetFileReference(fileName);

            if (policyName == null)
            {
                SharedAccessFilePolicy adHocSas = this.CreateAdHocFileSasPolicy(permissions);

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = fileRef.GetSharedAccessSignature(adHocSas);
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = fileRef.GetSharedAccessSignature(null, policyName);
            }

            return sasBlobToken;
        }

        private string GetFileShareSasToken(CloudFileShare container, SharedAccessFilePermissions permissions, string storedPolicyName = null)
        {
            string sasContainerToken;

            // If no stored policy is specified, create a new access policy and define its constraints.
            if (storedPolicyName == null)
            {
                var adHocSas = this.CreateAdHocFileSasPolicy(permissions);

                // Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(adHocSas, null);
            }
            else
            {
                // Generate the shared access signature on the container. In this case, all of the constraints for the
                // shared access signature are specified on the stored access policy, which is provided by name.
                // It is also possible to specify some constraints on an ad-hoc SAS and others on the stored access policy.
                // However, a constraint must be specified on one or the other; it cannot be specified on both.
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);
            }

            return sasContainerToken;
        }
    }
}