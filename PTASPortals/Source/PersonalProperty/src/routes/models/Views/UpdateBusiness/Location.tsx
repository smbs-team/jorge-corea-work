// Location.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useContext } from 'react';
import {
  SimpleDropDown,
  ContactInfoAlert,
  ListItem,
  DropDownItem,
  ErrorMessageContext,
  SnackContext,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as fmProfile from '../Profile/formatText';
import { makeStyles, Theme } from '@material-ui/core';
import useUpdateBusiness from './useUpdateBusiness';
import ContactInfoModal from '../Profile/ContactInfo/ContactInfoModal';
import ContactInfoView from '../Profile/ContactInfo/ContactInfoView';
import { v4 as uuid } from 'uuid';
import { AppContext } from 'contexts/AppContext';
import { apiService } from 'services/api/apiService';
import { contactApiService } from 'services/api/apiService/portalContact';
import { PortalAddress } from 'models/portalContact';
import { addressService } from 'services/api/apiService/address';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';

const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: 280,
    marginBottom: 25,
  },
  normalInput: {
    maxWidth: 270,
    marginBottom: 25,
    display: 'block',
    marginTop: 40,
  },
  dropdownStateOfIncorporation: {
    width: 202,
  },
  information: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    marginBottom: 42,
  },
  address: {
    marginBottom: 4,
    display: 'block',
  },
  title: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
}));

function Location(): JSX.Element {
  const classes = useStyles();
  const {
    stateOrProvince,
    updatedBusiness,
    saveUpdateBusiness,
    editingContact,
    setAddressList,
    addressList,
    addressSuggestions,
    stateItems,
    cityItems,
    zipCodeItems,
    setAddressSuggestions,
    setStateItems,
    setCityItems,
    setZipCodeItems,
    countryItems,
    setUpdatedBusiness,
  } = useUpdateBusiness();

  const { setSnackState } = useContext(SnackContext);

  const { portalContact, setPortalContact } = useContext(AppContext);

  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [manageInfoAnchor, setManageInfoAnchor] =
    useState<HTMLElement | null>(null);

  const handleChangeState = (item: DropDownItem): void => {
    saveUpdateBusiness('stateOfIncorporationId', item.value);
  };

  const onAddEmptyAddress = (): void => {
    setAddressList((prev) => {
      if (!prev || !portalContact?.id) return [];
      return [
        ...prev,
        {
          id: uuid(),
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
      const address = addressList.find((i) => i.id === addressId);
      if (address && addressList[0].id === addressId) {
        //Remove first address (primary)
        const newList = addressList.filter((i) => i.id !== addressId);
        if (newList.length) {
          const newFirst = { ...newList[0] };
          setPortalContact((prev) => {
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
        setAddressList((prev) => {
          if (!prev.length) return [];
          return prev.filter((i) => i.id !== addressId);
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
      const oldAddress = addressList.find((i) => i.id === address.id);
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
        setAddressList((prev) => {
          if (!prev) return [];
          return prev.map((currentAddress) =>
            currentAddress.id === address.id ? address : currentAddress
          );
        });
        if (address.id === addressList[0].id) {
          //Default/primary address
          setPortalContact((prev) => {
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
      setAddressList((prev) => {
        if (!prev) return [];
        return prev.map((currentAddress) =>
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
      const list = statesRes.data.map((item) => ({
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
      const list = citiesRes.data.map((item) => ({
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
      const list = zipCodesRes.data.map((item) => ({
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
      countryId = countryItems.find(
        (el) => el.label === addressSuggestion.country
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

  const handleSelectInfoAddress = async (item: ListItem): Promise<void> => {
    const itemFound = addressList.find((i) => i.id === item.value);
    setAddressAnchor(null);

    if (!itemFound) return;

    setUpdatedBusiness((prev) => {
      return {
        ...prev,
        addrCityLabel: itemFound.city,
        addrZipcodeLabel: itemFound.zipCode,
        addrStateLabel: itemFound.state,
        cityId: itemFound.cityId,
        zipId: itemFound.zipCodeId,
        stateId: itemFound.stateId,
      };
    });
  };

  const AddressPopup = (
    <ContactInfoAlert
      anchorEl={addressAnchor}
      items={addressList.map((i) => ({
        value: i.id,
        label: i.title,
      }))}
      onItemClick={handleSelectInfoAddress}
      buttonText={fmProfile.manageAddresses}
      onButtonClick={(): void => {
        setManageInfoAnchor(document.body);
      }}
      onClose={(): void => {
        setAddressAnchor(null);
      }}
    />
  );

  const ManageAddressModal = (
    <ContactInfoModal
      variant={'address'}//
      anchorEl={manageInfoAnchor}//
      onClose={(): void => setManageInfoAnchor(null)}//
      onClickNewItem={onAddEmptyAddress}//
      onRemoveItem={onRemoveAddress} // falta
      onChangeItem={onChangeAddress} // 
      addressList={addressList} // 
      addressSuggestions={addressSuggestions} // 
      countryItems={countryItems} // 
      stateSuggestions={stateItems} //
      citySuggestions={cityItems} //
      zipCodeSuggestions={zipCodeItems} //
      onAddressSearchChange={onAddressSearchChange} //
      onStateSearchChange={onStateSearchChange} //
      onCitySearchChange={onCitySearchChange} //
      onZipCodeSearchChange={onZipCodeSearchChange} //
      getAddressData={getAddressData}
      newItemText={fmProfile.newAddress}
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

  const AddressForm = (): JSX.Element => {
    if (!editingContact?.address?.isSaved) return <Fragment />;

    return (
      <ContactInfoView
        variant="address"
        onButtonClick={(
          evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
        ): void => setAddressAnchor(evt.currentTarget)}
        addressTitle={updatedBusiness.addressCont} // todo gl: confirm this field
        addressLine1={updatedBusiness.address}
        addressLine2={`${updatedBusiness.addrCityLabel}, ${updatedBusiness.addrStateLabel} ${updatedBusiness.addrZipcodeLabel}`}
        useOtherAddressText={fmProfile.otherAddress}
      />
    );
  };

  return (
    <div className={classes.contentWrap}>
      {AddressForm()}
      <SimpleDropDown
        items={stateOrProvince}
        value={updatedBusiness.stateOfIncorporationId}
        onSelected={handleChangeState}
        label={fm.stateOfIncorporation}
        classes={{
          root: classes.normalInput,
          textFieldRoot: classes.dropdownStateOfIncorporation,
        }}
      />
      {AddressPopup}
      {ManageAddressModal}
    </div>
  );
}

export default Location;
