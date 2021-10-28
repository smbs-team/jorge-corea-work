// PanelButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Button as MUIButton,
  Theme,
  withStyles,
  createStyles,
} from '@material-ui/core';

const PanelButton = withStyles((theme: Theme) =>
  createStyles({
    root: {
      color: theme.ptas.colors.theme.accent,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonLarge.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      marginRight: theme.spacing(3),
    },
  })
)(MUIButton);

export default PanelButton;
