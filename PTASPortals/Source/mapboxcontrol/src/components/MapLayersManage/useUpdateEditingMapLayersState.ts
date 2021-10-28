/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { MapRenderer } from 'services/map';
import { HomeContext } from 'contexts';

type UseUpdateEditingMapLayersState = (
  cb: (renderer: MapRenderer) => MapRenderer
) => void;

export const useUpdateEditingMapLayersState = (): UseUpdateEditingMapLayersState => {
  const { setEditingUserMap } = useContext(HomeContext);
  return (cb: (renderer: MapRenderer) => MapRenderer): void => {
    setEditingUserMap((prev) => {
      if (!prev) return;
      return {
        ...prev,
        mapRenderers: prev.mapRenderers?.map((renderer) => cb(renderer)) ?? [],
      };
    });
  };
};

export const useUpdateCurrentLayerState = (): UseUpdateEditingMapLayersState => {
  const { currentLayer } = useContext(HomeContext);
  const updateEditingMap = useUpdateEditingMapLayersState();
  return (cb: (renderer: MapRenderer) => MapRenderer): void => {
    updateEditingMap((renderer) => {
      if (
        renderer.rendererRules.layer.id === currentLayer?.rendererRules.layer.id
      )
        return cb(renderer);
      return renderer;
    });
  };
};
