// Styles.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 1027,
    display: 'flex',
    justifyContent: 'center',
    position: 'relative',
    boxSizing: 'border-box',
    paddingTop: 24,
    marginLeft: 'auto',
    marginRight: 'auto',
    background: theme.ptas.colors.theme.white,
    paddingRight: 10,
    paddingLeft: 10,
    borderRadius: 0,
    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
    },
  },
  contentWrap: {
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  },
  head: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: 29,
    width: '100%',
  },
  headTitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    lineHeight: '23px',
  },
  title: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: 24,
    fontFamily: theme.ptas.typography.titleFontFamily,
    display: 'block',
    lineHeight: '28px',
    marginBottom: 5,
  },
  textWrap: {
    width: '100%',
    maxWidth: 290,
    margin: '0 auto 31px auto',
    textAlign: 'center',
  },
  description: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    display: 'block',
    textAlign: 'center',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 8,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 320,
  },
  checkAllToApply: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  wrapOpt: {
    display: 'flex',
    justifyContent: 'space-between',
    width: 140,
    alignItems: 'start',
    flexDirection: 'column',
    marginBottom: 49,

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      width: 469,
    },
  },
  continue: {
    width: 135,
    height: 38,
    marginBottom: 31,
  },
  switchOption: {
    marginBottom: 10,

    [theme.breakpoints.up('sm')]: {
      marginBottom: 0,
    },
  },
  titlesWrap: {
    textAlign: 'center',
  },
  // classes moved
  movedRoot: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    width: 270,
  },
  newDate: {
    width: 135,
    marginBottom: 49,
    marginTop: 0,
  },
  tabs: {
    marginBottom: '23px !important',
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
  countryDropdown: {
    minWidth: 62,
  },
  rootDropdown: {
    marginRight: 'auto',
  },
  useOtherAddress: {
    marginLeft: 'auto',
  },
  buttonWrap: {
    width: 281,
    display: 'flex',
    justifyContent: 'space-between',
  },
}));

export default useStyles;
