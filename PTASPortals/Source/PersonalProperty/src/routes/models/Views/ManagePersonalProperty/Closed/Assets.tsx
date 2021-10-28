// Assets.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useContext, useEffect } from 'react';
import {
  CustomTextField,
  SimpleDropDown,
  CustomTextButton,
  CustomSearchTextField,
  ItemSuggestion,
  DropDownItem,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import { apiService } from 'services/api/apiService';
import { OptionSet } from 'models/optionSet';
import useManageData from '../useManageData';
import useAddress from 'hooks/useAddress';
import * as generalFm from '../../../../../GeneralFormatMessage';

function Assets(): JSX.Element {
  const classes = useStyles();
  const {
    dispositionAssetsOpts,
    setDispositionAssetsOpts,
    setFormIsInvalid,
  } = useContext(ChangedBusinessContext);
  const [showInfo, setShowInfo] = useState<boolean>(false);
  const { quickCollect, setQuickCollect } = useManageData('CLOSED');
  const {
    handleAddressSuggestions,
    getInfoFromAddressSuggestionSelected,
    addressSuggestionList,
    loadingAddressSug,
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
    isValid: addressIsValid,
    hasError: addressInputHasError,
    inputBlurHandler: addressInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.businessAddress1 ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: cityIsValid,
    hasError: cityInputHasError,
    inputBlurHandler: cityInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.businessCityLabel ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: stateIsValid,
    hasError: stateInputHasError,
    inputBlurHandler: stateInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.businessStateLabel ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: zipIsValid,
    hasError: zipInputHasError,
    inputBlurHandler: zipInputBlurHandler,
  } = useTextFieldValidation(
    quickCollect?.businessZipLabel ?? '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    fetchDispositionOfAssetsOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const fetchDispositionOfAssetsOptions = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_quickcollect',
      'ptas_dispositionofassets'
    );
    if (!data) return;
    setDispositionAssetsOpts(data);
  };

  useEffect(() => {
    const allFieldsIsOk =
      addressIsValid && cityIsValid && stateIsValid && zipIsValid;
    setFormIsInvalid(!allFieldsIsOk);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [addressIsValid, cityIsValid, stateIsValid, zipIsValid]);

  const items = [
    { value: 'one', label: 'One' },
    { value: 'two', label: 'Two' },
  ];

  const handleSelectDispositionAsset = (item: DropDownItem): void => {
    setShowInfo(true);
    setQuickCollect(prev => {
      return {
        ...prev,
        dispositionOfAssets: item.value as string,
      };
    });
  };

  const dropdownOptionsMapping = (
    options: OptionSet[]
  ): { value: number; label: string }[] => {
    return options.map(o => ({
      label: o.value,
      value: o.attributeValue,
    }));
  };

  useEffect(() => {
    if (loadingAddressSug && quickCollect?.requestorAddress1) {
      setQuickCollect(prev => {
        return {
          ...prev,
          requestorAddress1: '',
        };
      });
    }
    if (loadingCitySug && quickCollect?.requestorCityLabel) {
      setQuickCollect(prev => {
        return {
          ...prev,
          requestorCity: '',
        };
      });
    }
    if (loadingStateSug && quickCollect?.requestorStateLabel) {
      setQuickCollect(prev => {
        return {
          ...prev,
          requestorStateId: '',
          requestorStateLabel: '',
        };
      });
    }
    if (loadingZipSug && quickCollect?.requestorZipLabel) {
      setQuickCollect(prev => {
        return {
          ...prev,
          requestorZip: '',
        };
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [loadingAddressSug, loadingCitySug, loadingStateSug, loadingZipSug]);

  const handleSuggestionSelect = (inputName: string) => async (
    item: ItemSuggestion
  ): Promise<void> => {
    switch (inputName) {
      case 'address':
        {
          const addressData = await getInfoFromAddressSuggestionSelected(
            item.id as string
          );
          setQuickCollect(prev => {
            return {
              ...prev,
              requestorAddress1: addressData.address,
              requestorCity: addressData.city?.ptas_name ?? '',
              // TODO: (160944) country in quick collect not exist
            };
          });
        }
        break;
      case 'city':
        {
          const cityData = await getInfoFromCitySuggestionSelected(
            item.id as string
          );
          setQuickCollect(prev => {
            return {
              ...prev,
              requestorCity: cityData?.ptas_name ?? '',
            };
          });
        }
        break;
      case 'state':
        {
          const stateData = await getInfoFromStateSuggestionSelected(
            item.id as string
          );
          setQuickCollect(prev => {
            return {
              ...prev,
              requestorStateId: stateData?.ptas_stateorprovinceid ?? '',
              requestorStateLabel: stateData?.ptas_name ?? '',
            };
          });
        }
        break;
      case 'zipCode':
        {
          const zipData = await getInfoFromZipSuggestionSelected(
            item.id as string
          );
          setQuickCollect(prev => {
            return {
              ...prev,
              requestorZip: zipData?.ptas_name ?? '',
            };
          });
        }
        break;
      default:
        break;
    }
  };

  const onChangeCustomTextField = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    if (!e.currentTarget) return;
    const { name, value } = e.currentTarget;
    setQuickCollect(prev => {
      return {
        ...prev,
        [name]: value,
      };
    });
  };

  const renderInfo = (): JSX.Element => {
    if (!showInfo) return <Fragment />;

    return (
      <Fragment>
        <CustomTextButton
          ptasVariant="Text more"
          classes={{ root: classes.useOtherAddress }}
        >
          {fm.userOtherAddress}
        </CustomTextButton>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.addressOfAssets}
          onChangeDelay={500}
          hideSearchIcon={true}
          value={quickCollect?.requestorAddress1 ?? ''}
          onChange={handleAddressSuggestions}
          onBlur={addressInputBlurHandler}
          suggestion={{
            List: addressSuggestionList,
            onSelected: handleSuggestionSelect('address'),
            loading: loadingAddressSug,
          }}
          error={addressInputHasError}
          helperText={addressInputHasError ? generalFm.fieldRequired : ''}
          classes={{
            wrapper: classes.addressInputWrap,
          }}
        />
        <CustomTextField
          ptasVariant="outline"
          label={fm.addressCont}
          classes={{ root: classes.inputs }}
        />
        <div className={classes.inputWrap}>
          <CustomSearchTextField
            ptasVariant="squared outline"
            label={fm.city}
            onChangeDelay={500}
            hideSearchIcon={true}
            value={quickCollect?.requestorCityLabel ?? ''}
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
            value={quickCollect?.requestorStateLabel ?? ''}
            onChange={handleStateSuggestions}
            onBlur={stateInputBlurHandler}
            suggestion={{
              List: stateSuggestionList,
              onSelected: handleSuggestionSelect('state'),
              loading: loadingStateSug,
            }}
            error={stateInputHasError}
            helperText={stateInputHasError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.addressStateInputWrap,
            }}
          />
          <CustomTextField
            name="requestorAddress2"
            value={quickCollect?.requestorAddress2 ?? ''}
            onChange={onChangeCustomTextField}
            ptasVariant="outline"
            classes={{ root: classes.zip }}
            label={fm.zip}
          />
          <CustomSearchTextField
            ptasVariant="squared outline"
            label={fm.zip}
            onChangeDelay={500}
            hideSearchIcon={true}
            value={quickCollect?.requestorZipLabel ?? ''}
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
          items={items}
          label={fm.country}
          defaultValue="one"
          classes={{
            textFieldRoot: classes.countryDropdown,
            root: classes.rootDropdown,
          }}
        />
      </Fragment>
    );
  };

  return (
    <div className={classes.movedRoot}>
      <SimpleDropDown
        value={quickCollect?.dispositionOfAssets ?? ''}
        onSelected={handleSelectDispositionAsset}
        items={dropdownOptionsMapping(dispositionAssetsOpts)}
        label={fm.dispositionOfAssets}
        classes={{
          root: classes.dispositionAsset,
        }}
      />
      {renderInfo()}
    </div>
  );
}

export default Assets;
