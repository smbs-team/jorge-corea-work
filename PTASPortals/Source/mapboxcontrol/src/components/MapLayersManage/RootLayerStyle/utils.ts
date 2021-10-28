/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RGBA_TRANSPARENT } from 'appConstants';
import { PtasLayer } from 'services/map/model';

export const getLayerFillColor = (layer: PtasLayer | undefined): string => {
  let retVal: string | mapboxgl.StyleFunction | mapboxgl.Expression | undefined;
  switch (layer?.type) {
    case 'fill': {
      retVal = layer.paint?.['fill-color'];
      break;
    }
    case 'circle': {
      retVal = layer.paint?.['circle-color'];
      break;
    }
  }
  return typeof retVal === 'string' ? retVal : RGBA_TRANSPARENT;
};

export const getLayerOutlineColor = (layer: PtasLayer | undefined): string => {
  let retVal: string | mapboxgl.StyleFunction | mapboxgl.Expression | undefined;
  switch (layer?.type) {
    case 'fill': {
      retVal = layer.paint?.['fill-outline-color'];
      break;
    }
    case 'line': {
      retVal = layer.paint?.['line-color'];
      break;
    }
    case 'circle': {
      retVal = layer.paint?.['circle-stroke-color'];
      break;
    }
  }
  return typeof retVal === 'string' ? retVal : RGBA_TRANSPARENT;
};
