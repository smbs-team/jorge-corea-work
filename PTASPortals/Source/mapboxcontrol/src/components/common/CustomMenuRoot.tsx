// CustomMenuRoot.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Theme, withStyles } from '@material-ui/core';
import { MenuRoot } from '@ptas/react-ui-library';

export default withStyles((theme: Theme) => ({
  root: {
    height: 48,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: '1rem',
    '&:hover': {
      backgroundColor: 'rgb(7, 66, 171)',
      color: 'white',
    },
  },
  containedSecondary: {},
  isOpen: {},
}))(MenuRoot);
