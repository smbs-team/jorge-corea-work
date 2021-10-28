// usePermit.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useContext, useRef } from 'react';
import { CustomFile, ErrorMessageContext } from '@ptas/react-public-ui-library';
import { PermitsFileInfo } from 'routes/models/Views/Profile/Contact';
import { useAppContext } from 'contexts/AppContext';
import { apiService } from 'services/api/apiService';

interface UsePermit {
  openInstructionCard: boolean;
  xlsxFileSend: boolean;
  anchorElement: HTMLDivElement | null;
  files: CustomFile[];
  excelResultInfo: PermitsFileInfo | undefined;
  handleOpenModal: () => void;
  handleDownload: () => void;
  handleUploadFile: (filesToUpload: CustomFile[]) => Promise<CustomFile[]>;

  handlePreviewFile: () => void;
  handleSavePermits: () => void;

  handleRemoveFile: (file: CustomFile) => void;
  tabRef: React.MutableRefObject<null>;
  closePopover: () => void;
}

const usePermit = (): UsePermit => {
  const [openInstructionCard, setOpenInstructionCard] =
    useState<boolean>(false);
  /**
   * It will determine if the excel has been sent (with or without errors)
   */
  const [xlsxFileSend, setXlsxFileSend] = useState<boolean>(false);
  const [files, setFiles] = useState<CustomFile[]>([]);
  const [excelResultInfo, setExcelResultInfo] = useState<PermitsFileInfo>();
  const [anchorElement, setAnchorElement] =
    useState<HTMLDivElement | null>(null);
  const tabRef = useRef(null);
  const { contactProfile } = useAppContext();
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const closePopover = (): void => {
    setAnchorElement(null);
  };

  const handleOpenModal = (): void =>
    setOpenInstructionCard((prevState) => !prevState);

  /**
   * Allows you to download a file via a url
   */
  const handleDownload = (): void => {
    if (!excelResultInfo?.file) return;

    // once the file has been saved, the url for downloading the file is no longer valid.
    if (xlsxFileSend) {
      // create a temp url to download the file
      const tempUrl = URL.createObjectURL(files[0].file);
      // create a temp <a></a> element
      const link = document.createElement('a');
      // modify a element attribute
      link.setAttribute('href', tempUrl);
      // this attr was modified so that the file is downloaded with the original name
      link.setAttribute('download', files[0].fileName);
      // execute '<a></a>' click event to download the file
      link?.click();

      // delete "a" element and delete temp url after the file was downloaded
      document.body.removeChild(link);
      URL.revokeObjectURL(tempUrl);
      return;
    }

    // if the file was not sended, download the file with url server
    window.open(excelResultInfo.file, '_blank');
  };

  /**
   * Upload files to the file permit service
   */
  const handleUploadFile = async (
    filesToUpload: CustomFile[]
  ): Promise<CustomFile[]> => {
    // update this state to be able to display the Banner component again.
    setXlsxFileSend(false);
    const file = filesToUpload[0].file;

    if (file) sendFile(file);
    setFiles((prev) => [...prev, ...filesToUpload]);
    handlePreviewFile();
    return new Promise((res) => res(files));
  };

  /**
   * set anchor element to show preview file component
   */
  const handlePreviewFile = (): void => setAnchorElement(tabRef.current);

  /**
   * Saves file information in an annotation and updates file information
   */
  const handleSavePermits = (): void => {
    const file = files[0].file;

    if (!file) return;

    const annotationCreated = createAnnotation(file);
    const infoFileUpdated = updateInfoFile();

    Promise.all([annotationCreated, infoFileUpdated]).then((res) => {
      const notSended = res.includes(false);

      // close file preview after file was sended
      closePopover();

      if (notSended) {
        return setErrorMessageState({
          open: true,
        });
      }

      // hide remove button
      setFiles((prev) => [{ ...prev[0], hideRemoveButton: true }]);
      // set this states to able to display Banner component and hide "download for revision" button
      setXlsxFileSend(true);
    });
  };

  /**
   * Allow delete file from FileStorage
   */
  const deleteFile = async (fileName: string): Promise<void> => {
    if (!contactProfile?.jurisdictionId) return;

    const { hasError } = await apiService.deleteFile(
      fileName.toLocaleLowerCase(),
      contactProfile?.jurisdictionId
    );

    if (hasError) {
      return setErrorMessageState({
        open: true,
      });
    }
  };

  /**
   * Saves the information of a file, return true if file information was saved in permits
   * @example
   * ```
   *  New permits attachment.
   *  Uploaded by 'first_name' 'last_name'
   *  Document Name: 'documentName.ext'
   * ```
   */
  const createAnnotation = async (excelFile: File): Promise<boolean> => {
    if (!excelFile || !contactProfile?.jurisdictionId) return false;

    const note = `
          New permits attachment.
          Uploaded by ${contactProfile.firstName} ${contactProfile.lastName}
          Document Name: ${excelFile.name}
        `;

    const { hasError } = await apiService.createAnnotation(
      excelFile,
      contactProfile.jurisdictionId,
      note,
      'ptas_jurisdiction'
    );

    if (hasError) {
      return false;
    }

    return true;
  };

  /**
   * Move file to dynamics and delete file from 'permits' container
   */
  const updateInfoFile = async (): Promise<boolean> => {
    if (!excelResultInfo || !contactProfile?.jurisdictionId) return false;

    const { hasError } = await apiService.updatePermitFileInfo(
      excelResultInfo.file,
      contactProfile.jurisdictionId
    );

    if (hasError) {
      return false;
    }

    return true;
  };

  /**
   * Upload file to permits file service
   */
  const sendFile = async (excelFile: File): Promise<void> => {
    const jurisdictionId = contactProfile?.jurisdictionId;
    if (!excelFile || !jurisdictionId) return;

    const { data, hasError } = await apiService.uploadExcelFilePermits(
      excelFile,
      jurisdictionId
    );

    if (hasError) {
      setErrorMessageState({
        open: true,
      });

      return;
    }

    if (!data) return;

    setExcelResultInfo({
      ...data,
      file: data?.file.split('?')[0] ?? '',
    });

    setFiles((prev) => {
      return [
        {
          ...prev[0],
          isUploading: false,
        },
      ];
    });
  };

  /**
   * Allow to delete file from fileStorage
   */
  const handleRemoveFile = async (file: CustomFile): Promise<void> => {
    await deleteFile(file.fileName);
    setFiles([]);
  };

  return {
    anchorElement,
    openInstructionCard,
    xlsxFileSend,
    files,
    excelResultInfo,
    handleDownload,
    handleOpenModal,
    handlePreviewFile,
    handleRemoveFile,
    handleSavePermits,
    handleUploadFile,
    tabRef,
    closePopover,
  };
};

export default usePermit;
