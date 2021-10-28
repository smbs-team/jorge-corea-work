namespace PTASServicesCommon.CloudStorage
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that defines a contract that allows callers to get shared access signature to access cloud storage.
    /// </summary>
    public interface ICloudStorageSharedSignatureProvider
    {
        /// <summary>
        /// Gets the shared access signature.
        /// </summary>
        /// <param name="shareName">Name of the container to grant access to.</param>
        /// <param name="fileName">Name of the File to grant access to.</param>
        /// <param name="requestedPermissions">The requested permissions.</param>
        /// <returns>
        /// The shared access signature.
        /// </returns>
        Task<string> GetSharedFileSignature(string shareName, string fileName, string requestedPermissions = "Read");

        /// <summary>
        /// Gets the shared signature to access cloud storage.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="requestedPermissions">The requested permissions.</param>
        /// <returns>The shared signature.</returns>
        Task<string> GetSharedSignature(string containerName, string blobName, string requestedPermissions = "Read");
    }
}