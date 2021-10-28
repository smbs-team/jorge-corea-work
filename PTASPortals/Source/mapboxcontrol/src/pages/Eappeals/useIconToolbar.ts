/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { mapService } from 'services/map';
import { mapBoxDrawService } from 'services/map/mapboxDraw';
import navigateToCurrentLocation from 'utils/navigateToCurrentLocation';

type UseIconToolbar = {
  locationOnClick: () => void;
  rulerOnClick: () => void;
  selectionOnClick: () => void;
};

export const useIconToolbar = (): UseIconToolbar => {
  const selectionOnClick = (): void => {
    console.log('selection clicked');
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    return mapBoxDrawService.setMode('freeHand' as any);
  };

  const rulerOnClick = (): void => {
    return mapBoxDrawService.measure?.start();
  };

  const locationOnClick = (): void => {
    mapService.map && navigateToCurrentLocation(mapService.map);
  };

  return {
    selectionOnClick,
    rulerOnClick,
    locationOnClick,
  };
};
