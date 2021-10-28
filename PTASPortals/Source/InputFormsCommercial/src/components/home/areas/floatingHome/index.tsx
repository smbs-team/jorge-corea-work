// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Fragment } from 'react';
import { useEffectOnce } from 'react-use';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import useRibbonStore from 'stores/useRibbonStore';
import ReplacementCost from './sections/ReplacementCost';
import SlipValues from './sections/SlipValues';

function FloatingHome(): JSX.Element {
  const ribbonStore = useRibbonStore();
  const headerStore = useContentHeaderStore();

  useEffectOnce(() => {
    ribbonStore.setHideAdjustmentFactors(false);
    ribbonStore.setHideConfiguration(true);
    ribbonStore.setHideRentableArea(true);
    headerStore.setHidePagination(true);
    headerStore.setHideYear(false);
    ribbonStore.setHideSettings(true);
    ribbonStore.setShowHotelsConfig(false);
  });

  return (
    <Fragment>
      <SlipValues />
      <ReplacementCost />
    </Fragment>
  );
}

export default FloatingHome;
