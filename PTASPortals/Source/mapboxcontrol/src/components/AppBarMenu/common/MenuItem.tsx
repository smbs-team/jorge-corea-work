/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  createStyles,
  MenuItem as MuiMenuItem,
  StyleRules,
  Theme,
  withStyles,
} from '@material-ui/core';

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      fontSize: '0.875rem',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
      height: 27,
    },
    gutters: {
      padding: theme.spacing(1),
    },
  });

export default withStyles(useStyles)(MuiMenuItem);
