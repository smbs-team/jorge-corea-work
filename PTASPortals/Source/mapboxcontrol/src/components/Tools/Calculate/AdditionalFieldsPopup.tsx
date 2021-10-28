//AdditionalFieldsPopup.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  Box,
  makeStyles,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Theme,
  useTheme,
} from '@material-ui/core';
import { OverlapCalculatorData } from './useOverlapCalculator';

const useStyles = makeStyles((theme: Theme) => ({
  container: {
    overflowX: 'auto',
  },
  title: {
    textAlign: 'center',
    marginBottom: theme.spacing(2),
    fontWeight: 'bold',
  },
  tableContainer: {
    maxHeight: '363px',
    overflowY: 'auto',
  },
}));

export const AdditionalFieldsPopup = (props: {
  row: OverlapCalculatorData;
}): JSX.Element => {
  const classes = useStyles(useTheme());
  return (
    <Box className={classes.container}>
      <Box className={classes.title}>Parcel: {props.row.parcel}</Box>
      <TableContainer className={classes.tableContainer}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Field</TableCell>
              <TableCell>Value</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Object.entries(props.row.data).map(([key, value]) => (
              <TableRow key={key}>
                <TableCell>{key}</TableCell>
                <TableCell>{value}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
