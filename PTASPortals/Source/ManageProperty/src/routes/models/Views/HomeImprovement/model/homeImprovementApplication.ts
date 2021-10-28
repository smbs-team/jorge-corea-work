// homeImprovementApplication.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { v4 as uuid } from 'uuid';

export interface HomeImprovementApplicationEntity {
  ptas_applicationsignedbytaxpayer: boolean;
  statecode: number | null; //Status of the Home Improvement
  statuscode: number | null; //Reason for the status of the Home Improvement
  ptas_hipostcardsent: boolean;
  ptas_exemptionnumber: number;
  _ptas_parcelid_value: string; //Parcel ID
  ptas_homeimprovementid: string; //Primary key
  ptas_name: string; //The name of the custom entity
  _ptas_exemptionendyearid_value: string;
  _ptas_exemptionbeginyearid_value: string;
  ptas_exemptionbeginyearid: {
    ptas_name: string;
  } | null;
  _ptas_permitid_value: string; //Permit ID
  ptas_compositemailingaddress: string;
  ptas_estimatedconstructioncost: number;
  ptas_estimatedconstructioncost_base: number; //Value of the Estimated construction cost in base currency
  _ptas_buildingid_value: string;
  ptas_constructionbegindate: string;
  ptas_estimatedcompletiondate: string;
  ptas_exemptionamount: number;
  ptas_exemptionamount_base: number; //Value of the Exemption Amount in base currency
  ptas_phonenumber: string;
  ptas_emailaddress: string;
  _ptas_permitjurisdictionid_value: string;
  ptas_descriptionoftheimprovement: string;
  _ptas_portalcontactid_value: string;
  ptas_dateapplicationreceived: string;
  ptas_constructionpropertyaddress: string;
  _ptas_taxaccountid_value: string;
  ptas_datepermitissued: string;
  ptas_taxpayername: string;
  ptas_applicationsource: number | null; //Option set
}

export class HomeImprovementApplication {
  signedByTaxpayer: boolean;
  stateCode: number | null; //Status of the Home Improvement
  statusCode: number | null; //Reason for the status of the Home Improvement
  hiPostcardSent: boolean;
  exemptionNumber: number;
  parcelId: string; //Parcel ID
  homeImprovementId: string; //Primary key
  isSaved: boolean; //Custom field (not in entity)
  name: string; //The name of the custom entity
  exemptionEndYearId: string;
  exemptionBeginYearId: string;
  exemptionBeginYear: number;
  permitId: string; //Permit ID
  compositeMailingAddress: string;
  estimatedConstructionCost: number;
  estimatedConstructionCostBase: number; //Value of the Estimated construction cost in base currency
  buildingId: string;
  constructionBeginDate: Date;
  estimatedCompletionDate: Date;
  exemptionAmount: number;
  exemptionAmountBase: number; //Value of the Exemption Amount in base currency
  phoneNumber: string;
  emailAddress: string;
  permitJurisdictionId: string;
  descriptionOfTheImprovement: string;
  portalContactId: string;
  dateApplicationReceived: Date;
  constructionPropertyAddress: string;
  taxAccountId: string;
  datePermitIssued: Date;
  taxpayerName: string;
  applicationSource: number | null;

  constructor(entity?: HomeImprovementApplicationEntity) {
    this.signedByTaxpayer = entity?.ptas_applicationsignedbytaxpayer ?? false;
    this.stateCode = entity?.statecode ?? null; //Status of the Home Improvement
    this.statusCode = entity?.statuscode ?? null; //Reason for the status of the Home Improvement
    this.hiPostcardSent = entity?.ptas_hipostcardsent ?? false;
    this.exemptionNumber = entity?.ptas_exemptionnumber ?? 0;
    this.parcelId = entity?._ptas_parcelid_value ?? ''; //Parcel ID
    this.homeImprovementId = entity?.ptas_homeimprovementid ?? uuid(); //Primary key
    this.isSaved = !!entity?.ptas_homeimprovementid;
    this.name = entity?.ptas_name ?? ''; //The name of the custom entity
    this.exemptionEndYearId = entity?._ptas_exemptionendyearid_value ?? '';
    this.exemptionBeginYearId = entity?._ptas_exemptionbeginyearid_value ?? '';
    this.exemptionBeginYear = entity?.ptas_exemptionbeginyearid?.ptas_name
      ? Number.parseInt(entity?.ptas_exemptionbeginyearid.ptas_name)
      : 0;
    this.permitId = entity?._ptas_permitid_value ?? ''; //Permit ID
    this.compositeMailingAddress = entity?.ptas_compositemailingaddress ?? '';
    this.estimatedConstructionCost =
      entity?.ptas_estimatedconstructioncost ?? 0;
    this.estimatedConstructionCostBase =
      entity?.ptas_estimatedconstructioncost_base ?? 0; //Value of the Estimated construction cost in base currency
    this.buildingId = entity?._ptas_buildingid_value ?? '';
    this.constructionBeginDate = entity?.ptas_constructionbegindate
      ? new Date(entity.ptas_constructionbegindate)
      : new Date();
    this.estimatedCompletionDate = entity?.ptas_estimatedcompletiondate
      ? new Date(entity.ptas_estimatedcompletiondate)
      : new Date();
    this.exemptionAmount = entity?.ptas_exemptionamount ?? 0;
    this.exemptionAmountBase = entity?.ptas_exemptionamount_base ?? 0; //Value of the Exemption Amount in base currency
    this.phoneNumber = entity?.ptas_phonenumber ?? '';
    this.emailAddress = entity?.ptas_emailaddress ?? '';
    this.permitJurisdictionId = entity?._ptas_permitjurisdictionid_value ?? '';
    this.descriptionOfTheImprovement =
      entity?.ptas_descriptionoftheimprovement ?? '';
    this.portalContactId = entity?._ptas_portalcontactid_value ?? '';
    this.dateApplicationReceived = entity?.ptas_dateapplicationreceived
      ? new Date(entity.ptas_dateapplicationreceived)
      : new Date();
    this.constructionPropertyAddress =
      entity?.ptas_constructionpropertyaddress ?? '';
    this.taxAccountId = entity?._ptas_taxaccountid_value ?? '';
    this.datePermitIssued = entity?.ptas_datepermitissued
      ? new Date(entity.ptas_datepermitissued)
      : new Date();
    this.taxpayerName = entity?.ptas_taxpayername ?? '';
    this.applicationSource = entity?.ptas_applicationsource ?? null;
  }
}
