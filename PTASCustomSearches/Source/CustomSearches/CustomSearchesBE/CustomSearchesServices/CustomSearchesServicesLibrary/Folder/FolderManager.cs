namespace CustomSearchesServicesLibrary.Folder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Base folder manager.
    /// </summary>
    /// <typeparam name="TFolder">The type of each folder.</typeparam>
    /// <typeparam name="TFolderEntity">The type of each folder item.</typeparam>
    /// <typeparam name="TContext">The type of each dbContext.</typeparam>
    public abstract class FolderManager<TFolder, TFolderEntity, TContext> : IFolderManager<TFolder>
            where TFolder : class, IFolder<TFolder>, new()
            where TFolderEntity : class, IFolderEntity<TFolder>
            where TContext : DbContext
    {
        /// <summary>
        /// The user unsaved folder path.
        /// </summary>
        private const string UserUnsavedFolderPath = "/User/Unsaved";

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderManager{TFolder, TFolderItem, TContext}"/> class.
        /// </summary>
        /// <param name = "folderType">The folder type.</param>
        /// <param name = "userId">The user id.</param>
        /// <param name="dbContext">The SQL Server db context.</param>
        /// <param name="folderDbSet">The folder db set.</param>
        /// <param name="entityDbSet">The folder entity db set.</param>
        public FolderManager(CustomSearchFolderType folderType, Guid userId, TContext dbContext, DbSet<TFolder> folderDbSet, DbSet<TFolderEntity> entityDbSet)
        {
            this.DbContext = dbContext;
            this.FolderDbSet = folderDbSet;
            this.EntityDbSet = entityDbSet;

            this.FolderType = folderType;
            this.UserId = userId;
            this.FolderPathCache = new Dictionary<int, string>();
        }

        /// <summary>
        /// Gets or sets the folder db set.
        /// </summary>
        protected DbSet<TFolder> FolderDbSet { get; set; }

        /// <summary>
        /// Gets or sets the folder db set.
        /// </summary>
        protected DbSet<TFolderEntity> EntityDbSet { get; set; }

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        protected TContext DbContext { get; set; }

        /// <summary>
        /// Gets or sets the folder type.
        /// </summary>
        protected CustomSearchFolderType FolderType { get; set; }

        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        protected List<TFolder> Folders { get; set; }

        /// <summary>
        /// Gets or sets the folder path cache, where folder id is the key and the path is the value.
        /// </summary>
        protected Dictionary<int, string> FolderPathCache { get; set; }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        protected TFolder RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        protected Guid? UserId { get; set; }

        /// <summary>
        /// Validates the folder path.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="allowSystemFolder">Value indicating whether should be allowed the system folders.</param>
        /// <exception cref="FolderManagerException">Invalid folder path.</exception>
        public static void ValidateFolderPath(string folderPath, bool allowSystemFolder)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || (!folderPath.StartsWith("/")))
            {
                throw new FolderManagerException(
                    "The folder path should start with '/'.",
                    FolderManagerExceptionType.InvalidFolderFormat,
                    innerException: null);
            }

            CustomSearchFolderType customSearchFolderType;
            string[] folderPathSplitted = folderPath.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if ((folderPathSplitted == null) ||
                (folderPathSplitted.Length == 0) ||
                (!Enum.TryParse(folderPathSplitted[0].Trim(), ignoreCase: true, out customSearchFolderType)) ||
                (!allowSystemFolder && FolderManagerHelper.IsSystemFolderType(customSearchFolderType)))
            {
                string validTypes = allowSystemFolder ? "'user', 'shared', 'system' and 'systemrenderer'" : "'user' and 'shared'";
                throw new FolderManagerException(
                    $"Root folder {folderPath} not found (valid root folders are {validTypes}).",
                    FolderManagerExceptionType.FolderNotFound,
                    innerException: null);
            }
        }

        /// <summary>
        /// Gets the folder type.
        /// If folderPath is empty then use default type: User.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The folder type.
        /// </returns>
        public static CustomSearchFolderType GetFolderType(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return CustomSearchFolderType.User;
            }

            string[] folders = folderPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
            CustomSearchFolderType folderType = Enum.Parse<CustomSearchFolderType>(folders[0], ignoreCase: true);
            return folderType;
        }

        /// <summary>
        /// Gets folder path of the item.
        /// </summary>
        /// <param name="entity">The item to be assigned to the folder.</param>
        /// <returns>
        /// The folder path.
        /// </returns>
        public async Task<string> GetFolderPathAsync(TFolderEntity entity)
        {
            await this.BuildFolderCacheAsync();
            TFolder folder = await this.GetItemFolderAsync(entity);

            return this.GetFolderPath(folder);
        }

        /// <summary>
        /// Gets folder tree.
        /// </summary>
        /// <returns>
        /// The folder tree.
        /// </returns>
        public async Task<TFolder> GetFolderTreeAsync()
        {
            List<TFolder> folderList = await this.BuildFolderCacheQuery().Include(f => f.InverseParentFolder).ToListAsync();
            return folderList.FirstOrDefault(f => f.ParentFolderId == null);
        }

        /// <summary>
        /// Gets folder path of the item.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The folder.
        /// </returns>
        public async Task<TFolder> GetFolderAsync(string folderPath)
        {
            await this.BuildFolderCacheAsync();
            string[] folders = folderPath.Split("/", StringSplitOptions.RemoveEmptyEntries);

            TFolder parentFolder = this.RootFolder;
            for (int i = 1; i < folders.Length; i++)
            {
                string folderName = folders[i];
                TFolder currentFolder = this.Folders.FirstOrDefault(f => (f.ParentFolderId == parentFolder.FolderId) && (f.FolderName.ToLower() == folderName.ToLower()));
                if (currentFolder == null)
                {
                    return null;
                }

                parentFolder = currentFolder;
            }

            // Returns the leaf folder.
            return parentFolder;
        }

        /// <summary>
        /// Assign item to folder.
        /// If folderPath is empty then use default path: "User/Unsaved/".
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="entity">The item to be assigned to the folder.</param>
        /// <param name="modifiedBy">The user that is calling the assignment.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        /// <exception cref="CustomSearchesConflictException">The folder already has a entity with the same name.</exception>
        public async Task AssignFolderToItemAsync(string folderPath, TFolderEntity entity, Guid modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = UserUnsavedFolderPath;
            }

            TFolder previousFolder = await this.GetItemFolderAsync(entity);
            TFolder folder = await this.EnsurePathExistsAsync(folderPath);

            if (previousFolder != folder)
            {
                if (!this.CanBeAddedToFolder(folder, entity))
                {
                    throw new CustomSearchesConflictException(
                        $"The folder already has a entity with the same name.",
                        null);
                }

                entity.ParentFolder = folder;
                entity.LastModifiedBy = modifiedBy;
                entity.LastModifiedTimestamp = DateTime.UtcNow;

                if (previousFolder != null)
                {
                    this.RemoveEntityFromFolder(previousFolder, entity);
                }

                this.EntityDbSet.Update(entity);
            }

            await this.RemovesUnusedFoldersAsync(previousFolder);
        }

        /// <summary>
        /// Ensures folder path exists and returns the leaf folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The leaf folder.
        /// </returns>
        public async Task<TFolder> EnsurePathExistsAsync(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = UserUnsavedFolderPath;
            }

            await this.BuildFolderCacheAsync();
            string[] folders = folderPath.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if (this.RootFolder == null)
            {
                this.RootFolder = this.AddFolder(this.FolderType.ToString(), null);
            }

            TFolder parentFolder = this.RootFolder;

            for (int i = 1; i < folders.Length; i++)
            {
                string folderName = folders[i];
                TFolder currentFolder = this.Folders.FirstOrDefault(f => (f.ParentFolderId == parentFolder.FolderId) && (f.FolderName.ToLower() == folderName.ToLower()));
                if (currentFolder == null)
                {
                    currentFolder = this.AddFolder(folderName, parentFolder);
                }

                parentFolder = currentFolder;
            }

            // Returns the leaf folder.
            return parentFolder;
        }

        /// <summary>
        /// Removes unused folders starting from leaf folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="ignoreRemove">Value indicating if should removes the unused folders.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        public async Task RemovesUnusedFoldersAsync(TFolder folder, bool ignoreRemove = true)
        {
            await this.BuildFolderCacheAsync();

            if (!ignoreRemove)
            {
                TFolder targetFolder = folder;

                while ((targetFolder != null) && await this.IsEditableFolderAsync(targetFolder) && await this.IsFolderEmptyAsync(targetFolder))
                {
                    this.Folders.Remove(targetFolder);
                    this.FolderDbSet.Remove(folder);
                    targetFolder = this.Folders.FirstOrDefault(f => f.FolderId == folder.ParentFolderId);
                }
            }
        }

        /// <summary>
        /// Removes the empty folders.
        /// </summary>
        /// <param name="folder">The folder path.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        public async Task<bool> RemoveEmptyFoldersAsync(TFolder folder)
        {
            await this.BuildFolderCacheAsync();
            bool canBeRemoved = true;
            foreach (var currentFolder in folder.InverseParentFolder)
            {
                if (!await this.RemoveEmptyFoldersAsync(currentFolder))
                {
                    canBeRemoved = false;
                }
            }

            if (canBeRemoved && await this.IsEditableFolderAsync(folder) && await this.IsFolderEmptyAsync(folder))
            {
                this.Folders.Remove(folder);
                this.FolderDbSet.Remove(folder);
            }
            else
            {
                canBeRemoved = false;
            }

            return canBeRemoved;
        }

        /// <summary>
        /// Ensure folder path exists and returns the leaf folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The leaf folder.
        /// </returns>
        public async Task<TFolder> EnsureDefaultPathExistsAsync()
        {
            return await this.EnsurePathExistsAsync(UserUnsavedFolderPath);
        }

        /// <summary>
        /// Rename the folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="folderName">The folder name.</param>
        /// <returns>
        /// The renamed folder.
        /// </returns>
        public async Task<TFolder> RenameFolderAsync(string folderPath, string folderName)
        {
            TFolder folder = await this.ValidateFolderExistenceAsync(folderPath);

            await this.RenameFolderAsync(folder, folderName);

            return folder;
        }

        /// <summary>
        /// Validate the existence of the folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The folder.
        /// </returns>
        public async Task<TFolder> ValidateFolderExistenceAsync(string folderPath)
        {
            TFolder folder = await this.GetFolderAsync(folderPath);
            if (folder == null)
            {
                throw new FolderManagerException(
                    $"Folder {folderPath} not found.",
                    FolderManagerExceptionType.FolderNotFound,
                    innerException: null);
            }

            return folder;
        }

        /// <summary>
        /// Validate if the folder exists and it is editable.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The folder.
        /// </returns>
        /// <exception cref="FolderManagerException">Can't modify root folder or the folder was not found.</exception>
        public async Task<TFolder> ValidateEditableFolderExistenceAsync(string folderPath)
        {
            TFolder folder = await this.GetFolderAsync(folderPath);
            if (folder == null)
            {
                throw new FolderManagerException(
                    $"Folder '{folderPath}' not found.",
                    FolderManagerExceptionType.FolderNotFound,
                    innerException: null);
            }

            if (!await this.IsEditableFolderAsync(folder))
            {
                throw new FolderManagerException(
                    $"Can't modify root folder '{folderPath}'.",
                    FolderManagerExceptionType.Forbidden,
                    innerException: null);
            }

            return folder;
        }

        /// <summary>
        /// Gets the folder where is contained the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// The folder.
        /// </returns>
        public async Task<TFolder> GetItemFolderAsync(TFolderEntity item)
        {
            await this.BuildFolderCacheAsync();
            TFolder folder = this.Folders.FirstOrDefault(f => f.FolderId == item.FolderId);
            return folder;
        }

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <returns>
        /// The new folder.
        /// </returns>
        protected abstract TFolder CreateFolder();

        /// <summary>
        /// Checks if the folder has items.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// True if the folder has items, otherwise false.
        /// </returns>
        protected abstract bool FolderHasItems(TFolder folder);

        /// <summary>
        /// Checks if the entity can be added to the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// True if the the entity can be added to the folder, otherwise false.
        /// </returns>
        protected abstract bool CanBeAddedToFolder(TFolder folder, TFolderEntity entity);

        /// <summary>
        /// Removes the entity from the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="entity">The entity.</param>
        protected abstract void RemoveEntityFromFolder(TFolder folder, TFolderEntity entity);

        /// <summary>
        /// Builds folder cache.
        /// </summary>
        /// <returns>
        /// The list of folders.
        /// </returns>
        protected abstract IQueryable<TFolder> BuildFolderCacheQuery();

        /// <summary>
        /// Builds folder cache.
        /// </summary>
        /// <returns>
        /// The list of folders.
        /// </returns>
        protected async Task<List<TFolder>> BuildFolderCacheAsync()
        {
            if (this.Folders == null)
            {
                this.Folders = await this.BuildFolderCacheQuery().ToListAsync();
                this.RootFolder = this.Folders.FirstOrDefault(f => f.ParentFolderId == null);
            }

            return this.Folders;
        }

        /// <summary>
        /// Checks if folder is editable.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// True if the folder is editable, otherwise false.
        /// </returns>
        protected virtual async Task<bool> IsEditableFolderAsync(TFolder folder)
        {
            await this.BuildFolderCacheAsync();
            if (this.Folders.Contains(folder))
            {
                return folder.ParentFolderId != null;
            }

            return false;
        }

        /// <summary>
        /// Gets folder path from folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// The folder path.
        /// </returns>
        private string GetFolderPath(TFolder folder)
        {
            string folderPath = string.Empty;
            if (!this.FolderPathCache.ContainsKey(folder.FolderId))
            {
                if (folder.ParentFolderId.HasValue)
                {
                    TFolder parentFolder = this.Folders.FirstOrDefault(f => f.FolderId == folder.ParentFolderId);

                    folderPath = this.GetFolderPath(parentFolder);
                }

                folderPath += "/" + folder.FolderName;
                this.FolderPathCache[folder.FolderId] = folderPath;
            }
            else
            {
                folderPath = this.FolderPathCache[folder.FolderId];
            }

            return folderPath;
        }

        /// <summary>
        /// Rename the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="folderName">The folder name.</param>
        /// <exception cref="FolderManagerException">Can't modify root folder.</exception>
        /// <exception cref="CustomSearchesConflictException">The destination already contains a folder with the same name.</exception>
        private async Task RenameFolderAsync(TFolder folder, string folderName)
        {
            InputValidationHelper.AssertNotEmpty(folderName, nameof(folderName));

            if (await this.IsEditableFolderAsync(folder))
            {
                folderName = folderName.Trim();
                TFolder existingFolder = this.Folders
                    .FirstOrDefault(f => ((f.ParentFolderId == folder.ParentFolderId) && (f.FolderName.Trim().ToLower() == folderName.ToLower())));

                if (existingFolder != null)
                {
                    throw new CustomSearchesConflictException(
                        $"The destination already contains a folder named '{folderName}'.",
                        null);
                }

                folder.FolderName = folderName;
                this.FolderDbSet.Update(folder);
            }
            else
            {
                throw new FolderManagerException(
                    "Can't modify root folder.",
                    FolderManagerExceptionType.Forbidden,
                    innerException: null);
            }
        }

        /// <summary>
        /// Checks if the folder is empty.
        /// </summary>
        /// <param name="folder">The folder item.</param>
        /// <returns>
        /// True if the folder is empty, otherwise false.
        /// </returns>
        private async Task<bool> IsFolderEmptyAsync(TFolder folder)
        {
            await this.BuildFolderCacheAsync();
            bool containsFolders = this.Folders.Exists(f => f.ParentFolderId == folder.FolderId);
            return !containsFolders && !this.FolderHasItems(folder);
        }

        /// <summary>
        /// Checks if folder is editable.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        /// <param name="parentFolder">The parentfolder.</param>
        /// <returns>
        /// True if the folder is editable, otherwise false.
        /// </returns>
        private TFolder AddFolder(string folderName, TFolder parentFolder)
        {
            InputValidationHelper.AssertNotEmpty(folderName, nameof(folderName));

            TFolder folder = this.CreateFolder();
            folder.FolderName = folderName;
            folder.FolderType = this.FolderType.ToString();
            folder.ParentFolder = parentFolder;
            folder.UserId = this.UserId;

            this.FolderDbSet.Add(folder);
            this.Folders.Add(folder);

            return folder;
        }
    }
}
