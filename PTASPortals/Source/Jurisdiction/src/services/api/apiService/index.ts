// apiService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosFileInstance, axiosInstance } from '../axiosInstances';
import {
  Contact,
  JurisdictionContactEntity,
  PermitsFileInfo,
  TaxDistrictContactEntity,
  UptPermitsFileInfo,
} from 'routes/models/Views/Profile/Contact';
import { v4 as uuid } from 'uuid';
import { AddressSuggestion } from 'services/models/addressSuggestion';

class ApiService {
  /**
   *  Obtain jurisdiction or tax district contact
   */
  getContact = async (
    email: string
  ): Promise<ApiServiceResult<Contact | undefined>> => {
    const jurisdictionContactRes = await this.getJurisdictionContact(email);

    if (!jurisdictionContactRes.data?.id) {
      const taxDistrictContactRes = await this.getTaxDistrictContact(email);
      if (!taxDistrictContactRes.data?.id) {
        console.error('Contact not found');
      }
      return taxDistrictContactRes;
    }
    return jurisdictionContactRes;
  };

  /**
   * Gets jurisdiction contact
   */
  getJurisdictionContact = (
    email: string
  ): Promise<ApiServiceResult<Contact | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_jurisdictioncontact',
          query: `$top=1&$expand=ptas_jurisdictionid&$filter=ptas_email eq '${email}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const contact = (
        await axiosInstance.post<{
          items: { changes: JurisdictionContactEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<Contact>({
        data: new Contact(
          contact.items.length
            ? { ...contact.items[0].changes, type: 'jurisdiction' }
            : undefined
        ),
      });
    });
  };

  /**
   * Gets tax district contact
   */
  getTaxDistrictContact = (
    email: string
  ): Promise<ApiServiceResult<Contact | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_taxdistrictcontacts',
          query: `$top=10&$expand=ptas_taxdistrictid&$filter=ptas_emailaddress eq '${email}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const contact = (
        await axiosInstance.post<{
          items: { changes: TaxDistrictContactEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<Contact>({
        data: new Contact(
          contact.items.length
            ? { ...contact.items[0].changes, type: 'taxDistrict' }
            : undefined
        ),
      });
    });
  };

  /**
   * Upload permits file.
   * Only xlsx files are expected
   */

  uploadExcelFilePermits = (
    file: File,
    jurisdictionId: string
  ): Promise<ApiServiceResult<PermitsFileInfo>> => {
    const bodyFD = new FormData();
    bodyFD.append('File', file);

    return handleReq(async () => {
      const url = `/PermitFile/SaveFile/${jurisdictionId}`;

      const responseFile = (
        await axiosInstance.post<PermitsFileInfo>(url, bodyFD, {
          headers: {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
          },
        })
      ).data;

      return new ApiServiceResult<PermitsFileInfo>({
        data: responseFile,
      });
    });
  };

  /**
   * Takes info in excel file and saves it to dynamics.
   */

  updatePermitFileInfo = (
    fileUrl: string,
    jurisdictionId: string
  ): Promise<ApiServiceResult<UptPermitsFileInfo>> => {
    const bodyFD = new FormData();
    bodyFD.append('FileUrl', fileUrl);
    bodyFD.append('JurisdictionId', jurisdictionId);

    return handleReq(async () => {
      const url = `/PermitFile/UpdatePermitInfo`;

      const responseFile = (
        await axiosInstance.put<UptPermitsFileInfo>(url, bodyFD, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })
      ).data;

      return new ApiServiceResult<UptPermitsFileInfo>({
        data: responseFile,
      });
    });
  };

  /**
   * Create annotation-permits
   */

  createAnnotation = (
    file: File,
    jurisdictionId: string,
    note: string,
    objectTypeId: 'ptas_jurisdiction' | 'ptas_taxdistrict'
  ): Promise<
    ApiServiceResult<{
      result?: string;
    }>
  > => {
    const bodyFD = new FormData();
    bodyFD.append('File', file);
    bodyFD.append('NoteText', note);
    bodyFD.append('ObjectId', jurisdictionId);
    bodyFD.append('ObjectIdType', objectTypeId);

    return handleReq(async () => {
      const url = 'Annotations';

      const annotation = (
        await axiosInstance.post<{
          result?: string;
        }>(url, bodyFD, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })
      ).data;

      return new ApiServiceResult<{
        result?: string;
      }>({
        data: annotation,
      });
    });
  };

  /**
   * Service for uploading files to FileStorage
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
            containerName: process.env.REACT_APP_CONTAINER_DEFAULT,
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
   * Get files from file storage by contact id
   */

  getFiles = (id: string): Promise<ApiServiceResult<string[]>> => {
    return handleReq(async () => {
      const url = 'v1.0/api/FileStore';

      const urlFile = (
        await axiosFileInstance.get<string[]>(url, {
          params: {
            containerName: process.env.REACT_APP_CONTAINER_DEFAULT,
            id,
            includeSAS: 'false',
          },
        })
      ).data;

      return new ApiServiceResult<string[]>({
        data: urlFile,
      });
    });
  };

  /**
   * Delete file from FileStorage
   */

  deleteFile = (
    fileName: string,
    id: string
  ): Promise<ApiServiceResult<{ message: string }>> => {
    return handleReq(async () => {
      const url = 'v1.0/api/FileStore';
      const data = {
        containerName: process.env.REACT_APP_CONTAINER_DEFAULT,
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
   * Updates a jurisdiction contact
   */
  saveJurisdictionContact = (
    newContact: Contact,
    oldContact?: Contact
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getContactChanges(newContact, oldContact);
    const data = {
      items: [
        {
          entityName: 'ptas_jurisdictioncontact',
          entityId: newContact.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosInstance.post<{
          items: { changes: JurisdictionContactEntity }[];
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
   * Updates a tax district contact
   */
  saveTaxDistrictContact = (
    newContact: Contact,
    oldContact?: Contact
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getContactChanges(newContact, oldContact);
    const data = {
      items: [
        {
          entityName: 'ptas_taxdistrictcontacts',
          entityId: newContact.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosInstance.post<{
          items: { changes: JurisdictionContactEntity }[];
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
   * Obtains the parcel suggestions through an address
   */
  getAddressSuggestions = (
    lookupValue: string
  ): Promise<ApiServiceResult<AddressSuggestion[]>> => {
    const data = {
      searchvalue: lookupValue,
    };

    return handleReq(async () => {
      const url = '/addresslookup';
      const result = (
        await axiosInstance.post<AddressSuggestion[]>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<AddressSuggestion[]>({
        data: result?.length
          ? result.map((item) => ({ ...item, id: uuid() }))
          : [],
      });
    });
  };

  /**
   * Compares and returns an object with the changes made between an updated object and an initial object
   */
  getContactChanges = (newContact: Contact, oldContact?: Contact): object => {
    const changes: { [k: string]: string } = {};
    if (oldContact) {
      if (oldContact.firstName !== newContact.firstName) {
        changes['ptas_firstname'] = newContact.firstName;
      }
      if (oldContact.lastName !== newContact.lastName) {
        changes['ptas_lastname'] = newContact.lastName;
      }
      if (oldContact.email !== newContact.email) {
        if (newContact.type === 'jurisdiction') {
          changes['ptas_email'] = newContact.email;
        } else if (newContact.type === 'taxDistrict') {
          changes['ptas_emailaddress'] = newContact.email;
        }
      }
      if (oldContact.phoneNumber !== newContact.phoneNumber) {
        changes['ptas_phonenumber'] = newContact.phoneNumber;
      }
      if (oldContact.jobTitle !== newContact.jobTitle) {
        changes['ptas_jobtitle'] = newContact.jobTitle;
      }
      if (
        oldContact.address?.line1 !== newContact.address?.line1 &&
        newContact.address
      ) {
        changes['ptas_address1'] = newContact.address?.line1;
      }
      if (
        oldContact.address?.line2 !== newContact.address?.line2 &&
        newContact.address
      ) {
        changes['ptas_address2'] = newContact.address?.line2;
      }
      if (
        oldContact.address?.city !== newContact.address?.city &&
        newContact.address
      ) {
        changes['ptas_city'] = newContact.address?.city;
      }
      if (
        oldContact.address?.state !== newContact.address?.state &&
        newContact.address
      ) {
        changes['ptas_state'] = newContact.address?.state;
      }
      if (
        oldContact.address?.zip !== newContact.address?.zip &&
        newContact.address
      ) {
        changes['ptas_zipcode'] = newContact.address?.zip;
      }
    } else {
      changes['ptas_firstname'] = newContact.firstName;
      changes['ptas_lastname'] = newContact.lastName;
      changes['ptas_email'] = newContact.email;
      changes['ptas_phonenumber'] = newContact.phoneNumber;
      changes['ptas_jobtitle'] = newContact.jobTitle;
      changes['ptas_address1'] = newContact.address?.line1 ?? '';
      changes['ptas_address2'] = newContact.address?.line2 ?? '';
      changes['ptas_city'] = newContact.address?.city ?? '';
      changes['ptas_state'] = newContact.address?.state ?? '';
      changes['ptas_zipcode'] = newContact.address?.zip ?? '';
    }

    return changes;
  };
}

export const apiService = new ApiService();
