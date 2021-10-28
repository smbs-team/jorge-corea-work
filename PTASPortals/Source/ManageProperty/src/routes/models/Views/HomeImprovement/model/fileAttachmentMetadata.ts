// fileAttachmentMetadata.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface FileAttachmentMetadataEntity {
  ptas_fileattachmentmetadataid: string; //Primary key
  ptas_name: string;
  _ptas_parcelid_value: string;
  _ptas_attachmentid_value: string; //Unique identifier for Home Improvement Application associated with File attachment
  ptas_bloburl: string;
  ptas_isblob: boolean;
  ptas_issharepoint: boolean;
  ptas_sharepointurl: string;
}

export class FileAttachmentMetadata {
  fileAttachmentMetadataId: string; //Primary key
  name: string;
  parcelId: string;
  homeImprovementApplicationId: string;
  blobUrl: string;
  isBlob: boolean;
  isSharePoint: boolean;
  sharepointUrl: string;

  constructor(entity?: FileAttachmentMetadataEntity) {
    this.fileAttachmentMetadataId = entity?.ptas_fileattachmentmetadataid ?? '';
    this.name = entity?.ptas_name ?? '';
    this.parcelId = entity?._ptas_parcelid_value ?? '';
    this.homeImprovementApplicationId = entity?._ptas_attachmentid_value ?? '';
    this.blobUrl = entity?.ptas_bloburl ?? '';
    this.isBlob = entity?.ptas_isblob ?? true;
    this.isSharePoint = entity?.ptas_issharepoint ?? false;
    this.sharepointUrl = entity?.ptas_sharepointurl ?? '';
  }
}
