// CustomPopover.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  PopoverOrigin,
  Theme,
  Box
} from "@material-ui/core";
import Popover from "@material-ui/core/Popover";
import { Close } from "@material-ui/icons";
import { TransitionProps } from "@material-ui/core/transitions/transition";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  anchorEl: HTMLElement | null | undefined;
  children: React.ReactNode;
  anchorOrigin?: PopoverOrigin | undefined;
  transformOrigin?: PopoverOrigin | undefined;
  marginThreshold?: number | undefined;
  onClose?: () => void;
  tail?: boolean;
  border?: boolean;
  showCloseButton?: boolean;
  TransitionProps?: TransitionProps;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    paper: {
      borderRadius: 8,
      overflow: "visible"
    },
    root: {},
    border: {
      border: `1px solid ${theme.ptas.colors.theme.black}80`
    },
    tail: {
      backgroundColor: theme.ptas.colors.theme.white,
      transform: "rotate(45deg)",
      position: "absolute",
      top: -8,
      right: 40,
      width: 15,
      height: 15,
      boxShadow:
        "0px 3px 3px -2px rgba(0,0,0,0.2),0px 3px 4px 0px rgba(0,0,0,0.14),0px 1px 8px 0px rgba(0,0,0,0.12)",
      clipPath: "inset(-5px 0px 0px -5px)"
    },
    closeButton: {
      position: "absolute",
      top: theme.spacing(1),
      right: theme.spacing(1),
      cursor: "pointer",
      color: theme.ptas.colors.theme.black
    },
    closeIcon: {
      width: 30,
      height: 30
    }
  });

/**
 * CustomPopover
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomPopover(props: Props): JSX.Element {
  const {
    children,
    onClose,
    anchorOrigin,
    transformOrigin,
    classes,
    marginThreshold,
    tail,
    border
  } = props;

  const [anchorEl, setAnchorEl] = useState<HTMLElement | null | undefined>(
    null
  );

  // handleClick from outside
  useEffect(() => {
    setAnchorEl(props.anchorEl);
  }, [props.anchorEl]);

  const handleClose = () => {
    setAnchorEl(null);
    onClose?.();
  };

  const open = Boolean(anchorEl);
  const id = open ? "simple-popover" : undefined;

  return (
    <Popover
      keepMounted
      id={id}
      open={open}
      anchorEl={anchorEl}
      onClose={handleClose}
      anchorOrigin={
        anchorOrigin ?? {
          vertical: "center",
          horizontal: "center"
        }
      }
      transformOrigin={
        transformOrigin ?? {
          vertical: "top",
          horizontal: "left"
        }
      }
      marginThreshold={marginThreshold}
      elevation={3}
      classes={{
        paper: `${classes.paper} ${border ? classes.border : ""}`,
        root: classes.root
      }}
      onClick={(e): void => e.stopPropagation()}
      TransitionProps={props.TransitionProps}
    >
      {tail && <div className={classes.tail} />}
      {children}
      {props.showCloseButton && (
        <Box onClick={handleClose} className={classes.closeButton}>
          <Close classes={{ root: classes.closeIcon }} />
        </Box>
      )}
    </Popover>
  );
}

export default withStyles(useStyles)(CustomPopover);
