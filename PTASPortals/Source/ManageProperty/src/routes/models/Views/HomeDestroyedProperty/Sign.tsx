// Sign.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { makeStyles, Theme } from '@material-ui/core';
import * as fmHomeImprovement from '../HomeImprovement/formatMessage';
import * as fmTask from './formatText';
import {
  CustomTextField,
  CustomPhoneTextField,
} from '@ptas/react-public-ui-library';
import { Person as PersonIcon } from '@material-ui/icons';
import utilService from 'services/utilService';
import * as fm from '../HomeImprovement/formatMessage';
import useDestroyedProperty from './useDestroyedProperty';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

const useStyles = makeStyles((theme: Theme) => ({
  sign: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    width: '100%',
    maxWidth: 368,
    marginTop: theme.spacing(2),
  },
  signEmailAnimated: {
    top: -10,
    left: 10,
  },
  signEmailShrink: {
    top: -8,
    left: 4,
  },
  descWrapper: {
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    lineHeight: '15px',
    marginBottom: 10,
    color: theme.ptas.colors.theme.black,
  },
  signSignatureInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(3.125),

    '& .MuiOutlinedInput-root': {
      background: theme.ptas.colors.theme.white,
    },
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
  signPhoneInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(4.125),
  },
  signatureInput: {
    paddingTop: 11,
    paddingLeft: 10,
  },
  personIcon: {
    marginRight: 14,
  },
  description: {
    fontSize: theme.ptas.typography.finePrint.fontSize,
    marginBottom: 16,
    display: 'block',
  },
}));

function Sign(props: Props): JSX.Element {
  const classes = useStyles(props);
  const {
    task,
    saveTask,
    setEmailInputError,
    emailInputError,
    setPhoneInputError,
    phoneInputError,
  } = useDestroyedProperty();

  const handleChangeEmail = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const email = e.target.value;

    saveTask('email', email);

    if (!utilService.validEmail(email)) {
      return setEmailInputError(true);
    }

    setEmailInputError(false);
  };

  const handleChangePhone = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const phoneNumber = e.target.value;

    saveTask('phoneNumber', phoneNumber);

    if (!utilService.validPhone(phoneNumber)) {
      return setPhoneInputError(true);
    }

    setPhoneInputError(false);
  };

  return (
    <div className={classes.sign}>
      <div className={classes.descWrapper}>
        <span className={classes.description}>
          {fmHomeImprovement.signDeclaration1}
        </span>
        <span className={classes.description}>
          {fmHomeImprovement.signDeclaration2}
        </span>
      </div>
      <CustomTextField
        classes={{ root: classes.signSignatureInput }}
        ptasVariant="overlay"
        label={fmTask.yourSignature}
        placeholder={fmTask.toSignEnterFullName}
        onChangeDelay={500}
        // onChange={handleChangeName}
        // helperText={taxPayerInputError ? fm.fieldRequired : ''}
        InputProps={{
          endAdornment: <PersonIcon className={classes.personIcon} />,
          classes: {
            input: classes.signatureInput,
          },
        }}
        // value={task?.taxPayerName}
      />
      <CustomTextField
        classes={{
          root: classes.signEmailInput,
          animated: classes.signEmailAnimated,
          shrinkRoot: classes.signEmailShrink,
        }}
        error={emailInputError}
        helperText={emailInputError ? fm.requiredEmail : ''}
        ptasVariant="email"
        label={fmHomeImprovement.signEmail}
        variant="standard"
        onChangeDelay={500}
        onChange={handleChangeEmail}
        value={task?.email}
        placeholder="user@domain.com"
      />
      <CustomPhoneTextField
        classes={{ root: classes.signPhoneInput }}
        label={fmHomeImprovement.signPhone}
        variant="standard"
        onChangeDelay={500}
        error={phoneInputError}
        helperText={phoneInputError ? fm.requiredPhone : ''}
        onChange={handleChangePhone}
        value={task?.phoneNumber}
        placeholder="000-000-0000"
      />
    </div>
  );
}

export default Sign;
