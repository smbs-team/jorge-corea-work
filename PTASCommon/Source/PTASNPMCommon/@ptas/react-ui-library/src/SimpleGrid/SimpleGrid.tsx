// SimpleGrid.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useEffect } from "react";
import { withStyles, WithStyles, createStyles } from "@material-ui/core";

import Tooltip from "@material-ui/core/Tooltip";
import {
  Grid,
  Table,
  TableHeaderRow,
  TableFilterRow,
  TableSelection,
  TableColumnResizing,
  VirtualTable
} from "@devexpress/dx-react-grid-material-ui";

import {
  SortingState,
  IntegratedSorting,
  FilteringState,
  IntegratedFiltering,
  SelectionState,
  DataTypeProvider,
  Column,
  IntegratedSelection
} from "@devexpress/dx-react-grid";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  columns: Column[];
  rows: object[];
  columnResizeWidths?: ColumnResize[];
  onSelected: (index: number) => void;
  height: number;
}

interface ColumnResize {
  columnName: string;
  width: number;
}

/**
 * Component styles
 */
const useStyles = () =>
  createStyles({
    head: {
      backgroundColor: "#c0c0c0",
      fontWeight: "bolder"
    },
    filter: {
      backgroundColor: "white"
    },
    root: {
      "& thead tr th": {
        fontFamily: "Helvetica",
        fontWeight: "bolder",
        fontSize: "14px"
      },
      "& tbody tr td": {
        fontFamily: "Helvetica"
      }
    }
  });

/**
 * SimpleGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SimpleGrid(props: Props): JSX.Element {
  const { classes, columns, columnResizeWidths, onSelected } = props;

  const [sorting, setSorting] = useState<any>();
  const [selection, setSelection] = useState<any>([]);
  const [columnWidths, setColumnWidths] = useState<any[]>([]);
  useEffect(() => {
    if (columnResizeWidths) {
      setColumnWidths(columnResizeWidths);
    } else {
      columns.forEach((c) => {
        setColumnWidths((s) => [...s, { columnName: c.name, width: 180 }]);
      });
    }
  }, []);

  useEffect(() => {
    onSelected(selection[0]);
  }, [selection, onSelected]);

  const handleOnSelect = (index: any) => {
    const lastSelected = index.find(
      (selected: any) => selection.indexOf(selected) === -1
    );

    if (lastSelected !== undefined) {
      setSelection([lastSelected]);
    } else {
      setSelection([-1]);
    }
  };

  const headComponent = ({ ...restProps }: any) => (
    <Table.TableHead {...restProps} className={classes.head} />
  );

  const filterComponent = ({ ...restProps }: any) => (
    <Table.Row {...restProps} className={classes.filter} />
  );

  const tableComponent = ({ ...restProps }: any) => (
    <Table.Table {...restProps} className={classes.root} />
  );

  const TooltipFormatter = ({ value }: any) => (
    <Tooltip title={<span>{value}</span>}>
      <span>{value}</span>
    </Tooltip>
  );

  const CellTooltip = (props: any) => (
    <DataTypeProvider
      for={columns.map(({ name }) => name)}
      formatterComponent={TooltipFormatter}
      {...props}
    />
  );

  return (
    <Fragment>
      <Grid rows={props.rows} columns={columns}>
        <SelectionState
          selection={selection}
          onSelectionChange={handleOnSelect}
        />
        <IntegratedSelection />
        <SortingState sorting={sorting} onSortingChange={setSorting} />
        <IntegratedSorting />
        <FilteringState defaultFilters={[]} />
        <IntegratedFiltering />
        <CellTooltip />
        <VirtualTable
          height={props.height}
          headComponent={headComponent}
          tableComponent={tableComponent}
        />
        <TableColumnResizing
          columnWidths={columnWidths}
          onColumnWidthsChange={setColumnWidths}
        />
        <TableHeaderRow showSortingControls />
        <TableFilterRow rowComponent={filterComponent} />
        <TableSelection
          selectByRowClick
          highlightRow
          showSelectionColumn={false}
        />
      </Grid>
    </Fragment>
  );
}

export default withStyles(useStyles)(SimpleGrid);
