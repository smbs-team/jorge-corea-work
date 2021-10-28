// gridUtility.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { InvalidRow } from '@ptas/react-ui-library';
import { CellClassParams } from 'ag-grid-community';
import { CSSProperties } from 'react';

export const getCellStyle = (params: CellClassParams): CSSProperties | null => {
  const errors = params.context.errors as InvalidRow[];
  if (errors.some((e) => e.index === params.node.rowIndex)) {
    return {
      backgroundColor:
        params.colDef.field === 'minEff' || params.colDef.field === 'maxEff'
          ? '#e5fff9'
          : undefined,
      borderBottomColor: 'red',
    };
  }
  return {
    backgroundColor:
      params.colDef.field === 'minEff' || params.colDef.field === 'maxEff'
        ? '#e5fff9'
        : undefined,
    borderBottomColor: '#d3d3d3',
  };
};

export const getCellStyleResults = (
  params: CellClassParams
): CSSProperties | null => {
  const errors = params.context.errors as InvalidRow[];
  if (errors.some((e) => e.index === params.node.rowIndex)) {
    return {
      backgroundColor:
        params.colDef.field === 'minEff' || params.colDef.field === 'maxEff'
          ? '#e5fff9'
          : '#e5e5e5',
      borderBottomColor: 'red',
    };
  }
  return {
    backgroundColor:
      params.colDef.field === 'minEff' || params.colDef.field === 'maxEff'
        ? '#e5fff9'
        : '#e5e5e5',
    borderBottomColor: '#d3d3d3',
  };
};
