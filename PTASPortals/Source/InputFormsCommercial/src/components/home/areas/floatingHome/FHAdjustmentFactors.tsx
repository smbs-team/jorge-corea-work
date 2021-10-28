// FloatingHomeAdjustmentFactors.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { InputBase, makeStyles } from '@material-ui/core';
import { Fragment } from 'react';
import NumberFormat from 'react-number-format';
import usePageManagerStore from 'stores/usePageManagerStore';

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
}));

function FloatingHomeAdjustmentFactors(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <Fragment>
      <table className={classes.table}>
        <tbody>
          <tr>
            <td className={classes.td}>Slip Value (%)</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                value={pageManagerStore.slipValueAf}
                onValueChange={(values) => {
                  pageManagerStore.setSlipValueAf(values.value);
                }}
                decimalScale={2}
                decimalSeparator="."
              />
            </td>
          </tr>
          <tr>
            <td className={classes.td}>RCN per SqFt (%)</td>
            <td className={classes.value}>
              <NumberFormat
                className={classes.inputBase}
                customInput={InputBase}
                value={pageManagerStore.replacementCostAf}
                onValueChange={(values) => {
                  pageManagerStore.setReplacementCostAf(values.value);
                }}
                decimalScale={2}
                decimalSeparator="."
              />
            </td>
          </tr>
        </tbody>
      </table>
    </Fragment>
  );
}

export default FloatingHomeAdjustmentFactors;
