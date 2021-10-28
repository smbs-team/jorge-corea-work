// CustomButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment, PropsWithChildren } from "react";
import { createStyles, Theme, WithStyles, withStyles } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import clsx from "clsx";
import ArrowBackIosIcon from "@material-ui/icons/ArrowBackIos";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { GenericWithStyles } from "../common";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  ptasVariant?:
    | "Clear"
    | "Text"
    | "Text large"
    | "Text more"
    | "Text more small"
    | "Text more small min"
    | "Inverse more blue"
    | "Inverse more"
    | "Inverse clear"
    | "Danger more";
  disabled?: boolean;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  variant?: "contained" | "outlined" | "text";
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      backgroundColor: "transparent",
      padding: 0,
      fontSize: theme.ptas.typography.body.fontSize,
      color: theme.ptas.colors.theme.accent,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      border: "none",
      "&:hover": {
        background: "transparent"
      },
      "&:focus": {
        textDecoration: "underline"
      },
      "&:disabled": {
        opacity: theme.ptas.opacity.disabled,
        color: theme.ptas.colors.theme.accent
      }
    },
    rootLabel: {
      transition: "0.5s ease",
      "&:hover": {
        textShadow: "1px 8px 11px rgba(128,161,184,0.90) !important"
      },
      "&:focus": {
        textDecoration: "underline"
      }
    },
    clear: {
      color: `${theme.ptas.colors.theme.accent} !important`
    },
    inverseClear: {
      color: `${theme.ptas.colors.theme.white} !important`,
      "&:disabled": {
        color: theme.ptas.colors.theme.white
      }
    },
    inverseClearLabel: {
      "&:hover": {
        textShadow: "1px 8px 11px rgba(255,255,255,0.90) !important"
      }
    },
    textLarge: {
      fontSize: `${theme.ptas.typography.buttonLarge.fontSize} !important`
    },
    text: {
      fontSize: `${theme.ptas.typography.bodySmall.fontSize} !important`
    },
    textMoreSmallMin: {
      fontSize: `${theme.ptas.typography.finePrint} !important`,
      color: `${theme.ptas.colors.theme.black} !important`
    },
    textMoreSmallMinLabel: {
      "&:hover": {
        textShadow: "1px 8px 11px rgba(0,0,0, 0.9) !important"
      }
    },
    inverseMoreBlue: {
      fontSize: `${theme.ptas.typography.buttonLarge.fontSize} !important`,
      color: `${theme.ptas.colors.theme.accentLight} !important`
    },
    inverseMoreBlueLabel: {
      "&:hover": {
        textShadow: `1px 8px 11px ${theme.ptas.colors.theme.accentDark} !important`
      }
    },
    inverseMoreLabel: {
      "&:hover": {
        textShadow: `1px 8px 11px rgba(255, 255, 255, 255) !important`
      }
    },
    dangerMore: {
      fontSize: `${theme.ptas.typography.bodySmall.fontSize} !important`,
      color: `${theme.ptas.colors.utility.danger} !important`
    },
    dangerMoreLabel: {
      "&:hover": {
        textShadow: `1px 8px 11px ${theme.ptas.colors.utility.danger} !important`
      }
    },
    smallText: { fontSize: 14 },
    expandMoreIcon: ({ ptasVariant }: Props) => ({
      color:
        ptasVariant === "Inverse more" || ptasVariant === "Inverse more blue"
          ? theme.ptas.colors.theme.white
          : theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.body.fontSize
    }),
    arrowBackIcon: {
      fontSize: 14
    }
  });
/**
 * CustomButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomButton(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;

  const setVariant = (): string => {
    switch (props.ptasVariant) {
      case "Clear":
        return clsx(classes.root, classes.clear);
      case "Inverse clear":
        return clsx(classes.root, classes.inverseClear);
      case "Text large":
        return clsx(classes.root, classes.textLarge);
      case "Text":
        return clsx(classes.root, classes.text);
      case "Text more":
        return clsx(classes.root, classes.text);
      case "Text more small min":
        return clsx(classes.root, classes.textMoreSmallMin);
      case "Inverse more blue":
        return clsx(classes.root, classes.inverseMoreBlue);
      case "Inverse more":
        return clsx(classes.root, classes.inverseClear);
      case "Danger more":
        return clsx(classes.root, classes.dangerMore);
      default:
        return classes.root;
    }
  };

  const setLabelVariant = (): string => {
    switch (props.ptasVariant) {
      case "Clear":
      case "Text large":
      case "Text":
      case "Text more":
        return classes.rootLabel;
      case "Text more small":
        return clsx(classes.rootLabel, classes.smallText);
      case "Inverse clear":
        return classes.inverseClearLabel;
      case "Text more small min":
        return classes.textMoreSmallMinLabel;
      case "Inverse more blue":
        return classes.inverseMoreBlueLabel;
      case "Inverse more":
        return classes.inverseMoreLabel;
      case "Danger more":
        return classes.dangerMoreLabel;
      default:
        return classes.rootLabel;
    }
  };

  const renderArrowIcon = (): JSX.Element =>
    props.ptasVariant === "Clear" || props.ptasVariant === "Inverse clear" ? (
      <ArrowBackIosIcon className={classes.arrowBackIcon} />
    ) : (
      <Fragment />
    );

  const renderExpandMoreIcon = (): JSX.Element =>
    props.ptasVariant === "Text more" ||
    props.ptasVariant === "Text more small" ||
    props.ptasVariant === "Text more small min" ||
    props.ptasVariant === "Inverse more blue" ||
    props.ptasVariant === "Danger more" ||
    props.ptasVariant === "Inverse more" ? (
      <ExpandMoreIcon className={classes.expandMoreIcon} />
    ) : (
      <Fragment />
    );

  return (
    <Button
      disabled={props.disabled}
      className={setVariant()}
      classes={{ label: setLabelVariant() }}
      onClick={props.onClick}
      variant={props.variant ?? "text"}
    >
      {renderArrowIcon()}
      {props.children}
      {renderExpandMoreIcon()}
    </Button>
  );
}

export default withStyles(useStyles)(CustomButton) as FC<
  GenericWithStyles<Props>
>;
