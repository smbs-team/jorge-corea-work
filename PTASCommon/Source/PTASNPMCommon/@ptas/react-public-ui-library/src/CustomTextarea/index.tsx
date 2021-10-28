// CustomTextarea.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  ChangeEvent,
  useState,
  useEffect,
  FC,
  useCallback
} from "react";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles,
  TextField
} from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";
import {
  formatMessageStructure,
  renderFormatMessage
} from "../utils/formatMessage";
import { debounce } from "lodash";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  disabled?: boolean;
  onChange?: (e: ChangeEvent<HTMLTextAreaElement>) => void;
  onBlur?: () => void;
  value?: string;
  placeholder?: string | formatMessageStructure;
  helperText?: string | React.ReactNode;
  label?: string | React.ReactNode;
  error?: boolean;
  rows?: number;
  variant?: "standard" | "filled" | "outlined";
  onChangeDelay?: number;
  /**
   * Dependencies for updating the onChange memoized callback (triggerOnChangeProps).
   * E.g. states used by the caller inside the onChange event.
   */
  onChangeDeps?: unknown[];
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      },
      backgroundColor: "transparent",
      "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      },
      "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.utility.selection
        }`,
        border: "1px solid"
      },
      "& .MuiInput-underline:after": {
        borderBottomColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.utility.selection
        }`
      },
      width: 270,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      color: theme.ptas.colors.theme.grayMedium,
      boxSizing: "border-box",
      outline: "none",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      "& .MuiInput-underline:before": {
        borderBottom: "none"
      }
    }),
    input: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      width: "100%"
    },
    fullWidth: {
      width: "100%",
      padding: "10px 8px 0 8px !important",
      height: 97,
      background: theme.ptas.colors.theme.white,
      borderRadius: 3
    },
    labelRoot: {
      color: theme.ptas.colors.theme.grayMedium,
      fontWeight: "normal",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      marginTop: "0 !important"
    },
    animated: {
      top: 3,
      fontSize: 16,
      left: 8,
      zIndex: 10
    },
    shrink: {
      fontSize: 16,
      top: 1,
      background: theme.ptas.colors.theme.white,
      paddingLeft: 7,
      borderRadius: 3,
      paddingRight: 7,
      left: 0
    },
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginTop: 0,
      lineHeight: "15px",
      marginLeft: 8
    }
  });

/**
 * CustomTextarea
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomTextarea(props: Props): JSX.Element {
  const {
    classes,
    onChange,
    disabled,
    value,
    placeholder,
    helperText,
    rows,
    onChangeDelay
  } = props;

  const [inputValue, setInputValue] = useState<string>("");

  useEffect(() => {
    if (value) setInputValue(value);
  }, [value]);

  const handleChange = (e: ChangeEvent<HTMLTextAreaElement>): void => {
    e.persist();
    setInputValue(e.target.value);
    triggerOnChangeProps({ ...e });
  };

  const triggerOnChangeProps = useCallback(
    debounce((e: ChangeEvent<HTMLTextAreaElement>) => {
      onChange?.(e);
    }, onChangeDelay ?? 0),
    props.onChangeDeps ?? []
  );

  return (
    <TextField
      variant={props.variant ?? "standard"}
      onChange={handleChange}
      onBlur={props.onBlur}
      error={props.error}
      size='small'
      value={inputValue}
      label={props.label}
      placeholder={renderFormatMessage(placeholder)}
      disabled={disabled}
      className={classes.root}
      InputProps={{
        classes: {
          input: classes.input,
          root: classes.fullWidth
        }
      }}
      helperText={helperText}
      InputLabelProps={{
        shrink: placeholder || inputValue ? true : false,
        classes: {
          root: classes.labelRoot,
          animated: classes.animated,
          shrink: classes.shrink
        }
      }}
      FormHelperTextProps={{
        classes: {
          root: classes.helperText
        }
      }}
      rows={rows ?? 5}
      multiline
    />
  );
}

export default withStyles(useStyles)(CustomTextarea) as FC<
  GenericWithStyles<Props>
>;
