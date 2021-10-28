// ProtectedRoute.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React from 'react';
import { Route, RouteProps, Redirect } from 'react-router-dom';

const ProtectedRoute = ({ ...rest }: RouteProps): JSX.Element => {
  const authorized = true;

  return authorized ? <Route {...rest} /> : <Redirect to="/intro" />;
};

export default ProtectedRoute;
