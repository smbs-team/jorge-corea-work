/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Paper } from '@material-ui/core';
import { Alert } from '@ptas/react-ui-library';
import React, { useEffect } from 'react';
import { FallbackProps } from 'react-error-boundary';
import { appInsights } from 'services/appInsights';

const useStyles = makeStyles(() => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    height: '100%',
  },
  child: {
    display: 'flex',
    justifyContent: 'center',
  },
  alertWrap: {
    width: '300px',
  },
}));

export default function ErrorFallback(props: FallbackProps): JSX.Element {
  const classes = useStyles();

  useEffect(() => {
    appInsights.trackException({
      exception: props.error,
    });
  }, [props.error]);

  return (
    <Paper classes={{ root: classes.root }}>
      <div className={classes.child}>
        <div className={classes.alertWrap}>
          <Alert
            contentText={'Something went wrong'}
            okButtonText={'Refresh'}
            okButtonClick={window.location.reload}
          />
        </div>
      </div>
    </Paper>
  );
}
