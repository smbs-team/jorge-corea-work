// SignIn.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  MsalAuthenticationTemplate,
  MsalAuthenticationResult,
} from '@azure/msal-react';
import { InteractionType } from '@azure/msal-browser';
import { Redirect, useLocation, useHistory } from 'react-router-dom';
import { loginRequest } from 'auth/AuthConfig';
import HomeNotSigned from 'routes/models/Views/Home/NotSigned';
import * as gfm from 'GeneralFormatMessage';
import GenericMessage from 'components/GenericMessage';

interface StateLocation {
  originalPath: string;
}

// MsalAuthenticationTemplate takes care of asking for the authentication when needed.
function SignIn(): JSX.Element {
  const history = useHistory();
  const location = useLocation<StateLocation>();
  // Use to display logging errors.
  function ErrorComponent(result: MsalAuthenticationResult): JSX.Element {
    const { errorCode, errorMessage } = result.error ?? {};

    switch (errorCode) {
      case 'user_cancelled':
        history.replace('/intro');
        break;
      case 'popup_window_error':
        return <GenericMessage msg={gfm.popupBlockedMsg} />;
      default:
        throw new Error(errorMessage ?? 'Auth error');
    }
    return <React.Fragment />;
  }

  // USe to display when it is authenticating.
  function LoadingComponent(): JSX.Element {
    return <HomeNotSigned />;
  }
  return (
    <MsalAuthenticationTemplate
      interactionType={InteractionType.Popup}
      authenticationRequest={loginRequest}
      errorComponent={ErrorComponent}
      loadingComponent={LoadingComponent}
    >
      <Redirect to={location.state?.originalPath} />
    </MsalAuthenticationTemplate>
  );
}

export default SignIn;
