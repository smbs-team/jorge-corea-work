// personalPropertyHistory.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { v4 as uuidv4 } from 'uuid';

export interface PersonalPropertyHistoryEntity {
  ptas_personalpropertyhistoryid: string;
  _ptas_personalpropertyid_value: string;
  ptas_preparername: string;
  ptas_preparerattention: string;
  ptas_prepareremail1: string;
  ptas_preparercellphone1: string;
  _ptas_preparercityid_value: string;
  _ptas_preparerstateid_value: string;
  _ptas_preparerzipid_value: string;
  _ptas_preparercountryid_value: string;
  ptas_property_type: number;
  ptas_businessstateincorporated_value: string;
  statecode: number;
  statuscode: number;
}

export class PersonalPropertyHistory {
  id: string; //Primary key
  personalPropertyId: string;
  preparerName: string;
  preparerAttention: string;
  preparerEmail1: string;
  preparerCellPhone1: string;
  preparerCityId: string;
  preparerCityLabel: string;
  preparerStateId: string;
  preparerStateLabel: string;
  preparerZipId: string;
  preparerZipLabel: string;
  preparerCountryId: string;
  propertyType: number;
  businessStateIncorporateId: string;
  stateCode: number;
  statusCode: number;

  constructor(entity?: PersonalPropertyHistoryEntity) {
    this.id = entity?.ptas_personalpropertyhistoryid ?? uuidv4();
    this.personalPropertyId = entity?._ptas_personalpropertyid_value ?? '';
    this.preparerName = entity?.ptas_preparername ?? '';
    this.preparerAttention = entity?.ptas_preparerattention ?? '';
    this.preparerEmail1 = entity?.ptas_prepareremail1 ?? '';
    this.preparerCellPhone1 = entity?.ptas_preparercellphone1 ?? '';
    this.preparerCityId = entity?._ptas_preparercityid_value ?? '';
    this.preparerCityLabel = '';
    this.preparerStateId = entity?._ptas_preparerstateid_value ?? '';
    this.preparerStateLabel = '';
    this.preparerZipId = entity?._ptas_preparerzipid_value ?? '';
    this.preparerZipLabel = '';
    this.preparerCountryId = entity?._ptas_preparercountryid_value ?? '';
    this.propertyType = entity?.ptas_property_type ?? 0;
    this.businessStateIncorporateId =
      entity?.ptas_businessstateincorporated_value ?? '';
    this.stateCode = entity?.statecode ?? 0;
    this.statusCode = entity?.statuscode ?? 0;
  }
}
