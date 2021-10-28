//-----------------------------------------------------------------------
// <copyright file="GeneralFormatTexts.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import { FormattedMessage } from 'react-intl';
import React from 'react';

export const when = <FormattedMessage id="when" defaultMessage="When" />;

export const where = <FormattedMessage id="where" defaultMessage="Where" />;

export const none = <FormattedMessage id="none" defaultMessage="None" />;

export const fullDatePlaceholder = date => (
  <FormattedMessage
    id="fullDatePlaceholder"
    defaultMessage="mm/dd/yyyy"
    description="full date placeholder"
  >
    {date}
  </FormattedMessage>
);

export const yearDatePlaceholder = date => (
  <FormattedMessage
    id="yearDatePlaceholder"
    defaultMessage="yyyy"
    description="year date placeholder"
  >
    {date}
  </FormattedMessage>
);
