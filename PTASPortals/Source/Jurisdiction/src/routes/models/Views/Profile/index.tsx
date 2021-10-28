// Profile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState } from 'react';
import {
  MainCard,
  CustomTextButton,
  CustomTabs,
  CustomButton,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import { useStyles } from './profileStyles';
import TabName from './TabName';
import TabAddress from './TabAddress';
import { useProfile, withProfileProvider } from './ProfileContext';
import { useAppContext } from 'contexts/AppContext';
import useScrollBehavior from 'hooks/useScrollBehavior';
import TabEmail from './TabEmail';
import TabPhone from './TabPhone';
import { useHistory } from 'react-router-dom';
import ProfileOptionsButton from 'components/ProfileOptionsButton';

function ProfileComp(): JSX.Element {
  const classes = useStyles();
  const {
    setEditingContact,
    currentStep,
    highestStepNumber,
    handleTabChange,
    handleContinue,
  } = useProfile();
  const { contactProfile } = useAppContext();
  const { headerRef, contentRef } = useScrollBehavior();
  const history = useHistory();
  const [formIsValid, setFormIsValid] = useState<boolean>(true);

  useEffect(() => {
    setEditingContact(contactProfile);
  }, [contactProfile, setEditingContact]);

  const [isLocalHost] = useState<boolean>(
    window.location.hostname === 'localhost' ||
      window.location.hostname === '127.0.0.1'
  );

  const updateFormIsValid = (isValid: boolean): void => {
    setFormIsValid(isValid);
  };

  const renderCardHeader = (): JSX.Element => (
    <Fragment>
      <Box className={classes.headerLine}>
        <CustomTextButton
          ptasVariant="Clear"
          onClick={(): void => {
            history.goBack();
          }}
          classes={{ root: classes.back }}
        >
          Back
        </CustomTextButton>
        <h2 className={classes.title}>{fm.title}</h2>
        <ProfileOptionsButton />
      </Box>
      <CustomTabs
        selectedIndex={
          currentStep === 'name'
            ? 0
            : currentStep === 'email'
            ? 1
            : currentStep === 'address'
            ? 2
            : currentStep === 'phone'
            ? 3
            : 0
        }
        ptasVariant="Switch"
        items={[
          { label: fm.tabName, disabled: false },
          {
            label: fm.tabEmail,
            disabled: highestStepNumber < 1 && !isLocalHost,
          },
          {
            label: fm.tabAddress,
            disabled: highestStepNumber < 2 && !isLocalHost,
          },
          {
            label: fm.tabPhone,
            disabled: highestStepNumber < 3 && !isLocalHost,
          },
        ]}
        onSelected={(tab: number): void => {
          handleTabChange(tab);
        }}
      />
    </Fragment>
  );

  return (
    <Fragment>
      <MainLayout>
        <MainCard
          width={702}
          header={renderCardHeader()}
          refs={{
            headerRef: headerRef,
            contentRef: contentRef,
          }}
          classes={{
            root: classes.card,
            header: classes.cardHeader,
            content: classes.cardContent,
          }}
        >
          <Box className={classes.content}>
            {currentStep === 'name' && (
              <TabName updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'email' && (
              <TabEmail updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'address' && (
              <TabAddress updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'phone' && (
              <TabPhone updateFormIsValid={updateFormIsValid} />
            )}

            <CustomButton
              disabled={!formIsValid}
              classes={{
                root:
                  currentStep === 'phone'
                    ? classes.doneButton
                    : classes.continueButton,
              }}
              onClick={(): void => {
                handleContinue();
              }}
              ptasVariant="Primary"
            >
              {currentStep === 'phone' ? fm.buttonDone : fm.buttonContinue}
            </CustomButton>
          </Box>
        </MainCard>
      </MainLayout>
    </Fragment>
  );
}

export default withProfileProvider(ProfileComp);
