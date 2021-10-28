import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { AuthConsumer } from '../contexts/AuthContext';

const AuthRoute = ({ component: Component, ...rest }) => (
  <AuthConsumer>
    {({ isLoggedIn }) => (
      <Route
        render={props =>
          isLoggedIn() ? <Redirect to="/home" /> : <Redirect to="/intro" />
        }
        {...rest}
      />
    )}
  </AuthConsumer>
);

export default AuthRoute;
