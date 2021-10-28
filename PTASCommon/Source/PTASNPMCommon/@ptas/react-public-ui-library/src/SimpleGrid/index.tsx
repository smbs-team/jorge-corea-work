// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from "react";
import { AgGridReact, AgGridColumn } from "ag-grid-react";
import {
  // CellClassParams,
  ColumnApi,
  GridApi,
  RowClassParams
} from "ag-grid-community";
import "ag-grid-community/dist/styles/ag-grid.css";
import { createStyles, WithStyles, withStyles } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";

type GridColumn = { field: string; headerName: string; highlight?: boolean };

/**
 * Component props
 */
interface Props<T> extends WithStyles<typeof styles> {
  rows: T[];
  columns: GridColumn[];
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    fontWeightBold: {
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    },
    highlight: {
      color: theme.ptas.colors.theme.accent
    },
    simpleGrid: {
      height: "100%",
      fontSize: theme.ptas.typography.body.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
      "& .ag-header-row": {
        fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
        // paddingBottom: theme.spacing(2)
      },
      // "& .ag-row": {
      //   paddingBottom: theme.spacing(2)
      // },
      //Remove border when header cell is selected
      "& .ag-header-row > div": {
        border: "none !important",
        outline: "none"
      },
      //Remove border when cell is selected
      "& .ag-row-focus > div": {
        border: "none !important",
        outline: "none"
      },
      "& .ag-header-cell": {
        padding: theme.spacing(0, 2)
      },
      "& .ag-cell": {
        padding: theme.spacing(0, 2)
      }
    }
  });

/**
 * SimpleGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SimpleGrid<T>(props: Props<T>): JSX.Element {
  const [, setGridApi] = useState<GridApi | null>(null);
  const [gridColumnApi, setGridColumnApi] = useState<ColumnApi | null>(null);

  const onGridReady = (params: {
    api: React.SetStateAction<GridApi | null>;
    columnApi: React.SetStateAction<ColumnApi | null>;
  }) => {
    setGridApi(params.api);
    setGridColumnApi(params.columnApi);
  };

  useEffect(() => {
    if (gridColumnApi) {
      gridColumnApi.autoSizeAllColumns();
    }
  }, [gridColumnApi]);

  const getRowClass = (params: RowClassParams): string | string[] => {
    if (params.data.bold) {
      return props.classes.fontWeightBold;
    }
    return "";
  };

  // const getCellClass = (params: CellClassParams): string | string[] => {
  //   console.log('params:', params);
  //   if (params.data.highlight) {
  //     return props.classes.highlight;
  //   }
  //   return "";
  // };
  const getCellClass = (column: GridColumn): string | string[] => {
    if (column.highlight) {
      return props.classes.highlight;
    }
    return "";
  };

  return (
    <div className={props.classes.simpleGrid}>
      <AgGridReact
        onGridReady={onGridReady}
        rowData={props.rows}
        getRowClass={getRowClass}
        gridOptions={{
          suppressCellSelection: true
        }}
      >
        {props.columns.map((column, index) => (
          <AgGridColumn
            key={index}
            field={column.field}
            headerName={column.headerName}
            // cellClass={getCellClass}
            cellClass={() => getCellClass(column)}
            sortable
          ></AgGridColumn>
        ))}
      </AgGridReact>
    </div>
  );
}

export default withStyles(styles)(SimpleGrid);