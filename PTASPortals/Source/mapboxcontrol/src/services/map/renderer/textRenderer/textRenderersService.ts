/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  featureCollection,
  Feature,
  Polygon,
  MultiPolygon,
  Properties,
  Point,
  bbox,
  point,
  midpoint,
  BBox,
  union,
  bboxPolygon,
  polygon,
} from '@turf/turf';
import { AppLayers } from 'appConstants';
import {
  ClassTagLabel,
  FILTER_TYPES,
  LineQuery,
} from 'components/MapLayersManage/TextRenderer/typing';
import { debounce, groupBy, uniqueId } from 'lodash';
import mapboxgl, { CirclePaint, GeoJSONSource } from 'mapbox-gl';
import { SymbolPtasLayer } from 'services/map';
import { DotFeature } from 'services/map/dot';
import { $onMapReady } from 'services/map/mapServiceEvents';
import labelBackground from './labelBackground';
import selectionService from 'services/parcel/selection';
import { layerService } from 'services/map/layerService';

type LabelRecordItem = {
  parcelFields?: string[];
  layerId: string;
  minZoom: number;
  maxZoom: number;
  refLayerId: string;
};

type LabelRecord = Record<string, LabelRecordItem>;

type FeatureProps = Properties & { layerId: string };
export interface RendererLabel {
  layer: SymbolPtasLayer;
  labelConfig: ClassTagLabel;
}

export const labelDisplayKeys = [
  'one-label-per-shape',
  'one-label-per-part',
  'fits-in-shape',
] as const;

type LabelFeature = Feature<Point, FeatureProps>;

/**
 * Calculate and render labels
 */
class TextRenderersService {
  map?: mapboxgl.Map;
  private _features: Map<string, LabelFeature[]> = new Map();
  labels: LabelRecord = {};
  testFeatures: Feature[] = [];
  constructor() {
    $onMapReady.subscribe(this._onMapReady);
  }

  triggerRedrawLabels = debounce(() => {
    this?.map?.fire('redraw-labels');
  }, 300);

  private _onMapReady = (map: mapboxgl.Map): void => {
    this.map = map;
    map.on('zoomstart', () => {
      map.once('idle', this.triggerRedrawLabels);
    });
    this._newSource('rend-test');
    // map.addLayer({
    //   filter: ['==', ['get', 'PIN'], '0654000090'],
    //   id: 'rend-test-line',
    //   type: 'line',
    //   source: 'rend-test', //'parcelSource',
    //   // 'source-layer': 'parcel',
    //   layout: {
    //     'line-join': 'round',
    //     'line-cap': 'round',
    //   },
    //   paint: {
    //     'line-color': 'red',
    //     'line-width': 2,
    //   },
    // });
  };

  hasLabels = (): boolean => !!Object.keys(this.labels).length;

  private _newSource = (id: string): void => {
    if (this.map?.getSource(id)) return;
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
    (this.map?.getSource(source) as GeoJSONSource | undefined)?.setData(
      fCollection
    );
  };

  private _splitBBox = (
    bbox: BBox,
    labelIndex: number,
    totalLabels: number
  ): BBox => {
    const [xMin, yMin, xMax, yMax] = bbox;
    if (totalLabels === 1) return bbox;

    const widthCalculated =
      totalLabels > 1 ? (xMax - xMin) / totalLabels : xMax - xMin;
    const minCalculated = xMin + widthCalculated * labelIndex;

    return [minCalculated, yMin, minCalculated + widthCalculated, yMax];
  };

  private _getTextAlign = (vAlign: string, hAlign: string): string =>
    [vAlign, hAlign].join('-');

  private _pointByTextAlign = (
    joined: Feature<Polygon | MultiPolygon, Properties>,
    textAlign: string,
    labelIndex: number,
    allLabels: RendererLabel[]
  ): Feature<Point, Properties> => {
    const bboxFeat = bbox(joined);
    const alignLength = allLabels.filter(
      (item) =>
        this._getTextAlign(
          item.labelConfig.verticalAlign,
          item.labelConfig.horizontalAlign
        ) === textAlign
    ).length;

    const [xMin, yMin, xMax, yMax] = this._splitBBox(
      bboxFeat,
      labelIndex,
      alignLength
    );

    const point1 = point([xMin, yMin]);
    const point2 = point([xMax, yMax]);
    const midPoint = midpoint(point1, point2);
    const center = midPoint.geometry?.coordinates ?? [];
    let pointReturn;
    switch (textAlign.toLowerCase()) {
      case 'top-center':
        pointReturn = point([center[0], yMax]);
        break;
      case 'top-right':
        pointReturn = point([xMax, yMax]);
        break;
      case 'middle-left':
        pointReturn = point([xMin, center[1]]);
        break;
      case 'middle-center':
        pointReturn = point([center[0], center[1]]);
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
        pointReturn = point([xMin, yMax]);
        break;
    }

    return pointReturn;
  };

  private _filterValidation = (
    typeFilter: string,
    featureProps: FeatureProps,
    queryFilter: LineQuery[],
    selectedParcels: DotFeature[]
  ): boolean => {
    if (!typeFilter || !Object.entries(featureProps).length) return true;

    const lineValidation = (line: LineQuery): boolean => {
      try {
        const parsedValue =
          line.type === 'number'
            ? parseInt(line.value as string)
            : (line.value as string);

        switch (line.operator) {
          case '==':
            return featureProps[line.property] === parsedValue;
          case '!=':
            return featureProps[line.property] !== parsedValue;
          case '>':
            return featureProps[line.property] > parsedValue;
          case '<':
            return featureProps[line.property] < parsedValue;
          case '>=':
            return featureProps[line.property] >= parsedValue;
          case '<=':
            return featureProps[line.property] <= parsedValue;
          case 'in':
            return parsedValue.toString().includes(featureProps[line.property]);
          case '!in':
            return !parsedValue
              .toString()
              .includes(featureProps[line.property]);
          default:
            return false;
        }
      } catch (error) {
        return false;
      }
    };

    // filter type = Only select
    if (typeFilter === FILTER_TYPES.onlySelected.value) {
      return (
        !!featureProps['pin'] &&
        selectedParcels.some((sp) => sp.properties.pin === featureProps['pin'])
      );
    }
    // type filter = query
    else if (typeFilter === FILTER_TYPES.useQuery.value) {
      const queries = queryFilter ?? [];

      if (!queries.some((q) => q.property) || !queries.some((q) => q.operators))
        return true;

      const applyOrFilter = queries.some((q) => q.lineOperator === 'or');
      if (applyOrFilter) {
        let groupValidation = true;
        for (const q of queries) {
          if (q.lineOperator === 'or') groupValidation = true;
          if (!lineValidation(q)) groupValidation = false;
        }
        return groupValidation;
      } else {
        const linesResult = queries.map((q) => {
          return lineValidation(q);
        });
        return !linesResult.some((r) => r === false);
      }
    }
    return true;
  };

  private _rendererLabelFilter = (
    currentZoom: number,
    selectedDots: DotFeature[]
  ) => (
    labels: RendererLabel[],
    joined: Feature<Polygon | MultiPolygon, Properties>,
    featureProps: FeatureProps
  ): void => {
    if (!this.map || !labels.length) return;
    const labelsToRender: RendererLabel[] = [];
    for (let i = 0, max = labels.length; i < max; i++) {
      const l = labels[i];
      const labelText = l.labelConfig?.content ?? '';
      const minZoom = l.labelConfig?.minZoom ?? 0;
      const maxZoom = l.labelConfig?.maxZoom ?? 22;
      if (!labelText.length) continue;

      if (
        l.labelConfig?.typeZoom !== 'all' &&
        (currentZoom > maxZoom || currentZoom < minZoom)
      )
        continue;

      const somePropAvailable = Object.entries(featureProps).some(
        ([key, val]) => val && labelText.includes(key)
      );
      if (!somePropAvailable) continue;

      if (!this.labels[l.labelConfig.id].parcelFields?.length) {
        labelsToRender.push(l);
        continue;
      }

      const filterValid = this._filterValidation(
        l.labelConfig?.typeFilter ?? '',
        featureProps,
        l.labelConfig?.queryFilter as LineQuery[],
        selectedDots
      );
      if (!filterValid) {
        continue;
      }

      labelsToRender.push(l);
    }

    labelsToRender.forEach((l, idx) => {
      const featProps: FeatureProps = {
        ...featureProps,
        layerId: l.layer.id,
      };
      this._generateFeatureByLabel(joined, l, featProps, idx, labelsToRender);
    });
  };

  private _generateFeatureByLabel = (
    _feature: Feature<Polygon | MultiPolygon, Properties>,
    label: RendererLabel,
    featureProps: FeatureProps,
    labelIndex: number,
    allLabels: RendererLabel[]
  ): void => {
    if (!label.layer.layout || !label.layer.paint) return;

    const addPoint = (
      _point: Feature<Point, FeatureProps>
    ): number | undefined =>
      this._features.get(label.labelConfig.id)?.push(_point);
    if (label.labelConfig?.typeLabelShow === 'one-label-per-part') {
      if (_feature.geometry?.type === 'MultiPolygon') {
        for (const _coords of _feature.geometry.coordinates) {
          addPoint({
            ...this._pointByTextAlign(
              polygon(_coords),
              this._getTextAlign(
                label.labelConfig.verticalAlign,
                label.labelConfig.horizontalAlign
              ),
              labelIndex,
              allLabels
            ),
            properties: featureProps,
          });
        }
      } else {
        addPoint({
          ...this._pointByTextAlign(
            _feature,
            this._getTextAlign(
              label.labelConfig.verticalAlign,
              label.labelConfig.horizontalAlign
            ),
            labelIndex,
            allLabels
          ),
          properties: featureProps,
        });
      }
    }
    //One label per shape
    else {
      const _bboxPolygon = bboxPolygon(bbox(_feature));
      addPoint({
        ...this._pointByTextAlign(
          _bboxPolygon,
          this._getTextAlign(
            label.labelConfig.verticalAlign,
            label.labelConfig.horizontalAlign
          ),
          labelIndex,
          allLabels
        ),
        properties: featureProps,
      });
    }
  };

  isValidLabelList = (labelList: RendererLabel[]): boolean =>
    !labelList.length ||
    labelList.every(
      (label) =>
        label.labelConfig.content !== undefined &&
        label.labelConfig.content.length
    );

  getParcelLabels = (): LabelRecordItem[] =>
    Object.values(this.labels).filter(
      (label) =>
        label.refLayerId === AppLayers.PARCEL_LAYER &&
        label.parcelFields?.length
    );

  private _getFields = (str: string | undefined): string[] =>
    str
      ?.match(/{[a-zA-Z0-9_]+}/g)
      ?.map((field) => field.replace(/{([a-zA-Z0-9_]+)}/, '$1')) ?? [];

  private _initNewFeatures = (labelList: RendererLabel[]): void => {
    this._features.clear();
    for (const label of labelList) {
      this._features.set(label.labelConfig.id, []);
    }
  };

  private _removeUnusedLabels = (labelList: RendererLabel[]): void => {
    for (const prevLabel of Object.keys(this.labels)) {
      if (labelList.every((newLabel) => newLabel.layer.id !== prevLabel)) {
        this._deleteLabel(prevLabel);
      }
    }
  };

  renderLabels = (labelList: RendererLabel[], hasChanges = true): void => {
    try {
      if (!this.map) return;
      const rendererLabelReducer = this._rendererLabelFilter(
        this.map.getZoom(),
        selectionService.getSelectedDotsList()
      );

      if (!labelList.length) return this._clearAllLabelFeatures();
      this._initNewFeatures(labelList);

      if (hasChanges) {
        this._removeUnusedLabels(labelList);

        for (const label of labelList) {
          if (this.labels[label.layer.id]) {
            this.map.removeLayer(label.layer.id);
            this.map?.removeImage(label.layer.id);
          } else {
            this._newSource(label.layer.id);
          }
          this.labels[label.layer.id] = {
            layerId: label.layer.id,
            refLayerId: label.labelConfig.refLayerId,
            maxZoom: label.layer.maxzoom ?? 22,
            minZoom: label.layer.minzoom ?? 0,
          };
          if (label.labelConfig?.refLayerId === AppLayers.PARCEL_LAYER) {
            const filterFields =
              label.labelConfig.queryFilter?.map((val) => val.property) ?? [];

            const parcelFields = this._getFields(label.labelConfig.content);

            this.labels[label.layer.id]['parcelFields'] = [
              ...parcelFields,
              ...filterFields,
            ];
          }
          const image = labelBackground({
            id: label.layer.id,
            background: label.labelConfig.backgroundColor,
          });
          this.map?.addLayer(label.layer);
          this.map?.addImage(image.id, image.imageData);
        }
      }

      const labelsByLayer = groupBy(
        labelList,
        (val) => val.labelConfig?.refLayerId
      );
      const shouldFilterRefFeatures = labelList.some(
        (label) => this.labels[label.labelConfig.id].parcelFields?.length
      );
      for (const labelGroup of Object.values(labelsByLayer)) {
        const refLayerId = labelGroup[0].labelConfig.refLayerId;
        const refLayerType = labelGroup[0].labelConfig.refLayerType;
        const refFeatures = !this.map.getLayer(refLayerId)
          ? []
          : this.map
              ?.queryRenderedFeatures(undefined, {
                filter: [
                  '==',
                  '$type',
                  refLayerType === 'fill' ? 'Polygon' : 'Point',
                ],
                layers: [refLayerId],
                validate: false,
              })
              .filter((f) => {
                if (
                  shouldFilterRefFeatures &&
                  refLayerId === AppLayers.PARCEL_LAYER
                ) {
                  return !!Object.values(f.state).length;
                }
                return true;
              }) ?? [];

        if (refLayerType === 'fill') {
          const groupedFeatures = groupBy(refFeatures, (b) => b.id);
          for (const _featureGroup of Object.values(groupedFeatures)) {
            const _featureProps: FeatureProps = {
              ...(refLayerId === AppLayers.PARCEL_LAYER
                ? _featureGroup[0].state
                : _featureGroup[0].properties || {}),
              layerId: '',
            };
            try {
              // const p = bboxPolygon(bbox(featureCollection(_featureGroup)));
              // p.properties = _featureGroup[0].properties;
              // this.testFeatures.push(p);
              const joined = union(...(_featureGroup as Feature<Polygon>[]));
              rendererLabelReducer(labelGroup, joined, _featureProps);
            } catch (e) {
              console.error(e);
              // if (_featureGroup[0].properties?.PIN === '0654000120') {
              //   // eslint-disable-next-line no-debugger
              //   debugger;
              // }
              // if (_featureGroup[0].properties?.PIN === '3110500000') {
              //   const l = [];
              //   for (const f of _featureGroup) {
              //     if (f.geometry.type === 'Polygon') {
              //       l.push(
              //         ...(polygonToLine((f as any) as any).geometry
              //           ?.coordinates ?? [])
              //       );
              //     }
              //   }
              //   this.testFeatures.push(lineString(l as any));
              // }
            }
          }
        } else {
          this._addCircleLayerLabels({
            refLayerId,
            refFeatures,
            labels: labelGroup,
          });
        }
      }

      for (const [k, _features] of this._features) {
        if (!_features.length) continue;
        //stuff for stacked parcels
        const label = this.labels[k];
        if (label.refLayerId === AppLayers.PARCEL_LAYER) {
          const stack = new Map<string, LabelFeature[]>();
          const foundLabel = labelList.find(
            (item) => item.layer.id === label.layerId
          );
          for (const f of _features) {
            if (!f.properties) continue;
            f.properties.text = foundLabel?.labelConfig.content;
            if (label.parcelFields?.length) {
              for (const field of label.parcelFields) {
                f.properties.text = f.properties.text.replaceAll(
                  `{${field}}`,
                  f.properties[field]
                );
              }
            }
            if (!f.geometry?.coordinates) continue;
            const mapKey = JSON.stringify(f.geometry.coordinates);
            if (!stack.has(mapKey)) {
              stack.set(mapKey, [f]);
            } else {
              stack.get(mapKey)?.push(f);
            }
          }

          for (let i = 0; i < _features.length; i++) {
            const f = _features[i];
            if (!f.geometry?.coordinates) continue;
            const mapKey = JSON.stringify(f.geometry.coordinates);
            const stackArr = stack.get(mapKey) ?? [];
            if (stackArr.length > 1) {
              _features.splice(i, 1);
              i--;
            } else {
              stack.delete(mapKey);
            }
          }

          for (const [, v] of stack) {
            const newF = v[0];
            if (newF.properties?.text) {
              newF.properties.text = v
                .map((item) => item.properties?.text)
                .join('\n');
            }
            this._features.get(newF.properties?.layerId ?? '')?.push(newF);
          }
        }
        this._setSourceFeatures(_features, k);
      }
      this._setSourceFeatures(this.testFeatures, 'rend-test');
    } catch (e) {
      console.error(e);
    }
  };

  private _addCircleLayerLabels = ({
    refLayerId,
    labels,
    refFeatures,
  }: {
    refFeatures: mapboxgl.MapboxGeoJSONFeature[];
    refLayerId: string;
    labels: RendererLabel[];
  }): void => {
    if (!this.map) return;
    if (!refFeatures.length) return;
    const radius = (layerService.getRenderer(refLayerId)?.rendererRules.layer
      .paint as CirclePaint | undefined)?.['circle-radius'];
    if (typeof radius !== 'number') return;

    const createPoint = (
      label: RendererLabel,
      _feature: mapboxgl.MapboxGeoJSONFeature,
      toPushArr: Feature<Point, FeatureProps>[]
    ): void => {
      if (_feature.geometry.type !== 'Point') return;
      const _point: Feature<Point, FeatureProps> = point(
        _feature.geometry.coordinates,
        {
          ..._feature.properties,
          layerId: label.layer.id,
        }
      );
      _point.id = uniqueId('circle-point-');
      toPushArr?.push(
        this._movePoint(_point, label, radius) as Feature<Point, FeatureProps>
      );
    };

    for (const label of labels) {
      const toPushArr = this._features.get(label.labelConfig.id);
      if (typeof toPushArr !== 'undefined') {
        for (const _feature of refFeatures) {
          createPoint(label, _feature, toPushArr);
        }
      }
    }
  };

  private _movePoint = (
    _point: Feature<Point>,
    label: RendererLabel,
    radius: number
  ): Feature<Point> => {
    const space = process.env.REACT_APP_CIRCLE_LAYER_SPACE
      ? parseInt(process.env.REACT_APP_CIRCLE_LAYER_SPACE)
      : 3;
    if (!this.map) throw new Error('map is undefined');
    if (!_point.geometry?.coordinates.length) throw new Error('Invalid point');
    if (radius === 0) {
      radius = space;
    }
    const projected = this.map.project(
      _point.geometry?.coordinates as [number, number]
    );
    if (
      label.labelConfig.verticalAlign === 'Top' ||
      label.labelConfig.verticalAlign === 'Bottom'
    ) {
      const isTop = label.labelConfig.verticalAlign === 'Top';
      const newPos = isTop ? projected.y - radius : projected.y + radius;

      const unprojected = this.map.unproject([projected.x, newPos]);
      _point.geometry.coordinates = [unprojected.lng, unprojected.lat];
    } else if (
      label.labelConfig.horizontalAlign === 'Right' ||
      label.labelConfig.horizontalAlign === 'Left'
    ) {
      const isRight = label.labelConfig.horizontalAlign === 'Right';
      const newPos = isRight ? projected.x + radius : projected.x - radius;

      const unprojected = this.map.unproject([newPos, projected.y]);
      _point.geometry.coordinates = [unprojected.lng, unprojected.lat];
    }

    return _point;
  };

  private _deleteLabel = (labelId: string): void => {
    if (!this.map?.getSource(labelId)) return;
    const labelRecord: LabelRecordItem | undefined = this.labels[labelId];
    if (!labelRecord) return console.warn(`Label ${labelId} doesn't exists`);

    this.map?.removeLayer(labelId);
    this.map?.removeSource(labelId);
    this.map?.removeImage(labelId);
    delete this.labels[labelId];
  };

  private _clearAllLabelFeatures = (): void => {
    for (const k of Object.keys(this.labels)) {
      this._deleteLabel(k);
    }
    this._features.clear();
    this.labels = {};
  };
}

export const textRenderersService = new TextRenderersService();
