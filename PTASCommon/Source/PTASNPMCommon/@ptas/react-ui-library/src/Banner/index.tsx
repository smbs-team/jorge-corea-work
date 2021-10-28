// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, PropsWithChildren, FC } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  IconButton,
  Theme
} from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";
import { GenericWithStyles } from "../common/types";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  message?: string;
  onClose?: () => void;
  color?: string | "warning" | "error" | "accent";
  hideClose?: boolean;
}

export type BannerProps = GenericWithStyles<Props>;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      backgroundColor: (props: Props) => {
        switch (props.color) {
          case "warning":
            return theme.ptas.colors.utility.warning;
          case "error":
            return theme.ptas.colors.utility.error;
          case "accent":
            return theme.ptas.colors.theme.accent;
          default:
            return props.color ?? theme.ptas.colors.utility.success;
        }
      },
      height: 50,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: "bold",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      color: "white",
      alignItems: "center",
      justifyContent: "center",
      display: "flex",
      position: "relative"
    },
    iconButton: {
      color: theme.ptas.colors.theme.white,
      position: "absolute",
      top: "-9px",
      right: "-9px",
      "&:hover": {
        backgroundColor: "unset"
      }
    },
    closeIcon: {}
  });

/**
 * Banner
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Banner(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;
  const [close, setClose] = useState<boolean>(false);

  const handleClose = () => {
    setClose(true);
    props.onClose && props.onClose();
  };

  return (
    <Fragment>
      {!close && (
        <Box
          className={classes.root}
          style={props.color ? { backgroundColor: props.color } : {}}
        >
          {props.children}
          {!props.hideClose && (
            <IconButton className={classes.iconButton} onClick={handleClose}>
              <CloseIcon />
            </IconButton>
          )}
        </Box>
      )}
    </Fragment>
  );
}

export default withStyles(useStyles)(Banner) as FC<BannerProps>;
