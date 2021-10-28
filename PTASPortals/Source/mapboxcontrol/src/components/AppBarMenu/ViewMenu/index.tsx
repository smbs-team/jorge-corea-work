// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useRef } from 'react';
import { MenuRootContent } from '@ptas/react-ui-library';
import { MenuItem } from '@material-ui/core';
import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';
import { CustomMenuRoot } from 'components/common';
import mapboxgl, { MapEventType } from 'mapbox-gl';
import { withMap } from 'hoc/withMap';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    item: {
      fontSize: '0.875rem',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      height: 27,
      padding: theme.spacing(1),
    },
  })
);

/**
 * Tools menu
 */
function ViewMenu({ map }: { map: mapboxgl.Map }): JSX.Element {
  const classes = useStyles();

  const onObliquesClick = useRef((closeMenu: () => void) => (): void => {
    map.off('mousemove', selectPointMove.current);
    map.off('click', clickedObliquesPoint.current);
    map.on('mousemove', selectPointMove.current);
    map.once('click', clickedObliquesPoint.current);
    closeMenu();
  });

  const onStreetViewClick = (closeMenu: () => void) => (): void => {
    map.off('mousemove', selectPointMove.current);
    map.off('click', clickedStreetViewPoint.current);
    map.on('mousemove', selectPointMove.current);
    map.once('click', clickedStreetViewPoint.current);
    closeMenu();
  };

  const selectPointMove = useRef((): void => {
    if (!map) return;
    map.getCanvas().style.cursor = 'crosshair';
  });

  const clickedObliquesPoint = useRef((e: MapEventType['click']): void => {
    if (!map) return;
    map.getCanvas().style.cursor = '';
    map.off('mousemove', selectPointMove.current);
    window.open(
      `${process.env.REACT_APP_PICTOMETRY_URL}?y=${e.lngLat.lat}&x=${e.lngLat.lng}`
    );
  });

  const clickedStreetViewPoint = useRef((e: MapEventType['click']): void => {
    if (!map) return;
    map.getCanvas().style.cursor = '';
    map.off('mousemove', selectPointMove.current);
    window.open(
      `http://maps.google.com/maps?q=&layer=c&cbll=${e.lngLat.lat},${e.lngLat.lng}&cbp=11,0,0,0,0`,
      '_blank'
    );
  });

  return (
    <CustomMenuRoot text="View">
      {({ closeMenu, menuProps }): JSX.Element => (
        <MenuRootContent {...menuProps}>
          <MenuItem
            className={classes.item}
            onClick={onObliquesClick.current(closeMenu)}
          >
            Obliques
          </MenuItem>
          <MenuItem
            className={classes.item}
            onClick={onStreetViewClick(closeMenu)}
          >
            Street View
          </MenuItem>
        </MenuRootContent>
      )}
    </CustomMenuRoot>
  );
}

export default withMap(ViewMenu) as FC;
