// <copyright file="DBRunner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PTASMediaDBLoaderClasses
{
  using System;

  using PTASMediaHelperClasses;

  /// <summary>
  /// Orchestrates the moving of newly modified files to the azure
  /// storage file share.
  /// </summary>
  public class DBRunner
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DBRunner" /> class.
    /// </summary>
    /// <param name="mover">File Mover.</param>
    /// <param name="rootFolder">Start Folder.</param>
    /// <param name="connectionString">Connection to DB.</param>
    /// <param name="logger">Logger object.</param>
    public DBRunner(IFileCopier mover, string rootFolder, string connectionString, ILogger logger)
    {
      if (string.IsNullOrEmpty(rootFolder))
      {
        throw new ArgumentException("Rootfolder was not provided.", nameof(rootFolder));
      }

      if (string.IsNullOrEmpty(connectionString))
      {
        throw new ArgumentException("Connection String was not provided", nameof(connectionString));
      }

      this.Mover = mover ?? throw new ArgumentNullException(nameof(mover));
      this.RootFolder = rootFolder;
      this.ConnectionString = connectionString;
      this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the mover this runner uses to move the files.
    /// </summary>
    private IFileCopier Mover { get; }

    /// <summary>
    /// Gets the root folder for the files.
    /// </summary>
    private string RootFolder { get; }

    /// <summary>
    /// Gets the database connection string.
    /// </summary>
    private string ConnectionString { get; }

    /// <summary>
    /// Gets who we are using to log our output.
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// Get and process all database records after a given time.
    /// </summary>
    /// <param name="startTime">Time limit.</param>
    /// <param name="queryToRun">Query to fecth the pending data.</param>
    public void Run(DateTime startTime, string queryToRun)
    {
      var count = 0;
      this.Logger.OptionalOutput("Running DB Loader");

      // for this files monitor we don't want watchers because there are literally millions
      // and this is just a db checker, does not rely on file system to know a file changed
      // first run forces the monitor to go over all the matching files once.
      IFilesToProcessProvider fileProvider = this.CreateFilesToProcessProvider(startTime, queryToRun);
      IFilesHandler filesHandler = this.CreateFilesHandler(fileProvider);
      filesHandler.Run(
        (fullPath, info) =>
        {
          this.Logger.OptionalOutput(info.ToString() + " " + (++count).ToString());
          if (!this.Mover.CopyFiles(fullPath, info))
          {
            this.Logger.OptionalOutput("NOT COPIED ALREADY EXISTED");
          }
          else
          {
            this.Logger.WriteInfo($"Moved file {fullPath} succesfully.");
          }
        });
    }

    /// <summary>
    /// Create a file to process source. Helps with decoupling from actual provider.
    /// </summary>
    /// <param name="startTime">Starting date of items to fetch.</param>
    /// <param name="queryToRun">Query to fetch the pending data.</param>
    /// <returns>A list of files for processing.</returns>
    protected virtual IFilesToProcessProvider CreateFilesToProcessProvider(DateTime startTime, string queryToRun)
    {
      return new DBLoader(this.ConnectionString, startTime, this.RootFolder, this.Logger, queryToRun);
    }

    /// <summary>
    /// Gets the file handler specific to this operation. Allows for later decoupling and testing.
    /// </summary>
    /// <param name="dbLoader">Loader to use.</param>
    /// <returns>The file handler.</returns>
    protected virtual IFilesHandler CreateFilesHandler(IFilesToProcessProvider dbLoader)
    {
      return new FilesMonitor(dbLoader, this.Logger);
    }
  }
}
