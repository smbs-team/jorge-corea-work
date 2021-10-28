// CommAdjustmentFactors.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { InputBase, makeStyles } from '@material-ui/core';
import { RadioButtonGroup } from '@ptas/react-ui-library';
import useGetArea from 'hooks/useGetArea';
import { Fragment } from 'react';
import NumberFormat from 'react-number-format';
import { useLatest } from 'react-use';
import usePageManagerStore, { ValueModifier } from 'stores/usePageManagerStore';
import { applyDollarOrPercentAdjustmentFactor } from 'utils';

const useStyles = makeStyles((theme) => ({
  table: {
    borderCollapse: 'collapse',
    textAlign: 'center',
    marginTop: 5,
    '& td, th': {
      border: '1px solid #c0c0c0',
    },
    '& td': {
      paddingLeft: 8,
      paddingRight: 8,
    },
  },
  radioGroup: {
    flexDirection: 'row',
  },
  formControl: {
    marginRight: 12,
  },
  radioIcon: {
    width: 9,
    height: 9,
  },
  td: {
    backgroundColor: '#e5fff9',
  },
  inputBase: {
    '& .MuiInputBase-input': {
      padding: 0,
      textAlign: 'center',
    },
  },
  value: {
    width: 80,
    '&:focus-within': {
      backgroundColor: '#ececec',
    },
  },
  radioTd: {
    borderTop: 'hidden !important',
    borderRight: 'hidden !important',
    borderBottom: 'hidden !important',
  },
}));

const radioOptions = [
  { value: '1', label: 'Dollar' },
  { value: '2', label: 'Percentage' },
];

function CommAdjustmentFactors(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();
  const { name } = useGetArea();
  const latestLeaseRateData = useLatest(
    pageManagerStore.leaseRateGrid.initialData
  );
  const latestMajorLeaseRateData = useLatest(
    pageManagerStore.majorLeaseRateGrid.initialData
  );

  const applyLeaseRateAdjustment = (
    adjustment: string,
    leaseRateType?: ValueModifier
  ): void => {
    pageManagerStore.leaseRateGrid.setData(
      applyDollarOrPercentAdjustmentFactor(
        latestLeaseRateData.current,
        adjustment,
        leaseRateType ?? pageManagerStore.leaseRate,
        ['id', 'minEff', 'maxEff']
      )
    );
  };

  const applyMajorLeaseRateAdjustment = (
    adjustment: string,
    leaseRateType?: ValueModifier
  ): void => {
    pageManagerStore.majorLeaseRateGrid.setData(
      applyDollarOrPercentAdjustmentFactor(
        latestMajorLeaseRateData.current,
        adjustment,
        leaseRateType ?? pageManagerStore.leaseRate,
        ['id', 'empty']
      )
    );
  };

  return (
    <Fragment>
      <table className={classes.table}>
        <tbody>
          <tr>
            <td className={classes.td}>Lease Rate</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                value={pageManagerStore.leaseRateAf}
                onValueChange={(values) => {
                  pageManagerStore.setLeaseRateAf(values.value);
                  if (name === 'commercial')
                    applyLeaseRateAdjustment(values.value);
                  if (name === 'majorOffice')
                    applyMajorLeaseRateAdjustment(values.value);
                }}
                decimalScale={2}
                decimalSeparator="."
                thousandSeparator={pageManagerStore.leaseRate === 'dollar'}
                thousandsGroupStyle="thousand"
              />
            </td>
            <td className={classes.radioTd}>
              <RadioButtonGroup
                items={radioOptions}
                onChange={(value) => {
                  const currentValue = value === '1' ? 'dollar' : 'percent';
                  pageManagerStore.setLeaseRate(currentValue);

                  if (
                    name === 'commercial' &&
                    pageManagerStore.leaseRateGrid.initialData.length
                  ) {
                    applyLeaseRateAdjustment(
                      pageManagerStore.leaseRateAf,
                      currentValue
                    );
                  }

                  if (
                    name === 'majorOffice' &&
                    pageManagerStore.majorLeaseRateGrid.initialData.length
                  ) {
                    applyMajorLeaseRateAdjustment(
                      pageManagerStore.leaseRateAf,
                      currentValue
                    );
                  }
                }}
                classes={{
                  radioGroup: classes.radioGroup,
                  formControlLabelRoot: classes.formControl,
                  radioIcon: classes.radioIcon,
                }}
                value={pageManagerStore.leaseRate === 'dollar' ? '1' : '2'}
              />
            </td>
          </tr>
          <tr>
            <td className={classes.td}>Vacancy (%)</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                value={pageManagerStore.vacancyAf}
                onValueChange={(values) => {
                  if (!name) return;
                  pageManagerStore.setVacancyAf(values.value, name);
                }}
                decimalScale={2}
                decimalSeparator="."
              />
            </td>
          </tr>
          <tr>
            <td className={classes.td}>Operating Expenses</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                value={pageManagerStore.opExpensesAf}
                onValueChange={(values) => {
                  if (!name) return;
                  pageManagerStore.setOpExpensesAf(values.value, name);
                }}
                decimalScale={2}
                decimalSeparator="."
                thousandSeparator={
                  pageManagerStore.operatingExpenses === 'dollar'
                }
                thousandsGroupStyle="thousand"
              />
            </td>
            <td
              className={classes.radioTd}
              style={{
                opacity: 0.5,
              }}
            >
              <RadioButtonGroup
                items={radioOptions}
                onChange={() => {
                  //
                }}
                classes={{
                  radioGroup: classes.radioGroup,
                  formControlLabelRoot: classes.formControl,
                  radioIcon: classes.radioIcon,
                }}
                value={
                  pageManagerStore.operatingExpenses === 'dollar' ? '1' : '2'
                }
                radioProps={{ disabled: true }}
              />
            </td>
          </tr>
          <tr>
            <td className={classes.td}>Cap Rate (%)</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                decimalScale={2}
                decimalSeparator="."
                value={pageManagerStore.capRate}
                onValueChange={(values) => {
                  if (!name) return;
                  pageManagerStore.setCapRate(values.value, name);
                }}
              />
            </td>
          </tr>
        </tbody>
      </table>
    </Fragment>
  );
}

export default CommAdjustmentFactors;
