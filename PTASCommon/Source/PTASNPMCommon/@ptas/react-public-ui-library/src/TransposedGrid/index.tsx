// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import {
  createStyles,
  WithStyles,
  withStyles,
  TableContainer,
  Table,
  TableBody,
  TableRow,
  TableCell,
  TableSortLabel
} from "@material-ui/core";
import { orderBy as _orderBy } from "lodash";
import { KeyboardArrowDown } from "@material-ui/icons";
import clsx from "clsx";
import { Theme } from "@material-ui/core/styles";

export type TransposedGridOrder = "asc" | "desc";

/**
 * Component props
 */
interface Props<T> extends WithStyles<typeof styles> {
  rows: T[];
  columns: { field: string; headerName: string; bold?: boolean }[];
  order: TransposedGridOrder;
  setOrder: React.Dispatch<React.SetStateAction<TransposedGridOrder>>;
  // orderBy: keyof T;
  // setOrderBy: React.Dispatch<React.SetStateAction<keyof T>>;
  orderBy: string;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    fontWeightBold: {
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    },
    tableContainer: {},
    table: {},
    tableBody: {},
    tableRow: {},
    tableHeaderCell: {
      borderBottom: "none",
      textAlign: "right",
      padding: theme.spacing(1, 2),
      fontSize: theme.ptas.typography.bodyExtraBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight
    },
    tableCell: {
      borderBottom: "none",
      padding: theme.spacing(1, 2),
      fontSize: theme.ptas.typography.body.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight
    }
  });

/**
 * TransposedGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function TransposedGrid<T>(props: Props<T>): JSX.Element {
  return (
    <TableContainer className={props.classes.tableContainer}>
      <Table className={props.classes.table} aria-label='table'>
        <TableBody
          component='tbody'
          classes={{ root: props.classes.tableBody }}
        >
          {props.columns.map((column) => (
            <TableRow
              component='tr'
              key={column.field}
              classes={{ root: props.classes.tableRow }}
            >
              {/* Header cell */}
              <TableCell
                key={column.field}
                classes={{ root: props.classes.tableHeaderCell }}
                component='th'
                scope='row'
              >
                <TableSortLabel
                  active={props.orderBy === column.field}
                  direction={
                    props.orderBy === column.field ? props.order : "asc"
                  }
                  onClick={(): void => {
                    const ascending =
                      props.orderBy === column.field && props.order === "asc";
                    props.setOrder(ascending ? "desc" : "asc");
                    // props.setOrderBy(column.field as keyof T);
                    props.setOrderBy(column.field.toString());
                  }}
                  IconComponent={KeyboardArrowDown}
                >
                  {column.headerName}
                </TableSortLabel>
              </TableCell>

              {/* Content cells */}
              {_orderBy(props.rows, props.orderBy, props.order).map(
                (row, i) => (
                  <TableCell
                    key={column.field + i}
                    classes={{
                      root: clsx(
                        props.classes.tableCell,
                        column["bold"] || row["bold"]
                          ? props.classes.fontWeightBold
                          : ""
                      )
                    }}
                    scope='row'
                  >
                    {row[column.field]}
                  </TableCell>
                )
              )}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default withStyles(styles)(TransposedGrid);
