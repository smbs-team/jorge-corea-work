// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, lazy, Suspense, useContext } from 'react';
import {
  AppBar as MUIAppBar,
  useTheme,
  makeStyles,
  createStyles,
  LinearProgress,
  Switch,
  Theme,
} from '@material-ui/core';
import MainToolbar from 'components/MainToolbar';
import AppBarMenu from 'components/AppBarMenu';
import SearchForm from 'components/SearchForm';
import SecondaryMenu from 'components/SecondaryMenu';
import { HomeContext } from 'contexts';
import { withMap } from 'hoc/withMap';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';

const ZoomLevel = lazy(() => import('components/ZoomLevel'));
const DrawToolBar = lazy(() => import('components/DrawToolBar'));
const ParcelPagination = lazy(() => import('components/ParcelPagination'));

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      minHeight: '48px',
      background: 'none',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    },
    zoom: {
      marginLeft: 'auto',
      marginRight: 'auto',
    },
    switchChecked: {
      color: theme.ptas.colors.utility.selection,
    },
    switchTrack: {
      backgroundColor: theme.ptas.colors.utility.selection,
    },
  })
);

/**
 *
 * Material app bar with some customization
 *
 * @param props -Component props
 * @returns -Material AppBar component
 */
const AppBar = withMap(({ map }): JSX.Element | null => {
  const theme = useTheme();
  const classes = useStyles(theme);
  const {
    mapThumbnail,
    setMapThumbnail,
    panelContent,
    linearProgress,
  } = useContext(HomeContext);
  const { actionMode } = useContext(DrawToolBarContext);

  return (
    <MUIAppBar
      position="fixed"
      classes={{
        root: classes.root,
      }}
      elevation={0}
    >
      <MainToolbar>
        <AppBarMenu />
        <div className={classes.zoom}>
          <Suspense fallback={<Fragment></Fragment>}>
            <ZoomLevel map={map} />
          </Suspense>
        </div>
        <div>
          <Suspense fallback={null}>
            <ParcelPagination />
          </Suspense>
        </div>
        <span>Thumbnail</span>
        <Switch
          onChange={(): void => {
            setMapThumbnail((prev) => !prev);
          }}
          classes={{
            checked: classes.switchChecked,
            track: classes.switchTrack,
          }}
          color="primary"
          checked={mapThumbnail}
        />
        <SearchForm />
        <SecondaryMenu />
      </MainToolbar>
      <Suspense fallback={<Fragment></Fragment>}>
        <Suspense fallback={null}>
          {!panelContent && ['draw', 'measure'].includes(actionMode ?? '') && (
            <DrawToolBar />
          )}
        </Suspense>
      </Suspense>
      {linearProgress && <LinearProgress />}
    </MUIAppBar>
  );
});

export default AppBar;
