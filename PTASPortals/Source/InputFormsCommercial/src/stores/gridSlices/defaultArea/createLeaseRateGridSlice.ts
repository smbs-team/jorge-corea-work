/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  CustomRowData,
  InvalidRow,
  OnChangeEventTypes,
} from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import {
  calculatePercentChangeFromPrevious,
  calculateValuesGridValues,
  createNewCommercialRow,
} from 'utils';
import { validateEffYearGrids } from 'utils/validators';
import { GetState, SetState } from 'zustand';
import { CellValueChangedEvent } from 'ag-grid-community';
import { CommercialIncomeProps } from 'components/home/areas/commercial/income';

export interface LeaseRateGridSlice {
  initialData: CustomRowData<CommercialIncomeProps>[];
  setInitialData: (initialData: CustomRowData<CommercialIncomeProps>[]) => void;
  data: CustomRowData<CommercialIncomeProps>[];
  setData: (
    data: CustomRowData<CommercialIncomeProps>[],
    type?: OnChangeEventTypes,
    cellChangeParams?: CellValueChangedEvent
  ) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

const createLeaseRateGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): LeaseRateGridSlice => ({
  data: [],
  setData: (data) => {
    const isLeaseRateActive = parseFloat(get().leaseRateAf) !== 0;
    if (!isLeaseRateActive) {
      set((state) => ({
        leaseRateGrid: {
          ...state.leaseRateGrid,
          initialData: data,
        },
      }));
    }

    set((state) => {
      const valuesGridCalcResult = calculateValuesGridValues(
        data,
        state.leaseRate
      );
      return {
        leaseRateGrid: { ...state.leaseRateGrid, data },
        valuesGrid: {
          ...state.valuesGrid,
          data: valuesGridCalcResult,
        },
        prevPercentGrid: {
          ...state.prevPercentGrid,
          data: calculatePercentChangeFromPrevious(
            valuesGridCalcResult,
            state.prevDollarGrid.data,
            ['id', 'minEff', 'maxEff']
          ),
        },
        prevDollarGrid: {
          ...state.prevDollarGrid,
          data: createNewCommercialRow(data, state.prevDollarGrid.data, [
            'id',
            'minEff',
            'maxEff',
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
      leaseRateGrid: { ...state.leaseRateGrid, initialData },
    })),
  validate: () => {
    set((state) => ({
      leaseRateGrid: {
        ...state.leaseRateGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors = validateEffYearGrids(get().leaseRateGrid.data);
    if (errors.length) {
      set((state) => ({
        leaseRateGrid: {
          ...state.leaseRateGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      // getGrades(get().leaseRateGrid.initialData, get().leaseRateGrid.data, get().leaseRateAf, get().leaseRate);
      set((state) => ({
        leaseRateGrid: {
          ...state.leaseRateGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set((state) => ({
      leaseRateGrid: {
        ...state.leaseRateGrid,
        data: state.leaseRateGrid.initialData,
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
      leaseRateAf: '0',
    })),
});

export default createLeaseRateGridSlice;
