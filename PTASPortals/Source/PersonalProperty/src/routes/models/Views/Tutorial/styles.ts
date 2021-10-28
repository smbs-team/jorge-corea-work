// styles.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Theme } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 1292,
    display: 'flex',
    justifyContent: 'center',
    position: 'relative',
    boxSizing: 'border-box',
    paddingTop: 52,
    marginLeft: 'auto',
    marginRight: 'auto',
    height: 608,
    borderRadius: 0,
    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
    },
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 1000,
    marginBottom: 10,
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
  contentWrap: {
    width: '100%',
  },
}));

export default useStyles;
