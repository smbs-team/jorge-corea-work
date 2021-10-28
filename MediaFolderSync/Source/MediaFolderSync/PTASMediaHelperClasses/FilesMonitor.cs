namespace PTASMediaHelperClasses
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  /// <summary>
  /// Enumerates and monitors files to copy when changed.
  /// </summary>
  public class FilesMonitor : IFilesHandler
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FilesMonitor"/> class.
    /// </summary>
    /// <param name="provider">File provider to use.</param>
    /// <param name="logger">Output logger.</param>
    public FilesMonitor(IFilesToProcessProvider provider, ILogger logger)
    {
      this.Provider = provider ?? throw new ArgumentNullException(nameof(provider));
      this.Logger = logger;
    }

    /// <summary>
    /// Gets files provider for this object.
    /// </summary>
    private IFilesToProcessProvider Provider { get; }

    private ILogger Logger { get; }

    private Action<string, FileUploadTemplate> OnCall { get; set; }

    /// <inheritdoc/>
    public void Run(Action<string, FileUploadTemplate> onCall)
    {
      this.OnCall = onCall;
      var inputFiles = this.Provider.GetFiles().ToList();
      foreach (var inputFile in inputFiles)
      {
        string[] files = this.GetFiles(inputFile);
        foreach (var file in files)
        {
          try
          {
            this.OnCall(file, inputFile);
          }
          catch (Exception ex)
          {
            this.Logger.WriteError(ex.Message);
          }
        }
      }
    }

    /// <summary>
    /// Searches on a directory for matching files.
    /// example: abc*.* will produce abc.jpg, abc-small.jpg and abc-med.jpg.
    /// Since the upload template can have wildcards and
    /// we are checking against the file system, we need to
    /// expand the wildcard search if necesary.
    /// </summary>
    /// <param name="inputFile">Template to get the files from.</param>
    /// <returns>List of file names.</returns>
    protected virtual string[] GetFiles(FileUploadTemplate inputFile)
    {
      return Directory.GetFiles(inputFile.Path, inputFile.Filter, inputFile.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }
  }
}