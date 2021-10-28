// TabPhone.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect } from 'react';
import {
  CustomPhoneTextField,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useStyles } from './profileStyles';
import { useProfile } from './ProfileContext';
import { Contact } from './Contact';
import { CHANGE_DELAY_MS } from './constants';

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabPhone(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { editingContact, setEditingContact } = useProfile();
  const {
    isValid: phoneNumberIsValid,
    hasError: phoneNumberInputHasError,
    valueChangedHandler: phoneNumberInputChangedHandler,
    inputBlurHandler: phoneNumberInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.phoneNumber ?? '',
    utilService.validPhone
  );

  useEffect(() => {
    if (updateFormIsValid) {
      if (phoneNumberIsValid) {
        updateFormIsValid(true);
      } else {
        updateFormIsValid(false);
      }
    }
  }, [updateFormIsValid, phoneNumberIsValid]);

  const onPhoneNumberChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    setEditingContact((prev: Contact | undefined) => {
      if (!prev) return;
      return {
        ...prev,
        phoneNumber: e.target.value,
      };
    });
  };

  return (
    <Fragment>
      <Box className={classes.tab}>
        <CustomPhoneTextField
          classes={{ root: classes.formInput }}
          label={fm.phoneNumber}
          value={editingContact?.phoneNumber}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            phoneNumberInputChangedHandler(e);
            onPhoneNumberChange(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlurHandler={phoneNumberInputBlurHandler}
          error={phoneNumberInputHasError}
          helperText={phoneNumberInputHasError ? fm.requiredPhone : ''}
        />
      </Box>
    </Fragment>
  );
}

export default TabPhone;
