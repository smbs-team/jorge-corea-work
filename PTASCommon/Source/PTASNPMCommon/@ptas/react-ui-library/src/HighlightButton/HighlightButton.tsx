// HighlightButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import { ButtonProps, makeStyles } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import { Theme } from "@material-ui/core";
import { omit } from "lodash";

/**
 * Component props
 */
interface Props extends PropsWithChildren<ButtonProps> {
  isSelected?: boolean;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: (props: Props) => ({
    height: "30px",
    boxSizing: "border-box",
    border: `1px solid ${theme.ptas.colors.theme.black}`,
    borderRadius: theme.spacing(2),
    backgroundColor: props.isSelected
      ? "#d4e693"
      : theme.ptas.colors.theme.white,
    padding: theme.spacing(1 / 8, 12 / 8),
    "&:hover": {
      backgroundColor: theme.ptas.colors.utility.selectionLight
    },
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    width: "fit-content",
    margin: theme.spacing(1, 0)
  }),
  // disabled: {
  //   opacity: (props: Props) => (props.disabled ? "50%" : "100%"),
  // }
  disabled: (props: Props) => ({
    opacity: props.disabled ? "50%" : "100%"
  })
}));

/**
 * HightlightButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
function HighlightButton(props: Props): JSX.Element {
  const classes = useStyles(props);
  const newProps = omit(props, ["isSelected"]);

  return (
    <Button
      {...newProps}
      classes={{ root: classes.root, disabled: classes.disabled }}
    >
      {props.children}
    </Button>
  );
}

export default HighlightButton;
