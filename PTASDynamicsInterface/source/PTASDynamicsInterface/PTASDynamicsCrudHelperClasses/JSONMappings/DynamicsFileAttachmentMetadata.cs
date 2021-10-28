// <copyright file="DynamicsFileAttachmentMetadata.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// JSON mapping of a FileAttachmentMetadata.
    /// </summary>
    public class DynamicsFileAttachmentMetadata : IFileAttachmentMetadata
  {
    /// <inheritdoc/>
    [JsonProperty("ptas_fileattachmentmetadataid")]
    public Guid Id { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_accountnumber")]
    public string AccountNumber { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_bloburl")]
    public string BlobUrl { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_documentdate")]
    public DateTime? DocumentDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_documentsize")]
    public long? DocumentSize { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_documenttype")]
    public string DocumentType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_fileextension")]
    public string FileExtension { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icscheckedoutbyusername")]
    public string ICSCheckedOutByUsername { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icscheckedoutdate")]
    public DateTime? ICScheckedoutDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icscreatedbyusername")]
    public string ICSCreatedByUsername { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icsdocumentid")]
    public Guid? ICSDocumentId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icsentereddate")]
    public DateTime? ICSEnteredDate { get; set; }

     /// <inheritdoc/>
     /// [JsonProperty("ptas_icsfileid")]
    public Guid? ICSFileId { get; set; }

     /// <inheritdoc/>
    [JsonProperty("ptas_icsfullindex")]
    public string ICSFullIndex { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icsisinworkflow")]
    public int? ICSIsInWorkflow { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_icsmodifieddate")]
    public DateTime? ICSModifiedDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_insertdate")]
    public DateTime? InsertDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_isblob")]
    public bool IsBlob { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_isilinx")]
    public bool IsIlinx { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_loginuserid")]
    public string LoginUserId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_originalfilename")]
    public string OriginalFilename { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_pagecount")]
    public int? PageCount { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_recid")]
    public string RecId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_repositoryname")]
    public string RepositoryName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_rollyear")]
    public int? RollYear { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_scandate")]
    public DateTime? ScanDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_scannerid")]
    public string ScannerId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_updatedate")]
    public DateTime? UpdateDate { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_ptas_seniorexemptionapplication_value")]
    public string SeniorExemptionApplicationId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_ptas_seniorexemptionapplicationdetail_value")]
    public string SeniorExemptionApplicationDetailId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_portaldocument")]
    public string PortalDocument { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_portalsection")]
    public string PortalSection { get; set; }
    }
}
