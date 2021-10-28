/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import InfoMode from 'components/Tools/InfoMode';
import { HomeContext } from 'contexts';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import { useGlMap } from 'hooks/map/useGlMap';
import React, { useCallback, useEffect } from 'react';
import { useContext } from 'react';
import { mapService } from 'services/map';
import { mapBoxDrawService } from 'services/map/mapboxDraw';
import selectionService from 'services/parcel/selection';
import navigateToCurrentLocation from 'utils/navigateToCurrentLocation';

type UseIconToolBar = {
  infoOnClick: () => void;
  locationOnClick: () => void;
  rulerOnClick: () => void;
  selectionOnClick: () => void;
};

export const useIconToolbar = (): UseIconToolBar => {
  const map = useGlMap();
  const { setPanelContent } = useContext(HomeContext);
  const { setSelectedParcelInfo } = useContext(ParcelInfoContext);
  const { setActionMode, actionMode } = useContext(DrawToolBarContext);
  /**
   * Info mode
   */
  const infoOnClick = useCallback((): void => {
    setActionMode((prev) => {
      if (prev === 'info-mode') {
        setPanelContent(null);
        return null;
      }
      return 'info-mode';
    });
  }, [setActionMode, setPanelContent]);

  /**
   * Location
   */
  const locationOnClick = useCallback((): void => {
    if (actionMode === 'info-mode') {
      setPanelContent(null);
    }
    setActionMode('location');
  }, [actionMode, setActionMode, setPanelContent]);

  /**
   * Measure
   */
  const rulerOnClick = useCallback((): void => {
    if (actionMode === 'info-mode') {
      setPanelContent(null);
    } else if (actionMode === 'draw') {
      selectionService.changeSelection('disabled');
    }

    setActionMode((prev) => {
      if (prev === 'measure') {
        mapBoxDrawService.measure?.stop();
        return null;
      }

      return 'measure';
    });
  }, [actionMode, setActionMode, setPanelContent]);

  /**
   * Selection
   */
  const selectionOnClick = useCallback((): void => {
    if (actionMode === 'info-mode') {
      setPanelContent(null);
    }
    setActionMode((prev) => (prev === 'draw' ? null : 'draw'));
  }, [actionMode, setActionMode, setPanelContent]);

  useEffect(() => {
    if (!map) return;
    switch (actionMode) {
      case 'location': {
        mapBoxDrawService.measure?.stop();
        selectionService.changeSelection('disabled');
        return mapService.map && navigateToCurrentLocation(mapService.map);
      }
      case 'measure': {
        mapBoxDrawService.init(map);
        selectionService.changeSelection('disabled');
        return mapBoxDrawService.measure?.start();
      }
      case 'info-mode': {
        return setPanelContent(<InfoMode />);
      }
    }
  }, [actionMode, map, setPanelContent, setSelectedParcelInfo]);

  return {
    infoOnClick,
    locationOnClick,
    rulerOnClick,
    selectionOnClick,
  };
};
