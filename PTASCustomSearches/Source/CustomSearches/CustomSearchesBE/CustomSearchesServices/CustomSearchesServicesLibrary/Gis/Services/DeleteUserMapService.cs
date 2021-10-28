namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that deletes the user map.
    /// </summary>
    public class DeleteUserMapService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserMapService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public DeleteUserMapService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Deletes the user map.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userMapId">User map id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task DeleteUserMap(GisDbContext dbContext, int userMapId)
        {
            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == userMapId)
                .Include(m => m.ParentFolder)
                .Include(m => m.UserMapSelection)
                .Include(m => m.MapRenderer)
                .Include(m => m.UserMapCategoryUserMap)
                .FirstOrDefaultAsync();

            Folder folder = userMap.ParentFolder;

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userMap.CreatedBy, folder, userMap.IsLocked, "DeleteUserMap");

            folder.UserMap.Remove(userMap);
            CustomSearchFolderType folderType = Enum.Parse<CustomSearchFolderType>(folder.FolderType, ignoreCase: true);
            UserMapFolderManager folderManager = new UserMapFolderManager(folderType, userMap.CreatedBy, dbContext);
            await folderManager.RemovesUnusedFoldersAsync(folder);
            dbContext.UserMapSelection.RemoveRange(userMap.UserMapSelection);
            dbContext.MapRenderer.RemoveRange(userMap.MapRenderer);
            dbContext.UserMapCategoryUserMap.RemoveRange(userMap.UserMapCategoryUserMap);

            dbContext.UserMap.Remove(userMap);
            await dbContext.SaveChangesAsync();
        }
    }
}
