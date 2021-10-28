/* eslint-disable @typescript-eslint/no-explicit-any */
// types.d.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Feature, Point, Polygon, LineString } from '@turf/turf';
import MapboxDraw from '@mapbox/mapbox-gl-draw';
import { LngLat } from 'mapbox-gl';

export type SelectEvent = {
  featureTarget: {
    properties: {
      user_disableMove?: boolean;
    };
  };
};

export type ModeFnEvent = {
  point: {
    x: number;
    y: number;
  };
  lngLat: Pick<LngLat, 'lat' | 'lng'>;
  originalEvent: MouseEvent;
  type: string;
};

/**
 * Drawing modes
 */
export type DrawMode =
  | 'draw_circle'
  | 'draw_line_string'
  | 'draw_polygon'
  | 'draw_point'
  | 'simple_select'
  | 'direct_select'
  | 'static';

/**
 * Mode prototyped props
 */
export interface ModeAccessor {
  _ctx: MapboxDraw;
  setSelected: (features: GeoJSON.Feature[]) => void;
  setSelectedCoordinates: (coords: unknown) => void;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  getSelected: () => any;
  getSelectedIds: () => string[];
  isSelected: (id: string) => boolean;
  getFeature: (id: string) => GeoJSON.Feature | undefined;
  select: (id: string) => GeoJSON.Feature | undefined;
  deselect: (id: string) => GeoJSON.Feature | undefined;
  deleteFeature: (id: string) => GeoJSON.Feature | undefined;
  addFeature: (feature: Polygon | LineString | Point) => GeoJSON.Feature;
  clearSelectedFeatures: () => void;
  clearSelectedCoordinates: () => unknown;
  setActionableState: (actions: object) => unknown;
  changeMode: (mode: string, opts?: object, eventOpts?: object) => unknown;
  updateUIClasses: (opts: object) => unknown;
  activateUIButton: (name: string) => unknown;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  featuresAt: (event: any, bbox: any, bufferType?: string) => unknown;
  newFeature: (GeoJSON: GeoJSON.Feature) => unknown;
  isInstanceOf: (type: string, feature: GeoJSON.Feature) => boolean;
  doRender: (id: string) => unknown;
}

/**
 * Mode base methods
 */
export interface BaseMode<T, U extends ModeFnEvent | SelectEvent | object>
  extends ModeAccessor {
  readonly map: mapboxgl.Map;
  /**
   * Triggered while a mode is being transitioned into.
   *
   * @param opts  - This is the object passed via `draw.changeMode('mode', opts)`;
   * @returns This object will be passed to all other life cycle functions
   */
  onSetup: {
    (opts?: T): void;
  };
  /**
   * Triggered when a drag event is detected on the map
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  onDrag: { (state: T, e: U): void };
  /**
   * Triggered when the mouse is clicked
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  onClick: { (state: T, e: U): void };
  /**
   * Triggered with the mouse is moved
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  onMouseMove: { (state: T, e: U): void };
  /**
   * Triggered when the mouse button is pressed down
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onMouseDown: { (state: T, e: U): void };
  /**
   * Triggered when the mouse button is released
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onMouseUp: { (state: T, e: U): void };
  /**
   * Triggered when the mouse leaves the map's container
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onMouseOut: { (state: T, e: U): void };
  /**
   * Triggered when a key up event is detected
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onKeyUp: { (state: T, e: U): void };
  /**
   * Triggered when a key down event is detected
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onKeyDown: { (state: T, e: U): void };
  /**
   * Triggered when a touch event is started
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onTouchStart: { (state: T, e: U): void };

  /**
   * Triggered when one drags their finger on a mobile device
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onTouchMove: { (state: T, e: U): void };
  /**
   * Triggered when one removes their finger from the map
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onTouchEnd: { (state: T, e: U): void };
  /**
   * Triggered when one quickly taps the map
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  onTap: { (state: T, e: U): void };
  /**
   * Triggered when the mode is being exited, to be used for cleaning up artifacts such as invalid features
   *
   * @param state - A mutable state object created by onSetup
   */
  onStop: { (state: T): void };
  /**
   * Triggered when [draw.trash()](https://github.com/mapbox/mapbox-gl-draw/blob/master/API.md#trash-draw) is called.
   *
   * @param state - A mutable state object created by onSetup
   */
  onTrash: { (state: T): void };
  /**
   * Triggered when [draw.combineFeatures()](https://github.com/mapbox/mapbox-gl-draw/blob/master/API.md#combinefeatures-draw) is called.
   *
   * @param state - A mutable state object created by onSetup
   */
  onCombineFeature: { (state: T, e: U): void };
  /**
   * Triggered when [draw.uncombineFeatures()](https://github.com/mapbox/mapbox-gl-draw/blob/master/API.md#uncombinefeatures-draw) is called.
   *
   * @param state - A mutable state object created by onSetup
   */
  onUncombineFeature: { (state: T): void };
  /**
   * Triggered per feature on render to convert raw features into set of features for display on the map
   * See [styling draw](https://github.com/mapbox/mapbox-gl-draw/blob/master/API.md#styling-draw) for information about what geojson properties Draw uses as part of rendering.
   *
   * @param state - A mutable state object created by onSetup
   * @param geojson  - a geojson being evaluated. To render, pass to `display`.
   * @param display - all geojson objects passed to this be rendered onto the map
   */
  toDisplayFeatures: {
    (
      state: T,
      geojson: Feature<LineString> | Feature<Polygon> | Feature<Point>,
      display: Function
    ): void;
  };
}

export interface LineStringProto {
  id: string;
  addCoordinate: (path: any, lng: any, lat: any) => unknown;
  getCoordinate: (path: any) => any;
  isValid: () => boolean;
  removeCoordinate: (path: any) => any;
  updateCoordinate: (path: any, lng: any, lat: any) => any;
}

export type PrevEvent = Pick<ModeFnEvent, 'lngLat' | 'point'>;
export type DrawGeometryProps = {
  disableMove?: boolean;
  prevEvent?: PrevEvent;
};

type GeometryBase = {
  ctx: {
    api: MapboxDraw;
  };
};

export type DrawLineStringGeometry = LineString &
  GeometryBase & {
    properties: DrawGeometryProps;
  } & LineStringProto;

export type DrawPolygonGeometry = Polygon &
  GeometryBase & {
    id: string;
    properties: DrawGeometryProps;
  };

export interface ClickAnyWhereState {
  currentVertexPosition: number;
}

export interface DrawPolygonState extends ClickAnyWhereState {
  polygon: DrawPolygonGeometry;
}

export interface DrawLineStringState extends ClickAnyWhereState {
  direction: 'forward' | 'backwards';
  line: DrawLineStringGeometry;
}

export interface DrawPolygon extends BaseMode<DrawPolygonState, ModeFnEvent> {
  clickAnywhere: (state: DrawPolygonState, e: ModeFnEvent) => void;
  clickOnVertex: { (state: DrawPolygonState, e: ModeFnEvent): void };
}

export type DrawLineString = BaseMode<DrawLineStringState, ModeFnEvent> & {
  clickOnVertex: { (state: DrawLineStringState, e: ModeFnEvent): void };
  clickAnywhere: (state: DrawLineStringState, e: ModeFnEvent) => void;
};

export interface SimpleSelect extends BaseMode<object, SelectEvent> {
  /**
   * Triggered on dragging
   *
   * @param state - A mutable state object created by onSetup
   * @param e  - The captured event that is triggering this life cycle event
   */
  dragMove: { (state: any, e: any): void };
  onDrag: { (state: any, e: any): void };
  startDragging: { (state: any, e: any): void };
  startOnActiveFeature: { (state: any, e: any): void };
}

export interface DirectSelect extends BaseMode<object, object> {
  dragMove: { (state: any, e: any): void };
  startDragging: { (state: any, e: any): void };
  stopDragging: { (state: any, e: any): void };
  dragFeature: { (state: any, e: any, delta: any): void };
  dragVertex: { (state: any, e: any, delta: any): void };
  clickNoTarget: { (state: any, e: any): void };
}
