/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers } from 'appConstants';
import mapboxgl from 'mapbox-gl';
import { Subject } from 'rxjs';

type MapLayerEvent = mapboxgl.MapMouseEvent & {
  features?: mapboxgl.MapboxGeoJSONFeature[] | undefined;
} & mapboxgl.EventData;

export class MapThumbnailService {
  /** See: https://docs.mapbox.com/mapbox-gl-js/api/markers/#popup */
  static popupOptions: mapboxgl.PopupOptions = {
    closeButton: false,
    closeOnMove: false,
    closeOnClick: false,
    maxWidth: '400',
    offset: { top: [0, 10], left: [0, 0] },
  };
  private _enabled = false;
  private readonly _minZoom = 15.5;
  static instance: MapThumbnailService | undefined;
  map: mapboxgl.Map;

  _popUp?: mapboxgl.Popup;
  $onToggle = new Subject<{
    pin: string | undefined;
    coords: [number, number];
  }>();
  coords: [number, number] = [0, 0];
  constructor(map: mapboxgl.Map) {
    this.map = map;
    if (!MapThumbnailService.instance) {
      MapThumbnailService.instance = this;
    } else {
      return this;
    }

    this.map.on('mousemove', AppLayers.PARCEL_LAYER, this._onMousemove);
    map.on('mouseleave', AppLayers.PARCEL_LAYER, this._onMouseLeave);
    this.$onToggle.subscribe(({ pin }) => {
      if (!pin) {
        this._popUp?.remove();
      }
    });
  }

  set enabled(val: boolean) {
    this._enabled = val;
    if (!val) {
      this.$onToggle.next({ pin: undefined, coords: [0, 0] });
    }
  }

  get enabled(): boolean {
    return this._enabled;
  }

  private _onMousemove = (e: MapLayerEvent): void => {
    if (!this.enabled) return;
    if (this.map.getZoom() < this._minZoom)
      return this.$onToggle.next({ pin: undefined, coords: [0, 0] });
    if (!e.features?.[0].properties?.PIN) return;
    const pin = e.features[0].properties.PIN;
    const coordinates: [number, number] = [e.lngLat.lng, e.lngLat.lat];
    // Ensure that if the map is zoomed out such that multiple
    // copies of the feature are visible, the popup appears
    // over the copy being pointed to.
    while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
      coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
    }
    this.coords = coordinates;
    this.$onToggle.next({ pin, coords: coordinates });
  };

  private _onMouseLeave = (_e: MapLayerEvent): void => {
    if (!this.enabled) return;
    this.$onToggle.next({ pin: undefined, coords: [0, 0] });
  };
}
