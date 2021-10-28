//-----------------------------------------------------------------------
// <copyright file="SignIn.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import LoadingSection from '../common/LoadingSection';
import { FormattedMessage } from 'react-intl';
import { redirectToB2C } from '../../contexts/MagicLinkContext';

/**
 * Stateless component that will render a loading indicator as a transition screen
 * for the B2C redirect process. *
 * @returns a JSX component.
 */
const SignIn = () => {
  redirectToB2C();
  return (
    <LoadingSection
      label={
        <FormattedMessage
          id="App_singing_message"
          defaultMessage="Signing in..."
        />
      }
    />
  );
};

export default SignIn;
