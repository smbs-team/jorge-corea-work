namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that creates the folder.
    /// </summary>
    public class CreateFolderService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFolderService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public CreateFolderService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Creates the folder.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="createFolderData">Folder data.</param>
        /// <returns>
        /// The leaf folder id.
        /// </returns>
        /// <exception cref="FolderManagerException">Invalid folder path format or folder not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task<int> CreateFolderAsync(CustomSearchesDbContext dbContext, CreateFolderData createFolderData)
        {
            DatasetFolderManager.ValidateFolderPath(createFolderData.FolderPath);

            CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(createFolderData.FolderPath);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            DatasetFolderManager folderManager = new DatasetFolderManager(folderType, userId, dbContext);
            Folder leafFolder = await folderManager.EnsurePathExistsAsync(createFolderData.FolderPath);
            await dbContext.ValidateAndSaveChangesAsync();

            return leafFolder.FolderId;
        }
    }
}
