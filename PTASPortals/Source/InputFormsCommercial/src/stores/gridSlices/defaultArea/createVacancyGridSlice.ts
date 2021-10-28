/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  CustomRowData,
  InvalidRow,
  OnChangeEventTypes,
} from '@ptas/react-ui-library';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';

export interface VacancyGridSlice {
  initialData: CustomRowData<CommercialIncomeProps>[];
  setInitialData: (initialData: CustomRowData<CommercialIncomeProps>[]) => void;
  data: CustomRowData<CommercialIncomeProps>[];
  setData: (
    data: CustomRowData<CommercialIncomeProps>[],
    eventType?: OnChangeEventTypes
  ) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createVacancyGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): VacancyGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ vacancyGrid: { ...state.vacancyGrid, data } }));
    if (parseFloat(get().vacancyAf) === 0) {
      set((state) => ({
        vacancyGrid: { ...state.vacancyGrid, initialData: data },
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
      vacancyGrid: { ...state.vacancyGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      vacancyGrid: {
        ...state.vacancyGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().vacancyGrid.data);
    if (errors.length) {
      set((state) => ({
        vacancyGrid: {
          ...state.vacancyGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        vacancyGrid: {
          ...state.vacancyGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      vacancyGrid: {
        ...state.vacancyGrid,
        data: state.vacancyGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      vacancyAf: "0"
    })),
});

export default createVacancyGridSlice;
