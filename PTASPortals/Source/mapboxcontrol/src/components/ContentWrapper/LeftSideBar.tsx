/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createStyles, makeStyles } from '@material-ui/core';
import IconToolBar from 'components/IconToolBar';
import { useIconToolbar } from 'components/IconToolBar/useIconToolbar';
import { HomeContext } from 'contexts';
import React, { useContext } from 'react';
import SideTree from './SideTree';
import { useHeaderHeight } from './useHeaderHeight';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      display: 'flex',
      zIndex: 5,
      position: 'absolute',
      top: 83,
      backgroundColor: 'white',
    },
  })
);

function LeftSidebar(): JSX.Element {
  const classes = useStyles();
  const { panelHeight, panelContent } = useContext(HomeContext);
  const headerHeight = useHeaderHeight();
  const {
    selectionOnClick,
    rulerOnClick,
    locationOnClick,
    infoOnClick,
  } = useIconToolbar();
  return (
    <div
      className={classes.root}
      style={{
        top: panelContent ? `${panelHeight + headerHeight}px` : headerHeight,
        height: panelContent
          ? `calc(100% - (${panelHeight + headerHeight}px))`
          : `calc(100% - ${headerHeight}px)`,
      }}
    >
      <SideTree />
      <IconToolBar
        infoOnClick={infoOnClick}
        locationOnClick={locationOnClick}
        rulerOnClick={rulerOnClick}
        selectionOnClick={selectionOnClick}
      />
    </div>
  );
}

export default LeftSidebar;
