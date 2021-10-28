// AssetsTable.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { FC, PropsWithChildren } from 'react';
import {
  createStyles,
  StyleRules,
  Theme,
  withStyles,
  WithStyles,
} from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import { GenericWithStyles } from '@ptas/react-public-ui-library';

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    table: {
      padding: 0,
      borderCollapse: 'separate',
      fontSize: 14,
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    body: {
      display: 'table',
      tableLayout: 'fixed',
      width: 789,
      borderSpacing: '0 5px',
      margin: '0 auto',
    },
  });

type Props = WithStyles<typeof useStyles>;

const AssetsTable = (props: PropsWithChildren<Props>): JSX.Element => {
  const { children, classes } = props;

  return (
    <Table className={classes.table} aria-label="caption table">
      <TableBody className={classes.body}>{children}</TableBody>
    </Table>
  );
};

export default withStyles(useStyles)(AssetsTable) as FC<
  GenericWithStyles<Props>
>;
