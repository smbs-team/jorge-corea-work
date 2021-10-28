// taxAccount.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { City, Country, State, ZipCode } from './addresses';

export interface TaxAccount {
  ptas_taxaccountid: string;
  ptas_name: string;
  ptas_phone1: string;
  statecode: string;
  statuscode: string;
  ptas_email: string;
  ptas_taxpayername: string;
  ptas_addr1_compositeaddress: string;
}

export interface TaxAccountAddress {
  ptas_taxaccountid: string;
  ptas_taxpayername?: string;
  ptas_isnonusaddress?: boolean;
  ptas_addr1_street_intl_address: string;
  ptas_addr1_city: string;
  ptas_addr1_compositeaddress_oneline?: string;
  _ptas_addr1_stateid_value: string;
  ptas_addr1_stateid?: State;
  _ptas_addr1_cityid_value: string;
  ptas_addr1_cityid?: City;
  _ptas_addr1_zipcodeid_value: string;
  ptas_addr1_zipcodeid?: ZipCode;
  _ptas_addr1_countryid_value: string;
  ptas_addr1_countryid?: Country;
}
