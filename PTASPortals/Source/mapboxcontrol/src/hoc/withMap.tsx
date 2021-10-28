/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useGlMap } from 'hooks/map/useGlMap';
import mapboxgl from 'mapbox-gl';
import React, { Fragment } from 'react';
import { ComponentType, FC, PropsWithChildren } from 'react';

export const withMap = <P extends object>(
  Component: ComponentType<P & { map: mapboxgl.Map }>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  const map = useGlMap();
  return (
    <Fragment>
      {map && (
        <Component {...props} map={map}>
          {props.children}
        </Component>
      )}
    </Fragment>
  );
};
