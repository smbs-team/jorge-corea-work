/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

//See: https://docs.mapbox.com/help/glossary/zoom-level/

import { MAP_INITIAL_COORDINATES } from 'appConstants';
import { round } from 'lodash';
import { INCHES_PER_METER, utilService } from 'services/common';

/**
 * Convert mapbox zoom to scale
 * @param zoom - Mapbox Zoom
 */

export const mapZoomToScale = (zoom: number): number => {
  const metersPerPixel = utilService.mapZoomToMetersPerPixel(
    MAP_INITIAL_COORDINATES[1],
    zoom
  );

  return round(metersPerPixel * utilService.screenDPI * INCHES_PER_METER);
};

/**
 * Convert Scale to mapbox zoom
 * @param x - The scale
 */

export const scaleToMapZoom = (x: number): number =>
  utilService.metersPerPixelToMapZoom(
    MAP_INITIAL_COORDINATES[1],
    x / utilService.screenDPI / INCHES_PER_METER
  );
