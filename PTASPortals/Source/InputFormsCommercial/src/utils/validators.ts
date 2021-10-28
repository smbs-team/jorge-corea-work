// validators.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';

export const validateEffYearGrids = (
  rows: CustomRowData<CommercialIncomeProps>[]
): InvalidRow[] => {
  const errors: InvalidRow[] = [];
  rows.forEach((r, i) => {
    if (!r.data.minEff || !r.data.maxEff) {
      errors.push({
        index: r.rowIndex,
        errorMessage: 'Effective years should not be empty.',
      });
      return;
    }
    if (parseInt(r.data.minEff) >= parseInt(r.data.maxEff)) {
      errors.push({
        index: r.rowIndex,
        errorMessage:
          'Minimum effective year is greater or equal than the maximum effective year.',
      });
      return;
    }

    if (parseInt(r.data.maxEff) < parseInt(r.data.minEff)) {
      errors.push({
        index: r.rowIndex,
        errorMessage:
          'Maximum effective year is lower than the minimum effective year.',
      });
      return;
    }

    const prev = rows[i - 1];
    if (!prev) return;
    if (
      !prev.data.minEff ||
      !prev.data.maxEff ||
      !r.data.minEff ||
      !r.data.maxEff
    ) {
      errors.push({
        index: r.rowIndex,
        errorMessage: 'Effective years should not be empty.',
      });
      return;
    }

    if (parseInt(prev.data.maxEff) >= parseInt(r.data.minEff)) {
      errors.push({
        index: r.rowIndex,
        errorMessage:
          'Previous maximum effective year is greater or equal than the current effective minimum year.',
      });
      return;
    }
    if (parseInt(r.data.minEff) - parseInt(prev.data.maxEff) > 1) {
      errors.push({
        index: r.rowIndex,
        errorMessage:
          'There should not be a gap between previous maximum effective year and current minimum effective year.',
      });
      return;
    }
  });

  return errors;
};
