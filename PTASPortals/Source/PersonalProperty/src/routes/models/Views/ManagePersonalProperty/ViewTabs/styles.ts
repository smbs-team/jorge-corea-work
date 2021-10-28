// Styles.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) => ({
  tabs: {
    marginBottom: '40px !important',
  },
  continue: {
    width: 135,
    height: 38,
    marginBottom: 31,
  },
  lastTabButton: {
    width: 227,
  },
}));

export default useStyles;
