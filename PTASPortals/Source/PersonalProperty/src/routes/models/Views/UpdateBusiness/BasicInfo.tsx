// BasicInfo .tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomSearchTextField,
  ItemSuggestion,
  DropDownItem,
  utilService,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { makeStyles, Box, Theme } from '@material-ui/core';
import clsx from 'clsx';
import useUpdateBusiness from './useUpdateBusiness';
import { businessApiService } from 'services/api/apiService/business';
import * as generalFm from '../../../../GeneralFormatMessage';
import { PersonalProperty } from 'models/personalProperty';

const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: 280,
    marginBottom: 25,
  },
  normalInput: {
    maxWidth: 270,
    marginBottom: 25,
    display: 'block',
  },
  ubiNumber: {
    maxWidth: 143,
  },
  NAICSNumber: {
    maxWidth: 126,
  },
  dropdownBusiness: {
    width: 270,
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
}));

function BasicInfo(): JSX.Element {
  const classes = useStyles();
  const {
    propertyType,
    updatedBusiness,
    setNaicsNumberSuggestionList,
    setUpdatedBusiness,
    saveUpdateBusiness,
    naicsnumberSuggestionList,
  } = useUpdateBusiness();

  const {
    hasError: businessNameInputHasError,
    inputBlurHandler: businessNameInputBlurHandler,
  } = useTextFieldValidation(
    updatedBusiness.businessName ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: ubiNumberInputHasError,
    inputBlurHandler: ubiNumberInputBlurHandler,
  } = useTextFieldValidation(updatedBusiness.ubi ?? '', utilService.isNotEmpty);

  const {
    hasError: naicsNumberInputHasError,
    inputBlurHandler: naicsNumberInputBlurHandler,
  } = useTextFieldValidation(
    updatedBusiness.naicsNumber ?? '',
    utilService.isNotEmpty
  );

  const handleChangeNaicsNumber = async (
    e: React.ChangeEvent<HTMLInputElement>
  ): Promise<void> => {
    const isNotNumber = isNaN(parseInt(e.target.value));
    const value = e.target.value;

    if (!value || isNotNumber) {
      setNaicsNumberSuggestionList([]);

      return setUpdatedBusiness((prev) => {
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
      saveUpdateBusiness('naicsNumber', '');
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

  const handleSelectNaicsNumber = (item: ItemSuggestion): void => {
    setUpdatedBusiness((prev) => {
      return {
        ...prev,
        naicsNumber: (item.id as string) ?? '',
        naicsDescription: item.title ?? '',
      };
    });
  };

  const handleChangePropType = (item: DropDownItem): void => {
    saveUpdateBusiness('propertyType', parseInt(item.value.toString()));
  };

  const handleChangeCustomText = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const value = e.target.value;
    const name = e.target.name as keyof PersonalProperty;

    saveUpdateBusiness(name, value);
  };

  return (
    <div className={classes.contentWrap}>
      <CustomTextField
        ptasVariant="outline"
        label={fm.businessName}
        classes={{ root: classes.normalInput }}
        onChangeDelay={500}
        name="businessName"
        value={updatedBusiness.businessName}
        onChange={handleChangeCustomText}
        error={businessNameInputHasError}
        helperText={businessNameInputHasError ? generalFm.fieldRequired : ''}
        onBlur={businessNameInputBlurHandler}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.UBINumber}
        classes={{ root: clsx(classes.normalInput, classes.ubiNumber) }}
        onChangeDelay={500}
        name="ubi"
        value={updatedBusiness.ubi}
        onChange={handleChangeCustomText}
        error={ubiNumberInputHasError}
        helperText={ubiNumberInputHasError ? generalFm.fieldRequired : ''}
        onBlur={ubiNumberInputBlurHandler}
      />

      <Box className={classes.naicsNumber}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.NAICSNumber}
          value={updatedBusiness.naicsDescription}
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
          error={naicsNumberInputHasError}
          helperText={naicsNumberInputHasError ? generalFm.fieldRequired : ''}
          onBlur={naicsNumberInputBlurHandler}
        />
      </Box>
      <SimpleDropDown
        items={propertyType}
        label={fm.businessType}
        classes={{
          root: classes.normalInput,
          textFieldRoot: classes.dropdownBusiness,
        }}
        onSelected={handleChangePropType}
        value={updatedBusiness.propertyType.toString()}
      />
    </div>
  );
}

export default BasicInfo;
