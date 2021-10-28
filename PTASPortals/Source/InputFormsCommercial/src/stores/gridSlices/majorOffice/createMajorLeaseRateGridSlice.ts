import { MajorOfficeProps } from './../../../components/home/areas/commercial/majorOffice/index';
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

export interface MajorLeaseRateGridSlice {
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

const createMajorLeaseRateGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): MajorLeaseRateGridSlice => ({
  data: [],
  setData: (data) => {
    if (parseFloat(get().leaseRateAf) === 0) {
      console.log("IN", data);
      set((state) => ({
        majorLeaseRateGrid: { ...state.majorLeaseRateGrid, initialData: data },
      }));
    }

    set((state) => {
      const valuesGridCalcResult = calculateValuesGridValues(
        data,
        state.leaseRate
      );
      return {
        majorLeaseRateGrid: { ...state.majorLeaseRateGrid, data },
        majorValuesGrid: {
          ...state.majorValuesGrid,
          data: valuesGridCalcResult,
        },
        majorPrevPercentGrid: {
          ...state.majorPrevPercentGrid,
          data: calculatePercentChangeFromPrevious(
            valuesGridCalcResult,
            state.majorPrevDollarGrid.data,
            ['empty']
          ),
        },
        majorPrevDollarGrid: {
          ...state.majorPrevDollarGrid,
          data: createNewCommercialRow(data, state.majorPrevDollarGrid.data, [
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
      majorLeaseRateGrid: { ...state.majorLeaseRateGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      majorLeaseRateGrid: {
        ...state.majorLeaseRateGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set((state) => ({
        majorLeaseRateGrid: {
          ...state.majorLeaseRateGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      // getGrades(get().majorLeaseRateGrid.initialData, get().majorLeaseRateGrid.data, get().leaseRateAf, get().leaseRate);
      set((state) => ({
        majorLeaseRateGrid: {
          ...state.majorLeaseRateGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      majorLeaseRateGrid: {
        ...state.majorLeaseRateGrid,
        data: state.majorLeaseRateGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      leaseRateAf: "0"
    })),
});

export default createMajorLeaseRateGridSlice;
