// HICardPropertyInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';
import {
  CardPropertyInfo,
  CustomTabs,
  CustomButton,
} from '@ptas/react-public-ui-library';
import { Box, makeStyles, Theme } from '@material-ui/core';
import { HomeImprovementApplication } from './model/homeImprovementApplication';
import { useHI } from './HomeImprovementContext';
import {
  STATUS_CODE_ADDITIONAL_INFO_NEEDED,
  STATUS_CODE_APP_CREATED,
  STATUS_CODE_UNSUBMITTED,
} from './constants';
import * as fm from './formatMessage';
import isLocalHost from 'Utils/isLocalHost';
import TabConstruction from './TabConstruction';
import TabPermit from './TabPermit';
import TabSign from './TabSign';
import { useUpdateEffect } from 'react-use';
import { AppContext } from 'contexts/AppContext';
import { apiService } from 'services/api/apiService';
import { ApplicationStep } from './useHomeImprovement';
import { isEqual } from 'lodash';

interface Props {
  application: HomeImprovementApplication;
  homeImprovementId: string;
  selected: boolean;
  complete: boolean;
}

const useStyles = makeStyles((theme: Theme) => ({
  cardPropertyInfo: {
    maxWidth: 623,
    width: '100%',
    marginBottom: theme.spacing(4),
  },
  cardPropertyInfoContent: {
    background: 'rgba(0,0,0,0)',
  },
  cardPropertyInfoChildrenContent: {
    background: 'rgba(255, 255, 255, 0.5)',
    display: 'block',
  },
  applicationContent: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  continueButton: {
    width: 135,
  },
  continueButtonLarge: {
    width: 191,
  },
}));

function HICardPropertyInfo(props: Props): JSX.Element {
  const classes = useStyles();
  const { application, homeImprovementId, selected, complete } = props;
  const { portalContact } = useContext(AppContext);
  const {
    currentHiApplication,
    setCurrentHiApplication,
    setPrevApplication,
    parcelData,
    statusCodes,
    setApplicationList,
    currentStep,
    highestStepNumber,
    handleContinue,
    handleTabChange,
    setCurrentStep,
    setHighestStepNumber,
    redirectPage,
  } = useHI();
  const parcelInfo = parcelData.find(
    (p) => p.ptas_parceldetailid === application.parcelId
  );
  const [jsonSteps, setJsonSteps] = useState<object>();
  const [formIsValid, setFormIsValid] = useState<boolean>(true);

  const updateFormIsValid = (isValid: boolean): void => {
    setFormIsValid(isValid);
  };

  useEffect(() => {
    if (!selected || complete) return;
    if (!portalContact?.id) return;

    console.debug(
      `URL management useEffect 1. HI: ${homeImprovementId}`,
      `currentStep: ${currentStep}. highestStepNumber: ${highestStepNumber}`
    );

    getStep(portalContact.id, homeImprovementId);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selected, complete, portalContact, homeImprovementId]);

  useUpdateEffect(() => {
    if (!selected) return;
    if (!currentStep /*|| !highestStepNumber*/) return;
    if (!portalContact?.id) return;
    if (homeImprovementId !== currentHiApplication?.homeImprovementId) return;

    console.debug(
      `URL management useEffect 2. HI: ${homeImprovementId}`,
      `currentStep: ${currentStep}. highestStepNumber: ${highestStepNumber}`
    );

    redirectPage(homeImprovementId);

    const compareJson = {
      highestStep: highestStepNumber,
      currentStep: currentStep,
    };

    if (isEqual(compareJson, jsonSteps)) return;

    saveStep(
      portalContact.id,
      homeImprovementId,
      currentStep,
      highestStepNumber
    );
  }, [currentStep, highestStepNumber]);

  async function saveStep(
    contactId: string,
    applicationId: string,
    currentStep: string,
    highestStep: number
  ): Promise<void> {
    const steps = {
      highestStep,
      currentStep,
    };
    console.debug(
      `URL management saveStep HI: ${applicationId}`,
      `currentStep: ${currentStep}. highestStep: ${highestStep}`
    );

    setJsonSteps(steps);

    const { hasError } = await apiService.saveJson(
      `portals/home-improvement/${contactId}/${applicationId}/step`,
      steps
    );

    if (hasError) {
      console.error('Error to save JSON');
    }
  }

  async function getStep(
    contactId: string,
    applicationId: string
  ): Promise<void> {
    console.debug(`URL management getStep HI: ${applicationId}`);

    const { hasError, data } = await apiService.getJson(
      `portals/home-improvement/${contactId}/${applicationId}/step/step`
    );

    console.debug(
      `URL management getStep res HI: ${applicationId}`,
      `currentStep: ${data?.currentStep}. highestStep: ${data?.highestStep}`
    );

    if (hasError) {
      return console.error('Error to get step');
    }

    if (!data || (Array.isArray(data) && !data.length)) {
      redirectPage(homeImprovementId, 'construction');
      saveStep(contactId, applicationId, 'construction', 0);
      return;
    }

    //On first step
    if (!data.currentStep || !data.highestStep) {
      //This is required because if current and highest steps
      // do not change, the effects that update the URL
      // might not be fired.
      redirectPage(homeImprovementId, 'construction');
    }

    setJsonSteps({
      highestStep: data.highestStep as number,
      currentStep: data.currentStep as ApplicationStep,
    });
    setCurrentStep(data.currentStep as ApplicationStep);
    setHighestStepNumber(data.highestStep as number);
  }

  return (
    <CardPropertyInfo
      id={application.homeImprovementId}
      key={application.homeImprovementId}
      classes={{
        root: classes.cardPropertyInfo,
        cardContent: classes.cardPropertyInfoContent,
        content: classes.cardPropertyInfoChildrenContent,
      }}
      onClick={(id: string): void => {
        if (
          application.statusCode ===
            statusCodes.get(STATUS_CODE_ADDITIONAL_INFO_NEEDED) ||
          application.statusCode === statusCodes.get(STATUS_CODE_UNSUBMITTED)
        ) {
          if (
            currentHiApplication?.homeImprovementId !==
            application.homeImprovementId
          ) {
            setApplicationList((prev) => {
              if (application.homeImprovementId) {
                //Remove any unsaved HI application
                return prev.filter((i) => i.isSaved);
              }
              return prev;
            });
            setCurrentHiApplication(application);
            setPrevApplication({ ...application });
          }
        }
      }}
      shadow={false}
      type={'simple'}
      imageSrc={parcelInfo?.picture}
      parcelNumber={parcelInfo?.ptas_name}
      propertyName={parcelInfo?.ptas_namesonaccount}
      propertyAddress={parcelInfo?.ptas_address}
      upperStripText={
        application.statusCode &&
        application.statusCode === statusCodes.get(STATUS_CODE_APP_CREATED)
          ? fm.appliedExemption(
              new Date(application.dateApplicationReceived).toLocaleDateString()
            )
          : undefined
      }
    >
      {currentHiApplication?.homeImprovementId ===
        application.homeImprovementId &&
        !complete && (
          <Box className={classes.applicationContent}>
            <CustomTabs
              selectedIndex={
                currentStep === 'construction'
                  ? 0
                  : currentStep === 'permit'
                  ? 1
                  : currentStep === 'sign'
                  ? 2
                  : 0
              }
              ptasVariant="Switch"
              items={[
                { label: fm.tabConstruction, disabled: false },
                {
                  label: fm.tabPermit,
                  disabled: highestStepNumber < 1 && !isLocalHost(),
                },
                {
                  label: fm.tabSign,
                  disabled: highestStepNumber < 2 && !isLocalHost(),
                },
              ]}
              onSelected={(tab: number): void => {
                handleTabChange(tab);
              }}
            />
            {(!currentStep || currentStep === 'construction') && (
              <TabConstruction updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'permit' && (
              <TabPermit updateFormIsValid={updateFormIsValid} />
            )}
            {currentStep === 'sign' && (
              <TabSign updateFormIsValid={updateFormIsValid} />
            )}
            <CustomButton
              classes={{
                root:
                  currentStep === 'sign'
                    ? classes.continueButtonLarge
                    : classes.continueButton,
              }}
              disabled={!formIsValid}
              onClick={handleContinue}
              ptasVariant="Primary"
            >
              {currentStep === 'sign'
                ? fm.buttonSignFinalize
                : fm.buttonContinue}
            </CustomButton>
          </Box>
        )}
    </CardPropertyInfo>
  );
}

export default HICardPropertyInfo;
