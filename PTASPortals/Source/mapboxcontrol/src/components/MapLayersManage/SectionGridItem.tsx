/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Box,
  Theme,
  createStyles,
  withStyles,
  WithStyles,
  StyleRules,
} from '@material-ui/core';
import React, { PropsWithChildren } from 'react';

type Props = PropsWithChildren<object> & WithStyles<typeof useStyles>;

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      display: 'grid',
      gridTemplateColumns: '1fr',
      rowGap: theme.spacing(1.5) + 'px',
    },
  });

const SectionGridItem = (props: Props): JSX.Element => {
  const { classes } = props;
  return <Box className={classes.root}>{props.children}</Box>;
};

export default withStyles(useStyles)(SectionGridItem);
