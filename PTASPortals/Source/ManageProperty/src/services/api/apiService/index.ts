// apiService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import {
  axiosCsInstance,
  axiosHiInstance,
  axiosInstance,
  axiosMpInstance,
  axiosFileInstance,
  axiosSharePointInstance,
} from '../axiosInstances';
import { ParcelMediaInfo, Property } from 'services/map/model/parcel';
import { RequestPropertyDesc } from 'services/map/types';
import {
  Permit,
  PermitEntity,
} from 'routes/models/Views/HomeImprovement/model/permit';
import {
  Jurisdiction,
  JurisdictionEntity,
} from 'routes/models/Views/HomeImprovement/model/jurisdiction';
import {
  FileAttachmentMetadata,
  FileAttachmentMetadataEntity,
} from 'routes/models/Views/HomeImprovement/model/fileAttachmentMetadata';
import { OptionSetValue } from 'routes/models/Views/HomeImprovement/model/optionSet';
import { TaxAccount } from 'services/map/model/taxAccount';
import { CountryEntity } from 'models/country';
import { StateEntity } from 'models/state';
import { CityEntity } from 'models/city';
import { ZipCodeEntity } from 'models/zipCode';
import {
  FileAttachmentMetadataClass,
  FileAttachmentMetadataEntityFields,
} from 'models/fileAttachmentMetadata';
import { YearEntity } from 'models/year';

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
   * Get a parcel from the id, the account or the address.
   * @param id - Partial parcel id, or account or address.
   */

  getParcel = (id: string): Promise<ApiServiceResult<Property[] | undefined>> =>
    handleReq(async () => {
      const idWithoutSpace = id.trim();

      const url = `/parcellookup/${idWithoutSpace}`;

      const parcel = await axiosMpInstance.get<Property[]>(url);

      return new ApiServiceResult<Property[]>({
        data: parcel.data,
      });
    });

  /**
   * Get parcels
   * @param ids - IDs of parcels
   */
  getParcels = (
    ids: string[]
  ): Promise<ApiServiceResult<Property[] | undefined>> => {
    let requests: object[] = [];

    for (const id of ids) {
      if (!id) continue;

      requests = [
        ...requests,
        {
          entityName: 'ptas_parceldetail',
          query: `$filter=ptas_parceldetailid eq '${id}'&$select=ptas_parceldetailid,ptas_acctnbr,ptas_name,ptas_namesonaccount,ptas_address,ptas_streettype,ptas_district,ptas_zipcode,_ptas_taxaccountid_value`,
        },
      ];
    }

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: Property }[];
      }>(url, {
        requests,
      });

      return new ApiServiceResult<Property[]>({
        data: res.data.items.map((i) => i.changes),
      });
    });
  };

  /**
   * Gets a collection of media images associated with an specific parcel.
   * @param parcelPIN - the parcel ping number.
   * @returns A Collection of ParcelMediaInfo objects.
   */
  getMediaForParcel = (
    parcelPIN: string,
    mediaToken: string | undefined
  ): Promise<ApiServiceResult<string[]>> =>
    handleReq(async () => {
      // Clean parameter, if parcel # is missing the separator, include it.
      const parcelNumber = !parcelPIN.includes('-')
        ? `${parcelPIN.substring(0, 6)}-${parcelPIN.substring(6)}`
        : parcelPIN;

      const results = (
        await axiosCsInstance.get<ParcelMediaInfo[]>(
          `/GIS/GetParcelMediaInfo/${parcelNumber}`
        )
      ).data;

      return new ApiServiceResult({
        data: results.map(
          (item) =>
            `${process.env.REACT_APP_MEDIA_BLOB_URL}${item.MediaPath}${mediaToken}`
        ),
      });
    });

  /**
   *
   * @param request -
   * @returns
   */
  getParcelDescription = (
    request: RequestPropertyDesc
  ): Promise<ApiServiceResult<Property | undefined>> =>
    handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const parcel = await axiosMpInstance.post<{
        items: [
          {
            changes: Property;
          }
        ];
      }>(url, {
        requests: [request],
      });

      return new ApiServiceResult<Property>({
        data: parcel.data.items[0].changes,
      });
    });

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
  ): Promise<ApiServiceResult<OptionSetValue[] | undefined>> =>
    handleReq(async () => {
      const url = `/optionsets/${objectId}/${optionSetId}`;

      const optionSet = await axiosMpInstance.get<OptionSetValue[]>(url);

      return new ApiServiceResult<OptionSetValue[]>({
        data: optionSet.data,
      });
    });

  /**
   * Gets jurisdiction suggestions
   * @param lookupValue - Jurisdiction name to search
   */
  searchJurisdiction = (
    lookupValue: string
  ): Promise<ApiServiceResult<Jurisdiction[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_jurisdiction',
          query: `$top=4
          &$select=ptas_jurisdictionid,ptas_name
          &$orderby=ptas_name asc
          &$filter=contains(ptas_name, '${lookupValue}')`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosHiInstance.post<{
          items: { changes: JurisdictionEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<Jurisdiction[]>({
        data: result.items?.length
          ? result.items.map((item) => ({
              id: item.changes.ptas_jurisdictionid,
              name: item.changes.ptas_name,
            }))
          : [],
      });
    });
  };

  //TODO: move to HI file
  /**
   * Gets file attachments
   * @param hiApplicationId - ID of Home Improvement Application associated to file attachment
   */
  getFileAttachmentsByHiApplication = (
    hiApplicationId: string
  ): Promise<ApiServiceResult<FileAttachmentMetadata[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          query: `$select=ptas_fileattachmentmetadataid,ptas_name,ptas_bloburl,ptas_isblob,_ptas_parcelid_value,_ptas_attachmentid_value,ptas_issharepoint,ptas_sharepointurl
            &$filter=_ptas_attachmentid_value eq '${hiApplicationId}'`, //and ptas_isblob eq true`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: FileAttachmentMetadataEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<FileAttachmentMetadata[]>({
        data: res.items?.length
          ? res.items.map((item) => new FileAttachmentMetadata(item.changes))
          : undefined,
      });
    });
  };

  /**
   * Gets a tax account
   * @param taxAccountId - ID of tax account
   */
  getTaxAccount = (
    taxAccountId: string
  ): Promise<ApiServiceResult<TaxAccount | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_taxaccount',
          query: `$filter=ptas_taxaccountid eq '${taxAccountId}'
            &$select=ptas_taxaccountid,ptas_name,ptas_phone1,statecode,statuscode,ptas_email,ptas_taxpayername,ptas_addr1_compositeaddress`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: TaxAccount }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<TaxAccount>({
        data: res.items?.length ? res.items[0].changes : undefined,
      });
    });
  };

  /**
   * Gets permits for a property
   * @param parcelId - Parcel ID
   */
  getPermitsByParcel = (
    parcelId: string
  ): Promise<ApiServiceResult<Permit[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_permit',
          query: `$filter=_ptas_parcelid_value eq '${parcelId}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: PermitEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<Permit[]>({
        data: res.items?.length
          ? res.items.map((item) => new Permit(item.changes))
          : undefined,
      });
    });
  };

  /**
   * Creates a permit entity
   * @param permit - permit model
   */
  savePermit = (permit: Permit): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_permit',
          entityId: permit.permitId,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_permitid: permit.permitId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_name: permit.name,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_parcelid_value: permit.parcelId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_issueddate: permit.issuedDate,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_issuedbyid_value: permit.issuedById,
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
        await axiosHiInstance.post<{
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
        await axiosHiInstance.post<{
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
        await axiosHiInstance.post<{
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
        await axiosHiInstance.post<{
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
   * Get years
   */
  getYears = (): Promise<ApiServiceResult<YearEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_year',
          query: `$select=ptas_yearid,ptas_name`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: YearEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<YearEntity[]>({
        data: res.items?.length ? res.items.map((i) => i.changes) : [],
      });
    });
  };

  /**
   * generic get file attachment metadata
   * @param params - request, entityName field is not required
   */
  getFileAttachmentData = (
    params: GenDynamicsGetParams[]
  ): Promise<ApiServiceResult<FileAttachmentMetadataClass[]>> => {
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
        await axiosHiInstance.post<{
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
      return new ApiServiceResult<FileAttachmentMetadataClass[]>({
        data:
          result?.items.map(
            ({ changes }) =>
              new FileAttachmentMetadataClass(
                changes as FileAttachmentMetadataEntityFields
              )
          ) ?? ([] as FileAttachmentMetadataClass[]),
      });
    });
  };

  /**
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   */
  saveFileAttachmentData = (
    fileAttachment: FileAttachmentMetadataClass
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.fileAttachmentMetadataId,
          changes: fileAttachment.getEntityFields(),
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
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   */
  saveFileAttachment = (
    fileAttachment: FileAttachmentMetadata
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
            _ptas_attachmentid_value:
              fileAttachment.homeImprovementApplicationId,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_bloburl: fileAttachment.blobUrl,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_isblob: fileAttachment.isBlob,
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
    fileAttachment: FileAttachmentMetadata
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
            containerName: process.env.REACT_APP_BLOB_CONTAINER_HI,
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
        containerName: process.env.REACT_APP_BLOB_CONTAINER_HI,
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
        await axiosMpInstance.post<{
          items: { changes: T }[];
        }>(url, data)
      ).data;

      return new ApiServiceResult<T>({
        data: task.items[0].changes,
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
      const result = await axiosMpInstance.delete<{
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
   * save file in sharepoint
   * @param fileIds - file ids
   * @param blobContainer - container
   * @param recursive -
   * @returns object
   */
  saveFileInSharePoint = (
    ids: string[],
    blobContainer: string,
    recursive = false,
    deleteAfter = false
  ): Promise<ApiServiceResult<SharePointResponse[]>> => {
    return handleReq(async () => {
      const data = {
        BlobContainer: blobContainer,
        DestDrive: 'ASR Seniors',
        Recursive: recursive,
        DirectoriesToCopy: ids,
        DeleteAfter: deleteAfter,
      };

      const result = (
        await axiosSharePointInstance.post<SharePointResponse[]>('', data)
      ).data;

      return new ApiServiceResult<SharePointResponse[]>({
        data: result,
      });
    });
  };
  //#endregion sharepoint
}

export const apiService = new ApiService();
