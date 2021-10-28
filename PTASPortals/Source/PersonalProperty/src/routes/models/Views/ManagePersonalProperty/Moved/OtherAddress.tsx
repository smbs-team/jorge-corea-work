// UseOtherAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import * as fm from './formatText';
import * as fmProfile from 'routes/models/Views/Profile/formatText';
import {
  ErrorMessageContext,
  ItemSuggestion,
  SnackContext,
} from '@ptas/react-public-ui-library';
import ContactInfoModal from 'routes/models/Views/Profile/ContactInfo/ContactInfoModal';
import { PortalAddress } from 'models/portalContact';
import { AppContext } from 'contexts/AppContext';
import { v4 as uuidV4 } from 'uuid';
import { apiService } from 'services/api/apiService';
import { contactApiService } from 'services/api/apiService/portalContact';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';
import { addressService } from 'services/api/apiService/address';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';

interface Props {
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  addressList: PortalAddress[];
  setAddressList: React.Dispatch<React.SetStateAction<PortalAddress[]>>;
}

function OtherAddress(props: Props): JSX.Element {
  const { anchorEl, onClose, addressList, setAddressList } = props;
  const { setSnackState } = useContext(SnackContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const { portalContact, setPortalContact } = useContext(AppContext);
  const { countryOptions } = useContext(ChangedBusinessContext);
  const [addressSuggestions, setAddressSuggestions] = useState<
    AddressLookupEntity[]
  >([]);
  const [stateItems, setStateItems] = useState<ItemSuggestion[]>([]);
  const [cityItems, setCityItems] = useState<ItemSuggestion[]>([]);
  const [zipCodeItems, setZipCodeItems] = useState<ItemSuggestion[]>([]);

  const onAddEmptyAddress = (): void => {
    setAddressList(prev => {
      if (!prev || !portalContact?.id) return [];
      return [
        ...prev,
        {
          id: uuidV4(),
          isSaved: false,
          title: '',
          line1: '',
          line2: '',
          countryId: '',
          country: '',
          stateId: '',
          state: '',
          cityId: '',
          city: '',
          zipCodeId: '',
          zipCode: '',
          portalContactId: portalContact.id,
        },
      ];
    });
  };

  const onRemoveAddress = async (addressId: string): Promise<void> => {
    const deleteRes = await apiService.deleteEntity(
      'ptas_portaladdress',
      addressId
    );
    if (deleteRes.hasError) {
      console.error(
        'Error deleting portal address.',
        deleteRes.errorMessage,
        addressId
      );
      setErrorMessageState({
        open: true,
        errorHead: fmProfile.deleteAddressError,
      });
      return;
    } else {
      const address = addressList.find(i => i.id === addressId);
      if (address && addressList[0].id === addressId) {
        //Remove first address (primary)
        const newList = addressList.filter(i => i.id !== addressId);
        if (newList.length) {
          const newFirst = { ...newList[0] };
          setPortalContact(prev => {
            if (!prev) return;
            return {
              ...prev,
              address: newFirst,
            };
          });
        }
        setAddressList(newList);
      } else {
        //Remove address other than first
        setAddressList(prev => {
          if (!prev.length) return [];
          return prev.filter(i => i.id !== addressId);
        });
      }
    }
  };

  const onChangeAddress = async (address: PortalAddress): Promise<void> => {
    if (
      address.title &&
      address.countryId &&
      address.stateId &&
      address.cityId &&
      address.zipCodeId
    ) {
      //Only save if all required data is provided
      const oldAddress = addressList.find(i => i.id === address.id);
      const saveRes = await contactApiService.savePortalAddress(
        address,
        oldAddress
      );
      if (saveRes.hasError) {
        console.error(
          'Error saving portal address.',
          saveRes.errorMessage,
          address
        );
        setErrorMessageState({
          open: true,
          errorHead: fmProfile.saveAddressError,
        });
        return;
      } else {
        address.isSaved = true;
        setAddressList(prev => {
          if (!prev) return [];
          return prev.map(currentAddress =>
            currentAddress.id === address.id ? address : currentAddress
          );
        });
        if (address.id === addressList[0].id) {
          //Default/primary address
          setPortalContact(prev => {
            if (!prev) return;
            return {
              ...prev,
              address: address,
            };
          });
        }
      }
    } else {
      //If not all of the required data is provided, update address on memory only
      address.isSaved = false; //This address will be updated on memory, but not on DB
      setAddressList(prev => {
        if (!prev) return [];
        return prev.map(currentAddress =>
          currentAddress.id === address.id ? address : currentAddress
        );
      });
    }
  };

  const onAddressSearchChange = async (value: string): Promise<void> => {
    const { data } = await addressService.getAddressSuggestions(value);
    setAddressSuggestions(data ?? []);
  };

  const onStateSearchChange = async (
    value: string,
    countryId: string
  ): Promise<void> => {
    if (!value) {
      setStateItems([]);
      return;
    }
    if (!countryId) {
      setSnackState({
        severity: 'info',
        text: 'Please select a country first.', //TODO: move to fm
      });
      return;
    }

    const statesRes = await apiService.getStates(value, countryId);
    if (statesRes.data?.length) {
      const list = statesRes.data.map(item => ({
        id: item.ptas_stateorprovinceid,
        title: item.ptas_abbreviation,
        subtitle: item.ptas_name,
      }));
      setStateItems(list);
    } else {
      setStateItems([]);
    }
  };

  const onCitySearchChange = async (value: string): Promise<void> => {
    const citiesRes = await apiService.getCities(value);
    if (citiesRes.data?.length) {
      const list = citiesRes.data.map(item => ({
        id: item.ptas_cityid,
        title: item.ptas_name,
        subtitle: '',
      }));
      setCityItems(list);
    } else {
      setCityItems([]);
    }
  };

  const onZipCodeSearchChange = async (value: string): Promise<void> => {
    if (!value || value.length < 2) {
      return;
    }

    const zipCodesRes = await apiService.getZipCodes(value);
    if (zipCodesRes.data?.length) {
      const list = zipCodesRes.data.map(item => ({
        id: item.ptas_zipcodeid,
        title: item.ptas_name,
        subtitle: '',
      }));
      setZipCodeItems(list);
    } else {
      setZipCodeItems([]);
    }
  };

  const getAddressData = async (
    addressSuggestion: AddressLookupEntity
  ): Promise<BasicAddressData> => {
    let countryId;
    if (addressSuggestion.country) {
      countryId = countryOptions.find(
        el => el.label === addressSuggestion.country
      )?.value as string;
    }
    if (
      countryId &&
      (addressSuggestion.state ||
        addressSuggestion.city ||
        addressSuggestion.zip)
    ) {
      const { data } = await contactApiService.getStateCityZip(
        countryId,
        addressSuggestion.state,
        addressSuggestion.city,
        addressSuggestion.zip
      );
      return {
        country: addressSuggestion.country,
        countryId: countryId,
        state: data?.state ?? '',
        stateId: data?.stateId ?? '',
        city: data?.city ?? '',
        cityId: data?.cityId ?? '',
        zipCode: data?.zipCode ?? '',
        zipCodeId: data?.zipCodeId ?? '',
      };
    }

    return {
      country: addressSuggestion.country,
      countryId: countryId ?? '',
      state: '',
      stateId: '',
      city: '',
      cityId: '',
      zipCode: '',
      zipCodeId: '',
    };
  };

  if (!anchorEl) return <React.Fragment />;

  return (
    <ContactInfoModal
      variant={'address'}
      anchorEl={anchorEl}
      onClose={onClose}
      onClickNewItem={onAddEmptyAddress}
      onRemoveItem={onRemoveAddress}
      onChangeItem={onChangeAddress}
      addressList={addressList}
      addressSuggestions={addressSuggestions}
      countryItems={countryOptions}
      stateSuggestions={stateItems}
      citySuggestions={cityItems}
      zipCodeSuggestions={zipCodeItems}
      onAddressSearchChange={onAddressSearchChange}
      onStateSearchChange={onStateSearchChange}
      onCitySearchChange={onCitySearchChange}
      onZipCodeSearchChange={onZipCodeSearchChange}
      getAddressData={getAddressData}
      newItemText={fm.newAddress}
      addressTexts={{
        titleLabel: fmProfile.modalAddressTitle,
        addressLine1Label: fmProfile.modalAddressLine1,
        addressLine2Label: fmProfile.modalAddressLine2,
        countryLabel: fmProfile.modalAddressCountry,
        cityLabel: fmProfile.modalAddressCity,
        stateLabel: fmProfile.modalAddressState,
        zipLabel: fmProfile.modalAddressZip,
        removeButtonText: fmProfile.modalRemoveButtonAddress,
        removeAlertText: fmProfile.modalRemoveAlertAddress,
        removeAlertButtonText: fmProfile.modalRemoveAlertButtonAddress,
      }}
    />
  );
}

export default OtherAddress;
