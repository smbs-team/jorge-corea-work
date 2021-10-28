// useProfile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction, useContext, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { DropDownItem, SnackContext } from '@ptas/react-public-ui-library';
import { apiService } from 'services/api/apiService';
import { Contact } from './Contact';
import { useAppContext } from 'contexts/AppContext';

export interface UseProfileUtil {
  editingContact: Contact | undefined;
  setEditingContact: React.Dispatch<React.SetStateAction<Contact | undefined>>;
  handleTabChange: (tab: number) => void;
  handleContinue: () => void;
  currentStep: ProfileStep | undefined;
  setCurrentStep: Dispatch<SetStateAction<ProfileStep>>;
  highestStepNumber: number;
  setHighestStepNumber: Dispatch<SetStateAction<number>>;
  countryItems: DropDownItem[];
}

export type ProfileStep = 'name' | 'email' | 'address' | 'phone';
export type ProfileParams = {
  contact: string;
  step: ProfileStep;
};

function useProfileUtil(): UseProfileUtil {
  const [editingContact, setEditingContact] = useState<Contact | undefined>();
  const [currentStep, setCurrentStep] = useState<ProfileStep>('name');
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const { setSnackState } = useContext(SnackContext);
  const { contactProfile, setContactProfile } = useAppContext();
  const history = useHistory();

  const countryItems = [
    { value: 'USA', label: 'USA' },
    { value: 'Canada', label: 'Canada' },
  ];

  const handleTabChange = (tab: number): void => {
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
    if (!editingContact) return;
    const step = currentStep ?? 'name';

    if (currentStep === 'name') {
      setCurrentStep('email');
      if (highestStepNumber < 1) setHighestStepNumber(1);
      saveContactData(step);
    } else if (currentStep === 'email') {
      setCurrentStep('address');
      if (highestStepNumber < 2) setHighestStepNumber(2);
      saveContactData(step);
    } else if (currentStep === 'address') {
      setCurrentStep('phone');
      if (highestStepNumber < 3) setHighestStepNumber(3);
      saveContactData(step);
    } else if (currentStep === 'phone') {
      await saveContactData(step);
    }
  };

  const saveContactData = async (step: ProfileStep): Promise<void> => {
    if (!editingContact) return;
    const savingContact = { ...editingContact };
    if (savingContact.type === 'jurisdiction') {
      const updateRes = await apiService.saveJurisdictionContact(
        savingContact,
        contactProfile
      );
      if (updateRes.hasError) {
        setSnackState({
          severity: 'error',
          text: 'Error saving contact info',
        });
      } else {
        setContactProfile(savingContact);
        if (step === 'phone') {
          setSnackState({
            severity: 'success',
            text: 'Contact info updated successfully',
          });
          history.replace('/permits');
        }
      }
    } else if (savingContact.type === 'taxDistrict') {
      const updateRes = await apiService.saveTaxDistrictContact(
        savingContact,
        contactProfile
      );
      if (updateRes.hasError) {
        setSnackState({
          severity: 'error',
          text: 'Error saving contact info',
        });
      } else {
        setContactProfile(savingContact);
        if (step === 'phone') {
          setSnackState({
            severity: 'success',
            text: 'Contact info updated successfully',
          });
          history.replace('/levy');
        }
      }
    }
  };

  return {
    editingContact,
    setEditingContact,
    handleTabChange,
    handleContinue,
    currentStep,
    setCurrentStep,
    highestStepNumber,
    setHighestStepNumber,
    countryItems,
  };
}

export default useProfileUtil;
