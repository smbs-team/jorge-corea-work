/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { ReplacementCostProps } from 'components/home/areas/floatingHome/sections/ReplacementCost';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface ReplacementCostPrevYearDollarGridSlice {
  initialData: ReplacementCostProps[];
  setInitialData: (initialData: ReplacementCostProps[]) => void;
  data: CustomRowData<ReplacementCostProps>[];
  setData: (data: CustomRowData<ReplacementCostProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createReplacementCostPrevYearDollarGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): ReplacementCostPrevYearDollarGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      replacementCostPrevYearDollarGrid: {
        ...state.replacementCostPrevYearDollarGrid,
        data,
      },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      replacementCostPrevYearDollarGrid: {
        ...state.replacementCostPrevYearDollarGrid,
        initialData,
      },
    })),
  validate: () => {
    set(state => ({
      replacementCostPrevYearDollarGrid: {
        ...state.replacementCostPrevYearDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        replacementCostPrevYearDollarGrid: {
          ...state.replacementCostPrevYearDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        replacementCostPrevYearDollarGrid: {
          ...state.replacementCostPrevYearDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      replacementCostPrevYearDollarGrid: {
        ...state.replacementCostPrevYearDollarGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createReplacementCostPrevYearDollarGridSlice;
