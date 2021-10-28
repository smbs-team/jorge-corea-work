/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useCallback, useContext } from 'react';
import { wrap } from 'lodash';
import { SideTreeRow } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { UserMap } from 'services/map';
import { isInSystem } from 'utils/userMap';

type UseOnLayerClick = (row: SideTreeRow) => void;

export enum TreeMapId {
  USER_MAP = 0,
  SYSTEM_RENDERER = 1,
}

const userMapContainsLayer = (
  userMap: UserMap | undefined,
  layerId: string
): boolean =>
  !!userMap?.mapRenderers.some(
    (renderer) =>
      renderer.rendererRules.colorRule?.layer.id === layerId ||
      renderer.rendererRules.labels?.find((label) => label.layer.id === layerId)
  );

export const useOnLayerClick = (): UseOnLayerClick => {
  const {
    selectedUserMap,
    selectedSystemUserMap,
    setSelectedUserMap,
    setSelectedSystemUserMap,
  } = useContext(HomeContext);

  return useCallback(
    wrap(
      (row: SideTreeRow): void => {
        if (!selectedUserMap) return;
        const isSelected = row.isChecked;
        const isSystemRenderer =
          row.parentId === TreeMapId.SYSTEM_RENDERER ||
          userMapContainsLayer(selectedSystemUserMap, row.id.toString());
        const updatedUserMap = new UserMap(
          isSystemRenderer && selectedSystemUserMap
            ? selectedSystemUserMap
            : selectedUserMap
        );

        updatedUserMap.mapRenderers = updatedUserMap.mapRenderers.map(
          (layer) => {
            //Root Layer
            if (
              layer.rendererRules.layer.id === row.id &&
              layer.rendererRules.layer.layout
            ) {
              layer.rendererRules.layer.layout.visibility = isSelected
                ? 'visible'
                : 'none';
              if (layer.rendererRules.colorRule) {
                if (layer.rendererRules.colorRule.layer.layout) {
                  layer.rendererRules.colorRule.layer.layout.visibility = isSelected
                    ? 'visible'
                    : 'none';
                }
              }

              layer?.rendererRules?.labels?.forEach((label) => {
                if (label.layer.layout) {
                  label.layer.layout.visibility = isSelected
                    ? 'visible'
                    : 'none';
                }
              });
              return layer;
            }
            //Color rule layer
            else if (
              layer.rendererRules.colorRule &&
              layer.rendererRules.colorRule.layer.layout &&
              layer.rendererRules.colorRule.layer.id === row.id
            ) {
              layer.rendererRules.colorRule.layer.layout.visibility = isSelected
                ? 'visible'
                : 'none';
              return layer;
            } else if (layer?.rendererRules?.labels?.length) {
              if (row.isGroup) {
                for (const label of layer.rendererRules.labels) {
                  if (
                    label.labelConfig.group === row.name &&
                    label.layer.layout?.visibility
                  ) {
                    label.layer.layout.visibility = isSelected
                      ? 'visible'
                      : 'none';
                  }
                }
              } else {
                const label = layer.rendererRules.labels.find(
                  (l) => l.layer.id === row.id
                );
                if (label) {
                  if (!label.layer.layout) return layer;
                  label.layer.layout.visibility = isSelected
                    ? 'visible'
                    : 'none';
                }
                return layer;
              }
            }
            return layer;
          }
        );

        if (isInSystem(updatedUserMap)) {
          setSelectedSystemUserMap(updatedUserMap);
        } else {
          setSelectedUserMap(updatedUserMap);
        }
      },
      (fn, row) =>
        fn({
          ...row,
          id: row.id.toString().replace(/^\d-/, ''),
        })
    ),
    [
      selectedSystemUserMap,
      selectedUserMap,
      setSelectedSystemUserMap,
      setSelectedUserMap,
    ]
  );
};
