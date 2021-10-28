// useCurrentUse.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Dispatch,
  SetStateAction,
  useEffect,
  useState,
  useCallback,
  useContext,
} from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { ItemSuggestion, CustomFile } from '@ptas/react-public-ui-library';
import { useMount, useUpdateEffect } from 'react-use';
import { debounce } from 'lodash';
import { apiService } from 'services/api/apiService';
import { currentUseService } from 'services/api/apiService/currentUse';
import { AppContext } from '../../../../contexts/AppContext';
import { v4 as uuidV4 } from 'uuid';
import { APP_STATE_IN_PROGRESS, APP_STATE_NEW } from './constants';
import { FileAttachmentMetadataClass } from 'models/fileAttachmentMetadata';
import { getFileFromUrl } from 'services/common/getFileFromUrl';

interface ExemptionData {
  currentUseApplicationId: string;
  exemptionId: string;
  fileAttachmentIds: string[];
}

interface JsonRelations {
  contactId: string;
  contactEmail: string;
  currentUseApplicationId: string;
  currentExemptionId: string;
  exemptionList: CurrentUseExemption[];
  exemptionData: ExemptionData[];
}

interface JsonTabManager {
  currentTab: string;
  tabsEnabled: boolean;
}

interface UseCurrentUse {
  handleTabChange: (tab: number) => void;
  handleDone: () => void;
  handleClickOnInstructions: () => void;
  handleOnSearchChange: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => void;
  searchList: ItemSuggestion[];
  setSearchList: Dispatch<SetStateAction<ItemSuggestion[]>>;
  exemptionList: CurrentUseExemption[];
  setExemptionList: Dispatch<SetStateAction<CurrentUseExemption[]>>;
  currentExemptionId: string | undefined;
  setCurrentExemptionId: Dispatch<SetStateAction<string | undefined>>;
  onSearchItemSelected: (item: ItemSuggestion) => void;
  currentStep: CurrentUseExemptionStep | undefined;
  setCurrentStep: Dispatch<SetStateAction<CurrentUseExemptionStep | undefined>>;
  highestStepNumber: number;
  setHighestStepNumber: Dispatch<SetStateAction<number>>;
  openLink: (url: string) => void;
  loading: boolean;
  setSearchText: Dispatch<SetStateAction<string>>;
  searchText: string;
  files: CustomFile[];
  onUploadFile: (files: CustomFile[]) => Promise<CustomFile[]>;
  onRemoveFile: (file: CustomFile) => void;
  applicationCompleted: boolean;
  completeAppLoading: boolean;
  openFilePreview: boolean;
  setOpenFilePreview: Dispatch<SetStateAction<boolean>>;
  fileToPreview: CustomFile | undefined;
  setFileToPreview: Dispatch<SetStateAction<CustomFile | undefined>>;
  onZoomInFile: (file: CustomFile) => void;
}

export type CurrentUseExemption = {
  id: string;
  parcelNumber: string;
  applicationDate: number;
  complete: boolean;
  homeAddress: string;
  homeOwner: string;
  homePicture: string;
};
export type CurrentUseExemptionStep = 'farmLand' | 'forestLand' | 'openSpace';
export type CurrentUseParams = {
  exemptionSelected: string;
  contact: string;
  exemption: string;
  step: CurrentUseExemptionStep;
};

function useCurrentUse(): UseCurrentUse {
  const history = useHistory();
  const { exemptionSelected } = useParams<CurrentUseParams>();
  const [exemptionList, setExemptionList] = useState<CurrentUseExemption[]>([]);
  const [currentExemptionId, setCurrentExemptionId] = useState<string>();
  const [currentStep, setCurrentStep] = useState<CurrentUseExemptionStep>();
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchText, setSearchText] = useState<string>('');
  const [applicationCompleted, setApplicationCompleted] = useState<boolean>(
    false
  );
  const [completeAppLoading, setCompleteAppLoading] = useState<boolean>(false);

  const [searchList, setSearchList] = useState<ItemSuggestion[]>([]);
  const { mediaToken, portalContact } = useContext(AppContext);
  const [fileGroup, setFileGroup] = useState<Map<string, CustomFile[]>>(
    new Map()
  );
  const [currentUseAppState, setCurrentUseAppState] = useState<
    Map<string, number>
  >(new Map());
  const [currentUseApplicationId, setCurrentUseApplicationId] = useState<
    string
  >(uuidV4());
  const [fileToPreview, setFileToPreview] = useState<CustomFile | undefined>();
  const [openFilePreview, setOpenFilePreview] = useState<boolean>(false);

  const { id: portalContactId } = portalContact ?? {};

  const jsonRelationsUrl = `portals/currentuse/${portalContactId}/relations`;
  const tabManagerUrl = `portals/currentuse/${portalContactId}/${currentExemptionId}/tabManager`;

  const onZoomInFile = async (file: CustomFile): Promise<void> => {
    if (!file || !file.fileName) return;
    const fileName = file.fileName.toLowerCase();
    if (file.content && fileName.endsWith('.pdf') && !file.file) {
      const newFile = await getFileFromUrl(file.content as string);
      setFileToPreview({
        ...file,
        file: newFile,
      });
    } else {
      setFileToPreview(file);
    }
    setOpenFilePreview(true);
  };

  const loadCurrentUseApplicationStates = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_currentuseapplication',
      'statuscode'
    );
    if (!data) return;
    const fetchedStates = data.map((s): [string, number] => [
      s.value,
      s.attributeValue,
    ]);
    const mapStates = new Map<string, number>(fetchedStates);
    setCurrentUseAppState(mapStates);
  };

  const getJsonRelations = async (): Promise<JsonRelations | undefined> => {
    const { data, hasError } = await apiService.getJson(
      `${jsonRelationsUrl}/relations`
    );
    if (hasError || !data) return;
    return (data as unknown) as JsonRelations;
  };

  const extractCurrentExemptionData = (
    json?: JsonRelations
  ): ExemptionData | undefined => {
    const exempData: ExemptionData[] = json?.exemptionData ?? [];
    return exempData.find(ed => ed.exemptionId === currentExemptionId);
  };

  const saveJsonRelations = async (json: JsonRelations): Promise<boolean> => {
    const { hasError } = await apiService.saveJson(jsonRelationsUrl, json);
    return !hasError;
  };

  const loadDataToCurrentExemption = async (
    jsonFound?: JsonRelations
  ): Promise<void> => {
    const json = jsonFound ?? (await getJsonRelations());
    const currExemptionData = extractCurrentExemptionData(json);
    const tabsManagerJson = await getTabManagerJson();
    setCurrentStep(
      (tabsManagerJson?.currentTab || 'farmLand') as CurrentUseExemptionStep
    );

    let isApplicationApproved = false;
    if (currExemptionData?.currentUseApplicationId) {
      const currentUseFound = await (
        await currentUseService.getCurrentUse(
          currExemptionData.currentUseApplicationId
        )
      ).data;
      isApplicationApproved =
        currentUseAppState.get(APP_STATE_IN_PROGRESS) ===
        currentUseFound?.statuscode;
    }
    setCurrentUseApplicationId(
      currExemptionData?.currentUseApplicationId ?? uuidV4()
    );
    setApplicationCompleted(isApplicationApproved);

    const filesAttIds = currExemptionData?.fileAttachmentIds ?? [];

    let customFiles: CustomFile[] = [];
    if (filesAttIds.length) {
      const filesIdFormatted = filesAttIds.map(f => ({ entityId: f }));
      const fileAttachmentsFound = await (
        await apiService.getFileAttachmentData(filesIdFormatted)
      ).data;

      customFiles = (fileAttachmentsFound ?? []).map(fa => {
        return {
          id: fa.fileAttachmentMetadataId,
          fileName: fa.name,
          content: fa.sharepointUrl || fa.blobUrl,
          contentType: 'publicUrl',
          isUploading: false,
        } as CustomFile;
      });
    }
    // init file list
    initFilesByTabName(customFiles, tabsManagerJson?.currentTab);
  };

  const loadCurrenUseApplication = async (): Promise<void> => {
    const json = await getJsonRelations();

    setExemptionList(json?.exemptionList ?? []);

    const exemptionSelDefault =
      exemptionSelected || json?.currentExemptionId || '';

    if (currentExemptionId && exemptionSelDefault === currentExemptionId) {
      loadDataToCurrentExemption(json);
    } else {
      setCurrentExemptionId(exemptionSelDefault);
    }
  };

  useMount(() => {
    loadCurrentUseApplicationStates();
  });

  useEffect((): void => {
    portalContact && loadCurrenUseApplication();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

  useEffect((): void => {
    if (!portalContact || !currentExemptionId) return;
    loadDataToCurrentExemption();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact, currentExemptionId]);

  useUpdateEffect(() => {
    if (portalContactId && currentExemptionId) {
      saveTabManagerJson({
        currentTab: currentStep as string,
        tabsEnabled: !applicationCompleted,
      });
    }
  }, [currentStep]);

  const getTabManagerJson = async (): Promise<JsonTabManager | undefined> => {
    const { data, hasError } = await apiService.getJson(
      `${tabManagerUrl}/tabManager`
    );
    if (hasError || !data) return;
    return (data as unknown) as JsonTabManager;
  };

  const saveTabManagerJson = async (json: JsonTabManager): Promise<boolean> => {
    const { hasError } = await apiService.saveJson(tabManagerUrl, json);
    return !hasError;
  };

  const handleTabChange = (tab: number): void => {
    if (applicationCompleted) return;
    if (tab === 1) {
      setCurrentStep('forestLand');
    } else if (tab === 2) {
      setCurrentStep('openSpace');
    } else {
      setCurrentStep('farmLand');
    }
  };

  const handleDone = async (): Promise<void> => {
    setCompleteAppLoading(true);
    const jsonFile = await getJsonRelations();
    const currExemptionData = extractCurrentExemptionData(jsonFile);
    const filesId = currExemptionData?.fileAttachmentIds as string[];
    const {
      data,
      hasError: sharePointError,
    } = await apiService.saveFileInSharePoint(
      filesId,
      process.env.REACT_APP_BLOB_CONTAINER_CURRENT_USE ?? ''
    );
    if (sharePointError || !data || data.length === 0)
      return setCompleteAppLoading(false);
    const sharePointDataUpdate = data.map(d => [d.id, d.fileUrl]);
    const { hasError } = await apiService.updateFileAttachmentDataToSharePoint(
      sharePointDataUpdate
    );
    if (hasError) return setCompleteAppLoading(false);
    saveTabManagerJson({
      currentTab: currentStep as string,
      tabsEnabled: false,
    });
    await closeApplication();
    setApplicationCompleted(true);
    setCompleteAppLoading(false);
  };

  const handleOnSearchChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;

    setSearchText(inputValue);
  };

  /**
   * api service, find property
   */
  const getData = useCallback(
    debounce(async valueData => {
      const { data, hasError } = await apiService.getParcel(valueData);

      if (hasError) {
        setSearchList([]);
      }

      const itemSuggestionData = data?.map(propertyItem => {
        return {
          title: propertyItem.ptas_address,
          id: propertyItem.ptas_parceldetailid,
          subtitle: `${
            propertyItem.ptas_district
          }, WA ${propertyItem.ptas_zipcode ?? ''}`,
        };
      });

      setLoading(false);

      setSearchList(itemSuggestionData ?? []);
    }, 500),
    []
  );

  useUpdateEffect(() => {
    getData(searchText);
  }, [searchText]);

  const onSearchItemSelected = async (item: ItemSuggestion): Promise<void> => {
    if (!item.id) return;

    const existItem = exemptionList?.find(
      (itemSearcher): boolean => itemSearcher.id === item.id
    );

    if (existItem) return;

    const { data: selectedCardInfo } = await apiService.getParcelDescription({
      entityId: item.id as string,
      entityName: 'ptas_parceldetail',
    });

    if (!selectedCardInfo) return;

    const { data: imageParcel } = await apiService.getMediaForParcel(
      selectedCardInfo.ptas_name,
      mediaToken
    );

    const photo = imageParcel?.length ? imageParcel[0] : '';

    setExemptionList(prev => {
      return [
        ...prev,
        {
          id: selectedCardInfo?.ptas_parceldetailid,
          parcelNumber: selectedCardInfo.ptas_name,
          applicationDate: Date.now(), // todo: change later
          complete: false, // todo: change later
          homeAddress: `${selectedCardInfo.ptas_address} ${
            selectedCardInfo.ptas_district
          }, WA ${selectedCardInfo.ptas_zipcode ?? ''}`,
          homeOwner: selectedCardInfo.ptas_namesonaccount,
          homePicture: photo,
        },
      ];
    });
    setCurrentExemptionId(selectedCardInfo.ptas_parceldetailid);
  };

  const handleClickOnInstructions = (): void => {
    history.push('/instruction');
  };

  const openLink = (url: string): void => {
    window.open(url);
  };

  // handle file group state
  const getFilesByTabSelected = (): CustomFile[] => {
    const resp = fileGroup.get(currentStep ?? '') ?? [];
    return resp;
  };
  const setFilesByTabSelected = (newFiles: CustomFile[]): void => {
    if (!currentStep) return;
    setFileGroup(prev => {
      const prevMap = new Map(prev);
      prevMap.set(currentStep, newFiles);
      return prevMap;
    });
  };
  const initFilesByTabName = (newFiles: CustomFile[], tab?: string): void => {
    const groupName = tab ?? currentStep;
    setFileGroup(prev => {
      const prevMap = new Map(prev);
      prevMap.clear();
      groupName && prevMap.set(groupName, newFiles);
      return prevMap;
    });
  };
  const addFilesByTabSelected = (newFiles: CustomFile[]): void => {
    if (!currentStep) return;
    setFileGroup(prev => {
      const prevMap = new Map(prev);
      const prevFiles = prevMap.get(currentStep) ?? [];
      // Delete files of another tab (services)
      prevMap.forEach((value, key) => {
        if (key !== currentStep) deleteMultipleFiles(value);
      });
      prevMap.clear(); // clear all map
      prevMap.set(currentStep, [...prevFiles, ...newFiles]);
      return prevMap;
    });
  };
  // end handle file group state

  const uploadFileToServices = async (
    files: CustomFile[]
  ): Promise<[CustomFile[], string[]]> => {
    const fileAttachmentIds: string[] = [];
    const uploadingFiles = files.map(async f => {
      const fileAttachmentId = uuidV4();
      const fileAttachmentMetadataId = f.id ?? fileAttachmentId;
      const content =
        (await (
          await currentUseService.uploadFile(f.file as File, fileAttachmentId)
        ).data) ?? '';
      const newFile = new FileAttachmentMetadataClass();
      newFile.fileAttachmentMetadataId = fileAttachmentMetadataId;
      newFile.icsDocumentId = fileAttachmentMetadataId;
      newFile.name = f.fileName;
      newFile.parcelId = currentExemptionId ?? '';
      newFile.isBlob = true;
      newFile.document = currentStep as string;
      newFile.isSharePoint = false;
      newFile.blobUrl = content as string;
      const saveFileAttachmentResp = await apiService.saveFileAttachmentData(
        newFile
      );
      if (saveFileAttachmentResp.hasError) return;
      fileAttachmentIds.push(newFile.fileAttachmentMetadataId);

      return {
        ...f,
        isUploading: false,
        contentType: 'publicUrl',
        content,
      } as CustomFile;
    });
    const uploadedFiles = await Promise.all(uploadingFiles).catch(() => []);

    const filesToReturn = uploadedFiles.filter(uf => uf) as CustomFile[];

    return [filesToReturn, fileAttachmentIds];
  };

  const createNewApplication = async (): Promise<void> => {
    await currentUseService.createCurrentUseApplication({
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_currentuseapplicationid: currentUseApplicationId,
      statuscode: currentUseAppState.get(APP_STATE_NEW) ?? 0,
      statecode: 0,
      /* eslint-enable @typescript-eslint/camelcase */
    });
  };

  const closeApplication = async (): Promise<void> => {
    await currentUseService.updateCurrentUseApplication({
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_currentuseapplicationid: currentUseApplicationId,
      statuscode: currentUseAppState.get(APP_STATE_IN_PROGRESS) ?? 0,
      /* eslint-enable @typescript-eslint/camelcase */
    });
  };

  const handleApplication = async (
    fileAttachmentIds: string[]
  ): Promise<void> => {
    if (!portalContact) return;
    const { email } = portalContact;
    const json = await getJsonRelations();
    const currExemptionData = extractCurrentExemptionData(json);
    const newExemptionData: ExemptionData[] = [];
    if (!currExemptionData) {
      await createNewApplication();
      newExemptionData.push({
        exemptionId: currentExemptionId,
        currentUseApplicationId,
        fileAttachmentIds,
      } as ExemptionData);
    }
    const exemptionDataUpdated = (json?.exemptionData ?? []).map(
      (d: ExemptionData) => {
        if (d.exemptionId === currentExemptionId) {
          return {
            ...d,
            fileAttachmentIds: [...d.fileAttachmentIds, ...fileAttachmentIds],
          };
        }
        return d;
      }
    );
    const jsonToSave = {
      contactId: portalContactId,
      contactEmail: email?.email ?? '',
      currentUseApplicationId,
      currentExemptionId,
      exemptionList,
      exemptionData: [...exemptionDataUpdated, ...newExemptionData],
    } as JsonRelations;

    await saveJsonRelations(jsonToSave);
  };

  const onUploadFile = async (files: CustomFile[]): Promise<CustomFile[]> => {
    if (applicationCompleted) return [];

    const prevFileAttachmentIds = getFilesByTabSelected().map(
      f => f.id
    ) as string[];

    const [uploadedFiles, newFileAttachmentIds] = await uploadFileToServices(
      files
    );

    await handleApplication([
      ...prevFileAttachmentIds,
      ...newFileAttachmentIds,
    ]);

    addFilesByTabSelected(uploadedFiles);
    return uploadedFiles;
  };

  const deleteMultipleFiles = async (
    filesToDelete: CustomFile[]
  ): Promise<void> => {
    filesToDelete.forEach(f => {
      apiService.deleteFileAttachment(f?.id ?? '');
      apiService.deleteFile(f.fileName, f?.id ?? '');
    });
  };

  const onRemoveFile = async (file: CustomFile): Promise<void> => {
    const prevFiles = getFilesByTabSelected();
    const filesReduced = prevFiles.filter(f => f.id !== file.id);
    const newFileAttIds = filesReduced.map(f => f.id);
    const prevJsonFile = await getJsonRelations();
    const prevCurrentExemData = prevJsonFile?.exemptionData;
    if (!prevJsonFile || !prevCurrentExemData) return;
    const exemptionData = prevCurrentExemData.map(d => {
      if (d.exemptionId === currentExemptionId) {
        return {
          ...d,
          fileAttachmentIds: newFileAttIds as string[],
        };
      }
      return d;
    });
    await saveJsonRelations({
      ...prevJsonFile,
      exemptionData,
    });
    await apiService.deleteFileAttachment(file?.id ?? '');
    await apiService.deleteFile(file.fileName, file?.id ?? '');
    setFilesByTabSelected(filesReduced);
  };

  return {
    handleTabChange,
    handleDone,
    handleClickOnInstructions,
    handleOnSearchChange,
    searchList,
    setSearchList,
    exemptionList,
    setExemptionList,
    currentExemptionId,
    setCurrentExemptionId,
    onSearchItemSelected,
    currentStep,
    setCurrentStep,
    highestStepNumber,
    setHighestStepNumber,
    openLink,
    loading,
    searchText,
    setSearchText,
    files: getFilesByTabSelected(),
    onUploadFile,
    onRemoveFile,
    applicationCompleted,
    completeAppLoading,
    fileToPreview,
    setFileToPreview,
    openFilePreview,
    setOpenFilePreview,
    onZoomInFile,
  };
}

export default useCurrentUse;
