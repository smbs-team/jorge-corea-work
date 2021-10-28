// formatText.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const refrigeratorCooler = (
  <FormattedMessage
    id="business_form_refrigerator"
    defaultMessage="Refrigerator/cooler/ice M&E"
  />
);

export const acquired = (
  <FormattedMessage id="business_form_acquired" defaultMessage="Acquired" />
);

export const cost = (
  <FormattedMessage id="business_form_cost" defaultMessage="Cost" />
);

export const selectAReason = (
  <FormattedMessage
    id="business_form_refrigerator"
    defaultMessage="Select a reason"
  />
);

export const removeAsset = (
  <FormattedMessage
    id="business_form_remove_asset"
    defaultMessage="Remove Asset"
  />
);

export const removeAssetInfo = (
  <FormattedMessage
    id="business_form_remove_asset_info"
    defaultMessage="Permanently remove this asset information."
  />
);

export const includeCostForMaking = (
  <FormattedMessage
    id="business_form_include_cost_for_making"
    defaultMessage="Include costs for making the asset operational, such as freight, installation, and engineering, but not sales tax."
  />
);
