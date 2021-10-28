// AppContext.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, {
  ComponentType,
  FC,
  PropsWithChildren,
  useContext,
  useState,
} from 'react';
import { createContext } from 'react';
import { Contact } from '../routes/models/Views/Profile/Contact';
import { SnackProvider } from '@ptas/react-public-ui-library';
import { ProfileOption } from 'components/ProfileOptionsPopover';
import { useHistory } from 'react-router-dom';

interface AppContextProps {
  contactProfile: Contact | undefined;
  setContactProfile: React.Dispatch<React.SetStateAction<Contact | undefined>>;
  profileOptionsAnchor: HTMLElement | null;
  setProfileOptionsAnchor: React.Dispatch<
    React.SetStateAction<HTMLElement | null>
  >;
}

interface UseAppContextProps extends AppContextProps {
  showProfileOptions: (anchorEl: HTMLElement | null) => void;
  onClickProfileOption: (option: ProfileOption) => void;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const AppContext = createContext<AppContextProps>({} as any);

const AppProvider = ({ children }: PropsWithChildren<{}>): JSX.Element => {
  const [contactProfile, setContactProfile] = useState<Contact | undefined>();
  const [profileOptionsAnchor, setProfileOptionsAnchor] =
    useState<HTMLElement | null>(null);

  return (
    <AppContext.Provider
      value={{
        contactProfile,
        setContactProfile,
        profileOptionsAnchor,
        setProfileOptionsAnchor,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

const useAppContext = (): UseAppContextProps => {
  const context = useContext(AppContext);
  const { setProfileOptionsAnchor } = context;
  const history = useHistory();

  const showProfileOptions = (anchorEl: HTMLElement | null): void => {
    setProfileOptionsAnchor(anchorEl);
  };

  const onClickProfileOption = (option: ProfileOption): void => {
    if (option === 'editProfile') {
      history.push('/profile');
      showProfileOptions(null);
    } else if (option === 'signOut') {
      showProfileOptions(null);
    }
  };

  return {
    showProfileOptions,
    onClickProfileOption,
    ...context,
  };
};

const withAppProvider =
  <P extends object>(Component: ComponentType<P>): FC<P> =>
  (props: PropsWithChildren<P>): JSX.Element => {
    return (
      <SnackProvider>
        <AppProvider>
          <Component {...props} />
        </AppProvider>
      </SnackProvider>
    );
  };

export { AppProvider, useAppContext, withAppProvider };
