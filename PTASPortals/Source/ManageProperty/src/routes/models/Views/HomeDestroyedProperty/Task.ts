// Task.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface TaskEntity {
  ptas_taskid: string; // entity pk
  _ptas_taxaccountnumber_value: string; // lookup to taxaccount
  ptas_submissionsource: number; // "online"
  _ptas_parcelid_value: string; // lookup to ptas_parceldetail
  ptas_propertyaddress: string;
  ptas_phonenumber: string; // phone??
  ptas_taxpayername: string;
  ptas_dateofdestruction: string; //  date and time mm/dd/yyyy
  ptas_destroyedpropertydescription: string;
  ptas_lossoccurringasaresultof: ''; // multiple options
  ptas_anticipatedrepairdates: string;
  ptas_repairdatesunknownatthistime: boolean;
  _ptas_portalcontact_value: string;
  ptas_datesigned: string; // date
  ptas_signedby: number; // option set (taxpayer)
  statecode: number; // option set
  statuscode: number; // option set
  ptas_tasktype: number; // option set 'Destroyed property'
  ptas_permitissuedby: string;
  ptas_lossoccurringasaresultofother: string;
  ptas_email: string;
  ptas_citystatezip: string;
}

export class Task {
  id: string;
  taxAccountNumber: string;
  submissionSource: number | null;
  parcelId: string;
  propertyAddress: string;
  phoneNumber: string;
  taxPayerName: string;
  dateOfDestruction: string; // (date of destruction)
  destroyedPropertyDescription: string; // (description of destroyed property //property)
  lossOccurringAsAResultOf: string; // (property destroyed as result of)
  anticipatedRepairDates: string;
  repairDateUnknownAtThisTime?: boolean; // (Repair is planned and/or // property)
  portalContact: string;
  dateSigned: string;
  signedBy: number | null;
  stateCode: number | null;
  statusCode: number | null;
  taskType: number | null;
  permitIssuedBy: string;
  othercomments: string;
  email: string; // fields not include
  cityStateZip: string;

  constructor(entityFields?: TaskEntity) {
    this.id = entityFields?.ptas_taskid ?? '';
    this.taxAccountNumber = entityFields?._ptas_taxaccountnumber_value ?? '';
    this.submissionSource = entityFields?.ptas_submissionsource ?? null;
    this.parcelId = entityFields?._ptas_parcelid_value ?? '';
    this.propertyAddress = entityFields?.ptas_propertyaddress ?? '';
    this.phoneNumber = entityFields?.ptas_phonenumber ?? '';
    this.taxPayerName = entityFields?.ptas_taxpayername ?? '';
    this.dateOfDestruction = entityFields?.ptas_dateofdestruction ?? '';
    this.destroyedPropertyDescription =
      entityFields?.ptas_destroyedpropertydescription ?? '';
    this.lossOccurringAsAResultOf =
      entityFields?.ptas_lossoccurringasaresultof ?? '';
    this.anticipatedRepairDates =
      entityFields?.ptas_anticipatedrepairdates ?? '';
    this.repairDateUnknownAtThisTime =
      entityFields?.ptas_repairdatesunknownatthistime ?? false;
    this.portalContact = entityFields?._ptas_portalcontact_value ?? '';
    this.dateSigned = entityFields?.ptas_datesigned ?? '';
    this.signedBy = entityFields?.ptas_signedby ?? null;
    this.stateCode = entityFields?.statecode ?? null;
    this.statusCode = entityFields?.statuscode ?? null;
    this.taskType = entityFields?.ptas_tasktype ?? null;
    this.permitIssuedBy = entityFields?.ptas_permitissuedby ?? '';
    this.othercomments = entityFields?.ptas_lossoccurringasaresultofother ?? '';
    this.email = entityFields?.ptas_email ?? '';
    this.cityStateZip = entityFields?.ptas_citystatezip ?? '';
  }
}
