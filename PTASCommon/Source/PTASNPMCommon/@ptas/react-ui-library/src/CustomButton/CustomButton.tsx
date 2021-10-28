// CustomButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { PropsWithChildren } from "react";
import { ButtonProps, makeStyles } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import clsx from "clsx";
import { omit } from "lodash";
import React from "react";
import { forwardRef } from "react";

/**
 * Component props
 */
interface Props extends PropsWithChildren<ButtonProps> {
  ptasVariant?: "secondary" | "outlined" | "inverse" | "commercial";
  short?: boolean;
  fullyRounded?: boolean;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme) => ({
  root: (props: Props) => ({
    backgroundColor: theme.ptas.colors.theme.accent,
    boxShadow: "0px 2px 6px 0px rgba(128,161,184,0.69)",
    height: 36,
    minWidth: 105,
    fontSize: theme.ptas.typography.body.fontSize,
    background:
      "linear-gradient(240deg, rgba(0,143,184,1) 0%, rgba(24,112,137,1) 100%)",
    fontWeight: "bold",
    textTransform: "none",
    color: theme.ptas.colors.theme.white,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    borderRadius: props.fullyRounded ? "24px" : "9px",
    "&:hover": {
      boxShadow: "0px 7px 21px 0px rgba(128,161,184,0.69)"
    },
    "&:focus": {
      textDecoration: "underline"
    }
  }),
  secondary: {
    background:
      "linear-gradient(240deg, rgba(235,249,253,1) 0%, rgba(205,244,255,1) 100%)" +
      "!important",
    color: theme.ptas.colors.theme.accent + "!important"
  },
  outlinedCustom: {
    background: theme.ptas.colors.theme.white + "!important",
    border: "1px solid !important",
    borderColor: theme.ptas.colors.theme.accent + "!important",
    color: theme.ptas.colors.theme.accent + "!important"
  },
  inverseCustom: {
    background: theme.ptas.colors.theme.white + "!important",
    color: theme.ptas.colors.theme.accent + "!important"
  },
  disabled: {
    opacity: (props: Props) => (props.disabled ? "50%" : "100%"),
    color: (props: Props) =>
      props.ptasVariant
        ? theme.ptas.colors.theme.accent + "!important"
        : theme.ptas.colors.theme.white + "!important"
  },
  commercial: {
    backgroundColor: "#169db3 !important",
    background: "none !important",
    height: "32px !important",
    borderRadius: "3px !important",
    border: "1px solid #0a5f78",
    padding: "6px 14px !important",
    minWidth: "unset !important",
    width: "fit-content"
  }
}));

/**
 * CustomButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
const CustomButton = forwardRef<HTMLButtonElement, Props>(
  (props: Props, ref): JSX.Element => {
    const classes = useStyles(props);
    const newProps = omit(props, ["ptasVariant", "short", "fullyRounded"]);

    const setVariant = (): string => {
      switch (props.ptasVariant) {
        case "secondary":
          return clsx(classes.root, classes.secondary);
        case "outlined":
          return clsx(classes.root, classes.outlinedCustom);
        case "inverse":
          return clsx(classes.root, classes.inverseCustom);
        case "commercial":
          return clsx(classes.root, classes.commercial);
        default:
          return classes.root;
      }
    };

    return (
      <Button
        {...newProps}
        className={setVariant()}
        classes={{ disabled: classes.disabled }}
        ref={ref}
      >
        {props.children}
      </Button>
    );
  }
);

export default CustomButton;
