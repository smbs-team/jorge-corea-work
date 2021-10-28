// Sale.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useContext, useEffect } from 'react';
import {
  CustomTabs,
  CustomDatePicker,
  CustomFile,
  FileUpload,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import { apiService } from 'services/api/apiService';
import { CustomTabOption } from 'contexts/ChangedBusiness/types';
import { OptionSet } from 'models/optionSet';
import useManageData from '../useManageData';
import { useMount } from 'react-use';
import { QuickCollect } from 'models/quickCollect';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { formatDate } from 'utils/date';
import { v4 as uuidV4 } from 'uuid';
import { FileAttachmentMetadata } from 'models/fileAttachmentMetadata';
import { changedBusinessApiService } from 'services/api/apiService/changedBusiness';

function Sale(): JSX.Element {
  const classes = useStyles();
  const [currentFiles, setCurrentFiles] = useState<CustomFile[]>([]);
  const {
    reasonForSaleOpts,
    setReasonForSaleOpts,
    methodTransferOpts,
    setMethodTransferOpts,
    currentBusiness,
  } = useContext(ChangedBusinessContext);

  const {
    quickCollect,
    businessHistory,
    setQuickCollect,
    setBusinessHistory,
  } = useManageData('SOLD');

  useMount(() => {
    if (!quickCollect && !businessHistory) {
      if (!currentBusiness?.id) return;
      const newQuickCollect = new QuickCollect();
      newQuickCollect.personalPropertyId = currentBusiness.id;
      newQuickCollect.date = formatDate(new Date());
      setQuickCollect(newQuickCollect);
      const newBusinessHist = new PersonalPropertyHistory();
      newBusinessHist.personalPropertyId = currentBusiness.id;
      setBusinessHistory(newBusinessHist);
    }
    fetchReasonForSaleOptions();
    fetchMethodTransferOptions();
  });

  useEffect(() => {
    if (quickCollect?.billFileAttachmentMetadata) {
      // If quick collect exist, fetch files
      fetchFiles();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [quickCollect?.billFileAttachmentMetadata]);

  const fetchReasonForSaleOptions = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_quickcollect',
      'ptas_reasonforrequest'
    );
    if (!data) return;
    setReasonForSaleOpts(data);
  };

  const fetchMethodTransferOptions = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_quickcollect',
      'ptas_methodoftransfer'
    );
    if (!data) return;
    setMethodTransferOpts(data);
  };

  const onSelectReasonForSale = (tabIndex: number): void => {
    const reasonForRequestId = reasonForSaleOpts[tabIndex].attributeValue ?? 0;
    setQuickCollect(prev => {
      return {
        ...prev,
        reasonForRequestId,
      };
    });
  };

  const onSelectMethodOfTransfer = (tabIndex: number): void => {
    const methodOfTransferId = methodTransferOpts[tabIndex].attributeValue ?? 0;
    setQuickCollect(prev => {
      return {
        ...prev,
        methodOfTransferId,
      };
    });
  };

  // File manage
  const fetchFiles = async (): Promise<void> => {
    if (!quickCollect?.billFileAttachmentMetadataId) return;
    let blobDataToMap: [string, string][] = [];
    if (!quickCollect.billFileAttachmentMetadata) {
      const { data } = await changedBusinessApiService.getFileAttachmentData(
        quickCollect.billFileAttachmentMetadataId
      );
      if (!data) return;
      // set file attach to model on state
      setQuickCollect(prev => ({
        ...prev,
        billFileAttachmentMetadata: data,
      }));

      blobDataToMap = data.blobUrl ? JSON.parse(data.blobUrl) ?? [] : [];
    } else {
      const blobData = quickCollect.billFileAttachmentMetadata.blobUrl;
      blobDataToMap = blobData ? JSON.parse(blobData) ?? [] : [];
    }
    const files = blobDataToMap.map(([fileName, url]) => {
      return {
        id: uuidV4(),
        fileName: fileName,
        content: url,
        contentType: 'publicUrl',
        isUploading: false,
      } as CustomFile;
    });
    setCurrentFiles(files);
  };

  const generateNewFileAttach = (): FileAttachmentMetadata => {
    const fileAttachmentId = uuidV4();
    const newFileAttach = new FileAttachmentMetadata();
    newFileAttach.id = fileAttachmentId;
    newFileAttach.icsDocumentId = fileAttachmentId;
    newFileAttach.name = 'BillAttach';
    newFileAttach.isBlob = false;
    newFileAttach.document = 'changedBusinessSale';
    newFileAttach.isSharePoint = false;

    return newFileAttach;
  };

  const uploadFileToServices = async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    if (!quickCollect) return [];
    const { billFileAttachmentMetadata } = quickCollect;
    const fileAttach = billFileAttachmentMetadata
      ? { ...billFileAttachmentMetadata }
      : generateNewFileAttach();
    const uploadingFiles = files.map(async f => {
      const { data: content } = await apiService.uploadFile(
        f.file as File,
        fileAttach.id
      );
      if (!content) return;
      return {
        ...f,
        isUploading: false,
        contentType: 'publicUrl',
        content,
      } as CustomFile;
    });
    const uploadedFiles = await Promise.all(uploadingFiles).catch(() => []);

    const filesToReturn = uploadedFiles.filter(uf => uf) as CustomFile[];

    const newBlobData = filesToReturn.map(f => [f.fileName, f.content]);
    const prevBlobData = fileAttach.blobUrl
      ? JSON.parse(fileAttach.blobUrl)
      : [];
    const finalBlobData = JSON.stringify([...prevBlobData, ...newBlobData]);
    const fileAttachUpdated = {
      ...fileAttach,
      blobUrl: finalBlobData,
    } as FileAttachmentMetadata;

    if (quickCollect.billFileAttachmentMetadata) {
      await changedBusinessApiService.updateFileAttachmentData(
        fileAttachUpdated
      );
    } else {
      await changedBusinessApiService.saveFileAttachmentData(fileAttachUpdated);
    }

    setQuickCollect(prev => {
      return {
        ...prev,
        billFileAttachmentMetadataId: fileAttachUpdated.id,
        billFileAttachmentMetadata: fileAttachUpdated,
      };
    });

    return filesToReturn;
  };

  const handleFileUpload = async (
    files: CustomFile[]
  ): Promise<CustomFile[]> => {
    const uploadedFiles = await uploadFileToServices(files);
    setCurrentFiles(prev => [...prev, ...uploadedFiles]);
    fetchFiles();
    return files;
  };

  const optionSetMapping = (options: OptionSet[]): CustomTabOption[] => {
    return options.map(o => ({
      label: o.value,
      disabled: false,
    }));
  };

  const findIndexOnOptionSet = (
    options: OptionSet[],
    value: number | undefined
  ): number => {
    if (!value) return 0;
    return options.findIndex(o => o.attributeValue === value) ?? 0;
  };

  const colorOptionsTab = {
    tabsBackgroundColor: '#666666',
    itemTextColor: '#ffffff',
    selectedItemTextColor: '#000000',
    indicatorBackgroundColor: '#ffffff',
  };

  const onChangeDate = (date: Date | null): void => {
    if (!date) return;
    const dateFormatted = formatDate(date);
    setQuickCollect(prev => {
      return {
        ...prev,
        date: dateFormatted,
      };
    });
  };

  return (
    <div className={classes.saleTab}>
      <CustomDatePicker
        value={quickCollect?.date}
        onChange={onChangeDate}
        label={fm.moveDate}
        classes={{ root: classes.movedDate }}
        placeholder="mm/dd/yyyy"
      />
      <span className={classes.label}>{fm.reasonForSale}</span>
      <CustomTabs
        selectedIndex={findIndexOnOptionSet(
          reasonForSaleOpts,
          quickCollect?.reasonForRequestId
        )}
        ptasVariant="SwitchMedium"
        items={optionSetMapping(reasonForSaleOpts)}
        onSelected={onSelectReasonForSale}
        classes={{
          root: classes.optionTabs,
        }}
        {...colorOptionsTab}
      />
      <span className={classes.label}>{fm.methodOfTransfer}</span>
      <CustomTabs
        selectedIndex={findIndexOnOptionSet(
          methodTransferOpts,
          quickCollect?.methodOfTransferId
        )}
        ptasVariant="SwitchMedium"
        items={optionSetMapping(methodTransferOpts)}
        onSelected={onSelectMethodOfTransfer}
        classes={{
          root: classes.optionTabs,
        }}
        {...colorOptionsTab}
      />
      <span className={classes.label}>{fm.attachBillOfSale}</span>
      <FileUpload
        classes={{
          rootDropzone: classes.fileUploadRoot,
        }}
        currentFiles={currentFiles}
        attrAccept={[
          { extension: '.png', mimeType: 'image/png' },
          { extension: '.jpg', mimeType: 'image/jpeg' },
        ]}
        multipleFilesUploadAtOnce={false}
        bigDropzoneForAdditionalFiles={false}
        onFileUpload={handleFileUpload}
        onRemoveFile={(file): void => {
          console.log('Removing ', file.fileName);
        }}
      />
    </div>
  );
}

export default Sale;
