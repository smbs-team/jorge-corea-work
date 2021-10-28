// ParcelDataTable.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from 'react';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { orderBy as _orderBy } from 'lodash';
import { HeadCellData, Order } from './useParcelNavigation';
import { TableSortLabel, Theme } from '@material-ui/core';
import { KeyboardArrowDown } from '@material-ui/icons';
import {
  WalkingDistanceRowData,
  DrivingTimeRowData,
} from 'services/map/model/directions';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    tableContainer: {
      paddingRight: theme.spacing(4),
    },
    table: {
      minWidth: 650,
    },
    tableHeadRow: {},
    tableHeadCell: {
      borderColor: theme.ptas.colors.theme.grayMedium,
      padding: theme.spacing(1 / 2, 3, 1 / 2, 0),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
    },
    tableBody: {
      // overflowY: 'auto',
    },
    tableRow: {
      cursor: 'pointer',
      userSelect: 'none',
    },
    tableCell: {
      borderBottom: 'none',
      padding: theme.spacing(1, 3, 0, 0),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
    },
  })
);

export default function ParcelDataTable<
  T extends WalkingDistanceRowData | DrivingTimeRowData
>(props: {
  // parcelRows: T[];
  walkingDistanceParcelRows?: WalkingDistanceRowData[];
  drivingTimeParcelRows?: DrivingTimeRowData[];
  headCells: HeadCellData[];
  order: Order;
  setOrder: React.Dispatch<React.SetStateAction<Order>>;
  orderBy: keyof T;
  setOrderBy: React.Dispatch<React.SetStateAction<keyof T>>;
  numberWithCommas: (x: number) => string;
  clickOnDirectionRow: (
    row: WalkingDistanceRowData | DrivingTimeRowData,
    onlyRemoveRoute?: boolean
  ) => void;
}): JSX.Element {
  const classes = useStyles();
  const {
    walkingDistanceParcelRows,
    drivingTimeParcelRows,
    headCells,
    order,
    setOrder,
    orderBy,
    setOrderBy,
    numberWithCommas,
    clickOnDirectionRow,
  } = props;

  const [selectedRow, setSelectedRow] = useState<number>();

  return (
    <TableContainer className={classes.tableContainer}>
      <Table className={classes.table} aria-label="simple table">
        <TableHead>
          <TableRow classes={{ root: classes.tableHeadRow }}>
            {headCells &&
              headCells.map((cell, index) => (
                <TableCell
                  key={cell.id + index}
                  sortDirection={orderBy === cell.id ? order : false}
                  classes={{ root: classes.tableHeadCell }}
                >
                  <TableSortLabel
                    active={orderBy === cell.id}
                    direction={orderBy === cell.id ? order : 'asc'}
                    onClick={(): void => {
                      const ascending = orderBy === cell.id && order === 'asc';
                      setOrder(ascending ? 'desc' : 'asc');
                      setOrderBy(cell.id as keyof T);
                    }}
                    IconComponent={KeyboardArrowDown}
                  >
                    {cell.label}
                  </TableSortLabel>
                </TableCell>
              ))}
          </TableRow>
        </TableHead>
        <TableBody classes={{ root: classes.tableBody }}>
          {drivingTimeParcelRows &&
            _orderBy(drivingTimeParcelRows, orderBy, order).map((row, i) => (
              <TableRow
                key={row.parcel}
                classes={{ root: classes.tableRow }}
                onClick={(): void => {
                  if (selectedRow === i) {
                    clickOnDirectionRow(row, true);
                    setSelectedRow(-1);
                  } else {
                    clickOnDirectionRow(row);
                    setSelectedRow(i);
                  }
                }}
                style={{ backgroundColor: i === selectedRow ? '#d4e693' : '' }}
              >
                <TableCell
                  classes={{ root: classes.tableCell }}
                  component="th"
                  scope="row"
                >
                  {row.parcel}
                </TableCell>
                <TableCell classes={{ root: classes.tableCell }} align="center">
                  {row.time ? numberWithCommas(row.time) + ' min' : ''}
                </TableCell>
                <TableCell classes={{ root: classes.tableCell }}>
                  {row.address}
                </TableCell>
              </TableRow>
            ))}

          {walkingDistanceParcelRows &&
            _orderBy(walkingDistanceParcelRows, orderBy, order).map(
              (row, i) => (
                <TableRow
                  key={row.parcel}
                  classes={{ root: classes.tableRow }}
                  onClick={(): void => {
                    clickOnDirectionRow(row);
                    if (selectedRow === i) {
                      clickOnDirectionRow(row, true);
                      setSelectedRow(-1);
                    } else {
                      clickOnDirectionRow(row);
                      setSelectedRow(i);
                    }
                  }}
                  style={{
                    backgroundColor: i === selectedRow ? '#d4e693' : '',
                  }}
                >
                  <TableCell
                    classes={{ root: classes.tableCell }}
                    component="th"
                    scope="row"
                  >
                    {row.parcel}
                  </TableCell>
                  <TableCell
                    classes={{ root: classes.tableCell }}
                    align="center"
                  >
                    {row.distance ? numberWithCommas(row.distance) + ' ft' : ''}
                  </TableCell>
                  <TableCell classes={{ root: classes.tableCell }}>
                    {row.address}
                  </TableCell>
                </TableRow>
              )
            )}

          {/* Use this to test style with many rows on the table */}
          {/* {Array.from(Array(40).keys()).map((index) => (
            <TableRow key={index}>
              <TableCell classes={{root: classes.tableCell}} component="th" scope="row">
                {'parcel' + index}
              </TableCell>
              <TableCell classes={{root: classes.tableCell}} align="center">{'distance' + index}</TableCell>
              <TableCell classes={{root: classes.tableCell}}>{'address address address address address address address address ' + index}</TableCell>
            </TableRow>
          ))} */}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
