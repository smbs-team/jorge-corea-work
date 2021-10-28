// PanelBody.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from 'react';
import {
  withStyles,
  WithStyles,
  Theme,
  createStyles,
  Box,
  StyleRules,
} from '@material-ui/core';

type Props = object & WithStyles<typeof useStyles>;

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      paddingTop: theme.spacing(1),
      paddingRight: theme.spacing(4),
    },
  });

const PanelBody = (props: PropsWithChildren<Props>): JSX.Element => {
  return <Box className={props.classes.root}>{props.children}</Box>;
};

export default withStyles(useStyles)(PanelBody);
