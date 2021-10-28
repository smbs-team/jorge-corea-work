// EmailEntity.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useState } from "react";
import { createStyles, WithStyles, withStyles, Box } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomTextField } from "../CustomTextField";
import { CustomTextButton } from "../CustomTextButton";
import { Alert } from "../Alert";
import { CustomPopover } from "../CustomPopover";

export interface EmailEntityTextProps {
  label?: string | React.ReactNode;
  removeButtonText?: string | React.ReactNode;
  removeAlertText?: string | React.ReactNode;
  removeAlertButtonText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, EmailEntityTextProps {
  email: string;
  onRemove: (email: string) => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
      maxWidth: "294px"
    },
    emailInput: {
      maxWidth: "294px",
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(3.125)
    },
    removeLine: {
      display: "flex",
      justifyContent: "flex-end"
    },
    textButton: {
      color: theme.ptas.colors.utility.danger,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "19px"
    },
    popoverRoot: {
      width: 304,
      padding: "16px 32px 23px 32px ",
      boxSizing: "border-box"
    },
    alertText: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      color: theme.ptas.colors.theme.white
    },
    button: {
      width: 128,
      height: 38,
      marginLeft: "auto",
      marginRight: "auto",
      display: "block",
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight
    }
  });

/**
 * EmailEntity
 *
 * @param props - Component props
 * @returns A JSX element
 */
function EmailEntity(props: Props): JSX.Element {
  const { classes, email, onRemove } = props;
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  return (
    <Box className={classes.root}>
      <CustomTextField
        classes={{ root: classes.emailInput }}
        ptasVariant='email'
        type='email'
        label={props.label ?? "Email"}
        value={email}
      />
      <Box className={classes.removeLine}>
        <CustomTextButton
          classes={{ root: classes.textButton }}
          ptasVariant='Text more'
          onClick={(evt): void => setPopoverAnchor(evt.currentTarget)}
        >
          {props.removeButtonText ?? "Remove email"}
        </CustomTextButton>
      </Box>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
        }}
        ptasVariant='danger'
        showCloseButton
        tail
        tailPosition='end'
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "right"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "right"
        }}
      >
        <Alert
          contentText={props.removeAlertText ?? "Remove email"}
          ptasVariant='danger'
          okButtonText={props.removeAlertButtonText ?? "Remove"}
          okShowButton
          classes={{
            root: classes.popoverRoot,
            text: classes.alertText,
            buttons: classes.button
          }}
          okButtonClick={(): void => {
            setPopoverAnchor(null);
            onRemove(email);
          }}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(EmailEntity) as FC<GenericWithStyles<Props>>;
