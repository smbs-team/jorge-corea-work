namespace CustomSearchesServicesLibrary.CustomSearches.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.CustomSearches.Folder;
    using CustomSearchesServicesLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
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
        /// <returns>
        /// The folders.
        /// </returns>
        public async Task<GetFoldersForUserResponse> GetFoldersForUserAsync(CustomSearchesDbContext dbContext, Guid userId)
        {
            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userId, "GetFoldersForUser");

            GetFoldersForUserResponse response = new GetFoldersForUserResponse();

            if (userId == Guid.Empty)
            {
                throw new CustomSearchesRequestBodyException("UserId should be assigned.", null);
            }

            List<FolderData> foldersData = new List<FolderData>();

            DatasetFolderManager userFolderManager = new DatasetFolderManager(CustomSearchFolderType.User, userId, dbContext);
            DatasetFolderManager sharedFolderManager = new DatasetFolderManager(CustomSearchFolderType.Shared, userId, dbContext);

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

            response.Folders = foldersData.ToArray();
            return response;
        }
    }
}
