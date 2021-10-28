/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';
import { chain, groupBy } from 'lodash';
import { utilService } from 'services/common';
import { AppLayers } from 'appConstants';
import { MapRenderer, ParcelFeature, ParcelFeatureData } from 'services/map';
import {
  area,
  BBox,
  bbox,
  centerOfMass,
  Feature,
  featureCollection,
  MultiPolygon,
  Point,
  polygon,
  Polygon,
  Properties,
  union,
} from '@turf/turf';
import { $onMapReady } from 'services/map/mapServiceEvents';

class ParcelUtil {
  map: mapboxgl.Map | undefined;
  constructor() {
    $onMapReady.subscribe((map) => {
      this.map = map;
    });
  }
  /**
   *Verifies if the provided value meets this 001600-0525  or 0016000525 parcel id format
   *
   */
  isPin = (val: string): boolean => /^([0-9]{10}|[0-9]{6}-[0-9]{4})$/.test(val);

  /**
   * Verifies if value is multiple parcel search input
   *
   * @param val - The parcel pin to be tested
   * @returns The boolean test result
   *
   */
  isMultipleParcelsPin = (val: string): boolean =>
    chain(val)
      .thru((v) => v.split(','))
      .thru((v) => v.length > 1 && v.every((item) => this.isPin(item)))
      .value();

  /**
   * Get all parcel rendered features
   *
   * @returns All parcel rendered features as parcel model array
   */
  getRenderedFeatures = (): ParcelFeature[] => {
    return (this.map?.queryRenderedFeatures(undefined, {
      layers: [AppLayers.PARCEL_LAYER],
      validate: false,
    }) as ParcelFeature[])
      .filter((item) => item.properties.PIN && this.isPin(item.properties.PIN))
      .map((item) =>
        utilService.filterObject(
          new ParcelFeature(item),
          ([, v]) => v !== undefined
        )
      );
  };

  /**
   * Get point parcel feature
   *
   * @param point -A mapbox point representation
   * @returns In case of match, the matched feature, otherwise undefined
   */
  getFeaturesByPoint = (point: mapboxgl.Point): ParcelFeature[] | undefined =>
    this.map
      ?.queryRenderedFeatures((point as unknown) as mapboxgl.Point, {
        layers: [AppLayers.PARCEL_LAYER],
        validate: false,
      })
      .map((item) => new ParcelFeature(item as ParcelFeature));

  /**
   * Find features with a provided parcel pin number
   *
   * @param pin -The parcel pin
   * @returns In case of match, an array with features that have the given pin otherwise an empty array
   */
  getFeaturesByPin = (pin: string): ParcelFeature[] => {
    return (this.map?.queryRenderedFeatures(undefined, {
      layers: [AppLayers.PARCEL_LAYER],
      filter: ['==', 'PIN', pin],
      validate: false,
    }) as ParcelFeature[]).map((item) => new ParcelFeature(item));
  };

  /**
   *Given an array of parcel features, group them by id
   *
   * @param  features - Parcel features to group.
   * @returns An object with key that it's the id of the feature, and a value that is an array of features with it id
   */
  groupFeaturesById = (
    features: ParcelFeature[]
  ): Record<string, ParcelFeature[]> => groupBy(features, (b) => b.id);

  /**
   *
   * Piped parcel search query
   *
   * @remarks
   * It is a string array because whe have the possibility to search by multiple parcels pins
   * @returns string array with the search query
   */
  urlParcelsQuery = ((): string[] => {
    const url = new URL(window.location.href);
    const searchParams = new URLSearchParams(url.search);
    const filterAddress = (item: string): string => {
      const matchedResult = item.match(/[0-9]{4,6}-[0-9]{4}/);
      return Array.isArray(matchedResult)
        ? matchedResult.shift() || item
        : item;
    };
    const arr = (searchParams.get('parcelsQuery') || '')
      .split(',')
      .filter((item) => item.length)
      .map(filterAddress);

    return arr.length > 1
      ? arr.filter((item) => item && this.isPin(item))
      : arr;
  })();

  formatPin = (pin: string): string =>
    pin.replace(/(\d{6})(?=(\d{4})+)/g, '$1-');

  isParcelRenderer = (layer?: MapRenderer): boolean =>
    layer?.rendererRules.layer.id === AppLayers.PARCEL_LAYER;

  isEconomicUnitSearch =
    new URLSearchParams(window.location.href).get('isEconomicUnit') === 'true';

  getRenderedParcelData = (pin: string): ParcelFeatureData | undefined =>
    this.getFeaturesByPin(pin).shift()?.state.parcelData;

  polygons = (
    _features: Feature<Polygon | MultiPolygon, Properties>[]
  ): Feature<Polygon, Properties>[] =>
    _features.flatMap((_feature) =>
      _feature.geometry?.type === 'MultiPolygon'
        ? _feature.geometry.coordinates.map((val) => polygon(val))
        : [_feature]
    ) as Feature<Polygon, Properties>[];

  /**
   *
   * @param features -The features to calculate the point
   * @returns -A point feature or undefined in case of error
   */
  getPointFromFeatures = (
    features: Feature<Polygon | MultiPolygon, Properties>[],
    props?: Properties
  ): Feature<Point> | undefined => {
    try {
      const _feature: Feature<Polygon | MultiPolygon, Properties> = union(
        ...this.polygons(features)
      );
      if (!_feature.geometry) return;
      if (_feature.geometry?.type === 'MultiPolygon') {
        const _polygons = (_feature.geometry?.coordinates)
          .map((coords) => {
            const _polygon = polygon(coords);
            if (!_polygon.properties) {
              _polygon.properties = {};
            }
            _polygon.properties.area = area(_polygon);
            return _polygon;
          })
          .sort((a, b) => {
            const a1 = a.properties?.area || 0;
            const b1 = b.properties?.area || 0;
            if (a1 > b1) return -1;
            if (a1 < b1) return 1;
            return 0;
          });
        return centerOfMass(_polygons[0], props ?? features[0].properties);
      }

      return centerOfMass(_feature, props ?? features[0].properties);
    } catch (e) {
      console.error('getPointFromFeatures error');
      console.error(e);
      return;
    }
  };

  getMidPointFromPin = (pin: string): Feature<Point, Properties> | undefined =>
    this.getPointFromFeatures(this.getFeaturesByPin(pin));

  getParcelBbox = (pin: string): BBox | undefined => {
    const _polygons:
      | mapboxgl.MapboxGeoJSONFeature[]
      | undefined = this.map?.queryRenderedFeatures(undefined, {
      filter: ['==', 'PIN', pin],
      layers: [AppLayers.PARCEL_LAYER],
    });
    return _polygons?.length ? bbox(featureCollection(_polygons)) : undefined;
  };

  getMapBbox = (map: mapboxgl.Map): number[] => {
    const bounds = map.getBounds();
    return [
      bounds.getNorthEast().lng,
      bounds.getNorthEast().lat,
      bounds.getSouthWest().lng,
      bounds.getSouthWest().lat,
    ];
  };

  hasParcelLayer = (map?: mapboxgl.Map): boolean =>
    !!map?.getLayer(AppLayers.PARCEL_LAYER);
}

export const parcelUtil = new ParcelUtil();
