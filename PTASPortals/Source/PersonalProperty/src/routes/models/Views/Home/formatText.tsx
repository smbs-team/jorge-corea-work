// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';
import { Link } from 'react-router-dom';

export const title = (
  <FormattedMessage
    id="home_title"
    defaultMessage="Business personal property"
  />
);

export const signIn = (
  <FormattedMessage id="home_sign_in" defaultMessage="Sign in" />
);

export const description = (
  <FormattedMessage
    id="home_description"
    defaultMessage="Manage your account and taxable assets."
  />
);

export const whatsNew = (
  <FormattedMessage id="home_whats_new" defaultMessage="What’s new?" />
);

export const newAccount = (
  <FormattedMessage id="home_new_account" defaultMessage="New account access" />
);

export const newAccountParagraph = (
  <FormattedMessage
    id="home_new_account_p"
    defaultMessage="Easier and safer sign-in for owners, staff, and finance agents."
  />
);

export const easierEntry = (
  <FormattedMessage
    id="home_easier_entry"
    defaultMessage="Easier entry forms"
  />
);

export const easierEntryParagraph = (
  <FormattedMessage
    id="home_easier_entry_p"
    defaultMessage="Spend less time keeping your info up to date."
  />
);

export const unlistedBusiness = (
  <FormattedMessage
    id="home_unlisted_business"
    defaultMessage="Have an {br} unlisted business?"
    values={{
      br: <br />,
    }}
  />
);

export const unlistedBusinessParagraph = (
  <FormattedMessage
    id="home_easier_entry_p"
    defaultMessage="You can register a {link} or with an offline form."
    values={{
      link: <Link to="/new-business">new business online</Link>,
    }}
  />
);

//#region Businesses

export const businesses = (
  <FormattedMessage id="home_businesses" defaultMessage="Businesses" />
);

export const businessesAddBusiness = (
  <FormattedMessage
    id="home_businesses_add_business"
    defaultMessage="Add a business"
  />
);

export const businessesManageButton = (
  <FormattedMessage
    id="home_businesses_manage_button"
    defaultMessage="Manage"
  />
);

export const businessesWhoManageButton = (
  <FormattedMessage
    id="home_businesses_who_manage_button"
    defaultMessage="Who can manage"
  />
);

export const personalPropertyAccount = (acctNo: string): React.ReactNode => (
  <FormattedMessage
    id="home_personal_property_account"
    defaultMessage="Personal property account #{acctNo}"
    values={{
      acctNo: acctNo,
    }}
  />
);

//#endregion

//#region Add business

export const addBusinessText1 = (
  <FormattedMessage
    id="home_add_business_text1"
    defaultMessage="Listed businesses are added using the following from the upper-left portion of your printed listing form."
  />
);

export const addBusinessText2 = (
  <FormattedMessage
    id="home_add_business_text2"
    defaultMessage="If your business is new and unlisted, you’ll need to make a new one for us to certify."
  />
);

export const addBusinessAcctNo = (
  <FormattedMessage
    id="home_add_business_acct_no"
    defaultMessage="Account number"
  />
);

export const addBusinessAcctNoDesc = (
  <FormattedMessage
    id="home_add_business_acct_no_desc"
    defaultMessage="8 digits"
  />
);

export const addBusinessWrongAcc = (
  <FormattedMessage
    id="home_addBusinessWrongAcc"
    defaultMessage="Wrong account"
  />
);

export const addBusinessWrongCode = (
  <FormattedMessage
    id="home_addBusinessWrongCode"
    defaultMessage="Wrong code"
  />
);

export const addBusinessAccessCode = (
  <FormattedMessage
    id="home_add_business_access_code"
    defaultMessage="Access code"
  />
);

export const addBusinessAccessCodeDesc = (
  <FormattedMessage
    id="home_add_business_access_code_desc"
    defaultMessage="Letters or numbers"
  />
);

export const addBusinessButtonExisting = (
  <FormattedMessage
    id="home_add_business_button_existing"
    defaultMessage="Add this business"
  />
);

export const addBusinessButtonNew = (
  <FormattedMessage
    id="home_add_business_button_new"
    defaultMessage="Create a new business"
  />
);

//#endregion

export const addBusinessFirst = (
  <FormattedMessage
    id="home_add_business_first"
    defaultMessage="You can add people and access for people once you have created some business."
  />
);

export const addPeopleAccess = (
  <FormattedMessage
    id="home_add_people_access"
    defaultMessage="You can add access for people to view or edit business listing info."
  />
);

export const people = (
  <FormattedMessage id="home_people" defaultMessage="People" />
);

export const peopleAddPerson = (
  <FormattedMessage id="home_people_add_person" defaultMessage="Add a person" />
);

export const peopleEdit = (
  <FormattedMessage id="home_people_edit" defaultMessage="Edit" />
);

export const peopleHasAccess = (
  <FormattedMessage
    id="home_people_has_access"
    defaultMessage="Has access to"
  />
);

export const lastSeenDate = (date: string): React.ReactNode => (
  <FormattedMessage
    id="home_last_seen_date"
    defaultMessage="Last seen #{date}"
    values={{
      date: date,
    }}
  />
);

//#region KC staff

export const staffTitle = (
  <FormattedMessage
    id="home_staff_title"
    defaultMessage="Personal property support"
  />
);

export const signInTaxpayer = (
  <FormattedMessage
    id="home_staff_sign_in_taxpayer"
    defaultMessage="Sign in as taxpayer:"
  />
);

export const findTaxpayer = (
  <FormattedMessage
    id="home_staff_find_taxpayer"
    defaultMessage="Find taxpayer"
  />
);

export const enterSearchParam = (
  <FormattedMessage
    id="home_staff_enter_search_param"
    defaultMessage="Enter a name, email, or account number."
  />
);

export const uploadDragReject = (
  <FormattedMessage
    id="home_staff_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);

//#endregion
