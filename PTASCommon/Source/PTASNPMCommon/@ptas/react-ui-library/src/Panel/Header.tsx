// Header.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme
} from "@material-ui/core";
import IconToolBar, { IconToolBarItem } from "../IconToolBar/";
import CustomBreadcrumbs from "./CustomBreadcrumbs";
import clsx from "clsx";

/**
 * Component props
 */
export interface PanelHeaderProps extends WithStyles<typeof useStyles> {
  route?: React.ReactNode[];
  title?: string;
  icons?: IconToolBarItem[];
  miscContent?: React.ReactNode;
  detailTop?: string;
  detailBottom?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      flexDirection: "row",
      backgroundColor: theme.ptas.colors.theme.grayLight,
      padding: theme.spacing(1.5, 3, 1.5, 3),
      height: 56,
      boxSizing: "content-box",
      minWidth: 740
    },
    breadcrumbs: {},
    iconToolBar: {},
    details: {
      fontSize: "0.875rem",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      textAlign: "end"
    },
    column: {
      display: "flex",
      flexDirection: "column",
      justifyContent: "space-between"
    },
    editColumn: {
      display: "flex",
      justifyContent: "center",
      marginLeft: "auto"
    },
    label: {
      display: "block"
    },
    iconToolBarContainer: {
      display: "flex",
      width: "fit-content",
      alignItems: "end"
    },
    breadContent: {},
    title: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "1rem"
    }
  });

/**
 * Header
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Header(props: PropsWithChildren<PanelHeaderProps>): JSX.Element {
  const { classes } = props;
  return (
    <Box className={classes.root}>
      <Box className={classes.column}>
        {props.title && <label className={classes.title}>{props.title}</label>}
        {props.route && <CustomBreadcrumbs
          style={{ display: "block" }}
          className={classes.breadcrumbs}
        >
          {props.route.map((r, i) => (
            <span key={i} className={classes.breadContent}>{r}</span>
          ))}
        </CustomBreadcrumbs>}
        <Box className={classes.iconToolBarContainer}>
          <IconToolBar
            icons={props.icons ?? []}
            classes={{ root: classes.iconToolBar }}
          />
          {props.miscContent}
        </Box>
      </Box>
      {(props.detailBottom || props.detailTop) && (
        <Box className={clsx(classes.column, classes.editColumn)}>
          <Box className={classes.details}>
            <label className={classes.label} style={{ marginBottom: 4 }}>
              {props.detailTop}
            </label>
            <label className={classes.label}>{props.detailBottom}</label>
          </Box>
        </Box>
      )}
      {props.children}
    </Box>
  );
}

export default withStyles(useStyles)(Header);
