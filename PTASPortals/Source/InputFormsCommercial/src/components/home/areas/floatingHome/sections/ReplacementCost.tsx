// ReplacementCost.tsx
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

const useStyles = makeStyles((theme) => ({
  sectionWrapper: {
    width: '100%',
  },
  sectionContentWrapper: {
    alignItems: 'center',
  },
}));

export interface ReplacementCostProps {
  avgMinus: string;
  avg: string;
  avgPlus: string;
  goodMinus: string;
  good: string;
  goodPlus: string;
  excellentMinus: string;
  excellent: string;
  excellentPlus: string;
}

const columns = [
  { field: 'avgMinus', headerName: 'Average (-)' },
  { field: 'avg', headerName: 'Average' },
  { field: 'avgPlus', headerName: 'Average (+)' },
  { field: 'goodMinus', headerName: 'Good (-)' },
  { field: 'good', headerName: 'Good' },
  { field: 'goodPlus', headerName: 'Good (+)' },
  { field: 'excellentMinus', headerName: 'Excellent (-)' },
  { field: 'excellent', headerName: 'Excellent' },
  { field: 'excellentPlus', headerName: 'Excellent (+)' },
];

function ReplacementCost(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <GridSection
      title="Replacement Cost New per Square Foot ($/SqFt)"
      icons={[
        {
          icon: <InsertDriveFileIcon />,
          text: 'Save',
          onClick: pageManagerStore.replacementCostGrid.validate,
        },
        {
          icon: <CachedIcon />,
          text: 'Reset',
          onClick: pageManagerStore.replacementCostGrid.reset,
        },
      ]}
      classes={{
        sectionWrapper: classes.sectionWrapper,
        sectionContentWrapper: classes.sectionContentWrapper,
      }}
      miscContent={
        <Fragment>
          {pageManagerStore.replacementCostGrid.isDirty ? (
            !pageManagerStore.replacementCostGrid.errors.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined}
          {parseFloat(pageManagerStore.replacementCostAf) !== 0 && (
            <BuildIcon style={{ marginLeft: 8 }} />
          )}
        </Fragment>
      }
    >
      <Box width={1050}>
        <CommercialAgGrid
          rowData={pageManagerStore.replacementCostGrid.data.flatMap(
            (d) => d.data
          )}
          columnDefs={columns}
          gridOptions={{
            defaultColDef: {
              sortable: false,
              editable: true,
            },
          }}
          onChange={pageManagerStore.replacementCostGrid.setData}
          errors={pageManagerStore.replacementCostGrid.errors}
        />
      </Box>
      <PreviousYear<ReplacementCostProps, ReplacementCostProps>
        topOptions={{
          title: 'Previous replacement cost new per square foot ($/sqft.)',
          columnDefs: columns,
          width: '1050px',
          gridOptions: {
            defaultColDef: { cellStyle: { backgroundColor: '#e5e5e5' } },
          },
          rowData:
            pageManagerStore.replacementCostPrevYearDollarGrid.initialData,
          onChange: pageManagerStore.replacementCostPrevYearDollarGrid.setData,
          onSave: pageManagerStore.replacementCostPrevYearDollarGrid.validate,
          onReset: pageManagerStore.replacementCostPrevYearDollarGrid.reset,
          errors: pageManagerStore.replacementCostPrevYearDollarGrid.errors,
          isDirty: pageManagerStore.replacementCostPrevYearDollarGrid.isDirty,
        }}
        bottomOptions={{
          title: 'Percent change from previous',
          columnDefs: columns,
          width: '1050px',
          gridOptions: {
            defaultColDef: { cellStyle: { backgroundColor: '#e5e5e5' } },
          },
          rowData:
            pageManagerStore.replacementCostPrevYearPercentGrid.initialData,
          onChange: pageManagerStore.replacementCostPrevYearPercentGrid.setData,
          onSave: pageManagerStore.replacementCostPrevYearPercentGrid.validate,
          onReset: pageManagerStore.replacementCostPrevYearPercentGrid.reset,
          errors: pageManagerStore.replacementCostPrevYearPercentGrid.errors,
          isDirty: pageManagerStore.replacementCostPrevYearPercentGrid.isDirty,
        }}
      />
    </GridSection>
  );
}

export default ReplacementCost;
