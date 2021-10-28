/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { MapThumbnail } from 'components/MapThumbnail';
import { AppContext, HomeContext, HomeProvider } from 'contexts';
import { CalculateProvider } from 'contexts/CalculateContext';
import { MapProvider } from 'contexts/MapContext';
import { ParcelInfoProvider } from 'contexts/ParcelInfoContext';
import { withAuth } from 'hoc/withAuth';
import { withMap } from 'hoc/withMap';
import mapboxgl from 'mapbox-gl';
import React, {
  ComponentType,
  FC,
  Fragment,
  lazy,
  PropsWithChildren,
  Suspense,
  useContext,
} from 'react';
import { useEffectOnce } from 'react-use';
const ContentWrapper = lazy(() => import('components/ContentWrapper'));
const AppBar = lazy(() => import('components/AppBar'));
const ExportMapImgModal = lazy(() => import('components/ExportMapImgModal'));
const SaveMapPopovers = lazy(() =>
  import('components/ContentWrapper/SaveNewMapPopover')
);

type Props = {
  map: mapboxgl.Map;
};

function Home({ map }: Props): JSX.Element {
  const { mapThumbnail: enableMapThumbnail } = useContext(HomeContext);
  return (
    <Fragment>
      <CalculateProvider>
        <ParcelInfoProvider map={map}>
          <Suspense fallback={null}>
            <AppBar />
          </Suspense>
          <Suspense fallback={null}>
            <ContentWrapper />
          </Suspense>
          {enableMapThumbnail && <MapThumbnail />}
        </ParcelInfoProvider>
      </CalculateProvider>
      <Suspense fallback={null}>
        <ExportMapImgModal />
      </Suspense>
      <Suspense fallback={null}>
        <SaveMapPopovers />
      </Suspense>
    </Fragment>
  );
}

const withHome = <P extends object>(Component: ComponentType<P>): FC<P> => (
  props: PropsWithChildren<P>
): JSX.Element | null => {
  const { setShowBackdrop } = useContext(AppContext);
  useEffectOnce(() => {
    setShowBackdrop(true);
  });
  return (
    <MapProvider>
      <HomeProvider>{<Component {...props} />}</HomeProvider>
    </MapProvider>
  );
};

export default withHome(withMap(withAuth(Home)));
