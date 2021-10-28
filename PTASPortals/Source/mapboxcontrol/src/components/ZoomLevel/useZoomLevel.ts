/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useMapZoom } from 'hooks/map';
import { useCallback, useEffect, useState } from 'react';
import selectionService from 'services/parcel/selection';
import { fitAllBbox, fitSelectedBbox } from 'utils/dataset';
import { mapZoomToScale, scaleToMapZoom } from 'utils/zoom';
import { SelectedItem } from './ZoomMenu';

type useZoomLevel = {
  onMenuItemClick: (x: SelectedItem) => void;
  currentZoomLevel: number;
};

export const useZoomLevel = (map: mapboxgl.Map): useZoomLevel => {
  const mapZoom = useMapZoom();
  const [currentZoomLevel, setCurrentZoomLevel] = useState(0);

  useEffect(() => {
    setCurrentZoomLevel(mapZoomToScale(mapZoom));
  }, [mapZoom]);

  const onMenuItemClick = useCallback(
    (x: SelectedItem): void => {
      if (typeof x === 'number') {
        const zoom = scaleToMapZoom(x);
        map.zoomTo(zoom);
      } else if (x === 'county') {
        map.fitBounds([
          -120.9218706141953,
          47.877288481470885,
          -122.69779144791909,
          47.07351949649612,
        ] as [number, number, number, number]);
      }
      //Show all shows blue dots
      //show selected shows red dots
      else if (x === 'all') {
        fitAllBbox(map);
      } else if (x === 'selected') {
        fitSelectedBbox(map, selectionService.getSelectedDotsList());
      }
    },
    [map]
  );

  return {
    currentZoomLevel,
    onMenuItemClick,
  };
};
