// Contact.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import {
  CustomTextField,
  ContactInfoAlert,
  ListItem,
  PhoneType,
  SimpleDropDown,
  CustomSearchTextField,
  ItemSuggestion,
  ErrorMessageContext,
  utilService,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as fmProfile from '../Profile/formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import { Box, makeStyles, Theme } from '@material-ui/core';
import clsx from 'clsx';
import useNewBusiness from './useNewBusiness';
import useAddress from 'hooks/useAddress';
import { v4 as uuid } from 'uuid';
import { AppContext } from 'contexts/AppContext';
import { PersonalProperty } from 'models/personalProperty';
import ContactInfoView from '../Profile/ContactInfo/ContactInfoView';
import { apiService } from 'services/api/apiService';
import { contactApiService } from 'services/api/apiService/portalContact';
import { PortalAddress, PortalEmail, PortalPhone } from 'models/portalContact';
import ContactInfoModal from '../Profile/ContactInfo/ContactInfoModal';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';
import PhoneInfoFields from '../Profile/ContactInfo/PhoneInfoFields';

const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: 280,
    marginBottom: 25,
  },
  normalInput: {
    maxWidth: 270,
    marginBottom: 33,
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
  emailInput: {
    maxWidth: 270,
    marginTop: 24,
    marginBottom: 20,
  },
  phoneInput: {
    marginBottom: 34,
  },
  // address form class
  root: {
    width: '100%',
    maxWidth: '270px',
    marginTop: 8,
    marginBottom: theme.spacing(2.125),
  },
  line: {
    display: 'flex',
    justifyContent: 'space-between',
    marginBottom: theme.spacing(3.125),
    flexDirection: 'column',
    '& > div': {
      marginBottom: 10,
    },

    '& > div:last-child': {
      marginBottom: 0,
    },

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      '& > div': {
        marginBottom: 0,
      },
    },
  },
  lastLine: {
    marginBottom: theme.spacing(2.125),
  },
  addressTitle: {
    maxWidth: '198px',
  },
  countryDropdown: {
    maxWidth: '62px',
    minWidth: '62px',
  },
  countryDropdownInput: {
    height: '36px',
  },
  addressLine: {
    maxWidth: '270px',
  },
  city: {
    width: '107px',
  },
  state: {
    width: '45px',
  },
  statePadding: {
    padding: 0,

    '& > fieldset': {
      padding: 0,
    },
  },
  shrinkCity: {
    left: -6,
  },
  zip: {
    width: '101px',
  },
  addressInputWrap: {
    maxWidth: '100%',
    marginBottom: 25,
  },
  cityInputWrap: {
    maxWidth: 107,
  },
  stateInput: {
    maxWidth: 45,
  },
  zipInput: {
    maxWidth: 100,
  },
}));

function Contact(): JSX.Element {
  const classes = useStyles();

  const {
    contactInfo,
    newBusiness,
    phoneTypeList,
    countryOpts,
    addressSugLoading,
    addrContContactError,
    cityContactError,
    stateContactError,
    addrContactError,
    zipCodeContactError,
    emailAnchor,
    setManageInfoAnchor,
    setEmailAnchor,
    phoneAnchor,
    setPhoneAnchor,
    addressAnchor,
    setAddressAnchor,
    manageInfoAnchor,
    onChangeCountryContact,
    onSelectZipCodeContact,
    setAddressList,
    addressList,
    setNewBusiness,
    saveNewBusiness,
    emailList,
    phoneList,
    setEmailList,
    setPhoneList,
  } = useNewBusiness();

  const {
    handleAddressSuggestions,
    addressSuggestionList,
    zipCodeSuggestionList,
    citySuggestionList,
    stateSuggestionList,
    handleCitySuggestions,
    handleStateSuggestions,
    handleZipCodeSuggestions,
    getInfoFromAddressSuggestionSelected,
    loadingCitySug,
    loadingStateSug,
    loadingZipSug,
    addrSuggestions,
  } = useAddress();

  const { portalContact, setPortalContact } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const {
    hasError: preparerNameInputHasError,
    inputBlurHandler: preparerNameInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.preparerName ?? '',
    utilService.isNotEmpty
  );

  const {
    hasError: attentionInputHasError,
    inputBlurHandler: attentionInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.preparerAttention ?? '',
    utilService.isNotEmpty
  );

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

  const onSelectAddressSuggestion = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const addressData = await getInfoFromAddressSuggestionSelected(
      (item.id as string) ?? ''
    );

    setNewBusiness((prev) => {
      return {
        ...prev,
        preparerAddress: addressData?.address ?? '',
        preparerCountryId: addressData.country?.ptas_countryid ?? '',
        preparerCityId: addressData.city?.ptas_cityid ?? '',
        preparerCityLabel: addressData.city?.ptas_name ?? '',
      };
    });
  };

  const handleChangeLine2Contact = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const value = e.target.value;
    saveNewBusiness('preparerAddressLine2', value);
  };

  const onSelectState = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        preparerStateId: (item.id as string) ?? '',
        preparerStateLabel: item.title,
      };
    });
  };

  const onSelectCity = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        preparerCityId: (item.id as string) ?? '',
        preparerCityLabel: item.title,
      };
    });
  };

  const handleChangeInput = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const info = e.target.value;
    const name = e.target.name as keyof PersonalProperty;

    saveNewBusiness(name, info);
  };

  const handleChangeNumber = (value: string): void => {
    saveNewBusiness('preparerCellphone', value);
  };

  const mappingEmailList = emailList.map((i) => ({
    value: i.id,
    label: i.email,
  }));

  const mappingPhoneList = phoneList.map((i) => ({
    value: i.id,
    label: i.phoneNumber,
  }));

  const mappingAddressList = addressList.map((i) => ({
    value: i.id,
    label: i.title,
  }));

  const onItemEmailClick = (item: ListItem): void => {
    saveNewBusiness('preparerEmail', item.label);
  };

  const onItemClickPhone = (item: ListItem): void => {
    saveNewBusiness('preparerCellphone', item.label);
  };

  const handleSelectInfoAddress = async (item: ListItem): Promise<void> => {
    const itemFound = addressList.find((i) => i.id === item.value);

    if (!itemFound) return;

    setNewBusiness((prev) => {
      return {
        ...prev,
        preparerCityLabel: itemFound.city,
        preparerZipcodeLabel: itemFound.zipCode,
        preparerStateLabel: itemFound.state,
        preparerCityId: itemFound.cityId,
        preparerZipCodeId: itemFound.zipCodeId,
        preparerStateId: itemFound.stateId,
        preparerAddress: itemFound.line1,
        preparerBusinessTitle: itemFound.title,
      };
    });
  };

  const onRemoveEmail = async (emailId: string): Promise<void> => {
    const { hasError } = await apiService.deleteEntity(
      'ptas_portalemail',
      emailId
    );

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorHead: fmProfile.deleteEmailError,
      });
    }

    const email = emailList.find((i) => i.id === emailId);

    if (email && email.primaryEmail) {
      //Filter out removed email and set the first one as primary
      const newList = emailList.filter((i) => i.id !== emailId);
      const old = { ...newList[0] };
      const newPrimary = newList[0];
      newPrimary.primaryEmail = true;
      await contactApiService.savePortalEmail(newPrimary, old);
      setEmailList(newList);
      return setPortalContact((prev) => {
        if (!prev) return;
        return {
          ...prev,
          email: newPrimary,
        };
      });
    }

    //Filter out removed email
    setEmailList((prev) => {
      if (!prev.length) return [];
      return prev.filter((i) => i.id !== emailId);
    });
  };

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
    if (!utilService.validEmail(email.email)) return;

    const oldEmail = emailList.find((i) => i.id === email.id);

    const { hasError } = await contactApiService.savePortalEmail(
      email,
      oldEmail
    );

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorHead: fmProfile.saveEmailError,
      });
    }

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

  const getAddressData = async (
    addressSuggestion: AddressLookupEntity
  ): Promise<BasicAddressData> => {
    let countryId;

    if (addressSuggestion.country) {
      countryId = countryOpts.find(
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

  const onAddEmptyPhone = (): void => {
    setPhoneList((prev) => {
      if (!prev || !portalContact?.id) return [];
      return [
        ...prev,
        {
          id: uuid(),
          isSaved: false,
          phoneNumber: '',
          phoneTypeValue: 0,
          acceptsTextMessages: false,
          portalContactId: portalContact.id,
        },
      ];
    });
  };

  const onRemovePhone = async (phoneId: string): Promise<void> => {
    const { hasError } = await apiService.deleteEntity(
      'ptas_phonenumber',
      phoneId
    );

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorHead: fmProfile.deletePhoneError,
      });
    }

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

  const EmailAddressPopup = (
    <ContactInfoAlert
      anchorEl={emailAnchor}
      items={mappingEmailList}
      onItemClick={onItemEmailClick}
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
      items={mappingPhoneList}
      onItemClick={onItemClickPhone}
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
      items={mappingAddressList}
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
      onClickNewEmail={onAddEmptyEmail}
      onRemoveItem={onRemoveEmail}
      onClose={(): void => setManageInfoAnchor(null)}
      onChangeItem={onChangeEmail}
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
      variant="phone"
      anchorEl={manageInfoAnchor}
      onClickNewItem={onAddEmptyPhone}
      onClose={(): void => setManageInfoAnchor(null)}
      onPhoneTypeSelect={(id: string | number, phoneType: PhoneType): void =>
        console.log('Selected phone type:', phoneType, 'for id:', id)
      }
      onRemoveItem={onRemovePhone}
      phoneList={phoneList}
      newItemText={fmProfile.newPhone}
      onChangeItem={onChangePhone}
      phoneTypeList={phoneTypeList}
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
      variant="address"
      anchorEl={manageInfoAnchor}
      onClose={(): void => setManageInfoAnchor(null)}
      onRemoveItem={onRemoveAddress}
      onClickNewItem={onAddEmptyAddress}
      onChangeItem={onChangeAddress}
      addressList={addressList}
      countryItems={countryOpts}
      addressSuggestions={addrSuggestions}
      stateSuggestions={stateSuggestionList}
      citySuggestions={citySuggestionList}
      zipCodeSuggestions={zipCodeSuggestionList}
      onAddressSearchChange={handleAddressSuggestions}
      onStateSearchChange={handleStateSuggestions}
      onCitySearchChange={handleCitySuggestions}
      onZipCodeSearchChange={handleZipCodeSuggestions}
      newItemText={fmProfile.newAddress}
      getAddressData={getAddressData}
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
    if (contactInfo?.email?.isSaved) {
      return (
        <ContactInfoView
          variant="email"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setEmailAnchor(evt.currentTarget)}
          email={newBusiness.preparerEmail || contactInfo.email.email}
          useOtherEmailText={fmProfile.otherEmail}
        />
      );
    }

    return (
      <CustomTextField
        classes={{ root: classes.emailInput }}
        ptasVariant="email"
        type="email"
        label="Email"
        onChange={handleChangeInput}
        value={newBusiness.preparerEmail}
        name="preparerEmail"
        onChangeDelay={500}
      />
    );
  };

  const renderPhoneForm = (): JSX.Element => {
    if (contactInfo?.phone?.isSaved) {
      return (
        <ContactInfoView
          variant="phone"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setPhoneAnchor(evt.currentTarget)}
          phoneNumber={
            newBusiness.preparerCellphone || contactInfo.phone.phoneNumber
          }
          acceptsTextMessages={contactInfo.phone.acceptsTextMessages}
          useOtherPhoneText={fmProfile.otherPhone}
          phoneAcceptsMessagesText={fmProfile.phoneAcceptsMessages}
          phoneAcceptsNoMessagesText={fmProfile.phoneAcceptsNoMessages}
        />
      );
    }

    return (
      <PhoneInfoFields
        phone={{
          phoneNumber: newBusiness.preparerCellphone,
        }}
        phoneTypeList={phoneTypeList}
        onPhoneNumberChange={handleChangeNumber}
        classes={{
          root: classes.phoneInput,
        }}
      />
    );
  };

  const renderAddressForm = (): JSX.Element => {
    if (contactInfo?.address?.isSaved) {
      return (
        <ContactInfoView
          variant="address"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setAddressAnchor(evt.currentTarget)}
          addressTitle={
            newBusiness.preparerBusinessTitle || contactInfo.address.title
          }
          addressLine1={
            newBusiness.preparerAddress || contactInfo.address.line1
          }
          addressLine2={
            `${newBusiness.preparerCityLabel}, ${newBusiness.preparerStateLabel} ${newBusiness.preparerZipcodeLabel}` ||
            `${contactInfo.address.city}, ${contactInfo.address.state}`
          }
          useOtherAddressText={fmProfile.otherAddress}
        />
      );
    }

    return (
      <Box className={classes.root}>
        <Box className={classes.line}>
          <CustomTextField
            classes={{ root: classes.addressTitle }}
            ptasVariant="outline"
            label={'Address title'}
          />
          <SimpleDropDown
            items={countryOpts}
            label={'Country'}
            classes={{
              textFieldRoot: classes.countryDropdown,
              inputRoot: classes.countryDropdownInput,
            }}
            onSelected={onChangeCountryContact}
            value={newBusiness.preparerCountryId}
          />
        </Box>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="Address"
          onChangeDelay={500}
          hideSearchIcon={true}
          value={newBusiness.preparerAddress}
          onChange={handleAddressSuggestions}
          suggestion={{
            List: addressSuggestionList,
            onSelected: onSelectAddressSuggestion,
            loading: addressSugLoading,
          }}
          error={addrContactError}
          helperText={addrContactError ? generalFm.fieldRequired : ''}
          classes={{
            wrapper: classes.addressInputWrap,
          }}
        />
        <Box className={classes.line}>
          <CustomTextField
            classes={{ root: classes.addressLine }}
            ptasVariant="outline"
            label={'Address (cont.)'}
            value={newBusiness.preparerAddressLine2}
            name="addressCont"
            onChange={handleChangeLine2Contact}
            onChangeDelay={500}
            error={addrContContactError}
            helperText={addrContContactError ? generalFm.fieldRequired : ''}
          />
        </Box>
        <Box className={clsx(classes.line, classes.lastLine)}>
          <CustomSearchTextField
            ptasVariant="squared outline"
            label="City"
            onChangeDelay={500}
            hideSearchIcon={true}
            value={newBusiness.preparerCityLabel}
            onChange={handleCitySuggestions}
            suggestion={{
              List: citySuggestionList,
              onSelected: onSelectCity,
              loading: loadingCitySug,
            }}
            error={cityContactError}
            helperText={cityContactError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.cityInputWrap,
            }}
          />
          <CustomSearchTextField
            ptasVariant="squared outline"
            label="State"
            onChangeDelay={500}
            hideSearchIcon={true}
            value={newBusiness.preparerStateLabel}
            onChange={handleStateSuggestions}
            suggestion={{
              List: stateSuggestionList,
              onSelected: onSelectState,
              loading: loadingStateSug,
            }}
            error={stateContactError}
            helperText={stateContactError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.stateInput,
            }}
          />
          <CustomSearchTextField
            ptasVariant="squared outline"
            onChangeDelay={500}
            hideSearchIcon={true}
            label="Zip"
            value={newBusiness.preparerZipcodeLabel}
            onChange={handleZipCodeSuggestions}
            error={zipCodeContactError}
            helperText={zipCodeContactError ? generalFm.fieldRequired : ''}
            suggestion={{
              List: zipCodeSuggestionList,
              onSelected: onSelectZipCodeContact,
              loading: loadingZipSug,
            }}
            classes={{
              wrapper: classes.zipInput,
            }}
          />
        </Box>
      </Box>
    );
  };

  return (
    <div className={classes.contentWrap}>
      <CustomTextField
        ptasVariant="outline"
        label={fm.ownerTaxpayer}
        classes={{ root: classes.normalInput }}
        onChangeDelay={500}
        onChange={handleChangeInput}
        value={newBusiness.preparerName}
        name="preparerName"
        onBlur={preparerNameInputBlurHandler}
        error={preparerNameInputHasError}
        helperText={preparerNameInputHasError ? 'required' : ''}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.contactName}
        onChangeDelay={500}
        classes={{ root: classes.normalInput }}
      />
      <CustomTextField
        ptasVariant="outline"
        label={fm.attention}
        classes={{ root: classes.normalInput }}
        onChange={handleChangeInput}
        onChangeDelay={500}
        value={newBusiness.preparerAttention}
        name="preparerAttention"
        onBlur={attentionInputBlurHandler}
        error={attentionInputHasError}
        helperText={attentionInputHasError ? 'required' : ''}
      />
      <span className={classes.border}></span>
      {renderEmailForm()}
      <span className={classes.border}></span>

      {renderPhoneForm()}

      <span className={classes.border}></span>

      {renderAddressForm()}

      {renderInfoPopup()}
      {renderManageInfo()}
    </div>
  );
}

export default Contact;
