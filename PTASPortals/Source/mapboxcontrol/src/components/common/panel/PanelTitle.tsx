// PanelTitle.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Box,
  Typography,
  Divider,
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  StyleRules,
} from '@material-ui/core';
import React, { PropsWithChildren } from 'react';

type Props = {
  topLine?: boolean;
  bottomLine?: boolean;
} & WithStyles<typeof useStyles>;

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {},
    titleText: {
      color: theme.ptas.colors.theme.black,
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      fontWeight: 'bold',
      lineHeight: theme.ptas.typography.lineHeight,
      display: 'flex',
    },
    titleDivider: {
      marginBottom: theme.spacing(0.5),
      marginTop: theme.spacing(1),
    },
  });

const PanelTitle = (props: PropsWithChildren<Props>): JSX.Element => {
  const { classes } = props;
  const topLine = props.topLine !== undefined ? props.topLine : true;
  const bottomLine = props.bottomLine !== undefined ? props.bottomLine : true;
  return (
    <Box className={classes.root}>
      {topLine && <Divider className={classes.titleDivider} />}
      <Typography className={classes.titleText} variant="h6">
        {props.children}
      </Typography>
      {bottomLine && <Divider className={classes.titleDivider} />}
    </Box>
  );
};

export default withStyles(useStyles)(PanelTitle);
