/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import {
  calculatePercentChangeFromPrevious,
  calculateValuesGridValues,
  createNewCommercialRow,
} from 'utils';
import { GetState, SetState } from 'zustand';
import { MajorOfficeProps } from './../../../components/home/areas/commercial/majorOffice/index';

export interface MajorPrevDollarGridSlice {
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

const createMajorPrevDollarGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorPrevDollarGridSlice => ({
  data: [],
  setData: (data) => {
    set((state) => {
      const leaseRateData = createNewCommercialRow(
        data,
        state.majorLeaseRateGrid.data,
        ['empty']
      );

      const valuesGridCalcResult = calculateValuesGridValues(
        leaseRateData,
        state.leaseRate
      );

      return {
        majorPrevDollarGrid: { ...state.majorPrevDollarGrid, data },
        majorLeaseRateGrid: {
          ...state.majorLeaseRateGrid,
          data: leaseRateData,
        },
        majorValuesGrid: {
          ...state.majorValuesGrid,
          data: valuesGridCalcResult,
        },
        majorPrevPercentGrid: {
          ...state.majorPrevPercentGrid,
          data: calculatePercentChangeFromPrevious(valuesGridCalcResult, data, [
            'empty',
          ]),
        },
      };
    });
  },
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: [],
  setInitialData: (initialData) =>
    set((state) => ({
      majorPrevDollarGrid: { ...state.majorPrevDollarGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorPrevDollarGrid: {
        ...state.majorPrevDollarGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorPrevDollarGrid: {
          ...state.majorPrevDollarGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set((state) => ({
        majorPrevDollarGrid: {
          ...state.majorPrevDollarGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorPrevDollarGrid: {
        ...state.majorPrevDollarGrid,
        initialData: [],
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createMajorPrevDollarGridSlice;
