/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { HotelsProps } from 'components/home/areas/hotels';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface OccupancyRateGridSlice {
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

const createOccupancyRateGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): OccupancyRateGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({ occupancyRateGrid: { ...state.occupancyRateGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      occupancyRateGrid: { ...state.occupancyRateGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      occupancyRateGrid: {
        ...state.occupancyRateGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        occupancyRateGrid: {
          ...state.occupancyRateGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        occupancyRateGrid: {
          ...state.occupancyRateGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      occupancyRateGrid: {
        ...state.occupancyRateGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createOccupancyRateGridSlice;
