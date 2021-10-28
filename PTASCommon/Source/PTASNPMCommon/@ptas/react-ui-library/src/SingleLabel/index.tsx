// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  text: string;
  textValue?: string;
  defaultText?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    label: {
      width: "fit-content",
      display: "block",
      fontSize: theme.ptas.typography.body.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: "bold",
      lineHeight: theme.ptas.typography.lineHeight
    },
    noValue: {
      display: "inline",
      fontStyle: "italic",
      color: theme.ptas.colors.theme.gray
    }
  });

/**
 * SingleLabel
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SingleLabel(props: Props): JSX.Element {
  const { classes } = props;
  return (
    <label className={classes.label}>
      {props.text} ={" "}
      {props.textValue ? (
        props.textValue
      ) : (
        <label className={clsx(classes.label, classes.noValue)}>
          {props.defaultText ?? "To be calculated"}
        </label>
      )}
    </label>
  );
}

export default withStyles(useStyles)(SingleLabel);
