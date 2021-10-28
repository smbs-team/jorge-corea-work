// useRibbonStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createTrackedSelector } from 'react-tracked';
import create from 'zustand';

interface Props {
  hideAll: boolean;
  setHideAll: (hideAll: boolean) => void;
  hideAdjustmentFactors: boolean;
  setHideAdjustmentFactors: (hideAdjustmentFactors: boolean) => void;
  hideConfiguration: boolean;
  setHideConfiguration: (hideConfiguration: boolean) => void;
  hideRentableArea: boolean;
  setHideRentableArea: (hideRentableArea: boolean) => void;
  hideValuationReady: boolean;
  setHideValuationReady: (hideValuationReady: boolean) => void;
  hideSettings: boolean;
  setHideSettings: (hideSettings: boolean) => void;
  setDefault: () => void;
  showHotelsConfig: boolean;
  setShowHotelsConfig: (hotelsConfig: boolean) => void;
}

const useStore = create<Props>(set => ({
  hideAll: false,
  setHideAll: hideAll => set({ hideAll }),
  showHotelsConfig: false,
  setShowHotelsConfig: showHotelsConfig => set({ showHotelsConfig }),
  hideSettings: false,
  setHideSettings: hideSettings => set({ hideSettings }),
  hideAdjustmentFactors: false,
  setHideAdjustmentFactors: hideAdjustmentFactors =>
    set({ hideAdjustmentFactors }),
  hideConfiguration: false,
  setHideConfiguration: hideConfiguration => set({ hideConfiguration }),
  hideRentableArea: false,
  setHideRentableArea: hideRentableArea => set({ hideRentableArea }),
  hideValuationReady: false,
  setHideValuationReady: hideValuationReady => set({ hideValuationReady }),
  setDefault: () => {
    set({
      hideAll: false,
      hideAdjustmentFactors: false,
      hideConfiguration: false,
      hideRentableArea: false,
      hideValuationReady: false,
      hideSettings: false,
      showHotelsConfig: false,
    });
  },
}));

const useRibbonStore = createTrackedSelector(useStore);

export default useRibbonStore;
