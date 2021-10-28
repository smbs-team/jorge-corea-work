/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Theme, createStyles } from '@material-ui/core';

export const useCalculateStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
      padding: theme.spacing(3, 0, 0, 4),
      display: 'flex',
      flexDirection: 'column',
      maxHeight: '100%',
    },
    row: {
      display: 'flex',
      marginTop: theme.spacing(1),
      marginBottom: theme.spacing(2),
    },
    contentRow: {
      display: 'flex',
      height: 'calc(100% - 55px)',
    },
    overlapCalcContentRow: {
      display: 'flex',
      height: '100%',
      paddingTop: theme.spacing(0.5),
    },
    icon: {
      marginRight: -4,
    },
    flex: {
      display: 'flex',
    },
    getApp: {
      color: theme.ptas.colors.theme.accent,
      marginLeft: theme.spacing(11 / 8),
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    rightColDropdown: {
      minWidth: 166,
    },
    separator: {
      borderRight: '1px solid ',
      paddingRight: theme.spacing(4),
      margin: theme.spacing(0, 4, 0, 0),
      width: '25%',
      minWidth: 'fit-content',
    },
    tableContainer: {
      overflowY: 'auto',
      width: '75%',
    },
    upperBox: {
      marginBottom: theme.spacing(3),
    },
    intersect: {
      width: 230,
      marginRight: theme.spacing(2),
    },
    increase: {
      width: 100,
    },
    radio: {
      marginBottom: theme.spacing(10 / 8),
    },
    tableHeadCell: {
      borderColor: theme.ptas.colors.theme.grayMedium,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
    },
    tableCell: {
      borderBottom: 'none',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
    },
    startingPoint: {
      width: 650,
    },
  });
