// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { withStyles, WithStyles, createStyles, Theme, ButtonBase, ButtonBaseProps } from "@material-ui/core";
import { omit } from "lodash";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles>, Omit<ButtonBaseProps, "classes"> {
  text?: string;
  icon?: React.ReactNode;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      textTransform: "none",
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontWeight: "bold",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      color: theme.ptas.colors.theme.accent,
      "&:hover": {
        backgroundColor: "unset"
      },
      "&:active": {
        transform: "scale(0.98)"
      }
    },
    label: {
      marginLeft: 2,
      cursor: "pointer"
    },
    disabled: {
      opacity: 0.5
    }
  });

/**
 * CustomIconButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomIconButton(props: Props): JSX.Element {
  const newProps = omit(props, ["text", "icon", "classes"]);

  return (
    <ButtonBase {...newProps} classes={{root: props.classes.root, disabled: props.classes.disabled}}>
      {props.icon}
      {props.text && (
        <label className={props.classes.label}>{props.text}</label>
      )}
    </ButtonBase>
  );
}

export default withStyles(useStyles)(CustomIconButton);
