/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { BBox } from '@turf/helpers';
import { DEFAULT_DATASET_ID } from 'appConstants';
import { FeatureDataService } from 'services/api/datasetService/FeatureDataService';
import { GisMapDataFields, ParcelFeatureData } from 'services/map/model';
import { parcelUtil } from 'utils';
import { isGisMapDataFields } from '.';
import fieldsFormat from './fieldsFormat.json';

type FieldType = 'number' | 'date' | 'money';

const formatNumber = (n: number): string =>
  Intl.NumberFormat('en-US').format(n);

const formatMoney = (n: number): string => '$ ' + formatNumber(n);

const formatDate = (val: string): string =>
  new Date(val).toLocaleDateString('en-US');

const formatField = (
  val: string | number,
  type: FieldType
): string | number => {
  switch (type) {
    case 'number': {
      if (typeof val === 'number') return formatNumber(val);
      console.error('Error, cant format ' + val + ' because is not a number');
      return val;
    }
    case 'money': {
      return formatMoney(+val);
    }
    case 'date': {
      return formatDate(val.toString());
    }
    default: {
      return val;
    }
  }
};

const formatFields = (parcelInfo: ParcelFeatureData): ParcelFeatureData => {
  for (const field of fieldsFormat) {
    if (field.name in parcelInfo) {
      const k = field.name as keyof ParcelFeatureData;
      parcelInfo[k] = formatField(parcelInfo[k], field.format as FieldType);
    }
  }

  return parcelInfo;
};

export const featureDataService = new FeatureDataService();

const prefix = 'parcel-summary-';
export const getParcelDetail = async (
  pin: string,
  bbox?: BBox
): Promise<GisMapDataFields | undefined> => {
  const id = prefix + pin;
  const found = sessionStorage.getItem(id);
  if (found) return JSON.parse(found);

  const res = await featureDataService.getFeaturesData({
    force: true,
    bounds: bbox ?? parcelUtil.getParcelBbox(pin),
    datasetId: DEFAULT_DATASET_ID,
    mapEventName: 'parcel-data',
  });

  for (const item of res ?? []) {
    if (item.Major + item.Minor === pin) {
      if (!isGisMapDataFields(item))
        throw new Error(
          `Feature data result for parcel ${pin} has invalid fields`
        );
      sessionStorage.setItem(id, JSON.stringify(item));
      return formatFields(item) as GisMapDataFields;
    }
  }
  return;
};
