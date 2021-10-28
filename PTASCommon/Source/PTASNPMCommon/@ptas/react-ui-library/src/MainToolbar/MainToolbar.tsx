// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Toolbar,
  Theme
} from "@material-ui/core";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  hide?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      zIndex: 1,
      color: theme.ptas.colors.theme.black,
      minHeight: "auto"
    }
  });

function MainToolbar(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;
  return <Toolbar className={classes.root}>{props.children}</Toolbar>;
}

export default withStyles(useStyles)(MainToolbar);
