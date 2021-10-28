/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface MajorBaseYearsSlice {
  initialData: MajorOfficeProps[];
  setInitialData: (initialData: MajorOfficeProps[]) => void;
  data: CustomRowData<MajorOfficeProps>[];
  setData: (data: CustomRowData<MajorOfficeProps>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createMajorBaseYearsSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorBaseYearsSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      majorBaseYearsGrid: { ...state.majorBaseYearsGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      majorBaseYearsGrid: { ...state.majorBaseYearsGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      majorBaseYearsGrid: {
        ...state.majorBaseYearsGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        majorBaseYearsGrid: {
          ...state.majorBaseYearsGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        majorBaseYearsGrid: {
          ...state.majorBaseYearsGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      majorBaseYearsGrid: {
        ...state.majorBaseYearsGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createMajorBaseYearsSlice;
