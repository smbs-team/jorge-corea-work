// <copyright file="FormFileAttachmentMetadata.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// JSON mapping of a FileAttachmentMetadata for input or output.
    /// </summary>
    public class FormFileAttachmentMetadata : FormInput, IFileAttachmentMetadata
    {
        /// <inheritdoc/>
        public string AccountNumber { get; set; }

        /// <inheritdoc/>
        public string BlobUrl { get; set; }

        /// <inheritdoc/>
        public DateTime? DocumentDate { get; set; }

        /// <inheritdoc/>
        public long? DocumentSize { get; set; }

        /// <inheritdoc/>
        public string DocumentType { get; set; }

        /// <inheritdoc/>
        public string FileExtension { get; set; }

        /// <inheritdoc/>
        public string ICSCheckedOutByUsername { get; set; }

        /// <inheritdoc/>
        public DateTime? ICScheckedoutDate { get; set; }

        /// <inheritdoc/>
        public string ICSCreatedByUsername { get; set; }

        /// <inheritdoc/>
        public Guid? ICSDocumentId { get; set; }

        /// <inheritdoc/>
        public DateTime? ICSEnteredDate { get; set; }

        /// <inheritdoc/>
        public Guid? ICSFileId { get; set; }

        /// <inheritdoc/>
        public string ICSFullIndex { get; set; }

        /// <inheritdoc/>
        public int? ICSIsInWorkflow { get; set; }

        /// <inheritdoc/>
        public DateTime? ICSModifiedDate { get; set; }

        /// <inheritdoc/>
        public Guid Id { get; set; } = default;

        /// <inheritdoc/>
        public DateTime? InsertDate { get; set; }

        /// <inheritdoc/>
        public bool IsBlob { get; set; }

        /// <inheritdoc/>
        public bool IsIlinx { get; set; }

        /// <inheritdoc/>
        public string LoginUserId { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string OriginalFilename { get; set; }

        /// <inheritdoc/>
        public int? PageCount { get; set; }

        /// <inheritdoc/>
        public string RecId { get; set; }

        /// <inheritdoc/>
        public string RepositoryName { get; set; }

        /// <inheritdoc/>
        public int? RollYear { get; set; }

        /// <inheritdoc/>
        public DateTime? ScanDate { get; set; }

        /// <inheritdoc/>
        public string ScannerId { get; set; }

        /// <inheritdoc/>
        public DateTime? UpdateDate { get; set; }

        /// <inheritdoc/>
        public string SeniorExemptionApplicationId { get; set; }

        /// <inheritdoc/>
        public string SeniorExemptionApplicationDetailId { get; set; }

        /// <inheritdoc/>
        public string PortalDocument { get; set; }

        /// <inheritdoc/>
        public string PortalSection { get; set; }

        /// <summary>
        /// Sets the id for a new object.
        /// </summary>
        public override void SetId()
        {
            if (this.Id.Equals(default))
            {
                this.Id = Guid.NewGuid();
            }
        }
    }
}