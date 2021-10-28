/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface ValuesGridSlice {
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

const createValuesGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): ValuesGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({ valuesGrid: { ...state.valuesGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: (initialData) =>
    set((state) => ({
      valuesGrid: { ...state.valuesGrid, initialData },
    })),
  validate: () => {
    //
  },
  reset: () =>
    set(state => ({
      valuesGrid: {
        ...state.valuesGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createValuesGridSlice;
