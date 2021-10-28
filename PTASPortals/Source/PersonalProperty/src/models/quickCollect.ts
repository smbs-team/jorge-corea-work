// quickCollect.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { v4 as uuidv4 } from 'uuid';
import { FileAttachmentMetadata } from './fileAttachmentMetadata';

export interface QuickCollectEntity {
  ptas_quickcollectid: string;
  _ptas_personalpropertyid_value: string;
  ptas_date: string;
  ptas_personalpropinfo_addr_street1: string;
  ptas_personalpropinfo_addr_street2: string;
  ptas_personalpropinfo_addr_city: string;
  _ptas_personalpropinfo_addr_stateid_value: string;
  ptas_personalpropinfo_addr_zip: string;
  ptas_reasonforrequest: number;
  ptas_methodoftransfer: number;
  _ptas_billofsale_fileattachementmetadataid_value: string;
  ptas_newowneremail: string;
  ptas_newownername: string;
  ptas_newbusinessname: string;
  ptas_ubinumber: string;
  _ptas_businessnaicscodeid_value: string;
  ptas_totalsalesprice: number;
  ptas_equipment: number;
  ptas_leaseholdimprovements: number;
  ptas_intangibles: number;
  ptas_other: number;
  ptas_closingdate: string;
  ptas_dispositionofassets: string;
  ptas_requestorinfo_addr_street1: string;
  ptas_requestorinfo_addr_street2: string;
  ptas_requestorinfo_addr_city: string;
  _ptas_requestorinfo_addr_stateid_value: string;
  ptas_requestorinfo_addr_zip: string;
  statecode: number;
  statuscode: number;
}

export class QuickCollect {
  id: string; //Primary key
  personalPropertyId: string;
  date: string;
  businessAddress1: string;
  businessAddress2: string;
  businessCityId: string;
  businessCityLabel: string;
  businessStateId: string;
  businessStateLabel: string;
  businessZipId: string;
  businessZipLabel: string;
  reasonForRequestId: number;
  methodOfTransferId: number;
  billFileAttachmentMetadataId: string;
  billFileAttachmentMetadata?: FileAttachmentMetadata;
  newOwnerEmail: string;
  newOwnerName: string;
  newBusinessName: string;
  ubiNumber: string;
  naicsCodeId: string;
  totalSalesPrice: number;
  equipment: number;
  leaseHoldImprovements: number;
  intangibles: number;
  other: number;
  closingDate: string;
  dispositionOfAssets: string;
  requestorAddress1: string;
  requestorAddress2: string;
  requestorCityId: string;
  requestorCityLabel: string;
  requestorStateId: string;
  requestorStateLabel: string;
  requestorZipId: string;
  requestorZipLabel: string;
  stateCode: number;
  statusCode: number;

  constructor(entity?: QuickCollectEntity) {
    this.id = entity?.ptas_quickcollectid ?? uuidv4();
    this.personalPropertyId = entity?._ptas_personalpropertyid_value ?? '';
    this.date = entity?.ptas_date ?? '';
    this.businessAddress1 = entity?.ptas_personalpropinfo_addr_street1 ?? '';
    this.businessAddress2 = entity?.ptas_personalpropinfo_addr_street2 ?? '';
    this.businessCityId = ''; // Not exist in entity
    this.businessCityLabel = entity?.ptas_personalpropinfo_addr_city ?? '';
    this.businessStateId =
      entity?._ptas_personalpropinfo_addr_stateid_value ?? '';
    this.businessStateLabel = '';
    this.businessZipId = ''; // Not exist in entity
    this.businessZipLabel = entity?.ptas_personalpropinfo_addr_zip ?? '';
    this.reasonForRequestId = entity?.ptas_reasonforrequest ?? 0;
    this.methodOfTransferId = entity?.ptas_methodoftransfer ?? 0;
    this.billFileAttachmentMetadataId =
      entity?._ptas_billofsale_fileattachementmetadataid_value ?? '';
    this.newOwnerEmail = entity?.ptas_newowneremail ?? '';
    this.newOwnerName = entity?.ptas_newownername ?? '';
    this.newBusinessName = entity?.ptas_newbusinessname ?? '';
    this.ubiNumber = entity?.ptas_ubinumber ?? '';
    this.naicsCodeId = entity?._ptas_businessnaicscodeid_value ?? '';
    this.totalSalesPrice = entity?.ptas_totalsalesprice ?? 0;
    this.equipment = entity?.ptas_equipment ?? 0;
    this.leaseHoldImprovements = entity?.ptas_leaseholdimprovements ?? 0;
    this.intangibles = entity?.ptas_intangibles ?? 0;
    this.other = entity?.ptas_other ?? 0;
    this.closingDate = entity?.ptas_date ?? '';
    this.dispositionOfAssets = entity?.ptas_dispositionofassets ?? '';
    this.requestorAddress1 = entity?.ptas_requestorinfo_addr_street1 ?? '';
    this.requestorAddress2 = entity?.ptas_requestorinfo_addr_street2 ?? '';
    this.requestorCityId = ''; // Not exist in entity
    this.requestorCityLabel = entity?.ptas_requestorinfo_addr_city ?? '';
    this.requestorStateId =
      entity?._ptas_requestorinfo_addr_stateid_value ?? '';
    this.requestorStateLabel = '';
    this.requestorZipId = ''; // Not exist in entity
    this.requestorZipLabel = entity?.ptas_requestorinfo_addr_zip ?? '';
    this.stateCode = entity?.statecode ?? 0;
    this.statusCode = entity?.statuscode ?? 0;
  }
}
