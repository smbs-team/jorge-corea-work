namespace CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that uploads the rscript file.
    /// </summary>
    public class UploadRScriptFileService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadRScriptFileService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UploadRScriptFileService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Uploads the rscript file.
        /// </summary>
        /// <param name="modelId">The rscript model id.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="data">The data to upload.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The BLOB container.</param>
        /// <returns>
        /// The task.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task UploadRScriptFileAsync(int modelId, string fileName, Stream data, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("UploadRScriptFile");

            RscriptModel rscriptModel = await dbContext.RscriptModel
                .Where(m => m.RscriptModelId == modelId)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(rscriptModel, "RscriptModel", modelId);

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference($"{rscriptModel.RscriptFolderName}/{fileName}");
            await blockBlob.UploadFromStreamAsync(data);
        }
    }
}
