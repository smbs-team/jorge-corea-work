/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { MajorOfficeProps } from 'components/home/areas/commercial/majorOffice';
import { GetState, SetState } from 'zustand';

export interface MajorValuesGridSlice {
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

const createMajorValuesGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorValuesGridSlice => ({
  data: [],
  setData: data =>
    set(state => ({ majorValuesGrid: { ...state.majorValuesGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: initialData =>
    set(state => ({
      majorValuesGrid: { ...state.majorValuesGrid, initialData },
    })),
  validate: () => {
    //
  },
  reset: () =>
    set(state => ({
      majorValuesGrid: {
        ...state.majorValuesGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createMajorValuesGridSlice;
