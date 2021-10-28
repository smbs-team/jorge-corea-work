// usePrimaryProfile.tsx
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
} from '@ptas/react-public-ui-library';
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
} from 'models/portalContact';
import { AppContext } from 'contexts/AppContext';
import { useMount } from 'react-use';
import { apiService } from 'services/api/apiService';
import { contactApiService } from 'services/api/apiService/portalContact';
import * as fm from './formatText';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { v4 as uuid } from 'uuid';
import useAuth from 'auth/useAuth';
import { ProfileAction, ProfileFormIsValid, ProfileStep } from './types';

export interface UsePrimaryProfile {
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
  emailList: PortalEmail[];
  setEmailList: Dispatch<SetStateAction<PortalEmail[]>>;
  addressList: PortalAddress[];
  setAddressList: Dispatch<SetStateAction<PortalAddress[]>>;
  phoneList: PortalPhone[];
  setPhoneList: Dispatch<SetStateAction<PortalPhone[]>>;
  countryItems: DropDownItem[];
  setCountryItems: Dispatch<SetStateAction<DropDownItem[]>>;
  contactSuffixes: Map<string, number>;
  contactSuffixItems: DropDownItem[];
  phoneTypes: Map<string, number>;
  phoneTypeList: { value: number; label: string }[];
  businessAccessList: PersonalPropertyAccountAccess[];
  accessLevels: Map<string, number>;
  accessLevelList: { value: number; label: string }[];
  accessStatusCodes: Map<string, number>;
}

function usePrimaryProfile(): UsePrimaryProfile {
  const location = useLocation<{ newContact?: boolean }>();
  const [action, setAction] = useState<ProfileAction>('update');
  const history = useHistory();
  const auth = useAuth();
  const { portalContact, setPortalContact /*, redirectPath*/ } =
    useContext(AppContext);
  const portalContactIdRef = useRef<string | undefined>(undefined);
  const [editingContact, setEditingContact] =
    useState<PortalContact | undefined>();
  const [currentStep, setCurrentStep] = useState<ProfileStep>('name');
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const [formIsValid, setFormIsValid] = useState<ProfileFormIsValid>({
    step: 'name',
    valid: true,
  });

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
  //Phone data
  const [phoneList, setPhoneList] = useState<PortalPhone[]>([]);
  const [phoneTypes, setPhoneTypes] = useState<Map<string, number>>(new Map());
  const [phoneTypeList, setPhoneTypeList] = useState<
    { value: number; label: string }[]
  >([]);
  //Access data
  const [businessAccessList, setBusinessAccessList] = useState<
    PersonalPropertyAccountAccess[]
  >([]);
  const [accessLevels, setAccessLevels] = useState<Map<string, number>>(
    new Map()
  );
  const [accessLevelList, setAccessLevelList] = useState<
    { value: number; label: string }[]
  >([]);
  const [accessStatusCodes, setAccessStatusCodes] = useState<
    Map<string, number>
  >(new Map());

  const { setErrorMessageState } = useContext(ErrorMessageContext);

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
    loadAccessLevels();
    loadAccessStatusCodes();
  });

  useEffect(() => {
    if (portalContact) {
      setEditingContact(portalContact);
    } else {
      if (!auth?.email) {
        console.error('Token not found. Cannot read email.');
      }
      const newContact = new PortalContact();
      newContact.email = {
        id: uuid(),
        isSaved: false,
        email: auth?.email ?? '',
        primaryEmail: true,
        portalContactId: newContact.id,
      };
      setEditingContact(newContact);
    }
  }, [portalContact, setEditingContact, auth]);

  useEffect(() => {
    if (location.state && location.state.newContact) {
      setAction('create');
    }
  }, [location.state]);

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

        //Load personal property
        if (portalContact?.id) {
          const { data, hasError, errorMessage } =
            await businessContactApiService.getBusinessContactByContact(
              portalContact.id
            );
          if (hasError) {
            console.error('Error on load business access:', errorMessage);
          } else {
            const businessAccess = data?.filter(
              (el) => el.personalPropertyId && el.personalProperty
            );
            console.log('businessAccess:', businessAccess);
            if (businessAccess) {
              setBusinessAccessList(businessAccess);
            }
          }
        }
      })();
    }
  }, [portalContact]);

  const handleTabChange = (tab: number): void => {
    console.log('Tab selected:', tab);
    if (tab === 1) {
      setCurrentStep('email');
    } else if (tab === 2) {
      setCurrentStep('password');
    } else if (tab === 3) {
      setCurrentStep('address');
    } else if (tab === 4) {
      setCurrentStep('phone');
    } else if (tab === 5) {
      setCurrentStep('access');
    } else {
      setCurrentStep('name');
    }
  };

  const handleContinue = (): void => {
    if (currentStep === 'name') {
      setCurrentStep('email');
      if (highestStepNumber < 1) setHighestStepNumber(1);
      saveContactNameData();
    } else if (currentStep === 'email') {
      setCurrentStep('password');
      if (highestStepNumber < 2) setHighestStepNumber(2);
      saveContactEmail();
    } else if (currentStep === 'password') {
      setCurrentStep('address');
      if (highestStepNumber < 3) setHighestStepNumber(3);
    } else if (currentStep === 'address') {
      setCurrentStep('phone');
      if (highestStepNumber < 4) setHighestStepNumber(4);
      saveContactAddress();
    } else if (currentStep === 'phone') {
      setCurrentStep('access');
      if (highestStepNumber < 5) setHighestStepNumber(5);
      saveContactPhone();
    } else if (currentStep === 'access') {
      //Done
      history.replace('/home');
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

    //No need to validate existence of email here,
    // since the email is taken from the auth token
    // and there it is checked whether there exists
    // a user with that email address.

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
    }
  };

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
      setCountryItems(countries);
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

  const loadAccessLevels = async (): Promise<void> => {
    const { data: _accessLevels } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'ptas_accesslevel'
    );
    if (_accessLevels) {
      setAccessLevelList(
        _accessLevels.map((el) => ({
          value: el.attributeValue,
          label: el.value,
        }))
      );
      const map = new Map<string, number>(
        _accessLevels?.map((c) => [c.value, c.attributeValue])
      );
      setAccessLevels(map);
    }
  };

  const loadAccessStatusCodes = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'statuscode'
    );
    if (!data) return;
    const newOpts = data.map(
      (opt) => [opt.value, opt.attributeValue] as [string, number]
    );
    setAccessStatusCodes(new Map(newOpts));
  };

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
    emailList,
    setEmailList,
    addressList,
    setAddressList,
    phoneList,
    setPhoneList,
    countryItems,
    setCountryItems,
    contactSuffixes,
    contactSuffixItems,
    phoneTypes,
    phoneTypeList,
    businessAccessList,
    accessLevels,
    accessLevelList,
    accessStatusCodes,
  };
}

export default usePrimaryProfile;
