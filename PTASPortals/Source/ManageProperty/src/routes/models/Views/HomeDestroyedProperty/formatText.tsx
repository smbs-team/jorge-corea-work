// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="home_destroyed_property_title"
    defaultMessage="Destroyed property"
  />
);

export const description = (
  <FormattedMessage
    id="home_destroyed_property_description"
    defaultMessage="If your property has been destroyed and it’s eligible for a market value reduction, find your property and apply now."
  />
);

export const destruction = (
  <FormattedMessage
    id="home_destroyed_property_construction"
    defaultMessage="Destruction"
  />
);

export const property = (
  <FormattedMessage id="home_destroyed_property" defaultMessage="Property" />
);

export const sign = (
  <FormattedMessage id="home_destroyed_property_sign" defaultMessage="Sign" />
);

export const datePickerLabel = (
  <FormattedMessage
    id="home_destroyed_property_date_picker"
    defaultMessage="Date of destruction"
  />
);

export const datePickerLabelHelperText = (
  <FormattedMessage
    id="home_destroyed_property_date_picker_helper_text"
    defaultMessage="Within 3 years"
  />
);

export const fire = (
  <FormattedMessage id="home_destroyed_property_fire" defaultMessage="Fire" />
);

export const fireDesc = (
  <FormattedMessage
    id="home_destroyed_property_fire_desc"
    defaultMessage="(Required: Attach proof like photos, report, insurance)"
  />
);

export const naturalDisaster = (
  <FormattedMessage
    id="home_destroyed_property_natural_disaster"
    defaultMessage="Natural disaster"
  />
);

export const naturalDisasterDesc = (
  <FormattedMessage
    id="home_destroyed_property_natural_disaster_desc"
    defaultMessage="(Required: Attach proof like photos, report, insurance)"
  />
);

export const voluntaryDemolition = (
  <FormattedMessage
    id="home_destroyed_property_voluntary_demolition"
    defaultMessage="Voluntary demolition"
  />
);

export const other = (
  <FormattedMessage id="home_destroyed_property_other" defaultMessage="Other" />
);

export const uploadDragReject = (
  <FormattedMessage
    id="home_destroyed_property_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);

export const destroyedTitle = (
  <FormattedMessage
    id="home_destroyed_property_title"
    defaultMessage="Property destroyed as a result of"
  />
);

export const textAreaPlaceholder = {
  id: 'home_destroyed_textarea_placeholder',
  defaultMessage: 'Description of destroyed property',
};

export const textAreaHelperText = (
  <FormattedMessage
    id="home_destroyed_property_textarea_helperText"
    defaultMessage="Home, building, garage, land, etc."
  />
);

export const repairLabel = (
  <FormattedMessage
    id="home_destroyed_property_repair_label"
    defaultMessage="Repair is planned and/or"
  />
);

export const repairLabelDesc = (
  <FormattedMessage
    id="home_destroyed_property_repair_label_des"
    defaultMessage="permitted(optional)"
  />
);

export const propertyStartDate = (
  <FormattedMessage
    id="home_destroyed_property_start_date"
    defaultMessage="Start date"
  />
);

export const propertyEndDate = (
  <FormattedMessage
    id="home_destroyed_property_end_date"
    defaultMessage="End date"
  />
);

export const permitIssuedLabel = (
  <FormattedMessage
    id="home_destroyed_property_permit_issued"
    defaultMessage="Permit issued by"
  />
);

export const permitIssuedHelperText = (
  <FormattedMessage
    id="home_destroyed_property_permit_issued_helper_text"
    defaultMessage="King County or name of city"
  />
);

export const estimate = (
  <FormattedMessage
    id="home_destroyed_property_estimate"
    defaultMessage="(Estimate)"
  />
);

export const uploadTitle = (
  <FormattedMessage
    id="home_destroyed_upload_title"
    defaultMessage="Attach additional documents"
  />
);

/**
 * Your signature
 */
export const yourSignature = (
  <FormattedMessage
    id="home_destroyed_property_signature"
    defaultMessage="Your signature"
  />
);

/**
 * To sign, enter your full name
 */
export const toSignEnterFullName = {
  id: 'home_destroyed_property_sign_enter_full_name',
  defaultMessage: 'To sign, enter your full name',
};

/**
 * Enter a valid date
 */
export const enterAValidDate = (
  <FormattedMessage
    id="home_destroyed_property_valid_date"
    defaultMessage="Enter a valid date"
  />
);

export const fieldRequired = (
  <FormattedMessage
    id="home_destroyed_property_field_required"
    defaultMessage="Field required."
  />
);

export const selectAnOption = (
  <FormattedMessage
    id="home_des_prop_selecta_an_option"
    defaultMessage="Select an option or fill in the other field"
  />
);

export const appliedExemption = (date: string): JSX.Element => (
  <FormattedMessage
    id="destroyed_property_applied"
    defaultMessage="Applied {date}"
    values={{ date: date }}
  />
);

export const pendingMoreInfoExemption = (date: string): JSX.Element => (
  <FormattedMessage
    id="destroyed_property_pending_more_info"
    defaultMessage="Pending more info {date}"
    values={{ date: date }}
  />
);
