// TabPhone.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import {
  ContactInfoAlert,
  ListItem,
  PhoneType,
  ErrorMessageContext,
} from '@ptas/react-public-ui-library';
import ContactInfoModal from './ContactInfo/ContactInfoModal';
import PhoneInfoFields from './ContactInfo/PhoneInfoFields';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { AppContext } from 'contexts/AppContext';
import { v4 as uuid } from 'uuid';
import { PortalPhone } from 'models/portalContact';
import { contactApiService } from 'services/api/apiService/portalContact';
import { apiService } from 'services/api/apiService';
import ContactInfoView from './ContactInfo/ContactInfoView';
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

function TabPhone(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const { portalContact, setPortalContact } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const {
    editingContact,
    setEditingContact,
    phoneList,
    setPhoneList,
    phoneTypes,
    phoneTypeList,
  } = useProfile();

  const [phoneAnchor, setPhoneAnchor] = useState<HTMLElement | null>(null);
  const [managePhoneAnchor, setManagePhoneAnchor] =
    useState<HTMLElement | null>(null);

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
      setPhoneList((prev) => {
        if (!prev) return [];
        return prev.map((currentPhone) =>
          currentPhone.id === phone.id ? phone : currentPhone
        );
      });
    }
  };

  const onRemovePhone = async (phoneId: string): Promise<void> => {
    const phone = phoneList.find((i) => i.id === phoneId);
    if (!phone?.isSaved) {
      setPhoneList((prev) => {
        return prev.filter((i) => i.id !== phoneId);
      });
      return;
    }

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

  return (
    <Box className={classes.root}>
      {editingContact?.phone.isSaved && (
        <ContactInfoView
          variant="phone"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setPhoneAnchor(evt.currentTarget)}
          phoneNumber={editingContact.phone.phoneNumber}
          acceptsTextMessages={editingContact.phone.acceptsTextMessages}
          useOtherPhoneText={fm.otherPhone}
          phoneAcceptsMessagesText={fm.phoneAcceptsMessages}
          phoneAcceptsNoMessagesText={fm.phoneAcceptsNoMessages}
        />
      )}
      {!editingContact?.phone.isSaved && (
        <PhoneInfoFields
          phone={editingContact?.phone}
          phoneTypeList={phoneTypeList}
          onPhoneNumberChange={onPhoneNumberChange}
          onPhoneTypeSelect={onPhoneTypeChange}
          onAcceptMessagesChange={onAcceptsMessagesChange}
          updateFormIsValid={updateFormIsValid}
          phoneTexts={{
            titleText: fm.modalPhoneTitle,
            placeholderText: fm.modalPhonePlaceholder,
            tabCellText: fm.modalPhoneTabCell,
            tabWorkText: fm.modalPhoneTabWork,
            tabHomeText: fm.modalPhoneTabHome,
            tabTollFreeText: fm.modalPhoneTabTollFree,
            acceptMessagesText: fm.modalPhoneAcceptsMessages,
            removeButtonText: fm.modalRemoveButtonPhone,
            removeAlertText: fm.modalRemoveAlertPhone,
            removeAlertButtonText: fm.modalRemoveAlertButtonPhone,
          }}
        />
      )}

      {phoneAnchor && (
        <ContactInfoAlert
          anchorEl={phoneAnchor}
          items={phoneList.map((i) => ({
            value: i.id,
            label: i.phoneNumber,
          }))}
          onItemClick={(item: ListItem): void =>
            console.log('Selected item:', item)
          }
          buttonText={fm.managePhones}
          onButtonClick={(): void => {
            setManagePhoneAnchor(document.body);
          }}
          onClose={(): void => {
            setPhoneAnchor(null);
          }}
        />
      )}

      {managePhoneAnchor && (
        <ContactInfoModal
          variant={'phone'}
          anchorEl={managePhoneAnchor}
          onClickNewItem={onAddEmptyPhone}
          onClose={(): void => setManagePhoneAnchor(null)}
          onPhoneTypeSelect={(
            id: string | number,
            phoneType: PhoneType
          ): void =>
            console.log('Selected phone type:', phoneType, 'for id:', id)
          }
          onRemoveItem={onRemovePhone}
          onChangeItem={onChangePhone}
          phoneList={phoneList}
          phoneTypeList={phoneTypeList}
          newItemText={fm.newPhone}
          phoneTexts={{
            titleText: fm.modalPhoneTitle,
            placeholderText: fm.modalPhonePlaceholder,
            tabCellText: fm.modalPhoneTabCell,
            tabWorkText: fm.modalPhoneTabWork,
            tabHomeText: fm.modalPhoneTabHome,
            tabTollFreeText: fm.modalPhoneTabTollFree,
            acceptMessagesText: fm.modalPhoneAcceptsMessages,
            removeButtonText: fm.modalRemoveButtonPhone,
            removeAlertText: fm.modalRemoveAlertPhone,
            removeAlertButtonText: fm.modalRemoveAlertButtonPhone,
          }}
        />
      )}
    </Box>
  );
}

export default TabPhone;
