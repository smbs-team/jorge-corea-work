// ContactProfile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface TaxDistrictContactEntity {
  type: 'taxDistrict';
  ptas_taxdistrictcontactsid: string;
  ptas_firstname: string;
  ptas_lastname: string;
  ptas_emailaddress: string;
  ptas_phonenumber: string;
  ptas_jobtitle: string;
  ptas_state: string;
  ptas_city: string;
  ptas_zipcode: string;
  ptas_address1: string;
  ptas_address2: string;
  ptas_note: string;
}

export interface JurisdictionContactEntity {
  type: 'jurisdiction';
  ptas_jurisdictioncontactid: string;
  ptas_firstname: string;
  ptas_lastname: string;
  ptas_email: string;
  ptas_phonenumber: string;
  ptas_jobtitle: string;
  ptas_state: string;
  ptas_city: string;
  ptas_zipcode: string;
  ptas_address1: string;
  ptas_address2: string;
  ptas_note: string;
  _ptas_jurisdictionid_value: string;
}
export interface TaxDistrictContactEntity {
  ptas_taxdistrictcontactsid: string;
  ptas_name: string;
  ptas_email: string;
  ptas_emailaddress: string;
  _ptas_taxdistrictid_value: string;
}

export interface ContactAddress {
  line1: string;
  line2: string;
  city: string;
  state: string;
  zip: string;
}

export interface PermitsFileInfo {
  file: string;
  hasErrors: boolean;
  errorCount: number;
}
export interface UptPermitsFileInfo {
  message: string;
  hasErrors: boolean;
}

export class Contact {
  id: string;
  type: 'jurisdiction' | 'taxDistrict' | null;
  firstName: string;
  lastName: string;
  email: string;
  address?: ContactAddress;
  phoneNumber: string;
  jobTitle: string;
  jurisdictionId?: string;
  taxDistrictId?: string;

  constructor(
    entityFields?: JurisdictionContactEntity | TaxDistrictContactEntity
  ) {
    if (entityFields) {
      if (entityFields.type === 'jurisdiction') {
        this.type = 'jurisdiction';
        this.id = entityFields.ptas_jurisdictioncontactid;
        this.firstName = entityFields.ptas_firstname;
        this.lastName = entityFields.ptas_lastname;
        this.email = entityFields.ptas_email;
        this.phoneNumber = entityFields.ptas_phonenumber;
        this.jobTitle = entityFields.ptas_jobtitle;
        this.address = {
          state: entityFields.ptas_state,
          city: entityFields.ptas_city,
          zip: entityFields.ptas_zipcode,
          line1: entityFields.ptas_address1,
          line2: entityFields.ptas_address2,
        };
        this.jurisdictionId = entityFields._ptas_jurisdictionid_value;
      } else {
        this.type = 'taxDistrict';
        this.id = entityFields.ptas_taxdistrictcontactsid;
        this.firstName = entityFields.ptas_firstname;
        this.lastName = entityFields.ptas_lastname;
        this.email = entityFields.ptas_emailaddress;
        this.phoneNumber = entityFields.ptas_phonenumber;
        this.jobTitle = entityFields.ptas_jobtitle;
        this.address = {
          state: entityFields.ptas_state,
          city: entityFields.ptas_city,
          zip: entityFields.ptas_zipcode,
          line1: entityFields.ptas_address1,
          line2: entityFields.ptas_address2,
        };
        this.taxDistrictId = entityFields._ptas_taxdistrictid_value;
      }
    } else {
      this.id = '';
      this.type = null;
      this.firstName = '';
      this.lastName = '';
      this.email = '';
      this.phoneNumber = '';
      this.jobTitle = '';
      this.address = undefined;
    }
  }
}
