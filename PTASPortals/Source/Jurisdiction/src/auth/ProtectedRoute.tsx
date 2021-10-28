// ProtectedRoute.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { useEffect, useCallback } from 'react';
import {
  Route,
  RouteProps,
  Redirect,
  useHistory,
  useLocation,
} from 'react-router-dom';
import useAuth from './useAuth';
import { useAppContext } from 'contexts/AppContext';
import { debounce } from 'lodash';
import GenericMessage from 'components/GenericMessage';
import * as gfm from 'GeneralFormatMessage';

const ProtectedRoute: React.FC<RouteProps> = ({ ...rest }) => {
  const { isAuthenticated, userInfoFetching } = useAuth();
  const { contactProfile } = useAppContext();
  const history = useHistory();
  const location = useLocation();

  useEffect(() => {
    if (!userInfoFetching) return;

    let route = '/intro';

    if (contactProfile?.taxDistrictId) {
      route = '/levy';
    } else if (contactProfile?.jurisdictionId) {
      route = '/permits';
    } else if (!contactProfile?.id) {
      route = '/unauthorized';
    }

    redirectToRoute(location.pathname, route);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [contactProfile?.id, userInfoFetching]);

  const redirectToRoute = useCallback(
    debounce((currentPath: string, redirectPath: string) => {
      if (currentPath === redirectPath) return;

      history.replace(redirectPath);
    }, 800),
    []
  );

  if (!isAuthenticated) {
    return (
      <Redirect
        to={{
          pathname: '/signin',
          state: { originalPath: history.location },
        }}
      />
    );
  }

  if (!userInfoFetching) {
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

  return <Route {...rest} />;
};

export default ProtectedRoute;
