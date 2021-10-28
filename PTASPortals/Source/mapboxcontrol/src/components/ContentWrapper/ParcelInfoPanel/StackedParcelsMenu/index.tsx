/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  List,
  ListItem,
  ListItemText,
  makeStyles,
  useTheme,
} from '@material-ui/core';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import React, {
  useState,
  Fragment,
  useContext,
  useMemo,
  useCallback,
} from 'react';

const useStyles = makeStyles((theme) => ({
  root: {
    backgroundColor: theme.palette.background.paper,
    borderRadius: 5,
  },
  listItem: {
    '& span': {
      fontSize: theme.ptas.typography.small.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
  },
  selected: {
    backgroundColor: '#D4E693 !important',
  },
}));

type Item = { major: string; minor: string };

export default function StackedParcelsMenu(): JSX.Element {
  const classes = useStyles(useTheme());
  const [selected, setSelected] = useState(0);
  const { parcelStack: items, setPin } = useContext(ParcelInfoContext);

  const handleListItemClick = useCallback(
    (k: number, item: Item): void => {
      setSelected(k);
      setPin(item.major + item.minor);
    },
    [setPin]
  );

  return useMemo(
    () => (
      <Fragment>
        {!!items.length && (
          <List
            classes={{ root: classes.root }}
            style={{ height: `${7 * items.length}vh` }}
          >
            {items.map((item, i) => {
              const pin = item.major + item.minor;
              return (
                <ListItem
                  classes={{
                    selected: classes.selected,
                  }}
                  key={pin}
                  button
                  selected={i === selected}
                  onClick={(): void => handleListItemClick(i, item)}
                >
                  <ListItemText
                    classes={{ root: classes.listItem }}
                    primary={pin}
                  />
                </ListItem>
              );
            })}
          </List>
        )}
      </Fragment>
    ),

    // eslint-disable-next-line react-hooks/exhaustive-deps
    [
      classes.listItem,
      classes.root,
      classes.selected,
      handleListItemClick,
      // eslint-disable-next-line react-hooks/exhaustive-deps
      JSON.stringify(items),
      selected,
    ]
  );
}
