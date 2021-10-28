// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosPerPropInstance } from '../../axiosInstances';
import {
  FileAttachmentMetadata,
  FileAttachmentMetadataEntityFields,
} from 'models/fileAttachmentMetadata';
import { QuickCollect } from 'models/quickCollect';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';

class ChangedBusinessApiService {
  /**
   * get file attachment metadata for business
   * @param params - request, entityName field is not required
   */
  getFileAttachmentData = (
    fileAttachmentMetadataId: string
  ): Promise<ApiServiceResult<FileAttachmentMetadata | undefined>> => {
    const data = {
      requests: [
        {
          entityId: fileAttachmentMetadataId,
          entityName: 'ptas_fileattachmentmetadata',
        },
      ],
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
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      const dataToReturn = result?.items[0].changes
        ? new FileAttachmentMetadata(result?.items[0].changes)
        : undefined;
      return new ApiServiceResult<FileAttachmentMetadata | undefined>({
        data: dataToReturn,
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
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_fileattachmentmetadataid: fileAttachment.id,
            ptas_name: fileAttachment.name,
            _ptas_parcelid_value: fileAttachment.parcelId,
            ptas_bloburl: fileAttachment.blobUrl,
            ptas_portaldocument: fileAttachment.document,
            ptas_portalsection: fileAttachment.section,
            ptas_icsdocumentid: fileAttachment.icsDocumentId,
            ptas_isblob: fileAttachment.isBlob,
            ptas_issharepoint: fileAttachment.isSharePoint,
            ptas_sharepointurl: fileAttachment.sharepointUrl,
            /* eslint-enable @typescript-eslint/camelcase */
          },
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
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   */
  updateFileAttachmentData = (
    fileAttachment: FileAttachmentMetadata
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_bloburl: fileAttachment.blobUrl,
            ptas_isblob: fileAttachment.isBlob,
            ptas_issharepoint: fileAttachment.isSharePoint,
            ptas_sharepointurl: fileAttachment.sharepointUrl,
            /* eslint-enable @typescript-eslint/camelcase */
          },
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
   * Creates a quick collect entity
   * @param quickCollect - quick collect data model
   */
  saveQuickCollect = (
    quickCollect: QuickCollect
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_quickcollect',
          entityId: quickCollect.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_quickcollectid: quickCollect.id,
            _ptas_personalpropertyid_value: quickCollect.personalPropertyId,
            ptas_date: quickCollect.date,
            ptas_personalpropinfo_addr_street1: quickCollect.businessAddress1,
            ptas_personalpropinfo_addr_street2: quickCollect.businessAddress2,
            ptas_personalpropinfo_addr_city: quickCollect.businessCityLabel,
            _ptas_personalpropinfo_addr_stateid_value:
              quickCollect.businessStateId,
            ptas_personalpropinfo_addr_zip: quickCollect.businessZipLabel,
            ptas_reasonforrequest: quickCollect.reasonForRequestId,
            ptas_methodoftransfer: quickCollect.methodOfTransferId,
            _ptas_billofsale_fileattachementmetadataid_value:
              quickCollect.billFileAttachmentMetadataId,
            ptas_newowneremail: quickCollect.newOwnerEmail,
            ptas_newownername: quickCollect.newOwnerName,
            ptas_newbusinessname: quickCollect.newBusinessName,
            ptas_ubinumber: quickCollect.ubiNumber,
            _ptas_businessnaicscodeid_value: quickCollect.naicsCodeId,
            ptas_totalsalesprice: quickCollect.totalSalesPrice,
            ptas_equipment: quickCollect.equipment,
            ptas_leaseholdimprovements: quickCollect.leaseHoldImprovements,
            ptas_intangibles: quickCollect.intangibles,
            ptas_other: quickCollect.other,
            ptas_closingdate: quickCollect.closingDate,
            ptas_dispositionofassets: quickCollect.dispositionOfAssets,
            ptas_requestorinfo_addr_street1: quickCollect.requestorAddress1,
            ptas_requestorinfo_addr_street2: quickCollect.requestorAddress2,
            ptas_requestorinfo_addr_city: quickCollect.requestorCityLabel,
            _ptas_requestorinfo_addr_stateid_value:
              quickCollect.requestorStateId,
            ptas_requestorinfo_addr_zip: quickCollect.requestorZipLabel,
            statecode: 0,
            statuscode: 1,
            /* eslint-enable @typescript-eslint/camelcase */
          },
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
   * Creates a personal propertyhistory entity
   * @param personalPropertyHistory - personal propertyhistory data model
   */
  savePersonalPropertyHistory = (
    personalPropertyHistory: PersonalPropertyHistory
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_personalpropertyhistory',
          entityId: personalPropertyHistory.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_personalpropertyhistoryid: personalPropertyHistory.id,
            _ptas_personalpropertyid_value:
              personalPropertyHistory.personalPropertyId,
            ptas_preparername: personalPropertyHistory.preparerName,
            ptas_preparerattention: personalPropertyHistory.preparerAttention,
            ptas_prepareremail1: personalPropertyHistory.preparerEmail1,
            ptas_preparercellphone1: personalPropertyHistory.preparerCellPhone1,
            _ptas_preparercityid_value: personalPropertyHistory.preparerCityId,
            _ptas_preparerstateid_value:
              personalPropertyHistory.preparerStateId,
            _ptas_preparerzipid_value: personalPropertyHistory.preparerZipId,
            _ptas_preparercountryid_value:
              personalPropertyHistory.preparerCountryId,
            ptas_property_type: personalPropertyHistory.propertyType,
            ptas_businessstateincorporated_value:
              personalPropertyHistory.businessStateIncorporateId,
            statecode: 0,
            statuscode: 1,
            /* eslint-enable @typescript-eslint/camelcase */
          },
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
}

export const changedBusinessApiService = new ChangedBusinessApiService();
