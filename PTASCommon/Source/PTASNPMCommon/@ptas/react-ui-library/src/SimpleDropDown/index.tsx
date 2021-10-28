// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, FC } from "react";
import {
  withStyles,
  WithStyles,
  TextField,
  Theme,
  createStyles
} from "@material-ui/core";
import FormControl from "@material-ui/core/FormControl";
import MenuItem from "@material-ui/core/MenuItem";
import { GenericWithStyles } from "../common";

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
interface Props extends WithStyles<typeof styles> {
  items: DropDownItem[];
  defaultValue?: number | string;
  onSelected: (item: DropDownItem, prevValue?: React.ReactText) => void;
  title?: string;
  titleTop?: boolean;
  disabled?: boolean;
  label?: string;
  value?: string | number;
  disable?: Data;
}

export type SimpleDropDownProps = GenericWithStyles<Props>;

const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: "fit-content",
      maxWidth: "100%",
      flexShrink: 0,
      "& .MuiOutlinedInput-root": {
        "& fieldset": {
          borderColor: theme.ptas.colors.theme.black
        },
        "&.Mui-focused fieldset": {
          borderColor: theme.ptas.colors.utility.selection
        }
      }
    },
    select: {},
    label: {
      color: theme.ptas.colors.theme.grayMedium,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      "&.Mui-focused": {
        color: theme.ptas.colors.utility.selection
      }
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
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginBottom: theme.spacing(1)
    },
    inputRoot: {},
    input: {
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    animated: {},
    shrink: {},
    arrow: {
      width: 24,
      height: 24,
      position: "absolute",
      right: 6,
      pointerEvents: "none"
    },
    itemText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontWeight: theme.ptas.typography.formLabel.fontWeight
    },
    selectRoot: {
      "&:focus": {
        backgroundColor: "unset"
      }
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
  const { classes, items, onSelected } = props;
  const [value, setValue] = useState<string | number>("");

  const handleChange = (item: DropDownItem) => {
    onSelected(item, value);
  };

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
    setValue(props.value ?? "");
  }, [props.value]);

  const dropDownVal = items.some((item) => item.value === value) ? value : "";
  return (
    <FormControl className={classes.root}>
      <TextField
        id='customized-select'
        select
        variant='outlined'
        size='small'
        label={props.label}
        value={dropDownVal}
        defaultValue={props.defaultValue ? props.defaultValue : ""}
        SelectProps={{
          IconComponent: () => {
            return arrowIcon;
          },
          classes: { root: classes.selectRoot }
        }}
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
          }
        }}
        FormHelperTextProps={{
          classes: {
            root: classes.helperText
          },
          style: { backgroundColor: "blue" }
        }}
      >
        {items.map((item) => {
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
        })}
      </TextField>
    </FormControl>
  );
}

export default withStyles(styles)(SimpleDropDown) as FC<SimpleDropDownProps>;
