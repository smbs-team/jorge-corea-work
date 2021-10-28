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
import Tooltip from "@material-ui/core/Tooltip";
import ImageIcon from "@material-ui/icons/Image";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  header?: string;
  footer?: string;
  imgSrc?: string;
  onClick?: () => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      width: 167,
      height: 191,
      cursor: "pointer"
    },
    label: {
      fontSize: "18px",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      width: "100%",
      textAlign: "center",
      display: "block",
      overflow: "hidden",
      whiteSpace: "nowrap",
      textOverflow: "ellipsis"
    },
    image: {
      display: "block",
      margin: theme.spacing(1, 0, 1, 0),
      width: 167,
      height: 139
    },
    defaultIcon: {
      fontSize: 128,
      color: theme.ptas.colors.theme.gray
    }
  });

/**
 * DataSourceCard
 *
 * @param props - Component props
 * @returns A JSX element
 */
function DataSourceCard(props: Props): JSX.Element {
  const { classes } = props;
  return (
    <Box className={classes.root} onClick={props.onClick}>
      <Tooltip title={props.header ?? ""} placement='bottom'>
        <label className={classes.label}>{props.header}</label>
      </Tooltip>
      {props.imgSrc ? (
        <img src={props.imgSrc} className={classes.image} />
      ) : (
        <ImageIcon className={classes.defaultIcon} />
      )}
      <Tooltip title={props.footer ?? ""} placement='bottom'>
        <label className={classes.label}>{props.footer}</label>
      </Tooltip>
    </Box>
  );
}

export default withStyles(useStyles)(DataSourceCard);
