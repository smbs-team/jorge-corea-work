// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import OperatingExpenses from './sections/OperatingExpenses';
import { useEffectOnce, useWindowSize } from 'react-use';
import { SectionUses } from 'components/shared';
import useRibbonStore from 'stores/useRibbonStore';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import PreviousYear from 'components/shared/PreviousYear';
import { isEdge } from 'react-device-detect';
import usePageManagerStore from 'stores/usePageManagerStore';
import { getCellStyleResults } from 'components/shared/gridUtility';
import GenericSectionGrid from 'components/shared/GenericSectionGrid';
import { IncomeRates } from '../../types';

export interface CommercialIncomeProps extends IncomeRates {
  minEff?: string;
  maxEff?: string;
}

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
  { field: 'grade1', headerName: 'Low' },
  { field: 'grade2', headerName: 'Low/Av.' },
  { field: 'grade3', headerName: 'Average' },
  { field: 'grade4', headerName: 'Av./Good' },
  { field: 'grade5', headerName: 'Good' },
  { field: 'grade6', headerName: 'Good/Ex.' },
  { field: 'grade7', headerName: 'Excellent' },
];

function Income(): JSX.Element {
  const ribbonStore = useRibbonStore();
  const headerStore = useContentHeaderStore();
  const { width } = useWindowSize();
  const pageManagerStore = usePageManagerStore();

  useEffectOnce(() => {
    ribbonStore.setDefault();
    headerStore.setHidePagination(false);
    headerStore.setHideYear(false);
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
        <SectionUses showRentableArea />
        <GenericSectionGrid<CommercialIncomeProps>
          title={`Lease Rates ($/sqft.)`}
          GridOptions={{
            rowData: pageManagerStore.leaseRateGrid.data.flatMap((d) => d.data),
            columnDefs: columns,
            gridOptions: {
              defaultColDef: { cellEditorParams: { roundBy: 0.05 } },
            },
          }}
          onSave={pageManagerStore.leaseRateGrid.validate}
          errors={pageManagerStore.leaseRateGrid.errors}
          isDirty={pageManagerStore.leaseRateGrid.isDirty}
          onChange={pageManagerStore.leaseRateGrid.setData}
          iconOptions={{ addRowText: 'Add effective year range' }}
          onReset={() => pageManagerStore.leaseRateGrid.reset()}
          isAdjusted={parseFloat(pageManagerStore.leaseRateAf) !== 0}
        />
        <GenericSectionGrid<CommercialIncomeProps>
          title="Vacancy (%)"
          GridOptions={{
            rowData: pageManagerStore.vacancyGrid.data.flatMap((d) => d.data),
            columnDefs: columns,
            gridOptions: {
              defaultColDef: {
                cellEditorParams: {
                  thousandSeparator: false,
                  decimalScale: 2,
                },
              },
            },
          }}
          onSave={pageManagerStore.vacancyGrid.validate}
          onChange={(rows) => pageManagerStore.vacancyGrid.setData(rows)}
          onReset={() => pageManagerStore.vacancyGrid.reset()}
          isDirty={pageManagerStore.vacancyGrid.isDirty}
          iconOptions={{ addRowText: 'Add effective year range' }}
          errors={pageManagerStore.vacancyGrid.errors}
          isAdjusted={parseFloat(pageManagerStore.vacancyAf) !== 0}
        />
        <OperatingExpenses columns={columns}/>
        <GenericSectionGrid<CommercialIncomeProps>
          title="Capitalization Rates"
          GridOptions={{
            rowData: pageManagerStore.capRatesGrid.data.flatMap((d) => d.data),
            columnDefs: columns,
          }}
          onSave={() => pageManagerStore.capRatesGrid.validate}
          onChange={(rows) => pageManagerStore.capRatesGrid.setData(rows)}
          onReset={() => pageManagerStore.capRatesGrid.reset()}
          errors={pageManagerStore.capRatesGrid.errors}
          iconOptions={{ addRowText: 'Add effective year range' }}
          isDirty={pageManagerStore.capRatesGrid.isDirty}
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
        <GenericSectionGrid<CommercialIncomeProps>
          title="Values ($/sqft.)"
          GridOptions={{
            rowData: pageManagerStore.valuesGrid.data.flatMap((d) => d.data),
            columnDefs: columns,
            showRemove: false,
            showRowNumber: false,
          }}
          iconOptions={{ hideAll: true }}
          errors={pageManagerStore.valuesGrid.errors}
        />
        <PreviousYear<CommercialIncomeProps, CommercialIncomeProps>
          topOptions={{
            title: 'Previous values ($/sqft):',
            columnDefs: columns,
            rowData: pageManagerStore.prevDollarGrid.data.flatMap(
              (d) => d.data
            ),
            onChange: pageManagerStore.prevDollarGrid.setData,
            onSave: pageManagerStore.prevDollarGrid.validate,
            onReset: pageManagerStore.prevDollarGrid.reset,
            errors: pageManagerStore.prevDollarGrid.errors,
            isDirty: pageManagerStore.prevDollarGrid.isDirty,
            gridOptions: {
              defaultColDef: {
                cellStyle: getCellStyleResults,
              },
            },
          }}
          bottomOptions={{
            title: 'Percent change from previous:',
            columnDefs: columns,
            rowData: pageManagerStore.prevPercentGrid.data.flatMap(
              (d) => d.data
            ),
            onChange: pageManagerStore.prevPercentGrid.setData,
            onSave: pageManagerStore.prevPercentGrid.validate,
            onReset: pageManagerStore.prevPercentGrid.reset,
            errors: pageManagerStore.prevPercentGrid.errors,
            isDirty: pageManagerStore.prevPercentGrid.isDirty,
            gridOptions: {
              defaultColDef: {
                cellStyle: getCellStyleResults,
              },
            },
          }}
        />
      </div>
    </div>
  );
}

export default Income;
