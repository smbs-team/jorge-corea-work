// useMapClick.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useEffect, useRef, useState } from 'react';
import { MapEventType } from 'mapbox-gl';
import { useGlMap } from './useGlMap';

type MapClick = {
  event: MapEventType['click'] | undefined;
  capture: () => void;
};

export const useMapClick = (): MapClick => {
  const map = useGlMap();
  const [event, setEvent] = useState<MapEventType['click']>();

  const handleClick = useRef((e: MapEventType['click']): void => {
    if (!map) return;
    setEvent(e);
    map.off('click', handleClick.current);
  });

  useEffect(() => {
    if (!map) return;
    const handleClickFn = handleClick.current;
    return (): void => {
      map.off('click', handleClickFn);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const capture = (): void => {
    if (!map) return;
    map.on('click', handleClick.current);
  };

  return {
    event,
    capture,
  };
};
