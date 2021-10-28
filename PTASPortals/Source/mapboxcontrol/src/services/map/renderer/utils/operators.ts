/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  NumericOperatorId,
  OperatorRow,
  StringOperatorId,
} from '../../model/renderer';

export const numericOperators: Record<NumericOperatorId, OperatorRow> = {
  equals: {
    name: '=',
    mapboxOperator: '==',
  },
  lt: {
    name: '<',
    mapboxOperator: '<',
  },
  gt: {
    name: '>',
    mapboxOperator: '>',
  },
  gte: {
    name: '>=',
    mapboxOperator: '>=',
  },
  lte: {
    name: '<=',
    mapboxOperator: '<=',
  },
  different: {
    name: '<>',
    mapboxOperator: '!=',
  },
};

export const stringOperators: Record<StringOperatorId, OperatorRow> = {
  different: {
    mapboxOperator: numericOperators.different.mapboxOperator,
    name: 'Not Equal',
  },
  equals: {
    mapboxOperator: numericOperators.equals.mapboxOperator,
    name: 'Equal',
  },
  includes: {
    mapboxOperator: 'in',
    name: 'Included',
  },
  notIncludes: {
    mapboxOperator: '',
    name: 'Not included',
  },
};

export const isNumericOperator = (str: string): boolean =>
  Object.keys(numericOperators).includes(str);

export const isStringOperator = (str: string): boolean =>
  Object.keys(stringOperators).includes(str);
