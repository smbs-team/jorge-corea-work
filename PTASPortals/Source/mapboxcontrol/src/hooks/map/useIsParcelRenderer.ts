/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext } from 'react';
import { HomeContext } from 'contexts';
import { parcelUtil } from 'utils';

export const useIsParcelRenderer = (): boolean => {
  const { currentLayer } = useContext(HomeContext);
  return parcelUtil.isParcelRenderer(currentLayer);
};
