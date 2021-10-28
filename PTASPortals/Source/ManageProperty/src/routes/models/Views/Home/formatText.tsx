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
    id="home_manage_title"
    defaultMessage="Manage your property"
  />
);

export const itemList1 = (
  <FormattedMessage
    id="home_update_your_mailing"
    defaultMessage="Update your mailing address"
  />
);

export const itemList2 = (
  <FormattedMessage
    id="home_get_emailed"
    defaultMessage="Get emailed valuation notices"
  />
);

export const itemList3 = (
  <FormattedMessage
    id="home_apply_for_tax"
    defaultMessage="Apply for tax exemption"
  />
);

export const itemList4 = (
  <FormattedMessage
    id="home_appeal_your_property"
    defaultMessage="Appeal your property value"
  />
);

export const itemList5 = (
  <FormattedMessage
    id="home_manage_your_business"
    defaultMessage="Manage your business property"
  />
);

export const signIn = (
  <FormattedMessage id="home_sign_in" defaultMessage="Sign in" />
);

export const indication = (
  <FormattedMessage
    id="home_indication"
    defaultMessage="After signing in, find your property and confirm ownership."
  />
);

export const titleNew = (
  <FormattedMessage id="home_new" defaultMessage="What’s new?" />
);

export const newAccount = (
  <FormattedMessage id="home_newAccount" defaultMessage="New account access" />
);

export const newAccountIndication = (
  <FormattedMessage
    id="home_newAccount_indication"
    defaultMessage="Easier and safer sign-in for property owners."
  />
);

export const OnlineFilingLinks = (
  <FormattedMessage
    id="home_online_filing_links"
    defaultMessage="Online filing links"
  />
);

export const seniorExemption = (
  <FormattedMessage
    id="home_senior_exemption_link"
    defaultMessage="Senior Exemption"
  />
);
export const homeImprovement = (
  <FormattedMessage id="home_improvement" defaultMessage="Home improvement" />
);
export const currentUse = (
  <FormattedMessage id="home_current_use" defaultMessage="Current use" />
);
export const destroyProperty = (
  <FormattedMessage
    id="home_destroy_property"
    defaultMessage="Destroyed property"
  />
);

export const nameHelperText = (
  <FormattedMessage
    id="home_name_helper_text"
    defaultMessage="Doesn’t affect owner name"
  />
);

export const address = (
  <FormattedMessage
    id="home_address"
    defaultMessage="Same as physical address"
  />
);

export const addressLabel = (
  <FormattedMessage id="home_address_label" defaultMessage="Address title" />
);

export const goPaperlessTitle = (
  <FormattedMessage
    id="home_go_paperless_title"
    defaultMessage="Get official property value notices delivered to your inbox."
  />
);

export const goPaperlessFormTitle = (
  <FormattedMessage
    id="home_go_paperless_form_title"
    defaultMessage="Enter the following from your value notice"
  />
);

export const accountNumberLabel = (
  <FormattedMessage
    id="home_account_number_label"
    defaultMessage="Account number"
  />
);

export const accountNumberHelper = (
  <FormattedMessage
    id="home_account_number_helper"
    defaultMessage="10 digits, dash optional"
  />
);

export const accountNumberNotValid = (
  <FormattedMessage
    id="home_account_number_not_valid"
    defaultMessage="Account number not valid"
  />
);

export const evnCodeLabel = (
  <FormattedMessage id="home_evn_code" defaultMessage="EVN code" />
);

export const evnCodeHelper = (
  <FormattedMessage id="home_characters" defaultMessage="6 digits" />
);

export const codeNotValid = (
  <FormattedMessage id="home_code_no_valid" defaultMessage="Code not valid" />
);

export const goPaperlessButton = (
  <FormattedMessage
    id="home_go_paperless_button"
    defaultMessage="Go paperless"
  />
);

export const goPaperlessDescription = (
  <FormattedMessage
    id="home_go_paperless_description"
    defaultMessage="This doesn’t affect other King County documents and communications."
  />
);

export const requestEvnCode = (
  <FormattedMessage
    id="home_request_evn_code"
    defaultMessage="Request EVN code"
  />
);

export const popupMessage = (
  <FormattedMessage
    id="home_popup_message"
    defaultMessage="You can request that information at {linkValue}"
    values={{
      linkValue: (
        <Link to="https://www.kingcounty.gov/depts/finance-business-operations/treasury/property-tax.aspx">
          King County Property Tax Payment Information.
        </Link>
      ),
    }}
  />
);

export const moreTitle = (
  <FormattedMessage
    id="home_more_title"
    defaultMessage="Manage and apply for tax exemptions"
  />
);

export const electronicProperty = (
  <FormattedMessage
    id="home_electronic_property"
    defaultMessage="Electronic property value notices will be delivered to:"
  />
);

export const turnOff = (
  <FormattedMessage id="home_turn_off" defaultMessage="Turn off paperless" />
);

// cards
export const apply = (
  <FormattedMessage id="home_apply" defaultMessage="Apply" />
);

export const view = <FormattedMessage id="home_view" defaultMessage="View" />;

export const manage = (
  <FormattedMessage id="home_manage" defaultMessage="Manage" />
);

// senior exemption
export const seniorExemptionDesc = (
  <FormattedMessage
    id="home_senior_exemption_des"
    defaultMessage="Based on your ownership, occupancy, age, and income"
  />
);

// home improvement
export const homeImprovementDes = (
  <FormattedMessage
    id="home_home_improvement_desc"
    defaultMessage="For single family residences adding new rooms, etc."
  />
);

// current use
export const currentUseDes = (
  <FormattedMessage
    id="home_land_being_used"
    defaultMessage="Land being used for farming, agriculture, and forestry"
  />
);

// Property value appeal
export const propertyValueAppeal = (
  <FormattedMessage
    id="home_property_value_appeal"
    defaultMessage="Property value appeal"
  />
);

export const propertyValueAppealDesc = (
  <FormattedMessage
    id="home_property_value_desc"
    defaultMessage="Request a new value with supporting evidence"
  />
);

// Business personal property
export const businessPersonalProperty = (
  <FormattedMessage
    id="home_business_personal_property"
    defaultMessage="Business personal property"
  />
);

export const businessPersonalPropertyDes = (
  <FormattedMessage
    id="home_business_personal_property_des"
    defaultMessage="Manage your listing info and current assets"
  />
);

// Destroyed property

export const destroyPropertyDesc = (
  <FormattedMessage
    id="home_destroyed_property_desc"
    defaultMessage="Request a reduced assessed value from damage or loss"
  />
);

export const removeDescParagraph1 = (
  <FormattedMessage
    id="home_remove_this_property"
    defaultMessage="Permanently removes this property from your online account."
  />
);

export const removeDescParagraph2 = (
  <FormattedMessage
    id="home_tab_remove"
    defaultMessage="If you need access to it again, use the information on your valuation notice to reconfirm your ownership."
  />
);

export const mailingAddress = (
  <FormattedMessage
    id="home_mailing_address"
    defaultMessage="Mailing address"
  />
);

export const goPaperless = (
  <FormattedMessage id="home_go_paperless" defaultMessage="Go paperless" />
);

export const findMyProperty = (
  <FormattedMessage
    id="home_find_my_property"
    defaultMessage="Find my property"
  />
);

export const locateMe = (
  <FormattedMessage id="home_locate_me" defaultMessage="Locate me" />
);

export const enterAnAddress = (
  <FormattedMessage
    id="home_enter_an_address"
    defaultMessage="Enter an address, parcel #, or commercial project"
  />
);
