// banner.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useState, useEffect } from "react";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import { createStyles, withStyles, WithStyles, Theme } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";

type colorType = "warning" | "danger" | "success";

interface Props extends WithStyles<typeof useStyles> {
  handleClose?: () => void;
  color?: string | colorType;
  message: string | JSX.Element;
  open?: boolean;
  textColor?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: ({ color, textColor }: Props) => {
      const backgroundColor = getColorCodeByColor(color || "", theme)
        .bannerBackground;
      const colorMessage = getColorCodeByColor(color || "", theme).messageColor;

      return {
        width: "100%",
        height: 36,
        background: backgroundColor,
        color: colorMessage ?? textColor,
        textAlign: "center",
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontWeight: 700,
        fontSize: 16,
        paddingTop: 7,
        boxSizing: "border-box",
        position: "relative",
        "&.hide": {
          display: "none"
        }
      };
    },
    closeButton: {
      padding: 7,
      position: "absolute",
      right: 0,
      top: -5
    },
    closeIcon: {
      fontSize: 33
    }
  });

const getColorCodeByColor = (
  color: string,
  theme: Theme
): {
  bannerBackground: string;
  messageColor: string;
} => {
  if (!color)
    return {
      bannerBackground: theme.ptas.colors.utility.changed,
      messageColor: theme.ptas.colors.theme.black
    };

  switch (color) {
    case "warning":
      return {
        bannerBackground: theme.ptas.colors.utility.changed,
        messageColor: theme.ptas.colors.theme.black
      };
    case "danger":
      return {
        bannerBackground: theme.ptas.colors.utility.danger,
        messageColor: theme.ptas.colors.theme.white
      };
    case "success":
      return {
        bannerBackground: theme.ptas.colors.utility.success,
        messageColor: theme.ptas.colors.theme.white
      };
    default:
      return {
        bannerBackground: color,
        messageColor: ""
      };
  }
};

const Banner = (props: Props): JSX.Element => {
  const [show, setShow] = useState<boolean>(false);

  const { classes, handleClose, message, open, textColor } = props;

  useEffect(() => {
    setShow(!!open);
  }, [open]);

  const onCloseBanner = () => {
    setShow(false);
    handleClose?.();
  };

  return (
    <div className={`${classes.root} ${!show && "hide"}`}>
      <span style={{ color: textColor }}>{message}</span>
      <IconButton
        aria-label='close'
        color='inherit'
        className={classes.closeButton}
        onClick={onCloseBanner}
        style={{ color: textColor }}
      >
        <CloseIcon className={classes.closeIcon} />
      </IconButton>
    </div>
  );
};

export default withStyles(useStyles)(Banner) as FC<GenericWithStyles<Props>>;
