/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  withStyles,
  Theme,
  createStyles,
  Menu,
  MenuClassKey
} from "@material-ui/core";

const styles = (theme: Theme) =>
  createStyles<MenuClassKey, object>({
    paper: {
      color: theme.ptas.colors.theme.black,
      borderRadius: 0
    },
    list: {
      padding: 0
    }
  });

/**
 * A material menu with some styles applied
 */
export default withStyles(styles)(Menu);
