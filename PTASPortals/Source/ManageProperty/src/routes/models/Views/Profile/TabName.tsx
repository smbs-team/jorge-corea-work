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
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import clsx from 'clsx';
import { useStyles } from './profileStyles';
import { useProfile } from './ProfileContext';
import { PortalContact } from 'models/portalContact';
import { CHANGE_DELAY_MS } from './constants';
import { ProfileStep } from './types';

interface Props {
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabName(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    editingContact,
    setEditingContact,
    contactSuffixItems,
  } = useProfile();
  const {
    isValid: nameIsValid,
    hasError: nameInputHasError,
    valueChangedHandler: nameInputChangedHandler,
    inputBlurHandler: nameInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.firstName ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: lastNameIsValid,
    hasError: lastNameInputHasError,
    valueChangedHandler: lastNameInputChangedHandler,
    inputBlurHandler: lastNameInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.lastName ?? '',
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
    setEditingContact((prev: PortalContact | undefined) => {
      if (!prev) return;
      return {
        ...prev,
        [field]: value,
      };
    });
  };

  return (
    <Box className={clsx(classes.tab, classes.tabName)}>
      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        error={nameInputHasError}
        helperText={nameInputHasError ? fm.requiredField : ''}
        label={fm.firstName}
        value={editingContact?.firstName}
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
        value={editingContact?.middleName}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          onContactChange(e.target.value, 'middleName');
        }}
      />
      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        error={lastNameInputHasError}
        helperText={lastNameInputHasError ? fm.requiredField : ''}
        label={fm.lastName}
        value={editingContact?.lastName}
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
        items={contactSuffixItems}
        label={fm.suffix}
        classes={{
          textFieldRoot: classes.suffixDropdown,
          // animated: classes.dropdownLabel,
        }}
        value={editingContact?.suffix}
        onSelected={(item: DropDownItem): void => {
          onContactChange(item.value, 'suffix');
        }}
      />
    </Box>
  );
}

export default TabName;
