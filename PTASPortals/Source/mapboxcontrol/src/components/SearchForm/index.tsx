// SearchForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import { FormControlLabel, Theme } from '@material-ui/core';
import { makeStyles, useTheme, createStyles } from '@material-ui/core/styles';
import { AutoComplete } from '@ptas/react-ui-library';
import { useSearchForm } from './useSearchForm';

const useStyles = makeStyles((theme: Theme) => {
  return createStyles({
    formControlLabelRoot: {
      color: theme.ptas.colors.theme.white,
      marginRight: theme.spacing(2),
    },
    textRoot: {
      '& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline': {
        borderColor: 'transparent',
      },
      '&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline': {
        borderColor: 'transparent',
      },
    },
    textField: {
      height: 36,
    },
  });
});

const SearchForm = (): JSX.Element => {
  const classes = useStyles(useTheme());
  const { searchByAddress, autoCompleteData, runSearch } = useSearchForm();

  return (
    <Fragment>
      <FormControlLabel
        label=""
        labelPlacement="start"
        className={classes.formControlLabelRoot}
        control={
          <AutoComplete
            data={autoCompleteData.map((item) => ({
              title: item.formattedaddr,
            }))}
            onValueChanged={(val): void => {
              runSearch(val.title);
            }}
            onTextFieldValueChange={searchByAddress}
            onIconClick={runSearch}
            classes={{
              textRoot: classes.textRoot,
              textField: classes.textField,
            }}
            label="Find address, parcel, account"
            AutocompleteProps={{
              clearOnBlur: false,
            }}
          />
        }
      />
    </Fragment>
  );
};

export default SearchForm;
