// SlipValues.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CommercialAgGrid } from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import { Box, makeStyles } from '@material-ui/core';
import CachedIcon from '@material-ui/icons/Cached';
import PreviousYear from 'components/shared/PreviousYear';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import usePageManagerStore from 'stores/usePageManagerStore';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import CancelIcon from '@material-ui/icons/Cancel';
import { Fragment } from 'react';
import BuildIcon from '@material-ui/icons/Build';

export interface MobileHomeProps {
  empty: string;
  grade1: string;
  grade2: string;
  grade3: string;
  grade4: string;
  grade5: string;
  grade6: string;
  grade7: string;
}

const useStyles = makeStyles((theme) => ({
  sectionWrapper: {
    width: '100%',
  },
  sectionContentWrapper: {
    alignItems: 'center',
  },
}));

 const columns = [
  {
    field: 'empty',
    headerName: '',
    cellStyle: { backgroundColor: '#e5fff9' },
    editable: false,
  },
  { field: 'grade1', headerName: 'Grade 1' },
  { field: 'grade2', headerName: 'Grade 2' },
  { field: 'grade3', headerName: 'Grade 3' },
  { field: 'grade4', headerName: 'Grade 4' },
  { field: 'grade5', headerName: 'Grade 5' },
  { field: 'grade6', headerName: 'Grade 6' },
  { field: 'grade7', headerName: 'Grade 7' },
];

export const previousColumns = [
  { field: 'empty', headerName: '', cellStyle: { backgroundColor: '#e5fff9' } },
  {
    field: 'grade1',
    headerName: 'Grade 1',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade2',
    headerName: 'Grade 2',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade3',
    headerName: 'Grade 3',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade4',
    headerName: 'Grade 4',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade5',
    headerName: 'Grade 5',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade6',
    headerName: 'Grade 6',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
  {
    field: 'grade7',
    headerName: 'Grade 7',
    cellStyle: { backgroundColor: '#e5e5e5' },
  },
];

function SlipValues(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <GridSection
      title="Slip Values ($)"
      icons={[
        {
          icon: <InsertDriveFileIcon />,
          text: 'Save',
          onClick: pageManagerStore.slipValuesGrid.validate,
        },
        { icon: <span />, text: 'View Slip Grade Descriptions' },
        {
          icon: <CachedIcon />,
          text: 'Reset',
          onClick: pageManagerStore.slipValuesGrid.reset,
        },
      ]}
      classes={{
        sectionWrapper: classes.sectionWrapper,
        sectionContentWrapper: classes.sectionContentWrapper,
      }}
      miscContent={
        <Fragment>
          {pageManagerStore.slipValuesGrid.isDirty ? (
            !pageManagerStore.slipValuesGrid.errors.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined}
          {parseFloat(pageManagerStore.slipValueAf) !== 0 && <BuildIcon style={{ marginLeft: 8 }} />}
        </Fragment>
      }
    >
      <Box width={870}>
        <CommercialAgGrid
          rowData={pageManagerStore.slipValuesGrid.data.flatMap((d) => d.data)}
          columnDefs={columns}
          gridOptions={{
            defaultColDef: {
              sortable: false,
              editable: true,
            },
          }}
          onChange={pageManagerStore.slipValuesGrid.setData}
          errors={pageManagerStore.slipValuesGrid.errors}
        />
      </Box>
      <PreviousYear<MobileHomeProps, MobileHomeProps>
        topOptions={{
          title: 'Previous replacement cost new per square foot ($/sqft.)',
          columnDefs: previousColumns,
          width: '1050px',
          rowData: pageManagerStore.slipPrevYearDollarGrid.initialData,
          onChange: pageManagerStore.slipPrevYearDollarGrid.setData,
          onSave: pageManagerStore.slipPrevYearDollarGrid.validate,
          onReset: pageManagerStore.slipPrevYearDollarGrid.reset,
          errors: pageManagerStore.slipPrevYearDollarGrid.errors,
          isDirty: pageManagerStore.slipPrevYearDollarGrid.isDirty,
        }}
        bottomOptions={{
          title: 'Percent change from previous',
          columnDefs: previousColumns,
          width: '1050px',
          rowData: pageManagerStore.slipPrevYearPercentGrid.initialData,
          onChange: pageManagerStore.slipPrevYearPercentGrid.setData,
          onSave: pageManagerStore.slipPrevYearPercentGrid.validate,
          onReset: pageManagerStore.slipPrevYearPercentGrid.reset,
          errors: pageManagerStore.slipPrevYearPercentGrid.errors,
          isDirty: pageManagerStore.slipPrevYearPercentGrid.isDirty,
        }}
      />
    </GridSection>
  );
}

export default SlipValues;
