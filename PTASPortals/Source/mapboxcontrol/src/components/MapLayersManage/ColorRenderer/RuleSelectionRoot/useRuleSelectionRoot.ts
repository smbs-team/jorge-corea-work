// useRuleSelection.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DropDownItem, ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import {
  RendererSimpleRule,
  RendererUniqueValuesRule,
  RuleType,
} from 'services/map/model';
import {
  RendererClassBreakRule,
  Rule,
  BreakOptionKey,
} from 'services/map/model';
import { useMemo, useEffect, useContext } from 'react';
import { RuleContext } from '../Context';
import { useDebounce, useMount } from 'react-use';
import { HomeContext } from 'contexts';
import { useUpdateEditingMapLayersState } from 'components/MapLayersManage/useUpdateEditingMapLayersState';
import { rendererService } from 'services/map';
import { apiService } from 'services/api';
import userService from 'services/user/userService';
import { useIsParcelRenderer } from 'hooks/map/useIsParcelRenderer';
import { getErrorStr } from 'utils/getErrorStr';

type TypeOfRendererDropDownItem = DropDownItem & { value: RuleType };

interface UseRuleSelectionRoot {
  dataSetsDropDownItems: DropDownItem[];
}

export const typesOfRenderer: TypeOfRendererDropDownItem[] = [
  {
    label: 'Simple',
    value: 'Simple',
  },
  {
    label: 'Unique Values',
    value: 'Unique',
  },

  {
    label: 'Class Break',
    value: 'Class',
  },
];

export const toClassRule = (
  rule: Rule | undefined
): RendererClassBreakRule | undefined =>
  rule instanceof RendererClassBreakRule
    ? (rule as RendererClassBreakRule)
    : undefined;

export const useRuleSelectionRoot = (): UseRuleSelectionRoot => {
  const isParcelRenderer = useIsParcelRenderer();
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const {
    defaultDataSet,
    currentLayer,
    colorRuleAction,
    setLinearProgress,
  } = useContext(HomeContext);
  const {
    datasetId,
    setDatasetId,
    userRendererDatasets,
    setRuleType,
    rule,
    ruleType,
    setRule,
    setBreakOption,
    setNumberOfBreaks,
    setUserRendererDatasets,
    setDataSetColumns,
    currentLayerId,
    datasetField,
  } = useContext(RuleContext);
  const updateLayers = useUpdateEditingMapLayersState();

  /**
   * Datasets drop down
   */
  const dataSetsDropDownItems = useMemo<DropDownItem[]>(() => {
    const datasets =
      userRendererDatasets?.map((item) => ({
        label: item.datasetName,
        value: item.datasetId,
      })) ?? [];

    if (defaultDataSet) {
      const _defaultDataSetItem = {
        label: defaultDataSet.datasetName,
        value: defaultDataSet.datasetId,
      };
      datasets.unshift(_defaultDataSetItem);
    }

    return datasets;
  }, [defaultDataSet, userRendererDatasets]);

  /**
   * Get datasets
   */
  useMount(async () => {
    try {
      setLinearProgress(true);
      const r = await apiService.getUserRendererDataSets(
        userService.userInfo.id
      );
      setUserRendererDatasets(r);
    } catch (e) {
      console.error(e);
      showErrorMessage('Error getting user renderer datasets');
    } finally {
      setLinearProgress(false);
    }
  });

  /**
   * Set renderer dataset if for create event
   */
  useEffect(() => {
    if (colorRuleAction !== 'create') return;
    if (defaultDataSet) {
      setDatasetId(defaultDataSet.datasetId);
      return;
    }

    if (!userRendererDatasets?.length) return;
    setDatasetId(userRendererDatasets[0].datasetId);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [userRendererDatasets, defaultDataSet]);

  useEffect(() => {
    if (!datasetId) return;
    if (!isParcelRenderer) {
      setDataSetColumns([]);
      return;
    }
    const fn = async (): Promise<void> => {
      try {
        setLinearProgress(true);
        const columns = await apiService.getDataSetColumns(datasetId);
        setDataSetColumns(columns);
      } catch (e) {
        showErrorMessage(getErrorStr(e));
        console.error(e);
      } finally {
        setLinearProgress(false);
      }
    };
    fn();
  }, [
    currentLayerId,
    datasetId,
    isParcelRenderer,
    setDataSetColumns,
    setLinearProgress,
    showErrorMessage,
  ]);

  useEffect(() => {
    if (!currentLayer) return;
    if (!colorRuleAction) return;
    if (colorRuleAction === 'update') {
      if (!currentLayer.rendererRules.colorRule) return;
      setRuleType(currentLayer.rendererRules.colorRule?.rule.ruleType);
      setDatasetId(currentLayer.rendererRules.colorRule.rule.datasetId);
      setRule(currentLayer.rendererRules.colorRule?.rule);
      if (
        currentLayer.rendererRules.colorRule?.rule instanceof
        RendererClassBreakRule
      ) {
        const breakOption = currentLayer.rendererRules.colorRule.rule
          .breakOption as BreakOptionKey;
        setBreakOption(breakOption);
        if (breakOption !== BreakOptionKey.stDeviation) {
          setNumberOfBreaks(
            currentLayer.rendererRules.colorRule.rule.colorRanges.length
          );
        } else {
          setNumberOfBreaks(3);
        }
      }
    } else {
      setRuleType(typesOfRenderer[0].value);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [colorRuleAction, currentLayerId]);

  /**
   * Set renderer rule
   */
  useDebounce(
    () => {
      if (!rule) return;
      if (!rule.columnName) return;
      if (!currentLayer) return;

      updateLayers((layerRenderer) => {
        if (
          layerRenderer.rendererRules.layer.id ===
            currentLayer.rendererRules.layer.id &&
          layerRenderer.rendererRules.colorRule
        ) {
          const newRenderer = rendererService.genColorRule({
            ...layerRenderer,
            rendererRules: {
              ...layerRenderer.rendererRules,
              colorRule: {
                ...layerRenderer.rendererRules.colorRule,
                rule: rule,
              },
            },
          });
          return newRenderer;
        }
        return layerRenderer;
      });
      // eslint-disable-next-line react-hooks/exhaustive-deps
    },
    500,
    [rule]
  );

  /**
   * Set rule common fields
   */
  useEffect(() => {
    if (!ruleType) return;
    if (!datasetField) return;
    setRule((prev) => {
      if (!prev) return prev;
      if (prev instanceof RendererSimpleRule) {
        return new RendererSimpleRule({
          ...prev,
          datasetId: datasetField.datasetId,
          columnName: datasetField.columnName,
          columnType: datasetField.columnType,
        });
      }
      if (prev instanceof RendererClassBreakRule) {
        return new RendererClassBreakRule({
          ...prev,
          datasetId: datasetField.datasetId,
          columnType: datasetField.columnType,
          columnName: datasetField.columnName,
        });
      }
      if (prev instanceof RendererUniqueValuesRule) {
        return new RendererUniqueValuesRule({
          ...prev,
          datasetId: datasetField.datasetId,
          columnName: datasetField.columnName,
          columnType: datasetField.columnType,
        });
      }
      return prev;
    });
  }, [setRule, datasetField, ruleType]);

  return {
    dataSetsDropDownItems,
  };
};
