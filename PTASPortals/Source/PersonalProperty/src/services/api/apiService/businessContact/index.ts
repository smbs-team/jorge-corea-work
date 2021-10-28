// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  PersonalPropertyAccountAccess,
  PersonalPropertyAccountAccessEntity,
} from 'models/personalPropertyAccountAccess';
import { PortalContact } from 'models/portalContact';
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosPerPropInstance } from '../../axiosInstances';

class BusinessContactApiService {
  getBusinessContactByContact = (
    portalContactId: string
  ): Promise<ApiServiceResult<PersonalPropertyAccountAccess[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalpropertyaccountaccess',
          query: `$filter=_ptas_portalcontactid_value eq '${portalContactId}'&$expand=ptas_personalpropertyaccountid`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyAccountAccessEntity;
          }[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length) {
        dataResp = res.items.map((p): PersonalPropertyAccountAccess => {
          return new PersonalPropertyAccountAccess(p.changes);
        });
      }

      return new ApiServiceResult<PersonalPropertyAccountAccess[] | undefined>({
        data: dataResp,
      });
    });
  };

  /**
   * get contacts grouped by business
   * @param personalPropertyIds - business id list
   */
  getContactByBusinesses = (
    personalPropertyIds: string[]
  ): Promise<ApiServiceResult<Map<string, PortalContact[]> | undefined>> => {
    const requests = personalPropertyIds.map((id) => ({
      entityName: 'ptas_personalpropertyaccountaccess',
      query: `$filter=_ptas_personalpropertyaccountid_value eq '${id}'&$expand=ptas_portalcontactid`,
    }));
    const data = { requests };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyAccountAccessEntity;
          }[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length) {
        dataResp = res.items.reduce((acc, current): Map<
          string,
          PortalContact[]
        > => {
          /* eslint-enable @typescript-eslint/camelcase */
          const currentBusinessId =
            current.changes._ptas_personalpropertyaccountid_value;
          if (!currentBusinessId || !current.changes.ptas_portalcontactid)
            return acc;
          const prevContacts = acc.get(currentBusinessId) ?? [];
          acc.set(currentBusinessId, [
            ...prevContacts,
            new PortalContact(current.changes.ptas_portalcontactid),
          ]);
          return acc;
          /* eslint-disable @typescript-eslint/camelcase */
        }, new Map());
      }

      return new ApiServiceResult<Map<string, PortalContact[]> | undefined>({
        data: dataResp,
      });
    });
  };

  getBusinessContactByBusiness = (
    personalPropertyId: string
  ): Promise<ApiServiceResult<PersonalPropertyAccountAccess[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalpropertyaccountaccess',
          query: `$filter=_ptas_personalpropertyaccountid_value eq '${personalPropertyId}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyAccountAccessEntity;
          }[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length) {
        dataResp = res.items.map((p): PersonalPropertyAccountAccess => {
          return new PersonalPropertyAccountAccess(p.changes);
        });
      }

      return new ApiServiceResult<PersonalPropertyAccountAccess[] | undefined>({
        data: dataResp,
      });
    });
  };

  /**
   * Creates or updates a personal property account access record
   * @param personalPropertyAccountAccess - personal property account access model
   * @param omitFields - Entity fields to omit changes
   */
  addBusinessContact = (
    personalPropertyAccountAccess: PersonalPropertyAccountAccess
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = {
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_personalpropertyaccountaccessid: personalPropertyAccountAccess.id,
      _ptas_personalpropertyaccountid_value:
        personalPropertyAccountAccess.personalPropertyId,
      _ptas_portalcontactid_value:
        personalPropertyAccountAccess.portalContactId,
      ptas_accesslevel: personalPropertyAccountAccess.accessLevel,
      statecode: personalPropertyAccountAccess.stateCode,
      statuscode: personalPropertyAccountAccess.statusCode,
      /* eslint-enable @typescript-eslint/camelcase */
    };

    const data = {
      items: [
        {
          entityName: 'ptas_personalpropertyaccountaccess',
          entityId: personalPropertyAccountAccess.id,
          changes,
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
   * Creates or updates a personal property account access record
   * @param personalPropertyAccountAccess - PersonalPropertyAccountAccess model
   */
  changeContactBusiness = (
    personalPropertyAccountAccess: PersonalPropertyAccountAccess
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_personalpropertyaccountaccess',
          entityId: personalPropertyAccountAccess.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            _ptas_portalcontactid_value:
              personalPropertyAccountAccess.portalContactId,
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
   * Deletes a personal property account access entity
   * @param personalPropertyAccountAccessId - personal property account access id
   */
  deleteBusinessContact = (
    personalPropertyAccountAccessIds: string[]
  ): Promise<ApiServiceResult<{ id: string; result: boolean }>> => {
    const items = personalPropertyAccountAccessIds.map((id) => ({
      entityName: 'ptas_personalpropertyaccountaccess',
      entityId: id,
    }));
    const data = { items };

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

  /**
   * get contacts with access to businesses (View or Edit), by
   * the specified personal property IDs
   * @param personalPropertyIds - business id list
   * @returns Map with contact ID as key, and value PortalContact including business list
   */
  getContactsWithBusinessAccess = (
    personalPropertyIds: string[]
  ): Promise<ApiServiceResult<Map<string, PortalContact> | undefined>> => {
    const requests = personalPropertyIds.map((id) => ({
      entityName: 'ptas_personalpropertyaccountaccess',
      query: `$filter=_ptas_personalpropertyaccountid_value eq '${id}'
              &$expand=ptas_portalcontactid,ptas_personalpropertyaccountid($select=ptas_personalpropertyid,ptas_businessname)`,
    }));
    const data = { requests };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyAccountAccessEntity;
          }[];
        }>(url, data)
      ).data;
      
      let dataResp = undefined;
      if (res.items && res.items.length) {
        dataResp = res.items.reduce((map, currentAccess): Map<
          string,
          PortalContact
        > => {
          const contactId = currentAccess.changes._ptas_portalcontactid_value;
          if (!contactId) return map;

          const business = {
            id:
              currentAccess.changes?.ptas_personalpropertyaccountid
                ?.ptas_personalpropertyid ?? '',
            name:
              currentAccess.changes?.ptas_personalpropertyaccountid
                ?.ptas_businessname ?? '',
          };
          const currentContact = map.get(contactId);
          if (currentContact) {
            currentContact.businessAccess.push(business);
          } else {
            const contact = new PortalContact(
              currentAccess.changes.ptas_portalcontactid
            );
            contact.businessAccess.push(business);
            map.set(contactId, contact);
          }
          return map;
          /* eslint-disable @typescript-eslint/camelcase */
        }, new Map<string, PortalContact>());
      }

      return new ApiServiceResult<Map<string, PortalContact> | undefined>({
        data: dataResp,
      });
    });
  };
}

export const businessContactApiService = new BusinessContactApiService();
