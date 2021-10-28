// CustomButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, PropsWithChildren } from "react";
import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import clsx from "clsx";
import LoopIcon from "@material-ui/icons/Loop";
import { GenericWithStyles } from "@ptas/react-ui-library";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  ptasVariant?:
    | "Primary"
    | "Secondary"
    | "Outline"
    | "Inverse"
    | "Danger inverse"
    | "Danger"
    | "Roundish"
    | "Sharp"
    | "Large"
    | "Short"
    | "Slim"
    | "Slim outline"
    | "Inverse slim";
  disabled?: boolean;
  isActive?: boolean;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      backgroundColor: theme.ptas.colors.theme.accent,
      boxShadow: "0px 2px 6px 0px rgba(128,161,184,0.69)",
      height: 38,
      minWidth: 107,
      fontSize: theme.ptas.typography.body.fontSize,
      opacity: props.disabled ? theme.ptas.opacity.disabled : 1,
      background:
        "linear-gradient(240deg, rgba(0,143,184,1) 0%, rgba(24,112,137,1) 100%)",
      fontWeight: "bold",
      textTransform: "none",
      color: theme.ptas.colors.theme.white,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      borderRadius: 20,
      "&:hover": {
        boxShadow: "1px 10px 10px rgba(24, 112, 137, 0.5)"
      },
      "&:focus": {
        textDecoration: "underline"
      },

      "&.MuiButton-root.Mui-disabled": {
        color: theme.ptas.colors.theme.white
      }
    }),
    primary: {
      color: `${theme.ptas.colors.theme.white} !important`
    },
    "&:focus": {
      textDecoration: "underline"
    },
    secondary: {
      background: ` linear-gradient(90deg, #CDF4FF 0%, #EBF9FD 100.15%) !important`,
      color: `${theme.ptas.colors.theme.accent} !important`,
      "&:focus": {
        textDecoration: "none !important"
      },
      "&:hover": {
        boxShadow: "1px 10px 10px rgba(24, 112, 137, 0.5) !important"
      }
    },
    outline: {
      background: `${theme.ptas.colors.theme.white} !important`,
      border: "1px solid !important",
      borderColor: `${theme.ptas.colors.theme.accent} !important`,
      color: `${theme.ptas.colors.theme.accent} !important`,
      "&:hover": {
        boxShadow: "1px 10px 10px rgba(24, 112, 137, 0.5) !important"
      }
    },
    inverse: {
      background: `${theme.ptas.colors.theme.white} !important`,
      color: `${theme.ptas.colors.theme.accent} !important`,
      "&:hover": {
        boxShadow: "1px 10px 10px rgba(255, 255, 255, 0.5) !important"
      }
    },
    dangerInverse: {
      background: `${theme.ptas.colors.theme.white} !important`,
      color: `${theme.ptas.colors.utility.danger} !important`,
      "&:hover": {
        boxShadow: "1px 10px 10px rgba(255, 255, 255, 0.5) !important"
      }
    },
    danger: {
      background: `${theme.ptas.colors.theme.white} !important`,
      color: `${theme.ptas.colors.utility.danger} !important`,
      border: `0.5px solid ${theme.ptas.colors.utility.danger}`,
      "&:hover": {
        boxShadow: "0px 2px 2px rgba(183, 15, 10, 0.5) !important"
      }
    },
    roundish: {
      borderRadius: "9px !important"
    },
    sharp: {
      borderRadius: "0px !important"
    },
    large: {
      height: "48px !important",
      minWidth: "167px !important",
      borderRadius: 24
    },
    short: {
      height: "30px !important",
      borderRadius: 24
    },
    slim: {
      height: "23px !important",
      minWidth: "77px !important",
      fontSize: "14px !important",
      borderRadius: 24
    },
    slimOutline: {
      background: `${theme.ptas.colors.theme.white} !important`,
      border: "1px solid !important",
      borderColor: `${theme.ptas.colors.theme.accent} !important`,
      color: `${theme.ptas.colors.theme.accent} !important`,
      height: "23px !important",
      minWidth: "77px !important",
      fontSize: "14px !important"
    },
    inverseSlim: {
      background: `${theme.ptas.colors.theme.white} !important`,
      border: "none !important",
      color: `${theme.ptas.colors.theme.accent} !important`,
      height: "19px !important",
      minWidth: "70px !important",
      fontSize: "14px !important",
      "&:hover": {
        boxShadow: "0px 5px 8px 2px rgba(255,355,255,0.90) !important"
      }
    }
  });
/**
 * CustomButton
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomButton(props: PropsWithChildren<Props>): JSX.Element {
  const { classes, children, onClick, disabled } = props;

  const setVariant = (): string => {
    switch (props.ptasVariant) {
      case "Primary":
        return clsx(classes.root, classes.primary);
      case "Secondary":
        return clsx(classes.root, classes.secondary);
      case "Outline":
        return clsx(classes.root, classes.outline);
      case "Inverse":
        return clsx(classes.root, classes.inverse);
      case "Danger inverse":
        return clsx(classes.root, classes.dangerInverse);
      case "Danger":
        return clsx(classes.root, classes.danger);
      case "Roundish":
        return clsx(classes.root, classes.roundish);
      case "Sharp":
        return clsx(classes.root, classes.sharp);
      case "Large":
        return clsx(classes.root, classes.large);
      case "Short":
        return clsx(classes.root, classes.short);
      case "Slim":
        return clsx(classes.root, classes.slim);
      case "Slim outline":
        return clsx(classes.root, classes.slimOutline);
      case "Inverse slim":
        return clsx(classes.root, classes.inverseSlim);
      default:
        return classes.root;
    }
  };

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>): void => {
    if (onClick) onClick(event);
  };

  return (
    <Button className={setVariant()} onClick={handleClick} disabled={disabled}>
      {props.isActive && <LoopIcon />}
      {children}
    </Button>
  );
}

export default withStyles(useStyles)(CustomButton) as FC<
  GenericWithStyles<Props>
>;
