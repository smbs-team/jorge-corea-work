// RentableArea.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import { CustomNumericField } from '@ptas/react-ui-library';
import usePageManagerStore from 'stores/usePageManagerStore';

const useStyles = makeStyles(() => ({
  root: {
    display: 'flex',
    fontWeight: 'bold',
    flexDirection: 'column',
  },
  input: {
    marginTop: 16,
    width: 100,
  },
  marginDense: {
    paddingTop: 9.5,
    paddingBottom: 9.5,
  },
}));

function RentableArea(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <div className={classes.root}>
      <label>Net Rentable Area</label>
      <CustomNumericField
        label="Min SF"
        className={classes.input}
        defaultValue={pageManagerStore.minSf}
        onBlur={(event) => pageManagerStore.setMinSf(event.currentTarget.value)}
        NumericProps={{ thousandSeparator: true }}
        InputProps={{ classes: { inputMarginDense: classes.marginDense } }}
      />
      <CustomNumericField
        label="Max SF"
        className={classes.input}
        defaultValue={pageManagerStore.maxSf}
        onBlur={(event) => pageManagerStore.setMaxSf(event.currentTarget.value)}
        NumericProps={{ thousandSeparator: true }}
        InputProps={{ classes: { inputMarginDense: classes.marginDense } }}
      />
    </div>
  );
}

export default RentableArea;
