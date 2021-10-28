/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { ReplacementCostProps } from 'components/home/areas/floatingHome/sections/ReplacementCost';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface ReplacementCostPrevYearPercentGridSlice {
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

const createReplacementCostPrevYearPercentGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): ReplacementCostPrevYearPercentGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      replacementCostPrevYearPercentGrid: {
        ...state.replacementCostPrevYearPercentGrid,
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
      replacementCostPrevYearPercentGrid: {
        ...state.replacementCostPrevYearPercentGrid,
        initialData,
      },
    })),
  validate: () => {
    set(state => ({
      replacementCostPrevYearPercentGrid: {
        ...state.replacementCostPrevYearPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        replacementCostPrevYearPercentGrid: {
          ...state.replacementCostPrevYearPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        replacementCostPrevYearPercentGrid: {
          ...state.replacementCostPrevYearPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      replacementCostPrevYearPercentGrid: {
        ...state.replacementCostPrevYearPercentGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createReplacementCostPrevYearPercentGridSlice;
