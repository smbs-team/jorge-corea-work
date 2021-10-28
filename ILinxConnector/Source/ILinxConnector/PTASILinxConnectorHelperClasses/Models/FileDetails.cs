// <copyright file="FileDetails.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASLinxConnectorHelperClasses.Models
{
  using Newtonsoft.Json;

  /// <summary>
  /// Details for each ILinx file returned.
  /// </summary>
  public class FileDetails
  {
    /// <summary>
    /// Gets or sets id of the file.
    /// </summary>
    public string FileId { get; set; }

    /// <summary>
    /// Gets or sets name of this file.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets file extension.
    /// </summary>
    public string FileExtension { get; set; }

    /// <summary>
    /// Gets or sets file size.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Gets or sets actual bytes of the file.
    /// </summary>
    /// <remarks>Will be encoded as base64.</remarks>
    [JsonIgnore]
    public byte[] FileBytes { get; set; }
  }
}