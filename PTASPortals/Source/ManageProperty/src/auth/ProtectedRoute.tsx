// ProtectedRoute.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import GenericMessage from 'components/GenericMessage';
import { AppContext } from 'contexts/AppContext';
import React, { useContext, useEffect, useState, useRef } from 'react';
import {
  Route,
  RouteProps,
  useHistory,
  Redirect,
  useLocation,
} from 'react-router-dom';
import useAuth from './useAuth';
import * as gfm from 'GeneralFormatMessage';
import { contactApiService } from 'services/api/apiService/portalContact';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface ProtectedRouteProps extends RouteProps {}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ ...rest }) => {
  const { portalContact, setPortalContact, setRedirectPath } = useContext(
    AppContext
  );
  const fetchPortalContactRef = useRef<(email: string) => Promise<void>>();
  const history = useHistory();
  const location = useLocation();
  const [portalContactFetched, setPortalContactFetched] = useState<boolean>(
    false
  );
  //TODO: use auth when sign in functionality is done
  const { isAuthenticated, email } = useAuth() ?? {};

  const fetchPortalContact = async (email: string): Promise<void> => {
    setPortalContactFetched(false);
    const portalContactFound = (
      await contactApiService.getPortalContactByEmail(email)
    )?.data;
    if (!portalContactFound) {
      setRedirectPath(location.pathname);
      setPortalContactFetched(true);
      history.replace('/profile', { newContact: true });
    } else {
      setPortalContact(portalContactFound);
    }
    setPortalContactFetched(true);
  };

  fetchPortalContactRef.current = fetchPortalContact;

  useEffect(() => {
    //Check portal contact and redirect to profile page if not found
    const { current: fetchFnRef } = fetchPortalContactRef;
    if (isAuthenticated && !portalContact?.id && email && fetchFnRef) {
      fetchFnRef(email);
    }
  }, [isAuthenticated, email, portalContact]);

  if (!portalContactFetched && !portalContact && isAuthenticated) {
    const { component, ...pr } = rest;
    return (
      <Route
        {...pr}
        render={(): React.ReactNode => (
          <GenericMessage msg={gfm.userInformationLoading} />
        )}
      />
    );
  }

  if (!isAuthenticated)
    return (
      <Redirect
        to={{
          pathname: '/signin',
          state: { originalPath: location.pathname },
        }}
      />
    );

  return <Route {...rest} />;
};

export default ProtectedRoute;
