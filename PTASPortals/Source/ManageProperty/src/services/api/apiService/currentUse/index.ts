// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CurrentUseApplication } from 'routes/models/Views/CurrentUse/models/currentUse';
import { ApiServiceResult, handleReq } from 'services/common';
import { axiosFileInstance, axiosHiInstance } from '../../axiosInstances';

class CurrentUseService {
  /**
   * Gets a tax account
   * @param taxAccountId - ID of tax account
   */
  getCurrentUse = (
    taxAccountId: string
  ): Promise<ApiServiceResult<CurrentUseApplication | undefined>> => {
    const data = {
      requests: [
        {
          entityId: taxAccountId,
          entityName: 'ptas_currentuseapplication',
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: CurrentUseApplication }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<CurrentUseApplication>({
        data: res.items?.length ? res.items[0].changes : undefined,
      });
    });
  };
  /**
   * Creates current use application
   * @param currentUseApplication - currentUseApplication
   */
  createCurrentUseApplication = (
    currentUseApplication: CurrentUseApplication
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_currentuseapplication',
          entityId: currentUseApplication?.ptas_currentuseapplicationid,
          changes: currentUseApplication,
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
   * update current use application
   * @param currentUseApplication - currentUseApplication
   */
  updateCurrentUseApplication = (
    currentUseApplication: Partial<CurrentUseApplication>
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_currentuseapplication',
          entityId: currentUseApplication?.ptas_currentuseapplicationid,
          changes: currentUseApplication,
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
            containerName: process.env.REACT_APP_BLOB_CONTAINER_CURRENT_USE,
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
  /**
   * delete file
   * @param fileName - file name with ext
   * @param id - folder id
   * @returns url string
   */
  deleteFile = (
    fileName: string,
    id: string
  ): Promise<ApiServiceResult<{ message: string }>> => {
    return handleReq(async () => {
      const url = 'v1.0/api/FileStore';
      const data = {
        containerName: process.env.REACT_APP_BLOB_CONTAINER_CURRENT_USE,
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
}

export const currentUseService = new CurrentUseService();
