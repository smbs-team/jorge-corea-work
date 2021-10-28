// ProfileContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { ComponentType, FC, PropsWithChildren, useContext } from 'react';
import { createContext } from 'react';
import useProfileUtil, { UseProfileUtil } from './useProfileUtil';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface ProfileContextProps extends UseProfileUtil {}

const ProfileContext = createContext<ProfileContextProps | undefined>(
  undefined
);

const ProfileProvider = ({ children }: PropsWithChildren<{}>): JSX.Element => {
  return (
    <ProfileContext.Provider
      value={{
        ...useProfileUtil(),
      }}
    >
      {children}
    </ProfileContext.Provider>
  );
};

const useProfile = (): ProfileContextProps => {
  const context = useContext(ProfileContext);
  if (context === undefined) {
    throw new Error('useProfile must be used within a ProfileProvider');
  }
  return context;
};

const withProfileProvider = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  return (
    <ProfileProvider>
      <Component {...props} />
    </ProfileProvider>
  );
};

export { ProfileProvider, useProfile, withProfileProvider };
