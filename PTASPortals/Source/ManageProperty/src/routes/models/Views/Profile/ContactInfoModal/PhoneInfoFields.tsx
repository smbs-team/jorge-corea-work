// PhoneInfoFields.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, ReactNode, useEffect } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  useTheme,
} from '@material-ui/core';
import { Theme, StyleRules } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomPhoneTextField,
  CustomTabs,
  CustomSwitch,
  formatMessageStructure,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import { PortalPhone } from 'models/portalContact';
import { CHANGE_DELAY_MS } from '../constants';
import { ProfileStep } from '../types';

export type PhoneType = 'Cell' | 'Work' | 'Home' | 'Toll-free';

export interface PhoneInfoTextProps {
  titleText?: string | ReactNode;
  placeholderText?: string | formatMessageStructure;
  tabCellText?: string | ReactNode;
  tabWorkText?: string | ReactNode;
  tabHomeText?: string | ReactNode;
  tabTollFreeText?: string | ReactNode;
  acceptMessagesText?: string | ReactNode;
  validPhoneText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, PhoneInfoTextProps {
  phone: PortalPhone;
  onPhoneNumberChange?: (phoneNumber: string) => void;
  onPhoneTypeSelect?: (phoneType: number) => void;
  onAcceptMessagesChange?: (checked: boolean) => void;
  phoneTypeList: { value: number; label: string }[];
  updateFormIsValid?: (step: ProfileStep, valid: boolean) => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      width: '100%',
      maxWidth: '270px',
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125),
    },
    line: {
      marginBottom: theme.spacing(3),
    },
    phoneNumber: {
      maxWidth: '196px',
    },
    phoneTypeTabs: {
      maxWidth: '267px',
    },
    tab: {
      padding: '0 11px',
    },
    textMessagesSwitch: {},
  });

/**
 * PhoneInfoFields
 *
 * @param props - Component props
 * @returns A JSX element
 */
function PhoneInfoFields(props: Props): JSX.Element {
  const { classes, phone, phoneTypeList, updateFormIsValid } = props;
  const theme = useTheme();
  const {
    isValid: phoneNumberIsValid,
    hasError: phoneNumberInputHasError,
    valueChangedHandler: phoneNumberInputChangedHandler,
    inputBlurHandler: phoneNumberInputBlurHandler,
  } = useTextFieldValidation(
    phone?.phoneNumber ?? '',
    (value: string) => value.length >= 12
  );

  useEffect(() => {
    //prop updateFormIsValid is not required for validations on modal
    // component used to edit existing phones, addresses and phones
    if (updateFormIsValid) {
      if (phoneNumberIsValid) {
        updateFormIsValid('phone', true);
      } else {
        updateFormIsValid('phone', false);
      }
    }
  }, [updateFormIsValid, phoneNumberIsValid]);

  const onPhoneNumberChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    const value = e.target.value ? e.target.value.trim() : '';
    props.onPhoneNumberChange && props.onPhoneNumberChange(value);
  };

  return (
    <Box className={classes.root}>
      <Box className={classes.line}>
        <CustomPhoneTextField
          classes={{ root: classes.phoneNumber }}
          label={props.titleText ?? 'Phone'}
          placeholder={props.placeholderText ?? 'Phone'}
          value={phone.phoneNumber}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            onPhoneNumberChange(e);
            phoneNumberInputChangedHandler(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={phoneNumberInputBlurHandler}
          error={phoneNumberInputHasError}
          helperText={
            phoneNumberInputHasError
              ? props.validPhoneText ?? 'A valid phone number is required'
              : ''
          }
        />
      </Box>
      <Box className={classes.line}>
        <CustomTabs
          classes={{ root: classes.phoneTypeTabs, item: classes.tab }}
          selectedIndex={
            phoneTypeList.find((el) => el.value === phone.phoneTypeValue)
              ?.label === 'Cell'
              ? 0
              : phoneTypeList.find((el) => el.value === phone.phoneTypeValue)
                  ?.label === 'Work'
              ? 1
              : phoneTypeList.find((el) => el.value === phone.phoneTypeValue)
                  ?.label === 'Home'
              ? 2
              : phoneTypeList.find((el) => el.value === phone.phoneTypeValue)
                  ?.label === 'Toll-free'
              ? 3
              : 0
          }
          ptasVariant="SwitchMedium"
          items={[
            { label: props.tabCellText ?? 'Cell', disabled: false },
            {
              label: props.tabWorkText ?? 'Work',
              disabled: false,
            },
            {
              label: props.tabHomeText ?? 'Home',
              disabled: false,
            },
            {
              label: props.tabTollFreeText ?? 'Toll-free',
              disabled: false,
            },
          ]}
          onSelected={(tab: number): void => {
            const phoneTypeLabel: PhoneType =
              tab === 0
                ? 'Cell'
                : tab === 1
                ? 'Work'
                : tab === 2
                ? 'Home'
                : tab === 3
                ? 'Toll-free'
                : 'Cell';
            const newPhoneType =
              phoneTypeList.find((el) => el.label === phoneTypeLabel)?.value ??
              0;
            props.onPhoneTypeSelect && props.onPhoneTypeSelect(newPhoneType);
          }}
          indicatorBackgroundColor={theme.ptas.colors.theme.white}
          itemTextColor={theme.ptas.colors.theme.white}
          selectedItemTextColor={theme.ptas.colors.theme.black}
          tabsBackgroundColor={theme.ptas.colors.theme.gray}
        />
      </Box>
      <Box className={classes.line}>
        <CustomSwitch
          classes={{ root: classes.textMessagesSwitch }}
          label={props.acceptMessagesText ?? 'Accepts text messages'}
          ptasVariant="normal"
          showOptions
          value={phone.acceptsTextMessages}
          isChecked={(checked: boolean): void =>
            props.onAcceptMessagesChange?.(checked)
          }
        />
      </Box>
    </Box>
  );
}

export default withStyles(styles)(PhoneInfoFields) as FC<
  GenericWithStyles<Props>
>;
