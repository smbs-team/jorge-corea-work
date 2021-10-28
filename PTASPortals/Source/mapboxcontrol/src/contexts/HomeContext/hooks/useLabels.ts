/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers } from 'appConstants';
import { HomeContextProps } from 'contexts';
import { useEffect, useMemo } from 'react';
import { datasetService } from 'services/api';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';

export const useLabels = ({
  selectedUserMap,
  selectedSystemUserMap,
}: Pick<
  HomeContextProps,
  'selectedUserMap' | 'selectedSystemUserMap'
>): RendererLabel[] => {
  const labels = useMemo(
    () =>
      (
        selectedSystemUserMap?.mapRenderers.flatMap((renderer) =>
          renderer.rendererRules.layer.layout?.visibility === 'visible'
            ? renderer.rendererRules.labels?.filter(
                (label) => label.layer.layout?.visibility === 'visible'
              ) ?? []
            : []
        ) ?? []
      ).concat(
        selectedUserMap?.mapRenderers.flatMap((renderer) => {
          return (selectedSystemUserMap?.mapRenderers ?? []).every(
            (sysRenderer) =>
              sysRenderer.rendererRules.layer.id !==
                renderer.rendererRules.layer.id ||
              sysRenderer.rendererRules.layer.layout?.visibility === 'none'
          )
            ? renderer.rendererRules.labels?.filter(
                (label) => label.layer.layout?.visibility === 'visible'
              ) ?? []
            : [];
        }) ?? []
      ),
    [selectedUserMap, selectedSystemUserMap]
  );

  useEffect(() => {
    if (
      labels.some(
        (label) => label.labelConfig.refLayerId === AppLayers.PARCEL_LAYER
      )
    ) {
      setTimeout(() => {
        datasetService.getLabelsData();
      }, 500);
    }
  }, [labels]);

  return labels;
};
