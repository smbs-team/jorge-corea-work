/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers } from 'appConstants';
import { LayerSource } from 'services/map';

const getMostRecentPicLayer = ((): ((
  layerSources: LayerSource[]
) => LayerSource | undefined) => {
  let mostRecent: { layerSource: LayerSource; year: number } | undefined;
  return (layerSources: LayerSource[]): LayerSource | undefined => {
    for (const layer of layerSources) {
      const match = layer.organization.match(/\d{4}/)?.shift();
      if (!match) continue;
      const year = +match;
      if (!mostRecent || year > mostRecent.year) {
        mostRecent = {
          year,
          layerSource: layer,
        };
      }
    }
    return mostRecent?.layerSource;
  };
})();

export const layerSourcesPipeFn = (items: LayerSource[]): LayerSource[] => {
  const retVal: LayerSource[] = [];
  const mostRecentPicLayer = getMostRecentPicLayer(items);
  if (mostRecentPicLayer) {
    retVal.push(mostRecentPicLayer);
  }
  return items
    .filter((item) => item.defaultMapboxLayer.id === AppLayers.PARCEL_LAYER)
    .concat(mostRecentPicLayer ? [mostRecentPicLayer] : [])
    .sort((a) => (a.defaultMapboxLayer.type === 'raster' ? -1 : -1));
};
