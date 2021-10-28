//-----------------------------------------------------------------------
// <copyright file="FormatTexts.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import { FormattedMessage } from 'react-intl';
import React from 'react';

export const cropperWhatShouldObscure = (
  <FormattedMessage
    id="cropperWhatShouldObscure"
    defaultMessage="    
    Add privacy boxes to cover up Social Security and account numbers before you continue."
    values={{ br: <br /> }}
  />
);

export const nextPage = (
  <FormattedMessage id="cropperJS_nextPage" defaultMessage="Next page" />
);
export const prevPage = (
  <FormattedMessage id="cropperJS_prevPage" defaultMessage="Previous page" />
);

export const continueButton = (
  <FormattedMessage id="cropperJS_continue" defaultMessage="Continue" />
);

export const undo = (
  <FormattedMessage id="cropperJS_undo" defaultMessage="Undo" />
);

export const privacyMarker = (
  <FormattedMessage id="cropperJS_privacyMarker" defaultMessage="Privacy box" />
);

export const privacyBox = (
  <FormattedMessage
    id="cropperJS_privacyBox"
    defaultMessage="Add privacy box"
  />
);

export const crop = (
  <FormattedMessage id="cropperJS_crop" defaultMessage="Crop" />
);

export const rotate = (
  <FormattedMessage id="cropperJS_rotate" defaultMessage="Rotate" />
);

export const rotateLeft = (
  <FormattedMessage id="cropperJS_rotateLeft" defaultMessage="Rotate Left" />
);

export const rotateRight = (
  <FormattedMessage id="cropperJS_rotateRight" defaultMessage="Rotate Right" />
);

export const zoomIn = (
  <FormattedMessage id="cropperJS_zoomIn" defaultMessage="Zoom in" />
);

export const zoomOut = (
  <FormattedMessage id="cropperJS_zoomOut" defaultMessage="Zoom out" />
);
