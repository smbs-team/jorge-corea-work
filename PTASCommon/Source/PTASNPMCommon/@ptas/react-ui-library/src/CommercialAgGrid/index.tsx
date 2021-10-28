import React, {
  forwardRef,
  useImperativeHandle,
  useState,
  ForwardedRef,
  CSSProperties
} from "react";
import {
  CellValueChangedEvent,
  ColDef,
  ColGroupDef,
  ColumnApi,
  GridApi,
  GridOptions,
  GridReadyEvent,
  ICellRendererParams
} from "ag-grid-community";
import {
  ButtonBase,
  createStyles,
  withStyles,
  WithStyles
} from "@material-ui/core";
import { AgGridReact } from "ag-grid-react";
import "./styles.scss";
import { GenericWithStyles } from "../common";
import { omit, uniqueId } from "lodash";
import CloseIcon from "@material-ui/icons/Close";
import { useUpdateEffect } from "react-use";
import { useEffect } from "react";
import NumericCellEditor from "./NumericCellEditor";
import ErrorIcon from "@material-ui/icons/Error";
import ErrorBuilder from "./ErrorBuilder";

interface Props<T> extends WithStyles<typeof styles> {
  columnDefs: (ColDef | ColGroupDef)[];
  errors?: InvalidRow[];
  editable?: boolean;
  rowData: T[];
  gridOptions?: Omit<GridOptions, "rowData" | "columnDefs">;
  showRemove?: boolean;
  showRowNumber?: boolean;
  rowNumberOptions?: ColDef;
  headerHeight?: number;
  style?: CSSProperties;
  idPrefix?: string;
  onRemove?: (row: T) => void;
  onChange?: (
    rows: CustomRowData<T>[],
    eventType: OnChangeEventTypes,
    cellChangeParams?: CellValueChangedEvent
  ) => void;
}

export type OnChangeEventTypes =
  | "deleteRow"
  | "onSortChanged"
  | "onCellValueChanged"
  | "onDragStopped";

export interface CustomRowData<T> {
  id?: string;
  rowIndex: number | null;
  data: T;
}

export interface InvalidRow {
  index: number | null;
  errorMessage: string;
}

export interface CommAgGridElement<T> {
  isLoading: (state: boolean) => void;
  addRow: (row: T) => void;
  getAllRows: () => CustomRowData<T>[];
  updateRows: (updatedRows: T[]) => void;
  updateRow: (id: string, newData: T) => void;
  updateCell: (id: string, colField: string, newValue: string) => void;
  flashRow: (index: string) => void;
  refreshCells: () => void;
}

export type CommercialAgGridProps<T> = GenericWithStyles<Props<T>>;

const styles = () =>
  createStyles({
    removeButton: {
      color: "#aaaaaa",
      "&:hover": {
        color: "#ff3823"
      }
    }
  });

function CommercialAgGrid<T>(
  props: CommercialAgGridProps<T>,
  ref: ForwardedRef<CommAgGridElement<T>>
) {
  const { classes, onRemove } = props;

  const [gridApi, setGridApi] = useState<GridApi | null>(null);
  const [, setGridColumnApi] = useState<ColumnApi | null>(null);

  const rowNumberColumn: ColDef = {
    headerName: "",
    field: "rowNumber",
    valueGetter: "node.rowIndex + 1",
    rowDrag: true,
    width: 140,
    sortable: false,
    resizable: false,
    editable: false,
    cellClass: "row-number",
    hide: !props.showRowNumber ?? true,
    ...props.rowNumberOptions
  };

  const removeColumn: ColDef = {
    field: "remove",
    headerName: "",
    headerClass: "remove-header",
    cellClass: "remove-cell",
    cellRenderer: "removeRenderer",
    sortable: false,
    resizable: false,
    width: 40,
    suppressSizeToFit: true,
    editable: false,
    hide: !props.showRemove ?? true,
    cellStyle: { backgroundColor: "#fafafa" }
  };

  useImperativeHandle(ref, () => ({
    isLoading(state: boolean) {
      if (state) {
        gridApi?.showLoadingOverlay();
      } else {
        gridApi?.hideOverlay();
      }
    },
    addRow(row: T) {
      if (!gridApi) return;
      const columnName = (props.columnDefs[0] as ColDef).field;
      const index = gridApi.getDisplayedRowCount();
      gridApi.applyTransaction({
        add: [{ id: uniqueId(props.idPrefix), ...(row as T) }]
      });
      if (!columnName) return;
      gridApi.setFocusedCell(index, columnName);
      gridApi.startEditingCell({
        rowIndex: index,
        colKey: columnName
      });
      gridApi.sizeColumnsToFit();
    },
    getAllRows() {
      if (!gridApi) return [];
      const rows: CustomRowData<T>[] = [];
      gridApi.forEachNode((n) =>
        rows.push({ rowIndex: n.rowIndex, data: n.data })
      );
      return rows;
    },
    updateRows(rows: T[]) {
      if (!gridApi) return;
      gridApi.applyTransaction({ update: rows });
    },
    updateRow(id: string, newData: T) {
      if (!gridApi) return;
      const rowNode = gridApi.getRowNode(id);
      if (rowNode) {
        rowNode.setData(newData);
      }
    },
    updateCell(id: string, colField: string, newValue: string) {
      if (!gridApi) return;
      const rowNode = gridApi.getRowNode(id);
      if (rowNode) {
        rowNode.setDataValue(colField, newValue);
      }
    },
    flashRow(index) {
      if (!gridApi) return;
      const rowNode = gridApi.getDisplayedRowAtIndex(parseInt(index));
      if (rowNode) {
        gridApi.flashCells({ rowNodes: [rowNode] });
      }
    },
    refreshCells() {
      if (!gridApi) return;
      gridApi.refreshCells({
        force: true
      });
    }
  }));

  useUpdateEffect(() => {
    if (!props.errors) return;
    gridApi?.refreshCells({ force: true });
  }, [props.errors]);

  useEffect(() => {
    if (!gridApi) return;
    const columns = gridApi.getColumnDefs();
    if (props.editable) {
      const editableColumns = (columns as ColDef[]).map((c) => {
        return {
          ...c,
          editable:
            c.field === "empty" || c.field === "rowNumber" ? false : true
        };
      });
      gridApi.setColumnDefs([rowNumberColumn, ...editableColumns]);
    } else {
      const editableColumns = (columns as ColDef[]).map((c) => {
        return { ...c, editable: false };
      });
      gridApi.setColumnDefs(editableColumns);
    }
  }, [props.editable]);

  //Aggrid does not export an interface for the 'frameworkComponent' params.
  const RemoveRenderer = (params: any): JSX.Element => {
    return (
      <ButtonBase
        className={classes?.removeButton}
        onClick={() => {
          onRemove && onRemove(params.node.data as T);
          params.api.applyTransaction({ remove: [params.node.data] });
          props.onChange && props.onChange(getAllRows(params), "deleteRow");
        }}
      >
        {(params.valueFormatted ? params.valueFormatted : params.value) ?? (
          <CloseIcon />
        )}
      </ButtonBase>
    );
  };

  const ErrorCellRenderer = (params: ICellRendererParams) => {
    const errors = params.context.errors as InvalidRow[];
    let error: InvalidRow | undefined = undefined;
    if (errors) {
      error = errors.find((e) => e.index === params.rowIndex);
    }

    return (
      <div>
        {error && (
          <div style={{ position: "absolute", left: 24 }}>
            <ErrorIcon
              titleAccess={error && error.errorMessage}
              style={{ color: "red" }}
            />
          </div>
        )}
        <span
          className={"ag-cell-value"}
          style={{ marginLeft: error ? 18 : undefined }}
        >
          {params.value}
        </span>
      </div>
    );
  };

  const options: GridOptions = {
    defaultColDef: {
      sortable: true,
      resizable: true,
      headerClass: "cell-header",
      cellEditor: "numericEditor",
      suppressMovable: true,
      enableCellChangeFlash: true,
      cellEditorParams: {
        decimalScale: 2,
        decimalSeparator: ".",
        thousandsGroupStyle: "thousand",
        thousandSeparator: true,
        ...props.gridOptions?.defaultColDef?.cellEditorParams
      },
      ...omit(props.gridOptions?.defaultColDef, "cellEditorParams")
    },
    rowDragManaged: true,
    animateRows: true,
    frameworkComponents: {
      removeRenderer: RemoveRenderer,
      numericEditor: NumericCellEditor,
      errorCellRenderer: ErrorCellRenderer,
      ...props.gridOptions?.frameworkComponents
    },
    components: {},
    columnDefs: [rowNumberColumn, ...props.columnDefs, removeColumn],
    rowMultiSelectWithClick: true,
    onSortChanged: (params) => {
      if (props.showRowNumber) {
        params.api.refreshCells();
      }
      props.onChange && props.onChange(getAllRows(params), "onSortChanged");
    },
    onCellValueChanged: (params) => {
      params.api.refreshCells();
      props.onChange &&
        props.onChange(getAllRows(params), "onCellValueChanged", params);
    },
    onDragStopped: (params) => {
      if (props.showRowNumber) {
        params.api.refreshCells();
      }
      props.onChange && props.onChange(getAllRows(params), "onDragStopped");
    },
    ...omit(props.gridOptions, ["defaultColDef", "frameworkComponents"])
  };

  const onGridReady = (params: GridReadyEvent) => {
    setGridApi(params.api);
    setGridColumnApi(params.columnApi);
    params.api.sizeColumnsToFit();
  };

  useUpdateEffect(() => {
    if (!gridApi) return;
    const columns = gridApi.getColumnDefs();
    if (!props.showRemove) {
      gridApi.setColumnDefs(
        (columns as ColDef[])
          .filter((c) => c.field !== "remove")
          .map((c) => {
            return {
              ...c,
              rowDrag: false,
              editable: false
            };
          })
      );
    } else {
      const editableColumns = (columns as ColDef[]).map((c, i) => {
        return {
          ...c,
          editable: c.field === "rowNumber" ? false : true,
          rowDrag: i === 0 ? true : false
        };
      });
      gridApi.setColumnDefs([...editableColumns, removeColumn]);
    }

    gridApi.refreshCells();
    gridApi.sizeColumnsToFit();
  }, [props.showRemove]);

  const getAllRows = (params: { api: GridApi }) => {
    const rows: CustomRowData<T>[] = [];
    params.api.forEachNode((n) =>
      rows.push({ id: n.data.id, rowIndex: n.rowIndex, data: n.data })
    );
    return rows;
  };

  return (
    <div
      className='ag-theme-alpine'
      style={{ height: "100%", width: "100%", ...props.style }}
    >
      <AgGridReact
        onGridReady={onGridReady}
        rowData={props.rowData}
        gridOptions={options}
        headerHeight={props.headerHeight ?? 30}
        rowHeight={30}
        getRowNodeId={(d) => d.id}
        domLayout={props.style ? undefined : "autoHeight"}
        suppressDragLeaveHidesColumns
        context={{ errors: props.errors }}
      />
      {props.errors && <ErrorBuilder errors={props.errors} />}
    </div>
  );
}

export default withStyles(styles, { withTheme: true })(
  forwardRef(CommercialAgGrid)
) as <T>(
  props: CommercialAgGridProps<T> & { ref?: ForwardedRef<CommAgGridElement<T>> }
) => ReturnType<typeof CommercialAgGrid>;
