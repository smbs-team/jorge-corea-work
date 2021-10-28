/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createStyles, StyleRules, Theme, withStyles } from '@material-ui/core';
import { CustomNestedItem } from '@ptas/react-ui-library';

export default withStyles(
  (theme: Theme): StyleRules =>
    createStyles({
      root: {
        height: 27,
      },
      gutters: {
        padding: theme.spacing(1),
      },
    })
)(CustomNestedItem);
