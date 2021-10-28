// OperatingExpenses.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CommAgGridElement, CommercialAgGrid } from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import CachedIcon from '@material-ui/icons/Cached';
import usePageManagerStore from 'stores/usePageManagerStore';
import { Fragment, useRef } from 'react';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import { getCellStyle } from 'components/shared/gridUtility';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import CancelIcon from '@material-ui/icons/Cancel';
import BuildIcon from '@material-ui/icons/Build';
import { CommercialIncomeProps } from '..';
import {
  ColDef,
  ColGroupDef,
} from "ag-grid-community";

interface Props {
  columns: (ColDef | ColGroupDef)[];
}

function OperatingExpenses(props: Props): JSX.Element {
  const pageManagerStore = usePageManagerStore();
  const percentGridEle = useRef<CommAgGridElement<CommercialIncomeProps>>(null);
  const dollarGridEle = useRef<CommAgGridElement<CommercialIncomeProps>>(null);

  const handleOnSave = () => {
    if (pageManagerStore.operatingExpenses === 'percent') {
      pageManagerStore.opExpPercentGrid.validate();
    } else {
      pageManagerStore.opExpDollarGrid.validate();
    }
  };

  const handleOnReset = () => {
    if (pageManagerStore.operatingExpenses === 'percent') {
      pageManagerStore.opExpPercentGrid.reset();
    } else {
      pageManagerStore.opExpDollarGrid.reset();
    }
  };

  return (
    <GridSection
      title={'Operating Expenses'}
      icons={[
        { icon: <InsertDriveFileIcon />, text: 'Save', onClick: handleOnSave },
        {
          icon: <AddCircleOutlineIcon />,
          text: 'Add effective year range',
          onClick: () => {
            if (pageManagerStore.operatingExpenses === 'percent') {
              percentGridEle.current?.addRow({});
            } else {
              dollarGridEle.current?.addRow({});
            }
          },
        },
        { icon: <CachedIcon />, text: 'Reset', onClick: handleOnReset },
      ]}
      miscContent={
        <Fragment>
          {pageManagerStore.opExpPercentGrid.isDirty ||
          pageManagerStore.opExpDollarGrid.isDirty ? (
            !pageManagerStore.opExpPercentGrid.errors.length &&
            !pageManagerStore.opExpDollarGrid.errors.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined}
          {parseFloat(pageManagerStore.opExpensesAf) !== 0 && (
            <BuildIcon style={{ marginLeft: 8 }} />
          )}
        </Fragment>
      }
    >
      <div style={{ marginBottom: 16 }}>
        <label style={{ fontSize: '1rem', fontWeight: 'bold' }}>
          % of Effective Gross Income
        </label>
        <CommercialAgGrid
          showRowNumber
          showRemove={pageManagerStore.operatingExpenses === 'percent'}
          rowData={pageManagerStore.opExpPercentGrid.data.flatMap(
            (d) => d.data
          )}
          columnDefs={props.columns}
          rowNumberOptions={{
            rowDrag: pageManagerStore.operatingExpenses === 'percent',
          }}
          ref={percentGridEle}
          gridOptions={{
            defaultColDef: {
              editable: pageManagerStore.operatingExpenses === 'percent',
              cellStyle: getCellStyle,
            },
          }}
          errors={pageManagerStore.opExpPercentGrid.errors}
          onChange={pageManagerStore.opExpPercentGrid.setData}
        />
      </div>
      <div>
        <label style={{ fontSize: '1rem', fontWeight: 'bold' }}>
          $ per Square foot
        </label>
        <CommercialAgGrid
          showRowNumber
          showRemove={pageManagerStore.operatingExpenses === 'dollar'}
          rowNumberOptions={{
            rowDrag: pageManagerStore.operatingExpenses === 'dollar',
          }}
          rowData={pageManagerStore.opExpDollarGrid.data.flatMap((d) => d.data)}
          columnDefs={props.columns}
          ref={dollarGridEle}
          gridOptions={{
            defaultColDef: {
              editable: pageManagerStore.operatingExpenses === 'dollar',
              cellStyle: getCellStyle,
            },
          }}
          errors={pageManagerStore.opExpDollarGrid.errors}
          onChange={pageManagerStore.opExpDollarGrid.setData}
        />
      </div>
    </GridSection>
  );
}

export default OperatingExpenses;
