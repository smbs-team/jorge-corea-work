// TabEmail.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect } from 'react';
import {
  CustomTextField,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useStyles } from './profileStyles';
import { useProfile } from './ProfileContext';
import { Contact } from './Contact';

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabEmail(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    editingContact,
    setEditingContact,
  } = useProfile();
  const {
    isValid: emailIsValid,
    hasError: emailInputHasError,
    valueChangedHandler: emailInputChangedHandler,
    inputBlurHandler: emailInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.email ?? '',
    utilService.isNotEmpty
  );

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
    setEditingContact((prev: Contact | undefined) => {
      if (!prev) return;
      return {
        ...prev,
        email: e.target.value,
      };
    });
  };

  return (
    <Fragment>
      <Box className={classes.tab}>
        <CustomTextField
          classes={{ root: classes.formInput }}
          ptasVariant="email"
          label={fm.email}
          value={editingContact?.email}
          error={emailInputHasError}
          helperText={emailInputHasError ? fm.requiredEmail : ''}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            emailInputChangedHandler(e);
            onEmailChange(e);
            // setEditingEmail(e.target.value);
          }}
          onBlur={emailInputBlurHandler}
        />
      </Box>
    </Fragment>
  );
}

export default TabEmail;
