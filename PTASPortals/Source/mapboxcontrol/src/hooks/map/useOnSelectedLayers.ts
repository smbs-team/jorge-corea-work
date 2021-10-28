/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext } from 'react';
import { DEFAULT_MAX_ZOOM, DEFAULT_MIN_ZOOM } from 'appConstants';
import { LayerRowData } from 'components/common';
import { HomeContext } from 'contexts';
import { MapRenderer } from 'services/map';
import userService from 'services/user/userService';
import { scaleToMapZoom } from 'utils';

type UseOnSelectedLayers = (
  selectedRows: LayerRowData[],
  isSystemUserMap?: boolean | undefined
) => void;

export const useOnSelectedLayers = (): UseOnSelectedLayers => {
  const {
    setSelectedUserMap,
    isEditingUserMap,
    editingUserMap,
    selectedUserMap,
    setEditingUserMap,
  } = useContext(HomeContext);
  return (selectedRows: LayerRowData[], isSystemUserMap?: boolean): void => {
    if (!selectedRows) return;
    let currentRenderers = [];
    let userMapId;
    if (isEditingUserMap) {
      if (!editingUserMap?.mapRenderers) return;
      currentRenderers = editingUserMap.mapRenderers;
      userMapId = editingUserMap.userMapId;
    } else {
      if (!selectedUserMap?.mapRenderers) return;
      currentRenderers = selectedUserMap.mapRenderers;
      userMapId = selectedUserMap.userMapId;
    }

    const newRenderers: MapRenderer[] = [];
    //Iterate through current renderers in user map
    for (let i = 0; i < currentRenderers.length; i++) {
      const currentRenderer = currentRenderers[i];

      const newRenderer = selectedRows.find(
        (r) => r.layerSourceId === currentRenderer.layerSourceId
      );
      if (newRenderer) {
        //If layer source is found in new selection, keep its renderers
        const layerRenderers = currentRenderers.filter(
          (r) => r.layerSourceId === newRenderer.layerSourceId
        );
        if (layerRenderers) {
          layerRenderers.forEach((r) => newRenderers.push(r));
        }
      }
    }

    //Iterate through new selected rows
    for (let i = 0; i < selectedRows.length; i++) {
      if (
        !newRenderers
          .map((r) => r.layerSourceId)
          .includes(selectedRows[i].layerSourceId)
      ) {
        const convertedMin = scaleToMapZoom(DEFAULT_MIN_ZOOM);
        const convertedMax = scaleToMapZoom(DEFAULT_MAX_ZOOM);
        const minZoom =
          convertedMin >= 0 && convertedMin <= 22 ? convertedMin : 0;
        const maxZoom = convertedMax <= 22 && convertedMax ? convertedMax : 22;
        //Newly added layer source
        const newRenderer = new MapRenderer({
          createdBy: userService.userInfo.id,
          lastModifiedBy: userService.userInfo.id,
          layerSourceId: selectedRows[i].layerSourceId,
          mapRendererId: 0,
          mapRendererName: selectedRows[i].name,
          rendererRules: {
            nativeMapboxLayers: selectedRows[i].nativeMapboxLayers,
            layer: {
              ...selectedRows[i].defaultMapboxLayer,
              minzoom: minZoom,
              maxzoom: maxZoom,
              metadata: {
                ...(selectedRows[i].defaultMapboxLayer.metadata ?? {
                  isSystemRenderer: !!isSystemUserMap,
                  checked: true,
                }),
                scaleMinZoom: DEFAULT_MIN_ZOOM,
                scaleMaxZoom: DEFAULT_MAX_ZOOM,
              },
            },
          },
          userMapId: userMapId,
        });
        newRenderers.push(newRenderer);
      }
    }

    //Set new renderers in selectedUserMap or editingUserMap
    if (isEditingUserMap) {
      setEditingUserMap((prev) => {
        if (!prev) return;
        return {
          ...prev,
          mapRenderers: newRenderers,
        };
      });
    } else {
      setSelectedUserMap?.((prev) => {
        if (!prev) return;
        return {
          ...prev,
          mapRenderers: newRenderers,
        };
      });
    }
  };
};
