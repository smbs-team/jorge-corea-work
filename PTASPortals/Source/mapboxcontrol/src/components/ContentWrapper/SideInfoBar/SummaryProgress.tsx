/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CircularProgress, makeStyles } from '@material-ui/core';
import React from 'react';

const useStyles = makeStyles(() => ({
  root: {
    height: '100%',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  },
}));

function SummaryProgress(): JSX.Element {
  const classes = useStyles();
  return (
    <div className={classes.root}>
      <CircularProgress color="inherit" />
    </div>
  );
}

export default SummaryProgress;
