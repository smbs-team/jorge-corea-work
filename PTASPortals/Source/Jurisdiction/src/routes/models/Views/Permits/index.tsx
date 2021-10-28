// Profile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomCard,
  CustomTextButton,
  FileUpload,
  CustomPopover,
  CustomButton,
  OfficeFilePreview,
  Banner,
} from '@ptas/react-public-ui-library';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import { useStyles } from './styles';
import { Collapse } from '@material-ui/core';
import clsx from 'clsx';
import usePermit from './usePermit';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import { Link } from 'react-router-dom';

const Permits = (): JSX.Element => {
  const classes = useStyles();
  const {
    anchorElement,
    excelResultInfo,
    files,
    handleDownload,
    handleOpenModal,
    handlePreviewFile,
    handleRemoveFile,
    handleSavePermits,
    handleUploadFile,
    openInstructionCard,
    xlsxFileSend,
    tabRef,
    closePopover,
  } = usePermit();

  const renderInstructionCard = (): JSX.Element => {
    return (
      <Collapse in={openInstructionCard}>
        <div className={classes.instructionCardWrap}>
          <span className={classes.instructionTitle}>
            {fm.instructionTitle}
          </span>
          <ol className={classes.instructionList}>
            <li className={classes.instructionListItem}>
              {fm.useCurrentTemplate}
            </li>
            <li className={classes.instructionListItem}>{fm.selectOrDrag}</li>
            <li className={classes.instructionListItem}>
              {fm.reviewForErrors}
            </li>
            <li className={classes.instructionListItem}>{fm.savePermits}</li>
          </ol>
        </div>
      </Collapse>
    );
  };

  const renderPreviewFileHeader = (): JSX.Element => {
    if (xlsxFileSend || !excelResultInfo) return <Fragment />;

    return (
      <Fragment>
        <div className={classes.textWrap}>
          <span className={classes.headerTitle}>{fm.reviewImport}</span>
          <span className={classes.headerDescription}>
            {fm.reviewDescription}
          </span>
        </div>
        <CustomButton
          classes={{
            root: clsx(
              classes.saveButton,
              excelResultInfo.hasErrors && classes.errorButton
            ),
          }}
          ptasVariant={excelResultInfo.hasErrors ? 'Danger' : 'Primary'}
          onClick={handleSavePermits}
        >
          {excelResultInfo.hasErrors ? fm.savePermitsError : fm.savePermits}
        </CustomButton>
      </Fragment>
    );
  };

  const renderDownload = (): JSX.Element => {
    if (xlsxFileSend) return <Fragment />;

    return (
      <CustomTextButton ptasVariant="Text large" onClick={handleDownload}>
        {fm.downloadForCorrection}
      </CustomTextButton>
    );
  };

  const renderExcelResultsPreview = (): JSX.Element => {
    if (!excelResultInfo?.file || !anchorElement) return <Fragment />;

    return (
      <CustomPopover
        anchorEl={anchorElement}
        onClose={closePopover}
        ptasVariant="success"
        showCloseButton
        anchorPosition={{
          top: 0,
          left: 0,
        }}
        classes={{
          paper: classes.popover,
          closeIcon: classes.closeIcon,
          closeButton: classes.closeButton,
          backdropRoot: classes.backdropRoot,
        }}
      >
        <div className={classes.popoverHeader}>
          <div className={classes.headerContent}>
            {renderPreviewFileHeader()}
            {renderDownload()}
          </div>
        </div>
        <div className={classes.popoverContent}>
          <OfficeFilePreview fileUrl={excelResultInfo.file} />
        </div>
      </CustomPopover>
    );
  };

  return (
    <MainLayout anchorRef={tabRef}>
      <Banner
        color="success"
        open={xlsxFileSend}
        message="Information submitted successfully."
      />
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <div className={classes.header}>
          <h2 className={classes.title}>{fm.title}</h2>
          <ProfileOptionsButton />
        </div>
        <div className={classes.inputWrap}>
          <div className={classes.uploadContentWrap}>
            <span className={classes.uploadTitle}>{fm.uploadTitle}</span>

            <FileUpload
              classes={{
                rootDropzone: classes.fileUploadRoot,
                rootPreview: classes.fileUploadRoot,
              }}
              additionalUploads={false}
              attrAccept={[
                {
                  extension: '.xlsx',
                  mimeType:
                    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                },
              ]}
              dragRejectText={fm.uploadDragReject}
              bigDropzoneForAdditionalFiles={true}
              onFileUpload={handleUploadFile}
              currentFiles={files}
              onRemoveFile={handleRemoveFile}
              onZoomIn={handlePreviewFile}
            />
            {renderExcelResultsPreview()}
            <span className={classes.uploadDesc}>{fm.uploadDesc}</span>
            <Link to="/Permit.xlsx" className={classes.downloadTemplateLink}>
              {fm.permitsTemplate}
            </Link>
          </div>
          <span className={classes.border}></span>
          <div className={classes.buttonWrap}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{
                root: classes.instructionButton,
              }}
              onClick={handleOpenModal}
            >
              {fm.instruction}
            </CustomTextButton>
          </div>
          {renderInstructionCard()}
        </div>
      </CustomCard>
    </MainLayout>
  );
};

export default Permits;
