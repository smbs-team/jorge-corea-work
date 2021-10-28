// apiService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import {
  axiosInstance,
  axiosFileInstance,
  axiosPerPropInstance,
  axiosSharePointInstance,
} from '../axiosInstances';

import { OptionSet } from 'models/optionSet';
import {
  CityEntity,
  StateEntity,
  ZipCodeEntity,
  CountryEntity,
} from 'models/addresses';
import {
  FileAttachmentMetadata,
  FileAttachmentMetadataEntityFields,
} from 'models/fileAttachmentMetadata';

interface GenDynamicsGetParams {
  entityId?: string;
  entityName?: string;
  query?: string;
}

interface SharePointResponse {
  hasError: boolean;
  message: string;
  fileUrl: string;
  id: string;
}

class ApiService {
  /**
   * Get valid File Storage SAS token to read media files.
   * @returns SAS Token
   */
  getMediaToken = (): Promise<ApiServiceResult<string>> =>
    handleReq(async () => {
      const r = (
        await axiosInstance.get<string>(
          `${process.env.REACT_APP_MEDIA_API_WITHOUT_TOKEN}/GetSAS`
        )
      ).data;

      return new ApiServiceResult({
        data: r,
      });
    });

  /**
   * Gets an option set
   * @param objectId - Name of entity
   * @param optionSetId - Name of field of type option set
   */
  getOptionSet = (
    objectId: string,
    optionSetId: string
  ): Promise<ApiServiceResult<OptionSet[] | undefined>> =>
    handleReq(async () => {
      const url = `/optionsets/${objectId}/${optionSetId}`;

      const optionSet = await axiosPerPropInstance.get<OptionSet[]>(url);

      return new ApiServiceResult<OptionSet[]>({
        data: optionSet.data,
      });
    });

  //#region Get countries, states, cities and zip codes

  /**
   * Get countries
   */
  getCountries = (): Promise<ApiServiceResult<CountryEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_country',
          query: `$select=ptas_countryid,ptas_name`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: { changes: CountryEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<CountryEntity[]>({
        data: res.items?.length ? res.items.map((i) => i.changes) : [],
      });
    });
  };

  /**
   * Get states
   */
  getStates = (
    lookupValue: string,
    countryId: string
  ): Promise<ApiServiceResult<StateEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$select=ptas_stateorprovinceid,ptas_name,ptas_abbreviation,_ptas_countryid_value
                   &$top=4
                   &$orderby=ptas_name asc
                   &$filter=_ptas_countryid_value eq '${countryId}' and contains(ptas_name, '${lookupValue}')`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: { changes: StateEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<StateEntity[]>({
        data: res.items?.length ? res.items.map((i) => i.changes) : [],
      });
    });
  };

  /**
   * Get cities
   */
  getCities = (
    lookupValue: string
  ): Promise<ApiServiceResult<CityEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_city',
          query: `$select=ptas_cityid,ptas_name
                   &$top=4
                   &$orderby=ptas_name asc
                   &$filter=contains(ptas_name, '${lookupValue}')`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: { changes: CityEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<CityEntity[]>({
        data: res.items?.length ? res.items.map((i) => i.changes) : [],
      });
    });
  };

  /**
   * Get zip codes
   */
  getZipCodes = (
    lookupValue: string
  ): Promise<ApiServiceResult<ZipCodeEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_zipcode',
          query: `$select=ptas_zipcodeid,ptas_name
                   &$top=10
                   &$filter=startswith(ptas_name, '${lookupValue}')`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: { changes: ZipCodeEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<ZipCodeEntity[]>({
        data: res.items?.length ? res.items.map((i) => i.changes) : [],
      });
    });
  };

  //#endregion

  /**
   * generic get file attachment metadata
   * @param params - request, entityName field is not required
   */
  getFileAttachmentData = (
    params: GenDynamicsGetParams[]
  ): Promise<ApiServiceResult<FileAttachmentMetadata[]>> => {
    const paramsFormatted = params.map((p) => ({
      ...p,
      entityName: 'ptas_fileattachmentmetadata',
    }));
    const data = {
      requests: paramsFormatted,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: {
            changes: FileAttachmentMetadataEntityFields;
          }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<FileAttachmentMetadata[]>({
        data:
          result?.items.map(
            ({ changes }) =>
              new FileAttachmentMetadata(
                changes as FileAttachmentMetadataEntityFields
              )
          ) ?? ([] as FileAttachmentMetadata[]),
      });
    });
  };

  /**
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   */
  saveFileAttachmentData = (
    fileAttachment: FileAttachmentMetadata
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.id,
          changes: fileAttachment.getEntityFields(),
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
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
   * @param fileAttachments - [fileAttachmentId, url][]
   */
  updateFileAttachmentDataToSharePoint = async (
    fileAttachments: string[][]
  ): Promise<ApiServiceResult<unknown>> => {
    const items = fileAttachments.map(([id, url]) => {
      return {
        entityName: 'ptas_fileattachmentmetadata',
        entityId: id,
        changes: {
          /* eslint-disable @typescript-eslint/camelcase */
          ptas_bloburl: '',
          ptas_isblob: false,
          ptas_issharepoint: true,
          ptas_sharepointurl: url,
          /* eslint-enable @typescript-eslint/camelcase */
        },
      };
    });
    const data = {
      items,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  //TODO: use generic delete entity method
  /**
   * Deletes a file attachment metadata entity
   * @param fileAttachmentId - ID of file attachment metadata entity
   */
  deleteFileAttachment = (
    fileAttachmentId: string
  ): Promise<
    ApiServiceResult<{
      id: string;
      result: boolean;
    }>
  > => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachmentId,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/DeleteItems';
      const result = await axiosPerPropInstance.delete<{
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
            containerName: process.env.REACT_APP_BLOB_CONTAINER,
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
        containerName: process.env.REACT_APP_BLOB_CONTAINER,
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
   * upload file to sharePoint
   * @param file - any file
   * @param id - entity ID used to group files in folder
   * @returns url
   */
  uploadFileToSharePoint = (
    file: File,
    id: string
  ): Promise<ApiServiceResult<string>> => {
    const bodyFD = new FormData();
    bodyFD.append('File', file);

    return handleReq(async () => {
      const url = `/PutFileCollection/${id}`;

      const urlFile = (
        await axiosSharePointInstance.post<string[]>(url, bodyFD, {
          headers: {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
          },
          params: {
            fullRoute: true,
          },
        })
      ).data;

      return new ApiServiceResult<string>({
        data: urlFile[0] ?? '',
      });
    });
  };

  /**
   * Get entity info
   * @param entityName -
   * @param query -
   */
  getGenericDynamics = <T>(
    requests: {
      entityName: string;
      query: string;
    }[]
  ): Promise<ApiServiceResult<T | undefined>> => {
    const data = {
      requests,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const task = (
        await axiosPerPropInstance.post<{
          items: { changes: T }[];
        }>(url, data)
      ).data;

      return new ApiServiceResult<T>({
        data: task.items[0].changes,
      });
    });
  };

  /**
   * Get entity info
   * @param entityName -
   * @param query -
   */
  getGenericDynamicsAll = <T>(
    requests: {
      entityName: string;
      query: string;
    }[]
  ): Promise<ApiServiceResult<T[] | undefined>> => {
    const data = {
      requests,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const task = (
        await axiosPerPropInstance.post<{
          items: { changes: T }[];
        }>(url, data)
      ).data;

      return new ApiServiceResult<T[]>({
        data: task.items.map((el) => el.changes),
      });
    });
  };

  /**
   * Deletes a generic entity
   * @param entityName - Name of entity
   * @param id - ID of entity
   */
  deleteEntity = (
    entityName: string,
    id: string
  ): Promise<
    ApiServiceResult<{
      id: string;
      result: boolean;
    }>
  > => {
    const data = {
      items: [
        {
          entityName: entityName,
          entityId: id,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/DeleteItems';
      const result = await axiosPerPropInstance.delete<{
        results: { id: string; result: boolean }[];
      }>(url, {
        headers: {
          Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
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
  //#region JSON STORAGE
  /**
   * save json in jsonStore service
   * @param route -
   * @param json -
   * @returns object
   */

  saveJson = (
    route: string,
    json: object
  ): Promise<
    ApiServiceResult<{ result: string; error: boolean; errorMessage: string }>
  > => {
    return handleReq(async () => {
      const url = `v1.0/api/JsonStore?route=${route}&bypassBearer=true`;

      const result = (
        await axiosFileInstance.post<{
          result: string;
          error: boolean;
          errorMessage: string;
        }>(url, json)
      ).data;

      return new ApiServiceResult<{
        result: string;
        error: boolean;
        errorMessage: string;
      }>({
        data: result,
      });
    });
  };

  /**
   * get json from jsonStore service
   * @param route -
   * @returns object
   */
  getJson = (
    route: string
  ): Promise<
    ApiServiceResult<{
      [key: string]: string | number | string[];
    }>
  > => {
    return handleReq(async () => {
      const url = `v1.0/api/JsonStore?route=${route}&bypassBearer=true`;

      const json = (
        await axiosFileInstance.get<{
          [key: string]: string | number | string[];
        }>(url)
      ).data;

      return new ApiServiceResult<{
        [key: string]: string | number | string[];
      }>({
        data: json,
      });
    });
  };

  /**
   * save json in jsonStore service
   * @param route -
   * @returns object
   */

  deleteJson = (
    route: string
  ): Promise<
    ApiServiceResult<{ result: string; error: boolean; errorMessage: string }>
  > => {
    return handleReq(async () => {
      const url = `v1.0/api/JsonStore?route=${route}&bypassBearer=true`;

      const result = (
        await axiosFileInstance.delete<{
          result: string;
          error: boolean;
          errorMessage: string;
        }>(url)
      ).data;

      return new ApiServiceResult<{
        result: string;
        error: boolean;
        errorMessage: string;
      }>({
        data: result,
      });
    });
  };
  //#endregion JSON STORAGE

  //#region sharepoint
  /**
   * Move files from blob storage to sharepoint
   * @param fileIds - file ids
   * @param blobContainer - container
   * @param recursive -
   * @returns object
   */
  moveBlobStorageToSharePoint = (
    ids: string[],
    blobContainer: string,
    recursive = process.env.REACT_APP_BLOB_STORAGE_SERVICE_RECURSIVE === 'true',
    deleteAfter = process.env.REACT_APP_BLOB_STORAGE_SERVICE_DELETE_AFTER ===
      'true'
  ): Promise<ApiServiceResult<SharePointResponse[]>> => {
    return handleReq(async () => {
      const data = {
        BlobContainer: blobContainer,
        DestDrive: 'Personal Property',
        Recursive: recursive,
        DirectoriesToCopy: ids,
        DeleteAfter: deleteAfter,
      };

      const result = (
        await axiosSharePointInstance.post<SharePointResponse[]>(
          '/StorageToSharepoint',
          data
        )
      ).data;

      return new ApiServiceResult<SharePointResponse[]>({
        data: result,
      });
    });
  };
  //#endregion sharepoint
}

export const apiService = new ApiService();
