// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosFileInstance, axiosHiInstance } from '../../axiosInstances';
import {
  TaskEntity,
  Task,
} from 'routes/models/Views/HomeDestroyedProperty/Task';
import {
  FileAttachmentMetadataEntity,
  FileAttachmentMetadataTask,
} from 'routes/models/Views/HomeDestroyedProperty/fileAttachment';
import { isEmpty } from 'lodash';

class TaskService {
  /**
   * Get destroyed property application by parcel
   * @param parcelId - Parcel ID
   */
  getDestroyedPropByParcelAndContact = (
    parcelId: string,
    contactId: string
  ): Promise<ApiServiceResult<Task[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_task',
          query: `$filter=_ptas_parcelid_value eq '${parcelId}' and _ptas_portalcontact_value eq '${contactId}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: TaskEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<Task[]>({
        data: res.items?.length
          ? res.items.map((i) => new Task(i.changes))
          : [],
      });
    });
  };

  /**
   * Gets destroyed property app by contact
   * @param contactId - Portal contact ID
   */
  getDestroyedPropByContact = (
    contactId: string
  ): Promise<ApiServiceResult<Task[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_task',
          query: `$filter=_ptas_portalcontact_value eq '${contactId}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: TaskEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<Task[]>({
        data: res.items?.length
          ? res.items.map((i) => new Task(i.changes))
          : [],
      });
    });
  };

  /**
   * Creates or updates a task entity
   * @param newHiApplication - task model
   */
  saveTaskApp = (
    newTask: Task,
    oldDesPropertyApp?: Task
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getDestroyedPropChange(newTask, oldDesPropertyApp);

    if (isEmpty(changes)) {
      return new Promise((res) =>
        res(
          new ApiServiceResult<unknown>({
            data: {},
          })
        )
      );
    }

    const data = {
      items: [
        {
          entityName: 'ptas_task',
          entityId: newTask.id,
          changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosHiInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * get file attachment metadata by task
   * @param taskPk - destroyed property pk
   */
  getFileAttachmentByTask = (
    taskPk: string
  ): Promise<ApiServiceResult<FileAttachmentMetadataTask[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_task',
          // eslint-disable-next-line no-irregular-whitespace
          query: `$select=ptas_taskid&$filter=ptas_taskid eq '${taskPk}'&$expand=ptas_ptas_task_ptas_fileattachmentmetadata`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosHiInstance.post<{
          items: {
            changes: {
              ptas_ptas_task_ptas_fileattachmentmetadata: FileAttachmentMetadataEntity[];
            };
          }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<FileAttachmentMetadataTask[]>({
        data:
          result?.items[0]?.changes.ptas_ptas_task_ptas_fileattachmentmetadata.map(
            (file) => new FileAttachmentMetadataTask(file)
          ) ?? [],
      });
    });
  };

  /**
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   */
  saveFileAttachment = async (
    fileAttachment: FileAttachmentMetadataTask
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.fileAttachmentMetadataId,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_fileattachmentmetadataid:
              fileAttachment.fileAttachmentMetadataId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_name: fileAttachment.name,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_parcelid_value: fileAttachment.parcelId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_bloburl: fileAttachment.blobUrl,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_portaldocument: fileAttachment.document,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_portalsection: fileAttachment.section,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_icsdocumentid: fileAttachment.icsDocumentId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_isblob: fileAttachment.isBlob,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_issharepoint: fileAttachment.isSharePoint,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_filelibrary: fileAttachment.fileLibrary,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_originalfilename: fileAttachment.originalName,
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosHiInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * Update file attachment to indicate that the file has been moved to sharepoint.
   * @param fileAttachment - file attachment metadata model
   */
  updateFileAttachmentMetadata = async (
    fileAttachment: FileAttachmentMetadataTask
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.fileAttachmentMetadataId,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_bloburl: '',
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_isblob: fileAttachment.isBlob,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_issharepoint: fileAttachment.isSharePoint,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_sharepointurl: fileAttachment.sharepointUrl,
            // eslint-disable-next-line @typescript-eslint/camelcase
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosHiInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * delete a file attachment metadata entity
   * @param fileAttachmentPk - file attachment metadata pk
   */
  deleteFileAttachment = async (
    fileAttachmentPk: string,
    taskPk: string,
    fileName: string
  ): Promise<ApiServiceResult<unknown>> => {
    const { hasError } = await this.deleteFile(fileName, fileAttachmentPk);

    if (hasError) {
      console.log('Error to delete file');
    }

    // delete relation between task - file attachment
    const {
      hasError: errorRelation,
    } = await this.createOrDeleteFileAttachmentTaskRelation(
      taskPk,
      fileAttachmentPk,
      true
    );

    if (errorRelation) {
      console.log('Error to upload file to FileStore');
    }

    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachmentPk,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/DeleteItems';
      const result = await axiosHiInstance.delete<{
        results: { id: string; result: boolean }[];
      }>(url, {
        headers: {
          Authorization: 'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
        },
        data: data,
      });
      return new ApiServiceResult<{
        id: string;
        result: boolean;
      }>({
        data: result.data.results?.length ? result.data.results[0] : undefined,
      });
    });
  };

  deleteFile = (
    fileName: string,
    id: string
  ): Promise<ApiServiceResult<{ message: string }>> => {
    return handleReq(async () => {
      const url = 'v1.0/api/FileStore';
      const data = {
        containerName: process.env.REACT_APP_BLOB_CONTAINER_TASK,
        id,
        fileName: fileName.replace(/%20/g, ' '),
      };

      const urlFile = (
        await axiosFileInstance.delete<{ message: string }>(url, {
          data,
        })
      ).data;

      return new ApiServiceResult<{ message: string }>({
        data: urlFile,
      });
    });
  };

  /**
   * Creates a relationship between file attachment metadata and task
   * @param taskPk - file attachment metadata model
   * @param fileAttachmentPk - file attachment metadata model
   * @param isDelete - flag to save or delete relationship
   */
  createOrDeleteFileAttachmentTaskRelation = async (
    taskPk: string,
    fileAttachmentPk: string,
    isDelete: boolean
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      entityName: 'ptas_task',
      entityId: taskPk,
      counterpartEntityName: 'ptas_fileattachmentmetadata',
      counterparId: fileAttachmentPk,
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/${
        isDelete ? 'UnlinkEntities' : 'LinkEntities'
      }`;
      const result = (
        await axiosHiInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * upload file
   * @param file - any file
   * @param id - entity ID used to group files in folder
   * @returns url
   */
  uploadFile = (file: File, id: string): Promise<ApiServiceResult<string>> => {
    const bodyFD = new FormData();
    bodyFD.append('File', file);

    return handleReq(async () => {
      const url = 'v1.0/api/FileStore';

      const urlFile = (
        await axiosFileInstance.post<string[]>(url, bodyFD, {
          headers: {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
          },
          params: {
            containerName: process.env.REACT_APP_BLOB_CONTAINER_TASK,
            id,
            includeSAS: 'false',
          },
        })
      ).data;

      return new ApiServiceResult<string>({
        data: urlFile[0] ?? '',
      });
    });
  };

  getDestroyedPropChange = (
    newDesPropertyApp: Task,
    oldDesPropertyApp?: Task
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};

    if (oldDesPropertyApp) {
      if (
        oldDesPropertyApp.taxAccountNumber !==
        newDesPropertyApp.taxAccountNumber
      ) {
        changes['_ptas_taxaccountnumber_value'] =
          newDesPropertyApp.taxAccountNumber;
      }
      if (
        oldDesPropertyApp.submissionSource !==
        newDesPropertyApp.submissionSource
      ) {
        changes['ptas_submissionsource'] = newDesPropertyApp.submissionSource;
      }
      if (oldDesPropertyApp.parcelId !== newDesPropertyApp.parcelId) {
        changes['_ptas_parcelid_value'] = newDesPropertyApp.parcelId;
      }
      if (oldDesPropertyApp.phoneNumber !== newDesPropertyApp.phoneNumber) {
        changes['ptas_phonenumber'] = newDesPropertyApp.phoneNumber;
      }
      if (oldDesPropertyApp.id !== newDesPropertyApp.id) {
        changes['ptas_taskid'] = newDesPropertyApp.id;
      }
      if (oldDesPropertyApp.taxPayerName !== newDesPropertyApp.taxPayerName) {
        changes['ptas_taxpayername'] = newDesPropertyApp.taxPayerName;
      }
      if (
        oldDesPropertyApp.dateOfDestruction !==
        newDesPropertyApp.dateOfDestruction
      ) {
        changes['ptas_dateofdestruction'] = newDesPropertyApp.dateOfDestruction;
      }
      if (
        oldDesPropertyApp.destroyedPropertyDescription !==
        newDesPropertyApp.destroyedPropertyDescription
      ) {
        changes['ptas_destroyedpropertydescription'] =
          newDesPropertyApp.destroyedPropertyDescription;
      }
      if (
        oldDesPropertyApp.lossOccurringAsAResultOf !==
        newDesPropertyApp.lossOccurringAsAResultOf
      ) {
        changes['ptas_lossoccurringasaresultof'] =
          newDesPropertyApp.lossOccurringAsAResultOf;
      }
      if (
        oldDesPropertyApp.anticipatedRepairDates !==
        newDesPropertyApp.anticipatedRepairDates
      ) {
        changes['ptas_anticipatedrepairdates'] =
          newDesPropertyApp.anticipatedRepairDates;
      }
      if (
        oldDesPropertyApp.repairDateUnknownAtThisTime !==
        newDesPropertyApp.repairDateUnknownAtThisTime
      ) {
        changes[
          'ptas_repairdatesunknownatthistime'
        ] = !!newDesPropertyApp.repairDateUnknownAtThisTime;
      }
      if (oldDesPropertyApp.portalContact !== newDesPropertyApp.portalContact) {
        changes['_ptas_portalcontact_value'] = newDesPropertyApp.portalContact;
      }
      if (oldDesPropertyApp.signedBy !== newDesPropertyApp.signedBy) {
        changes['ptas_signedby'] = newDesPropertyApp.signedBy;
      }
      if (oldDesPropertyApp.stateCode !== newDesPropertyApp.stateCode) {
        changes['statecode'] = newDesPropertyApp.stateCode;
      }
      if (oldDesPropertyApp.statusCode !== newDesPropertyApp.statusCode) {
        changes['statuscode'] = newDesPropertyApp.statusCode;
      }
      if (oldDesPropertyApp.taskType !== newDesPropertyApp.taskType) {
        changes['ptas_tasktype'] = newDesPropertyApp.taskType;
      }
      if (
        oldDesPropertyApp.permitIssuedBy !== newDesPropertyApp.permitIssuedBy
      ) {
        changes['ptas_permitissuedby'] = newDesPropertyApp.permitIssuedBy;
      }
      if (oldDesPropertyApp.othercomments !== newDesPropertyApp.othercomments) {
        changes['ptas_lossoccurringasaresultofother'] =
          newDesPropertyApp.othercomments;
      }
      if (oldDesPropertyApp.cityStateZip !== newDesPropertyApp.cityStateZip) {
        changes['ptas_citystatezip'] = newDesPropertyApp.othercomments;
      }
      if (
        oldDesPropertyApp.propertyAddress !== newDesPropertyApp.propertyAddress
      ) {
        changes['ptas_propertyaddress'] = newDesPropertyApp.propertyAddress;
      }
      if (oldDesPropertyApp.dateSigned !== newDesPropertyApp.dateSigned) {
        changes['ptas_datesigned'] = newDesPropertyApp.dateSigned;
      }
    } else {
      changes['_ptas_taxaccountnumber_value'] =
        newDesPropertyApp.taxAccountNumber;
      changes['ptas_submissionsource'] = newDesPropertyApp.submissionSource;
      changes['_ptas_parcelid_value'] = newDesPropertyApp.parcelId;
      changes['ptas_phonenumber'] = newDesPropertyApp.phoneNumber;
      changes['ptas_taskid'] = newDesPropertyApp.id;
      changes['ptas_taxpayername'] = newDesPropertyApp.taxPayerName;
      changes['ptas_dateofdestruction'] = newDesPropertyApp.dateOfDestruction;
      changes['ptas_destroyedpropertydescription'] =
        newDesPropertyApp.destroyedPropertyDescription;
      changes['ptas_lossoccurringasaresultof'] =
        newDesPropertyApp.lossOccurringAsAResultOf;
      changes['ptas_anticipatedrepairdates'] =
        newDesPropertyApp.anticipatedRepairDates;
      changes[
        'ptas_repairdatesunknownatthistime'
      ] = !!newDesPropertyApp.repairDateUnknownAtThisTime;
      changes['_ptas_portalcontact_value'] = newDesPropertyApp.portalContact;
      changes['ptas_signedby'] = newDesPropertyApp.signedBy;
      changes['statecode'] = newDesPropertyApp.stateCode;
      changes['statuscode'] = newDesPropertyApp.statusCode;
      changes['ptas_tasktype'] = newDesPropertyApp.taskType;
      changes['ptas_permitissuedby'] = newDesPropertyApp.permitIssuedBy;
      changes['ptas_lossoccurringasaresultofother'] =
        newDesPropertyApp.othercomments;
      changes['ptas_citystatezip'] = newDesPropertyApp.othercomments;
      changes['ptas_propertyaddress'] = newDesPropertyApp.propertyAddress;
    }

    return changes;
  };
}

export const taskService = new TaskService();
