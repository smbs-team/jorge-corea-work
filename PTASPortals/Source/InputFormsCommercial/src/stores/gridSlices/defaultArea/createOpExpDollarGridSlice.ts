/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface OpExpDollarGridSlice {
  initialData: CustomRowData<CommercialIncomeProps>[];
  setInitialData: (initialData: CustomRowData<CommercialIncomeProps>[]) => void;
  data: CustomRowData<CommercialIncomeProps>[];
  setData: (data: CustomRowData<CommercialIncomeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createOpExpDollarGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): OpExpDollarGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ opExpDollarGrid: { ...state.opExpDollarGrid, data } }));
    if (parseFloat(get().opExpensesAf) === 0) {
      set((state) => ({
        opExpDollarGrid: { ...state.opExpDollarGrid, initialData: data },
      }));
    }
  },
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: (initialData) =>
    set((state) => ({
      opExpDollarGrid: { ...state.opExpDollarGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      opExpDollarGrid: {
        ...state.opExpDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().opExpDollarGrid.data);
    if (errors.length) {
      set((state) => ({
        opExpDollarGrid: {
          ...state.opExpDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        opExpDollarGrid: {
          ...state.opExpDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      opExpDollarGrid: {
        ...state.opExpDollarGrid,
        data: state.opExpDollarGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    opExpensesAf: "0"
    })),
});

export default createOpExpDollarGridSlice;
