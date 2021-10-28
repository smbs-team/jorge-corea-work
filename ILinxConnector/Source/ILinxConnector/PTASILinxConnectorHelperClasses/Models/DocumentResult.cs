// <copyright file="DocumentResult.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASLinxConnectorHelperClasses.Models
{
  using System.Collections.Generic;

  /// <summary>
  /// Result sent back when a document is requested.
  /// </summary>
  public class DocumentResult
  {
    /// <summary>
    /// Gets or sets id of the document.
    /// </summary>
    public string DocumentId { get; set; }

    /// <summary>
    /// Gets or sets list of files contained by the document.
    /// </summary>
    public IEnumerable<FileDetails> Files { get; set; }

    /// <summary>
    /// Gets or sets number of files contained.
    /// </summary>
    public int FileCount { get; set; }
  }
}