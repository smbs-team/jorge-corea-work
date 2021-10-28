/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import MapElement from 'components/MapElement';
import { useGlMap } from 'hooks/map/useGlMap';
import mapboxgl from 'mapbox-gl';
import React, {
  createContext,
  PropsWithChildren,
  useCallback,
  useEffect,
  useRef,
} from 'react';
import { mapService } from 'services/map';

type Props = {
  map: mapboxgl.Map | undefined;
};

type ProviderProps = {
  zoom?: number;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const MapContext = createContext<Props>(null as any);

export const MapProvider = (
  props: PropsWithChildren<ProviderProps>
): JSX.Element | null => {
  const mapboxElRef = useRef<HTMLDivElement>(null);
  const map = useGlMap();

  const init = useCallback(async (): Promise<void> => {
    if (!mapboxElRef.current) return;
    if (mapService.map) return;
    try {
      await mapService.init({
        mapContainerId: mapboxElRef.current.getAttribute('id') || '',
        zoom: props.zoom,
      });
    } catch (e) {
      alert('Error initializing map service');
      console.error(e);
    }
  }, [props.zoom]);

  useEffect(() => {
    init();
  }, [init]);

  return (
    <MapContext.Provider value={{ map }}>
      {props.children}
      <MapElement ref={mapboxElRef} />
    </MapContext.Provider>
  );
};
