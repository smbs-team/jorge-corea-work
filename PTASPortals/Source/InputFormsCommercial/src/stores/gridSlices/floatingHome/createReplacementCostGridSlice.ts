/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { ReplacementCostProps } from 'components/home/areas/floatingHome/sections/ReplacementCost';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface ReplacementCostGridSlice {
  initialData: CustomRowData<ReplacementCostProps>[];
  setInitialData: (initialData: CustomRowData<ReplacementCostProps>[]) => void;
  data: CustomRowData<ReplacementCostProps>[];
  setData: (data: CustomRowData<ReplacementCostProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createReplacementCostGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): ReplacementCostGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({
      replacementCostGrid: { ...state.replacementCostGrid, data },
    }));
    if (parseFloat(get().replacementCostAf) === 0) {
      set((state) => ({
        replacementCostGrid: {
          ...state.replacementCostGrid,
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
      replacementCostGrid: { ...state.replacementCostGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      replacementCostGrid: {
        ...state.replacementCostGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        replacementCostGrid: {
          ...state.replacementCostGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        replacementCostGrid: {
          ...state.replacementCostGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      replacementCostGrid: {
        ...state.replacementCostGrid,
        data: state.replacementCostGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      replacementCostAf: "0"
    })),
});

export default createReplacementCostGridSlice;
