// <copyright file="DeleteBatchResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
  using System.Collections.Generic;
  using PTASIlinxService.Classes;

  /// <summary>
  /// Result of a batch deletion.
  /// </summary>
  public class DeleteBatchResult
  {
    /// <summary>
    /// Gets or sets result of the deletion.
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// Gets or sets file per file delete attempt result.
    /// </summary>
    public IEnumerable<DeleteResult> Files { get; set; }
  }
}