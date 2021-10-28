// Instruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const mainTitle = (
  <FormattedMessage
    id="instruction_main_title"
    defaultMessage="How to manage your business personal property listing"
  />
);

// business
export const business = (
  <FormattedMessage id="instruction_business" defaultMessage="Businesses" />
);

// add a business, report change, or remove it (list)

export const AddABusinessTitle = (
  <FormattedMessage
    id="instruction_add_a_business_title"
    defaultMessage="Add a business, report changes, or remove it"
  />
);

export const addExisting = (
  <FormattedMessage
    id="instruction_add_extension"
    defaultMessage="Add an existing business"
  />
);

export const addExistingDesc = (
  <FormattedMessage
    id="instruction_add_extension_desc"
    defaultMessage="When you add a business to your web account, you need your account number and access code to verify that you’re authorized to maintain this business listing info."
  />
);

export const addANewUnlistedBusiness = (
  <FormattedMessage
    id="instruction_add_extension"
    defaultMessage="Add a new, unlisted business "
  />
);

export const addANewUnlistedBusinessDesc = (
  <FormattedMessage
    id="instruction_add_extension_desc"
    defaultMessage="You can create a new business listing without signing in. However, we recommend that you sign in so that you can reuse your contact info."
  />
);

export const reportYourBusiness = (
  <FormattedMessage
    id="instruction_report_business"
    defaultMessage="Report your business as moved, sold, or closed"
  />
);

export const reportYourBusinessDesc = (
  <FormattedMessage
    id="instruction_report_business_desc"
    defaultMessage="We'll update your tax account and billing information accordingly."
  />
);

export const removeABusiness = (
  <FormattedMessage
    id="instruction_remove_a_business"
    defaultMessage="Remove a business"
  />
);

export const removeABusinessDesc = (
  <FormattedMessage
    id="instruction_remove_a_business"
    defaultMessage="If you’re no longer associated with a business, remove it from your online account."
  />
);

// updating business listing info
export const updatingBusinessTitle = (
  <FormattedMessage
    id="instruction_updating_business_title"
    defaultMessage="Updating business listing info"
  />
);

export const updatingBusinessTitleDesc = (
  <FormattedMessage
    id="instruction_updating_business_desc"
    defaultMessage="You're required to update your business personal property listing every year, even if you don't receive a reminder. [insert link to RCW here]"
  />
);

export const updateViaOnline = (
  <FormattedMessage
    id="instruction_update_via_online"
    defaultMessage="Update via online form"
  />
);

// list update via online from
export const updateViaOnlineInstructionItem1 = (
  <FormattedMessage
    id="instruction_update_via_online_instruction_1"
    defaultMessage="You can see and update all the current information associated with your business listing."
  />
);

export const updateViaOnlineInstructionItem2 = (
  <FormattedMessage
    id="instruction_update_via_online_instruction_2"
    defaultMessage="This option is best if you’re making minor changes or if you’re not certain which details have changed since last year."
  />
);

export const updateViaOnlineInstructionItem3 = (
  <FormattedMessage
    id="instruction_update_via_online_instruction_3"
    defaultMessage="This option is the easiest way to add assets, because you can look up the category codes by name, category, examples from a category, or industry."
  />
);

// update via attached spreadsheet
export const updateViaAttached = (
  <FormattedMessage
    id="instruction_update_via_attached"
    defaultMessage="Update via attached spreadsheet"
  />
);

// update via attached spreadsheet list
export const updateViaAttachedListItem1 = (
  <FormattedMessage
    id="instruction_update_via_attached_list_1"
    defaultMessage="This option is best if you’re maintaining your info in software such as Property tax made simple (PTMS)"
  />
);

export const updateViaAttachedListItem2 = (
  <FormattedMessage
    id="instruction_update_via_attached_list_2"
    defaultMessage="To update your listing in one step, you can attach your export file."
  />
);

// signing in
export const signingIn = (
  <FormattedMessage id="instruction_signing_in" defaultMessage="Signing in" />
);

export const signingInDes = (
  <FormattedMessage
    id="instruction_signing_in_des"
    defaultMessage="You have a few options for signing in, whether you’re new or returning to a King County web account."
  />
);

export const signWithEmail = (
  <FormattedMessage
    id="instruction_signing_with_email"
    defaultMessage="Sign in with email"
  />
);

export const signWithEmailListItem1 = (
  <FormattedMessage
    id="instruction_signing_with_email_list_1"
    defaultMessage="With a password. If you’re new or you’ve migrated from a legacy account, we’ll ask you to create a password."
  />
);

export const signWithEmailListItem2 = (
  <FormattedMessage
    id="instruction_signing_with_email_list_2"
    defaultMessage="With an email we send you each time you sign in. With this option, you don’t need to create or remember a password. It’s also useful if you’ve forgotten your password."
  />
);

export const signWithAnAccountProvider = (
  <FormattedMessage
    id="instruction_signing_with_an_account_provider"
    defaultMessage="Sign in with an account provider"
  />
);

export const signWithAnAccountProviderListItem1 = (
  <FormattedMessage
    id="instruction_signing_with_an_account_provider_item_1"
    defaultMessage="This is an easy option if you’re already signed in with Google, Apple, or Microsoft using the email address you’d like to use with King County."
  />
);

export const signWithAnAccountProviderListItem2 = (
  <FormattedMessage
    id="instruction_signing_with_an_account_provider_item_2"
    defaultMessage="Your provider email address should be the one you want to use for King County communications"
  />
);

export const signWithAnAccountProviderListItem3 = (
  <FormattedMessage
    id="instruction_signing_with_an_account_provider_item_3"
    defaultMessage="That provider handles additional security options, depending on your settings."
  />
);

export const signingInInstruction = (
  <FormattedMessage
    id="instruction_signing_instruction"
    defaultMessage="Regardless of your preferred option, be sure to use your correct email address."
  />
);

// people
export const people = (
  <FormattedMessage id="instruction_people" defaultMessage="People" />
);

export const peopleDesc = (
  <FormattedMessage
    id="instruction_people"
    defaultMessage="You can delegate specific people to help manage or oversee your business listing info. For each of your verified businesses, you can assign people view or edit access."
  />
);

export const addAPerson = (
  <FormattedMessage
    id="instruction_add_a_person"
    defaultMessage="Add a person"
  />
);

export const addAPersonListItem1 = (
  <FormattedMessage
    id="instruction_add_a_person"
    defaultMessage="When you add a person to your account, they receive an email invitation to sign in."
  />
);

export const editAPerson = (
  <FormattedMessage
    id="instruction_edit_a_person"
    defaultMessage="edit a person"
  />
);

export const editAPersonListItem1 = (
  <FormattedMessage
    id="instruction_edit_a_person_item_1"
    defaultMessage="Edit when you need to change someone’s access to a business or to remove them from your online account."
  />
);
export const editAPersonListItem2 = (
  <FormattedMessage
    id="instruction_edit_a_person_item_2"
    defaultMessage="If you remove a person from your online account, they lose access to all of your businesses that they haven’t verified."
  />
);
