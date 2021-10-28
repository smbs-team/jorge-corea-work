/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorOpExpDollarGridSlice {
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

const createMajorOpExpDollarGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorOpExpDollarGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({
      majorOpExpDollarGrid: { ...state.majorOpExpDollarGrid, data },
    }));
    if (parseFloat(get().opExpensesAf) === 0) {
      set((state) => ({
        majorOpExpDollarGrid: {
          ...state.majorOpExpDollarGrid,
          initialData: data,
        },
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
      majorOpExpDollarGrid: { ...state.majorOpExpDollarGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorOpExpDollarGrid: {
        ...state.majorOpExpDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorOpExpDollarGrid: {
          ...state.majorOpExpDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        majorOpExpDollarGrid: {
          ...state.majorOpExpDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorOpExpDollarGrid: {
        ...state.majorOpExpDollarGrid,
        data: state.majorOpExpDollarGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      opExpensesAf: "0"
    })),
});

export default createMajorOpExpDollarGridSlice;
