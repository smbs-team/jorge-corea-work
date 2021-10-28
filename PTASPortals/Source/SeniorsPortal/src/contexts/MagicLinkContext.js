//-----------------------------------------------------------------------
// <copyright file="MagicLinkContext.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import { getUrlParameters } from '../lib/helpers/util';
import decodeJWT from 'jwt-decode';

const instance = process.env.REACT_APP_B2C_INSTANCE;
const tenant = process.env.REACT_APP_B2C_TENANT;
const signUpPolicy = process.env.REACT_APP_B2C_SIGNINPOLICY;
const applicationId = process.env.REACT_APP_B2C_APPLICATIONID;
const cacheLocation = process.env.REACT_APP_B2C_CACHELOCATION;
const redirectURL = process.env.REACT_APP_B2C_REDIRECTURI;

export /**
 * Creates a B2C valid url and redirects to display the user the login form.
 *
 */
const redirectToB2C = () => {
  const b2cURL = `${instance}${tenant}/oauth2/v2.0/authorize?p=${signUpPolicy}&client_id=${applicationId}&nonce=defaultNonce&redirectUrlOverride=${redirectURL}&redirect_uri=${redirectURL}&scope=openid&response_type=id_token&prompt=login`;
  window.location.href = b2cURL;
};

export /**
 * If found, stores on the cache the token information coming from the URL response from B2C.
 * Then redirects to the Home page.
 */
const saveAccessTokenIfAny = () => {
  const params = getUrlParameters();
  if (params['access_token']) {
    const accessToken = params['access_token'];

    setCacheValue('SEP_TK', accessToken);
    window.location.href = `${redirectURL}/home`;
  }
};

export /**
 * Remove the values from the cache. For the Magic Link model this is all it takes to sign out a user.
 * Then redirects to the base URL.
 */
const signOut = () => {
  setCacheValue('SEP_TK', null);
  window.location.href = `${redirectURL}/intro`;
};

export /**
 * Gets the current token from the cache storage.
 *
 * @returns {string} that represents the current access token.
 */
const getAccessToken = () => {
  return getCacheValue('SEP_TK');
};

export /**
 * Will check exp date for current B2C token can start the refresh token process if under 1 min to expire.
 *
 * @returns {string} B2C bearer token.
 */
const refreshToken = () => {
  const token = getAccessToken();
  if (token) {
    let decoded = decodeJWT(token);
    // convert to seconds, same as jwt token exp date, set it to start refreshing token 1 min before (6000 ms)
    const nowDate = (Date.now() - 6000) / 1000;
    if (nowDate >= decoded.exp) {
      redirectToB2C();
    }
  }
  return token;
};

/**
 * Stores a value in the cache store.
 *
 * @param {*} key
 * @param {*} value
 */
const setCacheValue = (key, value) => {
  switch (cacheLocation.toLowerCase()) {
    case 'localstorage':
      localStorage.setItem(key, value);
      break;

    case 'sessionstorage':
      sessionStorage.setItem(key, value);
      break;

    default:
      break;
  }
};

/**
 * Returns a value from the cache store.
 *
 * @param {*} key
 * @returns {string} value coming from the cache store.
 */
const getCacheValue = key => {
  let value = null;
  switch (cacheLocation.toLowerCase()) {
    case 'localstorage':
      value = localStorage.getItem(key);
      return value === 'null' ? null : value;

    case 'sessionstorage':
      value = sessionStorage.getItem(key);
      return value === 'null' ? null : value;

    default:
      return null;
  }
};
