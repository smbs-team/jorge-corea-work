namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes the folder.
    /// </summary>
    public class DeleteFolderService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolderService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteFolderService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folderData">Folder data.</param>
        /// <param name="dbContext">Database context.</param>
        /// <returns>
        /// The delete folder response.
        /// </returns>
        public async Task<DeleteFolderResponse> DeleteFolderAsync(CreateFolderData folderData, GisDbContext dbContext)
        {
            UserMapFolderManager.ValidateFolderPath(folderData.FolderPath);

            GisFolderItemType folderItemType = InputValidationHelper.ValidateEnum<GisFolderItemType>(folderData.FolderItemType, nameof(folderData.FolderItemType));

            DeleteFolderResponse response;
            CustomSearchFolderType folderType = UserMapFolderManager.GetFolderType(folderData.FolderPath);
            switch (folderItemType)
            {
                case GisFolderItemType.UserMap:
                    response = await this.DeleteUserMapFolderAsync(dbContext, folderData, folderType);
                    break;
                default:
                    throw new NotSupportedException($"Operation not supported for {nameof(folderItemType)}: '{folderItemType}'.");
            }

            await dbContext.SaveChangesAsync();

            return response;
        }

        /// <summary>
        /// Deletes the user map folder.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="folderData">Folder data.</param>
        /// <param name="folderType">Folder type.</param>
        /// <returns>
        /// The delete folder response.
        /// </returns>
        public async Task<DeleteFolderResponse> DeleteUserMapFolderAsync(GisDbContext dbContext, CreateFolderData folderData, CustomSearchFolderType folderType)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            UserMapFolderManager userMapFolderManager = new UserMapFolderManager(folderType, userId, dbContext);
            Folder folder = await userMapFolderManager.ValidateEditableFolderExistenceAsync(folderData.FolderPath);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, folder, isLocked: false, "DeleteUserMapFolder");

            var userMaps = await (from d in dbContext.UserMap
                                  join f in dbContext.Folder
                                  on d.ParentFolderId equals f.FolderId
                                  where (f.FolderType.ToLower() == folderType.ToString().ToLower()) &&
                                        (f.FolderItemType.ToLower() == GisFolderItemType.UserMap.ToString().ToLower() &&
                                        ((f.UserId == userId) ||
                                            (f.FolderType.ToLower() != CustomSearchFolderType.User.ToString().ToLower())))
                                  select d).
                                  Include(d => d.ParentFolder).
                                  ToArrayAsync();

            List<DeleteEntityErrorData> deleteEntityErrors = new List<DeleteEntityErrorData>();

            if ((userMaps != null) && (userMaps.Length > 0))
            {
                DeleteUserMapService service = new DeleteUserMapService(this.ServiceContext);

                foreach (var userMap in userMaps)
                {
                    string folderPath = await userMapFolderManager.GetFolderPathAsync(userMap);

                    if (folderPath.ToLower().StartsWith(folderData.FolderPath.ToLower()))
                    {
                        try
                        {
                            await service.DeleteUserMap(dbContext, userMap.UserMapId);
                        }
                        catch (AuthorizationException ex)
                        {
                            DeleteEntityErrorData entityError = new DeleteEntityErrorData
                            {
                                Id = userMap.UserMapId.ToString(),
                                Path = $"{folderPath}/{userMap.UserMapName}",
                                Message = ex.GetBaseException().Message
                            };

                            deleteEntityErrors.Add(entityError);
                        }
                    }
                }
            }

            await userMapFolderManager.RemoveEmptyFoldersAsync(folder);

            DeleteFolderResponse response = new DeleteFolderResponse();
            if (deleteEntityErrors.Count > 0)
            {
                response.DeleteEntityErrors = deleteEntityErrors.ToArray();
                response.Message = "The folder was not deleted because there were user maps that could not be deleted.";
            }

            return response;
        }
    }
}
