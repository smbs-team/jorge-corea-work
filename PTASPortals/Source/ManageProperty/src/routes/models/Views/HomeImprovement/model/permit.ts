// permit.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface PermitEntity {
  statecode: number;
  ptas_permittype: number;
  ptas_description: string;
  ptas_permitvalue_base: number;
  ptas_permitid: string; //Primary key
  ptas_parcelheadertext2: string;
  ptas_latestpermitinspectiondate: string;
  ptas_latestpermitinspectiontype: number;
  ptas_permitsource: number;
  ptas_permitvalue: number;
  _ptas_currentjurisdiction_value: string;
  _ptas_statusupdatedbyid_value: string;
  statuscode: number;
  ptas_errorreason: number;
  _ptas_issuedbyid_value: string;
  ptas_percentcomplete: number;
  ptas_name: string;
  ptas_deactivatedviastatus: boolean;
  ptas_statusdate: string;
  _ptas_parcelid_value: string;
  ptas_issueddate: string;
  ptas_linktopermit: string;
  ptas_parcelheadername: string;
  ptas_permitstatus: number;
  ptas_parcelheadertext: string;
}

export class Permit {
  stateCode: number;
  permitType: number;
  description: string;
  permitValueBase: number;
  permitId: string; //Primary key
  parcelHeaderText2: string;
  latestPermitInspectionDate: Date;
  latestPermitInspectionType: number;
  permitSource: number;
  permitValue: number;
  currentJurisdictionId: string;
  statusUpdatedById: string;
  statusCode: number;
  errorReason: number;
  issuedById: string;
  issuedByName: string; //Custom field (not in entity) used to keep the name of the jurisdiction before it is saved
  percentComplete: number;
  name: string;
  deactivatedViaStatus: boolean;
  statusDate: Date;
  parcelId: string;
  issuedDate: Date | undefined;
  linkToPermit: string;
  parcelHeaderName: string;
  permitStatus: number;
  parcelHeaderText: string;

  constructor(entity?: PermitEntity) {
    this.stateCode = entity?.statecode ?? 0;
    this.permitType = entity?.ptas_permittype ?? 0;
    this.description = entity?.ptas_description ?? '';
    this.permitValueBase = entity?.ptas_permitvalue_base ?? 0;
    this.permitId = entity?.ptas_permitid ?? '';
    this.parcelHeaderText2 = entity?.ptas_parcelheadertext2 ?? '';
    this.latestPermitInspectionDate = entity?.ptas_latestpermitinspectiondate
      ? new Date(entity.ptas_latestpermitinspectiondate)
      : new Date();
    this.latestPermitInspectionType =
      entity?.ptas_latestpermitinspectiontype ?? 0;
    this.permitSource = entity?.ptas_permitsource ?? 0;
    this.permitValue = entity?.ptas_permitvalue_base ?? 0;
    this.currentJurisdictionId = entity?._ptas_currentjurisdiction_value ?? '';
    this.statusUpdatedById = entity?._ptas_statusupdatedbyid_value ?? '';
    this.statusCode = entity?.statuscode ?? 0;
    this.errorReason = entity?.ptas_errorreason ?? 0;
    this.issuedById = entity?._ptas_issuedbyid_value ?? '';
    this.issuedByName = '';
    this.percentComplete = entity?.ptas_percentcomplete ?? 0;
    this.name = entity?.ptas_name ?? '';
    this.deactivatedViaStatus = entity?.ptas_deactivatedviastatus ?? false;
    this.statusDate = entity?.ptas_statusdate
      ? new Date(entity.ptas_statusdate)
      : new Date();
    this.parcelId = entity?._ptas_parcelid_value ?? '';
    this.issuedDate = entity?.ptas_issueddate
      ? new Date(entity.ptas_issueddate)
      : undefined;
    this.linkToPermit = entity?.ptas_linktopermit ?? '';
    this.parcelHeaderName = entity?.ptas_parcelheadername ?? '';
    this.permitStatus = entity?.ptas_permitstatus ?? 0;
    this.parcelHeaderText = entity?.ptas_parcelheadertext ?? '';
  }
}
