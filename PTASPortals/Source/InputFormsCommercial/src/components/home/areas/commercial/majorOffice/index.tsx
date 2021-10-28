// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffectOnce, useWindowSize } from 'react-use';

import OperatingExpenses from './sections/OperatingExpenses';
import { SectionUses } from 'components/shared';
import useRibbonStore from 'stores/useRibbonStore';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import PreviousYear from 'components/shared/PreviousYear';
import { isEdge } from 'react-device-detect';
import GenericSectionGrid from 'components/shared/GenericSectionGrid';
import usePageManagerStore from 'stores/usePageManagerStore';
import { IncomeRates } from '../../types';
import { CSSProperties } from '@material-ui/core/styles/withStyles';

export interface MajorOfficeProps extends IncomeRates {
  empty?: string;
}

const columns = [
  {
    field: 'empty',
    headerName: '',
    cellStyle: { backgroundColor: '#e5fff9' },
    cellEditor: '',
  },
  { field: 'grade1', headerName: 'Class C' },
  { field: 'grade2', headerName: 'Class B' },
  { field: 'grade3', headerName: 'Class A-/B+' },
  { field: 'grade4', headerName: 'Class A' },
  { field: 'grade5', headerName: 'Class A+' },
  { field: 'grade6', headerName: 'Class A++' },
];

const getBaseYearsStyle = (params: {
  data: { empty: string };
}): CSSProperties | undefined => {
  if (params.data.empty === 'Base Years') {
    return { 'background-color': '#e5fff9' };
  }
  return { 'background-color': '#e5e5e5' };
};

export const getBaseYearsStyleLease = (params: {
  data: { empty: string };
}): CSSProperties | undefined => {
  if (params.data.empty === 'Base Years') {
    return { 'background-color': '#e5fff9' };
  }
  return undefined;
};

function MajorOffice(): JSX.Element {
  const ribbonStore = useRibbonStore();
  const headerStore = useContentHeaderStore();
  const { width } = useWindowSize();
  const pageManagerStore = usePageManagerStore();

  useEffectOnce(() => {
    ribbonStore.setDefault();
    headerStore.setHidePagination(false);
    headerStore.setHideYear(false);
    ribbonStore.setShowHotelsConfig(false);
    ribbonStore.setHideAdjustmentFactors(false);
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
        <GenericSectionGrid<MajorOfficeProps>
          title="Base Years"
          GridOptions={{
            rowData: pageManagerStore.majorBaseYearsGrid.initialData,
            columnDefs: columns,
            showRemove: false,
            showRowNumber: false,
            gridOptions: {
              defaultColDef: {
                cellEditorParams: {
                  thousandSeparator: false,
                  decimalScale: 0,
                },
              },
            },
            rowNumberOptions: { rowDrag: false },
          }}
          iconOptions={{ hideIcons: ['addRow'] }}
          onChange={pageManagerStore.majorBaseYearsGrid.setData}
          onReset={pageManagerStore.majorBaseYearsGrid.reset}
          onSave={pageManagerStore.majorBaseYearsGrid.validate}
          errors={pageManagerStore.majorBaseYearsGrid.errors}
          isDirty={pageManagerStore.majorBaseYearsGrid.isDirty}
        />
        <GenericSectionGrid<MajorOfficeProps>
          title={`Lease Rates ($/sqft.)`}
          GridOptions={{
            rowData: pageManagerStore.majorLeaseRateGrid.data.flatMap(
              (d) => d.data
            ),
            columnDefs: columns,
            gridOptions: {
              getRowStyle: getBaseYearsStyleLease,
              defaultColDef: {
                cellEditorParams: { roundBy: 0.05 },
              },
            },
          }}
          onChange={pageManagerStore.majorLeaseRateGrid.setData}
          onReset={pageManagerStore.majorLeaseRateGrid.reset}
          onSave={pageManagerStore.majorLeaseRateGrid.validate}
          errors={pageManagerStore.majorLeaseRateGrid.errors}
          isDirty={pageManagerStore.majorLeaseRateGrid.isDirty}
          isAdjusted={parseFloat(pageManagerStore.leaseRateAf) !== 0}
        />
        <GenericSectionGrid<MajorOfficeProps>
          title="Vacancy (%)"
          GridOptions={{
            rowData: pageManagerStore.majorVacancyGrid.initialData,
            columnDefs: columns,
            showRowNumber: false,
            showRemove: false,
          }}
          onChange={pageManagerStore.majorVacancyGrid.setData}
          onReset={pageManagerStore.majorVacancyGrid.reset}
          onSave={pageManagerStore.majorVacancyGrid.validate}
          errors={pageManagerStore.majorVacancyGrid.errors}
          isDirty={pageManagerStore.majorVacancyGrid.isDirty}
          iconOptions={{ hideIcons: ['addRow'] }}
          isAdjusted={parseFloat(pageManagerStore.vacancyAf) !== 0}
        />
        <OperatingExpenses columns={columns} />
        <GenericSectionGrid<MajorOfficeProps>
          title="Capitalization Rates"
          GridOptions={{
            rowData: pageManagerStore.majorCapRatesGrid.initialData,
            columnDefs: columns,
            showRowNumber: false,
            showRemove: false,
          }}
          iconOptions={{ hideIcons: ['addRow'] }}
          onChange={pageManagerStore.majorCapRatesGrid.setData}
          onReset={pageManagerStore.majorCapRatesGrid.reset}
          onSave={pageManagerStore.majorCapRatesGrid.validate}
          errors={pageManagerStore.majorCapRatesGrid.errors}
          isDirty={pageManagerStore.majorCapRatesGrid.isDirty}
          isAdjusted={parseFloat(pageManagerStore.capRate) !== 0}
        />
      </div>
      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <GenericSectionGrid<MajorOfficeProps>
          title="Values ($/sqft.)"
          GridOptions={{
            rowData: pageManagerStore.majorValuesGrid.data.flatMap(
              (f) => f.data
            ),
            columnDefs: columns,
            showRemove: false,
            showRowNumber: false,
            editable: false,
            gridOptions: {
              getRowStyle: getBaseYearsStyle,
              defaultColDef: {
                editable: false,
              },
            },
          }}
          iconOptions={{ hideAll: true }}
          errors={pageManagerStore.majorValuesGrid.errors}
        />
        <PreviousYear<MajorOfficeProps, MajorOfficeProps>
          topOptions={{
            title: 'Previous values ($/sqft):',
            columnDefs: columns,
            rowData: pageManagerStore.majorPrevDollarGrid.data.flatMap(
              (d) => d.data
            ),
            onChange: pageManagerStore.majorPrevDollarGrid.setData,
            onSave: pageManagerStore.majorPrevDollarGrid.validate,
            onReset: pageManagerStore.majorPrevDollarGrid.reset,
            errors: pageManagerStore.majorPrevDollarGrid.errors,
            isDirty: pageManagerStore.majorPrevDollarGrid.isDirty,
            gridOptions: { getRowStyle: getBaseYearsStyle },
          }}
          bottomOptions={{
            title: 'Percent change from previous:',
            columnDefs: columns,
            rowData: pageManagerStore.majorPrevPercentGrid.data.flatMap(
              (d) => d.data
            ),
            onChange: pageManagerStore.majorPrevPercentGrid.setData,
            onSave: pageManagerStore.majorPrevPercentGrid.validate,
            onReset: pageManagerStore.majorPrevPercentGrid.reset,
            errors: pageManagerStore.majorPrevPercentGrid.errors,
            isDirty: pageManagerStore.majorPrevPercentGrid.isDirty,
            gridOptions: { getRowStyle: getBaseYearsStyle },
          }}
        />
      </div>
    </div>
  );
}

export default MajorOffice;
