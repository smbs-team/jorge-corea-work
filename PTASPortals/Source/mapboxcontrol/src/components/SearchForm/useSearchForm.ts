// useSearchForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useCallback, useContext } from 'react';
import { debounce } from 'lodash';
import { mapService, ParcelByAddressItem } from 'services/map';
import { getErrorStr } from 'utils/getErrorStr';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';

interface UseSearchForm {
  searchByAddress: (val: string) => void;
  autoCompleteData: ParcelByAddressItem[];
  runSearch: (val: string) => void;
}

export const useSearchForm = (): UseSearchForm => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const [autoCompleteData, setAutoCompleteData] = useState<
    ParcelByAddressItem[]
  >([]);

  const getAutoCompleteData = useCallback(
    debounce(async (val: string) => {
      if (!val) return;
      try {
        const r = await mapService.mapSearch?.getParcelByAddress(val);
        r && setAutoCompleteData(r);
      } catch (e) {
        console.error(e);
        showErrorMessage(getErrorStr(e));
      }
    }, 200),
    []
  );

  const runSearch = useCallback(
    debounce(async (val: string): Promise<void> => {
      try {
        if (!val) return;
        await mapService?.mapSearch?.runParcelSearch(val);
      } catch (e) {
        showErrorMessage(getErrorStr(e));
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
    }, 500),
    []
  );

  return {
    searchByAddress: getAutoCompleteData,
    autoCompleteData,
    runSearch,
  };
};
