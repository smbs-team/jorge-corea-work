namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Service that renames the user map.
    /// </summary>
    public class RenameUserMapService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameUserMapService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public RenameUserMapService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Renames the user map.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userMapId">User map id.</param>
        /// <param name="userMapName">User map name.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RenameUserMap(GisDbContext dbContext, int userMapId, string userMapName)
        {
            UserMap userMap = await dbContext.UserMap
                .Where(m => m.UserMapId == userMapId)
                .Include(m => m.ParentFolder)
                .FirstOrDefaultAsync();

            InputValidationHelper.AssertEntityExists(userMap, nameof(userMap), userMapId);

            this.ServiceContext.AuthProvider.AuthorizeFolderItemOperation<Folder>(userMap.CreatedBy, userMap.ParentFolder, userMap.IsLocked, "RenameUserMap");

            InputValidationHelper.AssertNotEmpty(userMapName, nameof(userMapName));
            userMapName = userMapName.Trim();

            if (userMap.UserMapName.Trim().ToLower() != userMapName.ToLower())
            {
                int repeatedNameCount = await dbContext.UserMap
                    .CountAsync(m => m.ParentFolderId == userMap.ParentFolderId && m.UserMapName.Trim().ToLower() == userMapName.ToLower());

                if (repeatedNameCount > 0)
                {
                    throw new CustomSearchesConflictException(
                        $"The folder already has a entity with the same name.",
                        null);
                }

                userMap.UserMapName = userMapName;
                userMap.LastModifiedTimestamp = DateTime.UtcNow;
                userMap.LastModifiedBy = this.ServiceContext.AuthProvider.UserInfoData.Id;

                dbContext.UserMap.Update(userMap);
                await dbContext.ValidateAndSaveChangesAsync();
            }
        }
    }
}
