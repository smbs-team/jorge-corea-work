// mapServiceEvents.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Subject } from 'rxjs';
import { MapRenderer, UserMap, ParcelClickEvent, LayerSource } from './model';

export const $onError = new Subject<string>();
export const $onMapReady = new Subject<mapboxgl.Map>();
export const $onParcelClick = new Subject<ParcelClickEvent>();
export const $onSelectedUserMapChange = new Subject<UserMap | undefined>();
export const $onSelectedSystemUserMapChange = new Subject<
  UserMap | undefined
>();
export const $onSelectedLayersChange = new Subject<MapRenderer[]>();
export const $onLayerSourcesReady = new Subject<LayerSource[]>();
export const $onParcelStack = new Subject<
  {
    major: string;
    minor: string;
  }[]
>();
