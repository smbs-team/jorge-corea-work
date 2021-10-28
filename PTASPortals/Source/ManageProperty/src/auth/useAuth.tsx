// useAuth.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { AuthContext } from './AuthContext';
import { UseAuthProvider } from './useAuthProvider';

const useAuth = (): UseAuthProvider | null => {
  const context = useContext(AuthContext);
  if (context === null) {
    throw new Error('useAuth must be used within a AuthProvider');
  }
  return context;
};

export default useAuth;
