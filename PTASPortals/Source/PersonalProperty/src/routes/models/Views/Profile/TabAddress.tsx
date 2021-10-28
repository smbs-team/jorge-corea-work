// TabAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import {
  ContactInfoAlert,
  ListItem,
  DropDownItem,
  ItemSuggestion,
  SnackContext,
  ErrorMessageContext,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import ContactInfoModal from './ContactInfo/ContactInfoModal';
import AddressInfoFields from './ContactInfo/AddressInfoFields';
import { contactApiService } from 'services/api/apiService/portalContact';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';
import { apiService } from 'services/api/apiService';
import { AppContext } from 'contexts/AppContext';
import { v4 as uuid } from 'uuid';
import { PortalAddress } from 'models/portalContact';
import ContactInfoView from './ContactInfo/ContactInfoView';
import useAddress from 'hooks/useAddress';
import { ProfileStep } from './types';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 270,
    padding: theme.spacing(4, 0, 4, 0),
  },
}));

interface Props {
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabAddress(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { portalContact, setPortalContact } = useContext(AppContext);
  const { setSnackState } = useContext(SnackContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const {
    editingContact,
    setEditingContact,
    addressList,
    setAddressList,
    countryItems,
  } = useProfile();
  const {
    addrSuggestions,
    handleAddressSuggestions,
    // addressSuggestionList,
    stateSuggestionList,
    handleStateSuggestions,
    citySuggestionList,
    handleCitySuggestions,
    zipCodeSuggestionList,
    handleZipCodeSuggestions,
    // getInfoFromAddressSuggestionSelected,
    // loadingCitySug,
    // loadingStateSug,
    // loadingZipSug,
  } = useAddress();

  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [manageAddressAnchor, setManageAddressAnchor] =
    useState<HTMLElement | null>(null);

  const onTitleChange = (value: string): void => {
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              title: value,
            }
          : undefined,
      };
    });
  };

  const onLine1Change = async (value: string): Promise<void> => {
    //TODO: use getInfoFromAddressSuggestionSelected from
    // hook useAddress, once it includes the zip code
    const suggestion = addrSuggestions.find((el) => el.formattedaddr === value);
    if (!suggestion) return;
    const addressData = await getAddressData(suggestion);

    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              line1: suggestion.streetname ?? '',
              countryId: addressData?.countryId ?? '',
              stateId: addressData?.stateId ?? '',
              state: addressData?.state ?? '',
              cityId: addressData?.cityId ?? '',
              city: addressData?.city ?? '',
              zipCodeId: addressData?.zipCodeId ?? '',
              zipCode: addressData?.zipCode ?? '',
            }
          : undefined,
      };
    });
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

  const onLine2Change = (value: string): void => {
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              line2: value,
            }
          : undefined,
      };
    });
  };

  const onCountryChange = (item: DropDownItem): void => {
    if (!editingContact?.address) return;
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              countryId: item.value as string,
              // When country changes, clear state/city/zip
              stateId: '',
              state: '',
              cityId: '',
              city: '',
              zipCodeId: '',
              zipCode: '',
            }
          : undefined,
      };
    });
  };

  const onCityChange = (item: ItemSuggestion): void => {
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              cityId: (item.id as string) ?? '',
              city: item.title,
            }
          : undefined,
      };
    });
  };

  const onStateChange = (item: ItemSuggestion): void => {
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              stateId: (item.id as string) ?? '',
              state: item.title,
            }
          : undefined,
      };
    });
  };

  const onZipCodeChange = (item: ItemSuggestion): void => {
    setEditingContact((prev) => {
      if (!prev) return;
      return {
        ...prev,
        address: prev.address
          ? {
              ...prev.address,
              zipCodeId: (item.id as string) ?? '',
              zipCode: item.title,
            }
          : undefined,
      };
    });
  };

  const onStateSearchChange = async (
    value: string,
    countryId: string
  ): Promise<void> => {
    if (!countryId) {
      setSnackState({
        severity: 'info',
        text: 'Please select a country first.', //TODO: move to fm
      });
      return;
    }

    handleStateSuggestions(value);
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
    const address = addressList.find((i) => i.id === addressId);
    if (!address?.isSaved) {
      setAddressList((prev) => {
        return prev.filter((i) => i.id !== addressId);
      });
      return;
    }

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
        errorHead: fm.deleteAddressError,
      });
      return;
    } else {
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
          errorHead: fm.saveAddressError,
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

  return (
    <Box className={classes.root}>
      {editingContact?.address?.isSaved && (
        <ContactInfoView
          variant="address"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setAddressAnchor(evt.currentTarget)}
          addressTitle={editingContact.address.title}
          addressLine1={editingContact.address.line1}
          addressLine2={`${editingContact.address.city}, ${editingContact.address.state}`}
          useOtherAddressText={fm.otherAddress}
        />
      )}
      {!editingContact?.address?.isSaved && (
        <AddressInfoFields
          address={editingContact?.address}
          addressSuggestions={addrSuggestions}
          countryItems={countryItems}
          stateSuggestions={stateSuggestionList}
          citySuggestions={citySuggestionList}
          zipCodeSuggestions={zipCodeSuggestionList}
          onTitleChange={onTitleChange}
          onLine1Change={onLine1Change}
          onLine2Change={onLine2Change}
          onCountryChange={onCountryChange}
          onCityChange={onCityChange}
          onStateChange={onStateChange}
          onZipCodeChange={onZipCodeChange}
          onAddressSearchChange={handleAddressSuggestions}
          onStateSearchChange={onStateSearchChange}
          onCitySearchChange={handleCitySuggestions}
          onZipCodeSearchChange={handleZipCodeSuggestions}
          updateFormIsValid={updateFormIsValid}
          addressTexts={{
            titleLabel: fm.addressTitle,
            addressLine1Label: fm.addressLine1,
            addressLine2Label: fm.addressLine2,
            countryLabel: fm.addressCountry,
            cityLabel: fm.addressCity,
            stateLabel: fm.addressState,
            zipLabel: fm.addressZip,
            removeButtonText: fm.modalRemoveButtonAddress,
            removeAlertText: fm.modalRemoveAlertAddress,
            removeAlertButtonText: fm.modalRemoveAlertButtonAddress,
          }}
        />
      )}

      {addressAnchor && (
        <ContactInfoAlert
          anchorEl={addressAnchor}
          items={addressList.map((i) => ({
            value: i.id,
            label: i.title,
          }))}
          onItemClick={(item: ListItem): void =>
            console.log('Selected item:', item)
          }
          buttonText={fm.manageAddresses}
          onButtonClick={(): void => {
            setManageAddressAnchor(document.body);
          }}
          onClose={(): void => {
            setAddressAnchor(null);
          }}
        />
      )}

      {manageAddressAnchor && (
        <ContactInfoModal
          variant={'address'}
          anchorEl={manageAddressAnchor}
          onClose={(): void => setManageAddressAnchor(null)}
          onClickNewItem={onAddEmptyAddress}
          onRemoveItem={onRemoveAddress}
          onChangeItem={onChangeAddress}
          addressList={addressList}
          addressSuggestions={addrSuggestions}
          countryItems={countryItems}
          stateSuggestions={stateSuggestionList}
          citySuggestions={citySuggestionList}
          zipCodeSuggestions={zipCodeSuggestionList}
          onAddressSearchChange={handleAddressSuggestions}
          onStateSearchChange={onStateSearchChange}
          onCitySearchChange={handleCitySuggestions}
          onZipCodeSearchChange={handleZipCodeSuggestions}
          getAddressData={getAddressData}
          newItemText={fm.newAddress}
          addressTexts={{
            titleLabel: fm.modalAddressTitle,
            addressLine1Label: fm.modalAddressLine1,
            addressLine2Label: fm.modalAddressLine2,
            countryLabel: fm.modalAddressCountry,
            cityLabel: fm.modalAddressCity,
            stateLabel: fm.modalAddressState,
            zipLabel: fm.modalAddressZip,
            removeButtonText: fm.modalRemoveButtonAddress,
            removeAlertText: fm.modalRemoveAlertAddress,
            removeAlertButtonText: fm.modalRemoveAlertButtonAddress,
          }}
        />
      )}
    </Box>
  );
}

export default TabAddress;
