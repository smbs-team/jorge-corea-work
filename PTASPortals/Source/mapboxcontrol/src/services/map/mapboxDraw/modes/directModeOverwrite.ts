// directModeOverwrite.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/* eslint-disable @typescript-eslint/no-explicit-any */

import { Feature } from '@turf/turf';
import { DirectMode } from 'mapbox-gl-draw-circle';
import { Subject } from 'rxjs';
import { ModeFnEvent, SelectEvent } from './types';

export type DirectModeSubjectProps = {
  e: ModeFnEvent;
  state: {
    feature: Feature;
    [k: string]: any;
  };
};

export const directModeEvt = {
  onMouseMove: new Subject<{
    state: object;
    e: SelectEvent;
  }>(),
  clickNoTarget: new Subject(),
  dragFeature: new Subject<DirectModeSubjectProps>(),
  dragVertex: new Subject<DirectModeSubjectProps>(),
};

/**
 * direct_select mode  overwrite
 * @remarks
 *
 * mapbox-gl-draw-circle DirectMode overwrite, which in turn overwrites mapboxgl-gl-draw direct_select mode
 */
const DirectModeOverwrite = {
  ...DirectMode,
  /**
   * Feature vertex drag
   *
   * @remarks
   * Drag feature vertex that allows us to resize the shape
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   * @param delta - Coordinates
   */
  dragVertex: function (state: any, e: any, delta: any): void {
    DirectMode.dragVertex.call(this, state, e, delta);
    directModeEvt.dragVertex.next({ state, e });
  },
  /**
   * Drag feature event
   *
   * @remarks
   * direct_select mode drag feature
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   * @param delta - Coordinates
   */
  dragFeature: function (state: any, e: any, delta: any): void {
    DirectMode.dragFeature.call(this, state, e, delta);
    directModeEvt.dragFeature.next({ state, e });
  },

  /**
   * Outside feature click in direct_select mode
   */
  clickNoTarget: function (state: any, e: any): void {
    DirectMode.clickNoTarget.call(this, state, e);
    directModeEvt.clickNoTarget.next();
  },
  /**
   * Triggered when the mouse is moved on direct select mode
   *
   * @param state - A mutable state object created by onSetup
   * @param e - The captured event that is triggering this life cycle event
   */
  onMouseMove: function (state: any, e: any): void {
    DirectMode.onMouseMove.call(this, state, e);
    directModeEvt.onMouseMove.next(e);
  },
};

export default DirectModeOverwrite;
