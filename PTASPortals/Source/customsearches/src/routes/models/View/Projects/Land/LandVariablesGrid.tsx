/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, PropsWithChildren, useEffect, useState } from 'react';
import { LandVariableGridRowData } from 'services/map.typings';
import { AgGridReact } from 'ag-grid-react';
import {
  CellValueChangedEvent,
  ColDef,
  GridApi,
  GridOptions,
  RowNode,
  SelectionChangedEvent,
  ValueFormatterParams,
} from 'ag-grid-community';
import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';
import { makeStyles } from '@material-ui/core';
import { CustomIconButton } from '@ptas/react-ui-library';
import DeleteIcon from '@material-ui/icons/Delete';
import { NumericCellEditor } from './numeric-editor-cell';
import AddCircleOutlineOutlinedIcon from '@material-ui/icons/AddCircleOutlineOutlined';
import NewViewModal, { NewViewModalValues } from './NewViewModal';
import NewNuisanceModal, { NewNuisanceModalValues } from './NewNuisanceModal';
// import { NumericCellEditor } from './numeric-cell-negative-editor';
interface GenericGridProps {
  rowData: LandVariableGridRowData[];
  datasetId: string;
  postProcessId: string;
  onlyNegative?: boolean;
  updateColData: (newData: LandVariableGridRowData[], change: boolean) => void;
  editing: boolean;
  // eslint-disable-next-line
  classes: any;
}

const useStyles = makeStyles({
  icons: {
    paddingLeft: '0',
    marginBottom: '15px',
  },
});

function formatNumber(number: number): string {
  return Math.floor(number)
    .toString()
    .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}

//eslint-disable-next-line
function compareObj(a: any, b: any): boolean {
  const aKeys = Object.keys(a).sort();
  const bKeys = Object.keys(b).sort();
  if (aKeys.length !== bKeys.length) {
    return false;
  }
  if (aKeys.join('') !== bKeys.join('')) {
    return false;
  }
  for (let i = 0; i < aKeys.length; i++) {
    if (a[aKeys[i]] !== b[bKeys[i]]) {
      return false;
    }
  }
  return true;
}

const LandVariableGrid = (
  props: PropsWithChildren<GenericGridProps>
): JSX.Element => {
  const [gridApi, setGridApi] = useState<GridApi | null>(null);
  const classes = useStyles();
  const [rowData, setrowData] = useState<LandVariableGridRowData[]>([]);
  const [addViewVisible, setAddViewVisible] = useState(false);
  const [addNuisanceVisible, setAddNuisanceVisible] = useState(false);
  const [rowsToDelete, setRowsToDelete] = useState<LandVariableGridRowData[]>(
    []
  );
  const emptyViewValues = {
    quality: -1,
    valueMethod: -1,
    viewType: -1,
    startValue: 1,
    endValue: '',
  };
  const empytNuisanceValues: NewNuisanceModalValues = {
    valueMethod: -1,
    viewType: -1,
    startValue: 1,
    endValue: '',
    airportNoiseLevel: 0,
    trafficNoiseLevel: 0,
  };
  const [newViewModalValues, setNewViewModalValues] =
    useState<NewViewModalValues>({ ...emptyViewValues });

  const [newNuisanceModalValues, setNewNuisanceModalValues] =
    useState<NewNuisanceModalValues>({ ...empytNuisanceValues });

  const adjustmentValueFormatter = (params: ValueFormatterParams): string => {
    const method = params.value;
    if (isNaN(method) || !method.length) return method;
    if (parseInt(method) < 0)
      return '- $' + formatNumber(parseInt(method) * -1);
    return '$' + formatNumber(method);
  };

  const adjustmentPercentajeFormatter = (
    params: ValueFormatterParams
  ): string => {
    const method = params.value;
    if (typeof method === 'number') return formatNumber(method) + '%';
    if (isNaN(method) || !method.length) return method;
    return formatNumber(method) + '%';
  };

  const [columnDef] = useState<ColDef[]>([
    {
      headerName: '',
      checkboxSelection: true,
      width: 30,
      maxWidth: 30,
      minWidth: 30,
    },
    {
      headerName: 'Characteristic Type',
      field: 'characteristicType',
      editable: false,
      flex: 2,
      colId: 'characteristicType',
      sortable: true,
      resizable: true,
    },
    {
      headerName: 'Description',
      field: 'description',
      editable: false,
      flex: 2,
      colId: 'description',
      sortable: true,
      resizable: true,
    },
    {
      headerName: 'Value',
      field: 'value',
      editable: false,
      flex: 1,
      colId: 'value',
      sortable: true,
      resizable: true,
    },
    {
      headerName: '$ min adj',
      field: 'minadjmoney',
      editable: false,
      flex: 1,
      colId: 'minadjmoney',
      valueFormatter: adjustmentValueFormatter,
      sortable: true,
      resizable: true,
      cellEditor: 'numericEditor',
      // valueParser: 'Number(newValue)',
    },
    {
      headerName: '$ max adj',
      field: 'maxadjmoney',
      editable: false,
      flex: 1,
      colId: 'maxadjmoney',
      valueFormatter: adjustmentValueFormatter,
      sortable: true,
      resizable: true,
      cellEditor: 'numericEditor',
      // valueParser: 'Number(newValue)',
    },
    {
      headerName: '% min adj',
      field: 'minadjpercentaje',
      editable: false,
      flex: 1,
      colId: 'minadjpercentaje',
      valueFormatter: adjustmentPercentajeFormatter,
      sortable: true,
      resizable: true,
      cellEditor: 'numericEditor',
      // valueParser: 'Number(newValue)',
    },
    {
      headerName: '% max adj',
      field: 'maxadjpercentaje',
      editable: false,
      flex: 1,
      colId: 'maxadjpercentaje',
      valueFormatter: adjustmentPercentajeFormatter,
      sortable: true,
      resizable: true,
      cellEditor: 'numericEditor',
      // valueParser: 'Number(newValue)',
    },
  ]);

  useEffect(() => {
    // if (!props.rowData.length) return;
    setrowData(props.rowData);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.rowData]);

  const updateData = (rowData: LandVariableGridRowData[]): void => {
    if (props.updateColData) {
      props.updateColData(rowData, true);
    }
  };

  useEffect(() => {
    updateData(rowData);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [rowData]);

  const onGridReady = (params: GridOptions): void => {
    setGridApi(params.api as GridApi);
  };

  const onRowChanged = async (
    _params: CellValueChangedEvent
  ): Promise<void> => {
    const tempRowData: LandVariableGridRowData[] = [];
    if (gridApi) {
      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        tempRowData.push({
          characteristicType: data.characteristicType,
          description: data.description,
          value: data.value,
          minadjmoney: data.minadjmoney,
          maxadjmoney: data.maxadjmoney,
          minadjpercentaje: data.minadjpercentaje,
          maxadjpercentaje: data.maxadjpercentaje,
          ...data,
        });
      });
    }
    setrowData(tempRowData);
    updateData(tempRowData);
  };

  const deleteRows = (): void => {
    if (!rowsToDelete || rowsToDelete.length === 0) {
      return;
    }

    const tempRowData: LandVariableGridRowData[] = [];

    if (gridApi) {
      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        if (!rowsToDelete.find((row) => compareObj(row, data))) {
          tempRowData.push({
            characteristicType: data.characteristicType,
            description: data.description,
            value: data.value,
            minadjmoney: data.minadjmoney,
            maxadjmoney: data.maxadjmoney,
            minadjpercentaje: data.minadjpercentaje,
            maxadjpercentaje: data.maxadjpercentaje,
            ...data,
          });
        }
      });
    }

    setrowData(tempRowData);
    updateData(tempRowData);
  };

  const onSelectionChanged = (event: SelectionChangedEvent): void => {
    const selectedNodes = event.api.getSelectedNodes();
    const colNames = selectedNodes.map((n: RowNode) => n.data);
    setRowsToDelete(colNames as LandVariableGridRowData[]);
  };

  return (
    <Fragment>
      <div className={classes.icons}>
        <CustomIconButton
          text="New View"
          icon={<AddCircleOutlineOutlinedIcon />}
          onClick={(): void => {
            setNewViewModalValues({ ...emptyViewValues });
            return setAddViewVisible(true);
          }}
          disabled={!props.editing}
          className={props.classes.firstIconButton}
        />
        <CustomIconButton
          text="New Nuisance"
          icon={<AddCircleOutlineOutlinedIcon />}
          onClick={(): void => {
            // todo cambiar por el otro modal.
            setNewNuisanceModalValues({ ...empytNuisanceValues });
            return setAddNuisanceVisible(true);
          }}
          disabled={!props.editing}
          className={props.classes.iconButton}
        />
        <CustomIconButton
          text="Delete"
          icon={<DeleteIcon />}
          onClick={deleteRows}
          disabled={!(props.editing && rowsToDelete.length)}
        />
        {props.children}
      </div>
      <div
        className="ag-theme-alpine adjustment-grid"
        style={{
          height: '320px',
          width: '100%',
          paddingLeft: 0,
          paddingRight: 0,
        }}
      >
        <NewViewModal
          isOpen={addViewVisible}
          values={newViewModalValues}
          onSave={(v): void => setrowData((prev) => [...prev, v])}
          onClose={(): void => setAddViewVisible(false)}
        ></NewViewModal>
        <NewNuisanceModal
          isOpen={addNuisanceVisible}
          values={newNuisanceModalValues}
          onSave={(v): void => setrowData((prev) => [...prev, v])}
          onClose={(): void => setAddNuisanceVisible(false)}
        ></NewNuisanceModal>
        <AgGridReact
          rowSelection="multiple"
          onGridReady={onGridReady}
          onCellValueChanged={onRowChanged}
          rowData={rowData}
          rowHeight={40}
          headerHeight={40}
          columnDefs={columnDef}
          onSelectionChanged={onSelectionChanged}
          rowStyle={{ background: 'white' }}
          frameworkComponents={{
            numericEditor: NumericCellEditor,
          }}
        />
      </div>
    </Fragment>
  );
};

export default LandVariableGrid;
