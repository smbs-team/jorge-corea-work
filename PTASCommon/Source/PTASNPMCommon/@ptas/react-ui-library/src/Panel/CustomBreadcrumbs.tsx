// CustomBreadcrumbs.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  Breadcrumbs,
  BreadcrumbsProps
} from "@material-ui/core";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      fontSize: "1rem",
      color: theme.ptas.colors.theme.black,
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    separator: {
      margin: theme.spacing(0, 1)
    },
    li: {
      "& a": {
        color: theme.ptas.colors.theme.black
      }
    }
  });

/**
 * CustomBreadcrumbs
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomBreadcrumbs(
  props: PropsWithChildren<Props> & BreadcrumbsProps
): JSX.Element {
  return (
    <Breadcrumbs separator='>' {...props}>
      {props.children}
    </Breadcrumbs>
  );
}

export default withStyles(useStyles)(CustomBreadcrumbs);
