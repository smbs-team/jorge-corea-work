// <copyright file="DeleteResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using Newtonsoft.Json;

  /// <summary>
  /// Result of deletion.
  /// </summary>
  public class DeleteResult
  {
    /// <summary>
    /// Gets or sets path of deleted object.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether result of deletion.
    /// </summary>
    public bool Deleted { get; set; }
  }
}