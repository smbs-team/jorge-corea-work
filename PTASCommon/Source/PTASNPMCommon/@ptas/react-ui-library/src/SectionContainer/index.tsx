// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, memo } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  LinearProgress
} from "@material-ui/core";

/**
 * Component props
 */
export interface SectionContainerProps extends WithStyles<typeof useStyles> {
  title?: string;
  details?: React.ReactNode;
  miscContent?: React.ReactNode;
  icon?: React.ReactNode;
  isLoading?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: "100%"
    },
    header: {
      height: 33,
      display: "flex",
      alignItems: "center",
      borderBottom: "1px solid gray"
    },
    title: {
      color: theme.ptas.colors.theme.black,
      fontSize: "1.5rem",
      fontWeight: "bold",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginLeft: theme.spacing(3)
    },
    details: {
      marginLeft: "auto",
      color: theme.ptas.colors.theme.black,
      fontSize: "0.875rem",
      fontWeight: "normal",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginRight: theme.spacing(3)
    },
    contentContainer: {
      padding: theme.spacing(2, 3, 2, 3)
    },
    linearProgress: {},
    headerContainer: {
      height: 36
    }
  });

/**
 * SectionContainer
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SectionContainer(props: PropsWithChildren<SectionContainerProps>): JSX.Element {
  const { classes, title, details, children, miscContent, icon } = props;

  return (
    <Box className={classes.root}>
      <Box className={classes.headerContainer}>
      <Box className={classes.header}>
        {icon}
        <label className={classes.title}>{title}</label>
        {miscContent}
        <span className={classes.details}>{details}</span>
      </Box>
      {props.isLoading && <LinearProgress className={classes.linearProgress} />}
      </Box>
      <Box className={classes.contentContainer}>{children}</Box>
    </Box>
  );
}

export default withStyles(useStyles)(memo(SectionContainer));
