// App.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  ptasCamaTheme,
  ErrorMessageProvider,
  SnackProvider,
} from '@ptas/react-public-ui-library';
import { ThemeProvider } from '@material-ui/styles';
import { Redirect, Route, Switch } from 'react-router';
import Profile from './routes/models/Views/Profile';
import Home from './routes/models/Views/Home';
import HomeNotSigned from './routes/models/Views/Home/NotSigned';
import HomeImprovement from './routes/models/Views/HomeImprovement';
import HomeDestroyedProperty from './routes/models/Views/HomeDestroyedProperty';
import CurrentUse from './routes/models/Views/CurrentUse';
import Instruction from './routes/models/Views/Instruction/Instruction';
import { IntlProvider } from 'react-intl';
import Spanish from './components/es-lang.json';
import English from './components/en-lang.json';
import { withAppProvider } from '../src/contexts/AppContext';
import { ErrorBoundary } from 'react-error-boundary';
import ErrorFallback from './services/common/ErrorBoundary';
import { makeStyles } from '@material-ui/core';
import backgroundImage from './assets/images/Alki.png';
import ProtectedRoute from 'auth/ProtectedRoute';
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
        width: '0',
      },
    },
  },
});

function App(): JSX.Element {
  useGlobalStyles();

  return (
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
                <ProtectedRoute
                  exact
                  path="/home-manage-property"
                  component={Home}
                />
                <ProtectedRoute
                  exact
                  path="/home-destroyed-property/"
                  component={HomeDestroyedProperty}
                />
                <ProtectedRoute
                  exact
                  path="/home-destroyed-property/:contactId"
                  component={HomeDestroyedProperty}
                />
                <ProtectedRoute
                  exact
                  path="/home-destroyed-property/:contactId/:destroyedPropertyId"
                  component={HomeDestroyedProperty}
                />
                <ProtectedRoute
                  exact
                  path="/home-destroyed-property/:contactId/:destroyedPropertyId/:step"
                  component={HomeDestroyedProperty}
                />
                <Route exact path="/intro" component={HomeNotSigned} />
                <Route exact path="/instruction" component={Instruction} />
                <ProtectedRoute
                  exact
                  path="/home-improvement"
                  component={HomeImprovement}
                />
                <ProtectedRoute
                  exact
                  path="/home-improvement/:contact"
                  component={HomeImprovement}
                />
                <ProtectedRoute
                  exact
                  path="/home-improvement/:contact/:applicationId"
                  component={HomeImprovement}
                />
                <ProtectedRoute
                  exact
                  path="/home-improvement/:contact/:applicationId/:step"
                  component={HomeImprovement}
                />
                <ProtectedRoute
                  exact
                  path="/home-improvement/:contact/:applicationId/:step/:parcelId"
                  component={HomeImprovement}
                />
                <ProtectedRoute
                  exact
                  path="/current-use"
                  component={CurrentUse}
                />
                <ProtectedRoute
                  exact
                  path="/current-use/:exemptionSelected"
                  component={CurrentUse}
                />
                <Route exact path="/profile" component={Profile} />
              </Switch>
            </ErrorMessageProvider>
          </SnackProvider>
        </IntlProvider>
      </ErrorBoundary>
    </ThemeProvider>
  );
}

export default withAppProvider(App);
