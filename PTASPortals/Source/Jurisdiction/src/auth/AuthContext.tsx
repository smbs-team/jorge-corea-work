/* eslint-disable @typescript-eslint/no-explicit-any */
// AuthContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { createContext } from 'react';
import useAuthProvider, { UseAuthProvider } from './useAuthProvider';

export const AuthContext = createContext<UseAuthProvider>({} as any);

const AuthProvider: React.FC = ({ children }) => {
  const auth = useAuthProvider();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};

export default AuthProvider;
