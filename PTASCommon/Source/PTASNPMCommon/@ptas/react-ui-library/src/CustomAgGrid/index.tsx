// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from "react";
import {
  AgGridColumnGroupProps,
  AgGridColumnProps,
  AgGridReact
} from "ag-grid-react";
import {
  ColumnApi,
  GridApi,
  GridOptions,
  GridReadyEvent,
  RowSelectedEvent
} from "ag-grid-community";
import "./styles.scss";

/**
 * Component props
 */
interface Props<T> {
  rows: T[];
  filterBy?: string;
  gridOptions?: GridOptions;
  onRowSelected?: (rows: T[]) => void;
}

export type ColumnProps = AgGridColumnProps | AgGridColumnGroupProps;

/**
 * CustomAgGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomAgGrid<T>(props: Props<T>): JSX.Element {
  const [gridApi, setGridApi] = useState<GridApi | null>(null);
  const [, setGridColumnApi] = useState<ColumnApi | null>(null);

  function onGridReady(params: GridReadyEvent) {
    setGridApi(params.api);
    setGridColumnApi(params.columnApi);
    params.columnApi.autoSizeAllColumns(true);
    params.api.sizeColumnsToFit();
  }

  const handleRowSelection = (_event: RowSelectedEvent) => {
    const nodes = gridApi?.getSelectedNodes();
    if (!nodes) return;
    props.onRowSelected && props.onRowSelected(nodes.flatMap((n) => n.data));
  };

  useEffect(() => {
    setSelectedItems();
  }, [gridApi]);

  const setSelectedItems = () => {
    if (!gridApi) return;
    gridApi.forEachNodeAfterFilterAndSort((rowNode) => {
      var data = rowNode.data;
      if (data.isSelected) rowNode.setSelected(true);
    });
  };

  return (
    <div className='ag-theme-alpine' style={{ height: 280, width: "100%" }}>
      <AgGridReact
        onGridReady={onGridReady}
        rowData={props.rows}
        rowSelection='multiple'
        onRowSelected={handleRowSelection}
        suppressCellSelection
        quickFilterText={props.filterBy}
        headerHeight={30}
        gridOptions={props.gridOptions}
      />
    </div>
  );
}

export default CustomAgGrid;
