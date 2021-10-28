namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that assigns the folder to dataset service.
    /// </summary>
    public class AssignFolderToDatasetService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignFolderToDatasetService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public AssignFolderToDatasetService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Assigns the folder to dataset.
        /// </summary>
        /// <param name="datasetId">The dataset id.</param>
        /// <param name="folderData">The folder data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Dataset was not found.</exception>
        /// <exception cref="AuthorizationException">Not authorized request.</exception>
        /// <exception cref="FolderManagerException">Invalid folder path format or folder not found.</exception>
        /// <exception cref="NotSupportedException">Assigning folder to dataset is only supported for user datasets.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task AssignFolderToDatasetAsync(Guid datasetId, AssignFolderToDatasetData folderData, CustomSearchesDbContext dbContext)
        {
            DatasetFolderManager.ValidateFolderPath(folderData.FolderPath);

            Dataset dataset = await dbContext.Dataset.Where(d => d.DatasetId == datasetId).Include(d => d.ParentFolder).FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(dataset, "Dataset", datasetId);

            if (DatasetHelper.IsUserDataset(dataset) == false)
            {
                throw new NotSupportedException(
                    $"Assigning folder to dataset is only supported for user datasets.",
                    null);
            }

            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(dataset.UserId, "AssignFolderToDataset");

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(folderData.FolderPath);
            CustomSearchFolderType prevFolderType = Enum.Parse<CustomSearchFolderType>(dataset.ParentFolder.FolderType, ignoreCase: true);
            this.ServiceContext.AuthProvider.AuthorizeChangeItemFolderOperation(dataset.UserId, prevFolderType, folderType, "AssignFolderToDataset");

            DatasetFolderManager folderManager = new DatasetFolderManager(folderType, dataset.UserId, dbContext);
            await folderManager.AssignFolderToItemAsync(folderData.FolderPath, dataset, modifiedBy: userId);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
