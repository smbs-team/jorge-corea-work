// apiService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { TaxAccount } from './taxAccount';

export interface Property {
  ptas_parceldetailid: string;
  ptas_acctnbr: string;
  ptas_name: string;
  ptas_namesonaccount: string;
  ptas_address: string;
  ptas_streettype: string;
  ptas_district: string;
  ptas_zipcode: string;
  lookup_source: string;
  picture: string;
  _ptas_taxaccountid_value: string;
  taxAccountData: TaxAccount | undefined;
  // temporary fields
  _ptas_addr1_stateid_value?: string;
  _ptas_addr1_zipcodeid_value?: string;
  _ptas_addr1_cityid_value?: string;
}

export interface PropertyDescription {
  ptas_address: string;
  ptas_district: string;
  ptas_zipcode: string;
  ptas_name: string;
  ptas_namesonaccount: string;
  ptas_parceldetailid: string;
  ptas_photo: string;
  ptas_evncode?: string;
  _ptas_taxaccountid_value?: string;
  _ptas_addr1_cityid_value?: string;
  _ptas_addr1_countryid_value?: string;
  _ptas_addr1_stateid_value?: string;
  _ptas_addr1_zipcodeid_value?: string;
}

export interface ParcelMediaInfo {
  MediaId: string;
  MediaPath: string;
  IsPrimary: boolean;
  ImageOrder: number;
}
