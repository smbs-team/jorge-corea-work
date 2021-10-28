// <copyright file="BlobDocumentContainer.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Container for a blob document.
  /// </summary>
  public class BlobDocumentContainer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BlobDocumentContainer"/> class.
    /// </summary>
    /// <param name="id">Id of the new document container.</param>
    /// <param name="detailId">Id of the detail.</param>
    /// <param name="url">Url to locate this document.</param>
    /// <param name="items">File items contained.</param>
    /// <param name="document">Socument id.</param>
    /// <param name="section">Section id.</param>
    public BlobDocumentContainer(Guid id, Guid detailId, string section, string document, string url, List<BlobFileDetails> items)
    {
      this.Id = id;
      this.DetailId = detailId;
      this.Section = section;
      this.Document = document;
      this.Url = url;
      this.BlobFiles = items;
    }

    /// <summary>
    /// Gets id of this blob.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets detail Id of this blob.
    /// </summary>
    public Guid DetailId { get; }

    /// <summary>
    /// Gets the section.
    /// </summary>
    public string Section { get; }

    /// <summary>
    /// Gets the document.
    /// </summary>
    public string Document { get; }

    /// <summary>
    /// Gets location url of this blob.
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// Gets list of blob files contained in this item.
    /// </summary>
    public List<BlobFileDetails> BlobFiles { get; }
  }
}