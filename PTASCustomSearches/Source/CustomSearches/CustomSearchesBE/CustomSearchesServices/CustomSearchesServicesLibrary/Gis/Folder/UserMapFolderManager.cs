namespace CustomSearchesServicesLibrary.Gis.Folder
{
    using System;
    using System.Linq;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Folder;
    using CustomSearchesServicesLibrary.Gis.Enumeration;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Dataset folder manager.
    /// </summary>
    /// <typeparam name="Folder">The type of each folder.</typeparam>
    /// <typeparam name="CustomSearchesDbContext">The type of each dbContext.</typeparam>
    public class UserMapFolderManager : FolderManager<Folder, UserMap, GisDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapFolderManager"/> class.
        /// </summary>
        /// <param name="folderType">The folder type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="dbContext">The SQL Server db context.</param>
        public UserMapFolderManager(CustomSearchFolderType folderType, Guid userId, GisDbContext dbContext)
            : base(folderType, userId, dbContext, dbContext.Folder, dbContext.UserMap)
        {
        }

        /// <summary>
        /// Validates the folder path.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <exception cref="FolderManagerException">Invalid folder path.</exception>
        public static void ValidateFolderPath(string folderPath)
        {
            FolderManager<Folder, UserMap, GisDbContext>.ValidateFolderPath(folderPath, allowSystemFolder: true);
        }

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <returns>
        /// The new folder.
        /// </returns>
        protected override Folder CreateFolder()
        {
            return new Folder
            {
                FolderItemType = GisFolderItemType.UserMap.ToString()
            };
        }

        /// <summary>
        /// Checks if the folder has items.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// True if the folder has items, otherwise false.
        /// </returns>
        protected override bool FolderHasItems(Folder folder)
        {
            return folder.UserMap.Count > 0;
        }

        /// <summary>
        /// Checks if the entity can be added to the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// True if the the entity can be added to the folder, otherwise false.
        /// </returns>
        protected override bool CanBeAddedToFolder(Folder folder, UserMap entity)
        {
            return folder.UserMap.FirstOrDefault(m => m.UserMapName == entity.UserMapName) == null;
        }

        /// <summary>
        /// Removes the entity from the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        protected override void RemoveEntityFromFolder(Folder folder, UserMap entity)
        {
            folder.UserMap.Remove(entity);
        }

        /// <summary>
        /// Builds folder cache.
        /// </summary>
        /// <returns>
        /// The list of folders.
        /// </returns>
        protected override IQueryable<Folder> BuildFolderCacheQuery()
        {
            // This will retrieve all user map for the root folder. Might be a good target for optimization.
            if (this.FolderType == CustomSearchFolderType.User)
            {
                return this.DbContext.Folder
                     .Where(f =>
                     (f.FolderItemType.ToLower() == GisFolderItemType.UserMap.ToString().ToLower())
                     && (f.FolderType.ToLower() == this.FolderType.ToString().ToLower())
                     && (f.UserId == this.UserId))
                     .Include(f => f.UserMap);
            }
            else
            {
                return this.DbContext.Folder
                    .Where(f =>
                    (f.FolderItemType.ToLower() == GisFolderItemType.UserMap.ToString().ToLower())
                    && (f.FolderType.ToLower() == this.FolderType.ToString().ToLower()))
                    .Include(f => f.UserMap);
            }
        }
    }
}
