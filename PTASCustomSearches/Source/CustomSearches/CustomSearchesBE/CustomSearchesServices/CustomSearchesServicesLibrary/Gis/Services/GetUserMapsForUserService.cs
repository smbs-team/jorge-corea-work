namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that gets the user maps for user.
    /// </summary>
    public class GetUserMapsForUserService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserMapsForUserService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public GetUserMapsForUserService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Gets the user maps for user.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderType">The folder type.</param>
        /// <returns>
        /// The user maps.
        /// </returns>
        public async Task<GetUserMapsForUserResponse> GetUserMapsForUser(GisDbContext dbContext, Guid userId, string folderType)
        {
            this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userId, "GetUserMapsForUser");

            GetUserMapsForUserResponse response = new GetUserMapsForUserResponse();

            IQueryable<UserMap> query;
            if (string.IsNullOrWhiteSpace(folderType))
            {
                query = from m in dbContext.UserMap
                        join f in dbContext.Folder
                        on m.ParentFolderId equals f.FolderId
                        where (f.FolderType.ToLower() != CustomSearchFolderType.SystemRenderer.ToString().ToLower()) &&
                              (f.FolderType.ToLower() != CustomSearchFolderType.User.ToString().ToLower() || (f.UserId == userId))
                        select m;
            }
            else
            {
                CustomSearchFolderType customSearchFolderType = InputValidationHelper.ValidateEnum<CustomSearchFolderType>(folderType, nameof(folderType));

                if (customSearchFolderType == CustomSearchFolderType.User)
                {
                    query = from m in dbContext.UserMap
                            join f in dbContext.Folder
                            on m.ParentFolderId equals f.FolderId
                            where f.FolderType.ToLower() == customSearchFolderType.ToString().ToLower()
                            && (f.UserId == userId)
                            select m;
                }
                else
                {
                    query = from m in dbContext.UserMap
                            join f in dbContext.Folder
                            on m.ParentFolderId equals f.FolderId
                            where f.FolderType.ToLower() == customSearchFolderType.ToString().ToLower()
                            select m;
                }
            }

            var userMaps = await query.
                                  Include(m => m.CreatedByNavigation).
                                  Include(m => m.LastModifiedByNavigation).
                                  Include(m => m.ParentFolder).
                                       ThenInclude(f => f.User).
                                  ToArrayAsync();

            response.UserMaps = new UserMapData[userMaps.Length];

            if (userMaps.Length > 0)
            {
                UserMapFolderManager userFolderManager = new UserMapFolderManager(CustomSearchFolderType.User, userId, dbContext);
                UserMapFolderManager sharedFolderManager = new UserMapFolderManager(CustomSearchFolderType.Shared, userId, dbContext);
                UserMapFolderManager systemFolderManager = new UserMapFolderManager(CustomSearchFolderType.System, userId, dbContext);
                UserMapFolderManager systemRendererFolderManager = new UserMapFolderManager(CustomSearchFolderType.SystemRenderer, userId, dbContext);

                Dictionary<Guid, UserInfoData> userDetails = new Dictionary<Guid, UserInfoData>();

                for (int i = 0; i < userMaps.Length; i++)
                {
                    var userMap = userMaps[i];

                    UserMapData userMapData = new UserMapData(userMap, ModelInitializationType.Summary, userDetails);
                    CustomSearchFolderType customSearchFolderType = Enum.Parse<CustomSearchFolderType>(userMap.ParentFolder.FolderType, ignoreCase: true);
                    switch (customSearchFolderType)
                    {
                        case CustomSearchFolderType.Shared:
                            userMapData.FolderPath = await sharedFolderManager.GetFolderPathAsync(userMap);
                            break;
                        case CustomSearchFolderType.System:
                            userMapData.FolderPath = await systemFolderManager.GetFolderPathAsync(userMap);
                            break;
                        case CustomSearchFolderType.SystemRenderer:
                            userMapData.FolderPath = await systemRendererFolderManager.GetFolderPathAsync(userMap);
                            break;
                        case CustomSearchFolderType.User:
                            userMapData.FolderPath = await userFolderManager.GetFolderPathAsync(userMap);
                            break;
                        default:
                            break;
                    }

                    response.UserMaps[i] = userMapData;
                }

                response.UsersDetails = UserDetailsHelper.GetUserDetailsArray(userDetails);
            }

            return response;
        }
    }
}
