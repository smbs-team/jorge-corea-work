namespace PTASMediaHelperClasses
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  /// <summary>
  /// Handles a list of files or wildcards.
  /// Executes the onCall action on each of the files
  /// produced by the FileUploadTemplate.
  /// </summary>
  public interface IFilesHandler
  {
    /// <summary>
    /// Class that will fetch and process a list of file templates.
    /// the onCall parameter is the callback to what has to be executed
    /// for each fetched file.
    /// </summary>
    /// <param name="onCall">What to call for each file template.</param>
    void Run(Action<string, FileUploadTemplate> onCall);
  }
}