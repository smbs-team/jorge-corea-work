// EmailEditor.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useState } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  Alert,
  CustomTextButton,
  CustomPopover,
} from '@ptas/react-public-ui-library';
import { PortalEmail } from 'models/portalContact';
import EmailInfoFields from './EmailInfoFields';

export interface EmailEditorTextProps {
  label?: string | React.ReactNode;
  removeButtonText?: string | React.ReactNode;
  removeAlertText?: string | React.ReactNode;
  removeAlertButtonText?: string | React.ReactNode;
  validEmailText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, EmailEditorTextProps {
  email: PortalEmail;
  onRemove: (emailId: string) => void;
  disableRemove: boolean;
  onChange: (email: PortalEmail) => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      width: '100%',
      maxWidth: '294px',
    },
    emailInput: {
      maxWidth: '294px',
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(3.125),
    },
    removeLine: {
      display: 'flex',
      justifyContent: 'flex-end',
    },
    textButton: {
      color: theme.ptas.colors.utility.danger,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    popoverRoot: {
      width: 304,
      padding: '16px 32px 23px 32px ',
      boxSizing: 'border-box',
    },
    alertText: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      color: theme.ptas.colors.theme.white,
    },
    button: {
      width: 128,
      height: 38,
      marginLeft: 'auto',
      marginRight: 'auto',
      display: 'block',
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    },
  });

/**
 * EmailEditor
 *
 * @param props - Component props
 * @returns A JSX element
 */
function EmailEditor(props: Props): JSX.Element {
  const { classes, email, onRemove, disableRemove, onChange } = props;
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  return (
    <Box className={classes.root}>
      {/* <CustomTextField
        classes={{ root: classes.emailInput }}
        ptasVariant="email"
        type="email"
        label={props.label ?? 'Email'}
        value={email.email}
        onChange={onEmailChange}
        error={emailInputError}
        helperText={
          emailInputError
            ? props.validEmailText ?? 'A valid email is required'
            : ''
        }
      /> */}
      <EmailInfoFields
        email={email}
        onEmailChange={(newEmail: string): void => {
          onChange && onChange({ ...email, email: newEmail });
        }}
        label={props.label}
        validEmailText={props.validEmailText}
      />
      <Box className={classes.removeLine}>
        <CustomTextButton
          disabled={disableRemove}
          classes={{ root: classes.textButton }}
          ptasVariant="Text more"
          onClick={(evt: React.MouseEvent<HTMLButtonElement>): void =>
            setPopoverAnchor(evt.currentTarget)
          }
        >
          {props.removeButtonText ?? 'Remove email'}
        </CustomTextButton>
      </Box>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
        }}
        ptasVariant="danger"
        showCloseButton
        tail
        tailPosition="end"
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
      >
        <Alert
          contentText={props.removeAlertText ?? 'Remove email'}
          ptasVariant="danger"
          okButtonText={props.removeAlertButtonText ?? 'Remove'}
          okShowButton
          classes={{
            root: classes.popoverRoot,
            text: classes.alertText,
            buttons: classes.button,
          }}
          okButtonClick={(): void => {
            setPopoverAnchor(null);
            onRemove(email.id);
          }}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(EmailEditor) as FC<GenericWithStyles<Props>>;
