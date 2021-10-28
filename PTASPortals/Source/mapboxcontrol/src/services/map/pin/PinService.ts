// pinService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl, { MapMouseEvent, MapEventType, LngLat } from 'mapbox-gl';
import { uniqueId } from 'lodash';
import { utilService } from 'services/common';

const PIN_DEFAULT_COLOR = '#3FB1CE';
const { createElementFromHTML } = utilService;

export class PinService {
  private static _marker?: mapboxgl.Marker;
  private _map: mapboxgl.Map;

  constructor(map: mapboxgl.Map) {
    this._map = map;
  }

  private _getPinHtml = (id: string, location: LngLat): HTMLDivElement => {
    const { lat, lng } = location;
    const text = `lat: ${lat} \nlng: ${lng}`;
    return createElementFromHTML<HTMLDivElement>(`
            <a
                id='${id}'
                href="http://maps.google.com/maps?q=&layer=c&cbll=${lat},${lng}&cbp=11,0,0,0,0"
                target="_blank"
            >
              <div class='pin' title="${text}"></div>
            </a>
     `);
  };

  public static removeMarker = (): void => {
    PinService._marker?.remove();
    PinService._marker = undefined;
  };

  public static hasMarker = (): boolean => PinService._marker !== undefined;

  createMarker = (e: MapMouseEvent): void => {
    const id = uniqueId('pinMarker-');
    const el = this._getPinHtml(id, e.lngLat);
    PinService._marker = new mapboxgl.Marker({
      draggable: false,
      element: el,
    })
      .setLngLat(e.lngLat)
      .addTo(this._map);

    window.open(
      `http://maps.google.com/maps?q=&layer=c&cbll=${e.lngLat.lat},${e.lngLat.lng}&cbp=11,0,0,0,0`,
      '_blank'
    );
  };

  placePinMove = (): void => {
    this._map.getCanvas().style.cursor = 'crosshair';
  };

  placePinClick = (e: MapEventType['click']): void => {
    this._map.getCanvas().style.cursor = '';
    this._map.off('mousemove', this.placePinMove);
    this.createMarker(e);
  };

  placePin = (): void => {
    this._map.off('mousemove', this.placePinMove);
    this._map.off('click', this.placePinMove);
    this._map.on('mousemove', this.placePinMove);
    this._map.once('click', this.placePinMove);
  };

  setCenterWithPinToMap = (
    lat: number,
    lng: number,
    pinId?: string,
    onClickPin?: () => void,
    pinColor?: string
  ): void => {
    if (pinId) {
      PinService._marker?.remove();
    }
    PinService._marker = new mapboxgl.Marker({
      draggable: false,
      color: pinColor ?? PIN_DEFAULT_COLOR,
    })
      .setLngLat({ lat, lng } as LngLat)
      .addTo(this._map);
    if (onClickPin) {
      PinService._marker.getElement().addEventListener('click', onClickPin);
    }
    this._map.setCenter({ lat, lng } as LngLat);
  };

  setPinToMap = (
    lat: number,
    lng: number,
    pinId?: string,
    onClickPin?: () => void,
    pinColor?: string
  ): void => {
    if (pinId) {
      PinService._marker?.remove();
    }

    PinService._marker = new mapboxgl.Marker({
      draggable: false,
      color: pinColor ?? PIN_DEFAULT_COLOR,
    })
      .setLngLat({ lat, lng } as LngLat)
      .addTo(this._map);
    if (onClickPin) {
      PinService._marker.getElement().addEventListener('click', onClickPin);
    }
  };
}
