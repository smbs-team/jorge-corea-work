/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { MobileHomeProps } from 'components/home/areas/floatingHome/sections/SlipValues';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface SlipPrevYearPercentSlice {
  initialData: MobileHomeProps[];
  setInitialData: (initialData: MobileHomeProps[]) => void;
  data: CustomRowData<MobileHomeProps>[];
  setData: (data: CustomRowData<MobileHomeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createSlipPrevYearPercentSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): SlipPrevYearPercentSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      slipPrevYearPercentGrid: { ...state.slipPrevYearPercentGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      slipPrevYearPercentGrid: {
        ...state.slipPrevYearPercentGrid,
        initialData,
      },
    })),
  validate: () => {
    set(state => ({
      slipPrevYearPercentGrid: {
        ...state.slipPrevYearPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        slipPrevYearPercentGrid: {
          ...state.slipPrevYearPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        slipPrevYearPercentGrid: {
          ...state.slipPrevYearPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      slipPrevYearPercentGrid: {
        ...state.slipPrevYearPercentGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createSlipPrevYearPercentSlice;
