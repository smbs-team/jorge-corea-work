// OverlapCalculator.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { memo, Fragment, useState, useContext } from 'react';
import {
  withStyles,
  WithStyles,
  Box,
  TableContainer,
  Table,
  TableBody,
  TableRow,
  TableCell,
  TableHead,
  TableSortLabel,
} from '@material-ui/core';
import { Refresh, GetApp, KeyboardArrowDown } from '@material-ui/icons';
import {
  PanelHeader,
  SimpleDropDown,
  CustomTextField,
  RadioButtonGroup,
  CustomModal,
  ErrorMessageAlertCtx 
} from '@ptas/react-ui-library';
import BasePanel from 'components/BasePanel';
import { useCalculateStyles } from './styles';
import {
  useOverlapCalculator,
  OverlapCalculatorData,
} from './useOverlapCalculator';
import { orderBy as _orderBy } from 'lodash';
import { AdditionalFieldsPopup } from './AdditionalFieldsPopup';
import { HomeContext } from 'contexts';
import { exportToExcel } from './common';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';

type Props = WithStyles<typeof useCalculateStyles>;

/**
 * A component that calculates distances
 */

const OverlapCalculator = (props: Props): JSX.Element => {
  const { classes } = props;
  const {
    order,
    setOrder,
    orderBy,
    setOrderBy,
    parcelRows,
    fillLayers,
    selectedFillLayer,
    setSelectedFillLayer,
    increaseBy,
    setIncreaseBy,
    headCells,
    destinationParcelOption,
    setDestinationParcelOption,
    destinationParcelRadioItems,
    onClickCalculate,
  } = useOverlapCalculator();
  const [currentRow, setCurrentRow] = useState<OverlapCalculatorData>();
  const [openModal, setOpenModal] = useState<boolean>(false);
  const { showErrorMessage} = useContext(ErrorMessageAlertCtx );
  const { setLinearProgress } = useContext(HomeContext);

  const handleExport = async (): Promise<void> => {
    if (!parcelRows) return;
    setLinearProgress(true);

    const rows = parcelRows.map(p => {
      return [p.parcel, p.overlap, p.percentage];
    });

    try {
      await exportToExcel(headCells, rows, 'overlaps');
      setLinearProgress(false);
    } catch (error) {
      setLinearProgress(false);
      if (error instanceof Error) {
        showErrorMessage(error.stack)
      } else {
        showErrorMessage(JSON.stringify(error))
      }
   
    }
  };

  return (
    <BasePanel
      disableScrollY
      toolbarItems={
        <Fragment>
          <PanelHeader
            route={[
              <label key={1}>Tools</label>,
              <label key={2}>Overlap calculator</label>,
            ]}
            icons={[
              {
                text: 'Calculate overlap',
                icon: <Refresh />,
                onClick: (): void => {
                  onClickCalculate();
                },
              },
              {
                text: 'Export',
                icon: <GetApp />,
                onClick: handleExport,
                disabled: !parcelRows || parcelRows.length < 1,
              },
            ]}
          />
        </Fragment>
      }
    >
      <Box className={classes.root}>
        <Box className={classes.overlapCalcContentRow}>
          <Box className={classes.separator}>
            <Box className={`${classes.flex} ${classes.upperBox}`}>
              <SimpleDropDown
                onSelected={(selected): void => {
                  setSelectedFillLayer(selected.value);
                }}
                items={fillLayers}
                value={selectedFillLayer ?? ''}
                label="Intersect layer"
                classes={{ root: classes.intersect }}
              />
              <CustomTextField
                label="Increase by X feet"
                defaultValue={increaseBy}
                type="number"
                onChange={(event): void => {
                  setIncreaseBy(+event.target.value);
                }}
                classes={{ root: classes.increase }}
              />
            </Box>
            <RadioButtonGroup
              items={destinationParcelRadioItems}
              defaultValue={destinationParcelOption}
              onChange={(event: string): void => {
                setDestinationParcelOption(
                  event === 'selected' ? event : 'checked'
                );
              }}
              classes={{ formControlLabelRoot: classes.radio }}
            />
          </Box>
          <Box className={classes.tableContainer}>
            <TableContainer>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    {headCells.map((cell, index) => (
                      <TableCell
                        key={cell.id + index}
                        sortDirection={orderBy === cell.id ? order : false}
                        classes={{ root: classes.tableHeadCell }}
                      >
                        <TableSortLabel
                          active={orderBy === cell.id}
                          direction={orderBy === cell.id ? order : 'asc'}
                          onClick={(): void => {
                            const ascending =
                              orderBy === cell.id && order === 'asc';
                            setOrder(ascending ? 'desc' : 'asc');
                            setOrderBy(cell.id as keyof OverlapCalculatorData);
                          }}
                          IconComponent={KeyboardArrowDown}
                        >
                          {cell.label}
                        </TableSortLabel>
                      </TableCell>
                    ))}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {_orderBy(parcelRows, orderBy, order).map((row, index) => {
                    return (
                      <TableRow key={index}>
                        <TableCell classes={{ root: classes.tableCell }}>
                          {row.parcel}
                        </TableCell>
                        <TableCell classes={{ root: classes.tableCell }}>
                          {row.overlap}
                        </TableCell>
                        <TableCell classes={{ root: classes.tableCell }}>
                          {row.percentage}
                        </TableCell>
                        <TableCell classes={{ root: classes.tableCell }}>
                          {/* {row.data} */}
                          <span
                            style={{ cursor: 'pointer', display: 'flex' }}
                            onClick={(): void => {
                              setCurrentRow(row);
                              setOpenModal(true);
                            }}
                          >
                            <label
                              style={{ cursor: 'pointer', marginRight: 4 }}
                            >
                              Click for more information
                            </label>
                            <InfoOutlinedIcon />
                          </span>
                        </TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </TableContainer>
            <CustomModal
              isOpen={openModal}
              onClose={(): void => setOpenModal(false)}
            >
              {currentRow && <AdditionalFieldsPopup row={currentRow} />}
            </CustomModal>
          </Box>
        </Box>
      </Box>
    </BasePanel>
  );
};

export default memo(withStyles(useCalculateStyles)(OverlapCalculator));
