// Destruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useCallback } from 'react';
import { makeStyles, Theme, Collapse, FormHelperText } from '@material-ui/core';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import {
  CustomDatePicker,
  CustomSwitch,
  CustomTextField,
  FileUpload,
  CustomFile,
  ErrorMessageContext,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';
import { apiService } from 'services/api/apiService';
import { useUpdateEffect } from 'react-use';
import { OptionSetValue } from '../HomeImprovement/model/optionSet';
import { v4 as uuid } from 'uuid';
import { taskService } from 'services/api/apiService/task';
import FilePreview from '../../../../components/PreviewFile';
import { FileAttachmentMetadataTask } from './fileAttachment';
import { getFileFromUrl } from 'services/common/getFileFromUrl';
import utilService from 'services/utilService';
import useDestroyedProperty from './useDestroyedProperty';
import {
  DOCUMENT_SECTION_DESTRUCTION,
  FILE_LIBRARY_ASR_SENIORS,
} from './constants';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface Props {}

type FileSection = 'fire' | 'natural disaster' | 'voluntary demolition';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    width: '100%',
    maxWidth: 270,
    '& .MuiGrid-justify-xs-space-around': {
      justifyContent: 'start',
    },
    paddingBottom: 33,
  },
  otherInput: {
    maxWidth: '100%',
    backgroundColor: theme.ptas.colors.theme.white,
    marginTop: 12,
  },
  destructionDate: {
    width: 129,
    marginTop: 25,
  },
  datePickerInput: {
    backgroundColor: theme.ptas.colors.theme.white,
  },
  fileUploadRoot: {
    margin: '0 0 17px auto',
  },
  dateOfDestructionLabel: {
    fontSize: theme.ptas.typography.finePrint.fontSize,
    zIndex: 10,
  },
  datePickerShrink: {
    color: `${theme.ptas.colors.theme.black} !important`,
    paddingRight: 4,
  },
  labelTitle: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    textAlign: 'start',
  },
  labelDesc: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.gray,
    display: 'block',
    lineHeight: '20px',
    marginBottom: 12,
  },
  marginBottomLabel: {
    marginBottom: 22,
  },
  title: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.black,
    marginBottom: 9,
    display: 'block',
  },
  marginBottomLabelDesc: {
    marginBottom: 12,
  },
  inputUploadMarginBottom: {
    marginBottom: 17,
  },
  helperText: {
    color: theme.ptas.colors.utility.danger,
    paddingLeft: 9,
  },
}));

function Destruction(props: Props): JSX.Element {
  const classes = useStyles(props);

  const {
    propDestroyedReason,
    setPropDestroyedReason,
    filePreview,
    setFilePreview,
    openPreviewFileModal,
    setOpenPreviewFileModal,
    // destructionFiles,
    // setDestructionFiles,
    lossOccurringAsAResultOfError,
    setLossOccurringAsAResultOf,
    dateOfDestructionInputError,
    setDateOfDestructionInputError,
    task,
    saveTask,
    filesAttachmentMetadataDestruction,
    savePartialTask,
    setFireCheck,
    fireCheck,
    naturalDisasterCheck,
    setNaturalDisasterCheck,
    voluntaryDemolitionCheck,
    setVoluntaryDemolitionCheck,
    fileLibraryList,
    setFireFile,
    fireFile,
    setNaturalDisasterFile,
    naturalDisasterFile,
  } = useDestroyedProperty();

  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const handleSetFire = (isChecked: boolean): void => {
    setFireCheck(isChecked);
    addOrRemoveDestroyedReason(isChecked, 'Fire');
  };

  const handleSetNaturalDisaster = (isChecked: boolean): void => {
    setNaturalDisasterCheck(isChecked);
    addOrRemoveDestroyedReason(isChecked, 'Natural disaster');
  };

  const handleSetVoluntaryDemolition = (isChecked: boolean): void => {
    setVoluntaryDemolitionCheck(isChecked);
    addOrRemoveDestroyedReason(isChecked, 'New Construction');
  };

  const handleChangeOther = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const value = e.target.value;

    saveTask('othercomments', value);
  };

  useUpdateEffect(() => {
    addOrRemoveDestroyedReason(!!task?.othercomments, 'Other');
  }, [task?.othercomments]);

  const addOrRemoveDestroyedReason = (
    add: boolean,
    reasonName: string
  ): void => {
    const attributeValue = propDestroyedReason.find(
      (reason) => reason.value === reasonName
    )?.attributeValue;

    const optSelected = task?.lossOccurringAsAResultOf;

    const options = !optSelected
      ? []
      : optSelected.split(',').map((reason) => {
          return Number.isNaN(parseInt(reason)) ? 0 : parseInt(reason);
        });

    if (add) {
      if (!attributeValue) return;

      if (options.includes(attributeValue)) return;
      const newOptions = [...options, attributeValue].join(',');
      return saveTask('lossOccurringAsAResultOf', newOptions);
    }

    const newReasonList = options
      .filter((reasonId) => attributeValue !== reasonId)
      .join(',');

    saveTask('lossOccurringAsAResultOf', newReasonList);
  };

  useUpdateEffect(() => {
    setLossOccurringAsAResultOf(!task?.lossOccurringAsAResultOf);
  }, [task?.lossOccurringAsAResultOf]);

  const loadFileAttachments = useCallback(async (): Promise<void> => {
    const handleFile = async (
      fa: FileAttachmentMetadataTask
    ): Promise<CustomFile> => {
      const type = fa.blobUrl.split('.').slice(-1)[0];

      const newFile: CustomFile = {
        id: fa.fileAttachmentMetadataId,
        isUploading: false,
        contentType: 'publicUrl',
        content: fa.blobUrl,
        fileName: fa.name,
      };

      if (type === 'pdf') {
        newFile.file = await getFileFromUrl(fa.blobUrl);
      }

      return newFile;
    };

    const {
      customFileFire,
      customNaturalDisaster,
    } = filesAttachmentMetadataDestruction.reduce(
      (acc, fa) => {
        if (fa.document === 'fire') {
          return {
            ...acc,
            customFileFire: [
              ...acc.customFileFire,
              handleFile(fa),
            ] as Promise<CustomFile>[],
          };
        }

        if (fa.document === 'natural disaster') {
          return {
            ...acc,
            customNaturalDisaster: [
              ...acc.customNaturalDisaster,
              handleFile(fa),
            ] as Promise<CustomFile>[],
          };
        }

        return acc;
      },
      {
        customFileFire: [] as Promise<CustomFile>[],
        customNaturalDisaster: [] as Promise<CustomFile>[],
      }
    );

    const fireFileResult = await Promise.all(customFileFire).catch(() => []);
    const naturalDisasterFileResult = await Promise.all(
      customNaturalDisaster
    ).catch(() => []);

    setFireFile(fireFileResult);
    setNaturalDisasterFile(naturalDisasterFileResult);
  }, [filesAttachmentMetadataDestruction, setFireFile, setNaturalDisasterFile]);

  useEffect(() => {
    loadFileAttachments();
  }, [loadFileAttachments]);

  const removeFile = (document: FileSection) => async (
    file: CustomFile
  ): Promise<void> => {
    if (!file) return;

    if (document === 'fire') {
      const newFiles = fireFile.filter((f) => f.id !== file.id);
      setFireFile(newFiles);
    }

    if (document === 'natural disaster') {
      const newFiles = naturalDisasterFile.filter((f) => f.id !== file.id);
      setNaturalDisasterFile(newFiles);
    }

    const fireFileAttachment =
      [...fireFile, ...naturalDisasterFile].find((fa) => fa.id === file.id)
        ?.id ?? '';

    await taskService.deleteFileAttachment(
      fireFileAttachment,
      task?.id ?? '',
      file.fileName
    );
  };

  const handleUploadFiles = (name: FileSection) => async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    const file = files[0];
    if (!file?.file) return [];

    // // create a temporary file to display it correctly in the ui
    const tempFile = {
      id: 'tempFile',
      isUploading: true,
      contentType: 'publicUrl',
      fileName: file.fileName,
      file: file.file,
    } as CustomFile;

    if (name === 'fire') {
      setFireFile((prev) => {
        return [...prev, tempFile];
      });
    }

    if (name === 'natural disaster') {
      setNaturalDisasterFile((prev) => {
        return [...prev, tempFile];
      });
    }

    await savePartialTask();

    const fileInfo = await saveFile(file, name);

    if (!fileInfo) return [];

    const newFile = {
      id: fileInfo.fileAttachmentId,
      isUploading: false,
      contentType: 'publicUrl',
      content: fileInfo.blobUrl ?? '',
      fileName: file.fileName,
      file: file.file,
    } as CustomFile;

    if (name === 'fire') {
      setFireFile((prev) => [
        ...prev.filter((f) => f.id !== 'tempFile'),
        newFile,
      ]);
    }

    if (name === 'natural disaster') {
      setNaturalDisasterFile((prev) => [
        ...prev.filter((f) => f.id !== 'tempFile'),
        newFile,
      ]);
    }

    return [newFile];
  };

  const FireInputUpload = (
    <Collapse in={fireCheck}>
      <FileUpload
        classes={{
          rootDropzone: classes.fileUploadRoot,
          rootPreview: classes.fileUploadRoot,
        }}
        attrAccept={[
          { extension: '.png', mimeType: 'image/png' },
          { extension: '.jpg', mimeType: 'image/jpeg' },
          {
            extension: '.xlsx',
            mimeType:
              'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          },
          {
            extension: '.docx',
            mimeType:
              'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
          },
          {
            extension: '.pdf',
            mimeType: 'application/pdf',
          },
        ]}
        dragRejectText={fm.uploadDragReject}
        bigDropzoneForAdditionalFiles={true}
        additionalUploads={false}
        onFileUpload={handleUploadFiles('fire')}
        currentFiles={fireFile}
        onRemoveFile={removeFile('fire')}
        onZoomIn={(file): void => {
          setFilePreview(file);
          setOpenPreviewFileModal(true);
        }}
      />
    </Collapse>
  );

  const naturalDisasterInputUpload = (
    <Collapse in={naturalDisasterCheck}>
      <FileUpload
        classes={{
          rootDropzone: classes.fileUploadRoot,
          rootPreview: classes.fileUploadRoot,
        }}
        attrAccept={[
          { extension: '.png', mimeType: 'image/png' },
          { extension: '.jpg', mimeType: 'image/jpeg' },
          {
            extension: '.xlsx',
            mimeType:
              'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          },
          {
            extension: '.docx',
            mimeType:
              'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
          },
          {
            extension: '.pdf',
            mimeType: 'application/pdf',
          },
        ]}
        dragRejectText={fm.uploadDragReject}
        bigDropzoneForAdditionalFiles={true}
        onFileUpload={handleUploadFiles('natural disaster')}
        additionalUploads={false}
        currentFiles={naturalDisasterFile}
        onRemoveFile={removeFile('natural disaster')}
        onZoomIn={(file): void => {
          setFilePreview(file);
          setOpenPreviewFileModal(true);
        }}
      />
    </Collapse>
  );

  const FireInputLabel = (
    <Fragment>
      <span className={classes.labelTitle}>{fm.fire}</span>
      <span className={classes.labelDesc}>{fm.fireDesc}</span>
    </Fragment>
  );

  const NaturalDisasterInputLabel = (
    <Fragment>
      <span className={classes.labelTitle}>{fm.naturalDisaster}</span>
      <span className={classes.labelDesc}>{fm.naturalDisasterDesc}</span>
    </Fragment>
  );

  const voluntaryDemolitionInputLabel = (
    <span className={clsx(classes.labelTitle, classes.marginBottomLabel)}>
      {fm.voluntaryDemolition}
    </span>
  );

  const handleChangeDate = (date: Date): void => {
    const validFormat = utilService.validDate(date);

    const dateInRange = utilService.validDateInRange(date);

    setDateOfDestructionInputError(!dateInRange);

    saveTask('dateOfDestruction', validFormat ? date.toISOString() : '');
  };

  const getPropertyOptSet = async (): Promise<void> => {
    const { hasError, data } = await apiService.getOptionSet(
      'ptas_task',
      'ptas_lossoccurringasaresultof'
    );

    if (hasError) {
      setErrorMessageState({
        open: true,
      });
    }

    if (!data) return;

    setInitialsReasons(data);
    setPropDestroyedReason(data);
  };

  const setInitialsReasons = (options: OptionSetValue[]): void => {
    const lossOccurringData = (
      task?.lossOccurringAsAResultOf.split(',') || []
    ).map((reason) => parseInt(reason));

    const optionSelected = options.filter(({ attributeValue }) =>
      lossOccurringData.includes(attributeValue)
    );

    for (const opt of optionSelected) {
      const label = opt.value.toLowerCase();
      if (label === 'fire') setFireCheck(true);
      else if (label === 'natural disaster') setNaturalDisasterCheck(true);
      else if (label === 'new construction') setVoluntaryDemolitionCheck(true);
    }
  };

  const saveFile = async (
    fileUploaded: CustomFile,
    inputName: string
  ): Promise<{
    blobUrl?: string;
    fileAttachmentId: string;
  } | void> => {
    if (!fileUploaded?.file) return;

    const fileAttachmentId = uuid();

    const {
      data: blobUrl,
      hasError: fileStoreError,
    } = await taskService.uploadFile(fileUploaded.file, fileAttachmentId);

    if (fileStoreError) {
      return setErrorMessageState({
        open: true,
        errorHead: 'Error to upload file to FileStore',
      });
    }

    const newFile = new FileAttachmentMetadataTask();
    newFile.fileAttachmentMetadataId = fileAttachmentId;
    newFile.icsDocumentId = fileAttachmentId;
    newFile.name = fileUploaded.fileName;
    newFile.parcelId = task?.parcelId ?? '';
    newFile.section = DOCUMENT_SECTION_DESTRUCTION;
    newFile.document = inputName;
    newFile.isBlob = true;
    newFile.isSharePoint = false;
    newFile.blobUrl = blobUrl as string;
    newFile.originalName = fileUploaded.fileName;
    if (fileLibraryList.get(FILE_LIBRARY_ASR_SENIORS)) {
      newFile.fileLibrary = fileLibraryList.get(FILE_LIBRARY_ASR_SENIORS);
    }

    if (!task?.id) return;

    const saveFileAttach = taskService.saveFileAttachment(newFile);

    const hasError = await Promise.all([saveFileAttach]).then(([{ data }]) => {
      if (!data) return true;

      taskService.createOrDeleteFileAttachmentTaskRelation(
        task?.id,
        newFile.fileAttachmentMetadataId,
        false
      );
      return false;
    });

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorHead: 'Error to create file attachment',
      });
    }

    return { blobUrl, fileAttachmentId: newFile.fileAttachmentMetadataId };
  };

  useEffect(() => {
    getPropertyOptSet();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const HelperTextError = lossOccurringAsAResultOfError && (
    <FormHelperText className={classes.helperText}>
      {fm.selectAnOption}
    </FormHelperText>
  );

  return (
    <div className={classes.root}>
      <CustomDatePicker
        label={fm.datePickerLabel}
        placeholder="mm/dd/yyy"
        classes={{
          root: classes.destructionDate,
          animated: classes.dateOfDestructionLabel,
          shrink: classes.datePickerShrink,
          input: classes.datePickerInput,
        }}
        helperText={
          dateOfDestructionInputError
            ? fm.enterAValidDate
            : fm.datePickerLabelHelperText
        }
        inputVariant="standard"
        onChange={handleChangeDate}
        error={dateOfDestructionInputError}
        value={new Date(task?.dateOfDestruction ?? '')}
      />
      {<span className={classes.title}>{fm.destroyedTitle}</span>}
      <CustomSwitch
        label={FireInputLabel}
        ptasVariant="small"
        showOptions
        isChecked={handleSetFire}
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        value={fireCheck}
      />
      {FireInputUpload}
      <CustomSwitch
        label={NaturalDisasterInputLabel}
        ptasVariant="small"
        showOptions
        isChecked={handleSetNaturalDisaster}
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        value={naturalDisasterCheck}
      />
      {naturalDisasterInputUpload}
      <CustomSwitch
        label={voluntaryDemolitionInputLabel}
        ptasVariant="small"
        showOptions
        isChecked={handleSetVoluntaryDemolition}
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        value={voluntaryDemolitionCheck}
      />
      {/* {voluntaryDemolitionInputUpload} */}
      <CustomTextField
        classes={{
          root: classes.otherInput,
        }}
        onChangeDelay={500}
        ptasVariant="overlay"
        label={fm.other}
        onChange={handleChangeOther}
        value={task?.othercomments}
        // showIcon
      />
      {HelperTextError}
      <FilePreview
        onClose={(): void => setOpenPreviewFileModal(false)}
        open={openPreviewFileModal}
        file={filePreview}
      />
    </div>
  );
}

export default Destruction;
