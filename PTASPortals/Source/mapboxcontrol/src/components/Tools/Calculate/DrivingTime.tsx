// DrivingTime.tsx
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
  ErrorMessageAlertCtx 
} from '@ptas/react-ui-library';
import BasePanel from 'components/BasePanel';
import { useCalculateStyles } from './styles';
import { useParcelNavigation } from './useParcelNavigation';
import ParcelDataTable from './ParcelDataTable';
import { exportToExcel } from './common';
import clsx from 'clsx';
import { useGlobalStyles } from 'hooks';
import { utilService } from 'services/common';
import {
  CalculateContext,
  CalculateParcelsOp,
} from 'contexts/CalculateContext';
import { useUnmount } from 'react-use';
import { PinService } from 'services/map/pin/PinService';

type Props = WithStyles<typeof useCalculateStyles>;

/**
 * Component to show driving times to parcels
 */

const DrivingTime = (props: Props): JSX.Element => {
  const { classes } = props;
  const {
    order,
    setOrder,
    orderBy,
    setOrderBy,
    destinationParcelRadioItems,
    headCells,
    onClickSetStartingPoint,
    onClickClearData,
    onClickCalculate,
    onTextFieldValueChange,
    onSuggestionSelected,
    autoCompleteData,
    onIconClick,
    clickOnDirectionRow,
  } = useParcelNavigation({
    navigationType: 'driving',
    component: <DrivingTimeExp />,
  });
  const { dtStartPointOption, setDtStartPointOption ,setFetching} = useContext(
    CalculateContext
  );
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx );
  const {
    dtDestinationParcelOption,
    setDtDestinationParcelOption,
    drivingTimeParcelRows,
    drivingTimeAddress,
  } = useContext(CalculateContext);
  const globalStyles = useGlobalStyles();

  const handleExport = async (): Promise<void> => {
    if (!drivingTimeParcelRows) return;
    setFetching(true);
    const rows = drivingTimeParcelRows.map(d => {
      return [d.parcel, `${d.time} min`, d.address];
    });

    try {
      await exportToExcel(headCells, rows, 'driving time');
      setFetching(false);
    } catch (error) {
      setFetching(false);
      if (error instanceof Error) {
        showErrorMessage(error.stack)
      } else {
        showErrorMessage(JSON.stringify(error))
      }
    }
  };

  useUnmount(() => {
    if (!PinService.hasMarker()) {
      setDtStartPointOption(false);
    }
  });

  return (
    <BasePanel
      disableScrollY
      toolbarItems={
        <Fragment>
          <PanelHeader
            route={[<label key={2}>Driving time</label>]}
            icons={[
              {
                text: dtStartPointOption ? 'Cancel' : 'Set starting point',
                icon: <Refresh />,
                onClick: onClickSetStartingPoint,
              },
              {
                text: 'Calculate driving time',
                icon: <Refresh />,
                onClick: onClickCalculate,
                disabled: !PinService.hasMarker(),
              },
              {
                text: 'Clear',
                icon: <Clear />,
                onClick: onClickClearData,
                disabled: !drivingTimeParcelRows.length,
              },
              {
                text: 'Export',
                icon: <GetApp />,
                onClick: handleExport,
                disabled: !drivingTimeParcelRows.length,
              },
            ]}
          />
        </Fragment>
      }
    >
      <Box className={classes.root}>
        {dtStartPointOption && (
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
            AutocompleteProps={{ value: drivingTimeAddress }}
            onIconClick={onIconClick}
          />
        </Box>
        <Box className={classes.contentRow}>
          <Box className={classes.separator}>
            <RadioButtonGroup
              items={destinationParcelRadioItems}
              onChange={(event): void => {
                setDtDestinationParcelOption(event as CalculateParcelsOp);
              }}
              defaultValue={dtDestinationParcelOption}
              classes={{ formControlLabelRoot: classes.radio }}
            />
          </Box>
          <Box className={classes.tableContainer}>
            <ParcelDataTable
              drivingTimeParcelRows={drivingTimeParcelRows}
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

const DrivingTimeExp = memo(withStyles(useCalculateStyles)(DrivingTime));
export default DrivingTimeExp;
