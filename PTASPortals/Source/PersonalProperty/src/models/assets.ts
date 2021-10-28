// assets.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { formatDate } from 'utils/date';

export type assetTypeCategory =
  | 'All'
  | 'Construction'
  | 'Factory'
  | 'Farm'
  | 'Office'
  | 'Retail'
  | 'Service';
export interface AssetEntity {
  ptas_personalpropertyassetid: string;
  _ptas_yearacquiredid_value: string;
  ptas_originalcost: number;
  _ptas_categorycodeid_value: string;
  ptas_changereason: number;
  _ptas_personalpropertyid_value: string;
  modifiedon: string;
}

export interface AssetCategoryEntity {
  ptas_categorycode: string;
  ptas_personalpropertycategoryid: string;
  ptas_perspropcategory: string;
  ptas_legacycategorycode: string;
  ptas_categorygrouph: string;
}

export interface YearEntity {
  ptas_name: string;
  ptas_yearid: string;
}

export class AssetCategory {
  id: string;
  code: string;
  persPropCategory: string;
  legancyCode: string;
  group: string;

  constructor(entityFields?: AssetCategoryEntity) {
    this.id = entityFields?.ptas_personalpropertycategoryid ?? '';
    this.code = entityFields?.ptas_categorycode ?? '';
    this.persPropCategory = entityFields?.ptas_perspropcategory ?? '';
    this.legancyCode = entityFields?.ptas_legacycategorycode ?? '';
    this.group = entityFields?.ptas_categorygrouph ?? '';
  }
}

export class Asset {
  id: string;
  yearAcquiredId: string;
  cost: number;
  categoryCodeId: string;
  changeReason: number;
  personalPropertyId: string;
  modifiedOn: string;

  constructor(entityFields?: AssetEntity) {
    const dateFormatted = formatDate(entityFields?.modifiedon ?? new Date());

    this.id = entityFields?.ptas_personalpropertyassetid ?? '';
    this.yearAcquiredId = entityFields?._ptas_yearacquiredid_value ?? '';
    this.cost = entityFields?.ptas_originalcost ?? 0;
    this.categoryCodeId = entityFields?._ptas_categorycodeid_value ?? '';
    this.changeReason = entityFields?.ptas_changereason ?? 0;
    this.personalPropertyId =
      entityFields?._ptas_personalpropertyid_value ?? '';
    this.modifiedOn = dateFormatted;
  }
}
