/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import mapboxgl, { LineLayer } from 'mapbox-gl';
import { debounce, uniqBy } from 'lodash';
import { RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';
import { DotFeature, dotService } from 'services/map/dot';
import { parcelUtil } from 'utils';

export class ParcelLineService {
  private _map: mapboxgl.Map;

  constructor(map: mapboxgl.Map) {
    this._map = map;

    const defaultSourceLayer: LineLayer = {
      id: 'parcel-line',
      type: 'line',
      source: 'parcelSource',
      'source-layer': 'parcel',
      layout: {
        'line-join': 'round',
        'line-cap': 'round',
      },
      paint: {
        'line-color': [
          'coalesce',
          ['feature-state', 'lineColor'],
          RGBA_TRANSPARENT,
        ],
        'line-width': 2,
      },
    };

    this._map.addLayer(defaultSourceLayer);

    dotService.$onChange.subscribe(this._updateLines);
  }

  private _getLineColor = (dot: DotFeature | undefined): string => {
    if (!dot?.properties.pin) return RGBA_BLACK;
    if (dot?.properties.circleColor) return dot.properties.circleColor;
    if (
      dot?.properties['icon-image'] === dotService.images.locationRed ||
      dot?.properties['icon-image'] === dotService.images.starIcon
    )
      return dotService.dotColors.red;
    if (
      parcelUtil.isEconomicUnitSearch &&
      parcelUtil.urlParcelsQuery
        .slice(1, parcelUtil.urlParcelsQuery.length)
        .map((val) => val.replace('-', ''))
        .includes(dot?.properties.pin)
    )
      return '#0ED145';

    return RGBA_BLACK;
  };

  private _updateLines = debounce((): void => {
    if (!parcelUtil.hasParcelLayer(this._map)) return;
    const renderedFeatures = uniqBy(
      parcelUtil.getRenderedFeatures(),
      (item) => item.id
    );

    const filteredFeatures = renderedFeatures.filter((val) => {
      const inStore = !!dotService.findInSources(val.properties.PIN);
      const inUrl = parcelUtil.urlParcelsQuery.includes(
        val.properties.PIN.replace('-', '')
      );
      return inStore || inUrl;
    });

    for (const _parcelFeature of renderedFeatures) {
      this._map.setFeatureState(_parcelFeature, {
        lineColor: null,
      });
    }

    for (const _parcelFeature of filteredFeatures) {
      const pin: string = _parcelFeature.properties?.PIN ?? '';
      const foundItem = dotService.findInSources(pin, {
        filterHidden: true,
      });
      if (foundItem) {
        const { dot } = foundItem;
        const lineColor = this._getLineColor(dot);
        this._map.setFeatureState(_parcelFeature, {
          lineColor,
        });
      }
    }
  }, 200);
}
