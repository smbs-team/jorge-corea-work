// styles.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 1292,
    display: 'flex',
    justifyContent: 'center',
    position: 'relative',
    boxSizing: 'border-box',
    paddingTop: 24,
    marginLeft: 'auto',
    marginRight: 'auto',
    height: 608,
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 29,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 1000,
  },
  title: {
    fontSize: theme.ptas.typography.h5.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontFamily: theme.ptas.typography.titleFontFamily,
    margin: '0 0 16px 0',
    textAlign: 'center',
  },
  closeButton: {
    position: 'absolute',
    right: 3,
    top: 3,
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    fontSize: 34,
  },
  wrapper: {
    width: '100%',
    maxWidth: 303,
    display: 'flex',
    justifyContent: 'center',
    flexDirection: 'column',
    margin: '0 auto',
  },
  item: {
    fontSize: theme.ptas.typography.body.fontSize,
    listStyle: 'none',

    '&::before': {
      content: `'•'`,
      marginRight: 8,
    },
  },
  subtitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
  },
  wrapperContent: {
    width: '100%',
  },
  list: {
    padding: 0,
    margin: 0,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 15,
  },
  date: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 16,
    textAlign: 'center',
  },
  joinWebinarButton: {
    width: 160,
    height: 30,
    margin: '0 auto',
  },
}));

export default useStyles;
