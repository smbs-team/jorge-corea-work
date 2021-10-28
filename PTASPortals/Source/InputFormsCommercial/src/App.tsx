// App.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import NotFound from 'components/NotFound';
import routes from 'router';
import { useRoutes } from 'hookrouter';
import {
  Backdrop,
  Box,
  CircularProgress,
  CssBaseline,
  Snackbar,
  ThemeProvider,
} from '@material-ui/core';
import { ptasCamaTheme, ErrorMessageAlert } from '@ptas/react-ui-library';
import AppBar from 'components/AppBar';
import './index.css';
import '@ptas/react-ui-library/dist/index.css';
import useTokenManager, { MagicTokenResult } from 'hooks/useAuthTokenManager';
import { useAsync } from 'react-use';
import { Fragment, useState } from 'react';
import useErrorSnackStore from 'stores/useErrorSnackStore';
import { setUpInterceptors } from 'api/AxiosLoader';

function App(): JSX.Element {
  const routeResult = useRoutes(routes);
  const { setMagicToken, isLoading } = useTokenManager();
  const [authResult, setAuthResult] = useState<MagicTokenResult>('failed');
  const errorSnackStore = useErrorSnackStore();

  useAsync(async () => {
    const result = await setMagicToken();
    setAuthResult(result);
    if (result === 'success') {
      setUpInterceptors(errorSnackStore);
      window.history.replaceState(null, '', window.location.pathname);
    }
  }, []);

  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <CssBaseline />
      {authResult === 'success' && (
        <Fragment>
          <AppBar />
          <Box paddingRight={3} paddingLeft={3}>
            {routeResult ? routeResult : <NotFound />}
          </Box>
        </Fragment>
      )}
      <Backdrop
        open={authResult === 'failed'}
        style={{ fontSize: 24, color: 'white' }}
      >
        {!isLoading ? (
          <label>Authentication failed, please login again.</label>
        ) : (
          <Fragment>
            <CircularProgress color="inherit" style={{ marginRight: 12 }} />
            <label>Authenticating...</label>
          </Fragment>
        )}
      </Backdrop>
      <Snackbar
        open={errorSnackStore.isOpen}
        autoHideDuration={errorSnackStore.errorDetails ? null : 6000}
        onClose={errorSnackStore.handleOnClose}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
        style={{ maxWidth: 'unset !important' }}
      >
        <ErrorMessageAlert
          onClose={errorSnackStore.handleOnClose}
          errorMessage={errorSnackStore.errorMessage}
          errorDetails={errorSnackStore.errorDetails}
          onButtonClick={errorSnackStore.handleOnClick}
        />
      </Snackbar>
    </ThemeProvider>
  );
}

export default App;
