// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import Backdrop from '@material-ui/core/Backdrop';
import CircularProgress from '@material-ui/core/CircularProgress';
import {
  makeStyles,
  createStyles,
  Theme,
  useTheme,
} from '@material-ui/core/styles';

/**
 * material ui isolated styles
 * @param theme -Material ul theme
 */
const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    backdrop: {
      zIndex: theme.zIndex.drawer + 1,
      color: '#fff',
    },
  })
);

/**
 * A backdrop loader to be displayed in a desired process progress
 */
export const BackdropLoader = (): JSX.Element => {
  const theme = useTheme();
  const classes = useStyles(theme);
  return (
    <>
      <Backdrop className={classes.backdrop} open={true}>
        <CircularProgress color="inherit" />
      </Backdrop>
    </>
  );
};
