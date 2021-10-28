// CustomPopover.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, Fragment, FC } from "react";
import {
  PopoverOrigin,
  Box,
  WithStyles,
  createStyles,
  withStyles
} from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import Popover, { PopoverPosition } from "@material-ui/core/Popover";
import { Close } from "@material-ui/icons";
import { GenericWithStyles } from "@ptas/react-ui-library";
import clsx from "clsx";
import { usePopoverPosition } from "./usePopoverPosition";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  anchorEl: HTMLElement | null | undefined;
  children: React.ReactNode;
  anchorOrigin?: PopoverOrigin | undefined;
  anchorPosition?: PopoverPosition;
  transformOrigin?: PopoverOrigin | undefined;
  marginThreshold?: number | undefined;
  onClose: () => void;
  disableBackdropClick?: boolean;
  tail?: boolean;
  border?: boolean;
  showCloseButton?: boolean;
  ptasVariant?: "danger" | "info" | "success";
  tailPosition?: "start" | "center" | "end";
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    paper: {
      overflow: "visible",
      borderRadius: 9,
      maxHeight: "calc(100% - 90px)"
    },
    root: {},
    border: {
      border: `1px solid ${theme.ptas.colors.theme.black}`
    },
    tail: (props: Props) => ({
      backgroundColor: setColorByVariant(props.ptasVariant, theme),
      transform: "rotate(45deg)",
      position: "absolute",
      top: -7,
      width: 15,
      height: 15,
      right: 40,
      boxShadow: "-2px -2px 4px -3px rgba(0,0,0,0.14)",
      clipPath: "inset(-5px 0px 0px -5px)"
    }),
    tailStart: {
      left: 40
    },
    tailCenter: {
      right: "inherit !important",
      left: "49%"
    },
    closeButton: (props: Props) => ({
      position: "absolute",
      top: 3,
      right: 3,
      cursor: "pointer",
      color:
        props.ptasVariant === "info" || props.ptasVariant === undefined
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white
    }),
    closeIcon: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize
    },
    backdropRoot: {
      background: "transparent !important"
    }
  });

const setColorByVariant = (
  variant: "danger" | "info" | "success" | undefined,
  theme: Theme
): string => {
  switch (variant) {
    case "danger":
      return theme.ptas.colors.utility.danger;
    case "info":
      return theme.ptas.colors.theme.white;
    case "success":
      return theme.ptas.colors.theme.accentDark;

    default:
      return theme.ptas.colors.theme.white;
  }
};

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
    marginThreshold,
    tail,
    border,
    classes,
    tailPosition,
    anchorPosition,
    disableBackdropClick
  } = props;
  const [popoverEl, setPopoverEl] = useState<HTMLElement | null>(null);
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null | undefined>(
    null
  );
  const { anchorOrigin, transformOrigin, tailRef } = usePopoverPosition(
    props.anchorOrigin,
    props.transformOrigin,
    anchorEl,
    popoverEl
  );

  // handleClick from outside
  useEffect(() => {
    setAnchorEl(props.anchorEl);
  }, [props.anchorEl]);

  const handleClose = () => {
    setAnchorEl(null);
    onClose();
  };

  const setTailClassByPosition = (): string => {
    switch (tailPosition) {
      case "start":
        return clsx(classes.tail, classes.tailStart);
      case "center":
        return clsx(classes.tail, classes.tailCenter);
      default:
        return classes.tail;
    }
  };

  const open = Boolean(anchorEl);
  const id = open ? "simple-popover" : undefined;

  return (
    <Fragment>
      <Popover
        id={id}
        ref={(ref) => {
          setPopoverEl(ref as HTMLElement);
        }}
        open={open}
        anchorEl={anchorEl}
        onClose={handleClose}
        disableBackdropClick={disableBackdropClick}
        BackdropProps={{
          className: classes.backdropRoot
        }}
        PaperProps={{
          //Setting id to paper element, because the following way of getting a ref
          // to the paper element causes the popover to lose its correct position,
          // even though the anchor is set.
          // ref: (ref) => {
          //   popoverPaperRef.current = ref;
          // }
          id: "poPaperEl"
        }}
        anchorPosition={anchorPosition}
        anchorOrigin={
          anchorPosition
            ? undefined
            : anchorOrigin ?? {
                vertical: "center",
                horizontal: "center"
              }
        }
        transformOrigin={
          anchorPosition
            ? undefined
            : transformOrigin ?? {
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
      >
        {tail && <div ref={tailRef} className={setTailClassByPosition()} />}
        {children}
        {props.showCloseButton && (
          <Box onClick={handleClose} className={classes.closeButton}>
            <Close classes={{ root: classes.closeIcon }} />
          </Box>
        )}
      </Popover>
    </Fragment>
  );
}

export default withStyles(useStyles)(CustomPopover) as FC<
  GenericWithStyles<Props>
>;
