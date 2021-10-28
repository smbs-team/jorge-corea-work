/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import axios, { Canceler } from 'axios';
import { cloneDeep, groupBy } from 'lodash';
import mapboxgl from 'mapbox-gl';

import {
  AppLayers,
  FEATURES_DATA_SPLIT_ZOOM,
  MIN_FEATURES_DATA_ZOOM,
} from 'appConstants';
import { utilService } from 'services/common';
import { mapService } from 'services/map';
import { $onError, $onMapReady } from 'services/map/mapServiceEvents';
import { ParcelFeatureData } from 'services/map/model';
import { parcelUtil } from 'utils/parcelUtil';
import { Subject } from 'rxjs';

type BuildUrlOptions = {
  bounds?: number[];
  filterFields?: string[];
  datasetId: string;
  filterDatasetId?: ReturnType<typeof utilService.getUrlSearchParam>;
  mapEventName?:
    | 'label-feature-data-loaded'
    | 'color-rend-feature-data-loaded'
    | 'parcel-data';
};

export type GetFeaturesDataOptions = {
  force?: boolean;
} & BuildUrlOptions;

export class FeatureDataService {
  $onFeaturesDataLoaded = new Subject<ParcelFeatureData[]>();
  private _reqTimeout = 120000;
  enableGetFeaturesData = true;
  private _lastUrl = '';
  cancelRequest: Canceler | undefined;
  private _map?: mapboxgl.Map;
  public static readonly requiredFilterFields = ['Major', 'Minor'];
  constructor() {
    $onMapReady.subscribe((map) => {
      this._map = map;
    });
  }

  public readonly getFeaturesData = async (
    options: GetFeaturesDataOptions
  ): Promise<ParcelFeatureData[] | undefined> => {
    if (!this._map) return;
    this.cancelRequest?.();
    const mapZoom = this._map.getZoom();
    if (mapZoom < MIN_FEATURES_DATA_ZOOM) return;

    const filterFields: string[] = options?.filterFields ?? [];

    const refUrl = this._buildUrl({
      ...options,
      filterFields: filterFields,
    });

    if (!options?.force && this._lastUrl === refUrl) return;

    this._lastUrl = refUrl;
    const fn = async (
      bounds: number[]
    ): Promise<ParcelFeatureData[] | undefined> => {
      if (!this.enableGetFeaturesData) {
        console.log('Get features data disabled');
        return;
      }
      if (!this._map) return;
      const url = this._buildUrl({
        bounds,
        filterFields,
        ...options,
      });
      console.log('=== loading features data ===');
      const { data } = await axios.get<{
        featuresDataCollections: { featuresData: ParcelFeatureData[] }[];
      }>(url, {
        cancelToken: new axios.CancelToken((c) => {
          this.cancelRequest = c;
        }),
        timeout: this._reqTimeout,
      });
      return data.featuresDataCollections[0]?.featuresData;
    };
    const t = Date.now();
    let retVal: ParcelFeatureData[] | undefined;

    if (options?.bounds?.length) {
      retVal = await fn(options?.bounds);
    } else if (mapZoom > FEATURES_DATA_SPLIT_ZOOM) {
      retVal = await fn(parcelUtil.getMapBbox(this._map));
    } else {
      const parts = (FEATURES_DATA_SPLIT_ZOOM - Math.floor(mapZoom)) * 4;
      const width = window.innerWidth;
      const height = window.innerHeight;
      const arr: number[][] = [];
      for (let i = 1; i <= parts; i++) {
        const right = i < parts ? Math.floor((width / parts) * i) : width;
        const left = i === 1 ? 0 : right - width / parts;
        //Right,top
        const unproject1 = this._map.unproject([right, 0]);
        const unproject2 = this._map.unproject([left, height]);
        arr.push([
          unproject1.lng,
          unproject1.lat,
          unproject2.lng,
          unproject2.lat,
        ]);
      }

      retVal = (await Promise.all(arr.map((val) => fn(val)))).flatMap(
        (val) => val ?? []
      );
    }

    if (retVal?.length) {
      this._featuresDataLoaded(this._map, options.mapEventName)(retVal);
    }

    if (options.mapEventName) {
      const diff = (Date.now() - t) / 1000;
      console.log(
        `=== Features data loaded for ${options.mapEventName} =  ${
          retVal?.length ?? 0
        } rows`
      );
      console.log('Elapsed time ' + diff + ' seconds');
    }

    return retVal;
  };

  private _featuresDataLoaded = (
    map: mapboxgl.Map,
    mapEventName?: GetFeaturesDataOptions['mapEventName']
  ) => (data: ParcelFeatureData[]): void => {
    try {
      const rowsDictionary: Record<
        string,
        ParcelFeatureData & { parcelData?: ParcelFeatureData }
      > = {};
      const rule = mapService.selectedUserMap?.mapRenderers.find(
        (renderer) => renderer.rendererRules.layer.id === AppLayers.PARCEL_LAYER
      )?.rendererRules.colorRule?.rule;
      const features = parcelUtil.getRenderedFeatures();

      for (const parcelInfo of data) {
        const pin = parcelInfo.Major + parcelInfo.Minor;
        rowsDictionary[pin] = cloneDeep(parcelInfo);
        for (const [k, v] of Object.entries(parcelInfo)) {
          if (rule?.columnName === k && rule.columnType === 'date') {
            const k1 = k as keyof ParcelFeatureData;
            rowsDictionary[pin][k1] = new Date(v).getTime();
          }
        }
        rowsDictionary[pin].parcelData = parcelInfo;
      }

      for (const featureGroup of Object.values(
        groupBy(features, (val) => val.id)
      )) {
        const parcelData: ParcelFeatureData =
          rowsDictionary[featureGroup[0]?.properties?.PIN];
        if (parcelData) {
          map.setFeatureState(featureGroup[0], parcelData);
        }
      }
      if (mapEventName) {
        this._map?.fire(mapEventName, { data });
      }
      this.$onFeaturesDataLoaded.next(data);
    } catch (e) {
      $onError.next('An error occurred after getting feature data');
      throw e;
    }
  };

  /**
   * Builds the url for loadFeatureData request
   * @param map - mapbox-gl map instance
   * @returns -Feature data formed request and map zoom
   */
  private _buildUrl = ({
    bounds,
    filterFields,
    datasetId,
    filterDatasetId,
    mapEventName,
  }: BuildUrlOptions): string => {
    if (!this._map) throw new Error('Invalid mapboxgl map');
    const zoom = this._map.getZoom();
    const url = new URL(
      process.env.REACT_APP_MAP_TILE_SERVICE_HOST +
        'v1.0/extentFeatureData/parcel' +
        '/' +
        [zoom.toFixed(0), ...(bounds ?? parcelUtil.getMapBbox(this._map))].join(
          '/'
        )
    );

    url.searchParams.set('datasetid', datasetId);

    if (filterDatasetId && filterDatasetId !== datasetId) {
      url.searchParams.set('filterDatasetId', filterDatasetId);
    }

    if (filterFields?.length) {
      url.searchParams.set('select', filterFields.join());
    }

    if (mapEventName) {
      url.searchParams.set('mapEventName', mapEventName);
    }
    return url.toString();
  };
}
