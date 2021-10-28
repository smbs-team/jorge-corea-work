// mapbox-draw.d.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/* eslint-disable @typescript-eslint/no-explicit-any */

/**
 * See
 * https://github.com/mapbox/mapbox-gl-draw
 * https://github.com/mapbox/mapbox-gl-draw/blob/master/docs/API.md
 * https://github.com/mapbox/mapbox-gl-draw/issues/842
 */

declare module '@mapbox/mapbox-gl-draw' {
  import {
    Geometry,
    Polygon,
    LineString,
    AllGeoJSON,
    Polygon,
  } from '@turf/turf';
  import { Feature, FeatureCollection } from 'geojson';
  import { IControl, EventData } from 'mapbox-gl';
  import {
    DrawMode,
    BaseMode,
    ModeAccessor,
    LineStringProto,
    ModeFnEvent,
    DrawLineStringGeometry,
    ClickAnyWhereState,
    DrawPolygonFeature,
    DrawPolygonState,
    DrawLineStringState,
    DrawPolygon,
    DrawLineString,
    SimpleSelect,
  } from 'services/map/mapboxDraw/modes';

  namespace MapboxDraw {
    export interface Modes {
      simple_select: SimpleSelect;
      direct_select: BaseMode;
      draw_point: BaseMode;
      draw_polygon: DrawPolygon;
      draw_line_string: DrawLineString;
      draw_circle: object;
      drag_circle: object;
      freeHand: object;
    }

    /**
     * Hide or show individual controls.Each property's name is a control, and value is a boolean indicating whether the control is on
     * or off.By default, all controls are on.To change that default, use displayControlsDefault
     * Drawing possible control,mapboxgl-draw have predefined controls, but we can add more with own modes
     */
    export interface MapboxDrawControls {
      /**
       *point control
       */
      point?: boolean;
      /**
       *Line string features control
       */
      line_string?: boolean;
      /**
       *Polygon features control
       */
      polygon?: boolean;
      /**
       *The trash icon
       */
      trash?: boolean;
      /**
       *Whether or not combine features
       */
      combine_features?: boolean;
      /**
       *Whether or not uncombine features
       */
      uncombine_features?: boolean;
    }
    /**
     * Mode state
     */
    export interface ModeState {
      /**
       * Can drag move the feature
       */
      canDragMove: boolean;
      /**
       * DragMoveLocation
       */
      dragMoveLocation: any;
      /**
       * Currently moving
       */
      dragMoving: boolean;
      /**
       * The feature at point
       */
      feature: Feature<Polygon | LineString>;
      /**
       * The feature id
       */
      featureId: string;
      /**
       * Coordinates paths to select
       */
      selectedCoordPaths: any[];
    }
    export let modes: Modes;
  }
  /**
   *Mapbox drawing definition
   */
  class MapboxDraw implements IControl {
    /**
     * Creates an instance of MapboxDraw.
     * Drawing configuration
     */
    constructor(options?: {
      /**
       * The default value for controls. For example, if you would like all controls to be off by default, and specify a whitelist
       * with controls, use displayControlsDefault: false
       */
      displayControlsDefault?: boolean;
      /**
       *Whether or not to enable keyboard interactions for drawing
       */
      keybindings?: boolean;
      /**
       * Whether or not to enable touch interactions for drawing
       */
      touchEnabled?: boolean;
      /**
       * Whether or not to enable box selection of features with shift+click+drag. If false, shift+click+drag zooms into an area
       */
      boxSelect?: boolean;
      /**
       * Number of pixels around any feature or vertex (in every direction) that will respond to a click
       */
      clickBuffer?: number;
      /**
       * Number of pixels around any feature of vertex (in every direction) that will respond to a touch
       */
      touchBuffer?: number;
      /**
       * Hide or show individual controls.
       *
       * @remarks
       * Each property's name is a control, and value is a boolean indicating
       * whether the control is on or off.Available control names are point, line_string, polygon, trash, combine_features and
       * uncombine_features.By default , all controls are on.To change that default , use displayControlsDefault
       */
      controls?: MapboxDraw.MapboxDrawControls;
      /**
       * An array of map style objects. By default, Draw provides a map style for you. To learn about overriding styles
       */
      styles?: object[];
      /**
       * Over ride the default modes with your own. MapboxDraw.modes can be used to see the default values
       */
      modes?: MapboxDraw.Modes;
      /**
       * The mode (from modes) that user will first land in, (default: 'simple_select')
       */
      defaultMode?: string;
      /**
       * Properties of a feature will also be available for styling and prefixed with user_, e.g., ['==', 'user_custom_label', 'Example']
       */
      userProperties?: boolean;
    });

    /**
     * This method takes either a GeoJSON Feature, FeatureCollection, or Geometry and adds it to Draw.
     *
     * @remarks
     * This method returns an array of ids for
     * interacting with the added features.If a feature does not have its own id, one is automatically generated.
     * The supported GeoJSON feature types are supported: Point, LineString, Polygon, MultiPoint, MultiLineString, and MultiPolygon.
     * If you add() a feature with an id that is already in use, the existing feature will be updated and no new feature will be added
     *
     * @param geojson - The Feature,FeatureCollection or Geometry to be added
     * @returns Array string of features ids
     */
    public add(geojson: Feature | FeatureCollection | Geometry): string[];

    /**
     * Returns the GeoJSON feature in Draw with the specified id, or undefined if the id matches no feature
     *
     * @param featureId - The feature id we want to find
     * @returns A found feature or undefined if nothing found
     */
    public get(featureId: string): Feature | undefined;

    /**
     * Returns an array of feature ids for features currently rendered at the specified point.
     *
     * @remarks
     * Notice that the point argument requires x, y coordinates from pixel space, rather than longitude, latitude coordinates.
     * With this function, you can use the coordinates provided by mouse events to get information out of Draw
     *
     * @param point - A point with x and y coordinates
     * @returns String array of matched point features ids
     */
    public getFeatureIdsAt(point: { x: number; y: number }): string[];

    /**
     * Returns an array of feature ids for features currently selected
     *
     * @returns selected features ids
     */
    public getSelectedIds(): string[];

    /**
     * Returns a FeatureCollection of all the features currently selected.
     *
     * @returns Currently selected figure
     */
    public getSelected(): FeatureCollection;

    /**
     * Returns a FeatureCollection of Points representing all the vertices currently selected.
     *
     * @returns Currently selected points
     */
    public getSelectedPoints(): FeatureCollection;

    /**
     * Returns a FeatureCollection of all features
     *
     * @returns FeatureCollection of all features
     */
    public getAll(): FeatureCollection;

    /**
     * Removes features with the specified ids. Returns the draw instance for chaining.
     * In direct_select mode, deleting the active feature will exit that mode and revert to the simple_select mode.
     */
    public delete(ids: string | string[]): this;

    /**
     *Removes all features
     *
     * @returns this instance
     */
    public deleteAll(): this;

    /**
     * Sets Draw's features to those in the specified FeatureCollection.
     *
     * @remarks
     * Performs whatever delete, create, and update actions are necessary to make Draw's features match the specified FeatureCollection.
     * Effectively, this is the same as Draw.deleteAll() followed by Draw.add(featureCollection) except that it does not affect
     * performance as much
     *
     * @param  featureCollection -The features collection to be established
     * @returns String array with the generated features ids
     */
    public set(featureCollection: FeatureCollection): string[];

    /**
     * Invokes the current mode's trash action. Returns the draw instance for chaining.
     *
     * @remarks
     * In simple_select mode, this deletes all selected features.
     * In direct_select mode, this deletes the selected vertices.
     * In drawing modes, this cancels drawing and reverts Draw to simple_select mode.
     * If you want to delete features regardless of the current mode, use the delete or deleteAll function
     *
     * @returns this instance
     */
    public trash(): this;

    /**
     * Invokes the current mode's combineFeatures action
     *
     * @remarks
     * In simple_select mode, this combines all selected features into a single Multi* feature, as long as they are all of
     * the same geometry type.For example:
     * Selection is two LineStrings =\> MultiLineString
     * Selection is a MultiLineString and a LineString =\> MultiLineString
     * Selection is two MultiLineStrings =\> MultiLineString
     * Calling this function when features of different geometry types are selected will not cause any changes. For example:
     * Selection is a Point and a LineString =\> no action taken
     * Selection is a MultiLineString and a MultiPoint =\> no action taken
     * In direct_select mode and drawing modes, no action is taken.
     *
     * @returns Returns the draw instance for chaining
     */

    public combineFeatures(): this;

    /**
     * Invokes the current mode's uncombineFeatures action
     *
     * @remarks
     * In simple_select mode, this splits each selected Multi* feature into its component feature parts, and leaves
     * non-multifeatures untouched.For example:
     * Selection is MultiLineString of two parts =\> LineString, LineString
     * Selection is MultiLineString of three parts =\> LineString, LineString, LineString
     * Selection is MultiLineString of two parts and one Point =\> LineString, LineString, Point
     * Selection is LineString =\> LineString
     * In the direct_select and drawing modes, no action is taken.
     *
     * @returns The draw instance for chaining
     */
    public uncombineFeatures(): this;

    /**
     * Returns Draw's current mode. For more about the modes, see above
     *
     * @returns The current mode
     */
    public getMode(): DrawMode;

    /**
     * Changes Draw to another mode. Returns the draw instance for chaining
     *
     * @returns This instance
     */
    public changeMode(mode: string, options?: object): this;

    /**
     * Sets the value of a property on the feature with the specified id. Returns the draw instance for chaining
     *
     * @param  featureId - The feature id that we want to define a property
     * @param property - The property name
     * @param property - The property value
     * @returns This instance
     */
    public setFeatureProperty(
      featureId: string,
      property: string,
      value: any
    ): this;

    /**
     * Fired when adding a control to the map
     *
     * @param map - mapbox-gl Map instance
     * @returns - The html element
     */
    onAdd(map: mapboxgl.Map): HTMLElement;

    /**
     * Fired when removing a control from the map
     * @param map - mapbox-gl map instance
     */
    onRemove(map: mapboxgl.Map): any;
  }

  export = MapboxDraw;
}

declare module 'mapbox-gl-draw-circle' {
  import {
    SimpleSelectModeDef,
    DirectSelect,
  } from 'services/map/mapboxDraw/modes';
  export const CircleMode: { [k: string]: any };
  export const DragCircleMode: { [k: string]: any };
  export const DirectMode: DirectSelect;
  export const SimpleSelectMode: SimpleSelectModeDef;
}

declare module '@mapbox/mapbox-gl-draw/src/lib/common_selectors' {
  /**
   * Checks meta value
   *
   * @param e -mapbox-gl event
   */
  export const isOfMetaType: {
    (e: any): boolean;
  };
  /**
   * Checks if shift + mouse down event is triggered
   *
   * @param e -mapbox-gl event
   */
  export const isShiftMousedown: {
    (e: any): boolean;
  };
  /**
   * Checks if the event features is active
   *
   * @param e -mapbox-gl event
   */
  export const isActiveFeature: {
    (e: any): boolean;
  };
  /**
   * Checks if the event features is inactive
   *
   * @param e -mapbox-gl event
   */
  export const isInactiveFeature: {
    (e: any): boolean;
  };
  /**
   * Checks if the event is thrown outside of feature
   *
   * @param e -mapbox-gl event
   */
  export const noTarget: {
    (e: any): boolean;
  };
  /**
   * Checks if the event has a feature
   *
   * @param e -mapbox-gl event
   */
  export const isFeature: {
    (e: any): boolean;
  };
  /**
   * Checks if the event feature is a vertex
   *
   * @param e -mapbox-gl event
   */
  export const isVertex: {
    (e: any): boolean;
  };
  /**
   * Checks if shift key is pressed
   *
   * @param e -mapbox-gl event
   */
  export const isShiftDown: {
    (e: any): boolean;
  };
  /**
   * Checks if scape key is pressed
   *
   * @param e -mapbox-gl event
   */
  export const isEscapeKey: {
    (e: any): boolean;
  };
  /**
   * Checks if enter key is pressed
   *
   * @param e -mapbox-gl event
   */
  export const isEnterKey: {
    (e: any): boolean;
  };
}

declare module 'mapbox-gl-controls/lib/ruler';
declare module 'mapbox-gl-controls/lib/zoom';
declare module 'mapbox-gl-controls/lib/tooltip';
declare module '@mapbox/mapbox-gl-draw/src/constants';
