// NewInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useContext, useState } from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomSwitch,
  useTextFieldValidation,
  utilService,
  DropDownItem,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import useManageData from '../useManageData';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import * as generalFm from '../../../../../GeneralFormatMessage';
import { addressService } from 'services/api/apiService/address';
import { useMount } from 'react-use';
import { apiService } from 'services/api/apiService';
import { dropdownOptionsMapping } from 'utils';

enum DropdownId {
  BusinessType = 'BusinessType',
  StateOfInc = 'StateOfInc',
}
type DropdownType = keyof typeof DropdownId;

function NewInfo(): JSX.Element {
  const classes = useStyles();
  const { setFormIsInvalid } = useContext(ChangedBusinessContext);
  const {
    quickCollect,
    businessHistory,
    setQuickCollect,
    setBusinessHistory,
  } = useManageData('SOLD');
  const [stateOrProvince, setStateOrProvince] = useState<DropDownItem[]>([]);
  const [propertyTypeOpts, setPropertyTypeOpts] = useState<DropDownItem[]>([]);
  const {
    isValid: ownerNameIsValid,
    hasError: ownerNameInputHasError,
    valueChangedHandler: ownerNameInputChangedHandler,
    inputBlurHandler: ownerNameInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.newOwnerName ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: businessNameIsValid,
    hasError: businessNameInputHasError,
    valueChangedHandler: businessNameInputChangedHandler,
    inputBlurHandler: businessNameInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.newBusinessName ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: ubiNumberIsValid,
    hasError: ubiNumberInputHasError,
    valueChangedHandler: ubiNumberInputChangedHandler,
    inputBlurHandler: ubiNumberInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.ubiNumber ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: stateOfIncorporationInputHasError,
    inputBlurHandler: stateOfIncorporationInputBlurHandler,
  } = useTextFieldValidation(
    businessHistory?.businessStateIncorporateId ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: propertyTypeInputHasError,
    inputBlurHandler: propertyTypeInputBlurHandler,
  } = useTextFieldValidation(
    `${businessHistory?.propertyType ?? ''}`,
    utilService.isNotEmpty
  );

  useMount(() => {
    loadStateOrProvince();
    loadPropertyTypes();
  });

  useEffect(() => {
    const formIsOk =
      ownerNameIsValid && businessNameIsValid && ubiNumberIsValid;
    setFormIsInvalid(!formIsOk);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ownerNameIsValid, businessNameIsValid, ubiNumberIsValid]);

  const handleChangeCustomTextField = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    if (!e.currentTarget) return;
    const { name, value } = e.currentTarget;
    if (name === 'newOwnerName') ownerNameInputChangedHandler(e);
    if (name === 'newBusinessName') businessNameInputChangedHandler(e);
    if (name === 'ubiNumber') ubiNumberInputChangedHandler(e);
    if (Object.prototype.hasOwnProperty.call(PersonalPropertyHistory, name)) {
      setBusinessHistory(prev => {
        return {
          ...prev,
          [name]: value,
        };
      });
    } else {
      setQuickCollect(prev => {
        return {
          ...prev,
          [name]: value,
        };
      });
    }
  };

  const loadStateOrProvince = async (): Promise<void> => {
    const { data } = await addressService.getStatesOrProvince();
    const countriesOptions = (data || []).map(c => ({
      value: c.ptas_stateorprovinceid,
      label: c.ptas_name, // c.ptas_abbreviation
    }));

    setStateOrProvince(countriesOptions);
  };

  const loadPropertyTypes = async (): Promise<void> => {
    const { data = [] } = await apiService.getOptionSet(
      'ptas_personalpropertyhistory',
      'ptas_property_type'
    );
    const propertyTypesFound = dropdownOptionsMapping(
      data,
      'value',
      'attributeValue'
    );
    setPropertyTypeOpts(propertyTypesFound);
  };

  const onSelectDropdown = (dropdownId: DropdownType) => (
    item: DropDownItem
  ): void => {
    let updatedValue = {};
    switch (dropdownId) {
      case DropdownId.BusinessType:
        updatedValue = {
          propertyType: item.value,
        };
        break;
      case DropdownId.StateOfInc:
        updatedValue = {
          businessStateIncorporateId: item.value,
        };
        break;
      default:
        break;
    }

    setBusinessHistory(prev => {
      return {
        ...prev,
        ...updatedValue,
      };
    });
  };

  return (
    <div className={classes.newContractTab}>
      <CustomTextField
        ptasVariant="outline"
        label={fm.newOwnerTaxpayer}
        classes={{ root: classes.inputs }}
        name="newOwnerName"
        value={quickCollect?.newOwnerName}
        onChange={handleChangeCustomTextField}
        onBlur={ownerNameInputBlurHandler}
        error={ownerNameInputHasError}
        helperText={ownerNameInputHasError ? generalFm.fieldRequired : ''}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.newBusinessName}
        classes={{ root: classes.inputs }}
        name="newBusinessName"
        value={quickCollect?.newBusinessName}
        onChange={handleChangeCustomTextField}
        onBlur={businessNameInputBlurHandler}
        error={businessNameInputHasError}
        helperText={businessNameInputHasError ? generalFm.fieldRequired : ''}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.newUBINumber}
        classes={{ root: `${classes.inputs} ${classes.ubiNumber}` }}
        name="ubiNumber"
        value={quickCollect?.ubiNumber}
        onChange={handleChangeCustomTextField}
        onBlur={ubiNumberInputBlurHandler}
        error={ubiNumberInputHasError}
        helperText={ubiNumberInputHasError ? generalFm.fieldRequired : ''}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.NAICSNumber}
        classes={{ root: `${classes.inputs} ${classes.NAICSNumber}` }}
      />

      <SimpleDropDown
        items={propertyTypeOpts}
        label={fm.businessType}
        value={businessHistory?.propertyType ?? ''}
        onSelected={onSelectDropdown(DropdownId.BusinessType)}
        onBlur={propertyTypeInputBlurHandler}
        error={propertyTypeInputHasError}
        helperText={propertyTypeInputHasError ? 'field required' : ''}
        classes={{
          textFieldRoot: classes.dropdown,
          root: classes.inputs,
        }}
      />
      <SimpleDropDown
        items={stateOrProvince}
        label={fm.stateOfIncorporation}
        value={businessHistory?.businessStateIncorporateId ?? ''}
        onSelected={onSelectDropdown(DropdownId.StateOfInc)}
        onBlur={stateOfIncorporationInputBlurHandler}
        error={stateOfIncorporationInputHasError}
        helperText={stateOfIncorporationInputHasError ? 'field required' : ''}
        classes={{
          textFieldRoot: classes.dropdown,
          root: classes.inputs,
        }}
      />
      <CustomSwitch
        label={
          <span className={classes.switchLabel}>
            {fm.businessHasChangeLocation}
          </span>
        }
        ptasVariant="small"
        showOptions
      />
    </div>
  );
}

export default NewInfo;
