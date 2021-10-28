// fileAttachment.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface FileAttachmentMetadataEntity {
  ptas_fileattachmentmetadataid: string; //Primary key
  ptas_name: string;
  _ptas_parcelid_value: string;
  ptas_bloburl: string;
  ptas_portaldocument: string;
  ptas_portalsection: string;
  ptas_icsdocumentid: string;
  ptas_isblob: boolean;
  ptas_issharepoint: boolean;
  ptas_sharepointurl: string;
  ptas_originalfilename: string;
  ptas_filelibrary: number;
}

export class FileAttachmentMetadataTask {
  fileAttachmentMetadataId: string; //Primary key
  name: string;
  parcelId: string;
  blobUrl: string;
  document: string;
  section: string;
  icsDocumentId: string;
  isBlob: boolean;
  isSharePoint: boolean;
  sharepointUrl: string;
  originalName: string;
  fileLibrary: number | undefined;

  constructor(entity?: FileAttachmentMetadataEntity) {
    this.fileAttachmentMetadataId = entity?.ptas_fileattachmentmetadataid ?? '';
    this.name = entity?.ptas_name ?? '';
    this.parcelId = entity?._ptas_parcelid_value ?? '';
    this.blobUrl = entity?.ptas_bloburl ?? '';
    this.document = entity?.ptas_portaldocument ?? '';
    this.section = entity?.ptas_portalsection ?? '';
    this.icsDocumentId = entity?.ptas_icsdocumentid ?? '';
    this.isBlob = entity?.ptas_isblob ?? false;
    this.isSharePoint = entity?.ptas_issharepoint ?? false;
    this.sharepointUrl = entity?.ptas_sharepointurl ?? '';
    this.originalName = entity?.ptas_originalfilename ?? '';
    this.fileLibrary = entity?.ptas_filelibrary;
  }
}
