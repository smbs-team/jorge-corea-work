// parcel.model.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Polygon, MultiPolygon } from '@turf/turf';
import { parcelUtil } from 'utils/parcelUtil';

type BaseFields = {
  ptas_parcelDetailId?: string;
  ParcelID: string;
  Major: string;
  Minor: string;
  Selection?: boolean;
  FilteredDatasetSelection?: boolean;
};

export interface GisMapDataFields extends BaseFields {
  PropType: string;
  PropName: string;
  TaxStatus: string;
  TaxpayerName: string;
  BldgGrade: number | null;
  CondDescr: string | null;
  YrBltRen: string;
  SqFtTotLiving: number;
  CurrentZoning: string;
  SqFtLot: number;
  WfntLabel: string;
  LandProbDescrPart1: string;
  LandProbDescrPart2: string;
  ViewDescr: string;
  BaseLandValTaxYr: string;
  BaseLandVal: number;
  BLVSqFtCalc: number;
  LandVal: number;
  ImpsVal: number;
  TotVal: number;
  PrevLandVal: number;
  PrevImpsVal: number;
  PrevTotVal: number;
  PcntChgLand: number;
  PcntChgImps: number;
  PcntChgTotal: number;
  AddrLine: string;
  ApplGroup: string;
  ResAreaSub: string;
  NbrResAccys: number;
  PresentUse: string;
  CmlNetSqFtAllBldg: string;
  GeoAreaNbhd: string;
  SpecAreaNbhd: string;
  NbrCmlAccys: number;
  GeneralClassif:
    | 'ResImp'
    | 'ResVac'
    | 'ResMH'
    | 'ResAccy'
    | 'CmlImp'
    | 'CmlAccy'
    | 'CmlVac'
    | 'Other';
  CmlPredominantUse: string;
}

export type ParcelFeatureData =
  | (BaseFields & {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      [k: string]: any;
    })
  | GisMapDataFields;

/**
 * Parcel feature state
 */
export interface ParcelFeatureState {
  /**
   * A flag that determines if te feature is in selected state
   */
  selected?: boolean;
  /**
   * The parcel information
   */
  parcelData?: ParcelFeatureData;
}

export type ParcelClickEvent = {
  feature: ParcelFeature;
  lngLat: {
    lat: number;
    lng: number;
  };
};

/**
 * Parcel feature model
 */
export class ParcelFeature {
  /**
   * Feature properties
   */
  properties: {
    /**
     * Mayor pin part
     */
    MAJOR: string;
    /**
     * Minor pin part
     */
    MINOR: string;
    /**
     * The parcel pin
     */
    PIN: string;
    /**
     * A formatted pin
     */
    FORMATTER_PIN: string;
  };
  /**
   * The GeoJson type
   */
  type: 'Feature';
  /**
   * The parcel feature can be a polygon or a multipolygon
   */
  geometry: Polygon | MultiPolygon;
  /**
   * The feature id, whe are using the same id to features that belongs to the same parcel
   */
  id: string;
  /**
   * The feature bounding box
   */
  bbox?: GeoJSON.BBox;
  /**
   * Feature layer
   */
  layer: mapboxgl.Layer;
  /**
   *Feature map source to be used
   */
  source: string;
  /**
   *The feature source layer
   */
  sourceLayer: string;
  /**
   *The feature state
   */
  state: ParcelFeatureState;
  /**
   * Creates an instance of ParcelFeatureModel.
   *
   * @param fields - The json to be casted with this class
   */
  constructor(fields: ParcelFeature) {
    this.properties = {
      ...fields.properties,
      FORMATTER_PIN: parcelUtil.formatPin(fields.properties.PIN),
    };
    this.type = 'Feature';
    this.geometry = { ...fields.geometry };
    this.id = fields?.id?.toString() ?? '';
    this.bbox = fields.bbox;
    this.layer = fields.layer;
    this.source = fields.source;
    this.sourceLayer = fields.sourceLayer;
    this.state = fields.state;
    if (!fields.properties.PIN) {
      throw new Error('Invalid parcel feature pin');
    }
  }
}
