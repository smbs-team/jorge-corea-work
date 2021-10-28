// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomCard,
  CustomTextButton,
  PropertySearcher,
  CardPropertyInfo,
  CustomTabs,
  CustomButton,
  FileUpload,
} from '@ptas/react-public-ui-library';
import { Box, useTheme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatMessage';
import useCurrentUse from './useCurrentUse';
import CurrentUseCard from './currentUseCard';
import { useStyles } from './styles';
import PreviewFile from 'components/PreviewFile';
import ProfileOptionsButton from 'components/ProfileOptionsButton';

function CurrentUse(): JSX.Element {
  const {
    handleTabChange,
    handleDone,
    handleClickOnInstructions,
    handleOnSearchChange,
    searchList,
    exemptionList,
    currentExemptionId,
    setCurrentExemptionId,
    onSearchItemSelected,
    currentStep,
    openLink,
    loading,
    searchText,
    files,
    onUploadFile,
    onRemoveFile,
    applicationCompleted,
    completeAppLoading,
    fileToPreview,
    openFilePreview,
    setOpenFilePreview,
    onZoomInFile,
  } = useCurrentUse();
  const classes = useStyles();
  const theme = useTheme();

  return (
    <MainLayout>
      <div className={classes.cardWrapper}>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.card,
            wrapperContent: classes.contentWrapper,
          }}
        >
          <Box className={classes.header}>
            <h2 className={classes.title}>{fm.title}</h2>
            <ProfileOptionsButton />
          </Box>
          {!!exemptionList?.length &&
            exemptionList.map(exemption => (
              <Fragment key={exemption.id}>
                <CardPropertyInfo
                  id={exemption.id}
                  key={exemption.id}
                  classes={{
                    root: classes.cardPropertyInfo,
                    cardContent: classes.cardPropertyInfoContent,
                    content: classes.cardPropertyInfoChildrenContent,
                  }}
                  onClick={(id: string): void => {
                    console.log('Clicked on', id);
                    setCurrentExemptionId(id);
                  }}
                  shadow={false}
                  type={'simple'}
                  imageSrc={exemption.homePicture}
                  parcelNumber={exemption.parcelNumber}
                  propertyName={exemption.homeOwner}
                  propertyAddress={exemption.homeAddress}
                  upperStripText={
                    exemption.complete
                      ? fm.appliedExemption(
                          new Date(
                            exemption.applicationDate
                          ).toLocaleDateString()
                        )
                      : undefined
                  }
                >
                  {currentExemptionId === exemption.id && (
                    <Box className={classes.exemptionContent}>
                      <label className={classes.availableFormsLabel}>
                        {fm.availableForms}
                      </label>
                      <CustomTabs
                        selectedIndex={
                          currentStep === 'farmLand'
                            ? 0
                            : currentStep === 'forestLand'
                            ? 1
                            : currentStep === 'openSpace'
                            ? 2
                            : 0
                        }
                        opacityColor="#666666"
                        ptasVariant="SwitchMedium"
                        items={[
                          {
                            label: fm.tabFarmLand,
                            disabled: applicationCompleted,
                          },
                          {
                            label: fm.tabForestLand,
                            disabled: applicationCompleted,
                          },
                          {
                            label: fm.tabOpenSpace,
                            disabled: applicationCompleted,
                          },
                        ]}
                        onSelected={(tab: number): void => {
                          handleTabChange(tab);
                        }}
                        indicatorBackgroundColor={theme.ptas.colors.theme.white}
                        itemTextColor={theme.ptas.colors.theme.white}
                        selectedItemTextColor={theme.ptas.colors.theme.black}
                        tabsBackgroundColor={theme.ptas.colors.theme.gray}
                      />
                      {currentStep === 'farmLand' && (
                        <Box className={classes.farmLand}>
                          <CurrentUseCard headerText={fm.farmLandHeader}>
                            <Box className={classes.farmLandInstructions}>
                              <Box
                                className={classes.farmLandInstructionsLabel}
                              >
                                {fm.farmLandDownload}
                                <label
                                  className={classes.hightlightLink}
                                  onClick={(): void =>
                                    openLink(
                                      process.env
                                        .REACT_APP_FARM_LAND_FORM_URL || ''
                                    )
                                  }
                                >
                                  {fm.farmLandDownloadFileName}
                                </label>
                              </Box>
                              <Box
                                className={classes.farmLandInstructionsLabel}
                              >
                                {fm.farmLandCompleteApplication}
                              </Box>
                              <Box
                                className={classes.farmLandInstructionsLabel}
                              >
                                {fm.farmLandAttachApplication}
                              </Box>
                            </Box>
                            <FileUpload
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
                              bigDropzoneForAdditionalFiles={false}
                              onFileUpload={onUploadFile}
                              currentFiles={files}
                              onRemoveFile={onRemoveFile}
                              onZoomIn={onZoomInFile}
                              // additionalUploads={false}
                              // multipleFilesUploadAtOnce={false}
                            />
                            <CustomButton
                              classes={{
                                root: classes.doneButton,
                              }}
                              onClick={handleDone}
                              ptasVariant="Primary"
                              disabled={
                                applicationCompleted || completeAppLoading
                              }
                            >
                              {fm.buttonDone}
                            </CustomButton>
                          </CurrentUseCard>
                        </Box>
                      )}
                      {currentStep === 'forestLand' && (
                        <Box className={classes.forestLand}>
                          <CurrentUseCard headerText={fm.forestLandHeader}>
                            <Box className={classes.forestLandInstructions}>
                              <Box
                                className={classes.forestLandInstructionsLabel}
                              >
                                {fm.forestLandDownload}
                                <label
                                  className={classes.hightlightLink}
                                  onClick={(): void =>
                                    openLink(
                                      process.env
                                        .REACT_APP_FOREST_LAND_FORM_URL ?? ''
                                    )
                                  }
                                >
                                  {fm.forestLandDownLoadFileName1}
                                </label>
                                {fm.forestLandOr}
                                <label
                                  className={classes.hightlightLink}
                                  onClick={(): void =>
                                    openLink(
                                      process.env
                                        .REACT_APP_FOREST_LAND_FORM_MULTI_OWNER_URL ??
                                        ''
                                    )
                                  }
                                >
                                  {fm.forestLandDownLoadFileName2}
                                </label>
                              </Box>
                              <Box
                                className={classes.forestLandInstructionsLabel}
                              >
                                {fm.forestLandCompleteApplication}
                              </Box>
                              <Box
                                className={classes.forestLandInstructionsLabel}
                              >
                                {fm.forestLandAttachApplication}
                              </Box>
                            </Box>
                            <FileUpload
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
                              bigDropzoneForAdditionalFiles={false}
                              onFileUpload={onUploadFile}
                              currentFiles={files}
                              onRemoveFile={onRemoveFile}
                              onZoomIn={onZoomInFile}
                              // additionalUploads={false}
                              // multipleFilesUploadAtOnce={false}
                            />
                            <CustomButton
                              classes={{
                                root: classes.doneButton,
                              }}
                              onClick={handleDone}
                              ptasVariant="Primary"
                              disabled={
                                applicationCompleted || completeAppLoading
                              }
                            >
                              {fm.buttonDone}
                            </CustomButton>
                          </CurrentUseCard>
                        </Box>
                      )}
                      {currentStep === 'openSpace' && (
                        <Box className={classes.openSpace}>
                          <CurrentUseCard headerText={fm.openSpaceHeader}>
                            <Box className={classes.openSpaceInstructions}>
                              <Box
                                className={classes.openSpaceInstructionsLabel}
                              >
                                {fm.openSpaceGoTo}
                                <label
                                  className={classes.hightlightLink}
                                  onClick={(): void =>
                                    openLink(
                                      process.env
                                        .REACT_APP_OPEN_SPACE_REDIRECT_URL ?? ''
                                    )
                                  }
                                >
                                  {fm.openSpaceInfoPage}
                                </label>
                              </Box>
                              <Box
                                className={classes.openSpaceInstructionsLabel}
                              >
                                {fm.openSpaceDownloadForm}
                              </Box>
                              <Box
                                className={classes.openSpaceInstructionsLabel}
                              >
                                {fm.openSpaceFollowDirections}
                              </Box>
                            </Box>
                          </CurrentUseCard>
                        </Box>
                      )}
                    </Box>
                  )}
                </CardPropertyInfo>
              </Fragment>
            ))}
          <span className={classes.border} style={{ marginBottom: 32 }}></span>
          <PropertySearcher
            label={fm.findMyProperty}
            textButton={fm.locateMe}
            textDescription={fm.enterAddress}
            onChange={handleOnSearchChange}
            suggestion={{
              List: searchList,
              onSelected: onSearchItemSelected,
              loading: !!searchText && loading,
            }}
            onClickSearch={(): void => {
              console.log('searcher icon was clicked');
            }}
            onClickTextButton={(): void => {
              console.log('locate me button was clicked');
            }}
          />
          <span className={classes.indication}>{fm.indication}</span>
          <span className={classes.border}></span>
          <span className={classes.textHelp}>{fm.help}</span>
          <CustomTextButton onClick={handleClickOnInstructions}>
            {fm.instruction}
          </CustomTextButton>
          <PreviewFile
            onClose={(): void => setOpenFilePreview(false)}
            open={openFilePreview}
            file={fileToPreview}
          />
        </CustomCard>
      </div>
    </MainLayout>
  );
}

export default CurrentUse;
