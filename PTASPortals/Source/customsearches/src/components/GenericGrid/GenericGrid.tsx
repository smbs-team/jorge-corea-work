/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import { ExcelSheetJson, GenericGridRowData } from 'services/map.typings';
import { AgGridColumn, AgGridReact } from 'ag-grid-react';
import {
  CellClassParams,
  CellValueChangedEvent,
  ColDef,
  ColumnApi,
  GridApi,
  GridOptions,
  IsColumnFuncParams,
  RowNode,
  SelectionChangedEvent,
} from 'ag-grid-community';
import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';
import { CustomIconButton } from '@ptas/react-ui-library';
import AddCircleOutlineOutlinedIcon from '@material-ui/icons/AddCircleOutlineOutlined';
import GetApp from '@material-ui/icons/GetApp';
import {
  createStyles,
  LinearProgress,
  StyleRules,
  withStyles,
  WithStyles,
} from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete';
import useJsonTools from 'components/common/useJsonTools';
import { useLifecycles, useUpdateEffect } from 'react-use';
import useDisableVariablesMenu from 'components/common/useDisableVariablesMenu';
import { AppContext } from 'context/AppContext';
import '../../assets/grid-styles/genericgrid.scss';
import { ValidationErrorType } from 'components/ErrorMessage/ErrorMessage';
import { v4 as uuidv4 } from 'uuid';
import { ProjectContext } from 'context/ProjectsContext';

export interface GenericGridProps extends WithStyles<typeof useStyles> {
  rowData?: GenericGridRowData[];
  saveColData: () => boolean;
  updateColData: (newData: GenericGridRowData[], change: boolean) => void;
  gridOptions?: GridOptions;
  height?: string | number;
  hideTitle?: boolean;
  globalVariablesExportOrder?: boolean;
  useDropdown?: boolean;
  badExpressions?: ValidationErrorType[];
  lockPrecommitExpressions?: boolean;
}

export interface CellStyleType {
  backgroundColor: string;
}

interface RowStyleParams {
  //eslint-disable-next-line
  data: any;
  node: RowNode;
  rowIndex: number;
  //eslint-disable-next-line
  $scope: any;
  api: GridApi;
  columnApi: ColumnApi;
  //eslint-disable-next-line
  context: any;
}

export interface RowToDelete {
  name: string;
  type: string;
}

const typeOptions = ['Dependent', 'Independent', 'Calculated'];

const useStyles = (): StyleRules =>
  createStyles({
    displayField: {
      fontSize: '18px',
      display: 'inline',
      marginRight: '10px',
    },
    exportButton: {},
  });

const GenericGrid = (props: GenericGridProps): JSX.Element => {
  const { classes } = props;
  const { setSnackBar } = useContext(AppContext);
  const [gridApi, setGridApi] = useState<GridApi | null>(null);
  const [, setColumnApi] = useState<ColumnApi | null>(null);
  const { getExcelFromJson, isLoading } = useJsonTools();
  const [columTypeConfig, setColumTypeConfig] = useState<ColDef>({});
  const [defaultColDef, setDefaultColDef] = useState<ColDef>({
    sortable: true,
  });

  const [badExpressions, setBadExpressions] = useState<ValidationErrorType[]>(
    []
  );

  useEffect(() => {
    if (props.badExpressions) {
      setBadExpressions(props.badExpressions);
      setLastkey(uuidv4());
    }
  }, [props.badExpressions]);

  useEffect(() => {
    if (props.useDropdown) {
      setColumTypeConfig({
        cellEditor: 'agSelectCellEditor',
        cellEditorParams: { values: typeOptions },
        editable: true,
        cellClass: 'GenericGrid-showBorders',
      });
      setDefaultColDef({
        sortable: false,
      });
    }
  }, [props.useDropdown]);

  const [rowData, setRowData] = useState<GenericGridRowData[]>([]);
  const [lastKey, setLastkey] = useState<string>(uuidv4());

  const [addColumn, setAddColumn] = useState<boolean>(false);
  const [rowsToDelete, setRowsToDelete] = useState<RowToDelete[]>([]);
  const context = useContext(AppContext);
  const projectContext = useContext(ProjectContext);
  const disableVariables = useDisableVariablesMenu();
  useLifecycles(
    () => disableVariables(false),
    () => disableVariables(true)
  );

  //add variables selected from modals
  useEffect(() => {
    const variables = context.globalVariablesToAdd;
    if (!variables || variables.length < 1 || !gridApi) return;
    const addedVariables = variables.map((variable) => {
      return {
        ...variable,
        type: 'Calculated',
      };
    });
    setAddColumn(true);
    setRowData((data) => [...data, ...addedVariables]);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [context.globalVariablesToAdd]);

  const updateLocalData = (): void => {
    if (!props.rowData) return;
    setRowData(props.rowData);
  };

  useEffect(updateLocalData, [props.rowData]);

  const updateExternalData = (): void => {
    if (rowData) {
      props.updateColData(rowData, addColumn);
    }
  };

  useUpdateEffect(updateExternalData, [rowData, addColumn]);

  const onGridReady = (params: GridOptions): void => {
    if (params?.api) setGridApi(params?.api);
    if (params?.columnApi) setColumnApi(params?.columnApi);
  };

  const addItem = (): void => {
    if (props.gridOptions) {
      if (!gridApi || !props.gridOptions.api) return;

      const index = gridApi.getDisplayedRowCount();

      gridApi.applyTransaction({
        add: [
          {
            expressionRole: 'CalculatedColumnPostCommit',
            expressionType: 'TSQL',
          },
        ],
        addIndex: index,
      });

      gridApi.setFocusedCell(index, 'name');
      gridApi.startEditingCell({
        rowIndex: index,
        colKey: 'name',
      });
      return;
    }

    const newIndex = rowData.length;
    const newItemRow = {
      type: 'Calculated',
      expressionRole: 'CalculatedColumnPostCommit',
      expressionType: 'TSQL',
      name: 'defaultNewVariable',
      isNewRow: true,
    };
    const nv = [...rowData, { ...newItemRow }];
    setAddColumn(true);
    setRowData(nv as GenericGridRowData[]);
    setTimeout(() => {
      try {
        gridApi?.setFocusedCell(newIndex, 'name');
        gridApi?.startEditingCell({
          rowIndex: newIndex,
          colKey: 'name',
        });
      } catch (error) {
        console.log(error);
      }
    }, 100);
  };

  const onRowChanged = async (
    _params: CellValueChangedEvent
  ): Promise<void> => {
    setAddColumn(true);
    const tempRowData: GenericGridRowData[] = [];
    if (gridApi) {
      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        if (props.gridOptions) {
          tempRowData.push({
            name: data.name,
            transformation: data.transformation,
            category: data.category,
            type: data.type,
            note: data.note,
            expressionType: data.expressionType,
            expressionRole: data.expressionRole,
          });
        } else {
          tempRowData.push({
            type: data.type,
            name: data.name,
            expressionType: data.expressionType,
            expressionRole: data.expressionRole,
            transformation: data.transformation,
            category: data.category || '',
            note: data.note || '',
          });
        }
      });
    }
    if (props.useDropdown) {
      const calculated = tempRowData.filter(
        (rowData) => rowData.type === 'Calculated'
      );
      const dependent = tempRowData.filter(
        (rowData) => rowData.type === 'Dependent'
      );
      if (dependent.length > 1) {
        const extraDependent = dependent.pop();
        if (extraDependent) {
          calculated.push({
            ...extraDependent,
            type: 'Calculated',
          });
        }
        setSnackBar?.({
          severity: 'warning',
          text: 'There can only be one dependent variable',
        });
      }
      const independent = tempRowData.filter(
        (rowData) => rowData.type === 'Independent'
      );
      setRowData([...dependent, ...independent, ...calculated]);
    } else {
      setRowData(tempRowData);
    }
  };

  const saveGridChanges = async (): Promise<void> => {
    await props.saveColData();
  };

  const cellStyle = (params: CellClassParams): CellStyleType => {
    if (params.value === 'Dependent') return { backgroundColor: '#caf7ad' };
    if (params.value === 'Independent') return { backgroundColor: '#ffffc3' };
    return { backgroundColor: '#e5fff9' };
  };

  const onSelectionChanged = (event: SelectionChangedEvent): void => {
    const selectedNodes = event.api.getSelectedNodes();
    const colNames = selectedNodes
      .filter((n) => n.data.type !== 'CalculatedColumn')
      .map((n: RowNode) => ({
        name: n.data.name,
        type: n.data.type,
      }));
    setRowsToDelete(colNames);
  };

  const deleteRows = (): void => {
    if (!rowsToDelete || rowsToDelete.length === 0) {
      return;
    }

    if (props.gridOptions) {
      const toRemove = gridApi?.getSelectedRows();
      props.gridOptions?.api?.applyTransaction({
        remove: toRemove,
      });

      const newArray: GenericGridRowData[] = [];
      gridApi?.forEachNodeAfterFilterAndSort(({ data }) => {
        newArray.push({
          type: data.type,
          name: data.name,
          expressionType: data.expressionType,
          expressionRole: data.expressionRole,
          transformation: data.transformation,
          category: data.category || '',
          note: data.note || '',
        });
      });
      props.updateColData(newArray, true);
      return;
    }

    const tempRowData: GenericGridRowData[] = [];

    if (gridApi) {
      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        if (rowsToDelete.find((row) => row.name !== data.name)) {
          tempRowData.push({
            type: data.type,
            name: data.name,
            transformation: data.transformation,
            category: data.category || '',
            note: data.note || '',
          });
        }
      });
    }
    if (tempRowData.length) setRowData(tempRowData);
  };

  const toExcel = async (): Promise<void> => {
    let rows;
    let columns;
    if (props.globalVariablesExportOrder) {
      if (!gridApi) return;
      columns = ['Name', 'Transformation', 'Category', 'Type', 'Note'];
      const globalRows: string[][] = [];

      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        globalRows.push([
          data.name,
          data.transformation,
          data.category,
          data.type,
          data.note,
        ]);
      });
      rows = globalRows;
    } else {
      columns = ['Type', 'Name', 'Transformation', 'Category', 'Note'];

      rows = rowData.map((data) => {
        return [
          data.type ?? '',
          data.name,
          data.transformation ?? '',
          data.category ?? '',
          data.note ?? '',
        ];
      });
    }

    const toSend: ExcelSheetJson = {
      Sheet1: {
        headers: columns,
        rows: rows,
      },
    };

    await getExcelFromJson(toSend, 'variables.xlsx');
  };

  const isRowSelectable = (rowNode: RowNode): boolean => {
    if (!props.useDropdown) {
      return !['Independent', 'Dependent'].includes(rowNode.data.type);
    }
    return true;
  };

  const handleEditableCell = (params: IsColumnFuncParams): boolean => {
    if (projectContext.modelDetails?.isLocked) return false;
    if (!props.lockPrecommitExpressions) return true;
    if (params.node.data.type === 'CalculatedColumn') return false;
    if (!props.useDropdown) {
      return !['Independent', 'Dependent'].includes(params.data.type);
    }
    return true;
  };

  const getcheckboxSelection = (params: IsColumnFuncParams): boolean => {
    if (params.data.type === 'CalculatedColumn') return false;
    return true;
  };

  const getRowStyle = (params: RowStyleParams): {} => {
    if (
      badExpressions?.some((be) => be.validatedIndex === params.node.rowIndex)
    ) {
      return {
        borderStyle: 'groove',
        borderColor: 'red',
        height: '35px',
      };
    }
    return {};
  };

  return (
    <Fragment>
      <div className="grid-buttons">
        {!props.hideTitle && (
          <div className={classes.displayField}>
            <strong>Variables</strong>
          </div>
        )}
        <CustomIconButton
          text="New Variable"
          icon={<AddCircleOutlineOutlinedIcon />}
          onClick={addItem}
          disabled={
            projectContext.modelDetails?.isLocked ||
            !props.lockPrecommitExpressions
          }
        />
        <CustomIconButton
          text="Delete Variables"
          icon={<DeleteIcon />}
          onClick={deleteRows}
          style={{ marginLeft: 20 }}
          disabled={
            rowsToDelete.length === 0 ||
            projectContext.modelDetails?.isLocked ||
            !props.lockPrecommitExpressions
          }
        />
        <CustomIconButton
          text="Export"
          icon={<GetApp />}
          onClick={(): void => {
            saveGridChanges();
            toExcel();
          }}
          style={{ marginLeft: 20 }}
          disabled={
            (gridApi ? gridApi?.getDisplayedRowCount() < 1 : true) ||
            !props.lockPrecommitExpressions
          }
          className={classes.exportButton}
        />
      </div>
      {isLoading && <LinearProgress />}
      <div
        className="ag-theme-alpine"
        style={{
          height: props.height ?? '280px',
          width: '100%',
          paddingLeft: 0,
          paddingRight: 0,
        }}
      >
        <div className="GenericGrid">
          {props.gridOptions ? (
            <AgGridReact
              rowSelection="multiple"
              onGridReady={onGridReady}
              onCellValueChanged={onRowChanged}
              onSelectionChanged={onSelectionChanged}
              gridOptions={props.gridOptions}
              rowStyle={{ background: 'white' }}
            />
          ) : (
            <AgGridReact
              rowSelection="multiple"
              onGridReady={onGridReady}
              onCellValueChanged={onRowChanged}
              onSelectionChanged={onSelectionChanged}
              defaultColDef={defaultColDef}
              rowData={rowData}
              headerHeight={33}
              rowHeight={33}
              isRowSelectable={isRowSelectable}
              groupHeaderHeight={0}
              rowStyle={{ background: 'white' }}
              key={lastKey}
              getRowStyle={getRowStyle}
            >
              <Fragment>
                <AgGridColumn
                  field=""
                  checkboxSelection={getcheckboxSelection}
                  minWidth={25}
                  maxWidth={25}
                  width={25}
                ></AgGridColumn>
                <AgGridColumn
                  flex={1}
                  resizable={true}
                  field="type"
                  cellStyle={cellStyle}
                  {...columTypeConfig}
                ></AgGridColumn>
                <AgGridColumn
                  flex={1}
                  editable={handleEditableCell}
                  field="name"
                  resizable={true}
                ></AgGridColumn>
                <AgGridColumn
                  flex={6}
                  resizable={true}
                  field="transformation"
                  editable={handleEditableCell}
                ></AgGridColumn>
                <AgGridColumn
                  editable={!projectContext.modelDetails?.isLocked}
                  flex={1}
                  field="category"
                  resizable={true}
                ></AgGridColumn>
                <AgGridColumn
                  flex={1}
                  editable={!projectContext.modelDetails?.isLocked}
                  field="note"
                  resizable={true}
                ></AgGridColumn>
              </Fragment>
            </AgGridReact>
          )}
        </div>
      </div>
    </Fragment>
  );
};

export default withStyles(useStyles)(GenericGrid);
