/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { MobileHomeProps } from 'components/home/areas/floatingHome/sections/SlipValues';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface SlipPrevYearDollarSlice {
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

const createSlipPrevYearDollarSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): SlipPrevYearDollarSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      slipPrevYearDollarGrid: { ...state.slipPrevYearDollarGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      slipPrevYearDollarGrid: { ...state.slipPrevYearDollarGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      slipPrevYearDollarGrid: {
        ...state.slipPrevYearDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        slipPrevYearDollarGrid: {
          ...state.slipPrevYearDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        slipPrevYearDollarGrid: {
          ...state.slipPrevYearDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      slipPrevYearDollarGrid: {
        ...state.slipPrevYearDollarGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createSlipPrevYearDollarSlice;
