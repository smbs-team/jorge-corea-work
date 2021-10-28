namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model.RScript;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;

    /// <summary>
    /// Service that gets the dataset post process file.
    /// </summary>
    public class GetDatasetFileService : BaseService
    {
        /// <summary>
        /// Folder name for results in blob container.
        /// </summary>
        public const string BlobResultsFolderName = "results";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDatasetFileService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetDatasetFileService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the specified file in the dataset post process.
        /// </summary>
        /// <param name="datasetPostProcessId">The dataset post process id.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The BLOB container.</param>
        /// <param name="checkFileInResults">if set to <c>true</c> [check file in results].</param>
        /// <returns>
        /// The file bytes.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Post process or file was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<byte[]> GetDatasetFileAsync(int datasetPostProcessId, string fileName, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer, bool checkFileInResults)
        {
            InputValidationHelper.AssertNotEmpty(fileName, nameof(fileName));
            DatasetPostProcess datasetPostProcess = await dbContext.DatasetPostProcess.Where(d => (d.DatasetPostProcessId == datasetPostProcessId)).FirstOrDefaultAsync();
            InputValidationHelper.AssertEntityExists(datasetPostProcess, "DatasetPostProcess", datasetPostProcessId);

            if (checkFileInResults)
            {
                RScriptResultsData rScriptResultsData = JsonHelper.DeserializeObject<RScriptResultsData>(datasetPostProcess.ResultPayload);

                if (rScriptResultsData.FileResults.FirstOrDefault(f => string.Compare(f.FileName, fileName, ignoreCase: true) == 0) == null)
                {
                    throw new CustomSearchesEntityNotFoundException(
                        $"File name '{fileName}' not found in result payload.",
                        null);
                }
            }

            fileName = fileName.ToLower();

            try
            {
                NameValidator.ValidateFileName(fileName);
            }
            catch (ArgumentException ex)
            {
                throw new CustomSearchesRequestBodyException(
                    $"Invalid file name '{fileName}'.",
                    ex);
            }

            CloudBlobDirectory directory = blobContainer.GetDirectoryReference(
                $"{GetDatasetFileService.BlobResultsFolderName}/{datasetPostProcess.DatasetId}/{datasetPostProcess.DatasetPostProcessId}".ToLower());
            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(fileName);

            if (!await blockBlob.ExistsAsync())
            {
                throw new CustomSearchesEntityNotFoundException(
                    $"File '{fileName}' not found in blob.",
                    null);
            }

            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                stream.Position = 0;
                byte[] b = stream.ToArray();
                return b;
            }
        }
    }
}
