// AppContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ProfileOption } from 'components/ProfileOptionsPopover';
import { PortalContact } from 'models/portalContact';
import React, {
  useState,
  PropsWithChildren,
  FC,
  createContext,
  Dispatch,
  SetStateAction,
} from 'react';
import { useHistory } from 'react-router-dom';

interface AppContextProps {
  portalContact: PortalContact | undefined;
  setPortalContact: Dispatch<SetStateAction<PortalContact | undefined>>;
  isValidPortalContact: () => boolean;
  redirectPath: string | undefined;
  setRedirectPath: Dispatch<SetStateAction<string | undefined>>;
  profileOptionsAnchor: HTMLElement | null;
  showProfileOptions: (anchorEl: HTMLElement | null) => void;
  onClickProfileOption: (option: ProfileOption) => void;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const AppContext = createContext<AppContextProps>(undefined as any);

/**
 * A Context provider component
 *
 * @param props - Component children
 */

const AppProvider = (props: PropsWithChildren<{}>): JSX.Element => {
  const history = useHistory();
  const [portalContact, setPortalContact] =
    useState<PortalContact | undefined>(undefined);
  const [redirectPath, setRedirectPath] = useState<string>();
  const [profileOptionsAnchor, setProfileOptionsAnchor] =
    useState<HTMLElement | null>(null);

  const isValidPortalContact = (): boolean => {
    return (
      !!portalContact?.id &&
      !!portalContact.firstName &&
      !!portalContact.lastName &&
      !!portalContact.email?.email
    );
  };

  const showProfileOptions = (anchorEl: HTMLElement | null): void => {
    setProfileOptionsAnchor(anchorEl);
  };

  const onClickProfileOption = (option: ProfileOption): void => {
    if (option === 'editProfile') {
      setRedirectPath(history.location.pathname);
      history.push('/profile');
      showProfileOptions(null);
    } else if (option === 'signOut') {
      showProfileOptions(null);
      //TODO
    }
  };

  return (
    <AppContext.Provider
      value={{
        portalContact,
        setPortalContact,
        isValidPortalContact,
        redirectPath,
        setRedirectPath,
        profileOptionsAnchor,
        showProfileOptions,
        onClickProfileOption,
      }}
    >
      {props.children}
    </AppContext.Provider>
  );
};

export const withAppProvider =
  (Component: FC) =>
  (props: object): JSX.Element =>
    (
      <AppProvider>
        <Component {...props} />
      </AppProvider>
    );
