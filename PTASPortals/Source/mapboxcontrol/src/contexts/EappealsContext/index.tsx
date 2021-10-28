/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useGlMap } from 'hooks/map/useGlMap';
import React, { createContext, PropsWithChildren, useEffect } from 'react';
import { mapBoxDrawService } from 'services/map/mapboxDraw';
import { useLayers } from './useLayers';

type ContextProps = {};
let run = false;
export const EappealsContext = createContext<ContextProps>(null as never);
export const EappealsProvider = (props: PropsWithChildren<{}>): JSX.Element => {
  const map = useGlMap();
  useLayers();

  useEffect(() => {
    if (run) return;
    if (!map) return;
    mapBoxDrawService.init(map);
    run = true;
  }, [map]);

  return (
    <EappealsContext.Provider value={{}}>
      {props.children}
    </EappealsContext.Provider>
  );
};
