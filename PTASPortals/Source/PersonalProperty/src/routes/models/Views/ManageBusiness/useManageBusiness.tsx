// useManageBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext, useEffect } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { ManageBusinessContext } from 'contexts/ManageBusiness';
import { ManageBusinessContextProps } from 'contexts/ManageBusiness/types';
import { CustomFile } from '@ptas/react-public-ui-library';
import { AppContext } from 'contexts/AppContext';
import { businessApiService } from 'services/api/apiService/business';
import { PortalContact } from 'models/portalContact';
import { apiService } from 'services/api/apiService';
import { v4 as uuidV4 } from 'uuid';
import { FileAttachmentMetadata } from 'models/fileAttachmentMetadata';
import { getFileFromUrl } from 'services/common/getFileFromUrl';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { getOptionSetValue } from 'utils/business';

interface UseManageBusiness extends ManageBusinessContextProps {
  handleUpdateListingInfo: () => void;
  handleAttachListingInfo: () => void;
  openPreviewFile: (file: CustomFile) => Promise<void>;
  handleClosePreview: () => void;
  handleShowInstructionCard: () => void;
  handleRedirectToBusinessState: () => void;
  handleFileUpload: (files: CustomFile[]) => Promise<CustomFile[]>;
  handleRemoveFile: (file: CustomFile) => void;
  handleRemoveBusiness: () => Promise<void>;
  closeRemoveBusinessAlert: () => void;
  goToHome: () => void;
  handleSaveFiles: () => Promise<void>;
  // app contact fields
  portalContact: PortalContact | undefined;
}

interface RouteParams {
  businessId: string;
}

function useManageBusiness(): UseManageBusiness {
  const { businessId } = useParams<RouteParams>();

  const history = useHistory();
  const { portalContact } = useContext(AppContext);
  const manageBusinessContext = useContext(ManageBusinessContext);
  const {
    business,
    setBusiness,
    showInstruction,
    setShowInstruction,
    setShowAttachListingInfo,
    currentFiles,
    setCurrentFiles,
    sharePointFiles,
    setSharePointFiles,
    setPreviewData,
    setRemoveAlertAnchor,
    savingFiles,
    setSavingFiles,
  } = manageBusinessContext;

  useEffect(() => {
    if (businessId) {
      fetchBusiness();
      fetchFiles();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [businessId]);

  const fetchBusiness = async (): Promise<void> => {
    const { data } = await businessApiService.getBusinessById(businessId);
    if (data) setBusiness(data);
  };

  const fetchFiles = async (): Promise<void> => {
    const { data } = await businessApiService.getBusinessFileAttachmentData(
      businessId
    );

    const firstFile = (data ?? [])[0];

    if (!firstFile) return;

    const files = [firstFile].reduce(
      (group, fa): Map<string, CustomFile[]> => {
        const groupName = fa.isSharePoint ? 'sharepoint' : 'current';
        const prevFiles = group.get(groupName) ?? [];

        const customFile = {
          id: fa.id,
          fileName: fa.name,
          content: fa.sharepointUrl || fa.blobUrl,
          contentType: 'publicUrl',
          isUploading: false,
        } as CustomFile;

        group.set(groupName, [...prevFiles, customFile]);
        return group;
      },
      new Map([
        ['sharepoint', []],
        ['current', []],
      ]) as Map<string, CustomFile[]>
    );

    setCurrentFiles(files.get('current') ?? []);
    setSharePointFiles(files.get('sharepoint') ?? []);
  };

  const goToHome = (): void => {
    history.push('/home');
  };

  const handleUpdateListingInfo = (): void => {
    history.push('/update-business', { businessId });
  };

  const handleAttachListingInfo = (): void => {
    setShowAttachListingInfo((prevState) => !prevState);
    if (showInstruction) setShowInstruction(false);
  };

  const openPreviewFile = async (file: CustomFile): Promise<void> => {
    let filePreview = { ...file };
    if (!file.file) {
      const isSharePointFile = sharePointFiles.some((sf) => sf.id === file.id);
      const fileFound = await getFileFromUrl(file.content as string);
      if (isSharePointFile) {
        const updatedFiles = sharePointFiles.map((cf) => {
          if (cf.id === file.id) {
            const modifiedFile = {
              ...cf,
              file: fileFound,
            };
            filePreview = modifiedFile;
            return modifiedFile;
          }
          return cf;
        });
        setSharePointFiles(updatedFiles);
      } else {
        // Current files
        const updatedFiles = currentFiles.map((cf) => {
          if (cf.id === file.id) {
            const modifiedFile = {
              ...cf,
              file: fileFound,
            };
            filePreview = modifiedFile;
            return modifiedFile;
          }
          return cf;
        });
        setCurrentFiles(updatedFiles);
      }
    }
    setPreviewData({
      file: filePreview,
      open: true,
    });
  };

  const handleClosePreview = (): void => {
    setPreviewData({ open: false });
  };

  const handleShowInstructionCard = (): void =>
    setShowInstruction((prevState) => !prevState);

  const handleRedirectToBusinessState = (): void =>
    history.push(`/manage-business-personal-property/${business?.id ?? ''}`);

  const uploadFileToServices = async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    const uploadingFiles = files.map(async (f) => {
      const fileAttachmentId = uuidV4();
      const fileAttachmentMetadataId = f.id ?? fileAttachmentId;
      const { data: content } = await apiService.uploadFile(
        f.file as File,
        fileAttachmentMetadataId
      );
      if (!content) return;
      const newFile = new FileAttachmentMetadata();
      newFile.id = fileAttachmentMetadataId;
      newFile.icsDocumentId = fileAttachmentMetadataId;
      newFile.name = f.fileName;
      newFile.isBlob = true;
      newFile.document = 'manageBusiness';
      newFile.isSharePoint = false;
      newFile.blobUrl = content as string;
      newFile.documentType = 'Listing';
      newFile.filingMethod = await getOptionSetValue(
        'ptas_fileattachmentmetadata',
        'ptas_filingmethod',
        'ezlisting'
      );
      newFile.listingStatus = await getOptionSetValue(
        'ptas_fileattachmentmetadata',
        'ptas_listingstatus',
        'Received'
      );
      newFile.year =
        (await (
          await businessApiService.getYearsIdByYears(new Date().getFullYear())
        ).data) ?? '';
      newFile.loadDate = new Date().toISOString();
      newFile.loadBy = process.env.REACT_APP_SYSTEM_USER_ID ?? '';

      const saveFileAttachmentResp =
        await businessApiService.saveFileAttachmentData(
          newFile,
          business?.id ?? ''
        );
      if (saveFileAttachmentResp.hasError) return;
      return {
        ...f,
        isUploading: false,
        contentType: 'publicUrl',
        content,
      } as CustomFile;
    });
    const uploadedFiles = await Promise.all(uploadingFiles).catch(() => []);

    const filesToReturn = uploadedFiles.filter((uf) => uf) as CustomFile[];

    return filesToReturn;
  };

  const handleFileUpload = async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    const uploadedFiles = await uploadFileToServices(files);
    setCurrentFiles((prev) => [...prev, ...uploadedFiles]);
    fetchFiles();
    return files;
  };

  const handleRemoveFile = async (file: CustomFile): Promise<void> => {
    setCurrentFiles((prev) => {
      return prev.filter((cf) => cf.id !== file.id);
    });
    await apiService.deleteFileAttachment(file?.id ?? '');
    await apiService.deleteFile(file.fileName, file?.id ?? '');
    fetchFiles();
  };

  const handleSaveFiles = async (): Promise<void> => {
    if (savingFiles) return;
    setSavingFiles(true);
    const fileIds = currentFiles.map((cf) => cf.id) as string[];
    const { data } = await apiService.moveBlobStorageToSharePoint(
      fileIds,
      process.env.REACT_APP_BLOB_CONTAINER ?? ''
    );
    if (data) {
      const sharePointDataUpdate = data.map((d) => [d.id, d.fileUrl]);
      await apiService.updateFileAttachmentDataToSharePoint(
        sharePointDataUpdate
      );
      fetchFiles();
      setShowAttachListingInfo(false);
    }

    setSavingFiles(false);
  };

  const handleRemoveBusiness = async (): Promise<void> => {
    if (!portalContact || !businessId) return;
    setRemoveAlertAnchor(null);
    const { data: businessContactFound } =
      await businessContactApiService.getBusinessContactByBusiness(businessId);
    if (!businessContactFound) return;
    const businessContactIds = businessContactFound.map((bc) => bc.id);
    await businessContactApiService.deleteBusinessContact(businessContactIds);
    goToHome();
  };

  const closeRemoveBusinessAlert = (): void => {
    setRemoveAlertAnchor(null);
  };

  return {
    ...manageBusinessContext,
    handleUpdateListingInfo,
    handleAttachListingInfo,
    openPreviewFile,
    handleClosePreview,
    handleShowInstructionCard,
    handleRedirectToBusinessState,
    handleFileUpload,
    handleRemoveFile,
    handleRemoveBusiness,
    closeRemoveBusinessAlert,
    goToHome,
    handleSaveFiles,
    portalContact,
  };
}

export default useManageBusiness;
