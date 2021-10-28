import { BaseInputs } from './createBaseInputsGridSlice';
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData, InvalidRow } from '@ptas/react-ui-library';
import { PageManagerStoreProps } from 'stores/usePageManagerStore';
import { GetState, SetState } from 'zustand';

export interface HostsGridSlice {
  initialData: BaseInputs[];
  setInitialData: (initialData: BaseInputs[]) => void;
  data: CustomRowData<BaseInputs>[];
  setData: (data: CustomRowData<BaseInputs>[]) => void;
  errors: InvalidRow[];
  hasErrors: boolean;
  isDirty: boolean;
  validate: () => void;
  isLoading: boolean;
  reset: () => void;
}

export const hostData = [
  { name: 'ADR(base)', value: '$145.68' },
  { name: 'OCC(base)', value: '73.1%' },
  { name: 'Percentages of total income', value: '' },
  { name: 'Rooms', value: '80.5%' },
  { name: 'Food', value: '10.1%' },
  { name: 'Beverage', value: '3.0%' },
  { name: 'Other food & beverage', value: '2.3%' },
  { name: 'Other operated departments', value: '2.1%' },
  { name: 'Miscellaneous income', value: '2.0%' },
  { name: 'Departmental expense percentages', value: '' },
  { name: 'Rooms', value: '24.5%' },
  { name: 'Food & beverage', value: '76.3%' },
  { name: 'Other operated departments & rentals', value: '55.1%' },
  { name: 'Expense percentages', value: '' },
  { name: 'Administrative & general', value: '8.7%' },
  { name: 'Information & telecommunications systems', value: '1.1%' },
  { name: 'Marketing', value: '7.1%' },
  { name: 'Franchise fees', value: '3.5%' },
  { name: 'Utility costs', value: '3.5%' },
  { name: 'Property operation & maintenance', value: '4.2%' },
  { name: 'Management fees', value: '3.5%' },
  { name: 'Insurance', value: '1.0%' },
  { name: 'Insurance', value: '1.0%' },
  { name: 'Reserve for capital replacement', value: '2.2%' },
];

const createHostsGridSlice = (
  set: SetState<PageManagerStoreProps>,
  get: GetState<PageManagerStoreProps>
): HostsGridSlice => ({
  data: [],
  setData: data => set(state => ({ hostsGrid: { ...state.hostsGrid, data } })),
  hasErrors: false,
  errors: [],
  isDirty: false,
  isLoading: false,
  initialData: hostData,
  setInitialData: initialData =>
    set(state => ({
      hostsGrid: { ...state.hostsGrid, initialData },
    })),
  validate: () => {
    set(state => ({
      hostsGrid: {
        ...state.hostsGrid,
        isDirty: true,
        isLoading: true,
      },
    }));
    const errors: InvalidRow[] = [];
    if (errors.length) {
      set(state => ({
        hostsGrid: {
          ...state.hostsGrid,
          hasErrors: true,
          errors,
          isLoading: false,
        },
      }));
    } else {
      set(state => ({
        hostsGrid: {
          ...state.hostsGrid,
          hasErrors: false,
          errors,
          isLoading: false,
        },
      }));
    }
  },
  reset: () =>
    set(state => ({
      hostsGrid: {
        ...state.hostsGrid,
        initialData: hostData,
        data: [],
        errors: [],
        hasErrors: false,
        isDirty: false,
        isLoading: false,
      },
    })),
});

export default createHostsGridSlice;
