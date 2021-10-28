// addresses.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface Country {
  ptas_name: string;
  ptas_abbreviation: string;
  statuscode: number;
  statecode: number;
  ptas_countryid: string;
}

export interface City {
  statecode: number;
  statuscode: number;
  ptas_name: string;
  ptas_cityid: string;
}

export interface State {
  _ptas_countryid_value: string;
  ptas_stateorprovinceid: string;
  ptas_name: string;
  statecode: number;
  statuscode: number;
  ptas_abbreviation: string;
}

export interface ZipCode {
  statuscode: number;
  ptas_zipcodeid: string;
  statecode: number;
  ptas_name: string;
}

export interface AddressLookup {
  relevance: number;
  country: string;
  streetname: string;
  formattedaddr: string;
  state: string;
  city?: string;
  zip?: string;
  laitude: number;
  longitude: number;
}

export interface BasicAddressData {
  country: string;
  countryId: string;
  state: string;
  stateId: string;
  city: string;
  cityId: string;
  zipCode: string;
  zipCodeId: string;
}
