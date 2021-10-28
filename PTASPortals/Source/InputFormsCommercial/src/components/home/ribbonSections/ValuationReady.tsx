// ValuationReady.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Checkbox, FormControlLabel, makeStyles } from '@material-ui/core';
import usePageManagerStore from 'stores/usePageManagerStore';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    fontWeight: 'bold',
    flexDirection: 'column',
    width: 190
  },
  checkBoxLabel: {
    marginTop: 16,
    marginLeft: -3,
    marginRight: 0
  },
  statusDot: {
    borderRadius: '50%',
    width: 17,
    height: 17,
    marginTop: 4,
    marginLeft: 86.1585
  },
  lastSync: {
    marginTop: 16,
    fontSize: "0.875rem",
    fontWeight: "normal"
  },
  checkBox: {
    padding: 0,
    paddingRight: 9
  }
}));

function ValuationReady(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <div className={classes.root}>
      <label>Is Page Ready?</label>
      <FormControlLabel
        control={
          <Checkbox
            name="checkedB"
            color="primary"
            onChange={(event) => {
              pageManagerStore.setIsReady(event.target.checked);
            }}
            checked={pageManagerStore.isReady}
            className={classes.checkBox}
          />
        }
        label="Ready for valuation"
        classes={{ root: classes.checkBoxLabel }}
      />
      <div
        className={classes.statusDot}
        style={{
          backgroundColor: pageManagerStore.isReady ? '#72bb53' : '#ff3823',
        }}
      />
      <div className={classes.lastSync}>
        Last sync on 8/2/2020 at 7:43pm, by Adam Neel
      </div>
    </div>
  );
}

export default ValuationReady;
