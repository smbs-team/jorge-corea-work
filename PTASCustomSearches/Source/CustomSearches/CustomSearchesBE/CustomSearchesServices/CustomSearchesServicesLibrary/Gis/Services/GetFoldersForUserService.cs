namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Folder;
    using CustomSearchesServicesLibrary.Gis.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;

    /// <summary>
    /// Service that gets the folders for user.
    /// </summary>
    public class GetFoldersForUserService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFoldersForUserService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetFoldersForUserService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the folders for user.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderItemType">The folder item type. Should be 'UserMap'.</param>
        /// <returns>
        /// The user maps.
        /// </returns>
        public async Task<GetFoldersForUserResponse> GetFoldersForUser(GisDbContext dbContext, Guid userId, string folderItemType)
        {
            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userId, "GetFoldersForUser");

            GetFoldersForUserResponse response = new GetFoldersForUserResponse();

            GisFolderItemType folderItemTypeValue = InputValidationHelper.ValidateEnum<GisFolderItemType>(folderItemType, nameof(folderItemType));

            if (userId == Guid.Empty)
            {
                throw new CustomSearchesRequestBodyException("UserId should be assigned.", null);
            }

            List<FolderData> foldersData = new List<FolderData>();

            IFolderManager<Folder> userFolderManager = FolderManagerHelper.CreateFolderManager(folderItemTypeValue, CustomSearchFolderType.User, userId, dbContext);
            IFolderManager<Folder> sharedFolderManager = FolderManagerHelper.CreateFolderManager(folderItemTypeValue, CustomSearchFolderType.Shared, userId, dbContext);
            IFolderManager<Folder> systemFolderManager = FolderManagerHelper.CreateFolderManager(folderItemTypeValue, CustomSearchFolderType.System, userId, dbContext);

            Folder userFolder = await userFolderManager.GetFolderTreeAsync();
            if (userFolder != null)
            {
                foldersData.Add(new FolderData(userFolder, ModelInitializationType.FullObjectWithDepedendencies));
            }

            Folder sharedFolder = await sharedFolderManager.GetFolderTreeAsync();
            if (sharedFolder != null)
            {
                foldersData.Add(new FolderData(sharedFolder, ModelInitializationType.FullObjectWithDepedendencies));
            }

            Folder systemFolder = await systemFolderManager.GetFolderTreeAsync();
            if (systemFolder != null)
            {
                foldersData.Add(new FolderData(systemFolder, ModelInitializationType.FullObjectWithDepedendencies));
            }

            response.Folders = foldersData.ToArray();
            return response;
        }
    }
}
