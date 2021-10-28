/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface PrevPercentGridSlice {
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

const createPrevPercentGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): PrevPercentGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({ prevPercentGrid: { ...state.prevPercentGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: (initialData) =>
    set((state) => ({
      prevPercentGrid: { ...state.prevPercentGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      prevPercentGrid: {
        ...state.prevPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().prevPercentGrid.data);
    if (errors.length) {
      set(state => ({
        prevPercentGrid: {
          ...state.prevPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        prevPercentGrid: {
          ...state.prevPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      prevPercentGrid: {
        ...state.prevPercentGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createPrevPercentGridSlice;
