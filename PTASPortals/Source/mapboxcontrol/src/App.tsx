/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  ptasCamaTheme,
  SnackProvider,
  ErrorMessageAlert,
  ErrorMessageAlertProvider,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import { CssBaseline, Snackbar, ThemeProvider } from '@material-ui/core';
import { IntlProvider } from 'react-intl';
import { AppProvider } from 'contexts';
import { Pages } from 'pages';
import { ErrorBoundary } from 'react-error-boundary';
import { ErrorFallback } from 'components/common';

const lang = {};

const App = (): JSX.Element => {
  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <CssBaseline />
      <IntlProvider locale={'en'} messages={lang}>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
          <AppProvider>
            <SnackProvider>
              <ErrorMessageAlertProvider>
                <ErrorMessageAlertCtx.Consumer>
                  {({ errorMessage, errorDetail, open }): JSX.Element => {
                    return (
                      <Fragment>
                        <Snackbar
                          open={open}
                          autoHideDuration={6000}
                          onClose={(): void => {
                            return;
                          }}
                          anchorOrigin={{
                            vertical: 'top',
                            horizontal: 'center',
                          }}
                          style={{ maxWidth: 'unset !important' }}
                        >
                          <ErrorMessageAlert
                            onClose={(): void => {
                              return;
                            }}
                            errorMessage={errorMessage ?? ''}
                            errorDetails={errorDetail}
                            onButtonClick={(): void => {
                              window.location.reload();
                            }}
                          />
                        </Snackbar>
                        <Pages />
                      </Fragment>
                    );
                  }}
                </ErrorMessageAlertCtx.Consumer>
              </ErrorMessageAlertProvider>
            </SnackProvider>
          </AppProvider>
        </ErrorBoundary>
      </IntlProvider>
    </ThemeProvider>
  );
};

export default App;
