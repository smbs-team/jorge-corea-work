// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage id="permits_title" defaultMessage="Import permits" />
);

export const tabName = (
  <FormattedMessage id="profile_tab_name" defaultMessage="Name" />
);

export const firstName = (
  <FormattedMessage
    id="profile_first_name"
    defaultMessage="First (given) name"
  />
);

export const middleName = (
  <FormattedMessage id="profile_middle_name" defaultMessage="Middle name" />
);

export const lastName = (
  <FormattedMessage
    id="profile_last_name"
    defaultMessage="Last (family) name"
  />
);

export const tabEmail = (
  <FormattedMessage id="profile_tab_email" defaultMessage="Email" />
);

export const tabAddress = (
  <FormattedMessage id="profile_tab_address" defaultMessage="Address" />
);

export const tabPhone = (
  <FormattedMessage id="profile_tab_phone" defaultMessage="Phone" />
);

export const buttonContinue = (
  <FormattedMessage id="profile_button_continue" defaultMessage="Continue" />
);

export const buttonDone = (
  <FormattedMessage id="profile_button_done" defaultMessage="Done" />
);

export const uploadDragReject = (
  <FormattedMessage
    id="permits_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);

export const uploadTitle = (
  <FormattedMessage
    id="permits_upload_title"
    defaultMessage="Import permits sheet"
  />
);

export const uploadDesc = (
  <FormattedMessage
    id="permits_upload_desc"
    defaultMessage="Download the current"
  />
);

export const instruction = (
  <FormattedMessage id="permits_instruction" defaultMessage="Instructions" />
);

export const instructionTitle = (
  <FormattedMessage
    id="permits_instruction_title"
    defaultMessage="Importing permits"
  />
);

export const useCurrentTemplate = (
  <FormattedMessage
    id="permits_use_current_template"
    defaultMessage="Use current template"
  />
);

export const selectOrDrag = (
  <FormattedMessage
    id="permits_select_or_drag"
    defaultMessage="Select or drag in the file"
  />
);

export const reviewForErrors = (
  <FormattedMessage
    id="permits_review_for_errors"
    defaultMessage="Review for errors"
  />
);

export const savePermits = (
  <FormattedMessage id="permits_save" defaultMessage="Save permits" />
);

export const savePermitsError = (
  <FormattedMessage
    id="permits_save_error"
    defaultMessage="Save permits (errors)"
  />
);

export const downloadForCorrection = (
  <FormattedMessage
    id="download_for_correction"
    defaultMessage="Download for correction"
  />
);

export const download = (
  <FormattedMessage
    id="download"
    defaultMessage="Download"
  />
);

export const reviewImport = (
  <FormattedMessage id="permits_review_import" defaultMessage="Review import" />
);

export const reviewDescription = (
  <FormattedMessage
    id="permits_review_desc"
    defaultMessage="If you see any errors, correct them in your file and re-import."
  />
);

export const permitsTemplate = (
  <FormattedMessage
    id="permits_template_xlsx"
    defaultMessage="KC-import-permits-template.xlsx"
  />
);
