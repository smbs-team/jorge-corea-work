// WalkingDistance.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { memo, Fragment, useContext, useEffect } from 'react';
import { withStyles, WithStyles, Box } from '@material-ui/core';
import { Refresh, GetApp, Clear } from '@material-ui/icons';
import {
  PanelHeader,
  RadioButtonGroup,
  AutoComplete,
  SnackContext,
} from '@ptas/react-ui-library';
import BasePanel from 'components/BasePanel';
import { useCalculateStyles } from './styles';
import ParcelDataTable from './ParcelDataTable';
import { useParcelNavigation } from './useParcelNavigation';
import { HomeContext } from 'contexts';
import { exportToExcel } from './common';
import clsx from 'clsx';
import { useGlobalStyles } from 'hooks';
import { utilService } from 'services/common';
import { PinService } from 'services/map/pin/PinService';
import {
  CalculateContext,
  CalculateParcelsOp,
} from 'contexts/CalculateContext';
import { useUnmount } from 'react-use';

type Props = WithStyles<typeof useCalculateStyles>;

/**
 * Component to show walking distances to parcels
 */

const WalkingDistance = (props: Props): JSX.Element => {
  const { classes } = props;
  const {
    order,
    setOrder,
    orderBy,
    setOrderBy,
    destinationParcelRadioItems,
    headCells,
    onClickSetStartingPoint,
    onClickCalculate,
    onClickClearData,
    onTextFieldValueChange,
    onSuggestionSelected,
    autoCompleteData,
    onIconClick,
    clickOnDirectionRow,
  } = useParcelNavigation({
    navigationType: 'walking',
    component: <WalkingDistanceExp />,
  });
  const { wdStartPointOption, setWdStartPointOption ,fetching,setFetching} = useContext(
    CalculateContext
  );

  const {
    wdDestinationParcelOption,
    setWdDestinationParcelOption,
    walkingDistanceParcelRows,
    walkingDistanceAddress,
  } = useContext(CalculateContext);
  const { setSnackState } = useContext(SnackContext);
  const globalStyles = useGlobalStyles();

  const handleExport = async (): Promise<void> => {
    if (!walkingDistanceParcelRows) return;
    setFetching(true);

    const rows = walkingDistanceParcelRows.map(p => {
      return [p.parcel, p.distance + ' ft', p.address];
    });

    try {
      await exportToExcel(headCells, rows, 'walking distance');
      setFetching(false);
    } catch (error) {
      setFetching(false);
      setSnackState({
        severity: 'error',
        text: 'Error getting file to download',
      });
    }
  };

  useUnmount(() => {
    if (!PinService.hasMarker()) {
      setWdStartPointOption(false);
    }
  });

  return (
    <BasePanel
      disableScrollY
      toolbarItems={
        <Fragment>
          <PanelHeader
            route={[<label key={2}>Walking distance</label>]}
            icons={[
              {
                text: wdStartPointOption ? 'Cancel' : 'Set starting point',
                icon: <Refresh />,
                onClick: onClickSetStartingPoint,
              },
              {
                text: 'Calculate walking distance',
                icon: <Refresh />,
                onClick: onClickCalculate,
                disabled: !PinService.hasMarker(),
              },
              {
                text: 'Clear',
                icon: <Clear />,
                onClick: onClickClearData,
                disabled: !walkingDistanceParcelRows.length,
              },
              {
                text: 'Export',
                icon: <GetApp />,
                onClick: handleExport,
                disabled: !walkingDistanceParcelRows.length,
              },
            ]}
          />
        </Fragment>
      }
    >
      <Box className={classes.root}>
        {wdStartPointOption && (
          <Box className={classes.row}>
            {'Click on the map to get the start point coordinates.'}
          </Box>
        )}
        <Box className={classes.row}>
          <AutoComplete
            data={autoCompleteData.map(item => ({
              title: item.addressFull,
            }))}
            onValueChanged={onSuggestionSelected}
            onTextFieldValueChange={onTextFieldValueChange}
            classes={{
              textField: clsx(classes.startingPoint, globalStyles.basicInput),
            }}
            label="From this point"
            AutocompleteProps={{ value: walkingDistanceAddress }}
            onIconClick={onIconClick}
          />
        </Box>
        <Box className={classes.contentRow}>
          <Box className={classes.separator}>
            <RadioButtonGroup
              items={destinationParcelRadioItems}
              onChange={(event: string): void => {
                setWdDestinationParcelOption(event as CalculateParcelsOp);
              }}
              defaultValue={wdDestinationParcelOption}
              classes={{ formControlLabelRoot: classes.radio }}
            />
          </Box>
          <Box className={classes.tableContainer}>
            <ParcelDataTable
              walkingDistanceParcelRows={walkingDistanceParcelRows}
              headCells={headCells}
              order={order}
              setOrder={setOrder}
              orderBy={orderBy}
              setOrderBy={setOrderBy}
              numberWithCommas={utilService.numberWithCommas}
              clickOnDirectionRow={clickOnDirectionRow}
            />
          </Box>
        </Box>
      </Box>
    </BasePanel>
  );
};

const WalkingDistanceExp = memo(
  withStyles(useCalculateStyles)(WalkingDistance)
);
export default WalkingDistanceExp;
