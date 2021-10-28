/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorVacancyGridSlice {
  initialData: CustomRowData<MajorOfficeProps>[];
  setInitialData: (initialData: CustomRowData<MajorOfficeProps>[]) => void;
  data: CustomRowData<MajorOfficeProps>[];
  setData: (data: CustomRowData<MajorOfficeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createMajorVacancyGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorVacancyGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => ({ majorVacancyGrid: { ...state.majorVacancyGrid, data } }));
    if (parseFloat(get().vacancyAf) === 0) {
      set((state) => ({
        majorVacancyGrid: { ...state.majorVacancyGrid, initialData: data },
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
      majorVacancyGrid: { ...state.majorVacancyGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorVacancyGrid: {
        ...state.majorVacancyGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorVacancyGrid: {
          ...state.majorVacancyGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        majorVacancyGrid: {
          ...state.majorVacancyGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorVacancyGrid: {
        ...state.majorVacancyGrid,
        data: state.majorVacancyGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      vacancyAf: "0"
    })),
});

export default createMajorVacancyGridSlice;
