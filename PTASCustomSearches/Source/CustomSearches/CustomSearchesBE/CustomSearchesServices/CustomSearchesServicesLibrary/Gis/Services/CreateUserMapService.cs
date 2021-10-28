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
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that creates the user map.
    /// </summary>
    public class CreateUserMapService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserMapService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public CreateUserMapService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Creates the user map.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userMapData">User map data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<int> CreateUserMap(GisDbContext dbContext, UserMapData userMapData)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id;
            userMapData.CreatedBy = userId;
            userMapData.LastModifiedBy = userId;

            UserMapFolderManager.ValidateFolderPath(userMapData.FolderPath);
            InputValidationHelper.AssertNotEmpty(userMapData.UserMapName, nameof(userMapData.UserMapName));

            CustomSearchFolderType folderType = UserMapFolderManager.GetFolderType(userMapData.FolderPath);
            UserMapFolderManager folderManager = new UserMapFolderManager(folderType, userId, dbContext);
            Folder folder = await folderManager.EnsurePathExistsAsync(userMapData.FolderPath);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userId, folder, isLocked: false, "CreateUserMap");

            int repeatedNameCount = await dbContext.UserMap
                .CountAsync(m => m.ParentFolderId == folder.ParentFolderId && m.UserMapName.Trim().ToLower() == userMapData.UserMapName.Trim().ToLower());
            if (repeatedNameCount > 0)
            {
                throw new CustomSearchesConflictException(
                    $"The folder already has an entity with the same name.",
                    null);
            }

            UserMap userMap = userMapData.ToEfModel();
            userMap.CreatedBy = userId;
            userMap.CreatedTimestamp = DateTime.UtcNow;
            userMap.LastModifiedBy = userId;
            userMap.LastModifiedTimestamp = DateTime.UtcNow;
            userMap.ParentFolder = folder;
            userMap.UserMapId = 0;

            HashSet<string> rendererNames = new HashSet<string>();
            foreach (var mapRenderer in userMap.MapRenderer)
            {
                if (rendererNames.Contains(mapRenderer.MapRendererName))
                {
                    throw new CustomSearchesRequestBodyException(
                        $"Each map renderer should have a different '{nameof(mapRenderer.MapRendererName)}'",
                        innerException: null);
                }

                rendererNames.Add(mapRenderer.MapRendererName);

                mapRenderer.CreatedBy = userId;
                mapRenderer.CreatedTimestamp = DateTime.UtcNow;
                mapRenderer.LastModifiedBy = userId;
                mapRenderer.LastModifiedTimestamp = DateTime.UtcNow;
                mapRenderer.MapRendererId = 0;
            }

            dbContext.UserMap.Add(userMap);

            await dbContext.ValidateAndSaveChangesAsync();

            return userMap.UserMapId;
        }
    }
}
