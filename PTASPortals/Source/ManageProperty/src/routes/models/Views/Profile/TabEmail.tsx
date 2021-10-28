// TabEmail.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomTextButton,
  ContactInfoAlert,
  ListItem,
  CustomTextField,
} from '@ptas/react-public-ui-library';
import ContactInfoModal from './ContactInfoModal';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { useStyles } from './profileStyles';

function TabEmail(): JSX.Element {
  const classes = useStyles();
  const {
    editingContact,
    emailAnchor,
    setEmailAnchor,
    manageEmailAnchor,
    setManageEmailAnchor,
    emailList,
    onAddEmptyEmail,
    onRemoveEmail,
    onChangeEmail,
    onChangePrimaryEmail,
  } = useProfile();

  return (
    <Box className={classes.tab}>
      {editingContact?.email && (
        <Fragment>
          <Box className={classes.textButtonContainer}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{ root: classes.textButton }}
              onClick={(
                evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
              ): void => setEmailAnchor(evt.currentTarget)}
            >
              {fm.otherEmail}
            </CustomTextButton>
          </Box>
          <label className={classes.selectedEmail}>
            {editingContact.email.email}
          </label>
        </Fragment>
      )}
      {!editingContact?.email && (
        <Fragment>
          <CustomTextField
            classes={{ root: classes.emailInput }}
            ptasVariant="email"
            label={fm.emailLabel}
          />
          <Box className={classes.textButtonContainer}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{ root: classes.textButton }}
              onClick={(
                evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
              ): void => setEmailAnchor(evt.currentTarget)}
            >
              {fm.addEmail}
            </CustomTextButton>
          </Box>
        </Fragment>
      )}

      {emailAnchor && (
        <ContactInfoAlert
          anchorEl={emailAnchor}
          items={emailList.map((i) => ({
            value: i.id,
            label: i.email,
          }))}
          onItemClick={(item: ListItem): void => {
            onChangePrimaryEmail(item.value as string);
            setEmailAnchor(null);
          }}
          buttonText={fm.manageEmails}
          onButtonClick={(): void => {
            setManageEmailAnchor(document.body);
          }}
          onClose={(): void => {
            setEmailAnchor(null);
          }}
        />
      )}

      {manageEmailAnchor && (
        <ContactInfoModal
          variant={'email'}
          anchorEl={manageEmailAnchor}
          onClose={(): void => setManageEmailAnchor(null)}
          onRemoveItem={onRemoveEmail}
          onChangeItem={onChangeEmail}
          emailList={emailList}
          onClickNewEmail={onAddEmptyEmail}
          newItemText={fm.newEmail}
          emailTexts={{
            label: fm.modalLabelEmail,
            removeButtonText: fm.modalRemoveButtonEmail,
            removeAlertText: fm.modalRemoveAlertEmail,
            removeAlertButtonText: fm.modalRemoveAlertButtonEmail,
          }}
        />
      )}
    </Box>
  );
}

export default TabEmail;
