//-----------------------------------------------------------------------
// <copyright file="signalrService.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import http from './httpService';
import { refreshToken } from '../contexts/MagicLinkContext';

const instance = http.create({
  baseURL: process.env.REACT_APP_SIGNALR_URL,
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
 * Calls function to authenticate connection.
 *
 * @param {*} contactId
 * @returns json with url and token values
 */
const getSignalRKey = async contactId => {
  try {
    const { data } = await instance.get(`/negotiate`, setToken());

    if (data) {
      return data.accessToken;
    }
    return null;
  } catch (error) {
    console.log('Http request error', error);
  }
};

export /**
 * Sends a message by calling an azure function.
 *
 * @param {*} signalKey
 * @param {*} message
 * @returns
 */
const sendMessage = async message => {
  try {
    const { data } = await instance.post(`/notify`, message, setToken());

    return data;
  } catch (error) {
    console.log('Http request error', error);
  }
};
