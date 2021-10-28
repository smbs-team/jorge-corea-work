/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Theme, createStyles, makeStyles } from '@material-ui/core';

export const useManageLayerCommonStyles = makeStyles((theme: Theme) =>
  createStyles({
    columnFlexBoxTitle: {
      marginBottom: theme.spacing(2),
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    },
    columnFlexBoxBody: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
  })
);
