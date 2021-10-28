/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorCapRatesGridSlice {
  initialData: CustomRowData<MajorOfficeProps>[];
  setInitialData: (initialData: CustomRowData<MajorOfficeProps>[]) => void;
  data: CustomRowData<MajorOfficeProps>[];
  setData: (data: CustomRowData<MajorOfficeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createMajorCapRatesGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorCapRatesGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({
      majorCapRatesGrid: { ...state.majorCapRatesGrid, data },
    }));
    if (parseFloat(get().capRate) === 0) {
      set((state) => ({
        majorCapRatesGrid: { ...state.majorCapRatesGrid, initialData: data },
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
      majorCapRatesGrid: { ...state.majorCapRatesGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorCapRatesGrid: {
        ...state.majorCapRatesGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorCapRatesGrid: {
          ...state.majorCapRatesGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        majorCapRatesGrid: {
          ...state.majorCapRatesGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorCapRatesGrid: {
        ...state.majorCapRatesGrid,
        data: state.majorCapRatesGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      capRate: "0"
    })),
});

export default createMajorCapRatesGridSlice;
