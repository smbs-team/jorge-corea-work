// App.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect } from 'react';
import { ptasCamaTheme } from '@ptas/react-public-ui-library';
import { ThemeProvider } from '@material-ui/styles';
import { Redirect, Route, Switch } from 'react-router';
import HomeNotSigned from './routes/models/Views/Home/NotSigned';
import Profile from './routes/models/Views/Profile';
import Permits from './routes/models/Views/Permits';
import Levy from './routes/models/Views/Levy';
import { IntlProvider } from 'react-intl';
import Spanish from './components/es-lang.json';
import English from './components/en-lang.json';
import { withAppProvider } from 'contexts/AppContext';
import { makeStyles } from '@material-ui/core';
import backgroundImage from './assets/images/Alki.png';
import { ErrorMessageProvider } from '@ptas/react-public-ui-library';
import { ErrorBoundary } from 'react-error-boundary';
import ErrorFallback from './services/common/ErrorBoundary';
import ProtectedRoute from './auth/ProtectedRoute';
import SignIn from 'components/Auth/SignIn';
import useAuth from 'auth/useAuth';
import Unauthorized from 'routes/models/Views/Unauthorized';

const local = navigator.language;
let lang = {};
if (local === 'en-US') {
  lang = English;
} else {
  lang = Spanish;
}

const useGlobalStyles = makeStyles({
  '@global': {
    body: {
      backgroundImage: `url(${backgroundImage})`,
      backgroundAttachment: 'fixed',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',

      '&::-webkit-scrollbar': {
        width: '0',
      },
    },
  },
});

function App(): JSX.Element {
  useGlobalStyles();
  const { isAuthenticated, fetchAccessToken } = useAuth();

  useEffect(() => {
    isAuthenticated && fetchAccessToken();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isAuthenticated]);

  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <ErrorBoundary FallbackComponent={ErrorFallback}>
        <IntlProvider locale={'en'} messages={lang}>
          <ErrorMessageProvider>
            {/* <AppShowErrorMessage
              showErrorMessage={errorContact.error}
              message={errorContact.message ?? ''}
            > */}
            <Switch>
              <Route path="/signin" component={SignIn} />
              <ProtectedRoute
                exact
                path="/unauthorized"
                component={Unauthorized}
              />
              <Route
                exact
                path="/"
                render={(): React.ReactNode => <Redirect to="/intro" />}
              />
              <Route exact path="/intro" component={HomeNotSigned} />
              <ProtectedRoute exact path="/profile" component={Profile} />
              <ProtectedRoute exact path="/permits" component={Permits} />
              <ProtectedRoute exact path="/levy" component={Levy} />
            </Switch>
            {/* </AppShowErrorMessage> */}
          </ErrorMessageProvider>
        </IntlProvider>
      </ErrorBoundary>
    </ThemeProvider>
  );
}

export default withAppProvider(App);
