// Multimedia.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from "react";
import {
  createStyles,
  Theme,
  WithStyles,
  withStyles
} from "@material-ui/core/styles";
import { CustomSearchTextField, Suggestion } from "../CustomSearchTextField";
import { ReactComponent as Location } from "../assets/Icons/location-icon.svg";
import clsx from "clsx";

interface Props extends WithStyles<typeof useStyles> {
  suggestion?: Suggestion;
  onClickSearch?: () => void;
  textDescription: string | React.ReactNode;
  textButton?: string | React.ReactNode;
  onClickTextButton?: () => void;
  label?: string | React.ReactNode;
  value?: string;
  onChange?: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => void;
  autoFocus?: boolean;
  onChangeDelay?: number;
}

/**
 * Component styles
 */

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      position: "relative",
      background: "#505150",
      borderRadius: 24,
      width: "100%",
      maxWidth: 320,
      padding: "16px 10px 14px",

      [theme.breakpoints.up("sm")]: {
        padding: "16px 0px 14px"
      }
    },
    wrapper: {
      maxWidth: 260,
      marginBottom: 5,
      marginLeft: "auto",
      marginRight: "auto",
      [theme.breakpoints.up("sm")]: {
        maxWidth: 288
      }
    },
    description: {
      color: theme.ptas.colors.theme.white,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      maxWidth: 87,
      width: "100%",
      display: "block",

      [theme.breakpoints.up("sm")]: {
        maxWidth: 130
      }
    },
    fullWidthDescription: {
      maxWidth: "100%",
      padding: "0 31px"
    },
    contentWrap: {
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      width: "100%",
      maxWidth: 270,
      margin: "0 auto"
    },
    orText: {
      fontSize: theme.ptas.typography.finePrint.fontSize,
      color: "rgba(255, 255, 255, 0.5)",
      marginRight: "3px",
      marginLeft: 9,
      [theme.breakpoints.up("sm")]: {
        marginLeft: 19
      }
    },
    textButton: {
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
      fontSize: 14,
      color: theme.ptas.colors.theme.accentLight,
      border: "none",
      background: "transparent",
      outline: "none",
      display: "flex",
      cursor: "pointer"
    },
    icon: {
      marginRight: 4
    },
    outlineRoot: {
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `transparent !important`
      }
    },
    animated: {
      fontSize: theme.ptas.typography.body.fontSize
    },
    outlineInput: {
      paddingLeft: 12
    }
  });

const PropertySearcher = (props: Props) => {
  const { classes } = props;

  const {
    onClickSearch,
    suggestion,
    textDescription,
    label,
    textButton,
    onChange,
    onClickTextButton,
    value,
    autoFocus,
    onChangeDelay
  } = props;

  const handleClickTextButton = (): void => {
    if (onClickTextButton) onClickTextButton();
  };

  return (
    <div className={classes.root}>
      <CustomSearchTextField
        ptasVariant='outline'
        onClick={onClickSearch}
        suggestion={suggestion}
        label={label ?? ""}
        value={value}
        onChange={onChange}
        autoFocus={autoFocus}
        onChangeDelay={onChangeDelay}
        classes={{
          wrapper: classes.wrapper,
          outlineRoot: classes.outlineRoot,
          animated: classes.animated,
          outlineInput: classes.outlineInput
        }}
      />
      <div className={classes.contentWrap}>
        <span
          className={
            textButton
              ? classes.description
              : clsx(classes.description, classes.fullWidthDescription)
          }
        >
          {textDescription}
        </span>
        {textButton && (
          <Fragment>
            <span className={classes.orText}>or</span>
            <button
              className={classes.textButton}
              onClick={handleClickTextButton}
            >
              <Location className={classes.icon} />
              {textButton}
            </button>
          </Fragment>
        )}
      </div>
    </div>
  );
};

export default withStyles(useStyles)(PropertySearcher);
