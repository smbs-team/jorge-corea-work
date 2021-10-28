// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useEffect,
  useRef,
  useState,
  useContext,
  Fragment,
  useCallback,
} from 'react';
import {
  CustomCard,
  CustomTextButton,
  FileUpload,
  CustomFile,
  CustomPopover,
  CustomButton,
  PdfFilePreview,
  OfficeFilePreview,
  ErrorMessageContext,
  Banner,
} from '@ptas/react-public-ui-library';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import { useStyles } from './styles';
import clsx from 'clsx';
import { Box, Collapse } from '@material-ui/core';
import { apiService } from 'services/api/apiService';
import { v4 as uuid } from 'uuid';
import { getFileFromUrl, readFileAsync } from 'services/common/getFileFromUrl';
import { useAppContext } from 'contexts/AppContext';
import ProfileOptionsButton from 'components/ProfileOptionsButton';

const attrAccept = [
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
];

function Levy(): JSX.Element {
  const [openInstructionCard, setOpenInstructionCard] =
    useState<boolean>(false);
  const [uploadedFile, setUploadedFile] =
    useState<{
      file: File;
      // set if to detect file
      id?: string;
      // set flag to hide element header
      addedToAnnotation?: boolean;
      // set url to preview
      url?: string;
    }>();
  const { contactProfile } = useAppContext();
  const [currentFiles, setCurrentFiles] = useState<CustomFile[]>([]);
  // flag to able to display banner component
  const [fileSent, setFileSent] = useState<boolean>(false);
  // the file id is added if it has already been added to an annotation
  const [savedFilesId, setSavedFilesId] = useState<string[]>([]);
  const [fileType, setFileType] =
    useState<'pdf' | 'word' | 'excel' | 'image' | 'other' | undefined>(
      undefined
    );
  const [anchorElement, setAnchorElement] =
    useState<HTMLDivElement | null>(null);
  const tabRef = useRef(null);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  // save url list
  const [filesUrl, setFilesUrl] = useState<
    {
      id: string;
      url: string;
    }[]
  >([]);
  const [imageSize, setImageSize] = useState<{
    width: number;
    height: number;
  }>({
    height: 0,
    width: 0,
  });

  const classes = useStyles();

  useEffect(() => {
    if (uploadedFile) {
      const fileName = uploadedFile.file.name.toLowerCase();
      if (fileName.endsWith('.pdf')) {
        setFileType('pdf');
      } else if (fileName.endsWith('.doc') || fileName.endsWith('.docx')) {
        setFileType('word');
      } else if (fileName.endsWith('.xls') || fileName.endsWith('.xlsx')) {
        setFileType('excel');
      } else if (fileName.endsWith('.png') || fileName.endsWith('.jpg')) {
        setFileType('image');
      } else {
        setFileType('other');
      }
    } else {
      setFileType(undefined);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [uploadedFile]);

  /**
   * Load initial files by tax district id
   */
  const loadInitialFiles = useCallback(async (): Promise<void> => {
    if (!contactProfile?.taxDistrictId) return;

    const { data } = await apiService.getFiles(contactProfile.taxDistrictId);

    let files: CustomFile[] = [];
    let urls: {
      id: string;
      url: string;
    }[] = [];

    for (const url of data || []) {
      const file = await getFileFromUrl(url);
      const arrayBuffer = await readFileAsync(file);
      const id = uuid();
      const ext = url.split('.').slice(-1)[0].toLowerCase();
      const isImage = ['jpg', 'png'].includes(ext);

      // create array to set currentFiles
      files = [
        ...files,
        {
          id,
          file: file,
          isUploading: false,
          fileName: file.name.replace(/%20/g, ' '),
          contentType: isImage ? 'publicUrl' : 'arrayBuffer',
          content: isImage ? url : arrayBuffer ? arrayBuffer : '',
        },
      ];

      // create array to set url list
      urls = [
        ...urls,
        {
          id,
          url,
        },
      ];
    }

    setFilesUrl(urls);
    setCurrentFiles(files);
  }, [contactProfile]);

  /**
   * Saves the information of a file, return true if file information was saved
   * @example
   * ```
   *  New permits attachment.
   *  Uploaded by 'first_name' 'last_name'
   *  Document Name: 'documentName.ext'
   * ```
   */
  const createAnnotation = async (): Promise<void> => {
    if (!uploadedFile?.file || !contactProfile?.taxDistrictId) return;

    const note = `
        New permits attachment.
        Uploaded by ${contactProfile.firstName} ${contactProfile.lastName}
        Document Name: ${uploadedFile.file.name}
      `;

    const { hasError, data } = await apiService.createAnnotation(
      uploadedFile.file,
      contactProfile.taxDistrictId,
      note,
      'ptas_taxdistrict'
    );

    if (hasError) {
      setErrorMessageState({
        open: true,
      });

      return;
    }

    if (!data) return;

    // file id added
    setSavedFilesId((prev) => [...prev, uploadedFile.id ?? '']);

    // hide remove button
    setCurrentFiles((prev) => {
      return prev.map((file) =>
        file.id === uploadedFile.id ? { ...file, hideRemoveButton: true } : file
      );
    });

    await deleteFile(uploadedFile.file.name);

    closePopover();
  };

  useEffect(() => {
    loadInitialFiles();
  }, [loadInitialFiles]);

  /**
   * set anchor element to null to hide preview file component
   */
  const closePopover = (): void => {
    setAnchorElement(null);
  };

  const handleOpenModal = (): void =>
    setOpenInstructionCard((prevState) => !prevState);

  const renderInstructionCard = (): JSX.Element => {
    return (
      <Collapse in={openInstructionCard}>
        <div className={classes.instructionCardWrap}>
          <span className={classes.instructionTitle}>
            {fm.instructionTitle}
          </span>
          <ol className={classes.instructionList}>
            <li className={classes.instructionListItem}>
              {fm.instructionFillForms}
            </li>
            <li className={classes.instructionListItem}>
              {fm.instructionSelectFile}
            </li>
            <li className={classes.instructionListItem}>
              {fm.instructionReviewErrors}
            </li>
            <li className={classes.instructionListItem}>
              {fm.instructionSaveForm}
            </li>
          </ol>
        </div>
      </Collapse>
    );
  };

  const handleSaveAnnotation = (): void => {
    setFileSent(true);
    createAnnotation();
  };

  const handleCloseBanner = (): void => {
    setFileSent(false);
  };

  const deleteFile = async (fileName: string): Promise<void> => {
    if (!contactProfile?.taxDistrictId) return;

    const { hasError } = await apiService.deleteFile(
      fileName,
      contactProfile?.taxDistrictId
    );

    if (hasError) {
      return setErrorMessageState({
        open: true,
      });
    }
  };

  const handleRemove = async (file: CustomFile): Promise<void> => {
    await deleteFile(file.fileName);

    const newFiles = currentFiles.filter((f) => f.id !== file.id);

    setCurrentFiles(newFiles);
  };

  const handleUpload = async (files: CustomFile[]): Promise<CustomFile[]> => {
    const id = uuid();
    const file = files[0].file;
    // set this state to hide Banner component
    setFileSent(false);

    if (file && contactProfile?.taxDistrictId) {
      const { data: url } = await apiService.uploadFile(
        file,
        contactProfile?.taxDistrictId
      );

      // set current file
      setUploadedFile({ file, id, url });
      // add url to list url files
      setFilesUrl((prev) => [...prev, { id, url: url ?? '' }]);
    }

    setCurrentFiles((prev) => [
      ...prev,
      { ...files[0], id, isUploading: false },
    ]);

    // open popover preview
    setAnchorElement(tabRef.current);
    return new Promise((res) => res(files));
  };

  const handleOnZoomIn = (customFile: CustomFile): void => {
    const { id, file } = customFile;

    const addedToAnnotation = savedFilesId.includes(id ?? '');

    const currentUrl = filesUrl?.find((fUrl) => fUrl.id === id)?.url;

    if (file && currentUrl) {
      setUploadedFile({
        file,
        id,
        addedToAnnotation,
        url: currentUrl,
      });

      if (
        currentUrl.toLocaleLowerCase().endsWith('.png') ||
        currentUrl.toLocaleLowerCase().endsWith('.jpg')
      ) {
        const img = new Image();
        img.onload = (): void => {
          setAnchorElement(tabRef.current);
          setImageSize({
            height: img.height,
            width: img.width,
          });
        };
        img.src = currentUrl as string;
      } else {
        setAnchorElement(tabRef.current);
      }
    }
  };

  const renderHeaderPreviewFile = (): JSX.Element => {
    if (uploadedFile?.addedToAnnotation) return <Fragment />;

    return (
      <Fragment>
        <div className={classes.textWrap}>
          <span className={classes.headerTitle}>{fm.reviewImport}</span>
          <span className={classes.headerDescription}>
            {fm.reviewDescription}
          </span>
        </div>
        <CustomButton
          classes={{ root: classes.saveButton }}
          onClick={handleSaveAnnotation}
        >
          {fm.saveForm}
        </CustomButton>
      </Fragment>
    );
  };

  const renderPreviewFile = (): JSX.Element => {
    if (!uploadedFile) return <Fragment />;

    switch (fileType) {
      case 'excel':
        return <OfficeFilePreview fileUrl={uploadedFile.url} />;
      case 'image':
        return (
          <Box
            className={clsx(
              classes.imagePreviewContainer,
              imageSize.width < 616 && classes.dimBlackColor
            )}
          >
            <img
              src={uploadedFile.url}
              alt="Preview"
              // 616 is minimun size
              className={clsx(
                classes.imagePreview,
                imageSize.width < 616 && classes.smallWidth
              )}
            />
          </Box>
        );
      case 'pdf':
        return (
          <PdfFilePreview elementId={'pdfBody'} file={uploadedFile.file} />
        );
      case 'word':
        return <OfficeFilePreview fileUrl={uploadedFile.url} />;
      default:
        return <Fragment />;
    }
  };

  return (
    <MainLayout anchorRef={tabRef}>
      <Banner
        color="success"
        open={fileSent}
        message="Information has been sent successfully"
        handleClose={handleCloseBanner}
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
              attrAccept={attrAccept}
              dragRejectText={fm.uploadDragReject}
              multipleFilesUploadAtOnce={false}
              bigDropzoneForAdditionalFiles={false}
              currentFiles={currentFiles}
              onFileUpload={handleUpload}
              onZoomIn={handleOnZoomIn}
              onRemoveFile={handleRemove}
            />
            <CustomPopover
              marginThreshold={0}
              anchorEl={anchorElement}
              onClose={(): void => {
                closePopover();
              }}
              ptasVariant="success"
              showCloseButton
              anchorPosition={{
                top: 0,
                left: 0,
              }}
              classes={{
                paper: clsx(
                  classes.popover,
                  fileType === 'image' && classes.imageWith
                ),
                closeIcon: classes.closeIcon,
                closeButton: classes.closeButton,
                backdropRoot: classes.backdropRoot,
                root: classes.popoverRoot,
              }}
            >
              <div className={classes.popoverHeader}>
                <div className={classes.headerContent}>
                  {renderHeaderPreviewFile()}
                </div>
              </div>
              <div
                className={clsx(
                  classes.popoverContent,
                  fileType === 'excel'
                    ? classes.excelPopoverContent
                    : fileType === 'word'
                    ? classes.officeViewerPopoverContent
                    : fileType === 'pdf'
                    ? classes.pdfPopoverContent
                    : ''
                )}
              >
                {renderPreviewFile()}
              </div>
            </CustomPopover>
          </div>
          <div className={classes.buttonWrap}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{
                root: classes.instructionButton,
              }}
              onClick={handleOpenModal}
            >
              {fm.instructions}
            </CustomTextButton>
          </div>
          {renderInstructionCard()}
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default Levy;
