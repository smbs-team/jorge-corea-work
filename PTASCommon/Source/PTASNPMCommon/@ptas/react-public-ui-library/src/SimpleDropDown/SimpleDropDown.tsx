// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, FC, ReactNode } from "react";
import {
  withStyles,
  WithStyles,
  TextField,
  Theme,
  createStyles
} from "@material-ui/core";
import FormControl from "@material-ui/core/FormControl";
import MenuItem from "@material-ui/core/MenuItem";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { useUpdateEffect } from "react-use";
import clsx from "clsx";

export interface DropDownItem {
  value: number | string;
  label: string;
}

interface Data {
  [key: string]: unknown;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof ComponentStyles> {
  items: DropDownItem[];
  defaultValue?: string;
  onSelected?: (item: DropDownItem, prevValue?: React.ReactText) => void;
  title?: string;
  titleTop?: boolean;
  disabled?: boolean;
  label?: string | ReactNode;
  value?: string;
  disable?: Data;
  placeholder?: string;
  secondaryLabel?: string;
  helperText?: string | React.ReactNode;
  error?: boolean;
  onBlur?: () => void;
}

export type SimpleDropDownProps = GenericWithStyles<Props>;

/**
 * Component styles
 */
export const ComponentStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      flexShrink: 0,
      "& .MuiOutlinedInput-root": {
        "& fieldset": {
          borderColor: theme.ptas.colors.theme.black
        },
        "&.Mui-focused fieldset": {
          borderColor: theme.ptas.colors.utility.selection
        }
      },
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      }
    }),
    select: {},
    label: {
      color: theme.ptas.colors.theme.grayMedium,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      "&.Mui-focused": {
        color: theme.ptas.colors.utility.selection
      },

      /**
       * Fix for issue of label/placeholder text extending beyond the width of the input
       */
      overflow: "hidden",
      whiteSpace: "nowrap",
      textOverflow: "ellipsis",
      maxWidth: "calc(100% - 14px)"
    },
    icon: {
      right: 0,
      height: "100%",
      background: "#e6e6e6",
      borderTopRightRadius: "3px",
      borderBottomRightRadius: "3px",
      borderLeft: "1px solid black",
      top: "0.2px"
    },
    iconOpen: {
      //styles are reversed
      borderTopLeftRadius: "3px",
      borderTopRightRadius: "0px",
      borderBottomLeftRadius: "3px",
      borderBottomRightRadius: "0px",
      borderRight: "1px solid black",
      borderLeft: "none"
    },
    // helperText: {
    //   fontFamily: theme.ptas.typography.bodyFontFamily,
    //   marginBottom: 1
    // },
    inputRoot: { fontSize: 12 },
    input: {
      padding: "11px 8px",
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      minHeight: "auto"
    },
    animated: {
      top: -3,
      left: -1
    },
    shrink: {
      top: 1,
      fontSize: 12
    },
    arrow: {
      width: 24,
      height: 24,
      position: "absolute",
      right: 0,
      pointerEvents: "none"
    },
    itemText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontWeight: theme.ptas.typography.formLabel.fontWeight
    },
    textFieldRoot: (props: Props) => ({
      "&:after": {
        content: `"${props.secondaryLabel}"`,
        display: props.secondaryLabel ? "block" : "none",
        color: theme.ptas.colors.theme.grayMedium,
        fontWeight: "normal",
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.bodySmall.fontSize,
        position: "absolute",
        top: -10,
        right: 20,
        background: theme.ptas.colors.theme.white,
        paddingLeft: 3,
        paddingRight: 3,
        borderRadius: 4
      },
      minWidth: 170
    }),
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      lineHeight: "15px",
      marginLeft: 8,
      marginTop: 0
    },
    helperTextError: {
      color: theme.ptas.colors.utility.dangerInverse
    }
  });

/**
 * SimpleDropDown
 *
 * @param items - Items to render, must have value and label keys
 * @param defaultValue - The value that should appear by default (optional)
 * @param onSelected - Returns the value of the selected item
 * @param title - Text of the component (optional)
 * @returns A JSX element
 */
function SimpleDropDown(props: Props): JSX.Element {
  const { classes, items, onSelected, helperText, error, onBlur } = props;
  const [value, setValue] = useState<string>(props.value ?? "");

  const handleChange = (item: DropDownItem) => {
    if (onSelected) {
      onSelected(item, value);
    }
  };

  useUpdateEffect(() => {
    setValue(props.value ?? "");
  }, [props.value]);

  const arrowIcon = (
    <svg
      viewBox='0 0 24 24'
      focusable={false}
      aria-hidden={true}
      className={
        "MuiSvgIcon-root MuiSelect-icon MuiSelect-iconOutlined " + classes.arrow
      }
    >
      <path
        fill='currentColor'
        d='M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z'
      />
    </svg>
  );

  useEffect(() => {
    if (props.defaultValue) {
      setValue(props.defaultValue);
    }
  }, [props.defaultValue]);

  useEffect(() => {
    if (props.value) {
      setValue(props.value);
    }
  }, [props.value]);

  const dropDownVal = items.find((item) => item.value === value)
    ? value
    : props.placeholder
    ? "placeholderValue"
    : "";

  const renderMenuItems = (): JSX.Element[] => {
    const menuItems: JSX.Element[] = items.map((item) => {
      return (
        <MenuItem
          value={item.value}
          key={item.value}
          disabled={
            props.disable
              ? props.disable[item.value]
                ? true
                : false
              : undefined
          }
          className={classes.itemText}
        >
          {item.label}
        </MenuItem>
      );
    });

    if (props.placeholder) {
      return [
        <MenuItem
          value='placeholderValue'
          disabled
          className={classes.itemText}
        >
          {props.placeholder}
        </MenuItem>,
        ...menuItems
      ];
    }

    return menuItems;
  };

  return (
    <FormControl className={classes.root}>
      <TextField
        className={classes.textFieldRoot}
        id='customized-select'
        select
        onBlur={onBlur}
        variant='outlined'
        size='small'
        error={error}
        label={props.label}
        value={dropDownVal}
        SelectProps={{
          IconComponent: () => {
            return arrowIcon;
          }
        }}
        helperText={helperText}
        placeholder={props.placeholder}
        onChange={(e) => {
          const foundItem = items.find((item) => item.value === e.target.value);
          foundItem && handleChange(foundItem);
          setValue(e.target.value);
        }}
        InputProps={{
          classes: {
            root: classes.inputRoot,
            input: classes.input
          }
        }}
        disabled={props.disabled}
        InputLabelProps={{
          classes: {
            root: classes.label,
            animated: classes.animated,
            shrink: classes.shrink
          },
          shrink: props.placeholder || value ? true : false
        }}
        FormHelperTextProps={{
          classes: {
            root: clsx(classes.helperText, error ? classes.helperTextError : "")
          }
        }}
      >
        {renderMenuItems()}
      </TextField>
    </FormControl>
  );
}

export default withStyles(ComponentStyles)(SimpleDropDown) as FC<
  SimpleDropDownProps
>;
