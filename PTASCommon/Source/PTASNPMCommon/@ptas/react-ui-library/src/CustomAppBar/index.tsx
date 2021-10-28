// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import {
  AppBar,
  Toolbar,
  createStyles,
  WithStyles,
  AppBarProps,
  withStyles,
  ToolbarProps
} from "@material-ui/core";

interface Props extends WithStyles<typeof styles> {
  AppBarProps?: AppBarProps;
  ToolbarProps?: ToolbarProps;
}

const styles = () =>
  createStyles({
    appBar: {
      backgroundColor: "#001433",
      boxShadow: "none"
    },
    toolbar: {}
  });

function CustomAppBar(props: PropsWithChildren<Props>): JSX.Element {
  const { children, classes } = props;

  return (
    <AppBar position='sticky' className={classes.appBar}>
      <Toolbar
        variant='dense'
        className={classes.toolbar}
        {...props.ToolbarProps}
      >
        {children}
      </Toolbar>
    </AppBar>
  );
}

export default withStyles(styles)(CustomAppBar);
