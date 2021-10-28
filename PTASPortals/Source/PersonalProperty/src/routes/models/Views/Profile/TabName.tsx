// TabName.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect } from 'react';
import {
  CustomTextField,
  SimpleDropDown,
  DropDownItem,
  utilService,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { PortalContact } from 'models/portalContact';
import { CHANGE_DELAY_MS } from './constants';
import { ProfileStep } from './types';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 270,
    padding: theme.spacing(4, 0, 4, 0),
  },
  formInput: {
    maxWidth: '100%',
    marginBottom: theme.spacing(3.125),
  },
  formDropDown: {
    minWidth: 130,
    maxWidth: 130,
  },
}));

interface Props {
  profileType: 'primary' | 'secondary';
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabName(props: Props): JSX.Element {
  const { profileType, updateFormIsValid } = props;
  const classes = useStyles();
  const {
    editingContact,
    setEditingContact,
    editingContactSec,
    setEditingContactSec,
    contactSuffixItems,
  } = useProfile();
  const {
    isValid: nameIsValid,
    hasError: nameInputHasError,
    valueChangedHandler: nameInputChangedHandler,
    inputBlurHandler: nameInputBlurHandler,
  } = useTextFieldValidation(
    (profileType === 'primary'
      ? editingContact?.firstName
      : editingContactSec?.firstName) ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: lastNameIsValid,
    hasError: lastNameInputHasError,
    valueChangedHandler: lastNameInputChangedHandler,
    inputBlurHandler: lastNameInputBlurHandler,
  } = useTextFieldValidation(
    (profileType === 'primary'
      ? editingContact?.lastName
      : editingContactSec?.lastName) ?? '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    if (nameIsValid && lastNameIsValid) {
      updateFormIsValid('name', true);
    } else {
      updateFormIsValid('name', false);
    }
  }, [updateFormIsValid, nameIsValid, lastNameIsValid]);

  const onContactChange = (
    value: string | number,
    field: keyof PortalContact
  ): void => {
    if (profileType === 'primary') {
      setEditingContact((prev: PortalContact | undefined) => {
        if (!prev) return;
        return {
          ...prev,
          [field]: value,
        };
      });
    } else {
      setEditingContactSec((prev: PortalContact | undefined) => {
        if (!prev) return;
        return {
          ...prev,
          [field]: value,
        };
      });
    }
  };

  return (
    <Box className={classes.root}>
      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        label={fm.firstName}
        value={
          profileType === 'primary'
            ? editingContact?.firstName ?? ''
            : editingContactSec?.firstName ?? ''
        }
        error={nameInputHasError}
        helperText={nameInputHasError ? fm.requiredField : ''}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          nameInputChangedHandler(e);
          onContactChange(e.target.value, 'firstName');
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={nameInputBlurHandler}
      />
      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        label={fm.middleName}
        value={
          profileType === 'primary'
            ? editingContact?.middleName ?? ''
            : editingContactSec?.middleName ?? ''
        }
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          onContactChange(e.target.value, 'middleName');
        }}
        onChangeDelay={CHANGE_DELAY_MS}
      />
      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        label={fm.lastName}
        value={
          profileType === 'primary'
            ? editingContact?.lastName ?? ''
            : editingContactSec?.lastName ?? ''
        }
        error={lastNameInputHasError}
        helperText={lastNameInputHasError ? fm.requiredField : ''}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          lastNameInputChangedHandler(e);
          onContactChange(e.target.value, 'lastName');
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={lastNameInputBlurHandler}
      />
      <SimpleDropDown
        classes={{ textFieldRoot: classes.formDropDown }}
        items={contactSuffixItems}
        label="Suffix"
        value={
          profileType === 'primary'
            ? editingContact?.suffix
            : editingContactSec?.suffix ?? ''
        }
        onSelected={(item: DropDownItem): void => {
          onContactChange(item.value, 'suffix');
        }}
      ></SimpleDropDown>
    </Box>
  );
}

export default TabName;
