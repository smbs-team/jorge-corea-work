namespace PTASMediaHelperClasses
{
  using System.Timers;

  /// <summary>
  /// Generic File mover.
  /// </summary>
  public interface IFileCopier
  {
    /// <summary>
    /// Generic File mover.
    /// </summary>
    /// <param name="localSource">Where find the file locally.</param>
    /// <param name="destination">Where to copy to.</param>
    /// <returns>could copy.</returns>
    bool CopyFiles(string localSource, FileUploadTemplate destination);
  }
}
