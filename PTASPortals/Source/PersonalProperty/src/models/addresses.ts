// addresses.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface CityEntity {
  ptas_cityid: string;
  ptas_name: string;
}

export interface CountryEntity {
  ptas_name: string;
  ptas_abbreviation: string;
  ptas_countryid: string;
}

export interface StateEntity {
  ptas_stateorprovinceid: string;
  ptas_name: string;
  ptas_abbreviation: string;
  _ptas_countryid_value: string;
}

export interface ZipCodeEntity {
  ptas_zipcodeid: string;
  ptas_name: string;
}

export interface AddressLookupEntity {
  relevance: number;
  country: string;
  streetname: string;
  formattedaddr: string;
  state: string;
  laitude: number;
  longitude: number;
  city: string;
  zip: string;
}

export interface StateOrProvince {
  ptas_name: string;
  ptas_abbreviation: string;
  ptas_stateorprovinceid: string;
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
