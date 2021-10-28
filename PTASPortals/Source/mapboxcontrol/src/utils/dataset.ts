/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { bbox, featureCollection } from '@turf/turf';
import mapboxgl from 'mapbox-gl';
import { DotDsFeature, dotService } from 'services/map/dot';

export const fitAllBbox = (map: mapboxgl.Map): void => {
  const _features = dotService.getFeaturesListFor('dot-service-data-set');
  if (!_features.length) return;
  const _bbox = bbox(featureCollection(_features));
  map.fitBounds(_bbox as [number, number, number, number], {
    padding: {
      top: 100,
      bottom: 10,
      left: 0,
      right: 0,
    },
  });
};

export const fitSelectedBbox = (
  map: mapboxgl.Map,
  features: DotDsFeature[]
): void => {
  if (!features.length) return;
  const _bbox = bbox(featureCollection(features));
  map.fitBounds(_bbox as [number, number, number, number], {
    padding: { top: 50, bottom: 50, left: 50, right: 50 },
    minZoom: 8.5,
  });
};
