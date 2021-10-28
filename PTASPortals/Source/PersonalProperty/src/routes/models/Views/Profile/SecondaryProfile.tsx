// SecondaryProfile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useCallback } from 'react';
import {
  useScrollBehavior,
  CustomTextButton,
  CustomTabs,
  CustomButton,
  MainCard,
} from '@ptas/react-public-ui-library';
import { Box } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import TabName from './TabName';
import TabEmail from './TabEmail';
import TabAccess from './TabAccess';
import clsx from 'clsx';
import RemoveAlert from '../../../../components/RemoveAlert';
import HelpSection from '../../../../components/HelpSection';
import { useProfile, withProfileProvider } from './ProfileContext';
import { useStyles } from './profileStyles';
import { useHistory } from 'react-router-dom';
import { ProfileStep } from './types';

function SecondaryProfile(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();
  const {
    editingContactSec,
    currentStepSec,
    highestStepNumberSec,
    handleTabChangeSec,
    handleContinueSec,
    deleteEditingContactSec,
    action,
    formIsValid,
    setFormIsValid,
  } = useProfile();
  const { headerRef, contentRef } = useScrollBehavior();
  const [removeAccountAnchor, setRemoveAccountAnchor] =
    useState<HTMLElement | null>(null);

  const updateFormIsValid = useCallback(
    (step: ProfileStep, valid: boolean): void => {
      console.debug('Secondary profile form - valid:', valid, 'step:', step);
      setFormIsValid({ step, valid });
    },
    [setFormIsValid]
  );

  const [isLocalHost] = useState<boolean>(
    window.location.hostname === '_localhost' ||
      window.location.hostname === '_127.0.0.1'
  );

  const onRemoveContact = (): void => {
    deleteEditingContactSec();
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
        <Box className={classes.titleWrapper}>
          <h2 className={clsx(classes.title, classes.colorBlack)}>
            {fm.title1}
          </h2>
          <h2 className={clsx(classes.title, classes.colorGray)}>
            {editingContactSec?.isSaved
              ? `${editingContactSec.firstName} ${editingContactSec.lastName}`
              : fm.title3}
          </h2>
        </Box>
        {/* <ProfileOptionsButton /> */}
        <div style={{ width: '64px' }}></div>
      </Box>

      <CustomTabs
        classes={{ item: classes.tabWideItem }}
        selectedIndex={
          currentStepSec === 'name'
            ? 0
            : currentStepSec === 'email'
            ? 1
            : currentStepSec === 'access'
            ? 2
            : 0
        }
        ptasVariant="Switch"
        items={[
          { label: fm.tabName, disabled: false },
          {
            label: fm.tabEmail,
            disabled:
              highestStepNumberSec < 1 && action === 'create' && !isLocalHost,
          },
          {
            label: fm.tabAccess,
            disabled:
              highestStepNumberSec < 2 && action === 'create' && !isLocalHost,
          },
        ]}
        onSelected={(tab: number): void => {
          handleTabChangeSec(tab);
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
            {currentStepSec === 'name' && (
              <TabName
                profileType="secondary"
                updateFormIsValid={updateFormIsValid}
              />
            )}
            {currentStepSec === 'email' && (
              <TabEmail
                profileType="secondary"
                updateFormIsValid={updateFormIsValid}
              />
            )}
            {currentStepSec === 'access' && (
              <TabAccess profileType="secondary" />
            )}

            <CustomButton
              classes={{
                root:
                  currentStepSec === 'access'
                    ? classes.doneButton
                    : classes.continueButton,
              }}
              onClick={handleContinueSec}
              ptasVariant="Primary"
              disabled={!formIsValid}
            >
              {currentStepSec === 'access' ? fm.buttonDone : fm.buttonContinue}
            </CustomButton>

            {editingContactSec?.isSaved && (
              <CustomTextButton
                ptasVariant="Text more"
                classes={{ root: classes.textButton }}
                onClick={(
                  evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
                ): void => setRemoveAccountAnchor(evt.currentTarget)}
              >
                {fm.buttonRemoveAccount(editingContactSec.firstName)}
              </CustomTextButton>
            )}
            <HelpSection />
          </Box>
        </MainCard>
      </MainLayout>
      {removeAccountAnchor && (
        <RemoveAlert
          anchorEl={removeAccountAnchor}
          alertContent={fm.removeAccountAlert}
          buttonText={fm.removeAccountAlertButton}
          onClose={(): void => {
            setRemoveAccountAnchor(null);
          }}
          onButtonClick={onRemoveContact}
        />
      )}
    </Fragment>
  );
}

export default withProfileProvider(SecondaryProfile);
