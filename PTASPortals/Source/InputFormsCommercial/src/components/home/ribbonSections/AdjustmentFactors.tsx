// AdjustmentFactors.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import useGetArea from 'hooks/useGetArea';
import { Fragment } from 'react';
import CommAdjustmentFactors from '../areas/commercial/CommAdjustmentFactors';
import FloatingHomeAdjustmentFactors from '../areas/floatingHome/FHAdjustmentFactors';

const useStyles = makeStyles((theme) => ({
  options: {
    display: 'flex',
    marginTop: 9,
  },
}));

function AdjustmentFactors(): JSX.Element {
  const classes = useStyles();
  const { name } = useGetArea();

  return (
    <Fragment>
      <label style={{ fontWeight: 'bold' }}>Adjustment Factors</label>
      <div className={classes.options}></div>
      {(name === 'commercial' || name === 'majorOffice') && (
        <CommAdjustmentFactors />
      )}
      {name === 'floatingHome' && <FloatingHomeAdjustmentFactors />}
    </Fragment>
  );
}

export default AdjustmentFactors;
