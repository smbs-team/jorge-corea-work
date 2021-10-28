// useSearchForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useCallback } from 'react';
import { debounce } from 'lodash';
import { AddressItem } from 'services/map';
import { apiService } from 'services/api';

interface UseLocationFound {
  searchByAddress: (val: string) => void;
  autoCompleteData: AddressItem[];
  runSearch: (val: string) => void;
  searchResult: AddressItem | undefined;
}

export const useLocationFound = (): UseLocationFound => {
  const [searchResult, setSearchResult] = useState<AddressItem>();
  const [autoCompleteData, setAutoCompleteData] = useState<AddressItem[]>([]);

  const getAutoCompleteData = useCallback(
    debounce(async (val: string) => {
      if (!val) return;
      const r = await apiService.getAddressSuggestions(val);
      setAutoCompleteData(r.data ?? []);
    }, 200),
    []
  );

  const runSearch = useCallback(
    debounce(async (val: string) => {
      if (!val) return;
      const r = await apiService.getAddressSuggestions(val);
      setSearchResult((r.data ?? [])[0]);
    }, 500),
    []
  );

  return {
    searchByAddress: getAutoCompleteData,
    autoCompleteData,
    runSearch,
    searchResult,
  };
};
