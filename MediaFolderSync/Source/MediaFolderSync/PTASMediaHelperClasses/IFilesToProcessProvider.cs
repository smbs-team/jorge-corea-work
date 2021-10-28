namespace PTASMediaHelperClasses
{
  using System.Collections.Generic;

  /// <summary>
  /// Returns a list of files and/or wildcards to process.
  /// </summary>
  public interface IFilesToProcessProvider
  {
    /// <summary>
    /// Returns the list of files to work on.
    /// </summary>
    /// <returns>Files or wildcards to work on.</returns>
    IEnumerable<FileUploadTemplate> GetFiles();
  }
}
