import React, { forwardRef } from "react";
import {
  MenuItemProps,
  Theme,
  createStyles,
  withStyles,
  Box,
  WithStyles,
  StyleRules
} from "@material-ui/core";
import MUIMenuItem from "@material-ui/core/MenuItem";
import clsx from "clsx";
import { GenericWithStyles } from "../common";
import { omit } from "lodash";
import { MenuItemClassKey } from "@material-ui/core";

const customClasses = ["fill", "leftIcon", "rightElement", "children"] as const;
type CustomClassKey = typeof customClasses[number];
export type MenuRootItemClassKey = MenuItemClassKey | CustomClassKey;

const useStyles = (theme: Theme): StyleRules<MenuRootItemClassKey> =>
  createStyles<MenuRootItemClassKey, Props>({
    root: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.small.fontSize
    },
    children: (props) => ({
      overflow: "hidden",
      textOverflow: "ellipsis",
      width: props.rightElement ? "85%" : "unset"
    }),
    selected: {},
    gutters: {
      paddingLeft: 45
    },
    dense: {},
    fill: {
      backgroundColor: theme.ptas.colors.theme.grayLight + "e0",
      color: theme.palette.common.black,
      "&:hover": {
        backgroundColor: theme.ptas.colors.theme.grayLightest
      }
    },
    leftIcon: {
      color: theme.ptas.colors.utility.selection,
      left: 12,
      top: 9,
      position: "absolute"
    },
    rightElement: {
      position: "absolute",
      right: 0,
      color: theme.ptas.colors.utility.selection
    }
  });

type Props = MenuItemProps<"li", { button?: true }> &
  WithStyles<typeof useStyles> & {
    leftIcon?: JSX.Element;
    rightElement?: JSX.Element;
    variant?: "none" | "fill";
  };

export type MenuRootItemProps = GenericWithStyles<Props>;

const MenuRootItem = forwardRef<HTMLLIElement, Props>((props, ref) => {
  const menuItemProps = omit(props, [
    "leftIcon",
    "rightElement",
    "variant",
    "classes"
  ]);
  return (
    <MUIMenuItem
      ref={ref}
      {...menuItemProps}
      classes={{
        ...omit(props.classes, customClasses),
        root: clsx(
          props.classes.root,
          props.variant === "fill" && props.classes.fill
        )
      }}
    >
      {props.leftIcon && (
        <Box className={props.classes.leftIcon}>{props.leftIcon}</Box>
      )}
      <Box className={props.classes.children}>{props.children}</Box>
      {props.rightElement && (
        <Box className={props.classes.rightElement}>{props.rightElement}</Box>
      )}
    </MUIMenuItem>
  );
});

export default withStyles(useStyles, { withTheme: true })(MenuRootItem);
