// TabPhone.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomTextButton,
  ContactInfoAlert,
  ListItem,
} from '@ptas/react-public-ui-library';
import PhoneInfoFields from './ContactInfoModal/PhoneInfoFields';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { useStyles } from './profileStyles';
import ContactInfoModal from './ContactInfoModal';
import { ProfileStep } from './types';

interface Props {
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabPhone(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    editingContact: profile,
    phoneAnchor,
    setPhoneAnchor,
    managePhoneAnchor,
    setManagePhoneAnchor,
    phoneList,
    onPhoneNumberChange,
    onPhoneTypeChange,
    onAcceptsMessagesChange,
    onAddEmptyPhone,
    onChangePhone,
    phoneTypeList,
    onRemovePhone,
  } = useProfile();

  return (
    <Box className={classes.tab}>
      {profile?.phone.isSaved && (
        <Fragment>
          <Box className={classes.textButtonContainer}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{ root: classes.textButton }}
              onClick={(
                evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
              ): void => setPhoneAnchor(evt.currentTarget)}
            >
              {fm.otherPhone}
            </CustomTextButton>
          </Box>
          <label className={classes.phone}>{profile.phone.phoneNumber}</label>
          <label className={classes.textMessage}>
            {profile.phone.acceptsTextMessages
              ? fm.phoneAcceptsMessages
              : fm.phoneAcceptsNoMessages}
          </label>
        </Fragment>
      )}
      {!profile?.phone.isSaved && (
        <PhoneInfoFields
          phone={profile?.phone}
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
