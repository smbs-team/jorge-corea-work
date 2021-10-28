namespace PTASMediaHelperClasses
{
  using System;
  using System.IO;
  using System.Net;
  using System.Threading.Tasks;
  using Microsoft.Azure.Storage.DataMovement;
  using Microsoft.Azure.Storage.File;

  /// <summary>
  /// Moves file list to azure.
  /// </summary>
  public class ToAzureFileShareMover : IFileCopier
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ToAzureFileShareMover"/> class.
    /// </summary>
    /// <param name="storageConnection">Connections to the storage.</param>
    /// <param name="logger">Logger for output.</param>
    public ToAzureFileShareMover(string storageConnection, ILogger logger)
    {
      if (string.IsNullOrEmpty(storageConnection))
      {
        throw new ArgumentException("Storage Connection cannot be empty", nameof(storageConnection));
      }

      this.StorageConnection = storageConnection;
      this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets storage connection string.
    /// </summary>
    private string StorageConnection { get; }

    /// <summary>
    /// Gets the current information logger.
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// Try to copy files to azure.
    /// </summary>
    /// <param name="localSource">Where is the file locally.</param>
    /// <param name="dest">Where it has to land.</param>
    /// <returns>true if the file was copied.</returns>
    public bool CopyFiles(string localSource, FileUploadTemplate dest)
    {
      var relativePath = dest.GetAzureTargetPath(localSource);

      this.Logger.OptionalOutput($" {relativePath} ");
      var storageHelper = new AzureStorageHelper(this.StorageConnection);
      ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
      TransferManager.Configurations.BlockSize = 4 * 1024 * 1024; // 4MB
      var destinationFile = storageHelper.GetCloudFileAsync("media", relativePath).Result;
      UploadOptions options = new UploadOptions();

      SingleTransferContext context = new SingleTransferContext
      {
        SetAttributesCallbackAsync = async (destination) =>
        {
          CloudFile destFile;
          await Task.FromResult(destFile = destination as CloudFile);
        },
        ShouldOverwriteCallbackAsync = async (object a, object b) =>
        {
          var fileName = (string)a;
          var blobDate = ((CloudFile)b).Properties.LastModified.Value.ToUniversalTime();
          var fileDate = new FileInfo(fileName).LastWriteTimeUtc.ToUniversalTime();
          var result = blobDate < fileDate;
          return await Task.FromResult(result);
        },
      };
      try
      {
        TransferManager.UploadAsync(localSource, destinationFile, options, context).Wait();
      }
      catch (TransferSkippedException)
      {
        return false;
      }

      return true;
    }
  }
}
