// useLayersInfoStyles
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

export const useLayerInfoStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: '1rem',
    },
    table: {
      tableLayout: 'auto',
    },
    tableCell: {
      borderBottom: 'none',
      padding: theme.spacing(0.5, 1, 0.5, 1),
      fontFamily: 'inherit',
      fontSize: 'inherit',
    },
    tableCellKey: {
      textAlign: 'end',
      verticalAlign: 'top',
      fontWeight: 'bold',
      width: 90,
    },
    tableCellValue: {
      textAlign: 'start',
      verticalAlign: 'top',
    },
  })
);
