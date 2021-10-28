// NewContact.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect } from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomPhoneTextField,
  CustomSearchTextField,
  ItemSuggestion,
  DropDownItem,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import useManageData from '../useManageData';
import useAddress from 'hooks/useAddress';
import * as generalFm from '../../../../../GeneralFormatMessage';

function NewContact(): JSX.Element {
  const classes = useStyles();
  const { countryOptions, setFormIsInvalid } = useContext(
    ChangedBusinessContext
  );
  const { businessHistory, setBusinessHistory } = useManageData('SOLD');
  const {
    handleCitySuggestions,
    getInfoFromCitySuggestionSelected,
    citySuggestionList,
    loadingCitySug,
    handleStateSuggestions,
    getInfoFromStateSuggestionSelected,
    stateSuggestionList,
    loadingStateSug,
    handleZipCodeSuggestions,
    getInfoFromZipSuggestionSelected,
    zipCodeSuggestionList,
    loadingZipSug,
  } = useAddress();

  const {
    isValid: preparerNameIsValid,
    hasError: preparerNameInputHasError,
    valueChangedHandler: preparerNameInputChangedHandler,
    inputBlurHandler: preparerNameInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.preparerName ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: preparerPhoneIsValid,
    hasError: preparerPhoneInputHasError,
    valueChangedHandler: preparerPhoneInputChangedHandler,
    inputBlurHandler: preparerPhoneInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.preparerCellPhone1 ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: cityIsValid,
    hasError: cityInputHasError,
    inputBlurHandler: cityInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.preparerCityLabel ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: stateIsValid,
    hasError: stateInputHasError,
    inputBlurHandler: stateInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.preparerStateLabel ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: zipIsValid,
    hasError: zipInputHasError,
    inputBlurHandler: zipInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.preparerZipLabel ?? '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    const formIsOk =
      preparerNameIsValid &&
      preparerPhoneIsValid &&
      cityIsValid &&
      stateIsValid &&
      zipIsValid;
    setFormIsInvalid(!formIsOk);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    preparerNameIsValid,
    preparerPhoneIsValid,
    cityIsValid,
    stateIsValid,
    zipIsValid,
  ]);

  useEffect(() => {
    if (loadingCitySug && businessHistory?.preparerCityLabel) {
      setBusinessHistory(prev => {
        return {
          ...prev,
          preparerCityId: '',
          preparerCityLabel: '',
        };
      });
    }
    if (loadingStateSug && businessHistory?.preparerStateLabel) {
      setBusinessHistory(prev => {
        return {
          ...prev,
          preparerStateId: '',
          preparerStateLabel: '',
        };
      });
    }
    if (loadingZipSug && businessHistory?.preparerZipLabel) {
      setBusinessHistory(prev => {
        return {
          ...prev,
          preparerZipId: '',
          preparerZipLabel: '',
        };
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [loadingCitySug, loadingStateSug, loadingZipSug]);

  const handleChangeCustomTextField = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    if (!e.currentTarget) return;
    const { name, value } = e.currentTarget;
    if (name === 'preparerName') preparerNameInputChangedHandler(e);
    if (name === 'preparerCellPhone1') preparerPhoneInputChangedHandler(e);
    setBusinessHistory(prev => {
      return {
        ...prev,
        [name]: value,
      };
    });
  };

  const handleSuggestionSelect = (inputName: string) => async (
    item: ItemSuggestion
  ): Promise<void> => {
    switch (inputName) {
      case 'address':
        // Address field not found
        break;
      case 'city':
        {
          const cityData = await getInfoFromCitySuggestionSelected(
            item.id as string
          );
          setBusinessHistory(prev => {
            return {
              ...prev,
              preparerCityId: cityData?.ptas_cityid ?? '',
              preparerCityLabel: cityData?.ptas_name ?? '',
            };
          });
        }
        break;
      case 'state':
        {
          const stateData = await getInfoFromStateSuggestionSelected(
            item.id as string
          );
          setBusinessHistory(prev => {
            return {
              ...prev,
              preparerStateId: stateData?.ptas_stateorprovinceid ?? '',
              preparerStateLabel: stateData?.ptas_name ?? '',
            };
          });
        }
        break;
      case 'zipCode':
        {
          const zipData = await getInfoFromZipSuggestionSelected(
            item.id as string
          );
          setBusinessHistory(prev => {
            return {
              ...prev,
              preparerZipId: zipData?.ptas_zipcodeid ?? '',
              preparerZipLabel: zipData?.ptas_name ?? '',
            };
          });
        }
        break;
      default:
        break;
    }
  };

  const handleSelectCountry = (item: DropDownItem): void => {
    setBusinessHistory(prev => {
      return {
        ...prev,
        preparerCountryId: item.value as string,
      };
    });
  };

  return (
    <div className={classes.newContractTab}>
      <CustomTextField
        name="preparerName"
        value={businessHistory?.preparerName ?? ''}
        onChange={handleChangeCustomTextField}
        onBlur={preparerNameInputBlurHandler}
        error={preparerNameInputHasError}
        helperText={preparerNameInputHasError ? generalFm.fieldRequired : ''}
        ptasVariant="outline"
        label={fm.newContact}
        classes={{ root: classes.inputs }}
      />

      <CustomTextField
        name="preparerAttention"
        value={businessHistory?.preparerAttention ?? ''}
        onChange={handleChangeCustomTextField}
        ptasVariant="outline"
        label={fm.attention}
        classes={{ root: classes.inputs }}
      />

      <CustomTextField
        name="preparerEmail1"
        value={businessHistory?.preparerEmail1 ?? ''}
        onChange={handleChangeCustomTextField}
        label={fm.newOwnerEmail}
        classes={{ root: classes.inputs }}
        type="email"
        ptasVariant="email"
      />
      <CustomPhoneTextField
        name="preparerCellPhone1"
        value={businessHistory?.preparerCellPhone1 ?? ''}
        onChange={handleChangeCustomTextField}
        onBlur={preparerPhoneInputBlurHandler}
        error={preparerPhoneInputHasError}
        helperText={preparerPhoneInputHasError ? generalFm.fieldRequired : ''}
        label={fm.newOwnerPhone}
        placeholder="000-000-0000"
        classes={{ root: classes.inputPhone }}
      />
      <CustomTextField
        label={fm.newOwnerMailingAddress}
        classes={{ root: classes.inputs }}
        ptasVariant="outline"
      />
      <CustomTextField
        label={fm.addressAccount}
        classes={{ root: classes.inputs }}
        ptasVariant="outline"
      />
      <div className={classes.inputWrap}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.city}
          onChangeDelay={500}
          hideSearchIcon={true}
          value={businessHistory?.preparerCityLabel ?? ''}
          onChange={handleCitySuggestions}
          onBlur={cityInputBlurHandler}
          suggestion={{
            List: citySuggestionList,
            onSelected: handleSuggestionSelect('city'),
            loading: loadingCitySug,
          }}
          error={cityInputHasError}
          helperText={cityInputHasError ? generalFm.fieldRequired : ''}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.state}
          onChangeDelay={500}
          hideSearchIcon={true}
          value={businessHistory?.preparerStateLabel ?? ''}
          onChange={handleStateSuggestions}
          onBlur={stateInputBlurHandler}
          suggestion={{
            List: stateSuggestionList,
            onSelected: handleSuggestionSelect('state'),
            loading: loadingStateSug,
          }}
          classes={{
            wrapper: classes.addressStateInputWrap,
          }}
          error={stateInputHasError}
          helperText={stateInputHasError ? generalFm.fieldRequired : ''}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.zip}
          onChangeDelay={500}
          hideSearchIcon={true}
          value={businessHistory?.preparerZipLabel ?? ''}
          onChange={handleZipCodeSuggestions}
          onBlur={zipInputBlurHandler}
          suggestion={{
            List: zipCodeSuggestionList,
            onSelected: handleSuggestionSelect('zipCode'),
            loading: loadingZipSug,
          }}
          error={zipInputHasError}
          helperText={zipInputHasError ? generalFm.fieldRequired : ''}
        />
      </div>
      <SimpleDropDown
        items={countryOptions}
        label={fm.country}
        defaultValue={businessHistory?.preparerCountryId ?? ''}
        onSelected={handleSelectCountry}
        classes={{
          textFieldRoot: classes.countryDropdown,
          root: classes.rootDropdown,
        }}
      />
    </div>
  );
}

export default NewContact;
