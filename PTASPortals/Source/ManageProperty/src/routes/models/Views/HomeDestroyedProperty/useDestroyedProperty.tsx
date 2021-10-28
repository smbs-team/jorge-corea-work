// useNewBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext, useState, useEffect, useRef } from 'react';
import {
  ErrorMessageContext,
  ItemSuggestion,
} from '@ptas/react-public-ui-library';
import { DestroyedPropContextProps } from 'contexts/DestroyedProperty';
import { Task } from './Task';
import { DestroyedPropContext } from 'contexts/DestroyedProperty';
import { apiService } from 'services/api/apiService';
import { taskService } from 'services/api/apiService/task';
import { isEqual } from 'lodash';
import { AppContext } from 'contexts/AppContext';
import { v4 as uuid } from 'uuid';
import { Property } from 'services/map/model/parcel';
import { TaxAccount } from 'services/map/model/taxAccount';
import { DOCUMENT_SECTION_DESTRUCTION, STATE_CODE_ACTIVE } from './constants';

interface DestroyedPropertyType extends DestroyedPropContextProps {
  saveTask: (key: keyof Task, value: string) => void;
  getAttributeVal: (
    propertyName: string,
    optSetVal: string
  ) => Promise<number | undefined | void>;
  loadInitialFilesByDestroyedProp: () => Promise<void>;
  savePartialTask: () => Promise<void>;
  saveStep: (
    highestStep: number,
    currentStep: string,
    applicationId: string
  ) => Promise<void>;
  loadStateStatusCode: () => Promise<void>;
  createInitialTaskApp: (parcelId: string) => Promise<void>;
  loadDestroyedPropertyApps: (
    destroyedPropertyId: string,
    parcelId: string
  ) => Promise<void>;
  getParcelInfo: (
    valueData: string
  ) => Promise<{
    list: ItemSuggestion[];
    isLoading: boolean;
  } | void>;
  getAppsByContact: (contactId: string) => Promise<Task[] | void>;
  loadFileLibrary: () => Promise<void>;
}

const useDestroyedProperty = (): DestroyedPropertyType => {
  const context = useContext(DestroyedPropContext);
  const { portalContact, mediaToken } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const handleInitialTaskRef = useRef<() => void>();

  const [dataInitialTask, setDataInitialTask] = useState<{
    parcelId: string;
    parcels: Property[];
  }>({
    parcelId: '',
    parcels: [],
  });

  const {
    setTask,
    task,
    setFileAttachmentMetadataDestruction,
    setFileAttachmentDoc,
    oldDesPropertyApp,
    setOldDesPropertyApp,
    jsonSteps,
    setJsonSteps,
    setStateCode,
    setStatusCode,
    setDestroyedPropertyList,
    stateCode,
    setParcelInfo,
    statusCode,
    parcelInfo,
    setFileLibraryList,
    setInitialApps,
  } = context;

  /**
   * Save or update entity task state
   * @returns void
   */
  const saveTask = (key: keyof Task, value: string | number | Date): void =>
    setTask((prev) => ({
      ...prev,
      [key]: value,
    }));

  const getAttributeVal = async (
    propertyName: string,
    optSetVal: string
  ): Promise<number | undefined> => {
    const { data } = await apiService.getOptionSet('ptas_task', propertyName);

    if (!data) return;

    const attrVal = data.find((so) => so.value === optSetVal)?.attributeValue;

    return attrVal;
  };

  const loadInitialFilesByDestroyedProp = async (): Promise<void> => {
    if (!task?.id) return;

    const {
      data: fileAttachmentData,
      hasError,
    } = await taskService.getFileAttachmentByTask(task.id);

    if (hasError) return console.log('ERROR to get file attachment');

    const destructionsFile = (fileAttachmentData || []).filter(
      (fileAttachment) => {
        return (
          fileAttachment.section === DOCUMENT_SECTION_DESTRUCTION &&
          fileAttachment.isBlob
        );
      }
    );

    const documentsAfterSubmission = (fileAttachmentData || []).filter(
      (fd) => fd.section === 'DocumentAfterSubmission' && fd.isBlob
    );
    setFileAttachmentMetadataDestruction(destructionsFile);
    setFileAttachmentDoc(documentsAfterSubmission);
  };

  /**
   * create or update application task
   */
  const savePartialTask = async (): Promise<void> => {
    if (!task) return;

    const { hasError } = await taskService.saveTaskApp(task, oldDesPropertyApp);

    if (hasError) {
      return setErrorMessageState({
        open: true,
        errorDesc: 'Error save destroyed property app',
      });
    }

    // update old task to task saved
    setOldDesPropertyApp(task);

    setDestroyedPropertyList((prev) => [...prev, task]);
  };

  /**
   * save tabs in json
   *
   */
  const saveStep = async (
    highestStep: number,
    currentStep: string,
    applicationId: string
  ): Promise<void> => {
    const steps = {
      highestStep,
      currentStep,
    };

    if (isEqual(steps, jsonSteps)) return;

    setJsonSteps(steps);

    await apiService.saveJson(
      `portals/${portalContact?.id}/${applicationId}/step`,
      steps
    );
  };

  const getOptionSetMapping = async (
    fieldName: string
  ): Promise<Map<string, number> | void> => {
    const { data } = await apiService.getOptionSet('ptas_task', fieldName);

    if (!data) return;

    const optionSetMap = new Map<string, number>(
      data.map((code) => [code.value, code.attributeValue])
    );

    return optionSetMap;
  };

  const loadStateStatusCode = async (): Promise<void> => {
    const statusCodeMap = await getOptionSetMapping('statuscode');
    setStatusCode(statusCodeMap ? statusCodeMap : new Map());
    const stateCodeMap = await getOptionSetMapping('statecode');
    setStateCode(stateCodeMap ? stateCodeMap : new Map());
  };

  const loadFileLibrary = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_fileattachmentmetadata',
      'ptas_filelibrary'
    );

    if (!data) return;

    const optionSetMap = new Map<string, number>(
      data.map((code) => [code.value, code.attributeValue])
    );

    setFileLibraryList(optionSetMap);
  };

  const createInitialTaskApp = async (parcelId: string): Promise<void> => {
    if (!stateCode.size || !statusCode.size) return;

    const { data: parcels } = await apiService.getParcels([parcelId]);
    const tempTask = new Task();

    // set a temporary task to display in the ui while requests are being completed
    setDestroyedPropertyList((prev) => [
      ...prev,
      {
        ...tempTask,
        id: 'temptask',
      },
    ]);

    if (!parcels?.length) return;

    const parcel = parcels[0];

    const parcelList = [...parcelInfo, parcel];

    // save parcel info
    setParcelInfo((prev) => {
      return [
        ...prev,
        {
          ...parcel,
          // eslint-disable-next-line @typescript-eslint/camelcase
          ptas_address: `${parcel.ptas_address} ${parcel.ptas_district}, WA ${
            parcel.ptas_zipcode ?? ''
          }`,
        },
      ];
    });

    // create new task
    const submissionSource = await getAttributeVal(
      'ptas_submissionsource',
      'Online'
    );
    // TODO: gl uncomment this line
    const taskType = await getAttributeVal(
      'ptas_tasktype',
      'Destroyed property claim'
    );

    const stateCodeVal = stateCode.get(STATE_CODE_ACTIVE);

    const signedBy = await getAttributeVal('ptas_signedby', 'Taxpayer');

    const taxAccountInfo = await getTaxAccountData(
      parcel._ptas_taxaccountid_value
    );

    const newTask = new Task();
    newTask.parcelId = parcel.ptas_parceldetailid;
    newTask.email = portalContact?.email?.email ?? '';
    newTask.submissionSource = submissionSource ? submissionSource : null;
    newTask.taskType = taskType ? taskType : null;
    newTask.portalContact = portalContact?.id ?? '';
    //#region  tax account info
    newTask.email =
      taxAccountInfo?.ptas_email || portalContact?.email?.email || '';
    if (parcel._ptas_taxaccountid_value) {
      newTask.taxAccountNumber = parcel._ptas_taxaccountid_value;
    }
    newTask.phoneNumber =
      taxAccountInfo?.ptas_phone1?.trim() ??
      portalContact?.phone?.phoneNumber?.trim() ??
      '';
    if (taxAccountInfo?.ptas_taxpayername) {
      newTask.taxPayerName = taxAccountInfo?.ptas_taxpayername;
    }
    if (taxAccountInfo?.ptas_taxpayername) {
      newTask.taxPayerName = taxAccountInfo?.ptas_taxpayername;
    }
    //#endregion  tax account info
    newTask.signedBy = signedBy ? signedBy : null;
    newTask.statusCode = statusCode.get('Not started') ?? null;
    newTask.stateCode = stateCodeVal !== undefined ? stateCodeVal : null;
    newTask.cityStateZip = [
      parcel._ptas_addr1_cityid_value,
      parcel._ptas_addr1_stateid_value,
      parcel._ptas_addr1_zipcodeid_value,
    ]
      .filter((val) => val)
      .join(',');
    newTask.propertyAddress = `${parcel.ptas_address} ${
      parcel.ptas_district
    }, WA ${parcel.ptas_zipcode ?? ''}`;
    newTask.id = uuid();

    setTask(newTask);

    setOldDesPropertyApp(new Task());
    setDestroyedPropertyList((prev) => [
      // delete temp task
      ...prev.filter((t) => t.id !== 'temptask'),
      newTask,
    ]);

    loadParcelPicture(parcelList);
  };

  const loadParcelPicture = async (parcels: Property[]): Promise<void> => {
    if (!parcels.length) return;

    const pictureFetching = parcels.map(async (parcel) => {
      if (parcel.picture) return parcel;

      const { data: parcelImage } = await apiService.getMediaForParcel(
        parcel.ptas_name,
        mediaToken
      );

      const photo = parcelImage?.length
        ? parcelImage[0]
        : 'https://static.thenounproject.com/png/1583624-200.png';

      return {
        ...parcel,
        picture: photo,
      };
    });

    const newParcels = await Promise.all(pictureFetching).catch(() => []);

    setParcelInfo(newParcels);
  };

  /**
   * get taxAccountData from parcel
   * @returns void
   */
  const getTaxAccountData = async (
    taxAccountId: string
  ): Promise<TaxAccount | undefined> => {
    if (!taxAccountId) return undefined;

    const { data: taxAccount, hasError } = await apiService.getTaxAccount(
      taxAccountId
    );

    if (hasError) {
      setErrorMessageState({
        open: true,
        errorHead: 'Error getting account info',
      });

      return undefined;
    }

    return taxAccount;
  };

  const getAppsByContact = async (
    contactId: string
  ): Promise<Task[] | void> => {
    if (!contactId) return;

    const { data: appsFound } = await taskService.getDestroyedPropByContact(
      contactId
    );

    // set initials apps list
    return appsFound;
  };

  const loadDestroyedPropertyApps = async (
    destroyedPropertyId: string,
    parcelId: string
  ): Promise<void> => {
    const contactId = portalContact?.id;

    if (!contactId) return;

    const propertyWithEmails = (await getAppsByContact(contactId)) || [];

    // get initials parcel data list
    const parcelIds = (propertyWithEmails || []).map((app) => app.parcelId);

    const { data: parcelsFound } = await apiService.getParcels([
      ...parcelIds,
      parcelId,
    ]);

    // get parcel image
    const parcels = parcelsFound || [];

    setParcelInfo(parcels);

    setDataInitialTask({
      parcels,
      parcelId,
    });

    // set initials apps list
    setDestroyedPropertyList(propertyWithEmails);
    setInitialApps(propertyWithEmails);

    // load property by id
    const initialProperty = propertyWithEmails.find(
      (p) => p.id === destroyedPropertyId
    );

    if (initialProperty) {
      setTask(initialProperty);
      setOldDesPropertyApp(initialProperty);
    }
  };

  const handleInitialTask = (): void => {
    const { parcels, parcelId } = dataInitialTask;

    if (!parcels.length) return;

    if (parcelId) {
      createInitialTaskApp(parcelId);
      return;
    }

    loadParcelPicture(parcels);
  };

  handleInitialTaskRef.current = handleInitialTask;

  useEffect(() => {
    const { current } = handleInitialTaskRef;
    if (!current) return;
    current();
  }, [dataInitialTask]);

  /**
   * api service get parcel info
   */
  const getParcelInfo = async (
    valueData: string
  ): Promise<{
    list: ItemSuggestion[];
    isLoading: boolean;
  } | void> => {
    const { data, hasError } = await apiService.getParcel(valueData);

    if (hasError) {
      return setErrorMessageState({
        open: true,
      });
    }

    if (!data || !data.length)
      return {
        isLoading: false,
        list: [],
      };

    const itemSuggestionData = data.map((propertyItem) => {
      return {
        title: propertyItem.ptas_address,
        id: propertyItem.ptas_parceldetailid,
        subtitle: `${propertyItem.ptas_district}, WA ${
          propertyItem.ptas_zipcode ?? ''
        }`,
      };
    });

    return {
      isLoading: false,
      list: itemSuggestionData,
    };
  };

  return {
    saveTask,
    getAttributeVal,
    savePartialTask,
    saveStep,
    loadInitialFilesByDestroyedProp,
    loadStateStatusCode,
    createInitialTaskApp,
    loadDestroyedPropertyApps,
    getParcelInfo,
    getAppsByContact,
    loadFileLibrary,
    ...context,
  };
};

export default useDestroyedProperty;
