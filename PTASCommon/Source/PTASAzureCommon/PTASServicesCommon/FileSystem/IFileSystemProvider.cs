namespace PTASServicesCommon.FileSystem
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface that provides file system operations.
    /// </summary>
    public interface IFileSystemProvider
    {
        /// <summary>
        /// Reads all text in the file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>The text in the file.</returns>
        Task<string> ReadAllTextAsync(string path);
    }
}
