/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  StyleRules,
} from '@material-ui/core';
import { Box } from '@material-ui/core';

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  route: string[];
}

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    container: {
      width: 'fit-content',
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    separator: {
      color: theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
      margin: '0px 5px',
    },
    text: {
      fontSize: 16,
      fontWeight: 'normal',
      lineHeight: theme.ptas.typography.lineHeight,
      color: theme.ptas.colors.theme.black,
    },
  });

const TitleRoute = (props: Props): JSX.Element => {
  const { classes } = props;
  const separator = '>';
  return (
    <Box
      display="flex"
      justifyContent="flex-start"
      className={classes.container}
    >
      {props.route.map((value, index) => {
        return !props.route[index + 1] ? (
          <Box key={index} className={classes.text}>
            {value}
            <span
              className={classes.separator}
              style={{ visibility: 'hidden' }}
            >
              &nbsp;{separator}&nbsp;
            </span>
          </Box>
        ) : (
          <Box key={index} className={classes.text}>
            {value}
            <span className={classes.separator}>{separator}</span>
          </Box>
        );
      })}
    </Box>
  );
};

export default withStyles(useStyles)(TitleRoute);
