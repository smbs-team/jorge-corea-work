// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

 import React from "react";
import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core";
import { PropsWithChildren } from "react";
import { CustomButton } from "../CustomButton";

interface Props extends WithStyles<typeof useStyles> {
  title?: string;
  content?: string;
  buttonText?: string;
  onButtonClick?: () => void;
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      flexDirection: "column",
      backgroundColor: "white",
      width: 388,
      padding: theme.spacing(1.75, 4.25, 4.25, 4.25),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      border: "1px solid",
      borderColor: "rgba(0,0,0, 0.5)",
      borderRadius: 8
    },
    title: {
      fontSize: "1.375rem",
    },
    content: {
      fontSize: "0.875rem",
      marginTop: theme.spacing(1.625),
      marginBottom: theme.spacing(1.625),
    },
    button: {
      width: 111,
      alignSelf: "flex-end",
    },
  });

function CommAlert(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;

  return (
    <div className={classes.root}>
      <div className={classes.title}>{props.title}</div>
      <div className={classes.content}>{props.content || props.children}</div>
      <CustomButton
        classes={{ root: classes.button }}
        onClick={props.onButtonClick}
        fullyRounded
      >
        {props.buttonText ?? "Ok"}
      </CustomButton>
    </div>
  );
}

export default withStyles(useStyles)(CommAlert);
