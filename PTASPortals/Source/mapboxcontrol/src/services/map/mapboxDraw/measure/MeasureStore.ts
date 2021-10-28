/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Feature, Point, featureCollection } from '@turf/turf';
import { DrawLineStringGeometry } from '../modes/types';
import { Subject } from 'rxjs';
import {
  areaLayer,
  distanceLayers,
  measureSourceId,
  PointPosition,
} from './layers';

type CommonProps = DrawLineStringGeometry['properties'] & {
  'icon-image'?: string;
  'icon-size'?: number;
  feet?: number;
  refId?: string;
  text: string;
};

export type AreaProps = CommonProps & {
  layer: 'area';
};

type DistanceProps = CommonProps & {
  layer: PointPosition;
};

type GenFeature<T extends DistanceProps | AreaProps> = Omit<
  Feature<Point>,
  'id' | 'properties'
> & {
  id: string;
  properties: T;
};

export type AreaFeature = GenFeature<AreaProps>;
export type DistanceFeature = GenFeature<DistanceProps>;
type MeasureFeature = AreaFeature | DistanceFeature;

export class MeasureStore {
  store: Map<string, MeasureFeature[]> = new Map();
  $onChange = new Subject<Map<string, MeasureFeature[]>>();
  map: mapboxgl.Map;
  constructor(_map: mapboxgl.Map) {
    this.map = _map;
  }

  addSource = (): void => {
    this.map.addSource(measureSourceId, {
      type: 'geojson',
      data: featureCollection([]) as GeoJSON.FeatureCollection<
        GeoJSON.Geometry
      >,
    });

    for (const layer of Object.values(distanceLayers)) {
      this.map.addLayer(layer);
    }
    this.map.addLayer(areaLayer);
    this.addSource = (): boolean => false;
  };

  applyChanges = (): void => {
    const source = this.map.getSource(measureSourceId) as
      | mapboxgl.GeoJSONSource
      | undefined;
    if (source) {
      source?.setData(
        featureCollection(
          [...this.store].reduce<Feature<Point>[]>(
            (prev, [, currFeatures]) => [...prev, ...currFeatures],
            []
          )
        ) as GeoJSON.FeatureCollection<GeoJSON.Geometry>
      );
      this.$onChange.next(this.store);
    }
  };

  clear = (): void => {
    this.store.clear();
    this.applyChanges();
  };

  remove = (key: string, options?: { applyChanges: boolean }): void => {
    this.store.delete(key);
    if (options?.applyChanges === false) return;
    this.applyChanges();
  };

  set = (
    id: string,
    features: MeasureFeature[],
    options?: { applyChanges: boolean }
  ): void => {
    this.store.set(id, features);
    if (options?.applyChanges === false) return;
    this.applyChanges();
  };

  get = <T extends AreaFeature | DistanceFeature>(
    id: string
  ): T[] | undefined => this.store.get(id) as T[];
}
