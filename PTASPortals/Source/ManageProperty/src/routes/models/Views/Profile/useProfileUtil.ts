// useProfileUtil.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Dispatch,
  SetStateAction,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import {
  DropDownItem,
  ErrorMessageContext,
  ItemSuggestion,
  SnackContext,
} from '@ptas/react-public-ui-library';
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
} from 'models/portalContact';
import { contactApiService } from 'services/api/apiService/portalContact';
import { AppContext } from 'contexts/AppContext';
import * as fm from './formatText';
import { useMount } from 'react-use';
import { apiService } from 'services/api/apiService';
import { v4 as uuid } from 'uuid';
import utilService from 'services/utilService';
import { addressService } from 'services/api/apiService/address';
import { AddressLookup, BasicAddressData } from 'services/map/model/addresses';
import useAuth from 'auth/useAuth';
import {
  ProfileAction,
  ProfileFormIsValid,
  ProfileStep,
  SearchAddressSuggestions,
  SearchSuggestions,
} from './types';
import useAddressSuggestions from './useAddressSuggestions';

export interface UseProfileUtil {
  action: ProfileAction;
  editingContact: PortalContact | undefined;
  setEditingContact: Dispatch<SetStateAction<PortalContact | undefined>>;
  handleTabChange: (tab: number) => void;
  handleContinue: () => void;
  currentStep: ProfileStep;
  setCurrentStep: Dispatch<SetStateAction<ProfileStep>>;
  highestStepNumber: number;
  setHighestStepNumber: Dispatch<SetStateAction<number>>;
  formIsValid: ProfileFormIsValid;
  setFormIsValid: Dispatch<SetStateAction<ProfileFormIsValid>>;
  contactSuffixes: Map<string, number>;
  contactSuffixItems: DropDownItem[];
  addressList: PortalAddress[];
  phoneList: PortalPhone[];
  addressSuggestions: SearchAddressSuggestions;
  countryItems: DropDownItem[];
  stateSuggestions: SearchSuggestions;
  citySuggestions: SearchSuggestions;
  zipCodeSuggestions: SearchSuggestions;
  phoneTypes: Map<string, number>;
  phoneTypeList: { value: number; label: string }[];
  emailAnchor: HTMLElement | null;
  setEmailAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  addressAnchor: HTMLElement | null;
  setAddressAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  phoneAnchor: HTMLElement | null;
  setPhoneAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  manageEmailAnchor: HTMLElement | null;
  setManageEmailAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  manageAddressAnchor: HTMLElement | null;
  setManageAddressAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  managePhoneAnchor: HTMLElement | null;
  setManagePhoneAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  onTitleChange: (value: string) => void;
  onLine1Change: (value: string) => void;
  onLine2Change: (value: string) => void;
  onCountryChange: (value: DropDownItem) => void;
  onCityChange: (item: ItemSuggestion) => void;
  onStateChange: (item: ItemSuggestion) => void;
  onZipCodeChange: (item: ItemSuggestion) => void;
  onAddressSearchChange: (addressId: string, value: string) => void;
  onCitySearchChange: (addressId: string, value: string) => void;
  onStateSearchChange: (
    addressId: string,
    value: string,
    countryId: string
  ) => void;
  onZipCodeSearchChange: (addressId: string, value: string) => void;
  onPhoneNumberChange: (value: string) => void;
  onPhoneTypeChange: (phoneType: number) => void;
  onAcceptsMessagesChange: (checked: boolean) => void;
  emailList: PortalEmail[];
  onChangePrimaryEmail: (emailId: string) => void;
  onAddEmptyEmail: () => void;
  onRemoveEmail: (emailId: string) => void;
  onChangeEmail: (email: PortalEmail) => void;
  onAddEmptyAddress: () => void;
  onChangeAddress: (address: PortalAddress) => void;
  onRemoveAddress: (addressId: string) => void;
  onAddEmptyPhone: () => void;
  onChangePhone: (phone: PortalPhone) => void;
  onRemovePhone: (phoneId: string) => void;
  getAddressData: (
    addressSuggestion: AddressLookup
  ) => Promise<BasicAddressData>;
}

function useProfileUtil(): UseProfileUtil {
  const location = useLocation<{ newContact?: boolean }>();
  const [action, setAction] = useState<ProfileAction>('update');
  const history = useHistory();
  const auth = useAuth();
  const [editingContact, setEditingContact] = useState<
    PortalContact | undefined
  >();
  const [currentStep, setCurrentStep] = useState<ProfileStep>('name');
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const [formIsValid, setFormIsValid] = useState<ProfileFormIsValid>({
    step: 'name',
    valid: true,
  });
  const { setSnackState } = useContext(SnackContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const { portalContact, setPortalContact, redirectPath } = useContext(
    AppContext
  );
  const portalContactIdRef = useRef<string | undefined>(undefined);

  //Name data
  const [contactSuffixes, setContactSuffixes] = useState<Map<string, number>>(
    new Map()
  );
  const [contactSuffixItems, setContactSuffixItems] = useState<DropDownItem[]>(
    []
  );
  //Email data
  const [emailList, setEmailList] = useState<PortalEmail[]>([]);
  //Address data
  const [addressList, setAddressList] = useState<PortalAddress[]>([]);
  const [countryItems, setCountryItems] = useState<DropDownItem[]>([]);
  const {
    addressSuggestions,
    stateSuggestions,
    citySuggestions,
    zipCodeSuggestions,
    updateAddressSuggestions,
    updateCitySuggestions,
    updateStateSuggestions,
    updateZipCodeSuggestions,
  } = useAddressSuggestions();
  //Phone data
  const [phoneList, setPhoneList] = useState<PortalPhone[]>([]);
  const [phoneTypes, setPhoneTypes] = useState<Map<string, number>>(new Map());
  const [phoneTypeList, setPhoneTypeList] = useState<
    { value: number; label: string }[]
  >([]);

  //Anchors for popovers
  const [emailAnchor, setEmailAnchor] = useState<HTMLElement | null>(null);
  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [phoneAnchor, setPhoneAnchor] = useState<HTMLElement | null>(null);
  const [
    manageEmailAnchor,
    setManageEmailAnchor,
  ] = useState<HTMLElement | null>(null);
  const [
    manageAddressAnchor,
    setManageAddressAnchor,
  ] = useState<HTMLElement | null>(null);
  const [
    managePhoneAnchor,
    setManagePhoneAnchor,
  ] = useState<HTMLElement | null>(null);

  useEffect(() => {
    if (!auth?.email) {
      history.replace('/intro');
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [auth?.email]);

  useEffect(() => {
    if (location.state && location.state.newContact) {
      setAction('create');
    }
  }, [location.state]);

  //TODO: delete. TEMP for development.
  //Remove when sign-in implementation is done
  // useMount(async () => {
  //   const contactRes = await contactApiService.getTestContact();
  //   console.log('contactRes:', contactRes);
  //   if (!contactRes.data) {
  //     console.log('Test contact not found');
  //   } else {
  //     console.log('Test contact found:', contactRes.data);
  //     setPortalContact(contactRes.data);
  //   }
  // });

  useMount(() => {
    loadContactSuffixes();
    loadCountryItems();
    loadPhoneTypes();
  });

  useEffect(() => {
    if (portalContact) {
      setEditingContact(portalContact);
    } else {
      setEditingContact(new PortalContact());
    }
  }, [portalContact, setEditingContact]);

  useEffect(() => {
    //If portal contact ID changed
    if (portalContactIdRef.current !== portalContact?.id) {
      portalContactIdRef.current = portalContact?.id;
      (async (): Promise<void> => {
        //Load email list
        if (portalContact?.email) {
          const emailsRes = await contactApiService.getContactEmails(
            portalContact.id
          );
          if (emailsRes.data && emailsRes.data.length) {
            setEmailList(emailsRes.data);
          } else {
            console.error('Error on load email list:', emailsRes.errorMessage);
          }
        }

        //Load address list
        if (portalContact?.address) {
          const addressesRes = await contactApiService.getContactAddresses(
            portalContact.id
          );
          if (addressesRes.data && addressesRes.data.length) {
            setAddressList(addressesRes.data);
          } else {
            console.error(
              'Error on load address list:',
              addressesRes.errorMessage
            );
          }
        }

        //Load phone list
        if (portalContact?.phone) {
          const phonesRes = await contactApiService.getContactPhones(
            portalContact.id
          );
          if (phonesRes.data && phonesRes.data.length) {
            setPhoneList(phonesRes.data);
          } else {
            console.error('Error on load phone list:', phonesRes.errorMessage);
          }
        }
      })();
    }
  }, [portalContact]);

  const loadContactSuffixes = async (): Promise<void> => {
    const { data: suffixes } = await apiService.getOptionSet(
      'ptas_portalcontact',
      'ptas_suffix'
    );
    if (suffixes?.length) {
      const map = new Map<string, number>(
        suffixes?.map((c) => [c.value, c.attributeValue])
      );
      setContactSuffixes(map);
      setContactSuffixItems(
        suffixes.map((i) => ({ value: i.attributeValue, label: i.value }))
      );
    }
  };

  const loadCountryItems = async (): Promise<void> => {
    const countriesRes = await apiService.getCountries();
    if (countriesRes.data && countriesRes.data.length) {
      const countries = countriesRes.data.map((country) => ({
        value: country.ptas_countryid,
        label: country.ptas_name,
      }));
      //TEMP there are states and cities for USA only
      setCountryItems(
        countries.filter((c) =>
          ['United States', 'USA', 'U.S.A.'].includes(c.label)
        )
      );
    } else {
      console.error('Error on load countries:', countriesRes.errorMessage);
    }
  };

  const loadPhoneTypes = async (): Promise<void> => {
    const { data: _phoneTypes } = await apiService.getOptionSet(
      'ptas_phonenumber',
      'ptas_phonetype'
    );
    if (_phoneTypes) {
      setPhoneTypeList(
        _phoneTypes.map((el) => ({ value: el.attributeValue, label: el.value }))
      );
      const map = new Map<string, number>(
        _phoneTypes?.map((c) => [c.value, c.attributeValue])
      );
      setPhoneTypes(map);
    }
  };

  const handleTabChange = (tab: number): void => {
    //Only on tab "Name" is necessary to save data on tab change,
    //because tabs are enabled only after the user has reached them
    //by the Continue buttom, so the user can change to tabs "Email",
    //"Address" and "Phone" only when there is at least one entity
    //saved and therefore can only edit entities on the modal
    if (currentStep === 'name') {
      if (formIsValid.valid) {
        saveContactNameData();
      }
    }

    if (tab === 1) {
      setCurrentStep('email');
    } else if (tab === 2) {
      setCurrentStep('address');
    } else if (tab === 3) {
      setCurrentStep('phone');
    } else {
      setCurrentStep('name');
    }
  };

  const handleContinue = async (): Promise<void> => {
    if (currentStep === 'name') {
      setCurrentStep('email');
      if (highestStepNumber < 1) setHighestStepNumber(1);
      saveContactNameData();
    } else if (currentStep === 'email') {
      setCurrentStep('address');
      if (highestStepNumber < 2) setHighestStepNumber(2);
      saveContactEmail();
    } else if (currentStep === 'address') {
      setCurrentStep('phone');
      if (highestStepNumber < 3) setHighestStepNumber(3);
      saveContactAddress();
    } else if (currentStep === 'phone') {
      //Done
      saveContactPhone();
    }
  };

  const saveContactNameData = async (): Promise<void> => {
    if (!editingContact) return;
    const savingContact = { ...editingContact };
    const saveRes = await contactApiService.savePortalContact(
      savingContact,
      portalContact
    );
    if (saveRes.hasError) {
      console.error(
        'Error saving portal contact.',
        saveRes.errorMessage,
        savingContact
      );
      setErrorMessageState({
        open: true,
        errorHead: fm.saveContactError,
      });
      return;
    } else {
      //If it is a new contact, create email
      if (!savingContact.email) {
        const newEmail = {
          id: uuid(),
          isSaved: true,
          email: auth?.email ?? '',
          primaryEmail: true,
          portalContactId: savingContact.id,
        };
        savingContact.email = newEmail;
        await contactApiService.savePortalEmail(newEmail);
      } else {
        savingContact.email.isSaved = true;
      }

      savingContact.isSaved = true;
      setEditingContact({ ...savingContact });
      setPortalContact({ ...savingContact });
    }
  };

  const saveContactEmail = async (): Promise<void> => {
    if (!editingContact?.email) return;
    const savingEmail = { ...editingContact.email };
    const saveRes = await contactApiService.savePortalEmail(
      savingEmail,
      portalContact?.email
    );
    if (saveRes.hasError) {
      console.error(
        'Error saving portal email.',
        saveRes.errorMessage,
        savingEmail
      );
      setErrorMessageState({
        open: true,
        errorHead: fm.saveEmailError,
      });
      return;
    } else {
      setPortalContact({ ...editingContact });
    }
  };

  const saveContactAddress = async (): Promise<void> => {
    if (!editingContact?.address) return;
    const savingContact = { ...editingContact };
    if (!savingContact?.address) return;
    const savingAddress = savingContact.address;
    const saveRes = await contactApiService.savePortalAddress(
      savingAddress,
      portalContact?.address
    );
    if (saveRes.hasError) {
      console.error(
        'Error saving portal address.',
        saveRes.errorMessage,
        savingAddress
      );
      setErrorMessageState({
        open: true,
        errorHead: fm.saveAddressError,
      });
      return;
    } else {
      if (!savingAddress.isSaved) {
        savingAddress.isSaved = true;
        if (!addressList.length) {
          setAddressList([savingAddress]);
        }
      }
      setPortalContact({ ...savingContact });
    }
  };

  const saveContactPhone = async (): Promise<void> => {
    if (!editingContact?.phone) return;
    const savingContact = { ...editingContact };
    if (!savingContact?.phone) return;
    const savingPhone = savingContact.phone;
    if (!savingPhone.phoneTypeValue) {
      //If no phone type was selected, use default "Cell"
      savingPhone.phoneTypeValue = phoneTypes.get('Cell') ?? 0;
    }
    const saveRes = await contactApiService.savePortalPhone(
      savingPhone,
      portalContact?.phone
    );
    if (saveRes.hasError) {
      console.error(
        'Error saving portal phone.',
        saveRes.errorMessage,
        savingPhone
      );
      setErrorMessageState({
        open: true,
        errorHead: fm.savePhoneError,
      });
      return;
    } else {
      if (!savingPhone.isSaved) {
        savingPhone.isSaved = true;
        if (!phoneList.length) {
          setPhoneList([savingPhone]);
        }
      }
      setPortalContact({ ...savingContact });
      history.replace(redirectPath ?? '/home-manage-property');
    }
  };

  const onAddressSearchChange = async (
    addressId: string,
    value: string
  ): Promise<void> => {
    updateAddressSuggestions(addressId, undefined, true);
    const { data } = await addressService.getAddressSuggestions(value);
    updateAddressSuggestions(addressId, data ? data : [], false);
  };

  const onCitySearchChange = async (
    addressId: string,
    value: string
  ): Promise<void> => {
    updateCitySuggestions(addressId, undefined, true);
    const citiesRes = await apiService.getCities(value);
    if (citiesRes.data?.length) {
      const list = citiesRes.data.map((item) => ({
        id: item.ptas_cityid,
        title: item.ptas_name,
        subtitle: '',
      }));
      updateCitySuggestions(addressId, list, false);
    } else {
      updateCitySuggestions(addressId, [], false);
    }
  };

  const onStateSearchChange = async (
    addressId: string,
    value: string,
    countryId: string
  ): Promise<void> => {
    if (!value) {
      updateStateSuggestions(addressId, [], false);
      return;
    }
    if (!countryId) {
      setSnackState({
        severity: 'info',
        text: 'Please select a country first.', //TODO: move to fm
      });
      return;
    }

    updateStateSuggestions(addressId, undefined, true);
    const statesRes = await apiService.getStates(value, countryId);
    if (statesRes.data?.length) {
      const list = statesRes.data.map((item) => ({
        id: item.ptas_stateorprovinceid,
        title: item.ptas_abbreviation,
        subtitle: item.ptas_name,
      }));
      updateStateSuggestions(addressId, list, false);
    } else {
      updateStateSuggestions(addressId, [], false);
    }
  };

  const onZipCodeSearchChange = async (
    addressId: string,
    value: string
  ): Promise<void> => {
    if (!value || value.length < 2) {
      return;
    }

    updateZipCodeSuggestions(addressId, undefined, true);
    const zipCodesRes = await apiService.getZipCodes(value);
    if (zipCodesRes.data?.length) {
      const list = zipCodesRes.data.map((item) => ({
        id: item.ptas_zipcodeid,
        title: item.ptas_name,
        subtitle: '',
      }));
      updateZipCodeSuggestions(addressId, list, false);
    } else {
      updateZipCodeSuggestions(addressId, [], false);
    }
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

  const onLine1Change = async (value: string): Promise<void> => {
    if (!editingContact?.address?.id) return;
    const suggestionList =
      addressSuggestions.map.get(editingContact?.address?.id) ?? [];
    const suggestion = suggestionList.find((el) => el.formattedaddr === value);
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
    addressSuggestion: AddressLookup
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

  //#region Modals

  const onChangePrimaryEmail = async (emailId: string): Promise<void> => {
    const currentEmail = portalContact?.email;
    if (currentEmail && currentEmail.id !== emailId) {
      const newPrimary = emailList.find((el) => el.id === emailId);
      if (newPrimary) {
        const saveRes = await contactApiService.updatePrimaryEmail(
          newPrimary.id,
          currentEmail.id
        );
        if (saveRes.hasError) {
          console.error(
            'Error updating primary email.',
            saveRes.errorMessage,
            newPrimary
          );
          setErrorMessageState({
            open: true,
            errorHead: fm.updatePrimaryEmailError,
          });
          return;
        } else {
          newPrimary.primaryEmail = true;
          const newList = emailList.map((el) =>
            el.id === emailId ? newPrimary : { ...el, primaryEmail: false }
          );
          setEmailList(newList);
          setPortalContact((prev) => {
            if (!prev) return;
            return {
              ...prev,
              email: newPrimary,
            };
          });
        }
      }
    }
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
        errorHead: fm.deleteEmailError,
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

  const onChangeEmail = async (email: PortalEmail): Promise<void> => {
    const validEmail = utilService.validEmail(email.email);
    let emailExists = false;
    if (validEmail) {
      emailExists = await contactApiService.emailExists(
        email.id,
        email.email,
        email.isSaved
      );
      if (emailExists) {
        setErrorMessageState({
          open: true,
          errorHead: fm.emailExists,
        });
      }
    }

    //Only save if email is valid and does not exist
    if (validEmail && !emailExists) {
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
          errorHead: fm.saveEmailError,
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
    } else {
      //If email not valid, only update data on memory, do not save
      setEmailList((prev) => {
        if (!prev) return [];
        return prev.map((currentEmail) =>
          currentEmail.id === email.id
            ? { ...currentEmail, isSaved: false, email: email.email }
            : currentEmail
        );
      });
    }
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

  const onChangeAddress = async (address: PortalAddress): Promise<void> => {
    if (
      address.title &&
      address.line1 &&
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
        errorHead: fm.deleteAddressError,
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
    if (phone.phoneNumber.length >= 12 && phone.phoneTypeValue) {
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
          errorHead: fm.savePhoneError,
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
      phone.isSaved = false; //This phone will be updated on memory, but not on DB
      setPhoneList((prev) => {
        if (!prev) return [];
        return prev.map((currentPhone) =>
          currentPhone.id === phone.id ? phone : currentPhone
        );
      });
    }
  };

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
        errorHead: fm.deletePhoneError,
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

  //#endregion

  return {
    action,
    editingContact,
    setEditingContact,
    handleTabChange,
    handleContinue,
    currentStep,
    setCurrentStep,
    highestStepNumber,
    setHighestStepNumber,
    formIsValid,
    setFormIsValid,
    contactSuffixes,
    contactSuffixItems,
    addressList,
    phoneList,
    addressSuggestions,
    countryItems,
    stateSuggestions,
    citySuggestions,
    zipCodeSuggestions,
    phoneTypes,
    phoneTypeList,
    emailAnchor,
    setEmailAnchor,
    addressAnchor,
    setAddressAnchor,
    phoneAnchor,
    setPhoneAnchor,
    manageEmailAnchor,
    setManageEmailAnchor,
    manageAddressAnchor,
    setManageAddressAnchor,
    managePhoneAnchor,
    setManagePhoneAnchor,
    onTitleChange,
    onLine1Change,
    onLine2Change,
    onCountryChange,
    onCityChange,
    onStateChange,
    onZipCodeChange,
    onAddressSearchChange,
    onCitySearchChange,
    onStateSearchChange,
    onZipCodeSearchChange,
    onPhoneNumberChange,
    onPhoneTypeChange,
    onAcceptsMessagesChange,
    emailList,
    onChangePrimaryEmail,
    onAddEmptyEmail,
    onRemoveEmail,
    onChangeEmail,
    onAddEmptyAddress,
    onChangeAddress,
    onRemoveAddress,
    onAddEmptyPhone,
    onChangePhone,
    onRemovePhone,
    getAddressData,
  };
}

export default useProfileUtil;
