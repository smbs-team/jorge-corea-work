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
        public async Task<int> CreateFolder(GisDbContext dbContext, CreateFolderData createFolderData)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            UserMapFolderManager.ValidateFolderPath(createFolderData.FolderPath);

            GisFolderItemType folderItemType = InputValidationHelper.ValidateEnum<GisFolderItemType>(createFolderData.FolderItemType, nameof(createFolderData.FolderItemType));
            CustomSearchFolderType folderType = UserMapFolderManager.GetFolderType(createFolderData.FolderPath);

            IFolderManager<Folder> folderManager = FolderManagerHelper.CreateFolderManager(folderItemType, folderType, userId, dbContext);

            Folder leafFolder = await folderManager.EnsurePathExistsAsync(createFolderData.FolderPath);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, leafFolder, isLocked: false, "CreateFolder");

            await dbContext.ValidateAndSaveChangesAsync();

            return leafFolder.FolderId;
        }
    }
}
