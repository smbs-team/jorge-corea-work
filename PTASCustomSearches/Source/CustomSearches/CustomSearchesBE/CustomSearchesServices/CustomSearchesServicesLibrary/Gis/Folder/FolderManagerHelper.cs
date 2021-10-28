namespace CustomSearchesServicesLibrary.Gis.Folder
{
    using System;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Folder;
    using CustomSearchesServicesLibrary.Gis.Enumeration;

    /// <summary>
    /// Folder Manager helper.
    /// </summary>
    public class FolderManagerHelper
    {
        /// <summary>
        /// Creates the folder manager.
        /// </summary>
        /// <param name="folderItemType">The folder item type.</param>
        /// <param name="folderType">The folder type.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="dbContext">The SQL Server db context.</param>
        /// <exception cref="NotSupportedException">Operation not supported for folderItemType.</exception>
        /// <returns>The folder manager.</returns>
        public static IFolderManager<Folder> CreateFolderManager(GisFolderItemType folderItemType, CustomSearchFolderType folderType, Guid userId, GisDbContext dbContext)
        {
            IFolderManager<Folder> folderManager;
            switch (folderItemType)
            {
                case GisFolderItemType.UserMap:
                    folderManager = new UserMapFolderManager(folderType, userId, dbContext);
                    break;
                default:
                    throw new NotSupportedException($"Operation not supported for {nameof(folderItemType)}: '{folderItemType}'.");
            }

            return folderManager;
        }

        /// <summary>
        /// Checks if the type is System or SystemRenderer.
        /// </summary>
        /// <param name="folderType">The folder type.</param>
        /// <returns>
        /// True if the type is System or SystemRenderer, otherwise false.
        /// </returns>
        public static bool IsSystemFolderType(CustomSearchFolderType folderType)
        {
            return folderType == CustomSearchFolderType.System || folderType == CustomSearchFolderType.SystemRenderer;
        }

        /// <summary>
        /// Checks if the type is System or SystemRenderer.
        /// </summary>
        /// <param name="folderType">The folder type.</param>
        /// <returns>
        /// True if the type is System or SystemRenderer, otherwise false.
        /// </returns>
        public static bool IsSystemFolderType(string folderType)
        {
            CustomSearchFolderType customSearchFolderType;
            if (Enum.TryParse<CustomSearchFolderType>(folderType, ignoreCase: true, out customSearchFolderType))
            {
                return IsSystemFolderType(customSearchFolderType);
            }

            return false;
        }
    }
}
