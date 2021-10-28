/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface OpExpPercentGridSlice {
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

const createOpExpPercentGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): OpExpPercentGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ opExpPercentGrid: { ...state.opExpPercentGrid, data } }));
    if (parseFloat(get().opExpensesAf) === 0) {
      set((state) => ({
        opExpPercentGrid: { ...state.opExpPercentGrid, initialData: data },
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
      opExpPercentGrid: { ...state.opExpPercentGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      opExpPercentGrid: {
        ...state.opExpPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().opExpPercentGrid.data);
    if (errors.length) {
      set((state) => ({
        opExpPercentGrid: {
          ...state.opExpPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        opExpPercentGrid: {
          ...state.opExpPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      opExpPercentGrid: {
        ...state.opExpPercentGrid,
        data: state.opExpPercentGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      opExpensesAf: "0"
    })),
});

export default createOpExpPercentGridSlice;
