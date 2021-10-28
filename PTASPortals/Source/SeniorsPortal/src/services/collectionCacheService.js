//-----------------------------------------------------------------------
// <copyright file="collectionCacheService.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import * as jsonStore from './jsonStoreService';
import * as dynamicsService from './dynamics-service';

// base url to store the cache information.
const cachePath = 'collectionsCache';

export /**
 * Retrieves the collection cache object from the JSON Store.
 *
 * @returns the collection cache object.
 */
const getCacheInformation = async () => {
  return await jsonStore.getValue(`${cachePath}/${cachePath}`);
};

export /**
 * Stores the cache information on the JSON Store
 *
 * @param {object} cacheInformation
 */
const setCacheInformation = async cacheInformation => {
  await jsonStore.postValue(`${cachePath}`, cacheInformation);
};

export /**
 * Returns the respective collection from the JSON Store, if the cache should be refreshed it will post the new collection values
 * after fetching them from Dynamics.
 *
 * @param {string} collectionName
 * @param {object} cacheInformation
 * @returns {object} containing the collection array and the cache information.
 */
const getCollection = async (
  collectionName,
  cacheInformation,
  updateCacheCollectionState
) => {
  let collection = null;

  if (refreshCacheValidation(cacheInformation, collectionName)) {
    collection = await getDynamicsCollection(collectionName);
    await postCollection(collectionName, collection);
    updateCacheCollectionState(collectionName, new Date().toUTCString());
  } else {
    collection = await jsonStore.getValue(
      `${cachePath}/${collectionName}/${collectionName}`
    );
  }

  return collection;
};

/**
 * Validates the cacheInformation object to confirm if the cache will need to be refreshed or not.
 *
 * @param {object} cacheInformation
 * @returns {boolean} true if the cache needs refreshing, false otherwise.
 */
const refreshCacheValidation = (cacheInformation, collectionName) => {
  if (cacheInformation && cacheInformation[collectionName]) {
    let lastUpdateDate = new Date(cacheInformation[collectionName]);
    lastUpdateDate.setDate(
      lastUpdateDate.getDate() + process.env.REACT_APP_CACHE_DURATION
    );
    const currentDate = new Date();

    if (currentDate >= lastUpdateDate) {
      return true;
    } else {
      return false;
    }
  } else {
    return true;
  }
};

/**
 * Gets the collection from the Dynamics CE service.
 *
 * @param {*} collectionName
 * @returns {object[]} returns the collection array.
 */
const getDynamicsCollection = async collectionName => {
  switch (collectionName.toLowerCase()) {
    case 'counties':
      return await dynamicsService.getCounties();
    case 'years':
      return await dynamicsService.getYears();
    case 'relationships':
      return await dynamicsService.getRelationships();
    case 'accounttypes':
      return await dynamicsService.getAccountTypes();
    case 'exemptionsources':
      return await dynamicsService.getExemptionSources();
    case 'exemptiontypes':
      return await dynamicsService.getExemptionTypes();
    case 'incomelevels':
      return await dynamicsService.getIncomeLevels();
    case 'mediatypes':
      return await dynamicsService.getMediaTypes();
    case 'purposes':
      return await dynamicsService.getPurposes();
    case 'splitcodes':
      return await dynamicsService.getSplitCodes();
    case 'disabilityincomesources':
      return await dynamicsService.getDisabilityIncomeSources();
    case 'financialfilertypes':
      return await dynamicsService.getFinancialFilerTypes();
    case 'financialformtypes':
      return await dynamicsService.getFinancialFormTypes();
    case 'countries':
      return await dynamicsService.getCountries();
    case 'medicareplans':
      return await dynamicsService.getMedicarePlans();
    case 'seniorappstatuses':
      return await dynamicsService.getSeniorAppStatuses();
    case 'seniorappdetailstatuses':
      return await dynamicsService.getSeniorAppDetailStatuses();
    case 'portalattachmentlocations':
      return await dynamicsService.getPortalAttachmentLocations();
    default:
      return [];
  }
};

/**
 * Saves the collection to the JSON Store
 *
 * @param {string} collectionName
 * @param {object} collection
 */
const postCollection = async (collectionName, collection) => {
  if (collectionName && collection) {
    await jsonStore.postValue(`${cachePath}/${collectionName}`, collection);
  }
};
