// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  ButtonBase,
  Theme,
  ButtonBaseProps
} from "@material-ui/core";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  chipId: React.ReactText;
  label: string;
  isSelected?: boolean;
  onClick?: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  ButtonBaseProps?: ButtonBaseProps;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      height: 27,
      backgroundColor: "#ebebeb",
      borderRadius: 14
    },
    label: {
      padding: theme.spacing(0, 1.5, 0, 1.5),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize
    },
    selected: {
      backgroundColor: theme.ptas.colors.utility.selectionLight
    }
  });

/**
 * Chip
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Chip(props: Props): JSX.Element {
  const { classes } = props;

  return (
    <ButtonBase
      className={
        props.isSelected ? clsx(classes.root, classes.selected) : classes.root
      }
      focusRipple
      onClick={props.onClick}
      {...props.ButtonBaseProps}
    >
      <span className={classes.label}>{props.label}</span>
    </ButtonBase>
  );
}

export default withStyles(useStyles)(Chip);
