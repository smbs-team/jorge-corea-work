// useAddressSuggestions.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState } from 'react';
import { ItemSuggestion } from '@ptas/react-public-ui-library';
import { SearchAddressSuggestions, SearchSuggestions } from './types';
import { AddressLookup } from 'services/map/model/addresses';

export interface UseAddressSuggestions {
  addressSuggestions: SearchAddressSuggestions;
  stateSuggestions: SearchSuggestions;
  citySuggestions: SearchSuggestions;
  zipCodeSuggestions: SearchSuggestions;
  updateAddressSuggestions: (
    addressId: string,
    suggestionList?: AddressLookup[],
    loading?: boolean
  ) => void;
  updateCitySuggestions: (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ) => void;
  updateStateSuggestions: (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ) => void;
  updateZipCodeSuggestions: (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ) => void;
}

function useAddressSuggestions(): UseAddressSuggestions {
  const [
    addressSuggestions,
    setAddressSuggestions,
  ] = useState<SearchAddressSuggestions>({
    map: new Map<string, AddressLookup[]>(),
    loading: new Map<string, boolean>(),
  });
  const [citySuggestions, setCitySuggestions] = useState<SearchSuggestions>({
    map: new Map<string, ItemSuggestion[]>(),
    loading: new Map<string, boolean>(),
  });
  const [stateSuggestions, setStateSuggestions] = useState<SearchSuggestions>({
    map: new Map<string, ItemSuggestion[]>(),
    loading: new Map<string, boolean>(),
  });
  const [
    zipCodeSuggestions,
    setZipCodeSuggestions,
  ] = useState<SearchSuggestions>({
    map: new Map<string, ItemSuggestion[]>(),
    loading: new Map<string, boolean>(),
  });

  const updateAddressSuggestions = (
    addressId: string,
    suggestionList?: AddressLookup[],
    loading?: boolean
  ): void => {
    setAddressSuggestions((prev) => {
      const suggestionsMap = prev.map;
      if (suggestionList) {
        suggestionsMap.set(addressId, suggestionList);
      }
      const loadingMap = prev.loading;
      if (loading !== undefined) {
        loadingMap.set(addressId, loading);
      }
      return {
        map: suggestionsMap,
        loading: loadingMap,
      };
    });
  };

  const updateCitySuggestions = (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ): void => {
    setCitySuggestions((prev) => {
      const suggestionsMap = prev.map;
      if (suggestionList) {
        suggestionsMap.set(addressId, suggestionList);
      }
      const loadingMap = prev.loading;
      if (loading !== undefined) {
        loadingMap.set(addressId, loading);
      }
      return {
        map: suggestionsMap,
        loading: loadingMap,
      };
    });
  };

  const updateStateSuggestions = (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ): void => {
    setStateSuggestions((prev) => {
      const suggestionsMap = prev.map;
      if (suggestionList) {
        suggestionsMap.set(addressId, suggestionList);
      }
      const loadingMap = prev.loading;
      if (loading !== undefined) {
        loadingMap.set(addressId, loading);
      }
      return {
        map: suggestionsMap,
        loading: loadingMap,
      };
    });
  };

  const updateZipCodeSuggestions = (
    addressId: string,
    suggestionList?: ItemSuggestion[],
    loading?: boolean
  ): void => {
    setZipCodeSuggestions((prev) => {
      const suggestionsMap = prev.map;
      if (suggestionList) {
        suggestionsMap.set(addressId, suggestionList);
      }
      const loadingMap = prev.loading;
      if (loading !== undefined) {
        loadingMap.set(addressId, loading);
      }
      return {
        map: suggestionsMap,
        loading: loadingMap,
      };
    });
  };

  return {
    addressSuggestions,
    stateSuggestions,
    citySuggestions,
    zipCodeSuggestions,
    updateAddressSuggestions,
    updateCitySuggestions,
    updateStateSuggestions,
    updateZipCodeSuggestions,
  };
}

export default useAddressSuggestions;
