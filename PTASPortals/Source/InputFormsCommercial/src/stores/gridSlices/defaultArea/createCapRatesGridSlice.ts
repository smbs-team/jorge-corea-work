/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface CapRatesGridSlice {
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

const createCapRatesGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): CapRatesGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ capRatesGrid: { ...state.capRatesGrid, data } }));
    if (parseFloat(get().capRate) === 0) {
      set((state) => ({
        capRatesGrid: { ...state.capRatesGrid, initialData: data },
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
      capRatesGrid: { ...state.capRatesGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      capRatesGrid: {
        ...state.capRatesGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().capRatesGrid.data);
    if (errors.length) {
      set((state) => ({
        capRatesGrid: {
          ...state.capRatesGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        capRatesGrid: {
          ...state.capRatesGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      capRatesGrid: {
        ...state.capRatesGrid,
        data: state.capRatesGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      capRate: "0"
    })),
});

export default createCapRatesGridSlice;
