// SimpleCardPropertyInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState, useContext } from 'react';
import {
  CardPropertyInfo,
  CustomTabs,
  CustomButton,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import DestructionTab from './Destruction';
import Property from './Property';
import Sign from './Sign';
import { Task } from './Task';
import { taskService } from 'services/api/apiService/task';
import { FileAttachmentMetadataTask } from './fileAttachment';
import { useUpdateEffect } from 'react-use';
import utilService from 'services/utilService';
import { apiService } from 'services/api/apiService';
import { isEqual } from 'lodash';
import UploadMoreFiles from './UploadMoreFiles';
import useDestroyedProperty from './useDestroyedProperty';
import { AppContext } from 'contexts/AppContext';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 8,
    marginBottom: 15,
    maxWidth: 702,
    marginLeft: 'auto',
    marginRight: 'auto',
    overflow: 'visible',
  },
  cardPropertyInfo: {
    width: '100%',
    maxWidth: 623,
    marginBottom: theme.spacing(4),
  },
  cardPropertyInfoContent: {
    background: 'rgba(0,0,0,0)',
  },
  cardPropertyInfoChildrenContent: {
    background: 'rgba(255, 255, 255, 0.5)',
    boxSizing: 'border-box',
    padding: 0,
  },
  cardChildren: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    width: '100%',
    padding: 16,

    '& > div.MuiBox-root': {
      width: '100%',
    },
  },
  continueButton: {
    width: 135,
    height: 38,
  },
  signAndFinalizeButton: {
    width: 191,
    height: 38,
  },
  hide: {
    display: 'none',
  },
}));

export type DestroyedPropertyStep =
  | 'destruction'
  | 'property'
  | 'sign'
  | undefined;

export type HomeDestroyedPropertyExemption = {
  id: string;
  parcelNumber: string;
  completed: boolean;
  propertyAddress: string;
  propertyName: string;
  imageSrc: string;
};

export type DestroyedPropertyParams = {
  contactId?: string;
  destroyedPropertyId?: string;
  step?: DestroyedPropertyStep;
};

interface Props
  extends HomeDestroyedPropertyExemption,
    DestroyedPropertyParams {
  selected?: boolean;
  onClick?: (id: string) => void;
  onCompleted?: (id: string) => void;
  hideUploadMoreFileInput?: boolean;
  upperStripText?: JSX.Element;
}

function SimpleCardPropertyInfo(props: Props): JSX.Element {
  const classes = useStyles();
  const {
    contactId,
    setContactId,
    currentExemptionId,
    setCurrentExemptionId,
    continueButtonDisabled,
    setContinueButtonDisabled,
    task,
    setTask,
    oldDesPropertyApp,
    statusCode,
    savePartialTask,
    filesAttachmentMetadataDestruction,
    fileAttachmentDoc,
    jsonSteps,
    setJsonSteps,
    setOldDesPropertyApp,
    getAppsByContact,
    setDestroyedPropertyList,
    setFireCheck,
    setNaturalDisasterCheck,
    setVoluntaryDemolitionCheck,
    setInitialApps,
  } = useDestroyedProperty();
  const { portalContact } = useContext(AppContext);

  const [isCompleted, setIsCompleted] = useState<boolean>(false);
  const [currentStep, setCurrentStep] = useState<DestroyedPropertyStep>(
    'destruction'
  );
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);

  useEffect(() => {
    if (isCompleted) return;

    if (props.contactId) {
      setContactId(props.contactId);
    }

    if (props.destroyedPropertyId) {
      setCurrentExemptionId(props.destroyedPropertyId);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.contactId, props.destroyedPropertyId, props.completed]);

  useEffect(() => {
    redirectPage(currentExemptionId ?? '');
    // eslint-disable-next-line
  }, [contactId, currentExemptionId, currentStep]);

  useUpdateEffect(() => {
    if (isCompleted || !currentStep || !task) return;

    if (currentStep === 'destruction') {
      if (
        !utilService.validDateInRange(
          new Date(task?.dateOfDestruction ?? '')
        ) ||
        !task?.lossOccurringAsAResultOf
      ) {
        return setContinueButtonDisabled(true);
      }

      setContinueButtonDisabled(false);
    } else if (currentStep === 'property') {
      if (!task?.destroyedPropertyDescription) {
        return setContinueButtonDisabled(true);
      }

      setContinueButtonDisabled(false);
    } else if (currentStep === 'sign') {
      if (
        !utilService.validEmail(task?.email ?? '') ||
        !utilService.validPhone(task?.phoneNumber ?? '') ||
        // !task?.taxPayerName ||
        !utilService.validDateInRange(
          new Date(task?.dateOfDestruction ?? '')
        ) ||
        !task?.lossOccurringAsAResultOf ||
        !task?.destroyedPropertyDescription
      ) {
        return setContinueButtonDisabled(true);
      }

      setContinueButtonDisabled(false);
    }
  }, [currentStep, task]);

  const redirectPage = (exemptionId: string): void => {
    if (contactId && exemptionId && currentStep && isCompleted) {
      window.history.replaceState(
        null,
        '',
        `/home-destroyed-property/${contactId}/${exemptionId}/${currentStep}`
      );
    } else if (contactId && exemptionId) {
      window.history.replaceState(
        null,
        '',
        `/home-destroyed-property/${contactId}/${exemptionId}`
      );
    } else if (contactId) {
      window.history.replaceState(
        null,
        '',
        `/home-destroyed-property/${contactId}`
      );
    }
  };

  useEffect(() => {
    setIsCompleted(props.completed);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.completed]);

  const handleTabChange = (tab: number): void => {
    if (tab === 1) {
      setCurrentStep('property');
    } else if (tab === 2) {
      setCurrentStep('sign');
    } else {
      setCurrentStep('destruction');
    }
  };

  const renderStepChildren = (): JSX.Element => {
    switch (currentStep) {
      case 'destruction':
        return <DestructionTab />;
      case 'property':
        return <Property />;
      case 'sign':
        return <Sign />;
      default:
        return <Fragment />;
    }
  };

  const selectedTab = (): number => {
    if (currentStep === 'property') {
      return 1;
    } else if (currentStep === 'sign') {
      return 2;
    } else {
      return 0;
    }
  };

  const saveDestroyedProperty = async (): Promise<void> => {
    if (!task?.parcelId) return;

    const savedTask: Task = {
      ...task,
      statusCode: statusCode.get('Assigned') ?? null,
      dateSigned: new Date().toISOString(),
    };

    // save task
    if (!savedTask) return;

    // set portal contact on task
    const { hasError } = await taskService.saveTaskApp(
      savedTask,
      oldDesPropertyApp
    );

    if (hasError) {
      return console.log('Error to save task');
    }

    // delete route from json store
    const { hasError: jsonError } = await apiService.deleteJson(
      `portals/${contactId}/${props.id}/step/step`
    );

    if (jsonError) {
      console.log('Error to delete json');
    }

    if (filesAttachmentMetadataDestruction.length) {
      console.log('destruction files moved');
      // moved destruction files to sharepoint
      moveBlobToSharePoint(filesAttachmentMetadataDestruction);
    }

    // load new applications
    const currentApps = await getAppsByContact(portalContact?.id || '');

    setDestroyedPropertyList(currentApps || []);
    setInitialApps(currentApps || []);

    setTask(new Task());
    setOldDesPropertyApp(new Task());
    setIsCompleted(true);
    setFireCheck(false);
    setNaturalDisasterCheck(false);
    setVoluntaryDemolitionCheck(false);
  };

  const moveBlobToSharePoint = async (
    files: FileAttachmentMetadataTask[]
  ): Promise<void> => {
    const desFile = files.map((fd) => fd.fileAttachmentMetadataId);

    const {
      data,
      hasError: sharepointError,
    } = await apiService.saveFileInSharePoint(
      desFile,
      process.env.REACT_APP_BLOB_CONTAINER_TASK ?? ''
    );

    if (sharepointError) {
      return console.log('error to move blob to sharepoint');
    }

    if (!data) return;

    for (const updatedFile of data) {
      const updatedFileAttachment = new FileAttachmentMetadataTask();
      updatedFileAttachment.isBlob = false;
      updatedFileAttachment.isSharePoint = true;
      updatedFileAttachment.sharepointUrl = updatedFile.fileUrl;
      updatedFileAttachment.fileAttachmentMetadataId = updatedFile.id;

      const { hasError } = await taskService.updateFileAttachmentMetadata(
        updatedFileAttachment
      );
      if (hasError) {
        console.log('error to update fileattachment');
      }
    }
  };

  const handleClickContinue = async (): Promise<void> => {
    if (continueButtonDisabled || !currentStep) return;

    if (currentStep !== 'sign') await savePartialTask();

    switch (currentStep) {
      case 'destruction':
        setCurrentStep('property');
        if (highestStepNumber < 1) setHighestStepNumber(1);
        break;
      case 'property':
        setCurrentStep('sign');
        if (highestStepNumber < 2) setHighestStepNumber(2);
        break;
      case 'sign':
        await saveDestroyedProperty();
        break;
      default:
        break;
    }
  };

  const handleClick = (id: string): void => {
    if (props.onClick) props?.onClick(id);

    if (props.completed) {
      // update step in url
      setCurrentStep(undefined);
    }

    if (props.selected) return;

    redirectPage(props.id);
  };

  useEffect(() => {
    if (props.selected) {
      redirectPage(props.id);
    }

    if (props.selected && !props.completed && contactId) {
      getStep(props.id);
    }

    // eslint-disable-next-line
  }, [props.selected, contactId]);

  const getStep = async (id: string): Promise<void> => {
    if (!contactId) return;

    const { data: appsFound } = await taskService.getDestroyedPropByContact(
      contactId
    );

    if (!appsFound) return;

    const appFound = appsFound.find((app) => app.id === props.id);

    if (!appFound) {
      console.log('apps not found');
      setCurrentStep('destruction');
      setHighestStepNumber(0);
      return;
    }

    const { hasError, data } = await apiService.getJson(
      `portals/${contactId}/${id}/step/step`
    );

    if (hasError) {
      return console.log('Error to get step');
    }

    if (!data) return;

    const highestStep = data.highestStep ? (data.highestStep as number) : 0;
    const currentStep = data.currentStep
      ? (data.currentStep as DestroyedPropertyStep)
      : 'destruction';

    setJsonSteps({
      highestStep,
      currentStep,
    });

    setCurrentStep(currentStep);
    setHighestStepNumber(highestStep);
  };

  const saveStep = async (
    highestStep: number,
    currentStep: string
  ): Promise<void> => {
    const steps = {
      highestStep,
      currentStep,
    };

    setJsonSteps(steps);

    const { hasError } = await apiService.saveJson(
      `portals/${contactId}/${props.id}/step`,
      steps
    );

    if (hasError) {
      console.log('Error to save JSON');
    }
  };

  useUpdateEffect(() => {
    if (!currentStep || !highestStepNumber) return;

    const compareJson = {
      highestStep: highestStepNumber,
      currentStep,
    };

    if (isEqual(compareJson, jsonSteps)) return;

    saveStep(highestStepNumber, currentStep);
  }, [highestStepNumber, currentStep]);

  const renderStep = (): JSX.Element => {
    if (!props.selected || isCompleted) return <Fragment />;

    return (
      <div className={classes.cardChildren}>
        <CustomTabs
          ptasVariant="Switch"
          selectedIndex={selectedTab()}
          items={[
            { label: fm.destruction, disabled: false },
            {
              label: fm.property,
              disabled: highestStepNumber < 1,
            },
            {
              label: fm.sign,
              disabled: highestStepNumber < 2,
            },
          ]}
          onSelected={(tab: number): void => {
            handleTabChange(tab);
          }}
        />
        {renderStepChildren()}
        <CustomButton
          onClick={handleClickContinue}
          ptasVariant="Primary"
          classes={{
            root:
              currentStep === 'sign'
                ? classes.signAndFinalizeButton
                : classes.continueButton,
          }}
          disabled={continueButtonDisabled}
        >
          {currentStep !== 'sign'
            ? generalFm.continueText
            : generalFm.signAndFinalize}
        </CustomButton>
      </div>
    );
  };

  const renderUploadInput = (): JSX.Element => {
    if (!props.selected || !isCompleted || props.hideUploadMoreFileInput)
      return <Fragment />;

    return (
      <UploadMoreFiles
        onSave={async (): Promise<void> => {
          await moveBlobToSharePoint(fileAttachmentDoc);
          setTask(new Task());
        }}
      />
    );
  };

  //#endregion documents after submission
  return (
    <CardPropertyInfo
      imageSrc={props.imageSrc}
      id={props.id}
      parcelNumber={props.parcelNumber}
      propertyName={props.propertyName}
      propertyAddress={props.propertyAddress}
      type="simple"
      onClick={handleClick}
      classes={{
        root: classes.cardPropertyInfo,
        cardContent: classes.cardPropertyInfoContent,
        content:
          props.selected || !isCompleted
            ? classes.cardPropertyInfoChildrenContent
            : !props.selected || isCompleted
            ? classes.hide
            : classes.cardPropertyInfoChildrenContent,
      }}
      upperStripText={isCompleted ? props.upperStripText : undefined}
    >
      {renderStep()}
      {renderUploadInput()}
    </CardPropertyInfo>
  );
}

export default SimpleCardPropertyInfo;
