/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';
import {
  distance as turfDistance,
  round,
  Feature,
  LineString,
  lineString,
  Polygon,
  getCoords,
  point,
  lineToPolygon,
  area as turfArea,
  Point,
  polygonToLine,
  Properties,
  inside,
  midpoint,
} from '@turf/turf';
import { debounce, flow, omit, uniqueId } from 'lodash';
import MapboxDraw from '@mapbox/mapbox-gl-draw';
import polylabel from 'polylabel';
import {
  directModeEvt,
  drawLineStringEvt,
  DrawLineStringSubjectProps,
} from '../modes';
import { utilService } from 'services/common';
import { $drawAreaUpdated, mapBoxDrawService } from '../mapboxDrawService';
import measureCircle from './measureCircle';
import roundRectImg from 'assets/img/round-rect.png';
import {
  AreaFeature,
  AreaProps,
  DistanceFeature,
  MeasureStore,
} from './MeasureStore';
import { Subject } from 'rxjs';
import vertexIcon from 'assets/img/measure-vertex.png';
import { PointPosition } from './layers';
export * from './MeasureStore';

export type MeasureAreaText = {
  featureId: string;
  text: string;
};

const posOpposite: {
  [k in PointPosition]: PointPosition;
} = {
  left: 'right',
  right: 'left',
  top: 'bottom',
  bottom: 'top',
};

export const $areaTextChange = new Subject<MeasureAreaText | undefined>();

const feetTagMargins = {
  top: -35,
  bottom: 35,
  left: -35,
  right: 35,
};

export class MeasureService extends MeasureStore {
  enabled = false;
  areaText?: MeasureAreaText;
  circleImage = measureCircle('circle-measure-image');
  roundRectId = 'round-rect';
  draw: MapboxDraw;

  constructor(map: mapboxgl.Map, draw: MapboxDraw) {
    super(map);
    this.draw = draw;
    this._addImages();

    drawLineStringEvt.clickAnywhere.subscribe(this._lineStringClickAnyWhere);
    drawLineStringEvt.clickOnVertex.subscribe(this._lineStringClickOnVertex);

    directModeEvt.dragVertex.subscribe(this._onDragFeature);
    directModeEvt.dragFeature.subscribe(this._onDragFeature);

    $areaTextChange.subscribe((val) => {
      this.areaText = val;
    });
    $drawAreaUpdated.subscribe(this._drawAreaUpdated);
  }

  alignPointTo = (
    _point: DistanceFeature,
    options: {
      pos: PointPosition;
      margin?: number;
    }
  ): Feature<Point> => {
    const projected = this.map?.project(
      (_point.geometry?.coordinates ?? [0, 0]) as [number, number]
    );

    if (
      projected?.x !== undefined &&
      projected?.y !== undefined &&
      _point?.geometry?.coordinates
    ) {
      const _margin = options.margin ?? feetTagMargins[options.pos];
      if (options.pos === 'top' || options.pos === 'bottom') {
        projected.y = projected.y + _margin;
      } else {
        projected.x = projected.x + _margin;
      }
      const unProjected = this.map?.unproject([projected.x, projected.y]);
      return point([unProjected?.lng ?? 0, unProjected?.lat ?? 0]);
    }
    throw new Error('Invalid projected coordinates or given point coordinates');
  };

  private _drawAreaUpdated = debounce(() => {
    if (!this.enabled) return;
    const drawnIds = this.draw.getAll().features.map((item) => item.id);
    if (!drawnIds.length) {
      this.clear();
      if (this.areaText) {
        $areaTextChange.next();
      }
    } else {
      for (const [k] of this.store) {
        if (!drawnIds.includes(k)) {
          if (this.areaText?.featureId === k) {
            $areaTextChange.next();
          }
          this.remove(k, {
            applyChanges: false,
          });
        }
      }
      this.applyChanges();
    }
  }, 100);

  private _addImages = (): void => {
    this.map.loadImage(
      roundRectImg,
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
        this.map.addImage(this.roundRectId, image);
      }
    );

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    this.map.loadImage(vertexIcon, (error: any, image: any) => {
      if (error) throw error;
      this.map.addImage('vertex-icon', image);
    });

    this.map.addImage(this.circleImage.id, this.circleImage.imageData);
  };

  /**
   *
   * Calculates point position
   *
   * @param point1 - Feature point1
   * @param point2 - Feature point1
   *
   * @returns - The point position
   */
  private _getStyleKey = (
    point1: Feature<Point>,
    point2: Feature<Point>
  ): PointPosition => {
    const screenCoords1 = this.map.project(
      (point1?.geometry?.coordinates ?? [0, 0]) as [number, number]
    );
    const screenCoords2 = this.map.project(
      (point2?.geometry?.coordinates ?? [0, 0]) as [number, number]
    );

    if (screenCoords1?.x === undefined) return 'top';
    if (screenCoords1?.y === undefined) return 'top';
    if (screenCoords2?.x === undefined) return 'top';
    if (screenCoords2?.y === undefined) return 'top';

    const xDiff = screenCoords2.x - screenCoords1.x;
    const yDiff = screenCoords2.y - screenCoords1.y;
    if (Math.abs(xDiff) > Math.abs(yDiff)) {
      if (xDiff > 0) {
        return 'top';
      }
      return 'bottom';
    } else {
      if (yDiff > 0) {
        return 'right';
      }
      return 'left';
    }
  };

  private _getMidPointFor = (refId: string) => (
    point1: Feature<Point>,
    point2: Feature<Point>
  ): DistanceFeature => {
    const currLength = turfDistance(point1, point2);
    const feet = flow(utilService.kilometersToFeet, (n) => round(n))(
      currLength
    );

    return {
      ...midpoint(point1, point2),
      properties: {
        feet,
        text: Intl.NumberFormat('en-US').format(feet) + '\nfeet',
        layer: this._getStyleKey(point1, point2),
        refId,
        'icon-image': this.circleImage.id,
      },
      id: uniqueId('line-measure-point-'),
    };
  };

  translatePoints = (
    points: DistanceFeature[],
    _polygon: Feature<Polygon, Properties>
  ): DistanceFeature[] => {
    const firstPoint = points[0];
    const pointsInside =
      _polygon.geometry?.coordinates.length &&
      inside(
        this.alignPointTo(firstPoint, {
          pos: firstPoint.properties.layer,
        }),
        _polygon
      );
    if (!pointsInside) return points;
    return points.map((p) => ({
      ...p,
      properties: {
        ...p.properties,
        layer: posOpposite[p.properties.layer],
      },
    }));
  };

  private _getLineMeasurePoints = (
    line: Feature<LineString>,
    _polygon?: Feature<Polygon, Properties>,
    recalculate = true
  ): DistanceFeature[] => {
    if (!this.enabled) return [];
    if (!line.geometry) return [];
    if (getCoords(line).length === 1) return [];
    if (typeof line.id !== 'string') return [];
    const getMidPoint = this._getMidPointFor(line.id ?? '');
    const points: DistanceFeature[] = recalculate
      ? []
      : this.get<DistanceFeature>(line.id) ?? [];
    const max = getCoords(line).length - 1;
    for (let i = 0; i < max; i++) {
      if (points[i]) continue;
      const point1 = point(line.geometry.coordinates[i]);
      const point2 = point(line.geometry.coordinates[i + 1]);
      const midPoint = getMidPoint(point1, point2);
      if (
        midPoint.properties?.feet !== undefined &&
        midPoint.properties.feet > 0
      ) {
        points.push(midPoint);
      }
    }
    return points;
  };

  private _onDragFeature = debounce(({ state }) => {
    if (!this.enabled) return;
    const featureId = state.featureId;
    const { ctx, ..._geometry } = state.feature;

    const _lineString =
      _geometry?.type === 'LineString'
        ? lineString(_geometry.coordinates)
        : _geometry.type === 'Polygon'
        ? polygonToLine(_geometry as Polygon)
        : undefined;
    if (!_lineString) return;

    if (!_lineString.properties) {
      _lineString.properties = {};
    }

    const _polygon =
      _geometry.type === 'Polygon'
        ? { ...lineToPolygon(_lineString), id: featureId }
        : undefined;

    const _points =
      this._getLineMeasurePoints(
        {
          ..._lineString,
          id: featureId,
        },
        _polygon
      ) ?? [];

    this.set(featureId, _points);
    if (_polygon !== undefined) {
      this._createAreaText(_polygon);
    }
  }, 500);

  private _lineStringClickAnyWhere = ({
    state,
  }: DrawLineStringSubjectProps): void => {
    if (!this.enabled) return;
    if (state.currentVertexPosition === 1) return;

    const distancePoints = this._getLineMeasurePoints({
      ...lineString(state.line.coordinates),
      id: state.line.id,
      properties: {
        'icon-image': this.circleImage.id,
      },
    });

    const perimeter = distancePoints.reduce(
      (prev, curr) => prev + (curr?.properties?.feet ?? 0),
      0
    );
    this.set(state.line.id, distancePoints);
    state.line.ctx.api.setFeatureProperty(
      state.line.id,
      'perimeter',
      perimeter
    );
  };

  private _lineStringClickOnVertex = ({
    state,
    mode,
  }: DrawLineStringSubjectProps): void => {
    if (!this.enabled) return;
    if (state.line.coordinates.length < 4) return;
    if (state.currentVertexPosition !== state.line.coordinates.length) return;
    const _polygon = lineToPolygon(omit(state.line, 'ctx'));
    if (!_polygon) return;
    console.log('Click on vertex');
    const modePolygon = mode.newFeature({
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      ...(_polygon as any),
      id: state.line.id,
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    }) as Polygon;
    const _lineString = lineString(
      state.line.coordinates,
      state.line.properties
    );
    _lineString.id = state.line.id;
    const points = this.translatePoints(
      this._getLineMeasurePoints(_lineString, _polygon, false),
      _polygon
    );
    this.set(state.line.id, points);
    this._createAreaText({
      ..._polygon,
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      id: state.line.id,
    } as Feature<Polygon, AreaProps>);
    mode.addFeature(modePolygon);
  };

  private _createAreaText = (_polygon: Feature<Polygon>): void => {
    if (typeof _polygon.id !== 'string') return;
    const text = flow(
      turfArea,
      utilService.squareMetersToSquareFeet,
      (n) => round(n, 2),
      Intl.NumberFormat('en-US').format,
      (val) => val + '\nsq ft'
    )(_polygon);

    const midCoords = polylabel(getCoords(_polygon), 1.0);
    const _midPoint: AreaFeature = {
      id: _polygon.id,
      type: 'Feature',
      geometry: {
        type: 'Point',
        coordinates: [midCoords[0], midCoords[1]],
      },
      properties: {
        text,
        'icon-image': this.roundRectId,
        refId: _polygon.id,
        layer: 'area',
      },
    };
    this.get(_polygon.id)?.push(_midPoint);
    this.applyChanges();
    console.log(this.get(_polygon.id));
  };

  start = (): void => {
    mapBoxDrawService.setMode('draw_line_string');
    this.enabled = true;
    this.addSource();
    this.clear();
  };

  stop = (): void => {
    this.enabled = false;
    this.clear();
    mapBoxDrawService.deleteAll();
    mapBoxDrawService.setMode('simple_select');
  };
}
