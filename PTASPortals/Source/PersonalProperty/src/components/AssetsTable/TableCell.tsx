// CustomTableCell.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { FC, PropsWithChildren } from 'react';
import {
  createStyles,
  StyleRules,
  WithStyles,
  withStyles,
} from '@material-ui/core/styles';
import TableCell from '@material-ui/core/TableCell';
import { GenericWithStyles } from '@ptas/react-public-ui-library';

const useStyles = (): StyleRules =>
  createStyles({
    td: {
      padding: '4px 8px',
    },
  });

type Props = WithStyles<typeof useStyles>;

const CustomTableCell = (props: PropsWithChildren<Props>): JSX.Element => {
  const { classes } = props;

  const { children } = props;

  return <TableCell className={classes.td}>{children}</TableCell>;
};

export default withStyles(useStyles)(CustomTableCell) as FC<
  GenericWithStyles<Props>
>;
