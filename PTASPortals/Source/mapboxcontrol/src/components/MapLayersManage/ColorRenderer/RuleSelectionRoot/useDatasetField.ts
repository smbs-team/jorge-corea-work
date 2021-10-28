// useDataSetField.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffect } from 'react';
import {
  ColorRule,
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
} from 'services/map/model';
import { RuleContextProps } from '../Context';

export const useDataSetField = (ruleContext: RuleContextProps): void => {
  const { datasetField, setRule, embeddedDataField } = ruleContext;

  /**
   * Set Column name and column type from dataset field
   */
  useEffect(() => {
    if (!datasetField) return;
    setRule((prevRule) => {
      if (!prevRule) return;
      let updatedRule: ColorRule | undefined;
      if (prevRule instanceof RendererSimpleRule) {
        updatedRule = new RendererSimpleRule({ ...prevRule });
      }
      if (prevRule instanceof RendererClassBreakRule) {
        updatedRule = new RendererClassBreakRule({ ...prevRule });
      }
      if (prevRule instanceof RendererUniqueValuesRule) {
        updatedRule = new RendererUniqueValuesRule({ ...prevRule });
      }
      if (updatedRule) {
        updatedRule.columnName = datasetField.columnName;
        updatedRule.columnType = datasetField.columnType;
        updatedRule.datasetId = datasetField.datasetId;
      }
      return updatedRule;
    });
  }, [datasetField, setRule]);

  /**
   * Set Column name and column type from embedded data field
   */
  useEffect(() => {
    if (!embeddedDataField) return;
    if (!setRule) return;
    setRule((prevRule) => {
      if (!prevRule) return;
      if (prevRule instanceof RendererSimpleRule) {
        const updatedRule = new RendererSimpleRule({ ...prevRule });
        updatedRule.columnName = embeddedDataField.FieldName;
        updatedRule.columnType = embeddedDataField.FieldType;
        return updatedRule;
      }
      return;
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [embeddedDataField]);
};
