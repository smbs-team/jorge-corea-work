// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomCard,
  CustomTextButton,
  CustomButton,
  FileUpload,
} from '@ptas/react-public-ui-library';
import { Box, Collapse } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import { useStyles } from './styles';
import useManageBusiness from './useManageBusiness';
import DocumentIcon from '@material-ui/icons/InsertDriveFile';
import CloudUpload from '@material-ui/icons/CloudUpload';
import PreviewFile from '../../../../components/PreviewFile';
import HelpSection from 'components/HelpSection';
import RemoveAlert from 'components/RemoveAlert';
import CloseIcon from '@material-ui/icons/Close';
import IconButton from '@material-ui/core/IconButton';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import withManageBusinessProvider from 'contexts/ManageBusiness';
import { Link } from 'react-router-dom';

function ManageBusiness(): JSX.Element {
  const classes = useStyles();
  const {
    business,
    handleUpdateListingInfo,
    handleAttachListingInfo,
    removeAlertAnchor,
    setRemoveAlertAnchor,
    openPreviewFile,
    handleClosePreview,
    currentFiles,
    sharePointFiles,
    savingFiles,
    showInstruction,
    handleShowInstructionCard,
    showAttachListingInfo,
    handleRedirectToBusinessState,
    handleFileUpload,
    handleRemoveFile,
    handleRemoveBusiness,
    closeRemoveBusinessAlert,
    goToHome,
    handleSaveFiles,
    previewData,
  } = useManageBusiness();

  const renderInstructionCard = (): JSX.Element => {
    return (
      <Collapse in={showInstruction}>
        <div className={classes.instructionCard}>
          <span className={classes.instructionTitle}>
            {fm.convertPtmsToExcel}
          </span>
          <ol className={classes.instructionList}>
            <li className={classes.instructionItem}>
              {fm.createYourPtmsReport}
            </li>
            <li className={classes.instructionItem}>{fm.toolbar}</li>
            <li className={classes.instructionItem}>{fm.exportFormatExcel}</li>
            <li className={classes.instructionItem}>{fm.exportDiskLike}</li>
            <li className={classes.instructionItem}>{fm.exportOkay}</li>
            <li className={classes.instructionItem}>{fm.exportSave}</li>
          </ol>
        </div>
      </Collapse>
    );
  };

  const renderAttachListingInfo = (): JSX.Element => {
    return (
      <Collapse in={showAttachListingInfo}>
        <div className={classes.inputFileWrap}>
          <IconButton
            className={classes.closeButton}
            onClick={handleAttachListingInfo}
          >
            <CloseIcon className={classes.closeIcon} />
          </IconButton>
          <div className={classes.downloadTemplate}>
            {fm.download}
            <Link to="/" className={classes.link}>
              {fm.eListing}
            </Link>
            {fm.template}
          </div>
          <div className={classes.downloadTemplate}>
            <span className={classes.instructionSpace}>
              {fmGeneral.instruction}:
            </span>
            <CustomTextButton
              ptasVariant="Text more"
              onClick={handleShowInstructionCard}
            >
              {fm.ptmsToExcel}
            </CustomTextButton>
          </div>
          {renderInstructionCard()}
          <span className={classes.attachListingFile}>
            {fm.attachListingFile}
          </span>
          <FileUpload
            classes={{
              rootDropzone: classes.fileUploadRoot,
              rootPreview: classes.rootPreview,
            }}
            currentFiles={currentFiles}
            attrAccept={[
              {
                extension: '.xlsx',
                mimeType:
                  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
              },
            ]}
            additionalUploads={false}
            multipleFilesUploadAtOnce={false}
            bigDropzoneForAdditionalFiles={false}
            onZoomIn={openPreviewFile}
            onFileUpload={handleFileUpload}
            onRemoveFile={handleRemoveFile}
          />
          <Box className={classes.buttonsWrap}>
            <CustomButton
              classes={{
                root: classes.saveAttachBtn,
              }}
              onClick={handleSaveFiles}
              ptasVariant="Primary"
              disabled={savingFiles}
            >
              {fmGeneral.save}
            </CustomButton>
          </Box>
        </div>
      </Collapse>
    );
  };

  const renderAttachments = (): JSX.Element[] => {
    return sharePointFiles.map(f => {
      return (
        <Box
          key={f.id}
          className={classes.document}
          onClick={(): Promise<void> => openPreviewFile(f)}
        >
          <DocumentIcon className={classes.documentIcon} />
          <span className={classes.documentName}>{f.fileName}</span>
        </Box>
      );
    });
  };

  return (
    <Fragment>
      <MainLayout>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.card,
            wrapperContent: classes.contentWrap,
          }}
        >
          <Box className={classes.header}>
            <CustomTextButton
              ptasVariant="Clear"
              onClick={goToHome}
              classes={{ root: classes.back }}
            >
              Back
            </CustomTextButton>
            <h2 className={classes.title}>{fm.title}</h2>
            <div className={classes.name}>
              <ProfileOptionsButton />
            </div>
          </Box>
          <Box className={classes.content}>
            <Box className={classes.businessName}>{business?.businessName}</Box>
            <Box className={classes.propertyAccount}>
              <span>{fm.propertyAccount}</span>
              <span>{business?.accountNumber ?? ''}</span>
            </Box>
            <Box className={classes.dates}>
              <label className={classes.filedDate}>
                {`Filed ${business?.filedDate ?? '-'}`}
              </label>
              <label>{`Assessed ${business?.assessedYear}`}</label>
            </Box>
            <Collapse in={!showAttachListingInfo}>
              <CustomCard
                classes={{
                  root: classes.documentsCard,
                  content: classes.documentsCardContent,
                }}
              >
                <Box className={classes.documentsWrapper}>
                  {renderAttachments()}
                </Box>
              </CustomCard>
            </Collapse>
            {!showAttachListingInfo && <span className={classes.border}></span>}
            <Box className={classes.buttonsWrap}>
              <CustomButton
                classes={{
                  root: classes.updateListingInfo,
                }}
                onClick={handleUpdateListingInfo}
                ptasVariant="Primary"
              >
                {fm.updateListingInfo}
              </CustomButton>
              <span className={classes.labelOr}>or</span>
              <CustomButton
                classes={{
                  root: classes.attachListingInfo,
                  // label: classes.attachListingInfoLabel,
                }}
                disabled={showAttachListingInfo}
                onClick={handleAttachListingInfo}
                ptasVariant="Primary"
              >
                <span className={classes.attachListingInfoLabel}>
                  <CloudUpload className={classes.cloudUpload} />
                  {fm.attachListingInfo}
                </span>
              </CustomButton>
            </Box>
            {renderAttachListingInfo()}
            <CustomCard
              classes={{
                root: classes.removeCard,
                content: classes.removeCardContent,
              }}
            >
              <Box
                className={classes.businessMoved}
                onClick={handleRedirectToBusinessState}
              >
                {fm.businessMovedSoldClosed}
              </Box>
              <CustomTextButton
                classes={{ root: classes.removeButton }}
                ptasVariant="Text more"
                onClick={(evt: React.MouseEvent<HTMLButtonElement>): void =>
                  setRemoveAlertAnchor(evt.currentTarget)
                }
              >
                {fm.businessRemove}
              </CustomTextButton>
            </CustomCard>
            {/* TEMP div to test position of remove alert */}
            {/* <div style={{height: 500}} /> */}
          </Box>
          <HelpSection />
        </CustomCard>
      </MainLayout>
      {removeAlertAnchor && (
        <RemoveAlert
          anchorEl={removeAlertAnchor}
          alertContent={fm.removeAlert}
          buttonText={fm.removeBusiness}
          onClose={closeRemoveBusinessAlert}
          onButtonClick={handleRemoveBusiness}
        />
      )}
      <PreviewFile
        onClose={handleClosePreview}
        open={previewData.open}
        file={previewData.file}
      />
    </Fragment>
  );
}

export default withManageBusinessProvider(ManageBusiness);
