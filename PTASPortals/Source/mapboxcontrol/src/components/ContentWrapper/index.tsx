// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, Fragment, lazy, Suspense, useMemo } from 'react';
import { HomeContext } from 'contexts';
import useSelectedParcelLabel from 'hooks/map/useSelectedParcelLabel';
import { useLoadDsPoints } from './useLoadDsPoints';
import LeftSidebar from './LeftSideBar';
import { useHeaderHeight } from './useHeaderHeight';
import PanelContentRoot from './PanelContentRoot';
const ParcelInfoPanel = lazy(() => import('./ParcelInfoPanel'));

function ContentWrapper(): JSX.Element {
  const homeContext = useContext(HomeContext);
  const { panelHeight, panelContent } = homeContext;
  const headerHeight = useHeaderHeight();
  useSelectedParcelLabel();
  useLoadDsPoints();

  return useMemo(
    () => (
      <Fragment>
        {panelContent && <PanelContentRoot>{panelContent}</PanelContentRoot>}
        <LeftSidebar />
        <Suspense fallback={null}>
          <ParcelInfoPanel
            headerHeight={headerHeight}
            panelHeight={panelHeight}
          />
        </Suspense>
      </Fragment>
    ),
    [headerHeight, panelContent, panelHeight]
  );
}

export default ContentWrapper;
