/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, createStyles } from '@material-ui/core';

export const useGlobalStyles = makeStyles(() =>
  createStyles({
    basicInput: { borderRadius: '3px !important' },
    textCenter: {
      textAlign: 'center',
    },
    flexboxRow: {
      display: 'flex',
    },
    flexboxColumn: {
      display: 'flex',
      flexDirection: 'column',
    },
  })
);
