/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl, { EventData } from 'mapbox-gl';
import { useEffect, useState } from 'react';
import { mapService } from 'services/map';

type UseMapEvent = EventData & { target: mapboxgl.Map };

export const useMapEvent = (event: string): UseMapEvent | undefined => {
  const { map } = mapService;
  const [eventData, setEventData] = useState<UseMapEvent>();
  useEffect(() => {
    const fn = (e: UseMapEvent): void => {
      setEventData(e);
    };
    map?.on(event, fn);
    return (): void => {
      map?.off(event, fn);
    };
  }, [event, map]);

  return eventData;
};
