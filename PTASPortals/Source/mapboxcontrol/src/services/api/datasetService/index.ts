/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DEFAULT_DATASET_ID, QUERY_PARAM } from 'appConstants';
import {
  FeatureDataService,
  GetFeaturesDataOptions,
} from './FeatureDataService';
import { layerService } from 'services/map/layerService';
import { textRenderersService } from 'services/map/renderer/textRenderer/textRenderersService';
import { ParcelFeatureData, rendererService } from 'services/map';
import { axiosMapTileInstance } from '../axiosInstances';
import { utilService } from 'services/common';
import DsSelection from './DsSelection';
import { parcelUtil } from 'utils';

export type DataSetParcelItem = {
  centroid: [number, number];
  inSurfacePointCoords: [number, number];
  pin: string;
  major: string;
  minor: string;
  selected: 'selected' | 'notSelected' | 'all' | 'unknown';
};

type LoadParcelInfoOptions = Pick<GetFeaturesDataOptions, 'force'>;

class DatasetService {
  DsSelection: DsSelection;
  featuresData: {
    [k in 'colorRenderer' | 'label']: FeatureDataService;
  } = {
    colorRenderer: new FeatureDataService(),
    label: new FeatureDataService(),
  };

  filterSelectedKey = {
    selected: 'selected',
    notSelected: 'notSelected',
  };

  constructor() {
    this.DsSelection = new DsSelection();
  }

  getSelectedValue = (
    filterSelected?: string | null
  ): DataSetParcelItem['selected'] => {
    if (
      filterSelected === this.filterSelectedKey.selected ||
      filterSelected === this.filterSelectedKey.notSelected
    )
      return filterSelected as DataSetParcelItem['selected'];
    if (filterSelected === null) return 'all';
    return 'unknown';
  };

  getDataSetParcels = async (
    datasetId: string,
    filterSelected?: string | null
  ): Promise<DataSetParcelItem[]> => {
    const queryParams: {
      filter?: string;
    } = {};

    if (filterSelected !== null) {
      queryParams.filter = filterSelected;
    }
    const r = (
      await axiosMapTileInstance.get<
        {
          Long: number;
          Lat: number;
          Major: string;
          Minor: string;
          InSurfaceLong: number;
          InSurfaceLat: number;
        }[]
      >(`/datasetGeoLocation/${datasetId}`, {
        params: queryParams,
        timeout: 60000 * 5,
        transformResponse: (res) => {
          try {
            const retVal = JSON.parse(res);
            return retVal.featuresDataCollections?.shift()?.featuresData ?? [];
          } catch (e) {
            return res;
          }
        },
      })
    ).data;
    const selected = this.getSelectedValue(filterSelected);
    return r.map((item) => ({
      centroid: [item.Long, item.Lat],
      inSurfacePointCoords: [item.InSurfaceLong, item.InSurfaceLat],
      pin: item.Major + item.Minor,
      selected,
      major: item.Major,
      minor: item.Minor,
    }));
  };

  getLabelsData = async (
    options?: Pick<GetFeaturesDataOptions, 'force'>
  ): Promise<ParcelFeatureData[] | undefined> => {
    if (!parcelUtil.map) return;
    const currentZoom = parcelUtil.map.getZoom();
    const parcelLabels = textRenderersService.getParcelLabels();
    if (
      parcelLabels.every(
        (label) => currentZoom < label.minZoom || currentZoom > label.maxZoom
      )
    )
      return;
    if (!parcelLabels.length) return;
    const labelFields = [
      ...new Set([
        ...FeatureDataService.requiredFilterFields,
        ...Object.values(textRenderersService.labels).flatMap(
          (label) => label.parcelFields ?? []
        ),
      ]),
    ];
    console.log('Get labels data');
    return await this.featuresData.label.getFeaturesData({
      mapEventName: 'label-feature-data-loaded',
      force: options?.force,
      datasetId: DEFAULT_DATASET_ID,
      filterFields: labelFields,
    });
  };

  getColorRendererData = async (
    options?: Pick<GetFeaturesDataOptions, 'force'>
  ): Promise<ParcelFeatureData[] | undefined> => {
    const parcelRenderer = layerService.getParcelRenderer();
    if (!parcelRenderer) return;
    const ds =
      rendererService.getDatasetId() ??
      utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID);
    if (ds) {
      const filterFields = [
        ...new Set([...FeatureDataService.requiredFilterFields]),
      ];

      if (parcelRenderer.rendererRules.colorRule?.rule.columnName) {
        filterFields.push(
          parcelRenderer.rendererRules.colorRule.rule.columnName
        );
      }

      if (ds !== DEFAULT_DATASET_ID) {
        filterFields.push('Selection');
      }
      return await this.featuresData.colorRenderer.getFeaturesData({
        mapEventName: 'color-rend-feature-data-loaded',
        force: options?.force,
        datasetId: ds,
        filterFields,
        filterDatasetId: utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID),
      });
    }
  };
  /**
   *
   * @param options - Features data options, with pick options to select just one of the request
   */
  loadParcelsInfo = async (options?: LoadParcelInfoOptions): Promise<void> => {
    this.getColorRendererData();
    this.getLabelsData(options);
  };
}

export const datasetService = new DatasetService();
