// SelectToolBarOption.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from 'react';
import {
  Box,
  Theme,
  makeStyles,
  useTheme,
  Typography,
} from '@material-ui/core';

interface SelectToolBarOptionProps {
  title: string;
  onClick: () => void;
  isActive: boolean;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    cursor: 'pointer',
    marginRight: theme.spacing(3),
    '&:hover': {
      color: '#d4e693',
    },
  },
  content: {
    display: 'flex',
    '& p': {
      marginLeft: theme.spacing(0.5),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
    },
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    lineHeight: theme.ptas.typography.lineHeight,
  },
  underline: {
    width: '100%',
    height: 4,
    background: 'white',
  },
}));

const SelectToolBarOption = (
  props: PropsWithChildren<SelectToolBarOptionProps>
): JSX.Element => {
  const classes = useStyles(useTheme());
  return (
    <Box className={classes.root} onClick={props.onClick}>
      <Box className={classes.content}>
        {props.children}
        <Typography>{props.title}</Typography>
      </Box>
      {props.isActive && <Box className={classes.underline} />}
    </Box>
  );
};

export default SelectToolBarOption;
