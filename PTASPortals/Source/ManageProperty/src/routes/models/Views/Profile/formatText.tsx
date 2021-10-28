// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage id="profile_title" defaultMessage="My profile" />
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

export const email = (
  <FormattedMessage id="profile_email" defaultMessage="Email" />
);

export const phoneNumber = (
  <FormattedMessage id="profile_phone_number" defaultMessage="Phone" />
);

export const suffix = (
  <FormattedMessage id="profile_suffix" defaultMessage="Suffix" />
);

export const tabEmail = (
  <FormattedMessage id="profile_tab_email" defaultMessage="Email" />
);

export const emailExists = (
  <FormattedMessage
    id="profile_email_exists"
    defaultMessage="The specified email already exists."
  />
);

export const emailLabel = (
  <FormattedMessage id="profile_email_label" defaultMessage="Email" />
);

export const addEmail = (
  <FormattedMessage id="profile_add_email" defaultMessage="Add another email" />
);

export const otherEmail = (
  <FormattedMessage id="profile_other_email" defaultMessage="Use other email" />
);

export const manageEmails = (
  <FormattedMessage id="profile_manage_emails" defaultMessage="Manage emails" />
);

export const newEmail = (
  <FormattedMessage id="profile_new_email" defaultMessage="New email" />
);

export const modalLabelEmail = (
  <FormattedMessage id="profile_modal_label_email" defaultMessage="Email" />
);

export const modalRemoveButtonEmail = (
  <FormattedMessage
    id="profile_modal_remove_button_email"
    defaultMessage="Remove email"
  />
);

export const modalRemoveAlertEmail = (
  <FormattedMessage
    id="profile_modal_remove_alert_email"
    defaultMessage="Remove email"
  />
);

export const modalRemoveAlertButtonEmail = (
  <FormattedMessage
    id="profile_modal_remove_alert_button_email"
    defaultMessage="Remove"
  />
);

export const tabAddress = (
  <FormattedMessage id="profile_tab_address" defaultMessage="Address" />
);

export const otherAddress = (
  <FormattedMessage
    id="profile_other_address"
    defaultMessage="Use other address"
  />
);

export const manageAddresses = (
  <FormattedMessage
    id="profile_manage_addresses"
    defaultMessage="Manage addresses"
  />
);

export const newAddress = (
  <FormattedMessage id="profile_new_address" defaultMessage="New address" />
);

export const addressTitle = (
  <FormattedMessage
    id="profile_modal_address_title"
    defaultMessage="Address title"
  />
);

export const addressLine1 = (
  <FormattedMessage id="profile_modal_address_line1" defaultMessage="Address" />
);

export const addressLine2 = (
  <FormattedMessage
    id="profile_modal_address_line2"
    defaultMessage="Address (cont)"
  />
);

export const addressCountry = (
  <FormattedMessage
    id="profile_modal_address_country"
    defaultMessage="Country"
  />
);

export const addressCity = (
  <FormattedMessage id="profile_address_city" defaultMessage="City" />
);

export const addressState = (
  <FormattedMessage id="profile_modal_address_state" defaultMessage="State" />
);

export const addressZip = (
  <FormattedMessage id="profile_modal_address_zip" defaultMessage="Zip" />
);

export const modalRemoveButtonAddress = (
  <FormattedMessage
    id="profile_modal_remove_button_address"
    defaultMessage="Remove address"
  />
);

export const modalRemoveAlertAddress = (
  <FormattedMessage
    id="profile_modal_remove_alert_address"
    defaultMessage="Remove address"
  />
);

export const modalRemoveAlertButtonAddress = (
  <FormattedMessage
    id="profile_modal_remove_alert_button_address"
    defaultMessage="Remove"
  />
);

export const tabPhone = (
  <FormattedMessage id="profile_tab_phone" defaultMessage="Phone" />
);

export const otherPhone = (
  <FormattedMessage id="profile_other_phone" defaultMessage="Use other phone" />
);

export const phoneAcceptsMessages = (
  <FormattedMessage
    id="profile_phone_accepts_messages"
    defaultMessage="Accepts text messages"
  />
);

export const phoneAcceptsNoMessages = (
  <FormattedMessage
    id="profile_phone_accepts_no_messages"
    defaultMessage="Does not accept text messages"
  />
);

export const managePhones = (
  <FormattedMessage id="profile_manage_phones" defaultMessage="Manage phones" />
);

export const newPhone = (
  <FormattedMessage id="profile_new_phone" defaultMessage="New phone" />
);

export const modalPhoneTitle = (
  <FormattedMessage id="profile_modal_phone_title" defaultMessage="Phone" />
);

export const modalPhonePlaceholder = {
  id: 'profile_modal_phone_placeholder',
  defaultMessage: 'Phone',
};

export const modalPhoneTabCell = (
  <FormattedMessage id="profile_modal_phone_tab_cell" defaultMessage="Cell" />
);

export const modalPhoneTabWork = (
  <FormattedMessage id="profile_modal_phone_tab_work" defaultMessage="Work" />
);

export const modalPhoneTabHome = (
  <FormattedMessage id="profile_modal_phone_tab_home" defaultMessage="Home" />
);

export const modalPhoneTabTollFree = (
  <FormattedMessage
    id="profile_modal_phone_tab_toll_free"
    defaultMessage="Toll free"
  />
);

export const modalPhoneAcceptsMessages = (
  <FormattedMessage
    id="profile_modal_phone_accepts_messages"
    defaultMessage="Accepts text messages"
  />
);

export const modalRemoveButtonPhone = (
  <FormattedMessage
    id="profile_modal_remove_button_phone"
    defaultMessage="Remove phone"
  />
);

export const modalRemoveAlertPhone = (
  <FormattedMessage
    id="profile_modal_remove_alert_phone"
    defaultMessage="Remove phone"
  />
);

export const modalRemoveAlertButtonPhone = (
  <FormattedMessage
    id="profile_modal_remove_alert_button_phone"
    defaultMessage="Remove"
  />
);

export const buttonContinue = (
  <FormattedMessage id="profile_button_continue" defaultMessage="Continue" />
);

export const buttonDone = (
  <FormattedMessage id="profile_button_done" defaultMessage="Done" />
);

export const requiredField = (
  <FormattedMessage
    id="profile_required_field"
    defaultMessage="Field is required"
  />
);

export const requiredEmail = (
  <FormattedMessage
    id="profile_required_email"
    defaultMessage="A valid email is required"
  />
);

export const requiredPhone = (
  <FormattedMessage
    id="profile_required_phone"
    defaultMessage="A valid phone number is required"
  />
);

export const saveContactError = (
  <FormattedMessage
    id="profile_save_contact_error"
    defaultMessage="Error on save portal contact"
  />
);

export const saveEmailError = (
  <FormattedMessage
    id="profile_save_email_error"
    defaultMessage="Error on save portal email"
  />
);

export const updatePrimaryEmailError = (
  <FormattedMessage
    id="profile_update_primary_email_error"
    defaultMessage="Error on update primary email"
  />
);

export const saveAddressError = (
  <FormattedMessage
    id="profile_save_address_error"
    defaultMessage="Error on save portal address"
  />
);

export const savePhoneError = (
  <FormattedMessage
    id="profile_save_phone_error"
    defaultMessage="Error on save portal phone"
  />
);

export const deleteEmailError = (
  <FormattedMessage
    id="profile_delete_email_error"
    defaultMessage="Error on delete portal email"
  />
);

export const deleteAddressError = (
  <FormattedMessage
    id="profile_delete_address_error"
    defaultMessage="Error on delete portal address"
  />
);

export const deletePhoneError = (
  <FormattedMessage
    id="profile_delete_phone_error"
    defaultMessage="Error on delete portal phone"
  />
);
