// useSecondaryProfile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Dispatch,
  SetStateAction,
  useEffect,
  useState,
  useContext,
  useRef,
} from 'react';
import { useParams } from 'react-router-dom';
import { PortalContact } from 'models/portalContact';
import { contactApiService } from 'services/api/apiService/portalContact';
import { useAsync } from 'react-use';
import { ErrorMessageContext } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { useHistory } from 'react-router-dom';
import { ProfileStep } from './types';

export interface UseSecondaryProfile {
  editingContactSec: PortalContact | undefined;
  setEditingContactSec: Dispatch<SetStateAction<PortalContact | undefined>>;
  handleTabChangeSec: (tab: number) => void;
  handleContinueSec: () => void;
  currentStepSec: ProfileStep | undefined;
  setCurrentStepSec: Dispatch<SetStateAction<ProfileStep | undefined>>;
  highestStepNumberSec: number;
  setHighestStepNumberSec: Dispatch<SetStateAction<number>>;
  businessAccessListSec: PersonalPropertyAccountAccess[];
  setBusinessAccessListSec: Dispatch<
    SetStateAction<PersonalPropertyAccountAccess[]>
  >;
  deleteEditingContactSec: () => void;
  emailExistsAnchorSec: HTMLDivElement | null;
  setEmailExistsAnchorSec: Dispatch<SetStateAction<HTMLDivElement | null>>;
  emailRootRefSec: React.MutableRefObject<HTMLDivElement | null>;
  getExistingUserSec: () => void;
}

export type ProfileParams = {
  contact: string;
  step: ProfileStep;
};

function useSecondaryProfile(): UseSecondaryProfile {
  const history = useHistory();
  const { contact, step } = useParams<ProfileParams>();
  const [contactId, setContactId] = useState<string>();
  const [currentStep, setCurrentStep] = useState<ProfileStep>();
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const [editingContact, setEditingContact] =
    useState<PortalContact | undefined>();
  const editingContactIdRef = useRef<string | undefined>(undefined);
  const [prevContact, setPrevContact] = useState<PortalContact | undefined>();
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const [emailExistsAnchor, setEmailExistsAnchor] =
    useState<HTMLDivElement | null>(null);
  const emailRootRef = useRef<HTMLDivElement | null>(null);
  //Access data
  const [businessAccessList, setBusinessAccessList] = useState<
    PersonalPropertyAccountAccess[]
  >([]);

  // useEffect(() => {
  //   if (contactId) {
  //     setAction('update');
  //   }
  // }, [contactId]);

  //TODO: delete. TEMP for development.
  //Remove when sign-in implementation is done
  // useMount(async () => {
  //   const contactRes = await contactApiService.getTestContactSec();
  //   console.log('contactRes:', contactRes);
  //   if (!contactRes.data) {
  //     console.log('Test secondary contact not found');
  //   } else {
  //     console.log('Test secondary contact found:', contactRes.data);
  //     setEditingContact(contactRes.data);
  //     setPrevContact(contactRes.data);
  //   }
  // });

  useAsync(async () => {
    if (
      editingContact?.isSaved &&
      editingContact.id !== editingContactIdRef.current
    ) {
      editingContactIdRef.current = editingContact.id;

      //Load personal property
      const { data, hasError, errorMessage } =
        await businessContactApiService.getBusinessContactByContact(
          editingContact.id
        );
      if (hasError) {
        console.error('Error on load business access (sec):', errorMessage);
      } else {
        const businessAccess = data?.filter(
          (el) => el.personalPropertyId && el.personalProperty
        );
        console.log('businessAccess (sec):', businessAccess);
        if (businessAccess) {
          setBusinessAccessList(businessAccess);
        }
      }
    }
  }, [editingContact]);

  useEffect(() => {
    if (contactId && currentStep) {
      window.history.replaceState(
        null,
        '',
        `/sec-profile/${contactId}/${currentStep}`
      );
    } else if (contactId) {
      // setAction('update');
      window.history.replaceState(null, '', `/sec-profile/${contactId}`);
    }
  }, [contactId, currentStep]);

  useAsync(async () => {
    console.log('contact:', contact);
    console.log('step:', step);
    if (contact) {
      setContactId(contact);
      const res = await contactApiService.getPortalContactById(contact);
      if (res.hasError) {
        console.error(
          'Error on get portal contact for secondary profile.',
          res.errorMessage
        );
      } else if (res.data) {
        setEditingContact(res.data);
        setPrevContact(res.data);
      }
    } else {
      const newContact = new PortalContact();
      setEditingContact(newContact);
      setPrevContact(newContact);
      // return;
    }
    if (step) {
      setCurrentStep(step);
      if (step === 'email') {
        setHighestStepNumber(1);
      } else if (step === 'access') {
        setHighestStepNumber(2);
      }
    } else {
      setCurrentStep('name');
    }
  }, [contact, step]);

  const handleTabChange = async (tab: number): Promise<void> => {
    console.log('Tab selected:', tab);

    if (currentStep === 'name') {
      saveContactNameData();
    } else if (currentStep === 'email') {
      if (editingContact?.email?.email) {
        const emailExists = await contactApiService.emailExists(
          editingContact.email.id,
          editingContact.email.email,
          editingContact.email.isSaved
        );
        if (!editingContact.email.isSaved) {
          //If creating a new email, and the email address already exists,
          //show the user an option to recover their data
          if (emailExists) {
            setEmailExistsAnchor(emailRootRef.current);
            return;
          }
        } else {
          //If updating an email, and the email address exists,
          //the email should not be saved
          if (emailExists) {
            setErrorMessageState({
              open: true,
              errorHead: fm.emailExists,
            });
            return;
          }
        }

        saveContactEmail();
      }
    }

    if (tab === 1) {
      setCurrentStep('email');
    } else if (tab === 2) {
      setCurrentStep('access');
    } else {
      setCurrentStep('name');
    }
  };

  const handleContinue = async (): Promise<void> => {
    console.log('handleContinue editinContact:', editingContact);
    if (currentStep === 'name') {
      setCurrentStep('email');
      if (highestStepNumber < 1) setHighestStepNumber(1);
      saveContactNameData();
    } else if (currentStep === 'email') {
      if (!editingContact?.email?.email) return;

      const emailExists = await contactApiService.emailExists(
        editingContact.email.id,
        editingContact.email.email,
        editingContact.email.isSaved
      );
      if (!editingContact.email.isSaved) {
        //If creating a new email, and the email address already exists,
        //show the user an option to recover their data
        if (emailExists) {
          setEmailExistsAnchor(emailRootRef.current);
          return;
        }
      } else {
        //If updating an email, and the email address exists,
        //the email should not be saved
        if (emailExists) {
          setErrorMessageState({
            open: true,
            errorHead: fm.emailExists,
          });
          return;
        }
      }

      setCurrentStep('access');
      if (highestStepNumber < 2) setHighestStepNumber(2);
      saveContactEmail();
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
      prevContact
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
      savingContact.isSaved = true;
      setEditingContact({ ...savingContact });
      setPrevContact({ ...savingContact });
    }
  };

  const saveContactEmail = async (): Promise<void> => {
    if (!editingContact?.email) return;
    const savingEmail = { ...editingContact.email };
    const saveRes = await contactApiService.savePortalEmail(
      savingEmail,
      prevContact?.email
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
      savingEmail.isSaved = true;
      setPrevContact({ ...editingContact, email: savingEmail });
      setEditingContact({ ...editingContact, email: savingEmail });
    }
  };

  const getExistingUser = async (): Promise<void> => {
    if (!editingContact?.email?.email) return;
    const contactId = editingContact.id;
    const { data: existingContact } =
      await contactApiService.getPortalContactByEmail(
        editingContact.email.email
      );
    console.log('existingContact:', existingContact);
    if (!existingContact) return;

    setEditingContact(existingContact);

    //Delete the contact that will not be used
    contactApiService.deleteContact(contactId);
  };

  const deleteEditingContact = async (): Promise<void> => {
    if (!editingContact) return;
    const deleteRes = await contactApiService.deleteContact(editingContact.id);
    console.log('deleteRes:', deleteRes);
    if (deleteRes.hasError) {
      console.error(
        'Error deleting portal contact.',
        deleteRes.errorMessage,
        editingContact.id
      );
      setErrorMessageState({
        open: true,
        errorHead: fm.deleteContactError,
      });
      return;
    } else {
      history.goBack();
    }
  };

  return {
    editingContactSec: editingContact,
    setEditingContactSec: setEditingContact,
    handleTabChangeSec: handleTabChange,
    handleContinueSec: handleContinue,
    currentStepSec: currentStep,
    setCurrentStepSec: setCurrentStep,
    highestStepNumberSec: highestStepNumber,
    setHighestStepNumberSec: setHighestStepNumber,
    businessAccessListSec: businessAccessList,
    setBusinessAccessListSec: setBusinessAccessList,
    deleteEditingContactSec: deleteEditingContact,
    emailExistsAnchorSec: emailExistsAnchor,
    setEmailExistsAnchorSec: setEmailExistsAnchor,
    emailRootRefSec: emailRootRef,
    getExistingUserSec: getExistingUser,
  };
}

export default useSecondaryProfile;
