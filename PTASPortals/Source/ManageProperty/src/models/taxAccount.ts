// taxAccount.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface TaxAccountEntityFields {
  ptas_taxaccountid: string;
  ptas_name: string;
  ptas_phone1: string;
  statecode: number;
  statuscode: number;
  ptas_email: string;
  ptas_paperless?: boolean;
  ptas_taxpayername: string;
  ptas_addr1_compositeaddress: string;
  ptas_accountnumber: string;
}

export class TaxAccount {
  id: string;
  name: string;
  phone1: string;
  email: string;
  paperless?: boolean;
  taxPayerName: string;
  compositeAddress: string;
  accountNumber: string;
  stateCode: number;
  statusCode: number;

  constructor(entity?: TaxAccountEntityFields) {
    this.id = entity?.ptas_taxaccountid ?? '';
    this.name = entity?.ptas_name ?? '';
    this.phone1 = entity?.ptas_phone1 ?? '';
    this.email = entity?.ptas_email ?? '';
    this.paperless = entity?.ptas_paperless ?? false;
    this.taxPayerName = entity?.ptas_taxpayername ?? '';
    this.compositeAddress = entity?.ptas_addr1_compositeaddress ?? '';
    this.accountNumber = entity?.ptas_accountnumber ?? '';
    this.stateCode = entity?.statecode ?? 0;
    this.statusCode = entity?.statuscode ?? 0;
  }
}
