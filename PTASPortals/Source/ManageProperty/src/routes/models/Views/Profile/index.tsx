// Profile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useCallback } from 'react';
import {
  MainCard,
  CustomTextButton,
  CustomTabs,
  CustomButton,
  useScrollBehavior,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import { useStyles } from './profileStyles';
import TabName from './TabName';
import TabEmail from './TabEmail';
import TabAddress from './TabAddress';
import TabPhone from './TabPhone';
import { useProfile, withProfileProvider } from './ProfileContext';
import { useHistory } from 'react-router-dom';
import { ProfileStep } from './types';

function ProfileComp(): JSX.Element {
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
  const history = useHistory();

  const updateFormIsValid = useCallback(
    (step: ProfileStep, valid: boolean): void => {
      console.debug('Profile form - valid:', valid, 'step:', step);
      setFormIsValid({ step, valid });
    },
    [setFormIsValid]
  );

  const [isLocalHost] = useState<boolean>(
    window.location.hostname === '_localhost' ||
      window.location.hostname === '_127.0.0.1'
  );

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
        {/* <ProfileOptionsButton /> */}
        <div style={{ width: '64px' }}></div>
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
            disabled:
              highestStepNumber < 1 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabAddress,
            disabled:
              highestStepNumber < 2 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabPhone,
            disabled:
              highestStepNumber < 3 && action === 'create' && !isLocalHost,
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
            {currentStep === 'email' && <TabEmail />}
            {currentStep === 'address' && (
              <TabAddress updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'phone' && (
              <TabPhone updateFormIsValid={updateFormIsValid} />
            )}

            <CustomButton
              disabled={currentStep === formIsValid.step && !formIsValid.valid}
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
