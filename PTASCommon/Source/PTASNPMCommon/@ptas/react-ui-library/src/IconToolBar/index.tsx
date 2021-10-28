// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, PropsWithChildren } from "react";
import { withStyles, WithStyles, createStyles, Box } from "@material-ui/core";
import CustomIconButton from "../CustomIconButton";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  icons: IconToolBarItem[];
}

export interface IconToolBarItem {
  id?: React.ReactText;
  icon: React.ReactNode;
  text?: string;
  disabled?: boolean;
  onClick?: (
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => void | Promise<void>;
  node?: React.ReactNode;
  nodePosition?: "left" | "right";
}

/**
 * Component styles
 */
const useStyles = () =>
  createStyles({
    root: {},
    customIconButton: {
      marginRight: 16
    }
  });

/**
 * IconToolBar
 *
 * @param props - Component props
 * @returns A JSX element
 */
function IconToolBar(props: PropsWithChildren<Props>): JSX.Element {
  return (
    <Box className={props.classes.root}>
      {props.icons.map((item, i) => (
        <Fragment key={"icon-" + i}>
          {item.nodePosition === "left" ? item.node : ""}
          <CustomIconButton
            key={i}
            icon={item.icon}
            text={item.text}
            classes={{ root: props.classes.customIconButton }}
            onClick={item.onClick}
            disabled={item.disabled}
          />
          {item.nodePosition === "right" ? item.node : ""}
        </Fragment>
      ))}
    </Box>
  );
}

export default withStyles(useStyles)(IconToolBar);
