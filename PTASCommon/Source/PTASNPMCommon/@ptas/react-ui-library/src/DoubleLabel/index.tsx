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
  Box,
  Theme
} from "@material-ui/core";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  text1: string;
  text1Value?: string;
  text2: string;
  text2Value?: string;
  defaultText?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    container: {
      width: "fit-content"
    },
    label: {
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
 * DoubleLabel
 *
 * @param props - Component props
 * @returns A JSX element
 */
function DoubleLabel(props: Props): JSX.Element {
  const { classes } = props;
  return (
    <Box className={classes.container}>
      <label className={classes.label}>
        {props.text1} ={" "}
        {props.text1Value ?? (
          <label className={clsx(classes.label, classes.noValue)}>
            {props.defaultText ?? "To be calculated"}
          </label>
        )}
      </label>
      <label className={classes.label}>
        {props.text2} ={" "}
        {props.text2Value ?? (
          <label className={clsx(classes.label, classes.noValue)}>
            {props.defaultText ?? "To be calculated"}
          </label>
        )}
      </label>
    </Box>
  );
}

export default withStyles(useStyles)(DoubleLabel);
