/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Paper } from '@material-ui/core';
import { Alert } from '@ptas/react-public-ui-library';
import React, { useEffect } from 'react';
import { FallbackProps } from 'react-error-boundary';
import { useHistory } from 'react-router';
import { appInsights } from '../appInsights';

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
  const history = useHistory();

  useEffect(() => {
    appInsights.trackException({
      exception: props.error,
    });
  }, [props.error]);

  const refresh = (): void => {
    history.go(0);
  };

  return (
    <Paper classes={{ root: classes.root }}>
      <div className={classes.child}>
        <div className={classes.alertWrap}>
          <Alert
            contentText={'Something went wrong'}
            okButtonText={'Retry'}
            okShowButton
            okButtonClick={refresh}
          />
        </div>
      </div>
    </Paper>
  );
}
