// AuthConfig.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { LogLevel } from '@azure/msal-browser';

// Browser check variables
// If you support IE, our recommendation is that you sign-in using Redirect APIs
// If you as a developer are testing using Edge InPrivate mode, please add "isEdge" to the if check
const ua = window.navigator.userAgent;
const msie = ua.indexOf('MSIE ');
const msie11 = ua.indexOf('Trident/');
const msedge = ua.indexOf('Edge/');
const firefox = ua.indexOf('Firefox');
const isIE = msie > 0 || msie11 > 0;
const isEdge = msedge > 0;
const isFirefox = firefox > 0; // Only needed if you need to support the redirect flow in Firefox incognito

// Get values from .env file.
const tenant = process.env.REACT_APP_MSAL_TENANT;
const signInPolicy = process.env.REACT_APP_MSAL_SIGN_IN_POLICY;
const applicationID = process.env.REACT_APP_MSAL_APPLICATIONID;
const reactRedirectUri = process.env.REACT_APP_MSAL_REACT_REDIRECT_URI;
const instance = process.env.REACT_APP_MSAL_INSTANCE;
const signInAuthority = `${instance}${tenant}/${signInPolicy}`;

// Config object to be passed to Msal on creation
export const msalConfig = {
  auth: {
    clientId: applicationID as string,
    redirectUri: reactRedirectUri as string,
    postLogoutRedirectUri: '/intro',
    authority: signInAuthority as string,
    knownAuthorities: [signInAuthority as string],
  },
  cache: {
    storeAuthStateInCookie: isIE || isEdge || isFirefox,
  },
  system: {
    loggerOptions: {
      loggerCallback: (
        level: number,
        message: string,
        containsPii: boolean
      ): void => {
        if (containsPii) {
          return;
        }
        switch (level) {
          case LogLevel.Error:
            console.error(message);
            return;
          case LogLevel.Info:
            console.info(message);
            return;
          case LogLevel.Verbose:
            console.debug(message);
            return;
          case LogLevel.Warning:
            console.warn(message);
            return;
          default:
            return;
        }
      },
    },
  },
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export const loginRequest = {
  scopes: ['openid', 'profile'],
};
