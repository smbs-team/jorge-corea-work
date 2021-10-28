// portalContact.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { omit } from 'lodash';
import { v4 as uuid } from 'uuid';
import { CityEntity, StateEntity, ZipCodeEntity } from './addresses';

export interface PortalContactEntity {
  ptas_portalcontactid: string;
  ptas_firstname: string;
  ptas_middlename: string;
  ptas_lastname: string;
  ptas_suffix: string;
}

export class PortalContact {
  id: string;
  isSaved: boolean; //Custom field (not in entity)
  firstName: string;
  middleName: string;
  lastName: string;
  suffix: string;
  email?: PortalEmail;
  address?: PortalAddress;
  phone: PortalPhone;

  constructor(entityFields?: PortalContactEntity) {
    const contactId = entityFields?.ptas_portalcontactid ?? uuid();
    this.id = contactId;
    this.isSaved = !!entityFields?.ptas_portalcontactid;
    this.firstName = entityFields?.ptas_firstname ?? '';
    this.middleName = entityFields?.ptas_middlename ?? '';
    this.lastName = entityFields?.ptas_lastname ?? '';
    this.suffix = entityFields?.ptas_suffix ?? '';
    this.address = {
      id: uuid(),
      isSaved: false,
      title: '',
      line1: '',
      line2: '',
      countryId: '',
      state: '',
      stateId: '',
      city: '',
      cityId: '',
      zipCode: '',
      zipCodeId: '',
      portalContactId: contactId,
    };
    this.phone = {
      id: uuid(),
      isSaved: false,
      phoneNumber: '',
      // phoneType: '',
      phoneTypeValue: 0,
      acceptsTextMessages: false,
      portalContactId: contactId,
    };
  }

  getEntityFields = (
    fieldsOmit?: [keyof PortalContactEntity]
  ): Partial<PortalContactEntity> => {
    const entity = {
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_portalcontactid: this.id,
      ptas_firstname: this.firstName,
      ptas_middlename: this.middleName,
      ptas_lastname: this.lastName,
      ptas_suffix: this.suffix,
      /* eslint-enable @typescript-eslint/camelcase */
    };

    if (fieldsOmit && fieldsOmit.length) {
      return omit(entity, fieldsOmit);
    }

    return entity;
  };
}

export interface PortalEmail {
  id: string;
  isSaved: boolean;
  email: string;
  primaryEmail: boolean;
  portalContactId: string;
}

export interface PortalEmailEntity {
  _ptas_portalcontact_value: string;
  ptas_portalemailid: string;
  ptas_email: string;
  ptas_primaryemail: boolean;
}

export interface PortalAddress {
  id: string;
  isSaved: boolean;
  title: string;
  line1: string;
  line2: string;
  countryId: string;
  state: string;
  stateId: string;
  city: string;
  cityId: string;
  zipCode: string;
  zipCodeId: string;
  portalContactId: string;
}

export interface PortalAddressEntity {
  ptas_portaladdressid: string;
  ptas_addresstitle: string;
  _ptas_countryid_value: string;
  _ptas_stateid_value: string;
  ptas_stateid: StateEntity;
  _ptas_cityid_value: string;
  ptas_cityid: CityEntity;
  _ptas_zipcodeid_value: string;
  ptas_zipcodeid: ZipCodeEntity;
  _ptas_portalcontactid_value: string;
}

export interface PortalPhone {
  id: string;
  isSaved: boolean;
  phoneNumber: string;
  // phoneType: string; //Description of phone type. E.g. Cell
  phoneTypeValue: number; //Actual value saved on entity
  acceptsTextMessages: boolean;
  portalContactId: string;
}

export interface PortalPhoneEntity {
  ptas_phonenumberid: string;
  ptas_phonenumber: string;
  ptas_phonetype: number;
  ptas_acceptstextmessages: boolean;
  _ptas_portalcontactid_value: string;
}
