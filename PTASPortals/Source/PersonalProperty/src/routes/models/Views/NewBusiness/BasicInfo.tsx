// BasicInfo .tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomSwitch,
  CustomDatePicker,
  CustomTextarea,
  CustomSearchTextField,
  ItemSuggestion,
  utilService,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import { Box, makeStyles, Theme } from '@material-ui/core';
import clsx from 'clsx';
import useNewBusiness from './useNewBusiness';
import { businessApiService } from 'services/api/apiService/business';

const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: 280,
    marginBottom: 25,
    marginTop: 8,
  },
  normalInput: {
    maxWidth: 270,
    marginBottom: 25,
    display: 'block',
  },
  dateOpened: {
    width: 135,
    marginBottom: 17,
    marginRight: 'auto',
    marginTop: 0,
  },
  switchLabel: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  ubiNumber: {
    maxWidth: 143,
  },
  switch: {
    marginBottom: 23,
  },
  dropdownBusiness: {
    width: 270,
  },
  formPurchasedMoved: {
    paddingLeft: 44,
    marginTop: 18,
  },
  inputPurchased: {
    maxWidth: 222,
    marginBottom: 25,
    display: 'block',
  },
  naicsNumber: {
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    lineHeight: '15px',
    marginBottom: 25,
  },
  searchWrapper: {
    width: 126,
  },
  naicsNumberLabel: {
    width: '100%',
    color: 'rgba(0, 0, 0, 0.54)',
    paddingLeft: 9,
    boxSizing: 'border-box',
  },
}));

function BasicInfo(): JSX.Element {
  const classes = useStyles();
  const {
    showFormPurchasedMoved,
    setShowFormPurchasedMoved,
    propertyType,
    naicsnumberSuggestionList,
    newBusiness,
    handleChangeCustomText,
    handleSelectNaicsNumber,
    handleChangePropType,
    setNaicsNumberSuggestionList,
    setNewBusiness,
    saveNewBusiness,
  } = useNewBusiness();

  const {
    hasError: businessNameInputHasError,
    inputBlurHandler: businessNameInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.businessName ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: ubiNumberInputHasError,
    inputBlurHandler: ubiNumberInputBlurHandler,
  } = useTextFieldValidation(newBusiness.ubi ?? '', utilService.isNotEmpty);

  const {
    hasError: naicsNumberInputHasError,
    inputBlurHandler: naicsNumberInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.naicsNumber ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: businessTypeInputHasError,
    inputBlurHandler: businessTypeInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.propertyType.toString() ?? '',
    utilService.isNotEmpty
  );

  const handleChangeNaicsNumber = async (
    e: React.ChangeEvent<HTMLInputElement>
  ): Promise<void> => {
    const isNotNumber = isNaN(parseInt(e.target.value));
    const value = e.target.value;

    if (!value || isNotNumber) {
      setNaicsNumberSuggestionList([]);

      return setNewBusiness((prev) => {
        return {
          ...prev,
          naicsNumber: '',
          naicsDescription: value,
        };
      });
    }

    const { hasError, data: suggestionList } =
      await businessApiService.getNaicNumberSuggestions(parseInt(value));

    if (hasError || !suggestionList?.length) {
      saveNewBusiness('naicsNumber', '');
      return setNaicsNumberSuggestionList([]);
    }

    const naicsCodeMapping: ItemSuggestion[] = suggestionList.map((ncode) => {
      return {
        title: ncode.ptas_code.toString(),
        subtitle: ncode.ptas_description,
        id: ncode.ptas_naicscodeid,
      };
    });

    setNaicsNumberSuggestionList(naicsCodeMapping);
  };

  const renderFormPurchasedMoved = (): JSX.Element => {
    if (!showFormPurchasedMoved) return <Fragment />;

    return (
      <div className={classes.formPurchasedMoved}>
        <CustomTextField
          ptasVariant="outline"
          label={fm.previousOwnerName}
          classes={{ root: classes.inputPurchased }}
        />
        <CustomTextField
          ptasVariant="outline"
          label={fm.previousLocationAddress}
          classes={{ root: classes.inputPurchased }}
        />
        <CustomTextarea
          label={fm.previousLocationAddress}
          classes={{ root: classes.inputPurchased }}
        />
      </div>
    );
  };

  const handelShowForm = (isChecked: boolean): void =>
    setShowFormPurchasedMoved(isChecked);

  return (
    <div className={classes.contentWrap}>
      <CustomTextField
        ptasVariant="outline"
        label={fm.businessName}
        classes={{ root: classes.normalInput }}
        error={businessNameInputHasError}
        onChange={handleChangeCustomText}
        helperText={businessNameInputHasError ? generalFm.fieldRequired : ''}
        onBlur={businessNameInputBlurHandler}
        onChangeDelay={500}
        name="businessName"
        value={newBusiness.businessName}
      />
      <CustomDatePicker
        label={fm.dateOpened}
        classes={{ root: classes.dateOpened }}
        placeholder="mm/dd/yyyy"
      />
      <CustomSwitch
        label={
          <span className={classes.switchLabel}>{fm.startupBusiness}</span>
        }
        ptasVariant="small"
        showOptions
        classes={{
          formControlRoot: classes.switch,
        }}
      />
      <CustomSwitch
        label={
          <span className={classes.switchLabel}>
            {fm.purchasedOrRemovedThisBusiness}
          </span>
        }
        ptasVariant="small"
        showOptions
        classes={{
          formControlRoot: !showFormPurchasedMoved ? classes.switch : '',
        }}
        isChecked={handelShowForm}
      />
      {renderFormPurchasedMoved()}
      <CustomTextField
        ptasVariant="outline"
        label={fm.UBINumber}
        error={ubiNumberInputHasError}
        helperText={ubiNumberInputHasError ? generalFm.fieldRequired : ''}
        onBlur={ubiNumberInputBlurHandler}
        classes={{ root: clsx(classes.normalInput, classes.ubiNumber) }}
        onChange={handleChangeCustomText}
        onChangeDelay={500}
        name="ubi"
        value={newBusiness.ubi}
      />

      <Box className={classes.naicsNumber}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          error={naicsNumberInputHasError}
          helperText={naicsNumberInputHasError ? generalFm.fieldRequired : ''}
          onBlur={naicsNumberInputBlurHandler}
          label={fm.NAICSNumber}
          value={newBusiness.naicsDescription}
          classes={{
            wrapper: classes.searchWrapper,
          }}
          onChangeDelay={500}
          onChange={handleChangeNaicsNumber}
          hideSearchIcon
          suggestion={{
            List: naicsnumberSuggestionList,
            onSelected: handleSelectNaicsNumber,
          }}
        />
      </Box>

      <SimpleDropDown
        items={propertyType}
        error={businessTypeInputHasError}
        helperText={businessTypeInputHasError ? generalFm.fieldRequired : ''}
        onBlur={businessTypeInputBlurHandler}
        label={fm.businessType}
        classes={{
          root: classes.normalInput,
          textFieldRoot: classes.dropdownBusiness,
        }}
        onSelected={handleChangePropType}
        value={newBusiness.propertyType.toString()}
      />
    </div>
  );
}

export default BasicInfo;
