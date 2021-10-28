/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import MapboxDraw from '@mapbox/mapbox-gl-draw';
import {
  AllGeoJSON,
  bbox,
  bboxPolygon,
  Feature,
  featureCollection,
  midpoint,
  Point,
  point,
  Position,
} from '@turf/turf';
import { debounce, round } from 'lodash';
import mapboxgl from 'mapbox-gl';
import { utilService } from 'services/common';
import { mapUtilService } from 'services/map';

export class CircleTextService {
  private _sourceId = 'circle-text';
  private _map: mapboxgl.Map;
  private _draw: MapboxDraw;
  private _features: Feature<Point>[] = [];
  static instance: CircleTextService | undefined;
  constructor(map: mapboxgl.Map, draw: MapboxDraw) {
    this._map = map;
    this._draw = draw;
    if (CircleTextService.instance) return CircleTextService.instance;
    CircleTextService.instance = this;
    this._map?.addSource(this._sourceId, {
      type: 'geojson',
      data: featureCollection([]) as GeoJSON.FeatureCollection<
        GeoJSON.Geometry
      >,
    });

    this._map.addLayer({
      id: this._sourceId,
      type: 'symbol',
      source: this._sourceId,
      paint: {
        'text-translate': [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          ['literal', [0, -10]],
          22,
          ['literal', [0, -32]],
        ],
      },
      layout: {
        'text-field': ['coalesce', ['get', 'text-field'], ''],
        'text-size': ['coalesce', ['get', 'text-size'], 14],
        'icon-allow-overlap': true,
      },
    });
  }

  render = debounce((): void => {
    this._features = [];
    const _features = this._draw
      .getAll()
      .features.filter((_feature) => _feature?.properties?.isCircle);

    for (const _feature of _features) {
      if (!_feature.properties?.drawn) return;
      console.log(_feature);
      const _bboxPolygon = bboxPolygon(bbox(_feature as AllGeoJSON));
      const point1 = point(
        (_bboxPolygon?.geometry?.coordinates[0][3] ?? [0, 0]) as Position
      );
      const point2 = point(
        (_bboxPolygon?.geometry?.coordinates[0][2] ?? [0, 0]) as Position
      );
      const _midpoint = midpoint(point1, point2);
      _midpoint.properties = {
        'text-field': `Diameter: ${round(
          utilService.kilometersToFeet(_feature.properties.radiusInKm),
          2
        )} feet`,
      };

      this._features.push(_midpoint);
    }
    mapUtilService.setSource(this._map)(this._sourceId, this._features);
  }, 100);
}
