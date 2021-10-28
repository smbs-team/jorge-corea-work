// EmailInfoFields.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, ReactNode, useEffect } from 'react';
import { createStyles, WithStyles, withStyles, Box } from '@material-ui/core';
import { Theme, StyleRules } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  utilService,
  CustomTextField,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import { PortalEmail } from 'models/portalContact';

export interface EmailInfoTextProps {
  label: string | ReactNode;
  validEmailText?: string | React.ReactNode;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      width: '100%',
      maxWidth: '270px',
      // marginTop: theme.spacing(3),
      // marginBottom: theme.spacing(2.125),
    },
    emailInput: {
      maxWidth: '270px',
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(3.125),
    },
  });

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, EmailInfoTextProps {
  email?: PortalEmail;
  onEmailChange?: (email: string) => void;
  changeDelay?: number;
  updateFormIsValid?: (isValid: boolean) => void;
}

/**
 * EmailInfoFields
 *
 * @param props - Component props
 * @returns A JSX element
 */
function EmailInfoFields(props: Props): JSX.Element {
  const { classes, email, updateFormIsValid } = props;
  const {
    isValid: emailIsValid,
    hasError: emailInputHasError,
    valueChangedHandler: emailInputChangedHandler,
    inputBlurHandler: emailInputBlurHandler,
  } = useTextFieldValidation(email?.email ?? '', utilService.validEmail);

  useEffect(() => {
    if (emailIsValid) {
      updateFormIsValid?.(true);
    } else {
      updateFormIsValid?.(false);
    }
  }, [updateFormIsValid, emailIsValid]);

  const onEmailChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onEmailChange?.(e.target.value);
  };

  return (
    <Box className={classes.root}>
      <CustomTextField
        classes={{ root: classes.emailInput }}
        ptasVariant="email"
        type="email"
        label={props.label ?? 'Email'}
        value={email?.email ?? ''}
        onChange={(
          event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          console.log('Email changed:', event)
          emailInputChangedHandler(event);
          onEmailChange(event);
        }}
        onChangeDelay={props.changeDelay ?? 500}
        onBlur={emailInputBlurHandler}
        error={emailInputHasError}
        helperText={
          emailInputHasError
            ? props.validEmailText ?? 'A valid email is required'
            : ''
        }
      />
    </Box>
  );
}

export default withStyles(styles)(EmailInfoFields) as FC<
  GenericWithStyles<Props>
>;
