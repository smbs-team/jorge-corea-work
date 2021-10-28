//-----------------------------------------------------------------------
// <copyright file="ProtectedRoute.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { AuthConsumer } from '../contexts/AuthContext';

/**
 * Stateless component that customizes a protected route for the application.
 * It will redirect to the SignIn route when the user tries to access a component
 * that requires to be authenticated to access, it is currently not authenticated.
 * @param {*} { component: Component, ...rest }
 */
const ProtectedRoute = ({ component: Component, ...rest }) => (
  <AuthConsumer>
    {({ isLoggedIn }) => (
      <Route
        render={props =>
          isLoggedIn() ? <Component {...props} /> : <Redirect to="/signin" />
        }
        {...rest}
      />
    )}
  </AuthConsumer>
);

export default ProtectedRoute;
