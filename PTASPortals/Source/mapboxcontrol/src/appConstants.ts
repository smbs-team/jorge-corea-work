/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Default layers
 */
export enum AppLayers {
  PARCEL_LAYER = 'parcelLayer',
}

/**
 * Default dataset id
 */
export const DEFAULT_DATASET_ID = '5f6333e3-6acb-48f0-9268-b865e521aa08';

/**
 * Name of system renderers folder
 */

export enum SYSTEM_RENDERER_FOLDER {
  PATH = '/SystemRenderer',
  NAME = 'System',
}

export enum ROOT_FOLDER {
  SYSTEM = '/System',
  USER = '/User',
  SHARED = '/Shared',
}

/**
 * Mapbox draw events see https://github.com/mapbox/mapbox-gl-draw/blob/master/docs/API.md
 */
export enum MapboxDrawEvents {
  /**
   * Fired when a feature is created.
   * The following interactions will trigger this event:
   * Finish drawing a feature.Simply clicking will create a Point.A LineString or Polygon is only created when the user
   * has finished drawing it — i.e.double-clicked the last vertex or hit Enter — and the drawn feature is valid.
   */
  DRAW_CREATE = 'draw.create',
  /**
   * Fired when one or more features are deleted. The following interactions will trigger this event:
   * Click the trash button when one or more features are selected in simple_select mode.
   * Hit the Backspace or Delete keys when one or features are selected in simple_select mode.
   * Invoke draw.trash() when you have a feature selected in simple_select mode.
   */
  DRAW_DELETE = 'draw.delete',
  /**
   * Fired when one or more features are updated. The following interactions will trigger this event, which can be sub categorized by action:
   * action: 'move'
   * Finish moving one or more selected features in simple_select mode. The event will only fire when the movement is
   * finished (i.e.when the user releases the mouse button or hits Enter).
   * action: 'change_coordinates'
   * Finish moving one or more vertices of a selected feature in direct_select mode. The event will only fire when the movement
   * is finished (i.e.when the user releases the mouse button or hits Enter,
   * or her mouse leaves the map container).
   * Delete one or more vertices of a selected feature in direct_select mode, which can be done by hitting the Backspace or Delete keys,
   * clicking the Trash button, or invoking draw.trash().
   * Add a vertex to the selected feature by clicking a midpoint on that feature in direct_select mode.
   * This event will not fire when a feature is created or deleted.To track those interactions,
   * listen for draw.create and draw.delete events.
   */
  DRAW_UPDATE = 'draw.update',
  /**
   * Fired when features are combined. The following interactions will trigger this event:
   * Click the Combine button when more than one features are selected in simple_select mode.
   * Invoke draw.combineFeatures() when more than one features are selected in simple_select mode.
   */
  DRAW_COMBINE = 'draw.combine',
  /**
   * Fired when features are uncombined. The following interactions will trigger this event:
   * Click the Uncombine button when one or more multifeatures are selected in simple_select mode. Non-multifeatures may also be selected.
   * Invoke draw.uncombineFeatures() when one or more multifeatures are selected in simple_select mode.Non - multifeatures may also be selected.
   */
  DRAW_UNCOMBINE = 'draw.uncombine',
  /**
   * Fired when the selection is changed (i.e. one or more features are selected or deselected). The following interactions will trigger this event:
   *
   * Click on a feature to select it.
   * When a feature is already selected, shift-click on another feature to add it to the selection.
   * Click on a vertex to select it.
   * When a vertex is already selected, shift-click on another vertex to add it to the selection.
   * Create a box-selection that includes at least one feature.
   * Click outside the selected feature(s) to deselect.
   * Click away from the selected vertex(s) to deselect.
   * Finish drawing a feature (features are selected just after they are created).
   * When a feature is already selected, invoke draw.changeMode() such that the feature becomes deselected.
   */
  DRAW_SELECTION_CHANGE = 'draw.selectionchange',
  /**
   * Fired when the mode is changed. The following interactions will trigger this event:
   *
   * Click the point, line, or polygon buttons to begin drawing (enter a draw_* mode).
   * Finish drawing a feature (enter simple_select mode).
   * While in simple_select mode, click on an already selected feature (enter direct_select mode).
   * While in direct_select mode, click outside all features (enter simple_select mode).
   * This event is fired just after the current mode stops and just before the next mode starts.
   * A render will not happen until after all event handlers have been triggered, so you can force a
   * mode redirect by calling draw.changeMode() inside a draw.modechange handler.
   */
  DRAW_MODE_CHANGE = 'draw.modechange',
  /**
   * Fired just after Draw calls setData() on the Mapbox GL JS map.This does not imply that the set data call
   * has finished updating the map, just that the map is being updated.
   */
  DRAW_RENDER = 'draw.render',
  /**
   * Fired as the state of Draw changes to enable and disable different actions.Following this event will enable you know if draw.trash(),
   * draw.combineFeatures() and draw.uncombineFeatures() will have an effect.
   */
  DRAW_ACTION_ENABLE = 'draw.actionable',
}

/**
 * Fonts
 **/
export enum FONTS {
  OPEN_SANS_REGULAR = 'Open Sans Regular',
  OPEN_SANS_BOLD = 'Open Sans Bold',
  ARIAL_UNICODE_MS_BOLD = 'Arial Unicode MS Bold',
}

/**
 * Default min and max zoom scale
 */
export const DEFAULT_MIN_ZOOM = 4998393;
export const DEFAULT_MAX_ZOOM = 47;

/**
 * A default initial map coordinates
 */
export const MAP_INITIAL_COORDINATES: [number, number] = [
  -122.31933077513008,
  47.479971283143215,
];
export const RGBA_WHITE = 'rgba(255,255,255,1)';
export const RGBA_BLACK = 'rgba(0,0,0,1)';
export const RGBA_TRANSPARENT = 'rgba(0,0,0,0)';

export enum QUERY_PARAM {
  SELECTION_FILTER = 'filter',
  DATASET_ID = 'datasetId',
  PARCELS_QUERY = 'parcelsQuery',
}

export const MIN_FEATURES_DATA_ZOOM = 13.5;
export const FEATURES_DATA_SPLIT_ZOOM = 15;

export const MAPBOX_SEARCH_API_PARAMS = {
  country: 'us',
  region: 'US-WA',
  bbox:
    '-122.69779144791909,47.07351949649612,-120.9218706141953,47.877288481470885',
  // eslint-disable-next-line @typescript-eslint/camelcase
  access_token: process.env.REACT_APP_MAPBOX_GL_ACCESS_TOKEN,
};

export enum MapCustomEvent {
  LOAD_DS_POINTS = 'load-ds-points',
}
