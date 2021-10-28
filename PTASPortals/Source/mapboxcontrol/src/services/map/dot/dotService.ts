/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { CircleLayer, SymbolLayer, SymbolLayout } from 'mapbox-gl';
import { Feature, featureCollection, Point } from '@turf/turf';
import { Subject } from 'rxjs';
import { RGBA_BLACK } from 'appConstants';
import { mapUtilService } from '../mapUtilService';

import locationRedIcon from 'assets/img/location-red.png';
import locationBlueIcon from 'assets/img/location-blue.png';
import locationYellowIcon from 'assets/img/location-yellow.png';
import starIcon from 'assets/img/star.png';

export type DotFeatureState = {
  selected?: boolean;
};

type DotCommonProps = Pick<
  SymbolLayout,
  | 'icon-image'
  | 'icon-anchor'
  | 'icon-size'
  | 'icon-text-fit'
  | 'text-field'
  | 'text-size'
> & {
  major: string;
  minor: string;
  pin: string;
  circleColor?: string;
  layer?: DotLayerKey;
  refId?: string;
  render?: boolean;
};

export type DataSetDotProps = DotCommonProps & {
  dotType: 'dataset-dot';
  selected?: boolean;
  selectionType?: 'click-selection' | 'polygon-selection' | 'feature-data';
  prev?: Omit<DotDsFeature, 'properties'> & {
    properties: Omit<DataSetDotProps, 'prev'>;
  };
};

type SearchResultDotProps = DotCommonProps & {
  dotType: 'search';
  isEconomicUnit?: boolean;
};

export type DotFeature = Omit<Feature<Point>, 'id' | 'properties'> & {
  id: string;
  properties: DataSetDotProps | SearchResultDotProps;
  state?: DotFeatureState;
};

export type DotDsFeature = Omit<DotFeature, 'properties'> & {
  properties: DataSetDotProps;
};

export type DotSearchFeature = Omit<DotFeature, 'properties'> & {
  properties: SearchResultDotProps;
};

enum DotSource {
  'dot-service-data-set' = 'dot-service-data-set',
  'dot-service-search' = 'dot-service-search',
  'dot-service-helper' = 'dot-service-helper',
  'dot-service-selected' = 'dot-service-selected',
}

export type DotSourceKey = keyof typeof DotSource;

enum DotLayer {
  'dot-dataset-circle' = 'dot-dataset-circle',
  'dot-dataset-icon' = 'dot-dataset-icon',
  'dot-search-circle' = 'dot-search-circle',
  'dot-search-icon' = 'dot-search-icon',
  'dot-helper-icon' = 'dot-helper-icon',
  'dot-service-selected-icon' = 'dot-service-selected-icon',
  'dot-service-selected-circle' = 'dot-service-selected-circle',
}

export type DotLayerKey = keyof typeof DotLayer;

export type DotStore = Record<DotSourceKey, Map<string, DotFeature>>;

export type FoundInSources = {
  source: DotSourceKey;
  dot: DotFeature;
};

class DotService {
  images = {
    locationRed: 'location-red',
    locationBlue: 'location-blue',
    locationYellow: 'location-yellow',
    starIcon: 'star-icon',
  } as const;
  layers = DotLayer;
  sources = DotSource;
  dotColors = {
    red: 'rgb(255,100,100)',
    blue: 'rgb(0,255,255)',
  };

  map: mapboxgl.Map | undefined;

  //source id,pin, feature
  readonly store = {
    [DotSource['dot-service-data-set']]: new Map<string, DotFeature>(),
    [DotSource['dot-service-search']]: new Map<string, DotFeature>(),
    [DotSource['dot-service-helper']]: new Map<string, DotFeature>(),
    [DotSource['dot-service-selected']]: new Map<string, DotFeature>(),
  };

  $onChange = new Subject<DotStore>();

  init(map: mapboxgl.Map): void {
    this.map = map;
    this._addSources();
  }

  private _addSources = (): void => {
    //add sources
    for (const key of Object.keys(this.store)) {
      this.map?.addSource(key, {
        type: 'geojson',
        data: featureCollection([]) as GeoJSON.FeatureCollection<
          GeoJSON.Geometry
        >,
      });
    }

    const iconLayer: SymbolLayer = {
      id: DotLayer['dot-dataset-icon'],
      type: 'symbol',
      source: DotSource['dot-service-data-set'],
      filter: [
        'any',
        [
          '==',
          ['coalesce', ['get', 'layer'], ''],
          DotLayer['dot-dataset-icon'],
        ],
        ['all', ['has', 'icon-image'], ['==', ['has', 'layer'], false]],
      ],
      layout: {
        'icon-text-fit': 'both',
        'icon-image': ['get', 'icon-image'],
        'icon-anchor': ['coalesce', ['get', 'icon-anchor'], 'center'],
        'icon-size': ['coalesce', ['get', 'icon-size'], 0.8],
        'text-field': ['coalesce', ['get', 'text-field'], ''],
        'text-size': ['coalesce', ['get', 'text-size'], 16],
        'icon-allow-overlap': true,
        'icon-offset': [0, 15],
        'icon-ignore-placement': true,
        'icon-padding': 8,
      },
    };

    const circleLayer: CircleLayer = {
      id: DotLayer['dot-dataset-circle'],
      type: 'circle',
      source: DotSource['dot-service-data-set'],
      filter: [
        'any',
        [
          '==',
          ['coalesce', ['get', 'layer'], ''],
          DotLayer['dot-dataset-circle'],
        ],
        ['all', ['has', 'circleColor'], ['==', ['has', 'layer'], false]],
      ],
      paint: {
        'circle-color': ['get', 'circleColor'],
        'circle-radius': [
          'interpolate',
          ['linear'],
          ['zoom'],
          // zoom is 10 (or less) -> circle radius will be 6px
          13,
          5,
          // zoom is 12.1 (or greater) -> circle radius will be 10px
          13.1,
          10,
        ],
        'circle-stroke-color': RGBA_BLACK,
        'circle-stroke-width': 1,
      },
    };

    //add layer

    this.map?.addLayer(circleLayer);
    this.map?.addLayer(iconLayer);

    //Search layers

    this.map?.addLayer({
      ...iconLayer,
      source: DotSource['dot-service-search'],
      id: DotLayer['dot-search-icon'],
      filter: [
        'any',
        ['==', ['coalesce', ['get', 'layer'], ''], DotLayer['dot-search-icon']],
        ['all', ['has', 'icon-image'], ['==', ['has', 'layer'], false]],
      ],
    });

    this.map?.addLayer({
      ...circleLayer,
      source: DotSource['dot-service-search'],
      id: DotLayer['dot-search-circle'],
      filter: [
        'any',
        [
          '==',
          ['coalesce', ['get', 'layer'], ''],
          DotLayer['dot-search-circle'],
        ],
        ['all', ['has', 'circleColor'], ['==', ['has', 'layer'], false]],
      ],
    });

    //Selected dots layers

    this.map?.addLayer({
      ...circleLayer,
      source: DotSource['dot-service-selected'],
      id: DotLayer['dot-service-selected-circle'],
      filter: [
        'any',
        [
          '==',
          ['coalesce', ['get', 'layer'], ''],
          DotLayer['dot-service-selected-circle'],
        ],
        ['all', ['has', 'circleColor'], ['==', ['has', 'layer'], false]],
      ],
    });

    this.map?.addLayer({
      ...iconLayer,
      source: DotSource['dot-service-selected'],
      id: DotLayer['dot-service-selected-icon'],
      filter: [
        'any',
        [
          '==',
          ['coalesce', ['get', 'layer'], ''],
          DotLayer['dot-service-selected-icon'],
        ],
        ['all', ['has', 'icon-image'], ['==', ['has', 'layer'], false]],
      ],
    });

    // Helper layers

    this.map?.addLayer({
      ...iconLayer,
      source: DotSource['dot-service-helper'],
      id: DotLayer['dot-helper-icon'],
      filter: [
        'any',
        ['==', ['coalesce', ['get', 'layer'], ''], DotLayer['dot-helper-icon']],
        ['all', ['has', 'icon-image'], ['==', ['has', 'layer'], false]],
        ['has', 'text-field'],
      ],
    });

    /**
     * location red icon
     */

    this.map?.loadImage(
      locationRedIcon,
      (
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        error: any,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        image: any
      ) => {
        if (error) {
          console.error(error);
          return;
        }
        this.map?.addImage(this.images.locationRed, image);
      }
    );

    /**
     * location blue
     */

    this.map?.loadImage(
      locationBlueIcon,
      (
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        error: any,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        image: any
      ) => {
        if (error) {
          console.error(error);
          return;
        }
        this.map?.addImage(this.images.locationBlue, image);
      }
    );

    /**
     * location yellow
     */

    this.map?.loadImage(
      locationYellowIcon,
      (
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        error: any,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        image: any
      ) => {
        if (error) {
          console.error(error);
          return;
        }
        this.map?.addImage(this.images.locationYellow, image);
      }
    );

    //star icon, for economic unit

    this.map?.loadImage(
      starIcon,
      (
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        error: any,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        image: any
      ) => {
        if (error) {
          console.error(error);
          return;
        }
        this.map?.addImage(this.images.starIcon, image);
      }
    );
  };

  getFeaturesListFor = (sourceId: DotSourceKey): DotFeature[] => {
    const storeData = this.store[sourceId];
    return [...storeData].map(([, v]) => v);
  };

  findInSources = (
    id: string,
    options?: { omit?: DotSourceKey; filterHidden?: boolean }
  ): FoundInSources | undefined => {
    const filterHidden = !!options?.filterHidden;
    for (const [k, v] of Object.entries(this.store)) {
      const val = v.get(id);
      if (val && k !== options?.omit) {
        if (filterHidden) {
          if (
            val.properties.render === undefined ||
            val.properties.render === true
          ) {
            return {
              source: k as DotSourceKey,
              dot: val,
            };
          }
        } else {
          return {
            source: k as DotSourceKey,
            dot: val,
          };
        }
      }
    }
  };

  set = ({
    source,
    dotKey,
    dot,
  }: {
    source: DotSourceKey;
    dotKey: string;
    dot: DotFeature;
  }): Map<string, DotFeature> | undefined =>
    this.store[source]?.set(dotKey, dot);

  remove = (source: DotSourceKey, id: string): boolean | undefined =>
    this.store[source]?.delete(id);

  setSource = (k: string, v: DotFeature[]): void => {
    this.map &&
      mapUtilService.setSource(this.map)(
        k,
        v.filter((val) => val.properties.render ?? true)
      );
  };

  render = (source?: DotSourceKey | DotSourceKey[]): void => {
    if (!this.map) return;
    if (Array.isArray(source)) {
      for (const k of source) {
        this.setSource(k, this.getFeaturesListFor(k));
      }
    } else if (source) {
      const _features = this.getFeaturesListFor(source);
      this.setSource(source, _features);
    } else {
      for (const [k, v] of Object.entries(this.store)) {
        this.setSource(
          k,
          [...v].map(([, v1]) => v1)
        );
      }
    }
    this.$onChange.next(this.store);
    console.log('dotService.render ' + source);
  };
}

export const dotService = new DotService();
