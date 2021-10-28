// HomeStaff.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState } from 'react';
import {
  PropertySearcher,
  ItemSuggestion,
} from '@ptas/react-public-ui-library';
import { Box, makeStyles, Theme } from '@material-ui/core';
import * as fm from './formatText';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  title: {
    fontSize: theme.ptas.typography.h5.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    marginBottom: 32,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,
  },
  subtitle: {
    fontSize: theme.ptas.typography.bodyBold.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    lineHeight: '22px',
  },
}));

function HomeStaff(): JSX.Element {
  const classes = useStyles();

  const [searchList, setSearchList] = useState<ItemSuggestion[]>([]);
  //TEMP list
  const propertiesList: ItemSuggestion[] = [
    {
      title: 'Contact 1',
      subtitle: 'Test 1',
    },
    {
      title: 'Contact 2',
      subtitle: 'Test 2',
    },
  ];

  const handleOnSearchChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;

    if (inputValue) {
      const newList = propertiesList.filter(item =>
        item.title.toLowerCase().includes(inputValue.toLowerCase())
      );

      setSearchList(newList);
    }
  };

  const onSearchItemSelected = (item: ItemSuggestion): void => {
    console.log('Contact selected:', item.title);
    //TODO: use contact selected
  };

  return (
    <Fragment>
      <h2 className={classes.title}>{fm.staffTitle}</h2>
      <Box className={classes.root}>
        <Box className={classes.subtitle}>{fm.signInTaxpayer}</Box>
        <PropertySearcher
          label={fm.findTaxpayer}
          textDescription={fm.enterSearchParam}
          onChange={handleOnSearchChange}
          suggestion={{
            List: searchList,
            onSelected: onSearchItemSelected,
          }}
          onClickSearch={(): void => {
            console.log('searcher icon was clicked');
          }}
          onClickTextButton={(): void => {
            console.log('locate me button was clicked');
          }}
        />
      </Box>
    </Fragment>
  );
}

export default HomeStaff;
