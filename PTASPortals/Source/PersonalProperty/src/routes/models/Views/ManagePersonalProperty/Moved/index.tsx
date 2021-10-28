// Moved.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState, useEffect } from 'react';
import {
  CustomTextButton,
  CustomDatePicker,
  CustomTextField,
  SimpleDropDown,
  CustomSearchTextField,
  DropDownItem,
  ContactInfoAlert,
  ListItem,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import useAddress from 'hooks/useAddress';
import useManageData from '../useManageData';
import { useMount, useUpdateEffect } from 'react-use';
import { QuickCollect } from 'models/quickCollect';
import { contactApiService } from 'services/api/apiService/portalContact';
import { AppContext } from 'contexts/AppContext';
import { PortalAddress } from 'models/portalContact';
import { formatDate } from 'utils/date';
import * as generalFm from '../../../../../GeneralFormatMessage';
import OtherAddress from './OtherAddress';

function Moved(): JSX.Element {
  const classes = useStyles();
  const { quickCollect, setQuickCollect } = useManageData('MOVED');
  const {
    // update fields ---------
    updateInputsData,
    // address ----------
    addressSuggestionList,
    addressInputValue,
    addressDataSelected,
    addressIsValid,
    addressInputHasError,
    addressInputBlurHandler,
    handleAddressSuggestionSel,
    handleAddressSuggestions,
    loadingAddressSug,
    // state -------------
    stateSuggestionList,
    stateInputValue,
    stateDataSelected,
    stateIsValid,
    stateInputHasError,
    stateInputBlurHandler,
    handleStateSuggestionSel,
    handleStateSuggestions,
    loadingStateSug,
    // city ----------------
    citySuggestionList,
    cityInputValue,
    cityDataSelected,
    cityIsValid,
    cityInputHasError,
    cityInputBlurHandler,
    handleCitySuggestionSel,
    handleCitySuggestions,
    loadingCitySug,
    // zip -------------
    zipCodeSuggestionList,
    zipInputValue,
    zipDataSelected,
    zipIsValid,
    zipInputHasError,
    zipInputBlurHandler,
    handleZipSuggestionSel,
    handleZipCodeSuggestions,
    loadingZipSug,
  } = useAddress({
    address: {
      value: quickCollect?.businessAddress1 ?? '',
    },
    state: quickCollect?.businessStateId
      ? {
          value: quickCollect.businessStateId,
          valueType: 'ID',
        }
      : undefined,
    city: quickCollect?.businessCityLabel
      ? {
          value: quickCollect.businessCityLabel,
          valueType: 'LABEL',
        }
      : undefined,
    zip: quickCollect?.businessZipLabel
      ? {
          value: quickCollect.businessZipLabel,
          valueType: 'LABEL',
        }
      : undefined,
  });
  const { portalContact } = useContext(AppContext);
  const { countryOptions, currentBusiness, setFormIsInvalid } = useContext(
    ChangedBusinessContext
  );
  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [
    manageAddressAnchor,
    setManageAddressAnchor,
  ] = useState<HTMLElement | null>(null);
  const [addressList, setAddressList] = useState<PortalAddress[]>([]);

  useMount(() => {
    if (!quickCollect) {
      if (!currentBusiness?.id) return;
      const newQuickCollect = new QuickCollect();
      newQuickCollect.personalPropertyId = currentBusiness.id;
      setQuickCollect(newQuickCollect);
    }
  });

  const fetchAddressList = async (): Promise<void> => {
    if (!portalContact) return;
    const addressesRes = await contactApiService.getContactAddresses(
      portalContact.id
    );
    if (addressesRes.data && addressesRes.data.length) {
      setAddressList(addressesRes.data);
    } else {
      console.error('Error on load address list:', addressesRes.errorMessage);
    }
  };

  useUpdateEffect(() => {
    setQuickCollect(prev => {
      return {
        ...prev,
        businessAddress1: addressDataSelected,
        businessStateId: stateDataSelected?.ptas_stateorprovinceid ?? '',
        businessStateLabel: stateDataSelected?.ptas_name ?? '',
        businessCityId: cityDataSelected?.ptas_cityid ?? '',
        businessCityLabel: cityDataSelected?.ptas_name ?? '',
        businessZipId: zipDataSelected?.ptas_zipcodeid ?? '',
        businessZipLabel: zipDataSelected?.ptas_name ?? '',
      };
    });
  }, [
    addressDataSelected,
    stateDataSelected,
    cityDataSelected,
    zipDataSelected,
  ]);

  useEffect(() => {
    const allFieldsIsOk =
      addressIsValid && cityIsValid && stateIsValid && zipIsValid;
    setFormIsInvalid(!allFieldsIsOk);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [addressIsValid, cityIsValid, stateIsValid, zipIsValid]);

  useEffect(() => {
    portalContact && fetchAddressList();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

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

  // TODO: (LF) The country field is not yet available
  const handleCountrySelect = (item: DropDownItem): void => {
    console.log('Select country', item);
  };

  const onChangeDate = (date: Date | null): void => {
    if (!date) return;
    const dateFormatted = formatDate(date);
    setQuickCollect(prev => {
      return {
        ...prev,
        date: dateFormatted,
      };
    });
  };

  const handleSelectInfoAddress = async (item: ListItem): Promise<void> => {
    const itemFound = addressList.find(i => i.id === item.value);
    setAddressAnchor(null);

    if (!itemFound) return;

    // line 2 address is not included in useAddress hook
    setQuickCollect(prev => {
      return {
        ...prev,
        businessAddress2: itemFound.line2,
      };
    });

    // update inputs values managed by useAddress hook
    updateInputsData({
      address: {
        value: itemFound.line1,
      },
      state: {
        value: itemFound.stateId,
        valueType: 'ID',
      },
      city: {
        value: itemFound.cityId,
        valueType: 'ID',
      },
      zip: {
        value: itemFound.zipCodeId,
        valueType: 'ID',
      },
    });
  };

  const closeContactInfoModal = (): void => setManageAddressAnchor(null);

  const openManageAddressPopup = (): void => {
    setManageAddressAnchor(document.body);
  };

  const closeAddressListPopup = (): void => {
    setAddressAnchor(null);
  };

  return (
    <div className={classes.movedRoot}>
      <CustomDatePicker
        value={quickCollect?.date}
        onChange={onChangeDate}
        label={fm.moveDate}
        classes={{ root: classes.newDate }}
        placeholder="mm/dd/yyyy"
      />
      <CustomTextButton
        ptasVariant="Text more"
        classes={{ root: classes.useOtherAddress }}
        onClick={(evt: React.MouseEvent<HTMLButtonElement, MouseEvent>): void =>
          setAddressAnchor(evt.currentTarget)
        }
      >
        {fm.useOtherAddress}
      </CustomTextButton>

      {/* Add variant on AddressInfoFields to show the Country field at the bottom, 
      and use here instead of the following controls */}
      {/* <AddressInfoFields address={{}} /> */}

      <CustomSearchTextField
        ptasVariant="squared outline"
        label={fm.newAddress}
        onChangeDelay={800}
        hideSearchIcon={true}
        value={addressInputValue}
        onChange={handleAddressSuggestions}
        onBlur={addressInputBlurHandler}
        suggestion={{
          List: addressSuggestionList,
          onSelected: handleAddressSuggestionSel,
          loading: loadingAddressSug,
        }}
        error={addressInputHasError}
        helperText={addressInputHasError ? generalFm.fieldRequired : ''}
        classes={{
          wrapper: classes.addressInputWrap,
        }}
      />

      <CustomTextField
        name="businessAddress2"
        value={quickCollect?.businessAddress2 ?? ''}
        onChange={onChangeCustomTextField}
        ptasVariant="outline"
        label={fm.addressAccount}
        classes={{ root: classes.inputs }}
      />
      <div className={classes.inputWrap}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.city}
          onChangeDelay={800}
          hideSearchIcon={true}
          value={cityInputValue}
          onChange={handleCitySuggestions}
          onBlur={cityInputBlurHandler}
          suggestion={{
            List: citySuggestionList,
            onSelected: handleCitySuggestionSel,
            loading: loadingCitySug,
          }}
          error={cityInputHasError}
          helperText={cityInputHasError ? generalFm.fieldRequired : ''}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.state}
          onChangeDelay={800}
          hideSearchIcon={true}
          value={stateInputValue}
          onChange={handleStateSuggestions}
          onBlur={stateInputBlurHandler}
          suggestion={{
            List: stateSuggestionList,
            onSelected: handleStateSuggestionSel,
            loading: loadingStateSug,
          }}
          error={stateInputHasError}
          helperText={stateInputHasError ? generalFm.fieldRequired : ''}
          classes={{
            wrapper: classes.addressStateInputWrap,
          }}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.zip}
          onChangeDelay={800}
          hideSearchIcon={true}
          value={zipInputValue}
          onChange={handleZipCodeSuggestions}
          onBlur={zipInputBlurHandler}
          suggestion={{
            List: zipCodeSuggestionList,
            onSelected: handleZipSuggestionSel,
            loading: loadingZipSug,
          }}
          error={zipInputHasError}
          helperText={zipInputHasError ? generalFm.fieldRequired : ''}
        />
      </div>
      <SimpleDropDown
        items={countryOptions}
        label={fm.country}
        defaultValue="one"
        classes={{
          textFieldRoot: classes.countryDropdown,
          root: classes.rootDropdown,
        }}
        onSelected={handleCountrySelect}
      />

      {addressAnchor && (
        <ContactInfoAlert
          anchorEl={addressAnchor}
          items={addressList.map(i => ({
            value: i.id,
            label: i.title,
          }))}
          onItemClick={handleSelectInfoAddress}
          buttonText={fm.manageAddresses}
          onButtonClick={openManageAddressPopup}
          onClose={closeAddressListPopup}
        />
      )}
      <OtherAddress
        anchorEl={manageAddressAnchor}
        onClose={closeContactInfoModal}
        addressList={addressList}
        setAddressList={setAddressList}
      />
    </div>
  );
}

export default Moved;
