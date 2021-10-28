// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from "react";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Theme, withStyles, createStyles, WithStyles } from "@material-ui/core";
import { CSSProperties, StyleRules } from "@material-ui/core/styles/withStyles";
import SearchIcon from "@material-ui/icons/Search";
import CustomTextField, { CustomTextFieldProps } from "../CustomTextField";

export interface AutoCompleteRow {
  title: string;
  originalRow?: unknown;
}

interface AutocompleteProps {
  value?: string;
  clearOnBlur?: boolean;
  selectOnFocus?: boolean;
  style?: CSSProperties;
  className?: string;
}

export interface CustomAutocompleteProps extends WithStyles<typeof useStyles> {
  onIconClick?: (
    text: string,
    event?: React.MouseEvent<HTMLElement, MouseEvent>
  ) => void;
  /**
   * The autocomplete data to suggest
   */
  data: AutoCompleteRow[];
  label?: string;
  TextFieldProps?: CustomTextFieldProps;
  AutocompleteProps?: AutocompleteProps;
  /**
   * Triggered when selecting a suggested option or pressing enter key on the input
   */
  onValueChanged?: (val: AutoCompleteRow) => void;
  onTextFieldValueChange?: (val: string) => void;

  defaultStyle?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    textField: {
      minWidth: 250,
      width: 250,
      borderRadius: 16,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      backgroundColor: theme.ptas.colors.theme.white
    },
    default: {
      minWidth: 206,
      width: 206,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize
    },
    textRoot: {
      "& .MuiInputBase-input": {
        paddingTop: 6,
        paddingBottom: 6
      }
    },
    textRootDefault: {
      "& .MuiInputBase-input": {
        padding: 9
      },
      "& .MuiInputLabel-outlined": {
        transform: "translate(14px, 9px) scale(1)"
      },
      "& .MuiInputLabel-shrink": {
        transform: "translate(14px, -9px) scale(0.75)"
      }
    },
    icon: {
      cursor: "pointer"
    }
  });

function AutoComplete(props: CustomAutocompleteProps): JSX.Element {
  const { classes } = props;
  const [text, setText] = useState<string>("");

  const handleOnChange = (
    _e: React.ChangeEvent,
    val: AutoCompleteRow | string | null
  ): void => {
    if (!val || !props.onValueChanged) return;
    if (typeof val === "string") {
      props.onValueChanged({ title: val });
      setText(val);
    } else {
      props.onValueChanged(val);
      setText(val.title);
    }
  };

  return (
    <Autocomplete
      freeSolo
      options={props.data}
      onChange={handleOnChange}
      getOptionLabel={(option) =>
        typeof option === "string" ? option : option.title
      }
      selectOnFocus
      clearOnBlur={props.defaultStyle ? false : true}
      handleHomeEndKeys
      renderInput={(params) => (
        <CustomTextField
          {...params}
          placeholder={props.defaultStyle ? undefined : props.label}
          label={props.defaultStyle ? props.label : undefined}
          variant='outlined'
          classes={{
            root: props.defaultStyle
              ? classes.textRootDefault
              : classes.textRoot
          }}
          InputLabelProps={{ shrink: props.defaultStyle ? undefined : false }}
          InputProps={{
            ...params.InputProps,
            className: props.defaultStyle ? classes.default : classes.textField,
            endAdornment: (
              <i
                onClick={(e) => {
                  e.stopPropagation();
                  props.onIconClick?.(text, e);
                }}
                className={classes.icon}
              >
                <SearchIcon />
              </i>
            )
          }}
          onChange={(evt) => {
            props.onTextFieldValueChange?.(evt.target.value);
            setText(evt.target.value);
          }}
          {...props.TextFieldProps}
        />
      )}
      {...props.AutocompleteProps}
    />
  );
}

export default withStyles(useStyles)(AutoComplete);
