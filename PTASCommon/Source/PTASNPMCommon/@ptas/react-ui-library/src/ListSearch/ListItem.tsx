// ListItem.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import { makeStyles } from "@material-ui/core";

interface Props {
  onClick?: (isSelected: boolean) => void;
  isSelected?: boolean;
  isDeselectable?: boolean;
}

const useStyles = makeStyles((theme) => ({
  root: (props: Props) => ({
    fontSize: "0.875rem",
    fontFamily: theme.ptas.typography.bodyFontFamily,
    cursor: props.isDeselectable === false ? "default" : "pointer",
    "&:hover": {
      backgroundColor: theme.ptas.colors.theme.grayLight
    },
    color:
      props.isDeselectable === false
        ? theme.ptas.colors.theme.accent
        : props.isSelected
        ? theme.ptas.colors.utility.selection
        : "",
    fontWeight: props.isSelected ? "bold" : "normal"
  })
}));

/**
 * ListItem
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ListItem(props: PropsWithChildren<Props>): JSX.Element {
  const classes = useStyles(props);

  const handleClick = (): void => {
    if (props.isDeselectable === false && props.isSelected === true) return;
    props.onClick?.(!props.isSelected);
  };

  return (
    <li onClick={handleClick} className={classes.root}>
      {props.children}
    </li>
  );
}

export default ListItem;
