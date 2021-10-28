// Settings.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Checkbox, FormControlLabel, makeStyles } from '@material-ui/core';
import usePageManagerStore from 'stores/usePageManagerStore';

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
    fontWeight: 'bold',
    flexDirection: 'column',
  },
  checkBoxLabel: {
    marginTop: 16,
    marginLeft: -3,
    marginRight: 0,
  },
  checkBox: {
    padding: 0,
    paddingRight: 9,
  },
}));

function Settings(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <div className={classes.root}>
      <label>Settings</label>
      <FormControlLabel
        control={
          <Checkbox
            name="checkedB"
            color="primary"
            onChange={event => {
              pageManagerStore.setAutoCalculate(event.target.checked);
            }}
            checked={pageManagerStore.autoCalculate}
            className={classes.checkBox}
          />
        }
        label="Auto calculate"
        classes={{ root: classes.checkBoxLabel }}
      />
      <FormControlLabel
        control={
          <Checkbox
            name="checkedB"
            color="primary"
            onChange={event => {
              pageManagerStore.setCopySectionUses(event.target.checked);
            }}
            checked={pageManagerStore.copySectionUses}
            className={classes.checkBox}
          />
        }
        label="Copy section uses on new page"
        classes={{ root: classes.checkBoxLabel }}
      />
    </div>
  );
}

export default Settings;
