// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const help = (
  <FormattedMessage id="home_help" defaultMessage="Need some help?" />
);

export const instruction = (
  <FormattedMessage id="home_instruction" defaultMessage="Instructions" />
);

export const continueText = (
  <FormattedMessage id="home_continue" defaultMessage="Continue" />
);

export const no = <FormattedMessage id="general_no" defaultMessage="No" />;

export const yes = <FormattedMessage id="general_yes" defaultMessage="Yes" />;

export const save = (
  <FormattedMessage id="general_save" defaultMessage="Save" />
);

export const editProfile = (
  <FormattedMessage id="general_edit_profile" defaultMessage="Edit profile" />
);

export const signOut = (
  <FormattedMessage id="general_sign_out" defaultMessage="Sign out" />
);

export const signIn = (
  <FormattedMessage id="general_sign_in" defaultMessage="Sign in" />
);

export const returnToSignIn = (
  <FormattedMessage
    id="general_return_sign_in"
    defaultMessage="Return to sign in"
  />
);

export const notAuthorized = (
  <FormattedMessage
    id="general_not_authorized"
    defaultMessage="Sorry, you’re not authorized to access this portal."
  />
);

export const popupBlockedMsg = (
  <FormattedMessage
    id="general_popup_blocked"
    defaultMessage="Popups are blocked for this site. Please allow popups to continue."
  />
);

export const userInformationLoading = (
  <FormattedMessage
    id="general_user_info_loading"
    defaultMessage="The user's information is loading..."
  />
);
