// <copyright file="BlobFileReturn.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  /// <summary>
  /// Result of fetching a blob file.
  /// </summary>
  public class BlobFileReturn
  {
    /// <summary>
    /// Gets or sets actual physical bytes.
    /// </summary>
    public byte[] FileBytes { get; set; }

    /// <summary>
    /// Gets or sets file name.
    /// </summary>
    public string FileName { get; set; }
  }
}