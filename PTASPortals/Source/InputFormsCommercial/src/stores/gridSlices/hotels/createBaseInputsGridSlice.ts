/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface BaseInputs {
  name: string;
  value: string;
}

const baseInputsData = [
  { name: 'ACLI Mortgage rate', value: '5.08%' },
  { name: 'PP Average per room', value: '$4,200' },
];

export interface BaseInputsGridSlice {
  initialData: BaseInputs[];
  setInitialData: (initialData: BaseInputs[]) => void;
  data: CustomRowData<BaseInputs>[];
  setData: (data: CustomRowData<BaseInputs>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createBaseInputsGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): BaseInputsGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      baseInputsGrid: { ...state.baseInputsGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: baseInputsData,
  setInitialData: initialData =>
    set(state => ({
      baseInputsGrid: { ...state.baseInputsGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      baseInputsGrid: {
        ...state.baseInputsGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        baseInputsGrid: {
          ...state.baseInputsGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        baseInputsGrid: {
          ...state.baseInputsGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      baseInputsGrid: {
        ...state.baseInputsGrid,
        initialData: baseInputsData,
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createBaseInputsGridSlice;
