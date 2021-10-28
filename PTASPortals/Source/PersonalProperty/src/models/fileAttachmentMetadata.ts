// fileAttachment.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { omit } from 'lodash';

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
  ptas_documenttype: string;
  ptas_filingmethod: number;
  ptas_listingstatus: number;
  ptas_yearid: string;
  ptas_loaddate: string;
  ptas_loadbyid: string;
}

export class FileAttachmentMetadata {
  id: string; //Primary key
  name: string;
  parcelId: string;
  blobUrl: string;
  document: string;
  section: string;
  icsDocumentId: string;
  isBlob: boolean;
  isSharePoint: boolean;
  sharepointUrl: string;
  documentType: string;
  filingMethod: number; // type ezListing
  listingStatus: number; // Received
  year: string;
  loadDate: string;
  loadBy: string;

  constructor(entity?: FileAttachmentMetadataEntityFields) {
    this.id = entity?.ptas_fileattachmentmetadataid ?? '';
    this.name = entity?.ptas_name ?? '';
    this.parcelId = entity?._ptas_parcelid_value ?? '';
    this.blobUrl = entity?.ptas_bloburl ?? '';
    this.document = entity?.ptas_portaldocument ?? '';
    this.section = entity?.ptas_portalsection ?? '';
    this.icsDocumentId = entity?.ptas_icsdocumentid ?? '';
    this.isBlob = entity?.ptas_isblob ?? false;
    this.isSharePoint = entity?.ptas_issharepoint ?? false;
    this.sharepointUrl = entity?.ptas_sharepointurl ?? '';
    this.documentType = entity?.ptas_documenttype ?? '';
    this.filingMethod = entity?.ptas_filingmethod ?? -1;
    this.listingStatus = entity?.ptas_listingstatus ?? -1;
    this.year = entity?.ptas_yearid ?? '';
    this.loadDate = entity?.ptas_loaddate ?? '';
    this.loadBy = entity?.ptas_loadbyid ?? '';
  }

  getEntityFields = (
    fieldsOmit?: [keyof FileAttachmentMetadataEntityFields]
  ): Partial<FileAttachmentMetadataEntityFields> => {
    const fieldsMapping = {
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_fileattachmentmetadataid: this.id,
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

    if (!fieldsOmit) {
      return fieldsMapping;
    }

    return omit(fieldsMapping, fieldsOmit);
  };
}
