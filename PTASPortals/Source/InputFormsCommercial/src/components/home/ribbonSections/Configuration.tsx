// Configuration.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import { SimpleDropDown } from '@ptas/react-ui-library';
import { Fragment } from 'react';
import usePageManagerStore, {
  StratificationType,
  ValueModifier,
} from 'stores/usePageManagerStore';

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
    fontWeight: 'bold',
    flexDirection: 'column',
  },
  dropdown: {
    marginTop: 16,
    width: 235,
  },
  dropdownInputRoot: {
    height: 38,
  },
}));

function Configuration(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();

  return (
    <Fragment>
      <div className={classes.root}>
        <label>Configuration Type</label>
        <SimpleDropDown
          items={[
            { label: 'Building Quality', value: 'buildingQuality' },
            { label: 'Leasing Class', value: 'leasingClass' },
          ]}
          onSelected={s =>
            pageManagerStore.setStratificationType(
              s.value as StratificationType
            )
          }
          label="Stratification Type"
          classes={{
            root: classes.dropdown,
            inputRoot: classes.dropdownInputRoot,
          }}
          value={pageManagerStore.stratificationType}
        />
        <SimpleDropDown
          items={[
            {
              label: '% of Effective Gross Income',
              value: 'percent',
            },
            {
              label: '$ per squarefoot',
              value: 'dollar',
            },
          ]}
          label="Operating Expenses"
          onSelected={s =>
            pageManagerStore.setOperatingExpenses(s.value as ValueModifier)
          }
          classes={{
            root: classes.dropdown,
            inputRoot: classes.dropdownInputRoot,
          }}
          value={pageManagerStore.operatingExpenses}
        />
      </div>
    </Fragment>
  );
}

export default Configuration;
