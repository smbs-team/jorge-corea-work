// HotelsConfig.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import { DropDownItem, SimpleDropDown } from '@ptas/react-ui-library';
import { orderBy } from 'lodash';
import { Fragment, useState } from 'react';
import { useEffectOnce } from 'react-use';
import useAreaTreeStore from 'stores/useAreaTreeStore';
import usePageManagerStore from 'stores/usePageManagerStore';

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
    fontWeight: 'bold',
    flexDirection: 'column',
  },
  dropdown: {
    marginTop: 16,
    width: 162,
  },
}));

function HotelsConfig(): JSX.Element {
  const classes = useStyles();
  const areaTreeStore = useAreaTreeStore();
  const pageMagerStore = usePageManagerStore();
  const [data, setData] = useState<DropDownItem[]>([
    { label: 'All', value: '0' },
  ]);

  useEffectOnce(() => {
    const list: DropDownItem[] = data;
    areaTreeStore.hotelNbhds.forEach((n, i) => {
      list.push({ label: n, value: i + 1 });
    });
    setData(orderBy(list, 'label', 'desc'));
  });

  return (
    <Fragment>
      <div className={classes.root}>
        <label>Options</label>
        <SimpleDropDown
          items={data}
          onSelected={pageMagerStore.setHotelNbhd}
          label="Apply to Neighborhood"
          classes={{ root: classes.dropdown }}
          value={pageMagerStore.hotelNbhd?.value ?? '0'}
        />
      </div>
    </Fragment>
  );
}

export default HotelsConfig;
