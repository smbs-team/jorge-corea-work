// AlertContent.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import Typography from "@material-ui/core/Typography";
import { CustomButton } from "./../../CustomButton";

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  okButtonText?: string;
  okButtonClick: () => void;
  cancelClick?: () => void;
  contentText?: string;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
      width: 308,
      padding: theme.spacing(4),
      backgroundColor: theme.ptas.colors.utility.danger,
      color: theme.ptas.colors.theme.white,
      borderRadius: "inherit"
    },
    buttons: {
      background: `linear-gradient(240deg, ${theme.ptas.colors.theme.white}, ${theme.ptas.colors.theme.white}) ${theme.ptas.colors.theme.white}`,
      color: theme.ptas.colors.utility.danger,
      width: "100%",
      height: 48,
      marginTop: theme.spacing(4)
    },
    text: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    closeButton: {
      position: "absolute",
      top: theme.spacing(1 / 8),
      right: theme.spacing(1 / 8),
      cursor: "pointer",
      color: theme.ptas.colors.theme.white
    },
    closeIcon: {
      width: 30,
      height: 30
    }
  });

/**
 * Alert
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Alert(props: Props): JSX.Element {
  const { classes, okButtonClick, okButtonText, contentText } = props;

  const message = `Sample text`;

  return (
    <div className={classes.root}>
      <Typography variant={"body1"} className={classes.text}>
        {contentText ?? message}
      </Typography>
      <CustomButton
        onClick={okButtonClick}
        fullyRounded
        classes={{ root: classes.buttons }}
      >
        {okButtonText ?? "Proceed"}
      </CustomButton>
    </div>
  );
}

export default withStyles(styles)(Alert);
