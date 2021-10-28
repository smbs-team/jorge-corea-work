namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.Datasets;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScript;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that gets the dataset post process dump file list.
    /// </summary>
    public class GetPostProcessDumpFilesService : BaseService
    {
        /// <summary>
        /// Folder name for results in blob container.
        /// </summary>
        public const string BlobResultsFolderName = "results";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPostProcessDumpFilesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetPostProcessDumpFilesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets a list of dump files for a failed post-process.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The BLOB container.</param>
        /// <returns>
        /// The list of files.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Post process or file was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<FilesData> GetPostProcessDumpFilesAsync(int datasetPostProcessId, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            var toReturn = new List<string>();

            DatasetPostProcess datasetPostProcess = await dbContext.DatasetPostProcess.Where(d => (d.DatasetPostProcessId == datasetPostProcessId)).FirstOrDefaultAsync();
            InputValidationHelper.AssertEntityExists(datasetPostProcess, "DatasetPostProcess", datasetPostProcessId);

            CloudBlobDirectory directory = blobContainer.GetDirectoryReference(
                $"{GetDatasetFileService.BlobResultsFolderName}/{datasetPostProcess.DatasetId}/{datasetPostProcess.DatasetPostProcessId}".ToLower());

            bool finished = false;
            BlobContinuationToken continuation = null;
            while (!finished)
            {
                var blobList = await directory.ListBlobsSegmentedAsync(continuation);

                foreach (IListBlobItem item in blobList.Results)
                {
                    toReturn.Add(item.StorageUri.PrimaryUri.LocalPath);
                }

                continuation = blobList.ContinuationToken;
                finished = continuation == null;
            }

            return new FilesData { Files = toReturn.ToArray() };
        }
    }
}
