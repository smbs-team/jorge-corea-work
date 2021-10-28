//-----------------------------------------------------------------------
// <copyright file="dynamics-service.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import http from './httpService';
import { refreshToken } from '../contexts/MagicLinkContext';

const instance = http.create({
  baseURL: process.env.REACT_APP_API_URL,
  timeout: process.env.REACT_APP_API_TIMEOUT,
  headers: {
    'Content-Type': 'application/json',
  },
});

const setToken = () => {
  return {
    headers: {
      Authorization: `Bearer ${refreshToken()}`,
    },
  };
};

export const addressLookup = async lookupValue => {
  try {
    if (lookupValue) {
      const { data } = await instance.get(
        `/addresslookup/${encodeURI(lookupValue)}`,
        setToken()
      );
      return data;
    }
    return [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getMedicarePlans = async () => {
  try {
    const { data } = await instance.get('/medicareplans', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getCountries = async () => {
  try {
    const { data } = await instance.get('/countries', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getCounties = async () => {
  try {
    const { data } = await instance.get('/counties', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getYears = async () => {
  try {
    const { data } = await instance.get('/years', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getSeniorAppStatuses = async () => {
  try {
    const { data } = await instance.get('/statuscodes/seniorapp', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getSeniorAppDetailStatuses = async () => {
  try {
    const { data } = await instance.get(
      '/statuscodes/seniordetail',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getRelationships = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/relationships',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getAccountTypes = async () => {
  try {
    const { data } = await instance.get('/optionsets/accounttypes', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getExemptionSources = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/exemptionsources',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getExemptionTypes = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/exemptiontypes',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getIncomeLevels = async () => {
  try {
    const { data } = await instance.get('/optionsets/incomelevels', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getMediaTypes = async () => {
  try {
    const { data } = await instance.get('/optionsets/mediatypes', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getPurposes = async () => {
  try {
    const { data } = await instance.get('/optionsets/purposes', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getSplitCodes = async () => {
  try {
    const { data } = await instance.get('/optionsets/splitcodes', setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
export const getDisabilityIncomeSources = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/disabilityincomesources',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getFinancialFilerTypes = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/financialfilertypes',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getFinancialFormTypes = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/financialformtypes',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getParcelDetail = async parcelId => {
  try {
    const { data } = await instance.get(
      `/ParcelDetail/${parcelId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

// Contacts CRU
export const getContactByEmail = async email => {
  try {
    const { data } = await instance.get(`/contactlookup/${email}`, setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createContact = async user => {
  try {
    const { data } = await instance.post(`/contacts`, user, setToken());
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateContact = async user => {
  try {
    const { data } = await instance.patch(
      `/contacts/${user.contactid}`,
      user,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getSeniorApplications = async contactId => {
  try {
    const { data } = await instance.get(
      `/seniorexemptionapplications/${contactId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createSeniorApplication = async seniorApp => {
  try {
    const { data } = await instance.post(
      `/seapplications`,
      seniorApp,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateSeniorApplication = async seniorApp => {
  try {
    delete seniorApp.contactid;

    const { data } = await instance.patch(
      `/seapplications/${seniorApp.seapplicationid}`,
      seniorApp,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 * Deletes a senior exemption application by id
 *
 * @param {GUID} seniorAppId
 * @returns
 */
const deleteSeniorApplications = async seniorAppId => {
  try {
    const { data } = await instance.delete(
      `/seapplications/${seniorAppId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getFileMetadata = async fileMetadataId => {
  try {
    const { data } = await instance.get(
      `/FileAttachmentsMetadata/${fileMetadataId}`,
      setToken()
    );

    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getFileMetadataListBySeniorAppId = async seniorAppId => {
  try {
    let { data } = await instance.get(
      `/fileattachmentmetadatalookup/${seniorAppId}`,
      setToken()
    );
    data = data ? data : [];

    // Filter only by is is Blob in true, the is iLinx in true will be ignored as they were already submitted,
    //portal won't support them but still fetch them in case this changes in the future.
    data = data.filter(d => d.isBlob === true && d.isIlinx === false);
    return data;
  } catch (error) {
    console.log('Http request error', error);
    return [];
  }
};

export const createFileMetadata = async fileMetadata => {
  try {
    const { data } = await instance.post(
      `/FileAttachmentsMetadata`,
      fileMetadata,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateFileMetadata = async fileMetadata => {
  try {
    const { data } = await instance.patch(
      `/FileAttachmentsMetadata/${fileMetadata.id}`,
      fileMetadata,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const deleteFileMetadata = async fileMetadataId => {
  try {
    const { data } = await instance.delete(
      `/FileAttachmentsMetadata/${fileMetadataId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const parcelLookup = async input => {
  try {
    const { data } = await instance.get(`/parcellookup/${encodeURI(input)}`, {
      ...setToken(),
      timeout: 10000,
      timeoutErrorMessage: 'timeout',
    });
    return data;
  } catch (error) {
    console.log('Http request error', error);
    if (error?.message === 'timeout') {
      return {
        isTimeout: true,
      };
    }
  }
};

// CRU SEAppOccupants
export const getSeniorOccupantsBySeniorAppId = async seniorAppId => {
  try {
    const { data } = await instance.get(
      `/seappoccupantlookup/${seniorAppId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createSeniorOccupant = async seniorOccupant => {
  try {
    const { data } = await instance.post(
      `/SEAppOccupants`,
      seniorOccupant,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateSeniorOccupant = async seniorOccupant => {
  try {
    const { data } = await instance.patch(
      `/SEAppOccupants/${seniorOccupant.seappoccupantId}`,
      seniorOccupant,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const deleteSeniorOccupant = async seniorOccupant => {
  try {
    const { data } = await instance.delete(
      `/SEAppOccupants/${seniorOccupant.seappoccupantId}`,

      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

// CRU SEAppOtherProps
export const getOtherPropertiesBySeniorAppId = async seniorAppId => {
  try {
    const { data } = await instance.get(
      `/seappotherproplookup/${seniorAppId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createOtherProperty = async otherProperty => {
  try {
    //otherProperty.seappotherpropid = uuid.v4();
    const { data } = await instance.post(
      `/SEAppOtherProps`,
      otherProperty,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateOtherProperty = async otherProperty => {
  try {
    const { data } = await instance.patch(
      `/SEAppOtherProps/${otherProperty.seappotherpropid}`,
      otherProperty,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const deleteOtherProperty = async otherProperty => {
  try {
    const { data } = await instance.delete(
      `/SEAppOtherProps/${otherProperty.seappotherpropid}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

//CRU SeniorAppDetails
export const getSeniorAppDetailsBySeniorAppId = async seniorAppId => {
  try {
    const { data } = await instance.get(
      `/seappdetaillookup/${seniorAppId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createSeniorAppDetail = async seniorAppDetail => {
  try {
    const { data } = await instance.post(
      `/SEApplicationDetails`,
      seniorAppDetail,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateSeniorAppDetail = async seniorAppDetail => {
  try {
    const { data } = await instance.patch(
      `/SEApplicationDetails/${seniorAppDetail.seappdetailid}`,
      seniorAppDetail,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const deleteSeniorAppDetail = async seniorAppDetailId => {
  try {
    const { data } = await instance.delete(
      `/SEApplicationDetails/${seniorAppDetailId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

// CRU Senior App Financials
export const getSeniorAppFinancialBySeniorAppDetailId = async seniorAppDetailId => {
  try {
    const { data } = await instance.get(
      `/seappfinanciallookup/${seniorAppDetailId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const createSeniorAppFinancial = async seniorAppFinancial => {
  try {
    const { data } = await instance.post(
      `/SEApplicationFinancial`,
      seniorAppFinancial,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const updateSeniorAppFinancial = async seniorAppFinancial => {
  try {
    const { data } = await instance.patch(
      `/SEApplicationFinancial/${seniorAppFinancial.seappdetailid}`,
      seniorAppFinancial,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 * Deletes an senior exemption application financial by id
 *
 * @param {GUID} financialId
 * @returns
 */
const deleteSeniorApplicationsFinancial = async financialId => {
  try {
    const { data } = await instance.delete(
      `/seapplicationfinancial/${financialId}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

//Occupant Forms

export const getFinancialFormLookupByOccupantId = async seniorAppOccupantId => {
  try {
    const { data } = await instance.get(
      `api/seafinancialformlookupbyoccupantid/${seniorAppOccupantId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getOccupantLookupByFinancialFormId = async seniorAppFinancialFormId => {
  try {
    const { data } = await instance.get(
      `/seaoccupantlookupbyfinancialformId/${seniorAppFinancialFormId}`,
      setToken()
    );
    return data ? data : [];
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const financialFormsOccupantsRelationShip = async (
  seniorAppOccupantId,
  seniorAppFinancial
) => {
  try {
    const { data } = await instance.post(
      `/SEAFinancialFormsSEAOpccupantsRelationship/${seniorAppFinancial.sefinancialformsid}`,
      {
        seappoccupantid: seniorAppOccupantId,
      },
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export const getPortalAttachmentLocations = async () => {
  try {
    const { data } = await instance.get(
      '/optionsets/portalattachmentlocations',
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
