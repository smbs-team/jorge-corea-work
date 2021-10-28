// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
  PortalContactEntity,
  PortalEmailEntity,
  PortalAddressEntity,
  PortalPhoneEntity,
} from 'models/portalContact';
import { CityEntity } from 'models/city';
import { StateEntity } from 'models/state';
import { ZipCodeEntity } from 'models/zipCode';
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { BasicAddressData } from 'services/map/model/addresses';
import { axiosMpInstance } from '../../axiosInstances';

class ContactApiService {
  //TODO: delete. TEMP method for development
  getTestContact = (): Promise<ApiServiceResult<PortalContact | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_portalemail',
          query: `$filter=_ptas_portalcontact_value eq 'e7a6049b-daf7-4e43-beec-fe6500c66d5a' and ptas_primaryemail eq true
          &$expand=ptas_portalcontact`,
          // query: `$filter=_ptas_portalcontact_value eq '310d774b-3b00-48f3-beee-7fc2a9b9ce39'
          // &$expand=ptas_portalcontact`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosMpInstance.post<{
          items: {
            changes: PortalEmailEntity & {
              ptas_portalcontact: PortalContactEntity;
            };
          }[];
        }>(url, data)
      ).data;
      if (res.items?.length) {
        const contact = new PortalContact(
          res.items[0].changes.ptas_portalcontact
        );
        contact.email = {
          id: res.items[0].changes.ptas_portalemailid,
          isSaved: true,
          email: res.items[0].changes.ptas_email,
          primaryEmail: res.items[0].changes.ptas_primaryemail,
          portalContactId: res.items[0].changes._ptas_portalcontact_value,
        };
        const addresses = await this.getContactAddresses(contact.id);
        if (addresses.data?.length) {
          const defaultAddress = addresses.data[0];
          contact.address = {
            ...defaultAddress,
            isSaved: true,
          };
        }
        console.log('address:', addresses);
        const phones = await this.getContactPhones(contact.id);
        console.log('phone:', phones);
        if (phones.data?.length) {
          const defaultPhone = phones.data[0];
          contact.phone = {
            ...defaultPhone,
            isSaved: true,
          };
        }

        return new ApiServiceResult<PortalContact | undefined>({
          data: contact,
        });
      } else {
        return new ApiServiceResult<PortalContact | undefined>({
          data: undefined,
        });
      }
    });
  };

  /**
   * Gets portal contact
   * @param email - Email associated to a portal contact
   */
  getPortalContactByEmail = (
    email: string
  ): Promise<ApiServiceResult<PortalContact | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_portalemail',
          query: `$filter=ptas_email eq '${email}'&$expand=ptas_portalcontact`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosMpInstance.post<{
          items: {
            changes: PortalEmailEntity & {
              ptas_portalcontact: PortalContactEntity;
            };
          }[];
        }>(url, data)
      ).data;
      if (res.items?.length) {
        const contact = new PortalContact(
          res.items[0].changes.ptas_portalcontact
        );
        contact.email = {
          id: res.items[0].changes.ptas_portalemailid,
          isSaved: true,
          email: res.items[0].changes.ptas_email,
          primaryEmail: res.items[0].changes.ptas_primaryemail,
          portalContactId: res.items[0].changes._ptas_portalcontact_value,
        };

        const addresses = await this.getContactAddresses(contact.id);
        if (addresses.data?.length) {
          const defaultAddress = addresses.data[0];
          contact.address = {
            ...defaultAddress,
            isSaved: true,
          };
        }

        const phones = await this.getContactPhones(contact.id);
        if (phones.data?.length) {
          const defaultPhone = phones.data[0];
          contact.phone = {
            ...defaultPhone,
            isSaved: true,
          };
        }

        return new ApiServiceResult<PortalContact | undefined>({
          data: contact,
        });
      } else {
        return new ApiServiceResult<PortalContact | undefined>({
          data: undefined,
        });
      }
    });
  };

  getPortalEmail = async (
    emailAddress: string
  ): Promise<ApiServiceResult<PortalEmail | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_portalemail',
          query: `$select=_ptas_portalcontact_value,ptas_portalemailid,ptas_email,ptas_primaryemail
            &$filter=ptas_email eq '${emailAddress}'
            &$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: PortalEmailEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<PortalEmail>({
        data: result.items?.length
          ? {
              id: result.items[0].changes.ptas_portalemailid,
              isSaved: true,
              email: result.items[0].changes.ptas_email,
              primaryEmail: result.items[0].changes.ptas_primaryemail,
              portalContactId:
                result.items[0].changes._ptas_portalcontact_value,
            }
          : undefined,
      });
    });
  };

  emailExists = async (
    emailId: string,
    emailAddress: string,
    isSaved: boolean
  ): Promise<boolean> => {
    if (!isSaved) {
      //If creating new email
      const { data: existingEmail } = await this.getPortalEmail(emailAddress);
      if (existingEmail) {
        return true;
      }
    } else {
      //If updating email
      const { data: existingEmail } = await this.getPortalEmail(emailAddress);
      if (existingEmail && existingEmail.id !== emailId) {
        return true;
      }
    }

    return false;
  };

  getContactAddresses = async (
    contactId: string
  ): Promise<ApiServiceResult<PortalAddress[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_portaladdress',
          query: `$select=ptas_portaladdressid,ptas_addresstitle,ptas_streetname,_ptas_countryid_value,_ptas_stateid_value,_ptas_cityid_value,_ptas_zipcodeid_value,_ptas_portalcontactid_value
          &$filter=_ptas_portalcontactid_value eq '${contactId}'
          &$orderby=createdon asc
          &$expand=ptas_cityid,ptas_stateid,ptas_zipcodeid`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: PortalAddressEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<PortalAddress[]>({
        data: result.items?.length
          ? result.items.map((item) => ({
              id: item.changes.ptas_portaladdressid,
              isSaved: true,
              title: item.changes.ptas_addresstitle,
              line1: item.changes.ptas_streetname,
              line2: '',
              countryId: item.changes._ptas_countryid_value,
              country: '',
              stateId: item.changes._ptas_stateid_value,
              state: item.changes.ptas_stateid?.ptas_abbreviation,
              cityId: item.changes._ptas_cityid_value,
              city: item.changes.ptas_cityid?.ptas_name,
              zipCodeId: item.changes._ptas_zipcodeid_value,
              zipCode: item.changes.ptas_zipcodeid?.ptas_name,
              portalContactId: item.changes._ptas_portalcontactid_value,
            }))
          : [],
      });
    });
  };

  getStateCityZip = async (
    countryId: string,
    state: string,
    city?: string,
    zip?: string
  ): Promise<ApiServiceResult<BasicAddressData>> => {
    const data: {
      requests: { entityName: string; query: string }[];
    } = {
      requests: [],
    };
    if (state) {
      const req = {
        entityName: 'ptas_stateorprovince',
        query: `$select=ptas_stateorprovinceid,ptas_name,ptas_abbreviation
                &$top=1
                &$filter=_ptas_countryid_value eq '${countryId}' and contains(ptas_name, '${state}')`,
      };
      data.requests.push(req);
    }
    if (city) {
      const req = {
        entityName: 'ptas_city',
        query: `$select=ptas_cityid,ptas_name
                &$top=1
                &$filter=contains(ptas_name, '${city}')`,
      };
      data.requests.push(req);
    }
    if (zip) {
      const req = {
        entityName: 'ptas_zipcode',
        query: `$select=ptas_zipcodeid,ptas_name
                &$top=1
                &$filter=startswith(ptas_name, '${zip}')`,
      };
      data.requests.push(req);
    }

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosMpInstance.post<{
          items: {
            entityName: string;
            changes: StateEntity | CityEntity | ZipCodeEntity;
          }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      const addressData: BasicAddressData = {
        country: '',
        countryId: countryId,
        state: '',
        stateId: '',
        city: '',
        cityId: '',
        zipCode: '',
        zipCodeId: '',
      };
      result.items.forEach((item) => {
        if (item.entityName === 'ptas_stateorprovince') {
          addressData.state = (item.changes as StateEntity).ptas_abbreviation;
          addressData.stateId = (item.changes as StateEntity).ptas_stateorprovinceid;
        } else if (item.entityName === 'ptas_city') {
          addressData.city = (item.changes as CityEntity).ptas_name;
          addressData.cityId = (item.changes as CityEntity).ptas_cityid;
        } else if (item.entityName === 'ptas_zipcode') {
          addressData.zipCode = (item.changes as ZipCodeEntity).ptas_name;
          addressData.zipCodeId = (item.changes as ZipCodeEntity).ptas_zipcodeid;
        }
      });

      return new ApiServiceResult<BasicAddressData>({
        data: addressData,
      });
    });
  };

  getContactPhones = async (
    contactId: string
  ): Promise<ApiServiceResult<PortalPhone[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_phonenumber',
          query: `$select=ptas_phonenumberid,ptas_phonenumber,ptas_phonetype,ptas_acceptstextmessages,_ptas_portalcontact_value
                  &$filter=_ptas_portalcontact_value eq '${contactId}'
                  &$orderby=createdon asc`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: PortalPhoneEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<PortalPhone[]>({
        data: result.items?.length
          ? result.items.map((item) => ({
              id: item.changes.ptas_phonenumberid,
              isSaved: true,
              phoneNumber: item.changes.ptas_phonenumber,
              phoneType: '',
              phoneTypeValue: item.changes.ptas_phonetype,
              acceptsTextMessages: item.changes.ptas_acceptstextmessages,
              portalContactId: item.changes._ptas_portalcontactid_value,
            }))
          : [],
      });
    });
  };

  getContactEmails = async (
    contactId: string
  ): Promise<ApiServiceResult<PortalEmail[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_portalemail',
          query: `$select=_ptas_portalcontact_value,ptas_portalemailid,ptas_email,ptas_primaryemail
          &$filter=_ptas_portalcontact_value eq '${contactId}'
          &$orderby=createdon asc`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: PortalEmailEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<PortalEmail[]>({
        data: result.items?.length
          ? result.items.map((item) => ({
              id: item.changes.ptas_portalemailid,
              isSaved: true,
              email: item.changes.ptas_email,
              primaryEmail: item.changes.ptas_primaryemail,
              portalContactId: item.changes._ptas_portalcontact_value,
            }))
          : [],
      });
    });
  };

  /**
   * Creates or updates a portal contact entity
   * @param newContact - contact model
   */
  savePortalContact = (
    newContact: PortalContact,
    oldContact?: PortalContact
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getPortalContactChanges(newContact, oldContact);
    //If "changes" is empty, don't send update request
    if (Object.keys(changes).length === 0) {
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
          entityName: 'ptas_portalcontact',
          entityId: newContact.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      if (result && !newContact.isSaved && newContact.email) {
        this.savePortalEmail(newContact.email);
      }
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * Creates or updates a portal email entity
   * @param newEmail - contact model
   */
  savePortalEmail = (
    newEmail: PortalEmail,
    oldEmail?: PortalEmail
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getPortalEmailChanges(newEmail, oldEmail);
    //If "changes" is empty, don't send update request
    if (Object.keys(changes).length === 0) {
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
          entityName: 'ptas_portalemail',
          entityId: newEmail.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
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

  updatePrimaryEmail = (
    newPrimaryEmailId: string,
    currentPrimaryEmailId: string
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_portalemail',
          entityId: newPrimaryEmailId,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_primaryemail: true,
          },
        },
        {
          entityName: 'ptas_portalemail',
          entityId: currentPrimaryEmailId,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_primaryemail: false,
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
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
   * Creates or updates a portal address entity
   * @param newAddress - contact model
   */
  savePortalAddress = (
    newAddress: PortalAddress,
    oldAddress?: PortalAddress
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getPortalAddressChanges(newAddress, oldAddress);
    //If "changes" is empty, don't send update request
    if (Object.keys(changes).length === 0) {
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
          entityName: 'ptas_portaladdress',
          entityId: newAddress.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
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
   * Creates or updates a portal phone entity
   * @param newPhone - contact model
   */
  savePortalPhone = (
    newPhone: PortalPhone,
    oldPhone?: PortalPhone
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getPortalPhoneChanges(newPhone, oldPhone);
    //If "changes" is empty, don't send update request
    if (Object.keys(changes).length === 0) {
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
          entityName: 'ptas_phonenumber',
          entityId: newPhone.id,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
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

  getPortalContactChanges = (
    newContact: PortalContact,
    oldContact?: PortalContact
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};
    if (oldContact) {
      if (oldContact.firstName !== newContact.firstName) {
        changes['ptas_firstname'] = newContact.firstName;
      }
      if (oldContact.middleName !== newContact.middleName) {
        changes['ptas_middlename'] = newContact.middleName ?? null;
      }
      if (oldContact.lastName !== newContact.lastName) {
        changes['ptas_lastname'] = newContact.lastName;
      }
      if (oldContact.suffix !== newContact.suffix) {
        changes['ptas_suffix'] = newContact.suffix ?? null;
      }
    } else {
      changes['ptas_firstname'] = newContact.firstName;
      changes['ptas_middlename'] = newContact.middleName ?? null;
      changes['ptas_lastname'] = newContact.lastName;
      changes['ptas_suffix'] = newContact.suffix ?? null;
    }
    return changes;
  };

  getPortalEmailChanges = (
    newEmail: PortalEmail,
    oldEmail?: PortalEmail
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};
    if (oldEmail) {
      if (
        !newEmail.isSaved ||
        oldEmail.portalContactId !== newEmail.portalContactId
      ) {
        changes['_ptas_portalcontact_value'] = newEmail.portalContactId;
      }
      if (oldEmail.email !== newEmail.email) {
        changes['ptas_email'] = newEmail.email;
      }
      if (oldEmail.primaryEmail !== newEmail.primaryEmail) {
        changes['ptas_primaryemail'] = newEmail.primaryEmail;
      }
    } else {
      changes['ptas_email'] = newEmail.email;
      changes['ptas_primaryemail'] = newEmail.primaryEmail;
      changes['_ptas_portalcontact_value'] = newEmail.portalContactId;
    }
    return changes;
  };

  getPortalAddressChanges = (
    newAddress: PortalAddress,
    oldAddress?: PortalAddress
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};
    if (oldAddress && newAddress.isSaved) {
      if (oldAddress.title !== newAddress.title) {
        changes['ptas_addresstitle'] = newAddress.title;
      }
      if (oldAddress.countryId !== newAddress.countryId) {
        changes['_ptas_countryid_value'] = newAddress.countryId;
      }
      if (oldAddress.stateId !== newAddress.stateId) {
        changes['_ptas_stateid_value'] = newAddress.stateId;
      }
      if (oldAddress.cityId !== newAddress.cityId) {
        changes['_ptas_cityid_value'] = newAddress.cityId;
      }
      if (oldAddress.zipCodeId !== newAddress.zipCodeId) {
        changes['_ptas_zipcodeid_value'] = newAddress.zipCodeId;
      }
      if (oldAddress.line1 !== newAddress.line1) {
        changes['ptas_streetname'] = newAddress.line1;
      }
    } else {
      changes['ptas_addresstitle'] = newAddress.title;
      changes['_ptas_countryid_value'] = newAddress.countryId;
      changes['_ptas_stateid_value'] = newAddress.stateId;
      changes['_ptas_cityid_value'] = newAddress.cityId;
      changes['_ptas_zipcodeid_value'] = newAddress.zipCodeId;
      changes['ptas_streetname'] = newAddress.line1;
      changes['_ptas_portalcontactid_value'] = newAddress.portalContactId;
    }
    return changes;
  };

  getPortalPhoneChanges = (
    newPhone: PortalPhone,
    oldPhone?: PortalPhone
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};
    if (oldPhone && newPhone.isSaved) {
      if (oldPhone.phoneNumber !== newPhone.phoneNumber) {
        changes['ptas_phonenumber'] = newPhone.phoneNumber;
      }
      if (oldPhone.phoneTypeValue !== newPhone.phoneTypeValue) {
        changes['ptas_phonetype'] = newPhone.phoneTypeValue;
      }
      if (oldPhone.acceptsTextMessages !== newPhone.acceptsTextMessages) {
        changes['ptas_acceptstextmessages'] = newPhone.acceptsTextMessages;
      }
    } else {
      changes['ptas_phonenumber'] = newPhone.phoneNumber;
      changes['ptas_phonetype'] = newPhone.phoneTypeValue;
      changes['ptas_acceptstextmessages'] = newPhone.acceptsTextMessages;
      changes['_ptas_portalcontact_value'] = newPhone.portalContactId;
    }
    return changes;
  };
}

export const contactApiService = new ContactApiService();
