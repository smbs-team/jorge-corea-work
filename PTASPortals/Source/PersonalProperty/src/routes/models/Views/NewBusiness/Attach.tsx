// Attach.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  FileUpload,
  CustomFile,
  CustomTextButton,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { makeStyles, Theme } from '@material-ui/core';
import clsx from 'clsx';
import { v4 as uuid } from 'uuid';
import { apiService } from 'services/api/apiService';
import useNewBusiness from './useNewBusiness';

const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: '100%',
    maxWidth: 510,
    marginBottom: 40,
  },
  title: {
    display: 'block',
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 8,
    textAlign: 'center',
  },
  description: {
    display: 'block',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.body.fontSize,
    marginBottom: 24,
    textAlign: 'center',

    '& > a': {
      textDecoration: 'none',
      color: theme.ptas.colors.theme.accent,
      '&::hover': {
        textDecoration: 'none',
        color: theme.ptas.colors.theme.accent,
      },
    },
  },
  fileUploadWrap: {
    width: 230,
    margin: '0 auto',
  },
  rootDropzoneAttach: {
    margin: '8px 0',
  },
  uploadDescription: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  download: {
    color: theme.ptas.colors.theme.accent,
  },
  instruction: {
    color: 'rgba(0, 0, 0, 0.5)',
    marginRight: 4,
  },
  rootPreview: {
    margin: 0,
    marginTop: 8,
  },
  rootImportData: {
    margin: '0 auto',
    display: 'block',
  },
}));

function Attach(): JSX.Element {
  const classes = useStyles();

  const { filesAttach, setFilesAttach, setNewBusiness } = useNewBusiness();

  const saveFile = async (fileUploaded: CustomFile): Promise<string | void> => {
    const file = fileUploaded;
    if (!file?.file) return;

    const fileAttachmentId = uuid();

    const { data: blobUrl } = await apiService.uploadFile(
      file.file,
      fileAttachmentId
    );

    return blobUrl;
  };

  const handleUpload = async (files: CustomFile[]): Promise<CustomFile[]> => {
    const file = files[0].file;
    const customFile = files[0];

    if (!file) return new Promise((res) => res(files));

    const url = await saveFile(customFile);

    if (!url) {
      console.log('Error to upload file');
    }

    const id = uuid();

    const newFile: CustomFile = {
      id,
      isUploading: false,
      contentType: 'publicUrl',
      content: url || '',
      fileName: customFile.fileName,
      file,
    };

    setFilesAttach((prev) => {
      return [...prev, newFile];
    });

    setNewBusiness((prev) => ({
      ...prev,
      attachId: id,
      attachName: customFile.fileName,
      attachUrl: url || '',
    }));

    return new Promise((res) => res(files));
  };

  const removeFile = (file: CustomFile): void => {
    if (!file) return;

    const newFiles = filesAttach.filter((doc) => doc.id !== file.id);

    setFilesAttach(newFiles);

    setNewBusiness((prev) => ({
      ...prev,
      attachId: '',
      attachName: '',
      attachUrl: '',
    }));

    apiService.deleteFile(file.fileName ?? '', file.id ?? '');
  };

  return (
    <div className={classes.contentWrap}>
      <span className={classes.title}>{fm.haveTheNextFew}</span>
      <span className={classes.description}>{fm.importYourInfo}</span>
      <div className={classes.fileUploadWrap}>
        <span className={classes.uploadDescription}>
          {fm.attachListingInfo}
        </span>
        <FileUpload
          attrAccept={[
            {
              extension: '.xlsx',
              mimeType:
                'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            },
          ]}
          // maxFileSize={2048}
          dragRejectText={fm.uploadDragReject}
          bigDropzoneForAdditionalFiles={false}
          onFileUpload={handleUpload}
          currentFiles={filesAttach}
          onRemoveFile={removeFile}
          classes={{
            rootDropzone: classes.rootDropzoneAttach,
            rootPreview: classes.rootPreview,
          }}
          additionalUploads={false}
          // multipleFilesUploadAtOnce={false}
        />
        <span className={clsx(classes.uploadDescription, classes.instruction)}>
          {fm.instructions}:
        </span>
        <CustomTextButton ptasVariant="Text more">
          {fm.PTMSToExcel}
        </CustomTextButton>
      </div>
    </div>
  );
}

export default Attach;
