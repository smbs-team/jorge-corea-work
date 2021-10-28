// useAuthTokenManager.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { getMagicToken } from 'api/TokenAPI';
import { useQueryParams } from 'hookrouter';
import jwt from 'jsonwebtoken';
import { useState } from 'react';

export type MagicTokenResult = 'success' | 'failed';

interface Result {
  setMagicToken: () => Promise<MagicTokenResult>;
  isLoading: boolean;
}

const MAGIC_TOKEN = 'magicToken';

const isTokenExpired = (token: string): boolean => {
  const decoded = jwt.decode(token ?? '');
  if (
    !decoded ||
    typeof decoded !== 'object' ||
    !decoded.exp ||
    Date.now() >= decoded.exp * 1000
  ) {
    return true;
  }
  return false;
};

function useAuthTokenManager(): Result {
  const [queryParams] = useQueryParams();
  const { token = 'invalid' } = queryParams;
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const analyzeMagicToken = (magicToken: string): MagicTokenResult => {
    if (!isTokenExpired(magicToken)) {
      setIsLoading(false);
      return 'success';
    }
    return 'failed';
  };

  return {
    setMagicToken: async (): Promise<MagicTokenResult> => {
      const localMagicToken = localStorage.getItem(MAGIC_TOKEN);

      if (localMagicToken && localMagicToken.trim().length) {
        const result = analyzeMagicToken(localMagicToken);
        if (result === 'success') return 'success';
      }

      localStorage.removeItem(MAGIC_TOKEN);

      if (token === 'invalid') {
        setIsLoading(false);
        return 'failed';
      }

      let magicToken = '';

      try {
        magicToken = await getMagicToken(token);
      } catch (error) {
        console.error('Failed getting token.', error);
        setIsLoading(false);
        return 'failed';
      }

      localStorage.setItem(MAGIC_TOKEN, magicToken);

      setIsLoading(false);

      return 'success';
    },
    isLoading,
  };
}

export default useAuthTokenManager;
