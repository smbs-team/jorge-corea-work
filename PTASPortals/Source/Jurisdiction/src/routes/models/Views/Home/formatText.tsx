// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="home_title"
    defaultMessage="Import levy request forms"
  />
);

export const signIn = (
  <FormattedMessage id="home_sign_in" defaultMessage="Sign in" />
);

export const jurisdictionAccess = (
  <FormattedMessage
    id="home_jurisdiction_access"
    defaultMessage="Jurisdiction access portal"
  />
);

export const jurisdictionAccessDesc = (
  <FormattedMessage
    id="home_jurisdiction_access_desc"
    defaultMessage="Transfer important information"
  />
);
