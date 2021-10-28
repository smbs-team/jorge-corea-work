// Footer.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { withStyles, createStyles, WithStyles } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";

interface Props extends WithStyles<typeof useStyles> {}

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonLarge.fontSize,
      backgroundColor: theme.ptas.colors.theme.black,
      color: theme.ptas.colors.theme.white,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight,
      display: "flex",
      justifyContent: "center",
      paddingTop: 46,
      paddingBottom: 54
    },
    column: {
      width: 215,
      height: 121,
      marginTop: 0,

      "&:last-child": {
        marginTop: 64
      },

      [theme.breakpoints.up("sm")]: {
        "&:last-child": {
          marginTop: 0
        }
      }
    },
    wrapper: {
      display: "flex",
      width: "100%",
      maxWidth: "492px",
      justifyContent: "center",
      alignItems: "center",
      flexDirection: "column",

      [theme.breakpoints.up("sm")]: {
        justifyContent: "space-between",
        flexDirection: "row"
      }
    },
    marginLeft: {
      marginLeft: 62
    },
    item: {
      marginBottom: 32,
      display: "block",
      "&:last-child": {
        marginBottom: 0
      }
    }
  });

/**
 * Footer
 *
 * @param props - Component props
 * @returns A JSX element
 */

function Footer(props: Props): JSX.Element {
  const { classes } = props;

  const defaultValues: string[] = [
    "Contact us",
    "Customer service",
    "206-296-3920",
    "Privacy policy",
    "Terms of use",
    "© 2020 King County, WA"
  ];

  return (
    <div className={classes.root}>
      <div className={classes.wrapper}>
        <div className={classes.column}>
          {defaultValues.slice(0, 3).map((item) => (
            <span className={classes.item}>{item}</span>
          ))}
        </div>
        <div className={classes.column}>
          {defaultValues.slice(3, 6).map((item) => (
            <span className={classes.item}>{item}</span>
          ))}
        </div>
      </div>
    </div>
  );
}

export default withStyles(useStyles)(Footer);
