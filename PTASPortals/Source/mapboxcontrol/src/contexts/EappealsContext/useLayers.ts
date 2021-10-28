/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useGlMap } from 'hooks/map/useGlMap';
import { useEffect } from 'react';
import { layerService } from 'services/map';

export const useLayers = (): void => {
  const map = useGlMap();

  useEffect(() => {
    if (!map) return;

    for (const layer of layerService.layersConfigurationList) {
      layer.defaultMapboxLayer.minzoom = 14;
      map.addLayer(layer.defaultMapboxLayer);
    }
  }, [map]);
};
