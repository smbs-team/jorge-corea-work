// Contact.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useContext, Fragment } from 'react';
import {
  CustomTextField,
  ContactInfoAlert,
  ListItem,
  PhoneType,
  ErrorMessageContext,
  utilService,
  SnackContext,
  DropDownItem,
  ItemSuggestion,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { makeStyles, Theme } from '@material-ui/core';
import * as fmProfile from '../Profile/formatText';
import useUpdateBusiness from './useUpdateBusiness';
import ContactInfoModal from '../Profile/ContactInfo/ContactInfoModal';
import { AppContext } from 'contexts/AppContext';
import { contactApiService } from 'services/api/apiService/portalContact';
import ContactInfoView from '../Profile/ContactInfo/ContactInfoView';
import PhoneInfoFields from '../Profile/ContactInfo/PhoneInfoFields';
import AddressInfoFields from '../Profile/ContactInfo/AddressInfoFields';

import { v4 as uuid } from 'uuid';
import { PortalEmail, PortalPhone } from 'models/portalContact';
import { apiService } from 'services/api/apiService';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';
import { addressService } from 'services/api/apiService/address';

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
  useOtherInfo: {
    marginLeft: 'auto',
    display: 'flex',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 8,
    marginTop: 15,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 320,
  },
  description: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    display: 'block',
    marginBottom: 4,
  },
  moreDesc: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: 14,
    color: 'rgba(0, 0, 0, 0.54)',
  },
  bold: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
}));

function Contact(): JSX.Element {
  const classes = useStyles();
  const {
    saveUpdateBusiness,
    updatedBusiness,
    emailList,
    setEmailList,
    editingContact,
    phoneList,
    setPhoneList,
    phoneTypes,
    phoneTypeList,
    setEditingContact,
    addressSuggestions,
    stateItems,
    cityItems,
    zipCodeItems,
    setAddressSuggestions,
    setStateItems,
    setCityItems,
    setZipCodeItems,
    setUpdatedBusiness,
    addressList,
    countryItems,
  } = useUpdateBusiness();

  const {
    hasError: preparerNameInputHasError,
    inputBlurHandler: preparerNameInputBlurHandler,
  } = useTextFieldValidation(
    updatedBusiness.preparerName ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: attentionInputHasError,
    inputBlurHandler: attentionInputBlurHandler,
  } = useTextFieldValidation(
    updatedBusiness.preparerAttention ?? '',
    utilService.isNotEmpty
  );

  const { portalContact, setPortalContact } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const { setSnackState } = useContext(SnackContext);

  const [emailAnchor, setEmailAnchor] = useState<HTMLElement | null>(null);
  const [manageInfoAnchor, setManageInfoAnchor] =
    useState<HTMLElement | null>(null);
  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [phoneAnchor, setPhoneAnchor] = useState<HTMLElement | null>(null);

  const handleChangeTaxpayerName = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const info = e.target.value;

    saveUpdateBusiness('preparerName', info);
  };

  const handleChangeTaxpayerAttention = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const info = e.target.value;

    saveUpdateBusiness('preparerAttention', info);
  };

  //#region email info functions
  const onAddEmptyEmail = (): void => {
    setEmailList((prev) => {
      if (!prev || !portalContact?.id) return [];
      return [
        ...prev,
        {
          id: uuid(),
          isSaved: false,
          email: '',
          primaryEmail: false,
          portalContactId: portalContact.id,
        },
      ];
    });
  };

  const onChangeEmail = async (email: PortalEmail): Promise<void> => {
    if (utilService.validEmail(email.email)) {
      const oldEmail = emailList.find((i) => i.id === email.id);
      const saveRes = await contactApiService.savePortalEmail(email, oldEmail);
      if (saveRes.hasError) {
        console.error(
          'Error saving portal email.',
          saveRes.errorMessage,
          email
        );
        setErrorMessageState({
          open: true,
          errorHead: fmProfile.saveEmailError,
        });
        return;
      } else {
        setEmailList((prev) => {
          if (!prev) return [];
          return prev.map((currentEmail) =>
            currentEmail.id === email.id
              ? { ...currentEmail, isSaved: true, email: email.email }
              : currentEmail
          );
        });
        if (email.primaryEmail) {
          setPortalContact((prev) => {
            if (!prev) return;
            return {
              ...prev,
              email: email,
            };
          });
        }
      }
    }
  };

  const onRemoveEmail = async (emailId: string): Promise<void> => {
    const deleteRes = await apiService.deleteEntity(
      'ptas_portalemail',
      emailId
    );
    if (deleteRes.hasError) {
      console.error(
        'Error deleting portal email.',
        deleteRes.errorMessage,
        emailId
      );
      setErrorMessageState({
        open: true,
        errorHead: fmProfile.deleteEmailError,
      });
      return;
    } else {
      const email = emailList.find((i) => i.id === emailId);
      if (email && email.primaryEmail) {
        //Filter out removed email and set the first one as primary
        const newList = emailList.filter((i) => i.id !== emailId);
        const old = { ...newList[0] };
        const newPrimary = newList[0];
        newPrimary.primaryEmail = true;
        await contactApiService.savePortalEmail(newPrimary, old);
        setEmailList(newList);
        setPortalContact((prev) => {
          if (!prev) return;
          return {
            ...prev,
            email: newPrimary,
          };
        });
      } else {
        //Filter out removed email
        setEmailList((prev) => {
          if (!prev.length) return [];
          return prev.filter((i) => i.id !== emailId);
        });
      }
    }
  };

  //#endregion email info functions

  // #region phone info functions
  const onRemovePhone = async (phoneId: string): Promise<void> => {
    const deleteRes = await apiService.deleteEntity(
      'ptas_phonenumber',
      phoneId
    );
    if (deleteRes.hasError) {
      console.error(
        'Error deleting portal phone.',
        deleteRes.errorMessage,
        phoneId
      );
      setErrorMessageState({
        open: true,
        errorHead: fmProfile.deletePhoneError,
      });
      return;
    } else {
      const phone = phoneList.find((i) => i.id === phoneId);
      if (phone && phoneList[0].id === phoneId) {
        //Remove first phone (primary)
        const newList = phoneList.filter((i) => i.id !== phoneId);
        if (newList.length) {
          const newFirst = { ...newList[0] };
          setPortalContact((prev) => {
            if (!prev) return;
            return {
              ...prev,
              phone: newFirst,
            };
          });
        }
        setPhoneList(newList);
      } else {
        //Remove phone other than first
        setPhoneList((prev) => {
          if (!prev.length) return [];
          return prev.filter((i) => i.id !== phoneId);
        });
      }
    }
  };

  const onAddEmptyPhone = (): void => {
    setPhoneList((prev) => {
      if (!prev || !portalContact?.id) return [];
      return [
        ...prev,
        {
          id: uuid(),
          isSaved: false,
          phoneNumber: '',
          phoneTypeValue: phoneTypes.get('Cell') ?? 0,
          acceptsTextMessages: false,
          portalContactId: portalContact.id,
        },
      ];
    });
  };

  const onChangePhone = async (phone: PortalPhone): Promise<void> => {
    if (phone.phoneNumber && phone.phoneTypeValue) {
      //Only save if all required data is provided
      const oldPhone = phoneList.find((i) => i.id === phone.id);
      const saveRes = await contactApiService.savePortalPhone(phone, oldPhone);
      if (saveRes.hasError) {
        console.error(
          'Error saving portal phone.',
          saveRes.errorMessage,
          phone
        );
        setErrorMessageState({
          open: true,
          errorHead: fmProfile.savePhoneError,
        });
        return;
      } else {
        phone.isSaved = true;
        setPhoneList((prev) => {
          if (!prev) return [];
          return prev.map((currentPhone) =>
            currentPhone.id === phone.id ? phone : currentPhone
          );
        });
        if (phone.id === phoneList[0].id) {
          //Default/primary phone
          setPortalContact((prev) => {
            if (!prev) return;
            return {
              ...prev,
              phone: phone,
            };
          });
        }
      }
    } else {
      //If not all of the required data is provided, update phone on memory only
      setPhoneList((prev) => {
        if (!prev) return [];
        return prev.map((currentPhone) =>
          currentPhone.id === phone.id ? phone : currentPhone
        );
      });
    }
  };

  const onPhoneNumberChange = (value: string): void => {
    setEditingContact((prev) => {
      if (!prev || !prev.phone) return;
      return {
        ...prev,
        phone: {
          ...prev.phone,
          phoneNumber: value,
        },
      };
    });
  };

  const onPhoneTypeChange = (phoneType: number): void => {
    setEditingContact((prev) => {
      if (!prev || !prev.phone) return;
      return {
        ...prev,
        phone: {
          ...prev.phone,
          phoneTypeValue: phoneType,
        },
      };
    });
  };

  const onAcceptsMessagesChange = (checked: boolean): void => {
    setEditingContact((prev) => {
      if (!prev || !prev.phone) return;
      return {
        ...prev,
        phone: {
          ...prev.phone,
          acceptsTextMessages: checked,
        },
      };
    });
  };

  const onSelectPhone = (item: ListItem): void => {
    saveUpdateBusiness('preparerCellphone', item.label);
  };

  // #endregion phone info functions

  // #region address info functions
  const onLine1Change = async (value: string): Promise<void> => {
    const suggestion = addressSuggestions.find(
      (el) => el.formattedaddr === value
    );
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

  const handleSelectInfoAddress = async (item: ListItem): Promise<void> => {
    const itemFound = addressList.find((i) => i.id === item.value);
    console.log(`itemFound`, itemFound);

    if (!itemFound) return;

    setUpdatedBusiness((prev) => {
      return {
        ...prev,
        preparerCityLabel: itemFound.city,
        preparerZipcodeLabel: itemFound.zipCode,
        preparerStateLabel: itemFound.state,
        preparerCityId: itemFound.cityId,
        preparerZipCodeId: itemFound.zipCodeId,
        preparerStateId: itemFound.stateId,
      };
    });
  };

  // #endregion address info functions

  const EmailAddressPopup = (
    <ContactInfoAlert
      anchorEl={emailAnchor}
      items={emailList.map((i) => ({
        value: i.id,
        label: i.email,
      }))}
      onItemClick={(item: ListItem): void => {
        saveUpdateBusiness('preparerEmail', item.label);
      }}
      buttonText={fmProfile.manageEmails}
      onButtonClick={(): void => {
        setManageInfoAnchor(document.body);
      }}
      onClose={(): void => {
        setManageInfoAnchor(null);
        setEmailAnchor(null);
      }}
    />
  );

  const ContactPhonePopup = (
    <ContactInfoAlert
      anchorEl={phoneAnchor}
      items={phoneList.map((i) => ({
        value: i.id,
        label: i.phoneNumber,
      }))}
      onItemClick={onSelectPhone}
      buttonText={fmProfile.managePhones}
      onButtonClick={(): void => {
        setManageInfoAnchor(document.body);
      }}
      onClose={(): void => {
        setManageInfoAnchor(null);
        setPhoneAnchor(null);
      }}
    />
  );

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
        setManageInfoAnchor(null);
        setAddressAnchor(null);
      }}
    />
  );

  const renderInfoPopup = (): JSX.Element | void => {
    if (emailAnchor) return EmailAddressPopup;
    if (phoneAnchor) return ContactPhonePopup;
    if (addressAnchor) return AddressPopup;
  };

  const ManageEmailModal = (
    <ContactInfoModal
      variant="email"
      anchorEl={manageInfoAnchor}
      onClickNewItem={onAddEmptyEmail}
      onChangeItem={onChangeEmail}
      onClose={(): void => setManageInfoAnchor(null)}
      onRemoveItem={onRemoveEmail}
      emailList={emailList}
      newItemText={fmProfile.newEmail}
      emailTexts={{
        label: fmProfile.modalLabelEmail,
        removeButtonText: fmProfile.modalRemoveButtonEmail,
        removeAlertText: fmProfile.modalRemoveAlertEmail,
        removeAlertButtonText: fmProfile.modalRemoveAlertButtonEmail,
      }}
    />
  );

  const ManagePhoneModal = (
    <ContactInfoModal
      variant={'phone'}
      anchorEl={manageInfoAnchor}
      onClickNewItem={onAddEmptyPhone}
      onClose={(): void => setManageInfoAnchor(null)}
      onPhoneTypeSelect={(id: string | number, phoneType: PhoneType): void =>
        console.log('Selected phone type:', phoneType, 'for id:', id)
      }
      onRemoveItem={onRemovePhone}
      onChangeItem={onChangePhone}
      phoneTypeList={phoneTypeList}
      phoneList={phoneList}
      newItemText={fmProfile.newPhone}
      phoneTexts={{
        tabCellText: fmProfile.modalPhoneTabCell,
        tabWorkText: fmProfile.modalPhoneTabWork,
        tabHomeText: fmProfile.modalPhoneTabHome,
        tabTollFreeText: fmProfile.modalPhoneTabTollFree,
        acceptMessagesText: fmProfile.modalPhoneAcceptsMessages,
        removeButtonText: fmProfile.modalRemoveButtonPhone,
        removeAlertText: fmProfile.modalRemoveAlertPhone,
        removeAlertButtonText: fmProfile.modalRemoveAlertButtonPhone,
      }}
    />
  );

  const ManageAddressModal = (
    <ContactInfoModal
      variant={'address'}
      anchorEl={manageInfoAnchor}
      onClickNewItem={(): void => console.log('onClickNewItem')}
      onClose={(): void => setManageInfoAnchor(null)}
      onRemoveItem={(id: string | number): void =>
        console.log('Remove address:', id)
      }
      addressList={addressList}
      countryItems={countryItems}
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

  const renderManageInfo = (): JSX.Element | void => {
    if (!manageInfoAnchor) return;
    if (emailAnchor) return ManageEmailModal;
    if (phoneAnchor) return ManagePhoneModal;
    if (addressAnchor) return ManageAddressModal;
  };

  const renderEmailForm = (): JSX.Element => {
    if (!editingContact?.email) return <Fragment />;

    return (
      <ContactInfoView
        variant="email"
        onButtonClick={(
          evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
        ): void => setEmailAnchor(evt.currentTarget)}
        email={updatedBusiness.preparerEmail || editingContact.email.email}
        useOtherEmailText={fmProfile.otherEmail}
      />
    );
  };

  const renderPhoneForm = (): JSX.Element => {
    if (editingContact?.phone.isSaved) {
      return (
        <ContactInfoView
          variant="phone"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setPhoneAnchor(evt.currentTarget)}
          phoneNumber={
            updatedBusiness.preparerCellphone ||
            editingContact.phone.phoneNumber
          }
          acceptsTextMessages={editingContact.phone.acceptsTextMessages}
          useOtherPhoneText={fmProfile.otherPhone}
          phoneAcceptsMessagesText={fmProfile.phoneAcceptsMessages}
          phoneAcceptsNoMessagesText={fmProfile.phoneAcceptsNoMessages}
        />
      );
    }

    return (
      <PhoneInfoFields
        phone={editingContact?.phone}
        phoneTypeList={phoneTypeList}
        onPhoneNumberChange={onPhoneNumberChange}
        onPhoneTypeSelect={onPhoneTypeChange}
        onAcceptMessagesChange={onAcceptsMessagesChange}
        phoneTexts={{
          titleText: fmProfile.modalPhoneTitle,
          placeholderText: fmProfile.modalPhonePlaceholder,
          tabCellText: fmProfile.modalPhoneTabCell,
          tabWorkText: fmProfile.modalPhoneTabWork,
          tabHomeText: fmProfile.modalPhoneTabHome,
          tabTollFreeText: fmProfile.modalPhoneTabTollFree,
          acceptMessagesText: fmProfile.modalPhoneAcceptsMessages,
          removeButtonText: fmProfile.modalRemoveButtonPhone,
          removeAlertText: fmProfile.modalRemoveAlertPhone,
          removeAlertButtonText: fmProfile.modalRemoveAlertButtonPhone,
        }}
      />
    );
  };

  const renderAddressForm = (): JSX.Element => {
    if (editingContact?.address?.isSaved) {
      return (
        <ContactInfoView
          variant="address"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setAddressAnchor(evt.currentTarget)}
          addressTitle={
            updatedBusiness.preparerAddress || editingContact.address.title
          }
          addressLine1={
            updatedBusiness.preparerAddressLine2 || editingContact.address.line1
          }
          addressLine2={
            `${updatedBusiness.preparerCityLabel}, ${updatedBusiness.preparerStateLabel} ${updatedBusiness.preparerZipcodeLabel}` ||
            `${editingContact.address.city}, ${editingContact.address.state}`
          }
          useOtherAddressText={fmProfile.otherAddress}
        />
      );
    }

    return (
      <AddressInfoFields
        address={editingContact?.address}
        addressSuggestions={addressSuggestions}
        stateSuggestions={stateItems}
        citySuggestions={cityItems}
        zipCodeSuggestions={zipCodeItems}
        onTitleChange={onTitleChange}
        onLine1Change={onLine1Change}
        onLine2Change={onLine2Change}
        onCountryChange={onCountryChange}
        onCityChange={onCityChange}
        onStateChange={onStateChange}
        onZipCodeChange={onZipCodeChange}
        onAddressSearchChange={onAddressSearchChange}
        onStateSearchChange={onStateSearchChange}
        onCitySearchChange={onCitySearchChange}
        onZipCodeSearchChange={onZipCodeSearchChange}
        addressTexts={{
          titleLabel: fm.addressTitle,
          addressLine1Label: fmProfile.addressLine1,
          addressLine2Label: fmProfile.addressLine2,
          countryLabel: fmProfile.addressCountry,
          cityLabel: fmProfile.addressCity,
          stateLabel: fmProfile.addressState,
          zipLabel: fmProfile.addressZip,
          removeButtonText: fmProfile.modalRemoveButtonAddress,
          removeAlertText: fmProfile.modalRemoveAlertAddress,
          removeAlertButtonText: fmProfile.modalRemoveAlertButtonAddress,
        }}
      />
    );
  };

  return (
    <div className={classes.contentWrap}>
      <CustomTextField
        ptasVariant="outline"
        label={fm.ownerTaxpayer}
        classes={{ root: classes.normalInput }}
        onChange={handleChangeTaxpayerName}
        onChangeDelay={500}
        value={updatedBusiness.preparerName}
        onBlur={preparerNameInputBlurHandler}
        error={preparerNameInputHasError}
        helperText={preparerNameInputHasError ? 'required' : ''}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.contactName}
        classes={{ root: classes.normalInput }}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.attention}
        classes={{ root: classes.normalInput }}
        onChange={handleChangeTaxpayerAttention}
        onChangeDelay={500}
        value={updatedBusiness.preparerAttention}
        onBlur={attentionInputBlurHandler}
        error={attentionInputHasError}
        helperText={attentionInputHasError ? 'required' : ''}
      />
      {/* Email manage */}
      {renderEmailForm()}
      <span className={classes.border}></span>
      {/* end email manage */}
      {/* phone manage */}
      {renderPhoneForm()}
      <span className={classes.border}></span>
      {/* end phone manage */}
      {/* address manage */}
      {renderAddressForm()}
      {/* end address manage */}
      {renderInfoPopup()}
      {renderManageInfo()}
    </div>
  );
}

export default Contact;
