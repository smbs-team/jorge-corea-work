/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl, { FillLayer, LineLayer } from 'mapbox-gl';

export class BaseMapService {
  private readonly _sourceId = 'default-source';
  static instance: BaseMapService | undefined;
  private _map: mapboxgl.Map;
  readonly useBaseMap =
    process.env.REACT_APP_USE_DEFAULT_MAP === 'true' &&
    !!process.env.REACT_APP_DEFAULT_MAP_URL;
  readonly minZoom = process.env.REACT_APP_DEFAULT_MAP_MIN_ZOOM
    ? parseInt(process.env.REACT_APP_DEFAULT_MAP_MIN_ZOOM)
    : 7;
  readonly url = process.env.REACT_APP_DEFAULT_MAP_URL ?? '';
  private _usingBaseMap = false;

  constructor(map: mapboxgl.Map) {
    if (BaseMapService.instance)
      throw new Error('BaseMapService instance  already created');
    BaseMapService.instance = this;
    this._map = map;
  }

  private _onZoomEnd = (): void => {
    const zoom = this._map.getZoom();
    this._setCompositeLayersVisibility(
      zoom > this.minZoom ? 'none' : 'visible'
    );
  };

  private _setCompositeLayersVisibility = (val: 'visible' | 'none'): void => {
    for (const layer of this._map.getStyle().layers ?? []) {
      if ((layer as FillLayer | LineLayer).source === 'composite') {
        this._map.setLayoutProperty(layer.id, 'visibility', val);
      }
    }
  };
  /**
   * See:
   * https://docs.mapbox.com/mapbox-gl-js/example/wms/
   * https://enterprise.arcgis.com/es/server/latest/publish-services/windows/communicating-with-a-wms-service-in-a-web-browser.htm
   * https://docs.geoserver.org/stable/en/user/styling/mbstyle/source.html
   */
  private _loadDefaultMapSource = (): void => {
    this._usingBaseMap = true;
    //this.setCompositeLayersVisibility('none');
    this._map.addSource(this._sourceId, {
      type: 'raster',
      // use the tiles option to specify a WMS tile source URL
      // https://docs.mapbox.com/mapbox-gl-js/style-spec/sources/
      tiles: [this.url],
      tileSize: 256,
    });
    this._map.addLayer(
      {
        id: this._sourceId,
        type: 'raster',
        source: 'default-source',
        minzoom: this.minZoom,
        paint: {},
        layout: {
          visibility: 'visible',
        },
      },
      'state-label'
    );
  };

  private _enabled = (): void => {
    this._loadDefaultMapSource();
    this._map.on('zoomend', this._onZoomEnd);
  };

  private _disabled = (): void => {
    this._map.removeLayer(this._sourceId);
    this._map.removeSource(this._sourceId);
    this._setCompositeLayersVisibility('visible');
    this._map.off('zoomend', this._onZoomEnd);
  };

  public toggle = (): boolean => {
    if (!this.useBaseMap)
      throw new Error('Error use base map disabled in env variables');
    this._usingBaseMap = !this._usingBaseMap;
    if (this._usingBaseMap) {
      this._enabled();
    } else {
      this._disabled();
    }
    return this._usingBaseMap;
  };
}
