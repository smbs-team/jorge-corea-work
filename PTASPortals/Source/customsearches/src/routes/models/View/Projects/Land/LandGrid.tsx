/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState } from 'react';
import { LandGridData } from 'services/map.typings';
import { AgGridReact } from 'ag-grid-react';
import {
  CellValueChangedEvent,
  ColDef,
  GridApi,
  GridOptions,
  ValueFormatterParams,
} from 'ag-grid-community';
import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';
import '../../../../../assets/grid-styles/styles.scss';
import numeral from 'numeral';
interface LandGridProps {
  rowData: LandGridData[];
  columnsDefs?: ColDef[];
  editing: boolean;
  updateColData: (newData: LandGridData[], change: boolean) => void;
}

const defaultColDef: ColDef = {
  resizable: true,
};

const currencyFormatter = (
  currency: string | undefined,
  sign = '$'
): string => {
  if (!currency) return '';
  const value = parseFloat(currency);
  const sansDec = value.toFixed(0);
  const formatted = sansDec.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  return sign + `${formatted}`;
};

const LandGrid = (props: LandGridProps): JSX.Element => {
  const [gridApi, setGridApi] = useState<GridApi | null>(null);
  const [rowData, setrowData] = useState<LandGridData[]>([]);
  const [addColumn, setAddColumn] = useState<boolean>(false);

  const [columnDef, setColumnDef] = useState<ColDef[]>([
    {
      headerName: 'SqFt',
      field: 'to',
      flex: 1,
    },
    {
      headerName: 'Default',
      field: 'default',
      flex: 1,
      editable: true,
    },
  ]);

  const getValueFormater = (columnName: string | undefined): ColDef => {
    if (!columnName) return {};
    //commited
    if (
      columnName !== 'SqFt' &&
      columnName !== 'Front Feet' &&
      columnName !== 'to'
    ) {
      return {
        valueFormatter: (params: ValueFormatterParams): string => {
          return currencyFormatter(params.value, '$');
        },
      };
    }
    return {
      valueFormatter: (params: ValueFormatterParams): string => {
        return numeral(params.value).format('0,0.00');
      },
    };
  };

  useEffect(() => {
    if (props.columnsDefs?.length) {
      const columns = props.columnsDefs.map((col) => {
        return {
          ...col,
          headerName: getHeaderName(col?.headerName),
          ...getValueFormater(col?.headerName),
        };
      });
      setColumnDef(columns);
    }
  }, [props.columnsDefs]);

  const getHeaderName = (headerName: string | undefined): string => {
    if (headerName) {
      if (headerName === 'to') {
        return 'SqFt';
      }
      if (headerName === 'LandValue') {
        return '';
      }
    }
    return `${headerName}`;
  };

  useEffect(() => {
    setrowData(props.rowData);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.rowData]);

  useEffect(() => {
    if (rowData) {
      props.updateColData(rowData, addColumn);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [rowData]);

  const onGridReady = (params: GridOptions): void => {
    setGridApi(params.api as GridApi);
  };

  const onRowChanged = async (
    _params: CellValueChangedEvent
  ): Promise<void> => {
    const tempRowData: LandGridData[] = [];

    if (gridApi) {
      gridApi.forEachNodeAfterFilterAndSort(({ data }) => {
        tempRowData.push({
          ...data,
          to: data.to || '0',
          default: data.default,
        });
      });
    }
    setAddColumn(true);
    setrowData(tempRowData);
  };

  return (
    <Fragment>
      <div className="MainGrid">
        <div
          className="ag-theme-alpine"
          style={{
            height: '320px',
            width: '100%',
            paddingLeft: 0,
            paddingRight: 0,
          }}
        >
          <AgGridReact
            defaultColDef={defaultColDef}
            rowSelection="multiple"
            onGridReady={onGridReady}
            onCellValueChanged={onRowChanged}
            rowData={rowData}
            columnDefs={columnDef}
            headerHeight={25}
            rowHeight={25}
            rowStyle={{ background: 'white' }}
            suppressClickEdit={!props.editing}
          />
        </div>
      </div>
    </Fragment>
  );
};

export default LandGrid;
