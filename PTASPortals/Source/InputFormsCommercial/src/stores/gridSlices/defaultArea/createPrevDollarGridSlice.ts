/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import {
  calculatePercentChangeFromPrevious,
  calculateValuesGridValues,
  createNewCommercialRow,
} from 'utils';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface PrevDollarGridSlice {
  initialData: CommercialIncomeProps[];
  setInitialData: (initialData: CommercialIncomeProps[]) => void;
  data: CustomRowData<CommercialIncomeProps>[];
  setData: (data: CustomRowData<CommercialIncomeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createPrevDollarGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): PrevDollarGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => {
      const leaseRateData = createNewCommercialRow(
        data,
        state.leaseRateGrid.data,
        ['minEff', 'maxEff']
      );

      const valuesGridCalcResult = calculateValuesGridValues(
        leaseRateData,
        state.leaseRate
      );

      return {
        prevDollarGrid: { ...state.prevDollarGrid, data },
        leaseRateGrid: {
          ...state.leaseRateGrid,
          data: leaseRateData,
        },
        valuesGrid: {
          ...state.valuesGrid,
          data: valuesGridCalcResult,
        },
        prevPercentGrid: {
          ...state.prevPercentGrid,
          data: calculatePercentChangeFromPrevious(valuesGridCalcResult, data, ["minEff", "maxEff"]),
        },
      };
    });
  },
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: (initialData) =>
    set((state) => ({
      prevDollarGrid: { ...state.prevDollarGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      prevDollarGrid: {
        ...state.prevDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().prevDollarGrid.data);
    if (errors.length) {
      set((state) => ({
        prevDollarGrid: {
          ...state.prevDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        prevDollarGrid: {
          ...state.prevDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      prevDollarGrid: {
        ...state.prevDollarGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createPrevDollarGridSlice;
