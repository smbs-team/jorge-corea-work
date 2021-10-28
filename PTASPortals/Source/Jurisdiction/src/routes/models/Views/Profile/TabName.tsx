// TabName.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect } from 'react';
import {
  CustomTextField,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import clsx from 'clsx';
import { useStyles } from './profileStyles';
import { useProfile } from './ProfileContext';
import { Contact } from './Contact';
import { CHANGE_DELAY_MS } from './constants';

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabName(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { editingContact, setEditingContact } = useProfile();
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
  const {
    isValid: jobTitleIsValid,
    hasError: jobTitleInputHasError,
    valueChangedHandler: jobTitleInputChangedHandler,
    inputBlurHandler: jobTitleInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.jobTitle ?? '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    if (nameIsValid && lastNameIsValid && jobTitleIsValid) {
      updateFormIsValid(true);
    } else {
      updateFormIsValid(false);
    }
  }, [updateFormIsValid, nameIsValid, lastNameIsValid, jobTitleIsValid]);

  const onContactChange = (
    value: string | number,
    field: keyof Contact
  ): void => {
    setEditingContact((prev: Contact | undefined) => {
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
        label={fm.firstName}
        value={editingContact?.firstName ?? ''}
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

      <CustomTextField
        classes={{ root: classes.formInput }}
        ptasVariant="outline"
        error={jobTitleInputHasError}
        helperText={jobTitleInputHasError ? fm.requiredField : ''}
        label={fm.jobTitle}
        value={editingContact?.jobTitle}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          jobTitleInputChangedHandler(e);
          onContactChange(e.target.value, 'jobTitle');
        }}
        onBlur={jobTitleInputBlurHandler}
      />

      {/* For testing scroll behavior */}
      {/* <p>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod
        tempor incididunt ut labore et dolore magna aliqua. Massa ultricies mi
        quis hendrerit dolor magna eget est. Mattis aliquam faucibus purus in
        massa tempor nec. Maecenas pharetra convallis posuere morbi leo urna.
        Tellus integer feugiat scelerisque varius morbi enim. Imperdiet sed
        euismod nisi porta lorem mollis aliquam. Leo vel orci porta non pulvinar
        neque laoreet suspendisse. Bibendum est ultricies integer quis auctor
        elit sed vulputate mi. Est velit egestas dui id ornare arcu odio ut sem.
        Amet massa vitae tortor condimentum lacinia quis vel.
      </p>

      <p>
        In egestas erat imperdiet sed euismod nisi. Varius morbi enim nunc
        faucibus a. Pharetra et ultrices neque ornare aenean euismod. Pulvinar
        elementum integer enim neque volutpat ac. Sodales neque sodales ut etiam
        sit. Tristique sollicitudin nibh sit amet. Lorem ipsum dolor sit amet
        consectetur adipiscing elit. Dictumst quisque sagittis purus sit amet
        volutpat. Ut tristique et egestas quis ipsum. Feugiat vivamus at augue
        eget. Viverra adipiscing at in tellus integer.
      </p>

      <p>
        Mauris nunc congue nisi vitae suscipit tellus mauris. Mattis rhoncus
        urna neque viverra. Ipsum suspendisse ultrices gravida dictum fusce ut
        placerat. Vulputate odio ut enim blandit volutpat. Tempor orci dapibus
        ultrices in. Ullamcorper malesuada proin libero nunc consequat interdum
        varius sit. Tellus in metus vulputate eu scelerisque. Viverra ipsum nunc
        aliquet bibendum enim facilisis. Suspendisse faucibus interdum posuere
        lorem ipsum dolor sit amet consectetur. Quis eleifend quam adipiscing
        vitae proin. Massa eget egestas purus viverra accumsan in. Ultrices
        tincidunt arcu non sodales neque sodales ut etiam. Orci sagittis eu
        volutpat odio facilisis. Sed risus pretium quam vulputate dignissim
        suspendisse. Augue lacus viverra vitae congue. Dolor sed viverra ipsum
        nunc aliquet. Duis convallis convallis tellus id interdum velit laoreet
        id. Dui nunc mattis enim ut. Aenean vel elit scelerisque mauris
        pellentesque.
      </p> */}
    </Box>
  );
}

export default TabName;
