/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { SideTreeRow } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useMapEvent } from 'hooks/map';
import { useUpdateMapRenderersState } from 'hooks/map/useUpdateMapRenderersState';
import { useCallback, useContext, useEffect, useRef, useState } from 'react';
import { useDebounce, useLocalStorage } from 'react-use';
import {
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
} from 'services/map/model';
import { textRenderersService } from 'services/map/renderer/textRenderer/textRenderersService';
import { TreeMapId, useOnLayerClick } from './useLayerClick';
import { groupRows, labelRows } from './utils';

type UseLeftSideTree = {
  sideRows: SideTreeRow[];
  handleSideTreeSelection: (row: SideTreeRow) => void;
};

type Legend = {
  legendText: string | number | null;
  fillColor: string;
  outlineColor: string;
};

export const isRootLayerRow = (row: SideTreeRow): boolean =>
  row.parentId === TreeMapId.SYSTEM_RENDERER ||
  row.parentId === TreeMapId.USER_MAP;

export const useSideTree = (): UseLeftSideTree => {
  const homeContext = useContext(HomeContext);
  const {
    selectedUserMap,
    selectedSystemUserMap,
    appliedRenderers,
    userMapLabels,
  } = homeContext;
  const [sideRows, setSideRows] = useState<SideTreeRow[]>([]);
  const isLoading = useRef<boolean>(true);
  const [listEnabled, setListEnabled] = useLocalStorage(
    'layers-list-enabled',
    true
  );
  const [systemListEnabled, setSystemListEnabled] = useLocalStorage(
    'system-layers-list-enabled',
    true
  );
  const updateLayers = useUpdateMapRenderersState();
  const onLayerClick = useOnLayerClick();
  const redrawLabels = useMapEvent('redraw-labels');
  const moveEnd = useMapEvent('moveend');
  const labelFeatureDataLoaded = useMapEvent('label-feature-data-loaded');

  const handleSideTreeSelection = useCallback(
    (row: SideTreeRow): void => {
      const updateAllLayers = (
        isSelected: boolean,
        isSystemRenderer: boolean
      ): void => {
        const updateLayersFn = isSystemRenderer
          ? updateLayers.systemUserMap
          : updateLayers.userMap;
        updateLayersFn((rend) => {
          const visibility = isSelected ? 'visible' : 'none';
          if (rend.rendererRules.layer.layout) {
            rend.rendererRules.layer.layout.visibility = visibility;
          }
          if (rend.rendererRules.colorRule?.layer.layout) {
            rend.rendererRules.colorRule.layer.layout.visibility = visibility;
          }
          for (const label of rend.rendererRules.labels ?? []) {
            if (label.layer.layout?.visibility && label.layer.metadata) {
              label.layer.layout.visibility = visibility;
            }
          }
          return { ...rend };
        });
      };
      if (row.id === TreeMapId.USER_MAP) {
        setListEnabled(row.isChecked);
        updateAllLayers(row.isChecked, false);
      } else if (row.id === TreeMapId.SYSTEM_RENDERER) {
        setSystemListEnabled(row.isChecked);
        updateAllLayers(row.isChecked, true);
      } else {
        onLayerClick(row);
      }
    },
    [
      onLayerClick,
      setListEnabled,
      setSystemListEnabled,
      updateLayers.systemUserMap,
      updateLayers.userMap,
    ]
  );

  /**
   * Apply labels
   */
  useDebounce(
    () => {
      textRenderersService.renderLabels(userMapLabels);
    },
    200,
    [
      userMapLabels,
      appliedRenderers,
      redrawLabels,
      labelFeatureDataLoaded,
      moveEnd,
    ]
  );

  /**
   * Set side rows on user map change
   */
  useEffect(() => {
    if (!selectedUserMap) {
      setSideRows([]);
      isLoading.current = false;
      return;
    }

    const newSideRows: SideTreeRow[] = [];
    isLoading.current = true;
    const allInSystemMap = !!selectedUserMap.mapRenderers.every((rend1) =>
      selectedSystemUserMap?.mapRenderers.find(
        (rend2) => rend2.rendererRules.layer.id === rend1.rendererRules.layer.id
      )
    );

    //#region User Map
    newSideRows.push({
      id: TreeMapId.USER_MAP,
      parentId: null,
      name: selectedUserMap.userMapName.length
        ? selectedUserMap.userMapName
        : 'New map',
      isCheckable: true,
      isSelected: false,
      isChecked: !!listEnabled,
      disableCheckBox: allInSystemMap,
    });

    if (selectedUserMap.mapRenderers?.length) {
      selectedUserMap.mapRenderers.forEach((rend) => {
        //Root layer
        const rootLayerRow: SideTreeRow = {
          id: TreeMapId.USER_MAP + '-' + rend.rendererRules.layer.id,
          parentId: TreeMapId.USER_MAP,
          name: rend.mapRendererName,
          isCheckable: true,
          isSelected: false,
          isChecked: rend.rendererRules.layer.layout?.visibility === 'visible',
          disableCheckBox: !!selectedSystemUserMap?.mapRenderers.find(
            (renderer) =>
              renderer.rendererRules.layer.id === rend.rendererRules.layer.id &&
              renderer.rendererRules.layer.layout?.visibility === 'visible'
          ),
        };
        newSideRows.push(rootLayerRow);

        //#region color renderer
        if (rend.rendererRules.colorRule) {
          const colorRule = rend.rendererRules.colorRule;
          let _legends: Legend[] = [];
          //#region Legends
          if (colorRule.rule instanceof RendererUniqueValuesRule) {
            _legends = colorRule.rule.colors.map((color) => {
              return {
                legendText: color.columnValue,
                fillColor: color.fill,
                outlineColor: color.outline,
              };
            });
          } else if (colorRule.rule instanceof RendererClassBreakRule) {
            _legends = colorRule.rule.colorRanges.map((colorRange) => {
              return {
                legendText:
                  colorRule.rule.columnName +
                  ' ' +
                  colorRange.from +
                  '-' +
                  colorRange.to,
                fillColor: colorRange.fill,
                outlineColor: colorRange.outline,
              };
            });
          } else if (colorRule.rule instanceof RendererSimpleRule) {
            _legends = [
              {
                legendText: colorRule.rule.conditionDescription,
                fillColor: colorRule.rule.fillColorTrue,
                outlineColor: colorRule.rule.outlineColorTrue,
              },
              {
                legendText: 'Other values',
                fillColor: colorRule.rule.fillColorFalse,
                outlineColor: colorRule.rule.outlineColorFalse,
              },
            ];
          }

          //#endregion
          newSideRows.push({
            id: colorRule.layer.id,
            name: colorRule.rule.columnName,
            parentId: rootLayerRow.id,
            isCheckable: true,
            isSelected: true,
            isChecked:
              colorRule?.layer?.layout?.visibility === 'visible' &&
              rootLayerRow.isChecked,
            disableCheckBox:
              rootLayerRow.disableCheckBox || !rootLayerRow.isChecked,
          });

          //Add legends
          _legends.forEach((legend) => {
            newSideRows.push({
              id: colorRule.layer.id + '-' + legend.legendText,
              name: legend.legendText?.toString() ?? 'null',
              parentId: colorRule.layer.id,
              isCheckable: false,
              isSelected: false,
              isChecked: false,
              shapeType: 'pill',
              fillColor: legend.fillColor,
              outlineColor: legend.outlineColor,
            });
          });
        }
        //#endregion

        //#region label renderer
        if (rend?.rendererRules?.labels?.length) {
          const labels = rend.rendererRules.labels;
          newSideRows.push(...groupRows(labels, rootLayerRow));
          newSideRows.push(...labelRows(labels, rootLayerRow));
        }
        //#endregion
      });
    }
    //#endregion

    //#region SystemUserMap
    if (selectedSystemUserMap?.mapRenderers.length) {
      //Map
      newSideRows.push({
        id: TreeMapId.SYSTEM_RENDERER,
        isCheckable: true,
        isChecked: !!systemListEnabled,
        isSelected: false,
        name: selectedSystemUserMap.userMapName.length
          ? selectedSystemUserMap.userMapName
          : 'New renderer',
        parentId: null,
      });

      for (const renderer of selectedSystemUserMap.mapRenderers) {
        const { rendererRules } = renderer;
        const rootLayerRowId =
          TreeMapId.SYSTEM_RENDERER + '-' + rendererRules.layer.id;
        //Root layer
        const rootLayerRow = {
          id: rootLayerRowId,
          isCheckable: true,
          isChecked: rendererRules?.layer.layout?.visibility === 'visible',
          isSelected: false,
          name: renderer.mapRendererName,
          parentId: TreeMapId.SYSTEM_RENDERER,
        };
        newSideRows.push(rootLayerRow);

        //Color renderer
        if (rendererRules.colorRule) {
          const colorRule = rendererRules.colorRule;
          let _legends: Legend[] = [];
          //#region Legends
          if (colorRule.rule instanceof RendererUniqueValuesRule) {
            _legends = colorRule.rule.colors.map((color) => {
              return {
                legendText: color.columnValue,
                fillColor: color.fill,
                outlineColor: color.outline,
              };
            });
          } else if (colorRule.rule instanceof RendererClassBreakRule) {
            _legends = colorRule.rule.colorRanges.map((colorRange) => {
              return {
                legendText:
                  colorRule.rule.columnName +
                  ' ' +
                  colorRange.from +
                  '-' +
                  colorRange.to,
                fillColor: colorRange.fill,
                outlineColor: colorRange.outline,
              };
            });
          } else if (colorRule.rule instanceof RendererSimpleRule) {
            _legends = [
              {
                legendText: colorRule.rule.conditionDescription,
                fillColor: colorRule.rule.fillColorTrue,
                outlineColor: colorRule.rule.outlineColorTrue,
              },
              {
                legendText: 'Other values',
                fillColor: colorRule.rule.fillColorFalse,
                outlineColor: colorRule.rule.outlineColorFalse,
              },
            ];
          }
          //#endregion;
          newSideRows.push({
            id: colorRule.layer.id ?? '',
            name: colorRule.rule.columnName,
            parentId: rootLayerRowId,
            isCheckable: true,
            isSelected: true,
            isChecked: colorRule.layer.layout?.visibility === 'visible',
            disableCheckBox: !rootLayerRow.isChecked,
          });

          _legends.forEach((legend) => {
            newSideRows.push({
              id: (colorRule.layer.id ?? '') + '-' + legend.legendText,
              name: legend.legendText?.toString() ?? 'null',
              parentId: colorRule.layer.id ?? '',
              isCheckable: false,
              isSelected: false,
              isChecked: false,
              shapeType: 'pill',
              fillColor: legend.fillColor,
              outlineColor: legend.outlineColor,
            });
          });
        }

        //#region Labels
        if (rendererRules.labels?.length) {
          const _groupRows = groupRows(rendererRules.labels, rootLayerRow);
          newSideRows.push(..._groupRows);
          newSideRows.push(...labelRows(rendererRules.labels, rootLayerRow));
        }
        //#endregion
      }
    }
    //#endregion
    setSideRows(newSideRows);
    isLoading.current = false;
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    // eslint-disable-next-line react-hooks/exhaustive-deps
    JSON.stringify(selectedUserMap),
    // eslint-disable-next-line react-hooks/exhaustive-deps
    JSON.stringify(selectedSystemUserMap),
  ]);

  return {
    sideRows,
    handleSideTreeSelection,
  };
};
