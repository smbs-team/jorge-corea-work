// useAuthProvider.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, Dispatch, SetStateAction } from 'react';
import { useIsAuthenticated } from '@azure/msal-react';

export interface UseAuthProvider {
  isAuthenticated: boolean;
  accessToken: string;
  identityToken: string;
  setAccessToken: Dispatch<SetStateAction<string>>;
  setIdentityToken: Dispatch<SetStateAction<string>>;
  setErrorContact: Dispatch<
    SetStateAction<{
      error: boolean;
      message?: string | undefined;
    }>
  >;
  errorContact: {
    error: boolean;
    message?: string | undefined;
  };
  userInfoFetching: boolean;
  setUserInfoFetching: Dispatch<SetStateAction<boolean>>;
}

const useAuthProvider = (): UseAuthProvider => {
  const isAuthenticated = useIsAuthenticated();

  const [userInfoFetching, setUserInfoFetching] = useState<boolean>(false);
  const [accessToken, setAccessToken] = useState<string>('');
  const [identityToken, setIdentityToken] = useState<string>('');
  const [errorContact, setErrorContact] = useState<{
    error: boolean;
    message?: string;
  }>({ error: false, message: 'User not configured to enter this page' });

  return {
    isAuthenticated,
    accessToken,
    identityToken,
    setAccessToken,
    setIdentityToken,
    errorContact,
    setErrorContact,
    userInfoFetching,
    setUserInfoFetching,
  };
};

export default useAuthProvider;
