// fileAttachment.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface FileAttachmentMetadataEntityFields {
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
}

export class FileAttachmentMetadataClass {
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

  constructor(entity?: FileAttachmentMetadataEntityFields) {
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
  }

  getEntityFields = (): FileAttachmentMetadataEntityFields => {
    return {
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_fileattachmentmetadataid: this.fileAttachmentMetadataId,
      ptas_name: this.name,
      _ptas_parcelid_value: this.parcelId,
      ptas_bloburl: this.blobUrl,
      ptas_portaldocument: this.document,
      ptas_portalsection: this.section,
      ptas_icsdocumentid: this.icsDocumentId,
      ptas_isblob: this.isBlob,
      ptas_issharepoint: this.isSharePoint,
      ptas_sharepointurl: this.sharepointUrl,
      /* eslint-enable @typescript-eslint/camelcase */
    };
  };
}
