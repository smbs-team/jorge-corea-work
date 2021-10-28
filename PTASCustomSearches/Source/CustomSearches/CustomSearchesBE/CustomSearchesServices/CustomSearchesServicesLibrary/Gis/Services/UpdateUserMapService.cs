namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that updates the user map.
    /// </summary>
    public class UpdateUserMapService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserMapService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UpdateUserMapService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Updates the user map.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userMapData">User map data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task UpdateUserMap(GisDbContext dbContext, UserMapData userMapData)
        {
            UserMapFolderManager.ValidateFolderPath(userMapData.FolderPath);

            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == userMapData.UserMapId)
                .Include(m => m.MapRenderer)
                .Include(m => m.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userMap, nameof(userMap), userMapData.UserMapId);

            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;

            if (userMap.IsLocked != userMapData.IsLocked)
            {
                this.ServiceContext.AuthProvider.AuthorizeCurrentUserOrAdminRole(userMap.CreatedBy, "UpdateUserMap");
            }
            else
            {
                this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, userMap.ParentFolder, userMap.IsLocked, "UpdateUserMap");
            }

            InputValidationHelper.AssertNotEmpty(userMapData.UserMapName, nameof(userMapData.UserMapName));

            string previousName = userMap.UserMapName.Trim();
            userMap.UserMapName = userMapData.UserMapName.Trim();

            Folder previousFolder = userMap.ParentFolder;

            if (!string.IsNullOrWhiteSpace(userMapData.FolderPath))
            {
                CustomSearchFolderType prevFolderType = Enum.Parse<CustomSearchFolderType>(userMap.ParentFolder.FolderType, ignoreCase: true);
                CustomSearchFolderType newFolderType = UserMapFolderManager.GetFolderType(userMapData.FolderPath);
                this.ServiceContext.AuthProvider.AuthorizeChangeItemFolderOperation(userMap.CreatedBy, prevFolderType, newFolderType, "UpdateUserMap");

                UserMapFolderManager prevFolderManager = null;
                Folder prevFolder = null;
                if (prevFolderType != newFolderType)
                {
                    prevFolderManager = new UserMapFolderManager(prevFolderType, userMap.CreatedBy, dbContext);
                    prevFolder = await prevFolderManager.GetItemFolderAsync(userMap);
                    prevFolder.UserMap.Remove(userMap);
                }

                UserMapFolderManager folderManager = new UserMapFolderManager(newFolderType, userMap.CreatedBy, dbContext);

                // If user map change of folder and the name is repeated throw an exception.
                await folderManager.AssignFolderToItemAsync(userMapData.FolderPath, userMap, userMap.CreatedBy);

                if (prevFolderType != newFolderType)
                {
                    await prevFolderManager.RemovesUnusedFoldersAsync(prevFolder);
                }
            }

            // If user map does not change of folder and the name is repeated throw an exception.
            if ((previousFolder == userMap.ParentFolder) && (previousName.ToLower() != userMap.UserMapName.ToLower()))
            {
                int repeatedNameCount = await dbContext.UserMap
                    .CountAsync(m => m.ParentFolderId == userMap.ParentFolderId && m.UserMapName.Trim().ToLower() == userMap.UserMapName.Trim().ToLower());
                if (repeatedNameCount > 0)
                {
                    throw new CustomSearchesConflictException(
                        $"The folder already has an entity with the same name.",
                        null);
                }
            }

            userMap.IsLocked = userMapData.IsLocked;
            userMap.LastModifiedBy = userId;
            userMap.LastModifiedTimestamp = DateTime.UtcNow;

            dbContext.UserMap.Update(userMap);

            foreach (var item in userMap.MapRenderer)
            {
                dbContext.MapRenderer.Remove(item);
            }

            userMap.MapRenderer.Clear();

            if (userMapData.MapRenderers != null)
            {
                foreach (var item in userMapData.MapRenderers)
                {
                    MapRenderer mapRenderer = item.ToEfModel();
                    mapRenderer.CreatedBy = userId;
                    mapRenderer.CreatedTimestamp = DateTime.UtcNow;
                    mapRenderer.LastModifiedBy = userId;
                    mapRenderer.LastModifiedTimestamp = DateTime.UtcNow;
                    mapRenderer.UserMap = userMap;

                    dbContext.MapRenderer.Add(mapRenderer);
                }
            }

            await dbContext.ValidateAndSaveChangesAsync();
        }
    }
}
