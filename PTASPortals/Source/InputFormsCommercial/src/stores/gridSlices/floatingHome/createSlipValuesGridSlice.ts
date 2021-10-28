/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { MobileHomeProps } from 'components/home/areas/floatingHome/sections/SlipValues';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface SlipValuesGridSlice {
  initialData: CustomRowData<MobileHomeProps>[];
  setInitialData: (initialData: CustomRowData<MobileHomeProps>[]) => void;
  data: CustomRowData<MobileHomeProps>[];
  setData: (data: CustomRowData<MobileHomeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createSlipValuesGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): SlipValuesGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ slipValuesGrid: { ...state.slipValuesGrid, data } }));
    if (parseFloat(get().slipValueAf) === 0) {
      set((state) => ({
        slipValuesGrid: { ...state.slipValuesGrid, initialData: data },
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
      slipValuesGrid: { ...state.slipValuesGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      slipValuesGrid: {
        ...state.slipValuesGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        slipValuesGrid: {
          ...state.slipValuesGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        slipValuesGrid: {
          ...state.slipValuesGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      slipValuesGrid: {
        ...state.slipValuesGrid,
        data: state.slipValuesGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      slipValueAf: "0"
    })),
});

export default createSlipValuesGridSlice;
