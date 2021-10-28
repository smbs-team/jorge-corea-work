// OperatingExpenses.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CommercialAgGrid } from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import CachedIcon from '@material-ui/icons/Cached';
import usePageManagerStore from 'stores/usePageManagerStore';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import { getCellStyle } from 'components/shared/gridUtility';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import CancelIcon from '@material-ui/icons/Cancel';
import {
  ColDef,
  ColGroupDef,
} from "ag-grid-community";

interface Props {
  columns: (ColDef | ColGroupDef)[];
}

function OperatingExpenses(props: Props): JSX.Element {
  const pageManagerStore = usePageManagerStore();

  const handleOnSave = () => {
    if (pageManagerStore.operatingExpenses === 'percent') {
      pageManagerStore.majorOpExpPercentGrid.validate();
    } else {
      pageManagerStore.majorOpExpDollarGrid.validate();
    }
  };

  const handleOnReset = () => {
    if (pageManagerStore.operatingExpenses === 'percent') {
      pageManagerStore.majorOpExpPercentGrid.reset();
    } else {
      pageManagerStore.majorOpExpDollarGrid.reset();
    }
  };

  return (
    <GridSection
      title={'Operating Expenses'}
      icons={[
        { icon: <InsertDriveFileIcon />, text: 'Save', onClick: handleOnSave },
        { icon: <CachedIcon />, text: 'Reset', onClick: handleOnReset },
      ]}
      miscContent={
        pageManagerStore.majorOpExpPercentGrid.isDirty ||
        pageManagerStore.majorOpExpDollarGrid.isDirty ? (
          !pageManagerStore.majorOpExpPercentGrid.errors.length &&
          !pageManagerStore.majorOpExpDollarGrid.errors.length ? (
            <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
          ) : (
            <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
          )
        ) : undefined
      }
    >
      <div style={{ marginBottom: 16 }}>
        <label style={{ fontSize: '1rem', fontWeight: 'bold' }}>
          % of Effective Gross Income
        </label>
        <CommercialAgGrid
          showRowNumber
          showRemove={pageManagerStore.operatingExpenses === 'percent'}
          rowData={pageManagerStore.majorOpExpPercentGrid.data.flatMap(
            (d) => d.data
          )}
          columnDefs={props.columns}
          rowNumberOptions={{
            rowDrag: pageManagerStore.operatingExpenses === 'percent',
          }}
          gridOptions={{
            defaultColDef: {
              editable: pageManagerStore.operatingExpenses === 'percent',
              cellStyle: getCellStyle,
            },
          }}
          errors={pageManagerStore.majorOpExpPercentGrid.errors}
          onChange={pageManagerStore.majorOpExpPercentGrid.setData}
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
          rowData={pageManagerStore.majorOpExpDollarGrid.data.flatMap(
            (d) => d.data
          )}
          columnDefs={props.columns}
          gridOptions={{
            defaultColDef: {
              editable: pageManagerStore.operatingExpenses === 'dollar',
              cellStyle: getCellStyle,
            },
          }}
          errors={pageManagerStore.majorOpExpDollarGrid.errors}
          onChange={pageManagerStore.majorOpExpDollarGrid.setData}
        />
      </div>
    </GridSection>
  );
}

export default OperatingExpenses;
