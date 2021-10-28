/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers } from 'appConstants';
import { FILTER_TYPES } from 'components/MapLayersManage/TextRenderer/typing';
import { HomeContext } from 'contexts';
import { useContext, useEffect } from 'react';
import { textRenderersService } from 'services/map/renderer/textRenderer/textRenderersService';
import { isInSystem } from 'utils/userMap';
import { useSelectedDots } from './useSelectedDots';

const useSelectedParcelLabel = (): void => {
  const {
    selectedUserMap,
    selectedSystemUserMap,
    setSelectedUserMap,
    setSelectedSystemUserMap,
  } = useContext(HomeContext);

  const selectedDots = useSelectedDots();

  useEffect(() => {
    const userMap = selectedSystemUserMap?.mapRenderers.some(
      (renderer) => renderer.rendererRules.layer.id === AppLayers.PARCEL_LAYER
    )
      ? selectedSystemUserMap
      : selectedUserMap;
    if (!userMap) return; //TODO: Check if we need to do any stuff in this case

    const setFn = isInSystem(userMap)
      ? setSelectedSystemUserMap
      : setSelectedUserMap;
    if (!setFn) return;

    const rendererParcel = userMap.mapRenderers.find(
      (ml) => ml.rendererRules.layer.id === AppLayers.PARCEL_LAYER
    );

    if (
      rendererParcel &&
      rendererParcel.rendererRules.labels?.some(
        (l) => l.labelConfig?.typeFilter === FILTER_TYPES.onlySelected.value
      )
    ) {
      textRenderersService.renderLabels(rendererParcel.rendererRules.labels);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedDots]);
};

export default useSelectedParcelLabel;
