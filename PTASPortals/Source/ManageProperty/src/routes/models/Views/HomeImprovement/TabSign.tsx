// TabSign.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Box } from '@material-ui/core';
import { Theme, makeStyles } from '@material-ui/core/styles';
import React, { useEffect } from 'react';
import {
  CustomPhoneTextField,
  CustomTextField,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import * as fm from './formatMessage';
import { Person as PersonIcon } from '@material-ui/icons';
import { useHI } from './HomeImprovementContext';
import { CHANGE_DELAY_MS } from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  sign: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    maxWidth: 374,
    width: '100%',
    marginTop: theme.spacing(2),
  },
  signDeclaration: {
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    lineHeight: '15px',
    marginBottom: theme.spacing(3),
    color: theme.ptas.colors.theme.black,
  },
  signSignatureInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(3.125),
  },
  signEmailInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(3.125),
    '& .MuiInputBase-root': {
      marginTop: 0,
    },
    '& .MuiInput-underline:before': {
      borderBottom: 'none',
    },
    '& .MuiSvgIcon-root': {
      marginRight: 8,
    },
    '& .MuiInput-underline.Mui-focused:before': {
      borderBottom: 'none !important',
    },
  },
  signEmailAnimated: {
    top: -10,
    left: 10,
  },
  signEmailShrink: {
    top: -8,
    left: 4,
  },
  signPhoneInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(4.125),
  },
}));

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabSign(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { currentHiApplication, updateCurrentHiApplication } = useHI();
  const {
    isValid: signIsValid,
    hasError: signInputHasError,
    valueChangedHandler: signInputChangedHandler,
    inputBlurHandler: signInputBlurHandler,
  } = useTextFieldValidation(
    currentHiApplication?.taxpayerName ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: emailIsValid,
    hasError: emailInputHasError,
    valueChangedHandler: emailInputChangedHandler,
    inputBlurHandler: emailInputBlurHandler,
  } = useTextFieldValidation(
    currentHiApplication?.emailAddress ?? '',
    utilService.validEmail
  );
  const {
    isValid: phoneNumberIsValid,
    hasError: phoneNumberInputHasError,
    valueChangedHandler: phoneNumberInputChangedHandler,
    inputBlurHandler: phoneNumberInputBlurHandler,
  } = useTextFieldValidation(
    currentHiApplication?.phoneNumber ?? '',
    (value: string) => value.length >= 12
  );

  useEffect(() => {
    if (signIsValid && emailIsValid && phoneNumberIsValid) {
      updateFormIsValid(true);
    } else {
      updateFormIsValid(false);
    }
  }, [updateFormIsValid, signIsValid, emailIsValid, phoneNumberIsValid]);

  const updateSignature = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    updateCurrentHiApplication('taxpayerName', e.target.value);
  };

  const updateEmailAddress = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    updateCurrentHiApplication('emailAddress', e.target.value);
  };

  const updatePhoneNumber = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    updateCurrentHiApplication('phoneNumber', e.target.value);
  };

  return (
    <Box className={classes.sign}>
      <Box className={classes.signDeclaration}>
        <Box>{fm.signDeclaration1}</Box>
        <br />
        <Box>{fm.signDeclaration2}</Box>
      </Box>
      <CustomTextField
        classes={{ root: classes.signSignatureInput }}
        ptasVariant="overlay"
        label={fm.signSignature}
        InputProps={{ endAdornment: <PersonIcon /> }}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          signInputChangedHandler(e);
          updateSignature(e);
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={signInputBlurHandler}
        error={signInputHasError}
        helperText={signInputHasError ? fm.fieldRequired : ''}
      />
      <CustomTextField
        classes={{
          root: classes.signEmailInput,
          animated: classes.signEmailAnimated,
          shrinkRoot: classes.signEmailShrink,
        }}
        ptasVariant="email"
        variant="standard"
        label={fm.signEmail}
        value={currentHiApplication?.emailAddress ?? ''}
        error={emailInputHasError}
        helperText={emailInputHasError ? fm.requiredEmail : ''}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          emailInputChangedHandler(e);
          updateEmailAddress(e);
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={emailInputBlurHandler}
      />
      <CustomPhoneTextField
        classes={{ root: classes.signPhoneInput }}
        label={fm.signPhone}
        variant="standard"
        value={currentHiApplication?.phoneNumber}
        error={phoneNumberInputHasError}
        helperText={phoneNumberInputHasError ? fm.requiredPhone : ''}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          phoneNumberInputChangedHandler(e);
          updatePhoneNumber(e);
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={phoneNumberInputBlurHandler}
      />
    </Box>
  );
}

export default TabSign;
