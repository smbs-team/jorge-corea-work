/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from 'react';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles(() => ({
  root: {
    position: 'relative',
    top: '48px',
  },
}));

function PanelContentRoot({
  children,
}: PropsWithChildren<unknown>): JSX.Element {
  const classes = useStyles();
  return <div className={classes.root}>{children}</div>;
}

export default PanelContentRoot;
