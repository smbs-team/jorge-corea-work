// personalProperty.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { v4 as uuidv4 } from 'uuid';
import { formatDate } from 'utils/date';

export interface PersonalPropertyEntity {
  // basic info
  ptas_personalpropertyid: string; //Primary key
  ptas_businessname: string;
  ptas_ubi: string;
  _ptas_naicscodeid_value: string; // (Lookup)
  ptas_naicsdescription: string;
  ptas_propertytype: number; // (option set)
  // location
  ptas_addr1_business: string;
  ptas_addr1_business_line2: string;
  _ptas_addr1_business_cityid_value: string;
  _ptas_addr1_business_stateid_value: string;
  _ptas_addr1_business_zipcodeid_value: string;
  _ptas_stateincorporatedid_value: string;
  _ptas_taxpayercountryid_value: string;
  // contact
  ptas_taxpayername: string;
  ptas_taxpayer_attention: string;
  ptas_taxpayer_cellphone1: string;
  ptas_taxpayer_email1: string;

  ptas_accountnumber?: string;
  ptas_elistingaccesscode?: string;
  ptas_filingdate: string;
  statecode: number;
  statuscode: number;
  ptas_preparer_attention: string;
  ptas_preparer_cellphone1: string;
  ptas_preparer_email1: string;
  ptas_preparername: string;
  ptas_addr1_preparer: string;
  ptas_addr1_preparer_line2: string;
  _ptas_preparercountryid_value: string;
  _ptas_addr1_preparer_cityid_value: string;
  _ptas_addr1_preparer_stateid_value: string;
  _ptas_addr1_preparer_zipcodeid_value: string;
}

export class PersonalProperty {
  id: string; //Primary key
  businessName: string;
  ubi: string;
  naicsNumber: string;
  propertyType: number;
  address: string;
  addressCont: string;
  cityId: string;
  stateId: string;
  zipId: string;
  countryId: string;
  taxpayerName: string;
  taxpayerAttention: string;
  stateOfIncorporationId: string;
  naicsDescription: string;
  accountNumber: string;
  accessCode: string;
  filedDate: string;
  assessedYear: string;
  stateCode: number;
  statusCode: number;
  preparerAttention: string;
  preparerCellphone: string;
  preparerEmail: string;
  preparerAddress: string;
  preparerAddressLine2: string;
  preparerCountryId: string;
  preparerCityId: string;
  preparerStateId: string;
  preparerZipCodeId: string;
  addrCityLabel: string;
  addrStateLabel: string;
  addrZipcodeLabel: string;
  preparerCityLabel: string;
  preparerStateLabel: string;
  preparerZipcodeLabel: string;
  taxpayerCellphone: string;
  taxpayerEmail: string;
  preparerName: string;
  preparerBusinessTitle: string;
  addrBusinessTitle: string;
  attachUrl: string;
  attachId: string;
  attachName: string;

  constructor(entity?: PersonalPropertyEntity) {
    const filedDateFormatted = formatDate(entity?.ptas_filingdate ?? '');

    this.id = entity?.ptas_personalpropertyid ?? uuidv4();
    this.businessName = entity?.ptas_businessname ?? '';
    this.ubi = entity?.ptas_ubi ?? '';
    this.naicsNumber = entity?._ptas_naicscodeid_value ?? '';
    this.propertyType = entity?.ptas_propertytype ?? 0;
    this.address = entity?.ptas_addr1_business ?? '';
    this.addressCont = entity?.ptas_addr1_business_line2 ?? '';
    this.cityId = entity?._ptas_addr1_business_cityid_value ?? '';
    this.stateId = entity?._ptas_addr1_business_stateid_value ?? '';
    this.zipId = entity?._ptas_addr1_business_zipcodeid_value ?? '';
    this.countryId = entity?._ptas_taxpayercountryid_value ?? '';
    this.taxpayerName = entity?.ptas_taxpayername ?? '';
    this.taxpayerAttention = entity?.ptas_taxpayer_attention ?? '';
    this.stateOfIncorporationId = entity?._ptas_stateincorporatedid_value ?? '';
    this.naicsDescription = entity?.ptas_naicsdescription ?? '';
    this.accountNumber = entity?.ptas_accountnumber ?? '';
    this.accessCode = entity?.ptas_elistingaccesscode ?? '';
    this.filedDate = filedDateFormatted;
    this.assessedYear = ''; // set AssessedDate externally
    this.stateCode = entity?.statecode ?? 0;
    this.statusCode = entity?.statuscode ?? 0;
    this.preparerAttention = entity?.ptas_preparer_attention ?? '';
    this.preparerCellphone = entity?.ptas_preparer_cellphone1 ?? '';
    this.preparerAddress = entity?.ptas_addr1_preparer ?? '';
    this.preparerAddressLine2 = entity?.ptas_addr1_preparer_line2 ?? '';
    this.preparerCountryId = entity?._ptas_preparercountryid_value ?? '';
    this.preparerCityId = entity?._ptas_addr1_preparer_cityid_value ?? '';
    this.preparerStateId = entity?._ptas_addr1_preparer_stateid_value ?? '';
    this.preparerZipCodeId = entity?._ptas_addr1_preparer_zipcodeid_value ?? '';
    this.preparerEmail = entity?.ptas_preparer_email1 ?? '';
    this.addrCityLabel = '';
    this.addrStateLabel = '';
    this.addrZipcodeLabel = '';
    this.preparerCityLabel = '';
    this.preparerStateLabel = '';
    this.preparerZipcodeLabel = '';
    this.preparerBusinessTitle = '';
    this.addrBusinessTitle = '';
    this.attachUrl = '';
    this.attachId = '';
    this.attachName = '';
    this.preparerName = entity?.ptas_preparername ?? '';
    this.taxpayerCellphone = entity?.ptas_taxpayer_cellphone1 ?? '';
    this.taxpayerEmail = entity?.ptas_taxpayer_email1 ?? '';
  }
}
