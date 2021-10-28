// SearchInput.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useMemo } from "react";
import InputBase from "@material-ui/core/InputBase";
import SearchIcon from "@material-ui/icons/Search";
import {
  alpha,
  createStyles,
  InputBaseProps,
  makeStyles
} from "@material-ui/core";

export enum SearchInputClassKey {
  root = "root",
  searchIcon = "searchIcon",
  inputRoot = "inputRoot",
  inputInput = "inputInput"
}

export interface SearchInputProps {
  icon?: JSX.Element;
  classes?: Record<SearchInputClassKey, string>;
  overwriteInputBaseProps?: {
    (param: InputBaseProps): InputBaseProps;
  };
}

const useStyles = makeStyles((theme) =>
  createStyles({
    root: () => ({
      position: "relative",
      borderRadius: theme.shape.borderRadius,
      backgroundColor: alpha(theme.ptas.colors.theme.white, 0.15),
      "&:hover": {
        backgroundColor: alpha(theme.ptas.colors.theme.white, 0.25)
      },
      marginLeft: 0,
      width: "100%",
      [theme.breakpoints.up("sm")]: {
        marginLeft: theme.spacing(1),
        width: "auto"
      }
    }),
    searchIcon: () => ({
      padding: theme.spacing(0, 2),
      height: "100%",
      position: "absolute",
      pointerEvents: "none",
      display: "flex",
      alignItems: "center",
      justifyContent: "center"
    }),
    inputRoot: () => ({
      color: "inherit"
    }),
    inputInput: () => ({
      padding: theme.spacing(1, 1, 1, 0),
      // vertical padding + font size from searchIcon
      paddingLeft: `calc(1em + ${theme.spacing(4)}px)`,
      transition: theme.transitions.create("width"),
      width: "100%",
      [theme.breakpoints.up("sm")]: {
        width: "12ch",
        "&:focus": {
          width: "20ch"
        }
      }
    })
  })
);

/**
 * Create a search input with icon
 * @param props - Element props
 */
export default function SearchInput(props: SearchInputProps): JSX.Element {
  const classes = useStyles();
  const inputBaseProps = {
    placeholder: "Search…",
    classes: {
      root: classes.inputRoot,
      input: classes.inputInput
    },
    inputProps: { "aria-label": "search" }
  };
  const Icon = useMemo(() => props.icon || <SearchIcon />, [props.icon]);

  return (
    <div className={classes.root}>
      <div className={classes.searchIcon}>{Icon}</div>
      <InputBase
        {...(props.overwriteInputBaseProps
          ? props.overwriteInputBaseProps(inputBaseProps)
          : inputBaseProps)}
      />
    </div>
  );
}
