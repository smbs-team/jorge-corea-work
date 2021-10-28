// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useMemo, useState } from 'react';
import {
  Typography,
  makeStyles,
  Box,
  IconButton,
  Theme,
} from '@material-ui/core';
import clsx from 'clsx';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import RemoveCircleOutlineIcon from '@material-ui/icons/RemoveCircleOutline';
import { CustomPopover } from '@ptas/react-ui-library';
import { ZoomMenu } from './ZoomMenu';
import { useZoomLevel } from './useZoomLevel';
import Tooltip from '@material-ui/core/Tooltip';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 'fit-content',
    display: 'flex',
    alignItems: 'center',
    margin: 0,
  },
  iconButton: {
    margin: theme.spacing('auto', 1),
    padding: 0,
    color: theme.ptas.colors.theme.white,
  },
  icon: {
    fontSize: 26,
  },
  popoverPaper: {
    borderRadius: 0,
    boxShadow: 'unset',
  },
  active: {
    color: theme.ptas.colors.utility.selectionLight,
  },
  zoomLevel: {
    alignItems: 'center',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    width: '90px',
  },
}));

/**
 * A component to display the map zoom level
 *
 * @remarks
 * This components reloads every time the map zoom changes
 */
const ZoomLevel = ({ map }: { map: mapboxgl.Map }): JSX.Element => {
  const classes = useStyles();
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>();
  const { onMenuItemClick, currentZoomLevel } = useZoomLevel(map);

  const removeIcon = useMemo(
    () => (
      <IconButton
        className={classes.iconButton}
        onClick={(): void => {
          const val = map.getZoom() - 1;
          if (val < 0) return;
          map.zoomTo(val);
        }}
      >
        <RemoveCircleOutlineIcon className={classes.icon} />
      </IconButton>
    ),
    [classes.icon, classes.iconButton, map]
  );

  const addIcon = useMemo(
    () => (
      <IconButton
        className={classes.iconButton}
        onClick={(): void => {
          const val = map.getZoom() + 1;
          if (val > 22) return;
          map.zoomTo(val);
        }}
      >
        <AddCircleOutlineIcon className={classes.icon} />
      </IconButton>
    ),
    [classes.icon, classes.iconButton, map]
  );

  const zoomEl = useMemo(
    () => (
      <Tooltip
        title={`1 screen inch : ~ ${Intl.NumberFormat('en-US').format(
          currentZoomLevel
        )} inch on the map.`}
        placement="bottom"
      >
        <IconButton
          onClick={(e): void => setAnchorEl(e.currentTarget)}
          className={classes.iconButton}
        >
          <Typography
            className={
              anchorEl
                ? clsx(classes.active, classes.zoomLevel)
                : classes.zoomLevel
            }
          >
            1:{' ' + Intl.NumberFormat('en-US').format(currentZoomLevel)}
          </Typography>
        </IconButton>
      </Tooltip>
    ),
    [
      anchorEl,
      classes.active,
      classes.iconButton,
      classes.zoomLevel,
      currentZoomLevel,
    ]
  );

  return (
    <Box className={classes.root} key="zoom-level">
      {removeIcon}
      {zoomEl}
      {addIcon}
      {anchorEl && (
        <CustomPopover
          anchorEl={anchorEl}
          onClose={(): void => {
            setAnchorEl(null);
            //TODO set text field input (state - value) to map zoom level
          }}
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'center',
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'center',
          }}
          classes={{ paper: classes.popoverPaper }}
        >
          <ZoomMenu
            onSelected={onMenuItemClick}
            onManualScaleEnter={onMenuItemClick}
          />
        </CustomPopover>
      )}
    </Box>
  );
};

export default ZoomLevel;
