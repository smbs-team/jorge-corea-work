// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffectOnce, useWindowSize } from 'react-use';
import { SectionUses } from 'components/shared';
import useRibbonStore from 'stores/useRibbonStore';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import { isEdge } from 'react-device-detect';
import GenericSectionGrid from 'components/shared/GenericSectionGrid';
import usePageManagerStore from 'stores/usePageManagerStore';
import { BaseInputs } from 'stores/gridSlices/hotels/createBaseInputsGridSlice';
import { CSSProperties } from '@material-ui/core/styles/withStyles';

export interface HotelsProps {
  minEff?: string;
  maxEff?: string;
  n10?: string;
  n20?: string;
  n40?: string;
  n50?: string;
  n60?: string;
}

const baseInputsColumns = [
  { field: 'name', headerName: 'Name' },
  { field: 'value', headerName: 'Value', editable: true },
];

const columns = [
  {
    field: 'minEff',
    headerName: 'Min Eff Year',
    cellEditorParams: {
      thousandSeparator: false,
      decimalScale: 0,
    },
  },
  {
    field: 'maxEff',
    headerName: 'Max Eff Year',
    cellEditorParams: {
      thousandSeparator: false,
      decimalScale: 0,
    },
  },
  { field: 'n10', headerName: 'Nbhd 10' },
  { field: 'n20', headerName: 'Nbhd 20' },
  { field: 'n30', headerName: 'Nbhd 30' },
  { field: 'n40', headerName: 'Nbhd 40' },
  { field: 'n50', headerName: 'Nbhd 50' },
  { field: 'n60', headerName: 'Nbhd 60' },
];

export const getHostsStyle = (params: {
  data: { value: string };
}): CSSProperties | undefined => {
  if (params.data.value === '') {
    return { 'background-color': '#e5fff9', 'font-weight': 'bold' };
  }
  return undefined;
};

function Hotels(): JSX.Element {
  const headerStore = useContentHeaderStore();
  const ribbonStore = useRibbonStore();
  const { width } = useWindowSize();
  const pageManagerStore = usePageManagerStore();

  useEffectOnce(() => {
    ribbonStore.setShowHotelsConfig(true);
    ribbonStore.setHideAdjustmentFactors(true);
    ribbonStore.setHideConfiguration(true);
    ribbonStore.setHideRentableArea(true);
    headerStore.setHidePagination(false);
    ribbonStore.setHideSettings(false);
  });

  return (
    <div
      style={{
        display: 'flex',
        gap: 30,
        justifyContent: width > (isEdge ? 2021 : 1920) ? 'center' : undefined,
      }}
    >
      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <SectionUses />
        <GenericSectionGrid<HotelsProps>
          title="Average Daily Room Rate (ADR)"
          GridOptions={{
            rowData: pageManagerStore.averageRoomRateGrid.initialData,
            columnDefs: columns,
          }}
          iconOptions={{ addRowText: 'Add effective year range' }}
          onChange={pageManagerStore.averageRoomRateGrid.setData}
          onReset={pageManagerStore.averageRoomRateGrid.reset}
          onSave={pageManagerStore.averageRoomRateGrid.validate}
          errors={pageManagerStore.averageRoomRateGrid.errors}
          isDirty={pageManagerStore.averageRoomRateGrid.isDirty}
        />
        <GenericSectionGrid<HotelsProps>
          title="Occupancy Rate (OCC)"
          GridOptions={{
            rowData: pageManagerStore.occupancyRateGrid.initialData,
            columnDefs: columns,
          }}
          iconOptions={{ addRowText: 'Add effective year range' }}
          onChange={pageManagerStore.occupancyRateGrid.setData}
          onReset={pageManagerStore.occupancyRateGrid.reset}
          onSave={pageManagerStore.occupancyRateGrid.validate}
          errors={pageManagerStore.occupancyRateGrid.errors}
          isDirty={pageManagerStore.occupancyRateGrid.isDirty}
        />
        <GenericSectionGrid<HotelsProps>
          title="Capitalization Rates"
          GridOptions={{
            rowData: pageManagerStore.hotelsCapRateGrid.initialData,
            columnDefs: columns,
          }}
          iconOptions={{ addRowText: 'Add effective year range' }}
          onChange={pageManagerStore.hotelsCapRateGrid.setData}
          onReset={pageManagerStore.hotelsCapRateGrid.reset}
          onSave={pageManagerStore.hotelsCapRateGrid.validate}
          errors={pageManagerStore.hotelsCapRateGrid.errors}
          isDirty={pageManagerStore.hotelsCapRateGrid.isDirty}
        />
      </div>
      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <GenericSectionGrid<BaseInputs>
          title="Base Inputs"
          GridOptions={{
            rowData: pageManagerStore.baseInputsGrid.initialData,
            columnDefs: baseInputsColumns,
            showRemove: false,
            showRowNumber: false,
          }}
          iconOptions={{ hideIcons: ['addRow', 'reset'] }}
          onChange={pageManagerStore.baseInputsGrid.setData}
          onReset={pageManagerStore.baseInputsGrid.reset}
          onSave={pageManagerStore.baseInputsGrid.validate}
          errors={pageManagerStore.baseInputsGrid.errors}
          isDirty={pageManagerStore.baseInputsGrid.isDirty}
        />
        <GenericSectionGrid<BaseInputs>
          title="STR HOST Almanac Income and Expense Values"
          GridOptions={{
            rowData: pageManagerStore.hostsGrid.initialData,
            columnDefs: baseInputsColumns,
            showRemove: false,
            showRowNumber: false,
            gridOptions: {
              getRowStyle: getHostsStyle,
              defaultColDef: {
                sortable: false,
              },
            },
          }}
          iconOptions={{ hideIcons: ['addRow', 'reset'] }}
          onChange={pageManagerStore.hostsGrid.setData}
          onReset={pageManagerStore.hostsGrid.reset}
          onSave={pageManagerStore.hostsGrid.validate}
          errors={pageManagerStore.hostsGrid.errors}
          isDirty={pageManagerStore.hostsGrid.isDirty}
        />
      </div>
    </div>
  );
}

export default Hotels;
