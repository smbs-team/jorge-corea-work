namespace PTASTileStorageWorkerLibrary.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Utility class to handle upload of directory and files to the blob container (the easy way).
    /// </summary>
    public class BlobUploader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobUploader"/> class.
        /// </summary>
        /// <param name="container">Blob target container.</param>
        public BlobUploader(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the target blob container. It is set on the contructor only.
        /// </summary>
        public CloudBlobContainer Container { get; private set; }

        /// <summary>
        /// Upload a folder to the container from source path to target path.
        /// </summary>
        /// <param name="sourcePath">Path to local shared folder source.</param>
        /// <param name="targetPath">target folder in container.</param>
        /// <returns>Async task to allow await.</returns>
        public async Task UploadFolder(string sourcePath, string targetPath)
        {
            // upload the files
            var targetDirectory = this.Container.GetDirectoryReference(targetPath);
            foreach (string item in System.IO.Directory.EnumerateFileSystemEntries(sourcePath))
            {
                var localPath = System.IO.Path.Combine(sourcePath, item);
                if (System.IO.Directory.Exists(localPath))
                {
                    await this.UploadFolder(localPath, targetPath + "/" + System.IO.Path.GetFileName(item));
                }
                else
                {
                    // Retrieve reference to a blob named "myblob".
                    CloudBlockBlob blockBlob = this.Container.GetBlockBlobReference(targetPath + "/" + System.IO.Path.GetFileName(item));
                    await blockBlob.UploadFromFileAsync(localPath);
                }
            }
        }
    }
}
