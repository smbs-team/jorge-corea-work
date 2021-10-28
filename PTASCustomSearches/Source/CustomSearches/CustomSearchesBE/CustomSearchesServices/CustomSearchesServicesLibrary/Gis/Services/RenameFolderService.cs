namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Folder;
    using CustomSearchesServicesLibrary.Gis.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that renames a gis folder.
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
        /// Renames a gis folder.
        /// </summary>
        /// <param name="renameFolderData">The folder data.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="CustomSearchesEntityNotFoundException">Custom search definition or parameter was not found.</exception>
        /// <exception cref="FolderManagerException">Can't modify root folder or invalid folder path format or folder was not found.</exception>
        /// <exception cref="CustomSearchesRequestBodyException">Required parameter was not added or parameter has an invalid value.</exception>
        public async Task RenameFolderAsync(RenameFolderData renameFolderData, GisDbContext dbContext)
        {
            UserMapFolderManager.ValidateFolderPath(renameFolderData.FolderPath);
            GisFolderItemType folderItemType = InputValidationHelper.ValidateEnum<GisFolderItemType>(renameFolderData.FolderItemType, nameof(renameFolderData.FolderItemType));

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            CustomSearchFolderType folderType = UserMapFolderManager.GetFolderType(renameFolderData.FolderPath);
            IFolderManager<Folder> folderManager = FolderManagerHelper.CreateFolderManager(folderItemType, folderType, userId, dbContext);

            Folder folder = await folderManager.RenameFolderAsync(renameFolderData.FolderPath, renameFolderData.NewName);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, folder, isLocked: false, "RenameFolder");

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}