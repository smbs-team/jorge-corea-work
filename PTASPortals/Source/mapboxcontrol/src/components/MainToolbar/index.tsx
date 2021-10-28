// MainToolbar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from 'react';
import Toolbar from '@material-ui/core/Toolbar';
import { createStyles, makeStyles } from '@material-ui/core';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      backgroundColor: '#001433',
      minHeight: 48,
    },
  })
);

function MainToolbar(props: PropsWithChildren<{}>): JSX.Element {
  const classes = useStyles();
  return <Toolbar className={classes.root}>{props.children}</Toolbar>;
}

export default MainToolbar;
