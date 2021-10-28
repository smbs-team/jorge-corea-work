// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React from 'react';
import {
  Theme,
  createStyles,
  makeStyles,
  useTheme,
  Box,
} from '@material-ui/core';
import { PanelBody as PtasPanelBody } from 'components/common/panel';
import { PropsWithChildren } from 'react';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    panelBodyRoot: {
      '& > *': {
        margin: theme.spacing(1),
      },
    },
    mainRow: {
      display: 'flex',
      flexWrap: 'wrap',
      '& > :nth-child(1)': {
        flexBasis: '50%',
        marginRight: 20,
      },
    },
  })
);

const PanelBody = (props: PropsWithChildren<object>): JSX.Element => {
  const classes = useStyles(useTheme());
  return (
    <PtasPanelBody classes={{ root: classes.panelBodyRoot }}>
      {props.children}
    </PtasPanelBody>
  );
};

export const PanelBodyMainRow = (
  props: PropsWithChildren<object>
): JSX.Element => {
  const classes = useStyles(useTheme());
  return <Box className={classes.mainRow}>{props.children}</Box>;
};

export default PanelBody;
