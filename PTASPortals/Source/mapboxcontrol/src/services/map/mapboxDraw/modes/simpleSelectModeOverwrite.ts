// simpleSelectModeOverwrite.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/* eslint-disable @typescript-eslint/no-explicit-any */

import { SimpleSelectMode } from 'mapbox-gl-draw-circle';
import { Subject } from 'rxjs';
import { EventData } from 'mapbox-gl';
import { SimpleSelect, SelectEvent } from './types';

export const simpleSelectEvt = {
  clickAnyWhere: new Subject<EventData>(),
  mouseMove: new Subject<{
    state: object;
    e?: SelectEvent;
  }>(),
  dragMove: new Subject(),
  onStop: new Subject(),
};

/**
 * simple_select mode  overwrite
 *
 * @remarks
 * mapbox-gl-draw-circle SimpleSelectMode overwrite, which in turn overwrites mapboxgl-gl-draw simple_select mode.
 * As the extending mode of this file is an third party library without type definition we allowed implicitly any for this file
 */
const SimpleSelectModeOverwrite: SimpleSelect = {
  ...SimpleSelectMode,
  /**
   * Triggered when the mouse is moved on simple select mode
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  onMouseMove: function (state, e) {
    SimpleSelectMode.onMouseMove.call(this, state, e);
    simpleSelectEvt.mouseMove.next({
      state,
      e,
    });
  },
  dragMove: function (state: any, e: any) {
    SimpleSelectMode.dragMove.call(this, state, e);
    simpleSelectEvt.dragMove.next();
  },

  startOnActiveFeature: function (state: any, e: any): void {
    if (e.featureTarget.properties?.user_disableMove) {
      state.canDragMove = false;
      return;
    }
    SimpleSelectMode.startOnActiveFeature.call(this, state, e);
  },

  /**
   * Outside feature click
   * @remarks
   * Outside feature click in simple_select mode
   * @param state -A mutable state object created by onSetup
   * @param e -The captured event that is triggering this life cycle event
   */
  clickAnywhere: (state: any, e: EventData) => {
    SimpleSelectMode.clickAnywhere.bind(SimpleSelectMode, state, e);
    simpleSelectEvt.clickAnyWhere.next(e);
  },
  onStop: function (opts) {
    SimpleSelectMode.onStop.bind(this, opts);
    simpleSelectEvt.onStop.next();
  },
};

export default SimpleSelectModeOverwrite;
