/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContextProps } from 'contexts';
import { useEffect, useState } from 'react';
import { datasetService } from 'services/api';
import { layerService, MapRenderer } from 'services/map';

export const useLayerServiceLayers = ({
  selectedSystemUserMap,
  selectedUserMap,
}: Pick<
  HomeContextProps,
  'selectedUserMap' | 'selectedSystemUserMap'
>): MapRenderer[] => {
  const [renderers, setRenderers] = useState<MapRenderer[]>([]);
  const [hasParcelColorRenderer, setHasParcelColorRenderer] = useState(false);
  useEffect(() => {
    const filtered = (
      selectedSystemUserMap?.mapRenderers.flatMap((renderer) =>
        renderer.rendererRules.layer.layout?.visibility === 'visible'
          ? {
              ...renderer,
              rendererRules: {
                ...renderer.rendererRules,
                colorRule:
                  renderer.rendererRules.colorRule?.layer.layout?.visibility ===
                  'visible'
                    ? renderer.rendererRules.colorRule
                    : undefined,
              },
            }
          : []
      ) ?? []
    ).concat(
      selectedUserMap?.mapRenderers.flatMap((renderer) => {
        return renderer.rendererRules.layer.layout?.visibility === 'visible' &&
          (selectedSystemUserMap?.mapRenderers ?? []).every(
            (sysRenderer) =>
              sysRenderer.rendererRules.layer.id !==
                renderer.rendererRules.layer.id ||
              sysRenderer.rendererRules.layer.layout?.visibility === 'none'
          )
          ? {
              ...renderer,
              rendererRules: {
                ...renderer.rendererRules,
                colorRule:
                  renderer.rendererRules.colorRule?.layer.layout?.visibility ===
                  'visible'
                    ? renderer.rendererRules.colorRule
                    : undefined,
              },
            }
          : [];
      }) ?? []
    );

    setRenderers(filtered);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedSystemUserMap?.mapRenderers, selectedUserMap?.mapRenderers]);

  useEffect(() => {
    layerService.setMapLayers(renderers);
    setHasParcelColorRenderer(
      !!layerService.getParcelRenderer()?.rendererRules.colorRule
    );
  }, [renderers]);

  useEffect(() => {
    if (hasParcelColorRenderer) {
      datasetService.getColorRendererData();
    }
  }, [hasParcelColorRenderer]);

  return renderers;
};
