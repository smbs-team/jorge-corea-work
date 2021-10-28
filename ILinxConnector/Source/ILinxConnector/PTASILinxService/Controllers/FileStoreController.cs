// <copyright file="FileStoreController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using Microsoft.WindowsAzure.Storage.Blob;

    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes.Entities;
    using PTASIlinxService.Classes.Exceptions;

    using PTASServicesCommon.CloudStorage;

    using Swashbuckle.Swagger.Annotations;

    /// <summary>
    /// File Store Controller.
    /// Note: Stores files in a {container}/{id}/filename.ext blob structure.
    /// </summary>
    public class FileStoreController : ApiController
    {
        private readonly ICloudStorageProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStoreController"/> class.
        /// </summary>
        /// <param name="provider">Cloud storage provider.</param>
        public FileStoreController(ICloudStorageProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Delete a file or a list of files in a container in an id.
        /// </summary>
        /// <param name="fsParams">Delete params.</param>
        /// <returns>Async result of operation.</returns>
        [HttpDelete]
        public async Task<dynamic> Delete([FromBody] FilestoreDeleteParams fsParams)
        {
            CloudBlobContainer blobStorage = this.GetBlobStorage(fsParams.ContainerName);

            // empty filename means delete directory.
            if (string.IsNullOrEmpty(fsParams.FileName))
            {
                var results = await Task.WhenAll(this
                    .ListFiles(fsParams.Id, blobStorage)
                    .Select(blob => (blob as CloudBlob)?.DeleteIfExistsAsync()));
                int successCount = results.Count(wasDeleted => wasDeleted);
                return new { message = $"Deleted: {successCount}. Failed: {results.Length - successCount}." };
            }

            var file = fsParams.Id.ToLower() + "/" + fsParams.FileName;
            var fref = blobStorage.GetBlockBlobReference(file);
            var deleted = await fref.DeleteIfExistsAsync();
            return new { message = deleted ? "Deleted one file" : "Could not delete file." };
        }

        /// <summary>
        /// Gets a list of files in a container under an id directory.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="id">Id to look into.</param>
        /// <param name="includeSAS">Include SAS.</param>
        /// <returns>List of file URLS.</returns>
        [HttpGet]
        public IEnumerable<string> Get(string containerName, string id, bool includeSAS = false)
        {
            var blobStorage = this.GetBlobStorage(containerName);
            IEnumerable<Microsoft.WindowsAzure.Storage.Blob.IListBlobItem> allFiles = this.ListFiles(id, blobStorage);

            var token = includeSAS ? blobStorage.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.Now.AddDays(1),
            }) : string.Empty;
            return allFiles
                .Select(b => $"{b.StorageUri.PrimaryUri.AbsoluteUri}{token}")
                .Select(b => HttpUtility.UrlDecode(b));
        }

        /// <summary>
        /// Posts a list of files to the storage.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="id">Id of the folder.</param>
        /// <param name="includeSAS">Include SAS token.</param>
        /// <returns>List of uploaded URLS.</returns>
        [HttpPost]
        [SwaggerOperationFilter(typeof(NeedsFilesFilter))]
        public async Task<IEnumerable<string>> Post(string containerName, string id, bool includeSAS = false)
        {
            id = id.ToLower();
            var files = this.GetFiles();
            if (!files.Any())
            {
                throw new WrongNumberOfFilesException(1, 0);
            }

            var blobStorage = this.GetBlobStorage(containerName);

            var token = includeSAS ? blobStorage.GetSharedAccessSignature(new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.Now.AddDays(1),
            }) : string.Empty;

            return await Task.WhenAll(
                    files
                        .Select(async f =>
                        {
                            try
                            {
                                var cloudBlockBlob = blobStorage.GetBlockBlobReference($"{id}/{f.fileName}");
                                await cloudBlockBlob
                                    .UploadFromStreamAsync(f.fileStream);
                                return $"{cloudBlockBlob.StorageUri.PrimaryUri.AbsoluteUri}{token}";
                            }
                            catch (Exception ex)
                            {
                                return "Error: " + ex.Message;
                            }
                        }));
        }

        /// <summary>
        /// Get a list of posted files.
        /// </summary>
        /// <returns>The files from the request.</returns>
        protected virtual IEnumerable<(string fileName, Stream fileStream)> GetFiles()
        {
            HttpFileCollection files = this.GetHttpContext().Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                yield return (files[i].FileName, files[i].InputStream);
            }
        }

        /// <summary>
        /// Returns current http context.
        /// </summary>
        /// <returns>Current http context.</returns>
        protected virtual HttpContext GetHttpContext()
        {
            return HttpContext.Current;
        }

        private CloudBlobContainer GetBlobStorage(string containerName)
        {
            return this.provider.GetCloudBlobContainer(containerName);
        }

        private IEnumerable<IListBlobItem> ListFiles(string id, Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer blobStorage) =>
                                    blobStorage.GetDirectoryReference(id.ToLower()).ListBlobs();
    }
}