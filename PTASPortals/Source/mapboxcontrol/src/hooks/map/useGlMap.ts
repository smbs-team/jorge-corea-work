/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useObservable } from 'react-use';
import { mapService } from 'services/map';
import { $onMapReady } from 'services/map/mapServiceEvents';

export const useGlMap = (): mapboxgl.Map | undefined =>
  useObservable($onMapReady, mapService.map);
