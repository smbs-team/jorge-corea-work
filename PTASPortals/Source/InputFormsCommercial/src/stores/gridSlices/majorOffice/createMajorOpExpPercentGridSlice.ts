/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorOpExpPercentGridSlice {
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

const createMajorOpExpPercentGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorOpExpPercentGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({
      majorOpExpPercentGrid: { ...state.majorOpExpPercentGrid, data },
    }));
    if (parseFloat(get().opExpensesAf) === 0) {
      set((state) => ({
        majorOpExpPercentGrid: {
          ...state.majorOpExpPercentGrid,
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
      majorOpExpPercentGrid: { ...state.majorOpExpPercentGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorOpExpPercentGrid: {
        ...state.majorOpExpPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorOpExpPercentGrid: {
          ...state.majorOpExpPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        majorOpExpPercentGrid: {
          ...state.majorOpExpPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorOpExpPercentGrid: {
        ...state.majorOpExpPercentGrid,
        data: state.majorOpExpPercentGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      opExpensesAf: "0"
    })),
});

export default createMajorOpExpPercentGridSlice;
