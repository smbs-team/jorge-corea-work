// useDataSetColumns.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useCallback, useContext, useEffect, useState } from 'react';
import { DropDownItem, SnackContext } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useIsParcelRenderer } from 'hooks/map/useIsParcelRenderer';
import { RuleContext } from '../Context';
import { DEFAULT_DATASET_ID } from 'appConstants';
import { RendererDataSetColumn } from 'services/map';

interface UseDataSetColumns {
  dataSetColumnsDropDownItems: DropDownItem[];
}

const useClassBreakExpressionsFilter =
  process.env?.REACT_APP_USE_CLASSBREAK_EXPRESSIONS_FILTER === 'true' ?? true;

export const useDataSetColumns = (): UseDataSetColumns => {
  const { setSnackState } = useContext(SnackContext);
  const { colorRuleAction, currentLayer } = useContext(HomeContext);
  const {
    dataSetColumns,
    setDatasetField,
    ruleType,
    embeddedDataFields,
    setRendError,
  } = useContext(RuleContext);
  const [
    dataSetColumnsDropDownItems,
    setDataSetColumnsDropDownItems,
  ] = useState<DropDownItem[]>([]);
  const isParcelRenderer = useIsParcelRenderer();

  const onColumnsFiltered = useCallback(
    (filteredDsCols: RendererDataSetColumn[]) => {
      if (!filteredDsCols.length) {
        setRendError(
          'The dataset has no valid fields for this type of renderer.'
        );
        setDatasetField(undefined);
        return setDataSetColumnsDropDownItems([]);
      }

      setRendError(undefined);
      setDatasetField((prev) => {
        if (
          !filteredDsCols.find((item) => item.columnName === prev?.columnName)
        ) {
          return filteredDsCols[0];
        }
        return prev;
      });
      setDataSetColumnsDropDownItems(
        filteredDsCols.map((item) => ({
          label: item.columnName,
          value: item.columnName,
        }))
      );
    },
    [setDatasetField, setRendError]
  );

  /**
   * Set field to render drop down items for non parcel renderer
   */
  useEffect(() => {
    if (isParcelRenderer) return;
    setDataSetColumnsDropDownItems(
      embeddedDataFields.map(
        (item): DropDownItem => {
          return {
            label: item.FieldName,
            value: item.FieldName,
          };
        }
      )
    );
  }, [embeddedDataFields, isParcelRenderer, setSnackState]);

  /**
   * Filter ds cols
   */
  useEffect(() => {
    if (!isParcelRenderer) return;
    if (!dataSetColumns.length) return;

    onColumnsFiltered(
      dataSetColumns.filter((item) => {
        if (!ruleType) return false;
        if (ruleType === 'Class') {
          if (item.columnType !== 'number') return false;
          if (
            useClassBreakExpressionsFilter &&
            item.datasetId === DEFAULT_DATASET_ID
          ) {
            return item.expressions.find(
              (exp) => exp.expressionRole === 'RangedValuesOverrideExpression'
            );
          }
          return true;
        }
        if (ruleType === 'Unique') return item.canBeUsedAsLookup;
        return true;
      })
    );
  }, [
    dataSetColumns,
    isParcelRenderer,
    onColumnsFiltered,
    ruleType,
    setDatasetField,
    setSnackState,
  ]);

  /**
   * On dataset columns changed, set dataset field
   */
  useEffect(() => {
    if (colorRuleAction === undefined) return;
    if (!dataSetColumns.length) return;
    if (colorRuleAction === 'update') {
      if (
        currentLayer &&
        currentLayer.mapRendererId &&
        currentLayer.rendererRules.colorRule?.rule
      ) {
        const field = dataSetColumns.find(
          (item) =>
            item.columnName ===
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            currentLayer.rendererRules.colorRule!.rule.columnName
        );
        if (!field) return;
        setDatasetField(field);
        return;
      }
    } else {
      setDatasetField(dataSetColumns[0]);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataSetColumns]);

  return {
    dataSetColumnsDropDownItems,
  };
};
