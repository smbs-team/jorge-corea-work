namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that updates the dataset attributes.
    /// </summary>
    public class UpdateDatasetAttributesService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDatasetAttributesService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateDatasetAttributesService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the dataset attributes.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="updateDatasetAttributesData">The dataset attributes.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        /// <exception cref="NotSupportedException">Updating dataset attributes is not supported for reference datasets.</exception>
        /// <exception cref="CustomSearchesConflictException">The folder already has a entity with the same name.</exception>
        public async Task UpdateDatasetAttributesAsync(Guid datasetId, UpdateDatasetAttributesData updateDatasetAttributesData, CustomSearchesDbContext dbContext)
        {
            InputValidationHelper.AssertNotEmpty(updateDatasetAttributesData.NewName, nameof(updateDatasetAttributesData.NewName));
            updateDatasetAttributesData.NewName = updateDatasetAttributesData.NewName.Trim();

            Dataset dataset = await dbContext.Dataset
                .Where(d => d.DatasetId == datasetId)
                .Include(d => d.ParentFolder)
                .ThenInclude(f => f.Dataset)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            UserProject userProject = await DatasetHelper.GetOwnerProjectAsync(dataset, dbContext);

            this.ServiceContext.AuthProvider.AuthorizeProjectOrFolderItemOperation<Folder>(dataset.UserId, dataset.ParentFolder, dataset.IsLocked, userProject, "UpdateDatasetAttributes");

            if (DatasetHelper.IsReferenceDataset(dataset))
            {
                throw new NotSupportedException(
                    $"Updating dataset attributes is not supported for reference datasets.",
                    null);
            }

            if (dataset.DatasetName.Trim().ToLower() != updateDatasetAttributesData.NewName.ToLower())
            {
                if (dataset.ParentFolder != null)
                {
                    var existingDataset = dataset.ParentFolder.Dataset.FirstOrDefault(d => d.DatasetName.Trim().ToLower() == updateDatasetAttributesData.NewName.ToLower());
                    if (existingDataset != null)
                    {
                        throw new CustomSearchesConflictException(
                            $"The folder already has a entity with the same name.",
                            null);
                    }
                }
            }

            dataset.DatasetName = updateDatasetAttributesData.NewName;
            dataset.Comments = updateDatasetAttributesData.NewComments;
            dataset.LastModifiedTimestamp = DateTime.UtcNow;
            dataset.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;
            dbContext.Dataset.Update(dataset);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
