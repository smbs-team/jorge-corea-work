namespace CustomSearchesServicesLibrary.CustomSearches.Folder
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Folder;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Dataset folder manager.
    /// </summary>
    /// <typeparam name="Folder">The type of each folder.</typeparam>
    /// <typeparam name="CustomSearchesDbContext">The type of each dbContext.</typeparam>
    public class DatasetFolderManager : FolderManager<Folder, Dataset, CustomSearchesDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetFolderManager"/> class.
        /// </summary>
        /// <param name = "folderType">The folder type.</param>
        /// <param name = "userId">The user id.</param>
        /// <param name="dbContext">The SQL Server db context.</param>
        public DatasetFolderManager(CustomSearchFolderType folderType, Guid userId, CustomSearchesDbContext dbContext)
            : base(folderType, userId, dbContext, dbContext.Folder, dbContext.Dataset)
        {
        }

        /// <summary>
        /// Validates the folder path.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <exception cref="FolderManagerException">Invalid folder path.</exception>
        public static void ValidateFolderPath(string folderPath)
        {
            FolderManager<Folder, Dataset, CustomSearchesDbContext>.ValidateFolderPath(folderPath, allowSystemFolder: false);
        }

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <returns>
        /// The new folder.
        /// </returns>
        protected override Folder CreateFolder()
        {
            return new Folder();
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
            return folder.Dataset.Count > 0;
        }

        /// <summary>
        /// Checks if the entity can be added to the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// True if the the entity can be added to the folder, otherwise false.
        /// </returns>
        protected override bool CanBeAddedToFolder(Folder folder, Dataset entity)
        {
            return folder.Dataset.FirstOrDefault(d => d.DatasetName == entity.DatasetName) == null;
        }

        /// <summary>
        /// Removes the entity from the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        protected override void RemoveEntityFromFolder(Folder folder, Dataset entity)
        {
            folder.Dataset.Remove(entity);
        }

        /// <summary>
        /// Builds folder cache.
        /// </summary>
        /// <returns>
        /// The list of folders.
        /// </returns>
        protected override IQueryable<Folder> BuildFolderCacheQuery()
        {
            // This will retrieve all datasets for the root folder. Might be a good target for optimization.
            if (this.FolderType == CustomSearchFolderType.User)
            {
                return this.DbContext.Folder
                    .Where(f => (f.FolderType.ToLower() == this.FolderType.ToString().ToLower()) && (f.UserId == this.UserId))
                    .Include(f => f.Dataset);
            }
            else
            {
                return this.DbContext.Folder
                    .Where(f => (f.FolderType.ToLower() == this.FolderType.ToString().ToLower()))
                    .Include(f => f.Dataset);
            }
        }

        /// <summary>
        /// Checks if folder is editable.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// True if the folder is editable, otherwise false.
        /// </returns>
        protected override async Task<bool> IsEditableFolderAsync(Folder folder)
        {
            bool isEditable = await base.IsEditableFolderAsync(folder);

            if (isEditable)
            {
                // '/user/unsaved' is not editable.
                if (this.FolderType == CustomSearchFolderType.User)
                {
                    isEditable = !((folder.ParentFolderId == this.RootFolder.FolderId) && (folder.FolderName.ToLower() == "unsaved"));
                }
            }

            return isEditable;
        }
    }
}
