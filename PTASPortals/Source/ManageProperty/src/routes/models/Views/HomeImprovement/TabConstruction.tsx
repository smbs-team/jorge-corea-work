// TabConstruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Box } from '@material-ui/core';
import { Theme, makeStyles } from '@material-ui/core/styles';
import clsx from 'clsx';
import React, { useCallback, useContext, useEffect } from 'react';
import {
  CustomDatePicker,
  CustomTextField,
  CustomTextarea,
  FileUpload,
  CustomFile,
  useTextFieldValidation,
  utilService,
  useDateValidation,
} from '@ptas/react-public-ui-library';
import * as fm from './formatMessage';
import { useHI } from './HomeImprovementContext';
import { v4 as uuid } from 'uuid';
import { apiService } from 'services/api/apiService';
import { FileAttachmentMetadata } from './model/fileAttachmentMetadata';
import { hiApiService } from 'services/api/apiService/homeImprovement';
import { ErrorMessageContext } from '@ptas/react-public-ui-library';
import { CHANGE_DELAY_MS } from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  construction: {
    width: 270,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    margin: theme.spacing(3, 0),
  },
  constructionLineMarginBottom: {
    marginBottom: theme.spacing(4),
  },
  constructionDates: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
  },
  constructionDate: {
    width: 129,
    backgroundColor: theme.ptas.colors.theme.white,
    borderRadius: 3,
  },
  constructionCostEstimate: {
    maxWidth: '100%',
    backgroundColor: theme.ptas.colors.theme.white,

    '& .MuiInputBase-root': {
      marginTop: 0,
    },

    '& .MuiInputAdornment-positionStart': {
      marginLeft: 8,
      marginTop: 2,
    },

    '& .MuiInput-underline:before': {
      borderBottom: 'none',
    },
  },
  constructionCostEstimateInput: {
    paddingBottom: 0,
  },
  constructionCostEstimateAnimated: {
    left: 24,
    top: -9,
  },
  constructionCostEstimateHelperText: {
    marginBottom: -16,
  },
  constructionUploadContainer: {
    width: 'fit-content',
    maxWidth: 230,
    marginBottom: theme.spacing(2),
  },
  constructionUploadLabel: {
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    lineHeight: '15px',
    marginBottom: theme.spacing(0.5),
  },
  constructionUploadRoot: {
    margin: 0,
  },
}));

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabConstruction(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    currentHiApplication,
    updateCurrentHiApplication,
    fileUploads,
    setFileUploads,
    isApplicationSaved,
    setInitApplicationFields,
    prevApplication,
    setPrevApplication,
    setCurrentHiApplication,
    setFileAttachments,
  } = useHI();
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const {
    isValid: beginDateIsValid,
    hasError: beginDateInputHasError,
    valueChangedHandler: beginDateInputChangedHandler,
    inputBlurHandler: beginDateInputBlurHandler,
  } = useDateValidation(
    currentHiApplication?.constructionBeginDate?.toString() ?? '',
    utilService.validDateStr
  );
  const {
    isValid: endDateIsValid,
    hasError: endDateInputHasError,
    valueChangedHandler: endDateInputChangedHandler,
    inputBlurHandler: endDateInputBlurHandler,
  } = useDateValidation(
    currentHiApplication?.estimatedCompletionDate?.toString() ?? '',
    utilService.validDateStr
  );
  const {
    isValid: costIsValid,
    hasError: costInputHasError,
    valueChangedHandler: costInputChangedHandler,
    inputBlurHandler: costInputBlurHandler,
  } = useTextFieldValidation(
    currentHiApplication?.estimatedConstructionCost
      ? currentHiApplication?.estimatedConstructionCost.toString()
      : '',
    utilService.isNotEmpty
  );
  const {
    isValid: descriptionIsValid,
    hasError: descriptionInputHasError,
    valueChangedHandler: descriptionInputChangedHandler,
    inputBlurHandler: descriptionInputBlurHandler,
  } = useTextFieldValidation(
    currentHiApplication?.descriptionOfTheImprovement
      ? currentHiApplication?.descriptionOfTheImprovement.toString()
      : '',
    utilService.isNotEmpty
  );

  useEffect(() => {
    if (
      beginDateIsValid &&
      endDateIsValid &&
      costIsValid &&
      descriptionIsValid
    ) {
      updateFormIsValid(true);
    } else {
      updateFormIsValid(false);
    }
  }, [
    updateFormIsValid,
    beginDateIsValid,
    endDateIsValid,
    costIsValid,
    descriptionIsValid,
  ]);

  const handleUpload = useCallback(
    async (files: CustomFile[]): Promise<CustomFile[]> => {
      if (!currentHiApplication) return [];
      const hiApp = { ...currentHiApplication };
      if (!isApplicationSaved()) {
        //First save new HI application
        setInitApplicationFields(hiApp);
        const saveRes = await hiApiService.saveHiApplication(
          hiApp,
          prevApplication
        );
        if (saveRes.hasError) {
          console.error(
            'Error saving home improvement application.',
            saveRes.errorMessage,
            hiApp
          );
          setErrorMessageState({
            open: true,
            errorHead: fm.saveApplicationError,
          });
          return [];
        } else {
          hiApp.isSaved = true;
          setPrevApplication(hiApp);
          setCurrentHiApplication(hiApp);
        }
      }
      for (const file of files) {
        if (file.file) {
          const newFileId = uuid();
          setFileUploads((prev) => [
            ...prev,
            {
              id: newFileId,
              isUploading: false,
              contentType: 'publicUrl',
              content: URL.createObjectURL(file.file),
              file: file.file,
              fileName: file.fileName,
            },
          ]);

          //Upload to file store
          const { data: url } = await apiService.uploadFile(
            file.file,
            hiApp.homeImprovementId + '/' + newFileId
          );
          //TODO: check if should set remote URL on fileUploads state
          //and clear the file attribute used to check if it's a new file
          console.log('File URL:', url);

          //Create metadata entitiy
          const fileAttachment = new FileAttachmentMetadata();
          fileAttachment.fileAttachmentMetadataId = newFileId;
          fileAttachment.name = file.fileName;
          fileAttachment.blobUrl = url ?? '';
          fileAttachment.parcelId = hiApp.parcelId;
          fileAttachment.homeImprovementApplicationId = hiApp.homeImprovementId;
          const saveAttachmentRes = await apiService.saveFileAttachment(
            fileAttachment
          );
          if (saveAttachmentRes.hasError) {
            console.error(
              'Error on save file attachment.',
              saveAttachmentRes.errorMessage
            );
          } else {
            setFileAttachments((prev) => {
              if (!prev) return [fileAttachment];
              return [...prev, fileAttachment];
            });
          }
        }
      }
      return new Promise((res) => res(files));
    },
    [
      currentHiApplication,
      isApplicationSaved,
      prevApplication,
      setCurrentHiApplication,
      setErrorMessageState,
      setFileUploads,
      setInitApplicationFields,
      setPrevApplication,
      setFileAttachments,
    ]
  );

  const handleRemoveFile = async (file: CustomFile): Promise<void> => {
    if (
      !file.id ||
      !file.fileName ||
      !currentHiApplication?.homeImprovementId
    ) {
      return;
    }
    setFileUploads((prev) => {
      return prev.filter((f) => f.id !== file.id);
    });
    const deleteAttachmentRes = await apiService.deleteFileAttachment(file.id);
    setFileAttachments((prev) => {
      return prev.filter((f) => f.fileAttachmentMetadataId !== file.id);
    });
    if (deleteAttachmentRes.hasError) {
      console.error(
        'Error on delete attachment.',
        deleteAttachmentRes.errorMessage
      );
    } else {
      const deleteFileRes = await apiService.deleteFile(
        file.fileName,
        currentHiApplication.homeImprovementId + '/' + file.id
      );
      if (deleteFileRes.hasError) {
        console.error('Error on delete blob.', deleteFileRes.errorMessage);
      }
    }
  };

  const updateCostEstimate = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    updateCurrentHiApplication(
      'estimatedConstructionCost',
      e.target.value === '' ? undefined : Number.parseFloat(e.target.value)
    );
  };

  const updateImprovementDescription = (
    e: React.ChangeEvent<HTMLTextAreaElement>
  ): void => {
    updateCurrentHiApplication('descriptionOfTheImprovement', e.target.value);
  };

  return (
    <Box className={classes.construction}>
      <Box
        className={clsx(
          classes.constructionDates,
          classes.constructionLineMarginBottom
        )}
      >
        <CustomDatePicker
          classes={{ root: classes.constructionDate }}
          label={fm.constructionStartDate}
          inputVariant="standard"
          value={currentHiApplication?.constructionBeginDate}
          onChange={(date: Date): void => {
            beginDateInputChangedHandler();
            updateCurrentHiApplication('constructionBeginDate', date);
          }}
          onBlur={beginDateInputBlurHandler}
          error={beginDateInputHasError}
          helperText={beginDateInputHasError ? fm.fieldRequired : ''}
        />
        <CustomDatePicker
          classes={{ root: classes.constructionDate }}
          label={fm.constructionEndDate}
          inputVariant="standard"
          value={currentHiApplication?.estimatedCompletionDate}
          onChange={(date: Date): void => {
            endDateInputChangedHandler();
            updateCurrentHiApplication('estimatedCompletionDate', date);
          }}
          onBlur={endDateInputBlurHandler}
          error={endDateInputHasError}
          helperText={endDateInputHasError ? fm.fieldRequired : ''}
        />
      </Box>
      <CustomTextField
        classes={{
          root: clsx(
            classes.constructionCostEstimate,
            classes.constructionLineMarginBottom
          ),
          animatedCurrency: classes.constructionCostEstimateAnimated,
          currencyInput: classes.constructionCostEstimateInput,
          helperText: classes.constructionCostEstimateHelperText, //
        }}
        ptasVariant="currency"
        label={fm.constructionCostEstimate}
        variant="standard"
        value={(
          currentHiApplication?.estimatedConstructionCost || 0
        ).toString()}
        error={costInputHasError}
        helperText={costInputHasError ? fm.fieldRequired : ''}
        onChange={(
          e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ): void => {
          costInputChangedHandler(e);
          updateCostEstimate(e);
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={costInputBlurHandler}
      />
      <CustomTextarea
        classes={{
          root: classes.constructionLineMarginBottom,
        }}
        placeholder="Improvement description" //TODO: use fm.constructionImprovementDescription when this props accepts an Element value
        value={currentHiApplication?.descriptionOfTheImprovement ?? ''}
        error={descriptionInputHasError}
        helperText={descriptionInputHasError ? fm.fieldRequired : ''}
        onChange={(e: React.ChangeEvent<HTMLTextAreaElement>): void => {
          descriptionInputChangedHandler(e);
          updateImprovementDescription(e);
        }}
        onChangeDelay={CHANGE_DELAY_MS}
        onBlur={descriptionInputBlurHandler}
      />
      <Box className={classes.constructionUploadContainer}>
        <label className={classes.constructionUploadLabel}>
          {fm.attachPhotos}
        </label>
        <FileUpload
          classes={{
            rootDropzone: classes.constructionUploadRoot,
            rootPreview: classes.constructionUploadRoot,
          }}
          attrAccept={[
            { extension: '.png', mimeType: 'image/png' },
            { extension: '.jpg', mimeType: 'image/jpeg' },
            {
              extension: '.pdf',
              mimeType: 'application/pdf',
            },
          ]}
          // maxFileSize={2048}
          dragRejectText={fm.uploadDragReject}
          bigDropzoneForAdditionalFiles={true}
          onFileUpload={handleUpload}
          currentFiles={fileUploads}
          onRemoveFile={handleRemoveFile}
          // additionalUploads={false}
          // multipleFilesUploadAtOnce={false}
        />
      </Box>
    </Box>
  );
}

export default TabConstruction;
