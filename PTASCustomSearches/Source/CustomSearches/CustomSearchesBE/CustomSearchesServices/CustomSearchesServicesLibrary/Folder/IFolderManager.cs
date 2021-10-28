namespace CustomSearchesServicesLibrary.Folder
{
    using System.Threading.Tasks;

    /// <summary>
    /// The folder item interface.
    /// </summary>
    /// <typeparam name="TFolder">The type of each folder.</typeparam>
    public interface IFolderManager<TFolder>
         where TFolder : class, new()
    {
        /// <summary>
        /// Gets folder tree.
        /// </summary>
        /// <returns>
        /// The folder tree.
        /// </returns>
        Task<TFolder> GetFolderTreeAsync();

        /// <summary>
        /// Ensures folder path exists and returns the leaf folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>
        /// The leaf folder.
        /// </returns>
        Task<TFolder> EnsurePathExistsAsync(string folderPath);

        /// <summary>
        /// Rename the folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="folderName">The folder name.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        Task<TFolder> RenameFolderAsync(string folderPath, string folderName);
    }
}
