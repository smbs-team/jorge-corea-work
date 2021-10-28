// CustomTableRow.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { FC, PropsWithChildren } from 'react';
import TableRow from '@material-ui/core/TableRow';
import {
  createStyles,
  StyleRules,
  withStyles,
  WithStyles,
} from '@material-ui/core/styles';
import { GenericWithStyles } from '@ptas/react-public-ui-library';

const useStyles = (): StyleRules =>
  createStyles({
    row: {
      background: 'transparent',
      boxShadow: '0px 2px 12px rgba(0, 0, 0, 0.25)',
      borderRadius: 9,
    },
  });

type Props = WithStyles<typeof useStyles>;

const CustomTableRow = (props: PropsWithChildren<Props>): JSX.Element => {
  const { classes, children } = props;

  return <TableRow className={classes.row}>{children}</TableRow>;
};

export default withStyles(useStyles)(CustomTableRow) as FC<
  GenericWithStyles<Props>
>;
