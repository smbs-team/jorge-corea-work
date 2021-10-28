/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { HotelsProps } from 'components/home/areas/hotels';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface AverageRoomRateGridSlice {
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

const createAverageRoomRateGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): AverageRoomRateGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      averageRoomRateGrid: { ...state.averageRoomRateGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      averageRoomRateGrid: { ...state.averageRoomRateGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      averageRoomRateGrid: {
        ...state.averageRoomRateGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        averageRoomRateGrid: {
          ...state.averageRoomRateGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        averageRoomRateGrid: {
          ...state.averageRoomRateGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      averageRoomRateGrid: {
        ...state.averageRoomRateGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createAverageRoomRateGridSlice;
