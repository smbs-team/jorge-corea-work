// AlertContent.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from "react";
import { createStyles, WithStyles, withStyles, Box } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import Typography from "@material-ui/core/Typography";
import { CustomButton } from "../CustomButton";
import { GenericWithStyles } from "@ptas/react-ui-library";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  okButtonText?: string | React.ReactNode;
  okButtonClick: () => void;
  cancelClick?: () => void;
  contentText?: string | React.ReactNode;
  ptasVariant?: "danger" | "info" | "success";
  okShowButton?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      flexGrow: 1,
      width: 308,
      padding: theme.spacing(4),
      backgroundColor: setColorAlert(props.ptasVariant, theme),
      color:
        props.ptasVariant === "info" || props.ptasVariant === undefined
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white,
      borderRadius: 9,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "24px",
      fontSize: theme.ptas.typography.bodyLarge.fontSize
    }),
    buttonContainer: {},
    buttons: (props: Props) => ({
      background: `linear-gradient(240deg, ${theme.ptas.colors.theme.white}, ${theme.ptas.colors.theme.white}) ${theme.ptas.colors.theme.white}`,
      color:
        props.ptasVariant === "danger"
          ? theme.ptas.colors.utility.danger
          : theme.ptas.colors.theme.accentDark,
      width: "100%",
      height: 48,
      marginTop: theme.spacing(4)
    }),
    text: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily
    }
  });

const setColorAlert = (
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
 * Alert
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Alert(props: Props): JSX.Element {
  const {
    okButtonClick,
    okButtonText,
    contentText,
    classes,
    okShowButton
  } = props;

  const message = `Sample text`;

  return (
    <div className={`${classes.root} ${props.classes?.root}`}>
      {!contentText ||
        (typeof contentText === "string" && (
          <Typography
            variant='body1'
            className={`${classes.text} ${props.classes?.text}`}
          >
            {contentText ?? message}
          </Typography>
        ))}
      {contentText && typeof contentText === "object" && (
        <Fragment>{contentText}</Fragment>
      )}
      {okShowButton && (
        <Box className={classes.buttonContainer}>
          <CustomButton
            onClick={okButtonClick}
            classes={{ root: classes.buttons }}
          >
            {okButtonText ?? "Proceed"}
          </CustomButton>
        </Box>
      )}
    </div>
  );
}

export default withStyles(useStyles)(Alert) as FC<GenericWithStyles<Props>>;
