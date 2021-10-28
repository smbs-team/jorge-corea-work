// <copyright file="JsonBlob.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  /// <summary>
  /// Result from a json blob.
  /// </summary>
  public class JsonBlob
  {
    /// <summary>
    /// Gets or sets route to this item.
    /// </summary>
    public string Route { get; set; }

    /// <summary>
    /// Gets or sets content of the blob.
    /// </summary>
    public string Content { get; set; }
  }
}