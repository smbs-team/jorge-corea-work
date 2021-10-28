import * as dynamicServices from './dynamics-service';
import {
  deleteValue,
  postValue,
  getValue,
  postSingleEntityOnDataService,
  postAllEntitiesOnDataService,
} from './jsonStoreService';
import { arrayNullOrEmpty } from '../lib/helpers/util';
import { checkForNullsOnFinancialForms } from '../lib/helpers/util';

const dataService = process.env.REACT_APP_DATA_SERVICE;

export const getRootPath = () => {
  return {
    rootPath: localStorage.getItem('rootPath'),
    contactId: localStorage.getItem('contact'),
    seniorAppId: localStorage.getItem('application'),
  };
};

export const setRootIds = (seniorApp, contact) => {
  let contactId = contact ? contact.contactid : seniorApp.contactid;
  let seniorAppId = seniorApp.seapplicationid;
  localStorage.setItem('contact', contactId);
  localStorage.setItem('application', seniorAppId);
  localStorage.setItem('rootPath', `${contactId}/${seniorAppId}`);
};

/**
 *
 *
 * @param {object or string} object
 * @param {string} route
 * @param {function()} dynamicsCall
 * @returns
 */
const selectGetProvider = async (object, route, controller, dynamicsCall) => {
  if (dataService == 'JSON') {
    const values = await getValue(route);
    if (values && values.length && controller) {
      const newValues = values.reduce(
        (a, o) => (o.json.controller == controller && a.push(o.json), a),
        []
      );
      return newValues;
    }
    return values ? values : [];
  } else {
    return await dynamicsCall(object);
  }
};

/**
 *
 *
 * @param {object or id} object
 * @param {string} route
 * @param {string} controller
 * @param {function()} dynamicsCall
 */
const selectPostProvider = async (object, route, controller, dynamicsCall) => {
  if (dataService == 'JSON') {
    await postValue(route, object, controller);
    return object;
  } else {
    return await dynamicsCall(object);
  }
};

/**
 *
 *
 * @param {object or string} object
 * @param {string} route
 */
const selectDeleteProvider = async (object, route, dynamicsCall) => {
  if (dataService == 'JSON') {
    await deleteValue(route, false);
  } else {
    await dynamicsCall(object);
  }
};

export /**
 * Gets all the senior application, pending from JSON Storage, and existing from Data Services
 *
 * @param {*} contactId
 * @returns
 */
const getSeniorApplications = async contactId => {
  setRootIds({ contactid: contactId, seapplicationid: null });
  let dynamicsResponse = await dynamicServices.getSeniorApplications(contactId);
  let jsonResponse = [];

  if (dataService == 'JSON') {
    // jsonResponse = await selectGetProvider(
    //   contactId,
    //   contactId,
    //   'seapplications',
    //   dynamicServices.getSeniorApplications
    // );
  }

  return [...dynamicsResponse, ...jsonResponse];
};

export /**
 * Creates a new senior application in Json Store or in Data Services
 *
 * @param {*} seniorApp
 * @returns
 */
const createSeniorApplication = async (seniorApp, dataServices) => {
  setRootIds(seniorApp);
  if (dataServices) {
    return await dynamicServices.createSeniorApplication(seniorApp);
  } else {
    return await selectPostProvider(
      seniorApp,
      `${seniorApp.contactid}/${seniorApp.seapplicationid}`,
      'seapplications',
      dynamicServices.createSeniorApplication
    );
  }
};

export /**
 * Updates a senior application on the Json Store
 *
 * @param {*} seniorApp
 * @returns
 */
const updateSeniorApplication = async (seniorApp, dataServices) => {
  if (dataServices) {
    return await dynamicServices.updateSeniorApplication(seniorApp);
  } else {
    return await selectPostProvider(
      seniorApp,
      `${seniorApp.contactid}/${seniorApp.seapplicationid}`,
      'seapplications',
      dynamicServices.updateSeniorApplication
    );
  }
};

export /**
 * Returns all file metadata entities associated with an specific senior app id from the Json store or the data service.
 *
 * @param {*} seniorAppId
 * @returns
 */
const getFileMetadataListBySeniorAppId = async seniorAppId => {
  return await selectGetProvider(
    seniorAppId,
    `${getRootPath().rootPath}`,
    'FileAttachmentsMetadata',
    dynamicServices.getFileMetadataListBySeniorAppId
  );
};

export /**
 * Creates a file metadata entity on Json Store or Data Services
 *
 * @param {*} fileMetadata
 * @returns
 */
const createFileMetadata = async (
  fileMetadata,
  useDataServiceProvider = true
) => {
  if (useDataServiceProvider) {
    return await selectPostProvider(
      fileMetadata,
      `${getRootPath().rootPath}/${fileMetadata.id}`,
      'FileAttachmentsMetadata',
      dynamicServices.createFileMetadata
    );
  } else {
    return await dynamicServices.createFileMetadata(fileMetadata);
  }
};

export /**
 *Updates a file metadata entity on Json Store or Data Services
 *
 * @param {*} fileMetadata
 * @returns
 */
const updateFileMetadata = async (
  fileMetadata,
  useDataServiceProvider = true
) => {
  if (useDataServiceProvider) {
    return await selectPostProvider(
      fileMetadata,
      `${getRootPath().rootPath}/${fileMetadata.id}`,
      'FileAttachmentsMetadata',
      dynamicServices.updateFileMetadata
    );
  } else {
    return await dynamicServices.updateFileMetadata(fileMetadata);
  }
};

export /**
 * Deletes  a file metadata entity on Json Store or Data Services.
 *
 * @param {*} fileMetadataId
 * @returns
 */
const deleteFileMetadata = async fileMetadataId => {
  return await selectDeleteProvider(
    fileMetadataId,
    `${getRootPath().rootPath}/${fileMetadataId}/${fileMetadataId}`,
    dynamicServices.deleteFileMetadata
  );
};

export /**
 * Returns all existing occupant entities associated to a specific Senior Application Id
 *
 * @param {*} seniorAppId
 * @returns
 */
const getSeniorOccupantsBySeniorAppId = async seniorAppId => {
  return await selectGetProvider(
    seniorAppId,
    getRootPath().rootPath,
    'SEAppOccupants',
    dynamicServices.getSeniorOccupantsBySeniorAppId
  );
};

export /**
 * Create a new occupant entity on Json Store or data services.
 *
 * @param {*} seniorOccupant
 * @returns
 */
const createSeniorOccupant = async seniorOccupant => {
  return await selectPostProvider(
    seniorOccupant,
    `${getRootPath().rootPath}/${seniorOccupant.seappoccupantId}`,
    'SEAppOccupants',
    dynamicServices.createSeniorOccupant
  );
};

export /**
 * Update an occupant entity on Json Store or data services.
 *
 * @param {*} seniorOccupant
 * @returns
 */
const updateSeniorOccupant = async seniorOccupant => {
  return await selectPostProvider(
    seniorOccupant,
    `${getRootPath().rootPath}/${seniorOccupant.seappoccupantId}`,
    'SEAppOccupants',
    dynamicServices.updateSeniorOccupant
  );
};

export /**
 * Delete an occupant entity on Json Store or data services.
 *
 * @param {*} seniorOccupant
 * @returns
 */
const deleteSeniorOccupant = async seniorOccupant => {
  return await selectDeleteProvider(
    seniorOccupant,
    `${getRootPath().rootPath}/${seniorOccupant.seappoccupantId}/${
      seniorOccupant.seappoccupantId
    }`,
    dynamicServices.deleteSeniorOccupant
  );
};

export /**
 * Get all existing properties associated with an specific senior app id.
 *
 * @param {*} seniorAppId
 * @returns
 */
const getOtherPropertiesBySeniorAppId = async seniorAppId => {
  return await selectGetProvider(
    seniorAppId,
    getRootPath().rootPath,
    'SEAppOtherProps',
    dynamicServices.getOtherPropertiesBySeniorAppId
  );
};

export /**
 * Create a new property entity on Json Store or data services.
 *
 * @param {*} otherProperty
 * @returns
 */
const createOtherProperty = async otherProperty => {
  return await selectPostProvider(
    otherProperty,
    `${getRootPath().rootPath}/${otherProperty.seappotherpropid}`,
    'SEAppOtherProps',
    dynamicServices.createOtherProperty
  );
};

export /**
 * Update a property entity on Json Store or data services.
 *
 * @param {*} otherProperty
 * @returns
 */
const updateOtherProperty = async otherProperty => {
  return await selectPostProvider(
    otherProperty,
    `${getRootPath().rootPath}/${otherProperty.seappotherpropid}`,
    'SEAppOtherProps',
    dynamicServices.updateOtherProperty
  );
};

export /**
 * Delete a property entity on Json Store or data services.
 *
 * @param {*} otherProperty
 * @returns
 */
const deleteOtherProperty = async otherProperty => {
  return await selectDeleteProvider(
    otherProperty,
    `${getRootPath().rootPath}/${otherProperty.seappotherpropid}/${
      otherProperty.seappotherpropid
    }`,
    dynamicServices.deleteOtherProperty
  );
};

export /**
 * Gets all the senior application years (details) associated with an specific senior application id from JsonStore and Data Service
 *
 * @param {*} seniorAppId
 * @returns
 */
const getSeniorAppDetailsBySeniorAppId = async seniorAppId => {
  let baseUrl = `${getRootPath().contactId}/${seniorAppId}`;
  let dynamicsResponse = [];
  dynamicsResponse = await dynamicServices.getSeniorAppDetailsBySeniorAppId(
    seniorAppId
  );

  dynamicsResponse = arrayNullOrEmpty(dynamicsResponse) ? [] : dynamicsResponse;
  let jsonResponse = [];

  if (dataService == 'JSON') {
    jsonResponse = await selectGetProvider(
      seniorAppId,
      baseUrl,
      'SEApplicationDetails',
      dynamicServices.getSeniorAppDetailsBySeniorAppId
    );
    jsonResponse = arrayNullOrEmpty(jsonResponse) ? [] : jsonResponse;
  }

  return [...dynamicsResponse, ...jsonResponse];
};

export /**
 * Create a new senior application year (detail) on the Json Store or the data service.
 *
 * @param {*} seniorAppDetail
 * @returns
 */
const createSeniorAppDetail = async seniorAppDetail => {
  return await selectPostProvider(
    seniorAppDetail,
    `${getRootPath().rootPath}/${seniorAppDetail.seappdetailid}`,
    'SEApplicationDetails',
    dynamicServices.createSeniorAppDetail
  );
};

export /**
 * Update a senior application year (detail) on the Json Store or the data service.
 *
 * @param {*} seniorAppDetail
 * @returns
 */
const updateSeniorAppDetail = async seniorAppDetail => {
  return await selectPostProvider(
    seniorAppDetail,
    `${getRootPath().rootPath}/${seniorAppDetail.seappdetailid}`,
    'SEApplicationDetails',
    dynamicServices.updateSeniorAppDetail
  );
};

export /**
 * Delete a senior application year (detail) on the Json Store or the data service.
 *
 * @param {*} seniorAppDetailId
 * @returns
 */
const deleteSeniorAppDetail = async seniorAppDetailId => {
  return await selectDeleteProvider(
    seniorAppDetailId,
    `${getRootPath().rootPath}/${seniorAppDetailId}/${seniorAppDetailId}`,
    dynamicServices.deleteSeniorAppDetail
  );
};

export /**
 * Gets all the senior application financial forms associated with an specific senior application year (detail) id.
 *
 * @param {*} seniorAppDetailId
 * @returns
 */
const getSeniorAppFinancialBySeniorAppDetailId = async seniorAppDetailId => {
  return await selectGetProvider(
    seniorAppDetailId,
    `${getRootPath().rootPath}/${seniorAppDetailId}`,
    'SEApplicationFinancial',
    dynamicServices.getSeniorAppFinancialBySeniorAppDetailId
  );
};

export /**
 * Create a new financial form entity on Json store or data services.
 *
 * @param {*} seniorAppFinancial
 * @returns
 */
const createSeniorAppFinancial = async seniorAppFinancial => {
  let propertiesToBeExcluded = {
    sefinancialformsid: 'sefinancialformsid',
    financialformtype: 'financialformtype',
    filertype: 'filertype',
    yearid: 'yearid',
    seappdetailid: 'seappdetailid',
  };
  if (
    checkForNullsOnFinancialForms(seniorAppFinancial, propertiesToBeExcluded)
  ) {
    return await selectPostProvider(
      seniorAppFinancial,
      `${getRootPath().rootPath}/${seniorAppFinancial.seappdetailid}/${
        seniorAppFinancial.sefinancialformsid
      }`,
      'SEApplicationFinancial',
      dynamicServices.createSeniorAppFinancial
    );
  }

  return null;
};

export /**
 * Update a financial form entity on Json store or data services.
 *
 * @param {*} seniorAppFinancial
 * @returns
 */
const updateSeniorAppFinancial = async seniorAppFinancial => {
  return await selectPostProvider(
    seniorAppFinancial,
    `${getRootPath().rootPath}/${seniorAppFinancial.seappdetailid}/${
      seniorAppFinancial.sefinancialformsid
    }`,
    'SEApplicationFinancial',
    dynamicServices.updateSeniorAppFinancial
  );
};

export /**
 * Deletes an senior exemption application financial by id
 *
 * @param {GUID} financialFormId
 * @returns
 */
const deleteSeniorApplicationsFinancial = async (
  financialFormId,
  seniorAppDetailId
) => {
  return await selectDeleteProvider(
    financialFormId,
    `${
      getRootPath().rootPath
    }/${seniorAppDetailId}/${financialFormId}/${financialFormId}`,
    dynamicServices.deleteSeniorApplicationsFinancial
  );
};

export /**
 * CAll API to update the contact entity. Applies only if data service is Json Store.
 *
 * @param {*} contactId
 * @returns
 */
const updateContactOnDataService = async contactId => {
  if (dataService == 'JSON') {
    return await postSingleEntityOnDataService(`${contactId}`, true);
  }
};

export /**
 * Call API to post all data from a senior application from the Json Store to Data service.
 *
 * @param {*} seniorApp
 * @returns
 */
const postSeniorApplicationOnDataService = async seniorApp => {
  let contactId = seniorApp.contactid;
  if (dataService == 'JSON') {
    return await postAllEntitiesOnDataService(
      `${contactId}/${seniorApp.seapplicationid}`
    );
  }
};

//Add occupants.

export /**
 *
 *
 * @param {*} seniorAppOccupantId
 * @returns
 */
const financialFormsOccupantsRelationShip = async (
  seniorAppOccupantId,
  seniorAppFinancial
) => {
  return await selectPostProvider(
    {
      seappoccupantid: seniorAppOccupantId,
    },
    `${getRootPath().rootPath}/${seniorAppFinancial.seappdetailid}/${
      seniorAppFinancial.sefinancialformsid
    }`,
    'SEAFinancialFormsSEAOpccupantsRelationship',
    dynamicServices.financialFormsOccupantsRelationShip
  );
};
