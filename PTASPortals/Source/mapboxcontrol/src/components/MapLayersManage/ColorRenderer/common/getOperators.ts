// useFieldOperator.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { DropDownItem } from '@ptas/react-ui-library';
import { DataSetColumnType } from 'services/map';
import { numericOperators, stringOperators } from 'services/map/renderer/utils';

export const getOperators = (
  columnType: DataSetColumnType = 'unknown'
): DropDownItem[] =>
  Object.entries(
    ['number', 'date'].includes(columnType)
      ? numericOperators
      : columnType === 'string'
      ? stringOperators
      : {}
  ).map(([k, v]) => ({
    value: k,
    label: v.name,
  }));
