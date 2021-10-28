// App.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  ptasCamaTheme,
  SnackProvider,
  ErrorMessageProvider,
} from '@ptas/react-public-ui-library';
import { ThemeProvider } from '@material-ui/styles';
import { Redirect, Route, Switch } from 'react-router';
import Home from './routes/models/Views/Home';
import HomeNotSigned from './routes/models/Views/Home/NotSigned';
import Instruction from './routes/models/Views/Instruction/';
import ManageBusiness from './routes/models/Views/ManageBusiness';
import { IntlProvider } from 'react-intl';
import Spanish from './components/es-lang.json';
import English from './components/en-lang.json';
import Webinar from './routes/models/Views/Webinar';
import Tutorial from './routes/models/Views/Tutorial';
import PrimaryProfile from 'routes/models/Views/Profile';
import SecondaryProfile from 'routes/models/Views/Profile/SecondaryProfile';
import ManagePersonalProperty from './routes/models/Views/ManagePersonalProperty';
import NewBusiness from './routes/models/Views/NewBusiness';
import UpdateBusiness from './routes/models/Views/UpdateBusiness';
import { makeStyles } from '@material-ui/core';
import backgroundImage from './assets/images/Alki.png';
import { ErrorBoundary } from 'react-error-boundary';
import ErrorFallback from './services/common/ErrorBoundary';
import { withAppProvider } from 'contexts/AppContext';
import ProtectedRoute from 'auth/ProtectedRoute';
import AuthProvider from 'auth/AuthContext';
import SignIn from 'routes/models/Views/Auth/SignIn';

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
        width: '0' /* Remove scrollbar space */,
      },
    },
  },
});

function App(): JSX.Element {
  useGlobalStyles();

  return (
    <AuthProvider>
      <ThemeProvider theme={ptasCamaTheme}>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
          <IntlProvider locale={'en'} messages={lang}>
            <SnackProvider>
              <ErrorMessageProvider>
                <Switch>
                  <Route path="/signin" component={SignIn} />
                  <Route
                    exact
                    path="/"
                    render={(): React.ReactNode => <Redirect to="/intro" />}
                  />
                  <ProtectedRoute exact path="/home" component={Home} />
                  <Route exact path="/intro" component={HomeNotSigned} />
                  <Route exact path="/webinar" component={Webinar} />
                  <Route exact path="/tutorial" component={Tutorial} />
                  <Route exact path="/instruction" component={Instruction} />
                  <ProtectedRoute
                    exact
                    path="/manage-business/:businessId"
                    component={ManageBusiness}
                  />
                  <ProtectedRoute exact path="/profile" component={PrimaryProfile} />
                  <ProtectedRoute
                    exact
                    path="/sec-profile"
                    component={SecondaryProfile}
                  />
                  <ProtectedRoute
                    exact
                    path="/sec-profile/:contact"
                    component={SecondaryProfile}
                  />
                  <ProtectedRoute
                    exact
                    path="/sec-profile/:contact/:step"
                    component={SecondaryProfile}
                  />
                  <ProtectedRoute
                    exact
                    path="/manage-business-personal-property/:businessId"
                    component={ManagePersonalProperty}
                  />
                  <ProtectedRoute
                    exact
                    path="/new-business"
                    component={NewBusiness}
                  />
                  <ProtectedRoute
                    exact
                    path="/update-business"
                    component={UpdateBusiness}
                  />
                </Switch>
              </ErrorMessageProvider>
            </SnackProvider>
          </IntlProvider>
        </ErrorBoundary>
      </ThemeProvider>
    </AuthProvider>
  );
}

export default withAppProvider(App);
