// useAuth.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useAppContext } from 'contexts/AppContext';
import { useContext } from 'react';
import { AuthContext } from './AuthContext';
import { loginRequest } from './AuthConfig';
import { UseAuthProvider } from './useAuthProvider';
import { msalInstance } from 'index';
import { apiService } from 'services/api/apiService';
import { BrowserUtils } from '@azure/msal-browser';

interface AuthProps extends UseAuthProvider {
  fetchAccessToken: () => Promise<void>;
  fetchContact: (email: string) => Promise<void>;
  signOut: () => void;
}

interface IdTokenKlaims {
  emails: string[];
}

const useAuth = (): AuthProps => {
  const { setContactProfile } = useAppContext();
  // const history = useHistory();

  const context = useContext(AuthContext);

  const {
    setAccessToken,
    setIdentityToken,
    setErrorContact,
    setUserInfoFetching,
  } = context;

  const fetchAccessToken = async (): Promise<void> => {
    const account = msalInstance.getActiveAccount();
    if (!account) return;

    const emailFound = account.idTokenClaims as IdTokenKlaims;

    fetchContact(emailFound.emails[0]);

    const response = await msalInstance.acquireTokenSilent({
      ...loginRequest,
      account,
    });

    const bearer = `Bearer ${response.accessToken}`;
    const identity = response.idToken;
    setAccessToken(bearer);
    setIdentityToken(identity);
  };

  const fetchContact = async (email: string): Promise<void> => {
    setUserInfoFetching(false);

    const contactResponse = await apiService.getContact(
      //   'nancy.jones@testemail.com' // permits
      // 'mary@example.com' // levy
      email
    );

    setContactProfile(contactResponse.data);
    setUserInfoFetching(true);

    if (contactResponse.data) return;

    setErrorContact((prev) => {
      return {
        ...prev,
        error: true,
      };
    });
  };

  const signOut = (): void => {
    // localStorage.clear();
    msalInstance.logoutRedirect({
      account: msalInstance.getActiveAccount(),
      onRedirectNavigate: () => !BrowserUtils.isInIframe(),
    });
  };

  return {
    fetchAccessToken,
    fetchContact,
    signOut,
    ...context,
  };
};

export default useAuth;
