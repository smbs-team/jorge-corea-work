/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import Toolbar from "@material-ui/core/Toolbar";
import { Theme, withStyles, WithStyles, createStyles } from "@material-ui/core";
import { GenericWithStyles } from "../common";

/**
 * Component props
 */
type Props = WithStyles<typeof useStyles>;
export type SubToolBarProps = GenericWithStyles<Props>;

/**
 * @param theme -The Material ui Theme
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      color: theme.ptas.colors.theme.white,
      backgroundColor: theme.ptas.colors.theme.gray,
      minHeight: 32
    }
  });

/**
 * Sub Tool bar
 *
 * @param props - Component props
 */
function SubToolBar(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;
  return <Toolbar className={classes.root}>{props.children}</Toolbar>;
}

export default withStyles(useStyles)(SubToolBar);
