// AddressInfoFields.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useEffect } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomTextField,
  DropDownItem,
  SimpleDropDown,
  CustomSearchTextField,
  ItemSuggestion,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';
import { PortalAddress } from 'models/portalContact';
import { AddressLookup } from 'services/map/model/addresses';
import { CHANGE_DELAY_MS } from '../constants';
import { ProfileStep } from '../types';

/**
 * Component styles
 */
// const styles = (theme: Theme): StyleRules<string, Props> =>
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      width: '100%',
      maxWidth: '270px',
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125),
    },
    line: {
      display: 'flex',
      justifyContent: 'space-between',
      marginBottom: theme.spacing(3.125),
      flexDirection: 'column',
      '& > div': {
        marginBottom: 10,
      },

      '& > div:last-child': {
        marginBottom: 0,
      },

      [theme.breakpoints.up('sm')]: {
        flexDirection: 'row',
        '& > div': {
          marginBottom: 0,
        },
      },
    },
    lastLine: {
      marginBottom: theme.spacing(2.125),
    },
    addressTitle: {
      maxWidth: '150px',
    },
    countryDropdown: {
      maxWidth: '110px',
      minWidth: '110px',
    },
    countryDropdownInput: {
      height: '36px',
    },
    addressLine: {
      maxWidth: '270px',
      width: '270px',
    },
    city: {
      width: '107px',
    },
    state: {
      width: '45px',
    },
    statePadding: {
      padding: 0,

      '& > fieldset': {
        padding: 0,
      },
    },
    shrinkCity: {
      left: -6,
    },
    zip: {
      width: '101px',
    },
  });

export interface AddressInfoTextProps {
  titleLabel?: string | React.ReactNode;
  addressLine1Label?: string | React.ReactNode;
  addressLine2Label?: string | React.ReactNode;
  countryLabel?: string | React.ReactNode;
  cityLabel?: string | React.ReactNode;
  stateLabel?: string | React.ReactNode;
  zipLabel?: string | React.ReactNode;
  fieldIsRequired: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, AddressInfoTextProps {
  address: PortalAddress;
  countryItems?: DropDownItem[];
  onTitleChange?: (value: string) => void;
  onCountryChange?: (value: DropDownItem) => void;
  onLine1Change?: (value: string) => void;
  onLine2Change?: (value: string) => void;
  onCityChange?: (item: ItemSuggestion) => void;
  onStateChange?: (item: ItemSuggestion) => void;
  onZipCodeChange?: (item: ItemSuggestion) => void;
  onAddressSearchChange?: (addressId: string, value: string) => void;
  addressSuggestions?: AddressLookup[];
  loadingAddressSuggestions?: boolean;
  onCitySearchChange?: (addressId: string, value: string) => void;
  citySuggestions?: ItemSuggestion[];
  loadingCitySuggestions?: boolean;
  onStateSearchChange?: (
    addressId: string,
    value: string,
    countryId: string
  ) => void;
  stateSuggestions?: ItemSuggestion[];
  loadingStateSuggestions?: boolean;
  onZipCodeSearchChange?: (addressId: string, value: string) => void;
  zipCodeSuggestions?: ItemSuggestion[];
  loadingZipCodeSuggestions?: boolean;
  updateFormIsValid?: (step: ProfileStep, valid: boolean) => void;
}

/**
 * AddressInfoFields
 *
 * @param props - Component props
 * @returns A JSX element
 */
function AddressInfoFields(props: Props): JSX.Element {
  const { classes, address, updateFormIsValid } = props;
  const {
    isValid: titleIsValid,
    hasError: titleInputHasError,
    valueChangedHandler: titleInputChangedHandler,
    inputBlurHandler: titleInputBlurHandler,
  } = useTextFieldValidation(address?.title ?? '', utilService.isNotEmpty);
  const {
    isValid: addressIsValid,
    hasError: addressInputHasError,
    valueChangedHandler: addressInputChangedHandler,
    inputBlurHandler: addressInputBlurHandler,
  } = useTextFieldValidation(address?.line1 ?? '', utilService.isNotEmpty);
  const {
    isValid: cityIsValid,
    hasError: cityInputHasError,
    valueChangedHandler: cityInputChangedHandler,
    inputBlurHandler: cityInputBlurHandler,
  } = useTextFieldValidation(address?.cityId ?? '', utilService.isNotEmpty);
  const {
    isValid: stateIsValid,
    hasError: stateInputHasError,
    valueChangedHandler: stateInputChangedHandler,
    inputBlurHandler: stateInputBlurHandler,
  } = useTextFieldValidation(address?.stateId ?? '', utilService.isNotEmpty);
  const {
    isValid: zipIsValid,
    hasError: zipInputHasError,
    valueChangedHandler: zipInputChangedHandler,
    inputBlurHandler: zipInputBlurHandler,
  } = useTextFieldValidation(address?.zipCodeId ?? '', utilService.isNotEmpty);

  useEffect(() => {
    //prop updateFormIsValid is not required for validations on modal
    // component used to edit existing emails, addresses and phones
    if (updateFormIsValid) {
      if (
        titleIsValid &&
        address?.countryId &&
        addressIsValid &&
        cityIsValid &&
        stateIsValid &&
        zipIsValid
      ) {
        updateFormIsValid('address', true);
      } else {
        updateFormIsValid('address', false);
      }
    }
  }, [
    updateFormIsValid,
    titleIsValid,
    address,
    addressIsValid,
    cityIsValid,
    stateIsValid,
    zipIsValid,
  ]);

  const onTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onTitleChange && props.onTitleChange(e.target.value);
  };

  const onAddressSearchChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onAddressSearchChange &&
      props.onAddressSearchChange(address.id, e.target.value);
  };

  const onAddressSelected = (value: string): void => {
    //TODO: check if it's required to show validation error right after
    //selecting a suggestion, if the selected suggestion does not
    //contain city or zip code

    // const suggestion = props.addressSuggestions.find(
    //   (el) => el.formattedaddr === value
    // );
    // if (!suggestion?.city) {
    //   setCityInputError(true);
    // } else {
    //   setCityInputError(false);
    // }
    // if (!suggestion?.zip) {
    //   setZipCodeInputError(true);
    // } else {
    //   setZipCodeInputError(false);
    // }
    props.onLine1Change && props.onLine1Change(value);
  };

  const onLine2Change = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onLine2Change && props.onLine2Change(e.target.value);
  };

  const onCitySearchChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onCitySearchChange &&
      props.onCitySearchChange(address.id, e.target.value);
  };

  const onStateSearchChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onStateSearchChange &&
      props.onStateSearchChange(
        address.id,
        e.target.value,
        address?.countryId ?? ''
      );
  };

  const onZipCodeSearchChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    props.onZipCodeSearchChange &&
      props.onZipCodeSearchChange(address.id, e.target.value);
  };

  return (
    <Box className={classes.root}>
      <Box className={classes.line}>
        <CustomTextField
          classes={{ root: classes.addressTitle }}
          ptasVariant="outline"
          label={props.titleLabel ?? 'Address title'}
          value={address?.title ?? ''}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            titleInputChangedHandler(e);
            onTitleChange(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={titleInputBlurHandler}
          error={titleInputHasError}
          helperText={
            titleInputHasError
              ? props.fieldIsRequired ?? 'This field is required'
              : ''
          }
        />
        <SimpleDropDown
          items={props.countryItems ?? []}
          label={props.countryLabel ?? 'Country'}
          onSelected={(item: DropDownItem): void =>
            props.onCountryChange?.(item)
          }
          defaultValue={address?.countryId}
          classes={{
            textFieldRoot: classes.countryDropdown,
            inputRoot: classes.countryDropdownInput,
            // animated: classes.dropdownLabel
          }}
        />
      </Box>
      <Box className={classes.line}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={props.addressLine1Label ?? 'Address'}
          value={address?.line1 ?? ''}
          classes={{
            wrapper: classes.addressLine,
          }}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            addressInputChangedHandler(e);
            onAddressSearchChange(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={addressInputBlurHandler}
          suggestion={{
            List: props.addressSuggestions?.map(
              (el) =>
                ({
                  title: el.formattedaddr,
                } as ItemSuggestion)
            ),
            loading: !!props.loadingAddressSuggestions,
            onSelected: (item: ItemSuggestion): void => {
              console.log('Suggestion selected');
              onAddressSelected(item.title);
            },
          }}
          error={addressInputHasError}
          helperText={
            addressInputHasError ? props.fieldIsRequired ?? '*Required' : ''
          }
        />
      </Box>
      <Box className={classes.line}>
        <CustomTextField
          classes={{ root: classes.addressLine }}
          ptasVariant="outline"
          label={props.addressLine2Label ?? 'Address (cont.)'}
          value={address?.line2 ?? ''}
          onChange={onLine2Change}
          onChangeDelay={CHANGE_DELAY_MS}
        />
      </Box>
      <Box className={clsx(classes.line, classes.lastLine)}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          hideSearchIcon
          label={props.cityLabel ?? 'City'}
          value={address?.city ?? ''}
          // initialValue={address?.city ?? ''}
          classes={{
            wrapper: classes.city,
          }}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            cityInputChangedHandler(e);
            onCitySearchChange(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={cityInputBlurHandler}
          suggestion={{
            List: props.citySuggestions,
            loading: !!props.loadingCitySuggestions,
            onSelected: (item: ItemSuggestion): void => {
              if (item.id && typeof item.id === 'string') {
                props.onCityChange?.(item);
              }
            },
          }}
          error={cityInputHasError}
          helperText={
            cityInputHasError ? props.fieldIsRequired ?? '*Required' : ''
          }
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          hideSearchIcon
          label={props.stateLabel ?? 'State'}
          value={address?.state ?? ''}
          classes={{
            wrapper: classes.state,
            shrinkRoot: classes.shrinkCity,
          }}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            stateInputChangedHandler(e);
            onStateSearchChange(e);
          }}
          onChangeDeps={[address]}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={stateInputBlurHandler}
          suggestion={{
            List: props.stateSuggestions,
            loading: !!props.loadingStateSuggestions,
            onSelected: (item: ItemSuggestion): void => {
              if (item.id && typeof item.id === 'string') {
                props.onStateChange?.(item);
              }
            },
          }}
          error={stateInputHasError}
          helperText={
            stateInputHasError ? props.fieldIsRequired ?? '*Required' : ''
          }
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          hideSearchIcon
          label={props.zipLabel ?? 'Zip'}
          value={address?.zipCode ?? ''}
          classes={{
            wrapper: classes.zip,
          }}
          onChange={(
            e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
          ): void => {
            zipInputChangedHandler(e);
            onZipCodeSearchChange(e);
          }}
          onChangeDelay={CHANGE_DELAY_MS}
          onBlur={zipInputBlurHandler}
          suggestion={{
            List: props.zipCodeSuggestions,
            loading: !!props.loadingZipCodeSuggestions,
            onSelected: (item: ItemSuggestion): void => {
              if (item.id && typeof item.id === 'string') {
                props.onZipCodeChange?.(item);
              }
            },
          }}
          error={zipInputHasError}
          helperText={
            zipInputHasError ? props.fieldIsRequired ?? '*Required' : ''
          }
        />
      </Box>
    </Box>
  );
}

export default withStyles(styles)(AddressInfoFields) as FC<
  GenericWithStyles<Props>
>;
