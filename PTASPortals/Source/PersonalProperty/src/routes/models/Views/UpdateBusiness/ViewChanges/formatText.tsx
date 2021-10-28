// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const headerText = (
  <FormattedMessage
    id="view_changes_header"
    defaultMessage="Updated information is highlighted in the following summary. When finished:"
  />
);

export const headerButtonText = (
  <FormattedMessage id="view_changes_button" defaultMessage="Save updates" />
);

export const ownerName = (
  <FormattedMessage
    id="view_changes_owner_name"
    defaultMessage="Owner/Taxpayer name"
  />
);

export const businessName = (
  <FormattedMessage
    id="view_changes_business_name"
    defaultMessage="Business name"
  />
);

export const ubiNumber = (
  <FormattedMessage id="view_changes_ubi_number" defaultMessage="UBI number" />
);

export const naicsNumber = (
  <FormattedMessage
    id="view_changes_naics_number"
    defaultMessage="NAICS number"
  />
);

export const businessType = (
  <FormattedMessage
    id="view_changes_business_type"
    defaultMessage="Business type"
  />
);

export const stateOfIncorporation = (
  <FormattedMessage
    id="view_changes_state_incorporation"
    defaultMessage="State of incorporation"
  />
);

export const contactName = (
  <FormattedMessage
    id="view_changes_contact_name"
    defaultMessage="Contact name"
  />
);

export const attention = (
  <FormattedMessage id="view_changes_attention" defaultMessage="Attention" />
);

export const email = (
  <FormattedMessage id="view_changes_email" defaultMessage="Email" />
);

export const phone = (
  <FormattedMessage id="view_changes_phone" defaultMessage="Phone" />
);

export const mailingAddress = (
  <FormattedMessage
    id="view_changes_mailing_address"
    defaultMessage="Mailing address"
  />
);

export const locationAddress = (
  <FormattedMessage
    id="view_changes_location_address"
    defaultMessage="Location address"
  />
);

export const city = (
  <FormattedMessage id="view_changes_city" defaultMessage="City" />
);

export const state = (
  <FormattedMessage id="view_changes_state" defaultMessage="State" />
);

export const zip = (
  <FormattedMessage id="view_changes_zip" defaultMessage="Zip" />
);

export const assets = (
  <FormattedMessage id="view_changes_assets" defaultMessage="Assets" />
);
