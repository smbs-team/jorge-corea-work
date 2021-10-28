// Styles.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) => ({
  // classes moved
  soldRoot: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    width: 586,
    marginBottom: 32,
  },
  saleTab: {
    marginBottom: 32,
    width: '100%',
    maxWidth: 500,
    marginLeft: 'auto',
    marginRight: 'auto',
  },
  newContractTab: {
    width: 270,
    marginBottom: 39,
  },
  oldInfoTab: {
    width: 270,
    marginBottom: 10,
  },
  assetsTab: {
    width: 765,
    marginBottom: 32,
  },
  currencyWrap: {
    width: '100%',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 27,
    flexDirection: 'column',
    '& > div': {
      marginBottom: 10,
    },

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      '& > div': {
        marginBottom: 0,
      },
    },
  },
  movedDate: {
    width: 135,
    marginBottom: 17,
    marginRight: 'auto',
    marginTop: 0,
  },
  label: {
    fontSize: 12,
    marginBottom: 3,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    display: 'block',
  },
  tabs: {
    marginBottom: '40px !important',
  },
  optionTabs: {
    marginBottom: '16px !important',
  },
  inputs: {
    maxWidth: 270,
    marginBottom: 24,
    display: 'block',
  },
  readOnly: {
    maxWidth: 270,
    marginBottom: 18,
    display: 'block',
    '& > .MuiInputBase-root:before': {
      border: 'none',
    },
    '& > .MuiInputBase-root:after': {
      border: 'none',
    },
    '& > .MuiInputBase-root:hover:before': {
      border: 'none',
    },
  },
  switchLabel: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  dropdown: {
    width: 270,
  },
  ubiNumber: {
    maxWidth: 143,
  },
  NAICSNumber: {
    maxWidth: 126,
  },
  inputPhone: {
    marginBottom: 24,
    maxWidth: 196,
  },
  currency: {
    maxWidth: 140,
  },
  city: {
    maxWidth: 107,
  },
  state: {
    width: 45,
    '& > .MuiFilledInput-root': {
      paddingRight: 0,
    },
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
  marginBottomNone: {
    marginBottom: 0,
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
  //
  fileUploadRoot: {
    margin: 0,
  },
  description: {
    color: 'rgba(0, 0, 0, 0.54)',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: 14,
    display: 'block',
    textAlign: 'center',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    display: 'block',
    marginBottom: 15,
    marginTop: 32,
    margin: '0 auto',
    width: '100%',
  },
  emailIcon: {
    position: 'absolute',
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    right: 3,
    bottom: 1,
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
