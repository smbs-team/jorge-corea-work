// TabEmail.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import {
  ContactInfoAlert,
  ListItem,
  ErrorMessageContext,
  utilService,
} from '@ptas/react-public-ui-library';
import ContactInfoModal from './ContactInfo/ContactInfoModal';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { AppContext } from 'contexts/AppContext';
import { contactApiService } from 'services/api/apiService/portalContact';
import { apiService } from 'services/api/apiService';
import { PortalEmail, PortalContact } from 'models/portalContact';
import { v4 as uuid } from 'uuid';
import ContactInfoView from './ContactInfo/ContactInfoView';
import EmailInfoFields from './ContactInfo/EmailInfoFields';
import EmailExistsPopover from './EmailExistsPopover';
import { ProfileStep } from './types';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 270,
    padding: theme.spacing(4, 0, 2, 0),
  },
  emailInput: {
    maxWidth: 270,
    marginBottom: theme.spacing(3.125),
  },
}));

interface Props {
  profileType: 'primary' | 'secondary';
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabEmail(props: Props): JSX.Element {
  const { profileType, updateFormIsValid } = props;
  console.log('profileType:', profileType);
  const classes = useStyles();
  const { portalContact, setPortalContact } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const {
    editingContact,
    editingContactSec,
    setEditingContactSec,
    emailList,
    setEmailList,
    emailExistsAnchorSec,
    setEmailExistsAnchorSec,
    emailRootRefSec,
    getExistingUserSec,
  } = useProfile();

  const [emailAnchor, setEmailAnchor] = useState<HTMLElement | null>(null);
  const [manageEmailAnchor, setManageEmailAnchor] =
    useState<HTMLElement | null>(null);

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

  const onRemoveEmail = async (emailId: string): Promise<void> => {
    const email = emailList.find((i) => i.id === emailId);
    if (!email?.isSaved) {
      setEmailList((prev) => {
        return prev.filter((i) => i.id !== emailId);
      });
      return;
    }

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

  const onChangeSecondaryEmail = (newEmail: string): void => {
    // onChange && onChange({ ...email, email: newEmail });
    setEditingContactSec((prev: PortalContact | undefined) => {
      if (!prev) return;
      if (!prev.email) {
        return {
          ...prev,
          email: {
            id: uuid(),
            email: newEmail,
            isSaved: false,
            portalContactId: prev.id,
            primaryEmail: true,
          },
        };
      }
      return {
        ...prev,
        email: {
          ...prev.email,
          email: newEmail,
        },
      };
    });
  };

  return (
    <Box className={classes.root}>
      {profileType === 'primary' && editingContact?.email && (
        <ContactInfoView
          variant="email"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setEmailAnchor(evt.currentTarget)}
          email={
            profileType === 'primary'
              ? editingContact?.email?.email
              : editingContactSec?.email?.email
          }
          useOtherEmailText={fm.otherEmail}
        />
      )}

      {profileType === 'secondary' && (
        <EmailInfoFields
          ref={emailRootRefSec}
          email={editingContactSec?.email}
          onEmailChange={onChangeSecondaryEmail}
          label={fm.modalLabelEmail}
          validEmailText={fm.requiredEmail}
          updateFormIsValid={updateFormIsValid}
        />
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
          onClickNewItem={onAddEmptyEmail}
          emailList={emailList}
          newItemText={fm.newEmail}
          emailTexts={{
            label: fm.modalLabelEmail,
            removeButtonText: fm.modalRemoveButtonEmail,
            removeAlertText: fm.modalRemoveAlertEmail,
            removeAlertButtonText: fm.modalRemoveAlertButtonEmail,
          }}
        />
      )}

      {emailExistsAnchorSec && (
        <EmailExistsPopover
          anchorEl={emailExistsAnchorSec}
          onClose={(): void => {
            setEmailExistsAnchorSec(null);
          }}
          onClick={(): void => {
            setEmailExistsAnchorSec(null);
            getExistingUserSec();
          }}
          buttonText={'Use existing user'}
        />
      )}
    </Box>
  );
}

export default TabEmail;
