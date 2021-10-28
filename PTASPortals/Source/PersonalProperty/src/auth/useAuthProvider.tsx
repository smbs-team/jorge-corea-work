// useAuthProvider.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffect, useState } from 'react';
import { useIsAuthenticated } from '@azure/msal-react';
import { msalInstance } from 'index';
import { loginRequest } from './AuthConfig';
import { BrowserUtils } from '@azure/msal-browser';

interface IdTokenKlaims {
  emails: string[];
}

export interface UseAuthProvider {
  isAuthenticated: boolean;
  accessToken: string;
  identityToken: string;
  email: string;
  signOut: () => void;
}

const useAuthProvider = (): UseAuthProvider => {
  const isAuthenticated = useIsAuthenticated();
  const [accessToken, setAccessToken] = useState<string>('');
  const [identityToken, setIdentityToken] = useState<string>('');
  const [email, setEmail] = useState<string>('');

  const fetchAccessToken = async (): Promise<void> => {
    const account = msalInstance.getActiveAccount();
    if (!account) return;

    const emailFound = account.idTokenClaims as IdTokenKlaims;
    setEmail(emailFound.emails[0]);
    const response = await msalInstance.acquireTokenSilent({
      ...loginRequest,
      account,
    });

    const bearer = `Bearer ${response.accessToken}`;
    const identity = response.idToken;
    setAccessToken(bearer);

    setIdentityToken(identity);
  };

  // Shows how to ask for an access token, needed to call web services.
  useEffect(() => {
    isAuthenticated && fetchAccessToken();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [msalInstance, isAuthenticated]);

  const signOut = (): void => {
    msalInstance.logoutRedirect({
      account: msalInstance.getActiveAccount(),
      onRedirectNavigate: () => !BrowserUtils.isInIframe(),
    });
  };

  return {
    isAuthenticated,
    accessToken,
    identityToken,
    email,
    signOut,
  };
};

export default useAuthProvider;
