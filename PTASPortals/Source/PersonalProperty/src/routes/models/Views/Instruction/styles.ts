// Styles.tsx
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
    borderRadius: 0,
    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
    },
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 32,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 1000,
  },
  title: {
    fontSize: theme.ptas.typography.h3.fontSize,
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
  description: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    display: 'block',
    marginTop: 8,
  },
  instructionWrapper: {
    marginBottom: 29,
  },
  listTitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: 24,
    lineHeight: '28px',
    marginBottom: 9,
    display: 'block',
  },
  listSubtitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
    marginBottom: 8,
  },
  mainItem: {
    color: theme.ptas.colors.theme.black,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    lineHeight: '22px',

    '&::before': {
      content: `'•'`,
      marginRight: 8,
    },
  },
  mainList: {
    marginTop: 8,
    marginBottom: 8,
    listStyle: 'none',
    paddingLeft: 20,
  },
  subList: {
    marginBottom: 6,
    listStyle: 'none',
    paddingLeft: 32,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  subItem: {
    position: 'relative',
    marginTop: 7,

    '&::before': {
      content: `'•'`,
      position: 'absolute',
      left: -11,
    },
  },
  contentWrap: {
    width: '100%',
    maxWidth: '972px',
  },
}));

export default useStyles;
