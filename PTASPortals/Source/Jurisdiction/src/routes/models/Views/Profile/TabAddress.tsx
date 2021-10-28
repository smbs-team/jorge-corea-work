// TabAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useEffect } from 'react';
import {
  CustomTextField,
  CustomSearchTextField,
  ItemSuggestion,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useStyles } from './profileStyles';
import { useProfile } from './ProfileContext';
import clsx from 'clsx';
import { Contact, ContactAddress } from './Contact';
import { apiService } from 'services/api/apiService';
import { AddressSuggestion } from 'services/models/addressSuggestion';
import utilServiceLocal from 'services/utilService';
import { CHANGE_DELAY_MS } from './constants';

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabAddress(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { editingContact, setEditingContact } = useProfile();
  const [addressList, setAddressList] = useState<ItemSuggestion[]>([]);
  const [addressSuggestions, setAddressSuggestions] = useState<
    AddressSuggestion[]
  >([]);
  const {
    isValid: addressIsValid,
    hasError: addressInputHasError,
    valueChangedHandler: addressInputChangedHandler,
    inputBlurHandler: addressInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.address?.line1 ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: cityIsValid,
    hasError: cityInputHasError,
    valueChangedHandler: cityInputChangedHandler,
    inputBlurHandler: cityInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.address?.city ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: stateIsValid,
    hasError: stateInputHasError,
    valueChangedHandler: stateInputChangedHandler,
    inputBlurHandler: stateInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.address?.state ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: zipIsValid,
    hasError: zipInputHasError,
    valueChangedHandler: zipInputChangedHandler,
    inputBlurHandler: zipInputBlurHandler,
  } = useTextFieldValidation(
    editingContact?.address?.zip ?? '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    if (updateFormIsValid) {
      if (addressIsValid && cityIsValid && stateIsValid && zipIsValid) {
        updateFormIsValid(true);
      } else {
        updateFormIsValid(false);
      }
    }
  }, [
    updateFormIsValid,
    addressIsValid,
    cityIsValid,
    stateIsValid,
    zipIsValid,
  ]);

  const onClickSearch = async (): Promise<void> => {
    if (!editingContact?.address?.line1) return;
    const addressRes = await apiService.getAddressSuggestions(
      editingContact.address.line1
    );

    if (addressRes.data?.length) {
      setAddressSuggestions(addressRes.data);
      const list = addressRes.data.map((addressItem) => ({
        id: addressItem.id,
        title: addressItem.formattedaddr,
        subtitle: addressItem.city,
      }));
      setAddressList(list);
    }
  };

  const onAddressFieldChange = (
    value: string,
    field: keyof ContactAddress
  ): void => {
    setEditingContact((prev: Contact | undefined) => {
      if (!prev || !prev.address) return;
      return {
        ...prev,
        address: {
          ...prev.address,
          [field]: value,
        },
      };
    });
  };

  const onAddressSearchChange = async (value: string): Promise<void> => {
    const { data } = await apiService.getAddressSuggestions(value);
    setAddressSuggestions(data ?? []);

    const list = data?.map((addressItem) => ({
      id: addressItem.id,
      title: addressItem.formattedaddr,
      subtitle: addressItem.city,
    }));
    setAddressList(list ?? []);
  };

  const updateAddress = (
    line1: string,
    line2: string,
    city: string,
    state: string,
    zip: string
  ): void => {
    if (line1.length > 100) {
      line1 = line1.substr(0, 100);
    }

    setEditingContact((prev: Contact | undefined): Contact | undefined => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              line1: line1,
              line2: line2,
              city: city,
              state: utilServiceLocal.abbrState(state, 'abbr'),
              zip: zip,
            }
          : undefined,
      };
    });
  };

  return (
    <Fragment>
      <Box className={classes.tab}>
        <Box className={classes.line}>
          <CustomSearchTextField
            ptasVariant="squared outline"
            label={fm.addressLine1}
            value={editingContact?.address?.line1}
            classes={{
              wrapper: classes.wrapper,
            }}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              addressInputChangedHandler(e);
              onAddressSearchChange(e.target.value);
            }}
            onChangeDelay={CHANGE_DELAY_MS}
            onBlur={addressInputBlurHandler}
            onClick={onClickSearch}
            suggestion={{
              List: addressList,
              onSelected: (item: ItemSuggestion): void => {
                const suggestion = addressSuggestions.find(
                  (i) => i.id === item.id
                );
                updateAddress(
                  suggestion?.formattedaddr ?? '',
                  '',
                  suggestion?.city ?? '',
                  suggestion?.state ?? '',
                  suggestion?.zip ?? ''
                );
              },
            }}
            // autoFocus={autoFocus}
            error={addressInputHasError}
            helperText={
              addressInputHasError ? fm.requiredField ?? '*Required' : ''
            }
          />
        </Box>

        <Box className={classes.line}>
          <CustomTextField
            classes={{ root: classes.addressLine }}
            ptasVariant="outline"
            label={fm.addressLine2}
            value={editingContact?.address?.line2}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              onAddressFieldChange(e.target.value, 'line2');
            }}
          />
        </Box>
        <Box className={clsx(classes.line, classes.lastLine)}>
          <CustomTextField
            classes={{ root: classes.city }}
            ptasVariant="outline"
            label={fm.addressCity}
            value={editingContact?.address?.city}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              cityInputChangedHandler(e);
              onAddressFieldChange(e.target.value, 'city');
            }}
            onChangeDelay={CHANGE_DELAY_MS}
            onBlur={cityInputBlurHandler}
            error={cityInputHasError}
            helperText={cityInputHasError ? fm.requiredField : ''}
          />
          <CustomTextField
            classes={{
              root: classes.state,
              fullWidth: classes.statePadding,
              shrinkRoot: classes.shrink,
            }}
            ptasVariant="outline"
            label={fm.addressState}
            value={editingContact?.address?.state}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              stateInputChangedHandler(e);
              onAddressFieldChange(e.target.value, 'state');
            }}
            onChangeDelay={CHANGE_DELAY_MS}
            onBlur={stateInputBlurHandler}
            error={stateInputHasError}
            helperText={stateInputHasError ? fm.requiredField : ''}
          />
          <CustomTextField
            classes={{ root: classes.zip }}
            ptasVariant="outline"
            label={fm.addressZip}
            value={editingContact?.address?.zip}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              zipInputChangedHandler(e);
              onAddressFieldChange(e.target.value, 'zip');
            }}
            onChangeDelay={CHANGE_DELAY_MS}
            onBlur={zipInputBlurHandler}
            error={zipInputHasError}
            helperText={zipInputHasError ? fm.requiredField : ''}
          />
        </Box>
      </Box>
    </Fragment>
  );
}

export default TabAddress;
