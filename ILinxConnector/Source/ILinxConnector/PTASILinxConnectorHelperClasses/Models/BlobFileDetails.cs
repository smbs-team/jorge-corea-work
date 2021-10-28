// <copyright file="BlobFileDetails.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using Microsoft.WindowsAzure.Storage.Blob;
  using Newtonsoft.Json;

  /// <summary>
  /// Details for a blob file.
  /// </summary>
  public class BlobFileDetails
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BlobFileDetails"/> class.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="blob">Initial blob.</param>
    /// <param name="bytes">Actual byte array.</param>
    public BlobFileDetails(string fileName, CloudBlockBlob blob, byte[] bytes)
    {
      this.FileName = fileName;
      this.Blob = blob;
      this.Bytes = bytes;
    }

    /// <summary>
    /// Gets file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the containing blob.
    /// </summary>
    [JsonIgnore]
    public CloudBlockBlob Blob { get; }

    /// <summary>
    /// Gets actual bytes of the blob.
    /// </summary>
    [JsonIgnore]
    public byte[] Bytes { get; }
  }
}