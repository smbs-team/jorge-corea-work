// App.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { PersonalProperty, PersonalPropertyEntity } from './personalProperty';
import { PortalContactEntity } from './portalContact';

export interface PersonalPropertyAccountAccessEntity {
  ptas_personalpropertyaccountaccessid: string; //Primary key
  _ptas_personalpropertyaccountid_value: string;
  _ptas_portalcontactid_value: string;
  ptas_accesslevel: number;
  ptas_personalpropertyaccountid?: PersonalPropertyEntity;
  ptas_portalcontactid?: PortalContactEntity;
  statecode: number;
  statuscode: number;
}

export class PersonalPropertyAccountAccess {
  id: string;
  personalPropertyId: string;
  portalContactId: string;
  accessLevel: number;
  stateCode: number;
  statusCode: number;
  personalProperty?: PersonalProperty;
  constructor(entity?: PersonalPropertyAccountAccessEntity) {
    const personalPropFound = entity?.ptas_personalpropertyaccountid
      ? new PersonalProperty(entity?.ptas_personalpropertyaccountid)
      : undefined;
    this.id = entity?.ptas_personalpropertyaccountaccessid ?? '';
    this.personalPropertyId =
      entity?._ptas_personalpropertyaccountid_value ?? '';
    this.portalContactId = entity?._ptas_portalcontactid_value ?? '';
    this.accessLevel = entity?.ptas_accesslevel ?? 0;
    this.personalProperty = personalPropFound;
    this.stateCode = entity?.statecode ?? 0;
    this.statusCode = entity?.statuscode ?? 0;
  }
}
