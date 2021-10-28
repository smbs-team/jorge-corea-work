// useLayerContentStyles
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

export const useLayerContentStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    line: {
      display: 'flex',
      marginBottom: theme.spacing(0.5),
    },
  })
);
