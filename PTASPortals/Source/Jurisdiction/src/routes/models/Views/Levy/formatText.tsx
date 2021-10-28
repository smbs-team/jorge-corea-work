// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="levy_title"
    defaultMessage="Import levy request forms"
  />
);

export const uploadDragReject = (
  <FormattedMessage
    id="levy_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);

export const uploadTitle = (
  <FormattedMessage
    id="levy_upload_title"
    defaultMessage="Import levy request forms"
  />
);

export const instructions = (
  <FormattedMessage id="levy_instruction" defaultMessage="Instructions" />
);

export const instructionTitle = (
  <FormattedMessage
    id="levy_instruction_title"
    defaultMessage="Importing forms"
  />
);

export const instructionFillForms = (
  <FormattedMessage
    id="levy_instruction_fill_forms"
    defaultMessage="Fill out the forms"
  />
);

export const instructionSelectFile = (
  <FormattedMessage
    id="levy_instruction_select_file"
    defaultMessage="Select or drag in the file"
  />
);

export const instructionReviewErrors = (
  <FormattedMessage
    id="levy_instruction_review_errors"
    defaultMessage="Review for errors"
  />
);

export const instructionSaveForm = (
  <FormattedMessage
    id="levy_instruction_save_form"
    defaultMessage="Save form"
  />
);

export const saveForm = (
  <FormattedMessage id="levy_save_form" defaultMessage="Save form" />
);

export const reviewImport = (
  <FormattedMessage id="levy_review_import" defaultMessage="Review import" />
);

export const reviewDescription = (
  <FormattedMessage
    id="levy_review_desc"
    defaultMessage="If you find any errors, correct them in your file and re-import."
  />
);
