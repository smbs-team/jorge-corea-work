// <copyright file="IFileAttachmentMetadata.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
  using System;

  /// <summary>
  /// Generic file attachment metadata template.
  /// </summary>
  public interface IFileAttachmentMetadata
    {
        /// <summary>
        /// Gets or sets AccountNumber.
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets AccountNumber.
        /// </summary>
        string BlobUrl { get; set; }

        /// <summary>
        /// Gets or sets DocumentDate.
        /// </summary>
        DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Gets or sets DocumentSize.
        /// </summary>
        long? DocumentSize { get; set; }

        /// <summary>
        /// Gets or sets DocumentType.
        /// </summary>
        string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets FileExtension.
        /// </summary>
        string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets ICSCheckedOutByUsername.
        /// </summary>
        string ICSCheckedOutByUsername { get; set; }

        /// <summary>
        /// Gets or sets ICScheckedoutDate.
        /// </summary>
        DateTime? ICScheckedoutDate { get; set; }

        /// <summary>
        /// Gets or sets ICScheckedoutDate.
        /// </summary>
        string ICSCreatedByUsername { get; set; }

        /// <summary>
        /// Gets or sets ICSDocumentId.
        /// </summary>
        Guid? ICSDocumentId { get; set; }

        /// <summary>
        /// Gets or sets ICSEnteredDate.
        /// </summary>
        DateTime? ICSEnteredDate { get; set; }

        /// <summary>
        /// Gets or sets ICSFileId.
        /// </summary>
        Guid? ICSFileId { get; set; }

        /// <summary>
        /// Gets or sets ICSFullIndex.
        /// </summary>
        string ICSFullIndex { get; set; }

        /// <summary>
        /// Gets or sets ICSIsInWorkflow.
        /// </summary>
        int? ICSIsInWorkflow { get; set; }

        /// <summary>
        /// Gets or sets ICSModifiedDate.
        /// </summary>
        DateTime? ICSModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets InsertDate.
        /// </summary>
        DateTime? InsertDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is blob?.
        /// </summary>
        bool IsBlob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is ilinx?.
        /// </summary>
        bool IsIlinx { get; set; }

        /// <summary>
        /// Gets or sets LoginUserId.
        /// </summary>
        string LoginUserId { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets OriginalFilename.
        /// </summary>
        string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets PageCount.
        /// </summary>
        int? PageCount { get; set; }

        /// <summary>
        /// Gets or sets RecId.
        /// </summary>
        string RecId { get; set; }

        /// <summary>
        /// Gets or sets RepositoryName.
        /// </summary>
        string RepositoryName { get; set; }

        /// <summary>
        /// Gets or sets RollYear.
        /// </summary>
        int? RollYear { get; set; }

        /// <summary>
        /// Gets or sets ScanDate.
        /// </summary>
        DateTime? ScanDate { get; set; }

        /// <summary>
        /// Gets or sets ScannerId.
        /// </summary>
        string ScannerId { get; set; }

        /// <summary>
        /// Gets or sets UpdateDate.
        /// </summary>
        DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets SeniorExemptionApplicationId.
        /// </summary>
        string SeniorExemptionApplicationId { get; set; }

        /// <summary>
        /// Gets or sets SeniorExemptionApplicationDetailId.
        /// </summary>
        string SeniorExemptionApplicationDetailId { get; set; }

        /// <summary>
        /// Gets or sets PortalDocument.
        /// </summary>
        string PortalDocument { get; set; }

        /// <summary>
        /// Gets or sets PortalSection.
        /// </summary>
        string PortalSection { get; set; }
    }
}