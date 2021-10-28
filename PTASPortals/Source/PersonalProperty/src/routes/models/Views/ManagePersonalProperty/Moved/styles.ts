// Styles.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) => ({
  movedRoot: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    width: 270,
    marginBottom: 48,
  },
  newDate: {
    width: 135,
    marginBottom: 49,
    marginTop: 0,
  },
  inputs: {
    maxWidth: 270,
    marginBottom: 24,
  },
  city: {
    maxWidth: 107,
  },
  state: {
    width: 45,
  },
  zip: {
    width: 101,
  },
  inputWrap: {
    display: 'flex',
    justifyContent: 'space-between',
    width: '100%',
    marginBottom: 24,
  },
  countryDropdown: {
    minWidth: 62,
  },
  rootDropdown: {
    marginRight: 'auto',
  },
  useOtherAddress: {
    marginLeft: 'auto',
  },
  addressInputWrap: {
    maxWidth: '100%',
    marginBottom: 25,
    width: '100%',
  },
  addressStateInputWrap: {
    padding: '0 5px',
  },
}));

export default useStyles;
