// LayerSource.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { camelCase } from 'lodash';
import {
  VectorSource,
  RasterSource,
  RasterLayer,
  FillLayer,
  LineLayer,
  SymbolLayer,
  CircleLayer,
} from 'mapbox-gl';
import { mapColumnType } from 'utils';
import { ApiColumnType, DataSetColumnType } from './renderer/rendererDataset';

export interface EmbeddedDataFields {
  FieldName: string;
  FieldType: DataSetColumnType;
}

export interface EmbeddedMapped {
  FieldName: string;
  FieldType: ApiColumnType;
}

/**
 * Map Sources vector and raster
 */
export class MapSource
  implements
    Omit<mapboxgl.VectorSource, 'type'>,
    Omit<mapboxgl.RasterSource, 'type'> {
  type: 'vector' | 'raster';
  tiles: string[];
  maxzoom: number | undefined;

  constructor(
    fields: Pick<
      LayerSource,
      'dataSourceUrl' | 'layerSourceType' | 'defaultMaxZoom'
    >
  ) {
    this.tiles = [fields.dataSourceUrl];
    this.type = fields.layerSourceType;
    this.maxzoom = fields.defaultMaxZoom;
  }
}

type LayerMetadata = {
  metadata: {
    scaleMinZoom?: number;
    scaleMaxZoom?: number;
    isSystemRenderer: boolean;
  };
};

export type SymbolPtasLayer = Omit<SymbolLayer, 'metadata'> & LayerMetadata;

export type PtasLayer =
  | SymbolPtasLayer
  | (Omit<FillLayer, 'metadata'> & LayerMetadata)
  | (Omit<LineLayer, 'metadata'> & LayerMetadata)
  | (Omit<RasterLayer, 'metadata'> & LayerMetadata)
  | (Omit<CircleLayer, 'metadata'> & LayerMetadata);

export type LayerSourceRes = Omit<LayerSource, 'embeddedDataFields'> & {
  embeddedDataFields: EmbeddedMapped[] | null;
};

/**
 * Layer configuration model, with map source
 */
export class LayerSource {
  hasOverlapSupport: boolean;
  nativeMapboxLayers: PtasLayer[];
  layerSourceId: number;
  layerSourceName: string;
  layerSourceAlias: string;
  layerSourceType: 'vector' | 'raster';
  jurisdiction: string;
  organization: string;
  description: string;
  isParcelSource: boolean;
  isVectorLayer: number;
  dataSourceUrl: string;
  defaultMapboxLayer: PtasLayer;
  mapSource: VectorSource | RasterSource;
  embeddedDataFields: EmbeddedDataFields[];
  defaultMaxZoom: number | undefined;
  metadata: {
    id: string;
    name: string;
    org: string;
    subject: string;
    dataType: string;
    abstract: string;
    kw: string;
  } | null = null;

  constructor(fields: LayerSourceRes) {
    this.hasOverlapSupport = fields.hasOverlapSupport;
    this.layerSourceId = fields.layerSourceId;
    this.layerSourceName = fields.layerSourceName;
    this.layerSourceAlias = fields.layerSourceAlias;
    this.jurisdiction = fields.jurisdiction;
    this.organization = fields.organization ?? '';
    this.description = fields.description;
    this.isParcelSource = fields.isParcelSource;
    this.isVectorLayer = fields.isVectorLayer;
    this.dataSourceUrl = `${process.env.REACT_APP_MAP_TILE_SERVICE_HOST}${fields.dataSourceUrl}`;
    this.defaultMaxZoom = fields.defaultMaxZoom;

    this.defaultMapboxLayer = {
      ...fields.defaultMapboxLayer,
      layout: {
        ...(fields.defaultMapboxLayer?.layout ?? {}),
        visibility: 'visible',
      },
    };
    this.embeddedDataFields =
      fields.embeddedDataFields?.map((val) => ({
        FieldName: val.FieldName,
        FieldType: mapColumnType(val.FieldType),
      })) ?? [];

    this.layerSourceType = fields.isVectorLayer ? 'vector' : 'raster';
    this.mapSource = new MapSource({
      dataSourceUrl: this.dataSourceUrl,
      layerSourceType: this.layerSourceType,
      defaultMaxZoom: this.defaultMaxZoom,
    });

    this.nativeMapboxLayers = fields.nativeMapboxLayers ?? [];

    if (fields.metadata) {
      this.metadata = (Object.entries(fields.metadata).reduce(
        (prev, [k, v]) => {
          return {
            ...prev,
            [camelCase(k)]: v,
          };
        },
        {}
      ) as unknown) as LayerSource['metadata'];
    }
  }
}
