// annotationLabelService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  AllGeoJSON,
  bbox,
  Feature,
  featureCollection,
  midpoint,
  MultiPolygon,
  point,
  Point,
  Polygon,
  Properties,
  truncate,
  union,
} from '@turf/turf';
import { FONTS, RGBA_BLACK } from 'appConstants';
import { debounce, groupBy } from 'lodash';
import mapboxgl, { GeoJSONSource, SymbolLayer } from 'mapbox-gl';
import { layerService } from '../layerService';
import { SearchService } from '../searchService';
import annotationClassesData from './annotationClassesData.json';

type FeatureProps = Properties & {
  refLayerId: string;
  layerId: string;
  textRotate: number;
  text: string;
  symbolSortKey: number;
  textSize: number;
  textFont: string[];
  textLetterSpacing: number;
  textAnchor: mapboxgl.Anchor;
};

const ANNO_LAYER_PROP_NAMES = {
  ANGLE: 'ANGLE',
  BOLD: 'BOLD',
  CHARACTERSPACING: 'CHARACTERSPACING',
  FONTNAME: 'FONTNAME',
  FONTSIZE: 'FONTSIZE',
  HORIZONTALALIGNMENT: 'HORIZONTALALIGNMENT',
  ITALIC: 'ITALIC',
  TEXTSTRING: 'TEXTSTRING',
  VERTICALALIGNMENT: 'VERTICALALIGNMENT',
  ZORDER: 'ZORDER',
  ANNOTATIONCLASSID: 'ANNOTATIONCLASSID',
};

export class AnnotationLabelService {
  public static instance: AnnotationLabelService | undefined;
  /**
   * The mapbox map instance
   *
   */
  map: mapboxgl.Map;

  layerBaseId = 'annotationLabelLayer';

  features: Feature<Point, FeatureProps>[] = [];

  constructor(map: mapboxgl.Map) {
    this.map = map;
    if (AnnotationLabelService.instance) return AnnotationLabelService.instance;
    AnnotationLabelService.instance = this;
    this.map.on('moveend', () => {
      this.map.once('idle', () => {
        this.onLayerAdded();
      });
    });
    layerService.$onLayerAdded.subscribe((val) => {
      if (this.isAnnoLayer(val.id)) {
        setTimeout(() => {
          this.onLayerAdded();
        }, 4000);
      }
    });

    layerService.$onRemoveLayer.subscribe((val) => {
      const featureGroups = groupBy(
        this.features,
        (val) => val.properties?.refLayerId
      );
      for (const [k, v] of Object.entries(featureGroups)) {
        const first = v.shift();
        if (k === val.id) {
          this._removeLayer(first?.properties?.layerId ?? '');
          this._removeSource(first?.properties?.layerId ?? '');
        }
      }
      this.features = this.features.filter(
        (f) => f.properties?.refLayerId !== val.id
      );
    });
  }

  /**
   *
   * The map search instance
   */
  public mapSearch: SearchService | undefined;

  private _newSource = (id: string): void => {
    this.map?.addSource(id, {
      type: 'geojson',
      data: {
        type: 'FeatureCollection',
        features: [],
      },
    });
  };

  private _setSourceFeatures = (_features: Feature[], source: string): void => {
    const fCollection = featureCollection(
      _features
    ) as GeoJSON.FeatureCollection;
    if (!this.map?.getSource(source)) return;
    (this.map?.getSource(source) as GeoJSONSource).setData(fCollection);
  };

  private _removeSource = (id: string): void => {
    if (!this.map?.getSource(id)) return;
    this.map.removeSource(id);
  };

  private _removeLayer = (id: string): void => {
    const { map } = this;
    if (!map.getLayer(id)) return;
    map.removeLayer(id);
  };

  private _getFeatures = (
    mainLayerId: string
  ): mapboxgl.MapboxGeoJSONFeature[] => {
    if (!this.map || !this.map.getLayer(mainLayerId)) return [];
    return this.map.queryRenderedFeatures(undefined, {
      layers: [mainLayerId],
    });
  };

  private _pointByTextAlign = (
    joined: Feature<Polygon | MultiPolygon, Properties>,
    textAlign: string
  ): Feature<Point, Properties> => {
    const bboxFeat = bbox(joined);

    const [xMin, yMin, xMax, yMax] = bboxFeat;

    const point1 = point([xMin, yMin]);
    const point2 = point([xMax, yMax]);
    const midPoint = midpoint(point1, point2);
    const center = midPoint.geometry?.coordinates ?? []; // [x, y]
    let pointReturn;
    switch (textAlign.toLowerCase()) {
      case 'top-left':
        pointReturn = point([xMin, yMax]);
        break;
      case 'top-center':
        pointReturn = point([center[0], yMax]);
        break;
      case 'top-right':
        pointReturn = point([xMax, yMax]);
        break;
      case 'middle-left':
        pointReturn = point([xMin, center[1]]);
        break;
      case 'middle-right':
        pointReturn = point([xMax, center[1]]);
        break;
      case 'bottom-left':
        pointReturn = point([xMin, yMin]);
        break;
      case 'bottom-center':
        pointReturn = point([center[0], yMin]);
        break;
      case 'bottom-right':
        pointReturn = point([xMax, yMin]);
        break;
      default:
        pointReturn = point([center[0], center[1]]);
        break;
    }

    return pointReturn;
  };

  private _generateLayer = (id: string, mainLayerId: string): SymbolLayer => {
    const refLayer = layerService.getRenderer(mainLayerId);
    const newLayer: SymbolLayer = {
      minzoom: refLayer?.rendererRules.layer.minzoom ?? 0,
      maxzoom: refLayer?.rendererRules.layer.maxzoom ?? 22,
      type: 'symbol',
      id: id,
      source: id,
      layout: {
        'text-field': [
          'format',
          ['get', 'text'],
          {
            'text-font': ['get', 'textFont'],
            'font-scale': 1,
          },
        ],
        visibility: 'visible',
        'text-rotate': ['get', 'textRotate'],
        'symbol-sort-key': ['get', 'symbolSortKey'],
        'text-letter-spacing': ['get', 'textLetterSpacing'],
        'text-size': [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          [
            '/',
            ['*', 0.3098, ['get', 'textSize']],
            ['/', 78271.484, ['^', 2, 1]],
          ],
          22,
          [
            '/',
            ['*', 0.3098, ['get', 'textSize']],
            ['/', 78271.484, ['^', 2, 22]],
          ],
        ],
        'text-anchor': ['get', 'textAnchor'],
        'text-allow-overlap': true,
        'text-max-width': 20,
      },
      paint: {
        'text-color': ['get', 'textColor'],
      },
    };

    return newLayer;
  };

  private _addPoint = (_point: Feature<Point, FeatureProps>): void => {
    this.features.push(_point);
  };

  private _addAnnoLayer = (layerId: string, mainLayerId: string): void => {
    console.log('Add annoLayer');
    const { map } = this;
    if (!map) return;
    const {
      ANGLE,
      TEXTSTRING,
      ZORDER,
      CHARACTERSPACING,
      FONTSIZE,
      ANNOTATIONCLASSID,
    } = ANNO_LAYER_PROP_NAMES;

    const currentZoom = map.getZoom();
    const annotationColors =
      (annotationClassesData.find((al) => al.layerId === mainLayerId) ?? {})
        .classes ?? [];
    if (!this.map.getSource(layerId)) {
      this._newSource(layerId);
    }

    const refFeatures = this._getFeatures(mainLayerId);
    const grouped = groupBy(refFeatures, (b) => b.id);
    if (refFeatures.length && !this.map.getLayer(layerId)) {
      const newLayer = this._generateLayer(layerId, mainLayerId);
      map.addLayer(newLayer);
    }
    for (const [, featureGroup] of Object.entries(grouped)) {
      const propertiesFound = featureGroup[0].properties || {};
      const textAlign = 'middle-center';
      const textAnchor = 'center';
      // Temporarily unavailable
      // const isBold = !!parseInt(BOLD ?? '0');
      // const isItalic = !!parseInt(ITALIC ?? '0');
      // const font = `${propertiesFound[FONTNAME] ?? 'Arial'} ${
      //   isBold ? 'Bold' : 'Regular'
      // } ${isItalic ? 'Italic' : ''}`;

      try {
        const joined = union(
          ...(featureGroup.map((f) => truncate(f as AllGeoJSON)) as Feature<
            Polygon
          >[])
        );
        if (joined.geometry?.type === 'MultiPolygon')
          throw new Error('Invalid polygon in annotations');

        const fontSize = parseInt(propertiesFound[FONTSIZE] ?? '16');
        const textSize = (currentZoom / 22 + 1) * fontSize;
        const textColor =
          (
            annotationColors.find(
              (al) => al.id === propertiesFound[ANNOTATIONCLASSID]
            ) ?? {}
          ).color ?? RGBA_BLACK;

        const _featureProps: FeatureProps = {
          ...propertiesFound,
          refLayerId: mainLayerId,
          layerId: layerId,
          textRotate: parseInt(propertiesFound[ANGLE] ?? '0') * -1,
          text: propertiesFound[TEXTSTRING] ?? '',
          symbolSortKey: parseInt(propertiesFound[ZORDER] ?? '0'),
          textLetterSpacing: parseInt(propertiesFound[CHARACTERSPACING] ?? '0'),
          textSize,
          textColor,
          textFont: [FONTS.OPEN_SANS_REGULAR],
          textAnchor: textAnchor as mapboxgl.Anchor,
        };

        this._addPoint({
          ...this._pointByTextAlign(joined, textAlign),
          properties: _featureProps,
        });
      } catch (e) {
        console.error(e);
      }
    }
    const featuresByLayer = this.features.filter(
      (f) => f.properties?.layerId === layerId
    );
    if (featuresByLayer.length) {
      this._setSourceFeatures(featuresByLayer, layerId);
    }
  };

  onLayerAdded = debounce(() => {
    const annoLayers = layerService.renderers.flatMap((l) =>
      this.isAnnoLayer(l.rendererRules.layer.id)
        ? [l.rendererRules.layer.id]
        : []
    );

    if (!annoLayers.length) return;
    this.features = [];
    annoLayers.forEach((al, idx) => {
      this._addAnnoLayer(`${this.layerBaseId}-${idx}`, al);
    });
  }, 500);

  isAnnoLayer = (id: string): boolean => /^ca[0-9]Layer/.test(id);
}
