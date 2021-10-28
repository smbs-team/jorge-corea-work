// drawPolygonOverwrite.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/* eslint-disable @typescript-eslint/no-explicit-any */

import MapboxDraw from '@mapbox/mapbox-gl-draw';
import { Subject } from 'rxjs';
import { DrawPolygonState, ModeFnEvent, DrawPolygon } from './types';
const { draw_polygon: originalMode } = MapboxDraw.modes;

export const drawPolygonEvt = {
  onStop: new Subject<{
    state: DrawPolygonState;
    map: mapboxgl.Map;
    mode: DrawPolygon;
  }>(),
  clickAnywhere: new Subject<{
    map: mapboxgl.Map;
    state: DrawPolygonState;
    e: ModeFnEvent;
  }>(),
  clickOnVertex: new Subject<{
    state: DrawPolygonState;
    e: ModeFnEvent;
  }>(),
  onMouseMove: new Subject<{
    state: DrawPolygonState;
    e: ModeFnEvent;
  }>(),
};

/**
 * draw_polygon mode  overwrite
 *
 * @remarks
 * As the extending mode of this file is an third party library without type definition implicitly any is allowed
 * As mapbox-gl-draw exported modes names are snake case camelcase rule is disabled
 */
const DrawPolygonOverwrite: DrawPolygon = {
  ...originalMode,
  clickOnVertex: function (state, e) {
    originalMode.clickOnVertex.call(this, state, e);
    drawPolygonEvt.clickOnVertex.next({
      e,
      state,
    });
  },
  /**
   * Triggered when clicks outside of feature
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  clickAnywhere: function (state, e) {
    originalMode.clickAnywhere.call(this, state, e);
    drawPolygonEvt.clickAnywhere.next({
      map: this.map,
      state,
      e,
    });
  },

  onMouseMove: function (state, e) {
    originalMode.onMouseMove.call(this, state, e);
    drawPolygonEvt.onMouseMove.next({
      e,
      state,
    });
  },

  /**
   * Triggered when the mode is being exited, to be used for cleaning up artifacts such as invalid features
   *
   * @param state  - A mutable state object created by onSetup
   */
  onStop: function (state) {
    originalMode.onStop.call(this, state);
    drawPolygonEvt.onStop.next({
      map: this.map,
      state,
      mode: this,
    });
  },
};

export default DrawPolygonOverwrite;
