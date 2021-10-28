/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorPrevPercentGridSlice {
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

const createMajorPrevPercentGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorPrevPercentGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({
      majorPrevPercentGrid: { ...state.majorPrevPercentGrid, data },
    })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      majorPrevPercentGrid: { ...state.majorPrevPercentGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      majorPrevPercentGrid: {
        ...state.majorPrevPercentGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        majorPrevPercentGrid: {
          ...state.majorPrevPercentGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        majorPrevPercentGrid: {
          ...state.majorPrevPercentGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      majorPrevPercentGrid: {
        ...state.majorPrevPercentGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createMajorPrevPercentGridSlice;
