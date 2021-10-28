namespace CustomSearchesServicesLibrary.CustomSearches.Services.RScriptModel
{
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Service that deletes a rscript model.
    /// </summary>
    public class DeleteRScriptModelService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRScriptModelService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteRScriptModelService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes a rscript model.
        /// </summary>
        /// <param name="rscriptModelId">The rscript model id.</param>
        /// <param name="isSoftDelete">If set to <c>true</c> it will soft delete the Rscript.  Otherwise a full delete will be tried.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="blobContainer">The blob container.</param>
        /// <exception cref="CustomSearchesConflictException">Can't delete RscriptModel because there is at least one associated post process.</exception>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <returns>The Task.</returns>
        public async Task DeleteRScriptModelAsync(int rscriptModelId, bool isSoftDelete, CustomSearchesDbContext dbContext, CloudBlobContainer blobContainer)
        {
            this.ServiceContext.AuthProvider.AuthorizeAdminRole("DeleteRScriptModel");

            RscriptModel rscriptModel = await dbContext.RscriptModel
                .Where(r => r.RscriptModelId == rscriptModelId)
                .Include(r => r.CustomSearchExpression)
                .Include(r => r.CustomSearchParameter)
                .ThenInclude(csp => csp.CustomSearchExpression)
                .Include(r => r.DatasetPostProcess)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(rscriptModel, "RscriptModel", rscriptModelId);

            if (isSoftDelete)
            {
                rscriptModel.IsDeleted = true;
            }
            else
            {
                if (rscriptModel.DatasetPostProcess.Count > 0)
                {
                    throw new CustomSearchesConflictException(
                        $"Can't delete RscriptModel '{rscriptModel}' because there is at least one associated post process.",
                        null);
                }

                dbContext.CustomSearchExpression.RemoveRange(rscriptModel.CustomSearchExpression);

                foreach (var parameter in rscriptModel.CustomSearchParameter)
                {
                    dbContext.CustomSearchExpression.RemoveRange(parameter.CustomSearchExpression);
                }

                dbContext.CustomSearchParameter.RemoveRange(rscriptModel.CustomSearchParameter);

                dbContext.RscriptModel.Remove(rscriptModel);

                if (!string.IsNullOrWhiteSpace(rscriptModel.RscriptFolderName))
                {
                    int folderUsageCount = await
                        (from r in dbContext.RscriptModel
                         where !string.IsNullOrWhiteSpace(r.RscriptFolderName) &&
                            r.RscriptFolderName.Trim().ToLower() == rscriptModel.RscriptFolderName.Trim().ToLower()
                         select r.RscriptFolderName).CountAsync();

                    if (folderUsageCount == 1)
                    {
                        await this.DeleteBlobContents(rscriptModel.RscriptFolderName, blobContainer);
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the BLOB contents.
        /// </summary>
        /// <param name="folderPath">The r script model.</param>
        /// <param name="blobContainer">The BLOB container.</param>
        private async Task DeleteBlobContents(string folderPath, CloudBlobContainer blobContainer)
        {
            var continuationToken = new BlobContinuationToken();
            do
            {
                var result = await blobContainer.ListBlobsSegmentedAsync(
                    folderPath,
                    useFlatBlobListing: true,
                    BlobListingDetails.None,
                    maxResults: null,
                    continuationToken,
                    options: null,
                    operationContext: null);

                continuationToken = result.ContinuationToken;
                await Task.WhenAll(result.Results
                    .Select(item => (item as CloudBlob)?.DeleteIfExistsAsync())
                    .Where(task => task != null));
            }
            while (continuationToken != null);
        }
    }
}
