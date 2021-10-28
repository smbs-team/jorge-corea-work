// CustomDatePicker.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
  KeyboardDatePickerProps
} from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import DateRangeIcon from "@material-ui/icons/DateRange";
import { createStyles, Theme, WithStyles, withStyles } from "@material-ui/core";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {}

export type CustomDate = Date | null | undefined;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      fontSize: "1rem",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      width: 138,
      minWidth: 138,
      "& .MuiInputAdornment-root": {
        margin: 0
      }
    },
    input: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      borderColor: theme.ptas.colors.theme.black,
      borderRadius: 3,
      height: 37,
      paddingRight: 8,
      "& .MuiInputBase-input": {
        padding: theme.spacing(0, 0, 0, 1.75)
      }
    },
    dateRangeIcon: {
      fontSize: 20,
      color: theme.ptas.colors.theme.black
    },
    textField: {},
    iconButton: {
      margin: 0,
      padding: 0
    },
    notchedOutline: {
      borderColor: theme.ptas.colors.theme.black
    }
  });

/**
 * CustomDatePicker
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomDatePicker(props: KeyboardDatePickerProps & Props): JSX.Element {
  const { classes, ...otherProps } = props;

  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils}>
      <KeyboardDatePicker
        autoOk
        variant='inline'
        inputVariant='outlined'
        format='MM/dd/yyyy'
        label='Choose a Date'
        KeyboardButtonProps={{ className: classes.iconButton }}
        InputProps={{
          classes: {
            root: classes.input,
            notchedOutline: classes.notchedOutline
          }
        }}
        className={classes.root}
        keyboardIcon={<DateRangeIcon className={classes.dateRangeIcon} />}
        {...otherProps}
      />
    </MuiPickersUtilsProvider>
  );
}

export default withStyles(useStyles)(CustomDatePicker);
