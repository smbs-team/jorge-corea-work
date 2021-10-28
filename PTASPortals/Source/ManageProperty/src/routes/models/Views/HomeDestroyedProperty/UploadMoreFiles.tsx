// UploadMoreFiles.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import {
  CustomButton,
  FileUpload,
  CustomFile,
  ErrorMessageContext,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import { taskService } from 'services/api/apiService/task';
import { v4 as uuid } from 'uuid';
import { FileAttachmentMetadataTask } from './fileAttachment';
import { useUpdateEffect } from 'react-use';
import { getFileFromUrl } from 'services/common/getFileFromUrl';
import PreviewFile from '../../../../components/PreviewFile';
import useDestroyedProperty from './useDestroyedProperty';
import { FILE_LIBRARY_ASR_SENIORS } from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  saveButton: {
    display: 'block',
    margin: '0 auto',
    marginTop: 48,
    width: 135,
    height: 38,
  },
  fileUploadRoot: {
    margin: 0,
    marginBottom: 16,
  },
  uploadTitle: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginBottom: 4,
    display: 'block',
  },
  uploadContentWrap: {
    margin: '0 auto',
    width: '100%',
    maxWidth: 230,
    paddingTop: 32,
    paddingBottom: 16,
  },
}));

interface Props {
  onSave?: () => void;
}

function UploadMoreFiles(props: Props): JSX.Element {
  const classes = useStyles();
  // files after submission
  const [files, setFiles] = useState<CustomFile[]>([]);
  // current file to preview
  const [fileToPreview, setFileToPreview] = useState<CustomFile>();
  // toggle preview state
  const [open, setOpen] = useState<boolean>(false);
  // bock continue button
  const [buttonDisabled, setButtonDisabled] = useState<boolean>(true);
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const {
    task,
    fileAttachmentDoc,
    loadInitialFilesByDestroyedProp,
    fileLibraryList,
  } = useDestroyedProperty();

  const saveFile = async (fileUploaded: CustomFile): Promise<string | void> => {
    const file = fileUploaded;
    if (!file?.file) return;

    const fileAttachmentId = uuid();

    const {
      data: blobUrl,
      hasError: fileStoreError,
    } = await taskService.uploadFile(file.file, fileAttachmentId);

    if (fileStoreError) {
      return setErrorMessageState({
        open: true,
        errorHead: 'Error to upload file to FileStore',
      });
    }

    const newFile = new FileAttachmentMetadataTask();
    newFile.fileAttachmentMetadataId = fileAttachmentId;
    newFile.icsDocumentId = fileAttachmentId;
    newFile.name = file.fileName;
    newFile.parcelId = task?.parcelId ?? '';
    newFile.isBlob = true;
    newFile.section = 'DocumentAfterSubmission';
    newFile.isSharePoint = false;
    newFile.blobUrl = blobUrl as string;
    newFile.originalName = file.fileName;
    if (fileLibraryList.get(FILE_LIBRARY_ASR_SENIORS)) {
      newFile.fileLibrary = fileLibraryList.get(FILE_LIBRARY_ASR_SENIORS);
    }

    const { hasError } = await taskService.saveFileAttachment(newFile);

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorHead: 'Error to create file attachment',
      });
    }

    if (!task?.id) return;

    const {
      hasError: errorRelation,
    } = await taskService.createOrDeleteFileAttachmentTaskRelation(
      task?.id,
      newFile.fileAttachmentMetadataId,
      false
    );

    if (errorRelation) {
      return setErrorMessageState({
        open: true,
        errorHead: 'Error to create relation',
      });
    }
    return blobUrl;
  };

  const handleUploadDoc = async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    const file = files[0].file;

    if (!file) return new Promise((res) => res(files));

    const id = uuid();

    const url = await saveFile({
      ...files[0],
      id,
    });

    if (!url) {
      console.log('Error to upload file');
    }

    loadInitialFilesByDestroyedProp();

    return new Promise((res) => res(files));
  };

  const removeFile = (file: CustomFile): void => {
    if (!file) return;

    const newFiles = files.filter((doc) => doc.id !== file.id);

    setFiles(newFiles);

    taskService.deleteFileAttachment(
      file.id ?? '',
      task?.id ?? '',
      file.fileName
    );
  };

  const loadFileAttachments = async (): Promise<void> => {
    let allFiles: CustomFile[] = [];

    for (const fd of fileAttachmentDoc) {
      const type = fd.blobUrl.split('.').slice(-1)[0];
      const newFile: CustomFile = {
        id: fd.fileAttachmentMetadataId,
        isUploading: false,
        contentType: 'publicUrl',
        content: fd.blobUrl,
        fileName: fd.name,
      };

      if (type === 'pdf') {
        newFile.file = await getFileFromUrl(fd.blobUrl);
      }

      allFiles = [...allFiles, newFile];
    }

    setFiles(allFiles);
  };

  useUpdateEffect(() => {
    loadFileAttachments();
  }, [fileAttachmentDoc]);

  useUpdateEffect(() => {
    setButtonDisabled(!files.length);
  }, [files]);

  const handleOnSave = async (): Promise<void> => {
    await props.onSave?.();
    loadInitialFilesByDestroyedProp();
  };

  //#endregion files after submission
  return (
    <div className={classes.uploadContentWrap}>
      <span className={classes.uploadTitle}>{fm.uploadTitle}</span>

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
        onFileUpload={handleUploadDoc}
        dragRejectText={fm.uploadDragReject}
        bigDropzoneForAdditionalFiles={true}
        currentFiles={files}
        onRemoveFile={removeFile}
        onZoomIn={(file): void => {
          setFileToPreview(file);
          setOpen(true);
        }}
      />
      <CustomButton
        onClick={handleOnSave}
        ptasVariant="Primary"
        classes={{ root: classes.saveButton }}
        disabled={buttonDisabled}
      >
        {generalFm.save}
      </CustomButton>
      <PreviewFile
        onClose={(): void => setOpen(false)}
        open={open}
        file={fileToPreview}
      />
    </div>
  );
}

export default UploadMoreFiles;
