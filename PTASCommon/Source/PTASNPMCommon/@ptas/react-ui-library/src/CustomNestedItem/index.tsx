// CustomNestedItem.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { forwardRef } from "react";
import {
  makeStyles,
  createStyles,
  Theme,
  withStyles
} from "@material-ui/core/styles";
import NestedMenuItem, {
  NestedMenuItemProps
} from "material-ui-nested-menu-item";
import { MenuItemClassKey } from "@material-ui/core";
import { StyleRules } from "@material-ui/core";

const useStyles = (theme: Theme): StyleRules<MenuItemClassKey> =>
  createStyles<MenuItemClassKey, NestedMenuItemProps>({
    root: {
      fontSize: theme.ptas.typography.small.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      height: 40
    },
    dense: {},
    gutters: {
      padding: "8px 8px 8px 45px"
    },
    selected: {}
  });

const useStyles2 = makeStyles(() =>
  createStyles({
    arrow: { width: 18 }
  })
);

/**
 * Nested Menu Item
 */
const CustomNestedItem = forwardRef<HTMLLIElement, NestedMenuItemProps>(
  (props: NestedMenuItemProps, ref): JSX.Element => {
    const classes = { ...props.classes, ...useStyles2() };
    const arrowIcon = (
      <svg viewBox='0 0 24 24' className={classes.arrow}>
        <path
          fill='currentColor'
          d='M8.59,16.58L13.17,12L8.59,7.41L10,6L16,12L10,18L8.59,16.58Z'
        />
      </svg>
    );
    return (
      <NestedMenuItem
        ref={ref}
        classes={{
          root: classes.root
        }}
        rightIcon={arrowIcon}
        {...props}
      />
    );
  }
);

export default withStyles(useStyles)(CustomNestedItem);
