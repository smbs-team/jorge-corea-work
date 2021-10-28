/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { useContext, useEffect, useMemo, useState } from 'react';
import { CircularProgress, makeStyles } from '@material-ui/core';
import { HomeContext } from 'contexts';
import { withMap } from 'hoc/withMap';
import { useRef } from 'react';
import { MapThumbnailService } from 'services/map/MapThumbnailService';
import { useDebounce, useUnmount } from 'react-use';
import mapboxgl from 'mapbox-gl';
import { useThumbnailData } from './useParcelData';
import ThumbnailEl from './ThumbnailEl';
import noImage from 'assets/img/no-image.png';

//See https://github.com/mapbox/mapbox-gl-js/issues/5285

const useStyles = makeStyles(() => ({
  loadingWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    '& > div:first-child': {
      paddingBottom: '4px',
    },
  },
  thumbnail: {
    height: 250,
    width: 250,
  },
}));

export const MapThumbnail = withMap(({ map }): JSX.Element | null => {
  const elRef = useRef<HTMLDivElement | null>(null);
  const classes = useStyles();
  const { mapThumbnail: enableMapThumbnail, mediaToken } = useContext(
    HomeContext
  );

  const mapThumbnailService = useMemo(() => new MapThumbnailService(map), [
    map,
  ]);
  const [pin, setPin] = useState<string>();
  const [coords, setCoords] = useState<[number, number]>([0, 0]);

  useEffect(() => {
    const sub = mapThumbnailService.$onToggle.subscribe(({ pin, coords }) => {
      setPin(pin);
      setCoords(coords);
    });
    return (): void => {
      sub.unsubscribe();
    };
  }, [mapThumbnailService.$onToggle]);

  const { image, loading, parcelDetails } = useThumbnailData({
    pin,
    service: mapThumbnailService,
    enableMapThumbnail,
    mediaToken,
  });

  useDebounce(
    () => {
      mapThumbnailService._popUp?.remove();
      if (!elRef.current) return;
      if (!pin) return;
      mapThumbnailService._popUp = new mapboxgl.Popup(
        MapThumbnailService.popupOptions
      )
        .setLngLat(coords)
        .setDOMContent(elRef.current)
        .addTo(map);
    },
    50,
    [map, pin, loading, parcelDetails, coords]
  );

  useUnmount(() => {
    mapThumbnailService._popUp?.remove();
  });

  return pin ? (
    <div style={{ display: 'none' }}>
      <div ref={elRef}>
        {parcelDetails ? (
          <ThumbnailEl
            pin={parcelDetails.Major + parcelDetails.Minor}
            imageUrl={image ?? noImage}
            footerText={{
              lineOne: ['CmlImp', 'CmlAccy', 'CmlVac'].includes(
                parcelDetails.GeneralClassif
              )
                ? parcelDetails.PropName
                : parcelDetails.TaxpayerName,
              lineTwo: parcelDetails.AddrLine,
            }}
          />
        ) : loading ? (
          <div className={classes.loadingWrap}>
            <div>Fetching data</div>
            <div>
              <CircularProgress color="inherit" />
            </div>
          </div>
        ) : null}
      </div>
    </div>
  ) : null;
});
