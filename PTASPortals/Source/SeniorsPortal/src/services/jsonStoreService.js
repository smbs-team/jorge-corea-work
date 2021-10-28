//-----------------------------------------------------------------------
// <copyright file="jsonStoreService.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import http from './httpService';
import { refreshToken } from '../contexts/MagicLinkContext';

const instance = http.create({
  baseURL: `${process.env.REACT_APP_DOCS_API_URL}/api`,
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

export /**
 *
 *
 * @param {*} route
 * @returns
 */
const getValue = async route => {
  console.log('notifyChangeOnFiles getValue', route);
  try {
    const { data } = await instance.get(
      `/JsonStore?route=${route}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 *
 *
 * @param {*} route
 * @param {*} value
 * @param {*} controller
 * @returns
 */
const postValue = async (route, value, controller) => {
  if (!route.includes('undefined') && !route.includes('null')) {
    try {
      if (controller) {
        value.controller = controller;
      }

      const { data } = await instance.post(
        `/JsonStore?route=${route}`,
        value,
        setToken()
      );

      return data;
    } catch (error) {
      console.log('Http request error', error);
    }
  }
  return null;
};

export /**
 *
 *
 * @param {*} route
 * @param {*} deleteDirectory
 * @returns
 */
const deleteValue = async (route, deleteDirectory) => {
  try {
    const { data } = await instance.delete(
      `/JsonStore?route=${route}&isDirectory=${deleteDirectory}`,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 * Call API to update a single record from the Json Store to Data Service.
 *
 * @param {*} route
 * @param {*} isUpdate
 * @returns
 */
const postSingleEntityOnDataService = async (route, isUpdate) => {
  try {
    const { data } = await instance.patch(
      `/ApplyJsonToDynamics/?route=${route}&isUpdate=${isUpdate}`,
      null,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 * Call API to update a all records matching the route (including child's) from the Json Store to Data Service.
 *
 * @param {*} route
 * @returns
 */
const postAllEntitiesOnDataService = async route => {
  try {
    const { data } = await instance.post(
      `/ApplyJsonToDynamics/?route=${route}`,
      null,
      setToken()
    );
    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
