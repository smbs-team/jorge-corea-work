// useMapZoomLevel.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useEffect, useRef } from 'react';
import { mapService } from 'services/map';
import { throttle } from 'lodash';
import { useMount } from 'react-use';

/**
 * Hook throttle time in milliseconds
 */
const hookThrottleTime = 50;

/**
 * Map zoom
 *
 * @remarks
 * The return value is truncated at two decimals
 * @returns - If map instance exists, returns the map zoom, otherwise returns de default zoom level
 */
const getZoom = (): number =>
  mapService.map?.getZoom() ?? mapService.defaultZoomLevel;

/**
 * A hook bound with map zoom level
 */
export const useMapZoom = (): number => {
  const [zoom, setZoom] = useState<number>(0);
  /**
   * A function created in order to remove in the unmounted event
   */
  const fnRef = useRef(
    throttle((): void => {
      setZoom(getZoom());
    }, hookThrottleTime)
  );

  useMount(() => {
    setZoom(getZoom());
  });

  useEffect(() => {
    const fn = fnRef.current;
    mapService?.map?.on('zoom', fn);
    return (): void => {
      mapService?.map?.off('zoom', fn);
    };
  });

  return zoom;
};
