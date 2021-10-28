/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { HotelsProps } from 'components/home/areas/hotels';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface HotelsCapRateGridSlice {
  initialData: HotelsProps[];
  setInitialData: (initialData: HotelsProps[]) => void;
  data: CustomRowData<HotelsProps>[];
  setData: (data: CustomRowData<HotelsProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createHotelsCapRateGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): HotelsCapRateGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({ hotelsCapRateGrid: { ...state.hotelsCapRateGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      hotelsCapRateGrid: { ...state.hotelsCapRateGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      hotelsCapRateGrid: {
        ...state.hotelsCapRateGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        hotelsCapRateGrid: {
          ...state.hotelsCapRateGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        hotelsCapRateGrid: {
          ...state.hotelsCapRateGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      hotelsCapRateGrid: {
        ...state.hotelsCapRateGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createHotelsCapRateGridSlice;
