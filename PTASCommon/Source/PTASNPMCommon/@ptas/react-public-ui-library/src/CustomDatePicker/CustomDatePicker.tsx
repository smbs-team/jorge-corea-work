import "date-fns";
import React, { FC, ReactNode } from "react";
import Grid from "@material-ui/core/Grid";
import DateFnsUtils from "@date-io/date-fns";
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker
} from "@material-ui/pickers";
import { createStyles, WithStyles, Theme, withStyles } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";
import {
  formatMessageStructure,
  renderFormatMessage
} from "../utils/formatMessage";
import clsx from "clsx";
import { useEffect } from "react";
import { debounce } from "lodash";
import { useCallback } from "react";

interface Props extends WithStyles<typeof useStyles> {
  label?: string | ReactNode;
  error?: boolean;
  helperText?: string;
  secondaryLabel?: string | ReactNode;
  className?: string;
  onChange?: (date: Date | null, value: string | null) => void;
  onBlur?: () => void;
  format?: string;
  placeholder?: string | formatMessageStructure;
  variant?: "dialog" | "inline" | "static";
  inputVariant?: "standard" | "filled" | "outlined";
  value?: Date;
  onChangeDelay: number;
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      fontSize: "1rem",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      maxWidth: 170,
      width: "100%",
      "& .MuiInputAdornment-root": {
        margin: 0
      },
      "&:after": {
        content: `"${props.secondaryLabel}"`,
        display: props.secondaryLabel ? "block" : "none",
        color: theme.ptas.colors.theme.grayMedium,
        fontWeight: "normal",
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.bodySmall.fontSize,
        position: "absolute",
        top: -8,
        right: 20,
        background: theme.ptas.colors.theme.white,
        paddingLeft: 3,
        paddingRight: 3,
        paddingBlockStart: 0,
        borderRadius: 4
      },
      zIndex: 10
    }),
    standardRoot: {
      "& > div": {
        marginTop: 0
      },
      "& .MuiInput-underline:before": {
        borderBottom: "none"
      }
    },
    standardAnimated: {
      top: "-12px !important",
      left: "10px !important"
    },
    standardShrink: {
      top: "-8px !important",
      left: "7px !important"
    },
    input: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      borderColor: theme.ptas.colors.theme.black,
      borderRadius: 3,
      height: 36,
      paddingRight: 8,
      "& .MuiInputBase-input": {
        padding: 10
      }
    },
    dateRangeIcon: {
      fontSize: 12,
      color: theme.ptas.colors.theme.black
    },
    textField: {},
    iconButton: {
      margin: 0,
      padding: 0,
      "& .MuiSvgIcon-root": {
        fontSize: 20
      }
    },
    animated: {
      top: -8,
      fontSize: 16,
      left: -4
    },
    shrink: {
      fontSize: 16,
      top: 1,
      left: -5,
      background: theme.ptas.colors.theme.white,
      paddingLeft: 7,
      borderRadius: 3,
      paddingRight: 7
    }
  });

const CustomDatePicker = (props: Props) => {
  const {
    classes,
    onChange,
    placeholder,
    format,
    label,
    helperText,
    error,
    variant,
    inputVariant
  } = props;

  const [selectedDate, setSelectedDate] = React.useState<Date | null>(null);

  useEffect(() => {
    if (props.value) {
      setSelectedDate(props.value);
    }
  }, [props.value]);

  const handleDateChange = (date: Date | null, value: string | null) => {
    setSelectedDate(date);

    triggerOnChangeProps(date, value);
  };

  const triggerOnChangeProps = useCallback(
    debounce((date: Date | null, value: string | null) => {
      onChange?.(date, value);
    }, props.onChangeDelay ?? 0),
    []
  );

  const setClassByVariants = (): {
    root: string;
    animated: string;
    shrink: string;
  } => {
    switch (inputVariant) {
      case "outlined":
        return {
          root: classes.root,
          animated: classes.animated,
          shrink: classes.shrink
        };
      case "standard":
        return {
          root: clsx(classes.root, classes.standardRoot),
          animated: clsx(classes.animated, classes.standardAnimated),
          shrink: clsx(classes.shrink, classes.standardShrink)
        };

      default:
        return {
          root: classes.root,
          animated: classes.animated,
          shrink: classes.shrink
        };
    }
  };

  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils}>
      <Grid container justify='space-around'>
        <KeyboardDatePicker
          autoOk
          disableToolbar
          inputVariant={inputVariant ?? "outlined"}
          variant={variant ?? "inline"}
          placeholder={renderFormatMessage(placeholder)}
          KeyboardButtonProps={{ className: classes.iconButton }}
          format={format ?? "MM/dd/yyyy"}
          margin='normal'
          id='date-picker-inline'
          label={label ?? "Date picker"}
          value={selectedDate}
          className={setClassByVariants().root}
          onChange={handleDateChange}
          onBlur={props.onBlur}
          InputProps={{
            classes: {
              root: classes.input
              // notchedOutline: classes.notchedOutline
            }
          }}
          InputLabelProps={{
            classes: {
              animated: setClassByVariants().animated,
              shrink: setClassByVariants().shrink
            },
            shrink: placeholder ? true : !!selectedDate
          }}
          helperText={helperText}
          error={error}
        />
      </Grid>
    </MuiPickersUtilsProvider>
  );
};

export default withStyles(useStyles)(CustomDatePicker) as FC<
  GenericWithStyles<Props>
>;
