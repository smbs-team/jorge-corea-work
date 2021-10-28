// mapUtilService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { featureCollection, Feature } from '@turf/turf';
import mapboxgl, { GeoJSONSource } from 'mapbox-gl';

/**
 * An utility for map related stuff
 */
class MapUtilService {
  /**
   * A flag that indicates if control key is pressed
   *
   */
  controlKeyPressed = false;

  /**
   * Fly to specified coordinates
   * @param map -The mapboxgl instance
   * @param coords - The longitude and latitude
   */
  mapFly = (map: mapboxgl.Map, [lon, lat]: [number, number]): void => {
    map.flyTo({
      center: [lon, lat],
      zoom: 17.5,
      duration: 0,
    });
  };

  setSource = (map: mapboxgl.Map) => (
    k: string,
    v: Feature[] | GeoJSON.FeatureCollection
  ): void => {
    const source = map.getSource(k) as GeoJSONSource | undefined;
    if (!source) {
      console.error('NOTICE: source ' + k + ' not found');
      return;
    }
    source.setData(
      Array.isArray(v)
        ? (featureCollection(v as Feature[]) as GeoJSON.FeatureCollection)
        : v
    );
  };

  bindCtlClickEvent = (): void => {
    document.addEventListener('mousedown', (e: MouseEvent) => {
      this.controlKeyPressed = e.ctrlKey || e.metaKey;
    });
  };
}

export const mapUtilService = new MapUtilService();
