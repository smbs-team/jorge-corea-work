namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that renames a dataset folder.
    /// </summary>
    public class RenameFolderService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameFolderService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RenameFolderService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Renames a dataset folder.
        /// </summary>
        /// <param name="renameFolderData">The folder data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="FolderManagerException">Can't modify root folder or invalid folder path format or folder was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task RenameFolderAsync(RenameFolderData renameFolderData, CustomSearchesDbContext dbContext)
        {
            DatasetFolderManager.ValidateFolderPath(renameFolderData.FolderPath);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            CustomSearchFolderType folderType = DatasetFolderManager.GetFolderType(renameFolderData.FolderPath);
            DatasetFolderManager folderManager = new DatasetFolderManager(folderType, userId, dbContext);
            await folderManager.RenameFolderAsync(renameFolderData.FolderPath, renameFolderData.NewName);
            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
