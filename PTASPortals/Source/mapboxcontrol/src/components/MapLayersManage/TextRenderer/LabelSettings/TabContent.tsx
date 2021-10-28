// TabContent.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  Box,
  createStyles,
  makeStyles,
  useTheme,
  Theme,
} from '@material-ui/core';

interface Props {
  index: number;
  value: number;
  children: React.ReactNode;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      paddingTop: '24px',
      paddingBottom: '24px',
      borderTop: '1px solid ' + theme.ptas.colors.theme.black,
      marginTop: '10px',
    },
  })
);

const TabContent = (props: Props): JSX.Element => {
  const classes = useStyles(useTheme());
  const { value, index, children } = props;
  return value === index ? (
    <Box className={classes.container}>{children}</Box>
  ) : (
    <Fragment />
  );
};

export default TabContent;
