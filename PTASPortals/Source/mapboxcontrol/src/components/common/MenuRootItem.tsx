/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createStyles, StyleRules, withStyles } from '@material-ui/core';
import {
  MenuRootItem,
  MenuRootItemClassKey,
  MenuRootItemProps,
} from '@ptas/react-ui-library';

export default withStyles(
  (): StyleRules =>
    createStyles<MenuRootItemClassKey, MenuRootItemProps>({
      root: {
        height: 40,
      },
      children: {},
      dense: {},
      fill: {},
      gutters: {},
      leftIcon: {},
      rightElement: {},
      selected: {},
    })
)(MenuRootItem);
