// useContentHeaderStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import create from 'zustand';
import { createTrackedSelector } from 'react-tracked';

interface Props {
  hidePagination: boolean;
  hideYear: boolean;
  setHideYear: (hideYear: boolean) => void;
  setHidePagination: (state: boolean) => void;
}

const useStore = create<Props>((set) => ({
  hidePagination: true,
  setHidePagination: (hidePagination) => set({ hidePagination }),
  hideYear: true,
  setHideYear: (hideYear) => set({ hideYear }),
}));

const useContentHeaderStore = createTrackedSelector(useStore);

export default useContentHeaderStore;
