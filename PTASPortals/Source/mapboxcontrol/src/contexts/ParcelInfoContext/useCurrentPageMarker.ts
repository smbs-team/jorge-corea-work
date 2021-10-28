/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DotDsFeature, dotService } from 'services/map/dot';

const dsSource = dotService.store['dot-service-data-set'];
const id = 'ctl-click-dot';

type UseCurrentPageMarker = {
  remove: () => void;
  set: (pin: string, coords: [number, number]) => void;
  id: string;
};

export const useCurrentPageMarker = (): UseCurrentPageMarker => {
  const getIconImage = (pin: string): string | undefined => {
    const dot = dsSource?.get(pin);
    if (dot?.properties.circleColor === dotService.dotColors.blue)
      return dotService.images.locationBlue;
    if (dot?.properties.circleColor === dotService.dotColors.red)
      return dotService.images.locationRed;
    return dotService.images.locationYellow;
  };

  const remove = (options?: { render?: boolean }): void => {
    const render = options?.render === undefined ? true : options.render;
    const dot = dsSource?.get(id);
    if (dot && dot.properties.dotType === 'dataset-dot') {
      dsSource?.delete(id);
      if (dot.properties.prev) {
        dsSource?.set(dot.properties.prev.properties.pin, dot.properties.prev);
      }
      render && dotService.render('dot-service-data-set');
    }
  };

  const set = (pin: string, coords: [number, number]): void => {
    const iconImage = getIconImage(pin);
    remove({ render: false });
    const prev = dsSource?.get(pin) as DotDsFeature | undefined;
    dsSource?.set(id, {
      type: 'Feature',
      geometry: {
        type: 'Point',
        coordinates: coords,
      },
      id: id,
      properties: {
        major: '',
        minor: '',
        prev: prev,
        dotType: 'dataset-dot',
        pin,
        'icon-image': iconImage,
        'icon-anchor': 'bottom',
      },
    });
    dsSource?.delete(pin);
    dotService.render('dot-service-data-set');
  };

  return {
    remove,
    set,
    id,
  };
};
