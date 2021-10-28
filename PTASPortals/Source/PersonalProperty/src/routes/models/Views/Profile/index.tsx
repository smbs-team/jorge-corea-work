// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useCallback } from 'react';
import {
  CustomTextButton,
  CustomTabs,
  CustomButton,
  MainCard,
  useScrollBehavior,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import TabName from './TabName';
import TabEmail from './TabEmail';
import TabAddress from './TabAddress';
import TabPhone from './TabPhone';
import TabAccess from './TabAccess';
import TabPassword from './TabPassword';
import clsx from 'clsx';
import HelpSection from '../../../../components/HelpSection';
import { useProfile, withProfileProvider } from './ProfileContext';
import { useStyles } from './profileStyles';
import { useHistory } from 'react-router-dom';
import { ProfileStep } from './types';

function PrimaryProfile(): JSX.Element {
  const classes = useStyles();
  const {
    currentStep,
    highestStepNumber,
    handleTabChange,
    handleContinue,
    action,
    formIsValid,
    setFormIsValid,
  } = useProfile();
  const { headerRef, contentRef } = useScrollBehavior();

  const updateFormIsValid = useCallback(
    (step: ProfileStep, valid: boolean): void => {
      console.debug('Primary profile form - valid:', valid, 'step:', step);
      setFormIsValid({ step, valid });
    },
    [setFormIsValid]
  );

  const [isLocalHost] = useState<boolean>(
    window.location.hostname === '_localhost' ||
      window.location.hostname === '_127.0.0.1'
  );
  const history = useHistory();

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
          {fm.back}
        </CustomTextButton>
        <Box className={classes.titleWrapper}>
          <h2 className={clsx(classes.title, classes.colorBlack)}>
            {fm.title1}
          </h2>
          <h2 className={clsx(classes.title, classes.colorGray)}>
            {fm.title2}
          </h2>
        </Box>
        {/* <ProfileOptionsButton /> */}
        <div style={{ width: '64px' }}></div>
      </Box>
      <CustomTabs
        selectedIndex={
          currentStep === 'name'
            ? 0
            : currentStep === 'email'
            ? 1
            : currentStep === 'password'
            ? 2
            : currentStep === 'address'
            ? 3
            : currentStep === 'phone'
            ? 4
            : currentStep === 'access'
            ? 5
            : 0
        }
        ptasVariant="Switch"
        items={[
          { label: fm.tabName, disabled: false },
          {
            label: fm.tabEmail,
            disabled:
              highestStepNumber < 1 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabPassword,
            disabled:
              highestStepNumber < 2 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabAddress,
            disabled:
              highestStepNumber < 3 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabPhone,
            disabled:
              highestStepNumber < 4 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabAccess,
            disabled:
              highestStepNumber < 5 && action === 'create' && !isLocalHost,
          },
        ]}
        onSelected={(tab: number): void => {
          handleTabChange(tab);
        }}
      />
    </Fragment>
  );

  return (
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
            <TabName
              profileType="primary"
              updateFormIsValid={updateFormIsValid}
            />
          )}
          {currentStep === 'email' && (
            <TabEmail
              profileType="primary"
              updateFormIsValid={updateFormIsValid}
            />
          )}
          {currentStep === 'password' && <TabPassword />}
          {currentStep === 'address' && (
            <TabAddress updateFormIsValid={updateFormIsValid} />
          )}
          {currentStep === 'phone' && (
            <TabPhone updateFormIsValid={updateFormIsValid} />
          )}
          {currentStep === 'access' && <TabAccess profileType="primary" />}

          <CustomButton
            classes={{
              root:
                currentStep === 'access'
                  ? classes.doneButton
                  : classes.continueButton,
            }}
            onClick={handleContinue}
            ptasVariant="Primary"
            disabled={currentStep === formIsValid.step && !formIsValid.valid}
          >
            {currentStep === 'access' ? fm.buttonDone : fm.buttonContinue}
          </CustomButton>
          <HelpSection />
        </Box>
      </MainCard>
    </MainLayout>
  );
}

export default withProfileProvider(PrimaryProfile);
