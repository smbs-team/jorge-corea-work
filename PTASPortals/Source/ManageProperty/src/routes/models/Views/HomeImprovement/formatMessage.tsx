// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="home_improvement_title"
    defaultMessage="Home improvement exemption"
  />
);

export const appliedExemption = (date: string): JSX.Element => (
  <FormattedMessage
    id="home_improvement_applied_exemption"
    defaultMessage="Applied {date}"
    values={{ date: date }}
  />
);

export const tabConstruction = (
  <FormattedMessage
    id="home_improvement_tab_construction"
    defaultMessage="Construction"
  />
);

export const tabPermit = (
  <FormattedMessage id="home_improvement_tab_permit" defaultMessage="Permit" />
);

export const tabSign = (
  <FormattedMessage id="home_improvement_tab_sign" defaultMessage="Sign" />
);

export const constructionStartDate = (
  <FormattedMessage
    id="home_improvement_construction_start_date"
    defaultMessage="Start date"
  />
);

export const constructionEndDate = (
  <FormattedMessage
    id="home_improvement_construction_end_date"
    defaultMessage="End date"
  />
);

export const constructionCostEstimate = (
  <FormattedMessage
    id="home_improvement_construction_cost_estimate"
    defaultMessage="Cost estimate"
  />
);

export const constructionImprovementDescription = (
  <FormattedMessage
    id="home_improvement_construction_improvement_description"
    defaultMessage="Improvement description"
  />
);

export const permitSelect = (
  <FormattedMessage
    id="home_improvement_permit_select"
    defaultMessage="Select your permit, if you have one."
  />
);

export const permitsFound = (
  <FormattedMessage
    id="home_improvement_permits_found"
    defaultMessage="Permits found for this property"
  />
);

export const permitNotListed = (
  <FormattedMessage
    id="home_improvement_permit_not_listed"
    defaultMessage="My permit isn't listed here"
  />
);

export const permitEnterInfo = (
  <FormattedMessage
    id="home_improvement_permit_enter_info"
    defaultMessage="If you have a permit, enter this info."
  />
);

export const permitNumber = (
  <FormattedMessage
    id="home_improvement_permit_number"
    defaultMessage="Permit number"
  />
);

export const permitIssuedBy = (
  <FormattedMessage
    id="home_improvement_permit_issued_by"
    defaultMessage="Permit issued by"
  />
);

export const permitIssuerName = (
  <FormattedMessage
    id="home_improvement_permit_issuer_name"
    defaultMessage="King County or name of city"
  />
);

export const permitDateIssued = (
  <FormattedMessage
    id="home_improvement_permit_date_issued"
    defaultMessage="Date issued"
  />
);

export const signDeclaration1 = (
  <FormattedMessage
    id="home_improvement_sign_declaration1"
    defaultMessage="I hereby certify and declare under penalty of perjury that I‘m the taxpayer or authorized agent of the taxpayer and that the foregoing information is true and complete to the best of my knowledge."
  />
);

export const signDeclaration2 = (
  <FormattedMessage
    id="home_improvement_sign_declaration2"
    defaultMessage="I further certify that I haven’t applied for and haven’t been granted a home improvement exemption on this property within the last 5 years."
  />
);

export const signSignature = (
  <FormattedMessage
    id="home_improvement_sign_signature"
    defaultMessage="Signature"
  />
);

export const signEmail = (
  <FormattedMessage id="home_improvement_sign_email" defaultMessage="Email" />
);

export const signPhone = (
  <FormattedMessage id="home_improvement_sign_phone" defaultMessage="Phone" />
);

export const buttonContinue = (
  <FormattedMessage
    id="home_improvement_button_continue"
    defaultMessage="Continue"
  />
);

export const buttonSignFinalize = (
  <FormattedMessage
    id="home_improvement_button_sign_finalize"
    defaultMessage="Sign and finalize"
  />
);

export const indication = (
  <FormattedMessage
    id="home_improvement_indication"
    defaultMessage="If your property is being improved and you think it’s eligible for an exemption, find your property and apply now."
  />
);

export const help = (
  <FormattedMessage
    id="home_improvement_help"
    defaultMessage="Need some help?"
  />
);

export const instruction = (
  <FormattedMessage
    id="home_improvement_instruction"
    defaultMessage="Instructions"
  />
);

export const findMyProperty = (
  <FormattedMessage
    id="home_improvement_find_my_property"
    defaultMessage="Find my property"
  />
);

export const enterAddress = (
  <FormattedMessage
    id="home_improvement_enter_address"
    defaultMessage="Enter an address, parcel #, or commercial project"
  />
);

export const locateMe = (
  <FormattedMessage
    id="home_improvement_locate_me"
    defaultMessage="Locate me"
  />
);

export const attachPhotos = (
  <FormattedMessage
    id="home_improvement_attach_photos"
    defaultMessage="Attach bid estimate or photos (optional):"
  />
);

export const uploadDragReject = (
  <FormattedMessage
    id="home_improvement_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);

export const saveApplicationError = (
  <FormattedMessage
    id="home_improvement_save_application_error"
    defaultMessage="Error saving home improvement application."
  />
);

export const savePermitError = (
  <FormattedMessage
    id="home_improvement_save_permit_error"
    defaultMessage="Error saving permit."
  />
);

export const fieldRequired = (
  <FormattedMessage
    id="home_improvement_field_required"
    defaultMessage="Field required."
  />
);

export const requiredEmail = (
  <FormattedMessage
    id="home_improvement_required_email"
    defaultMessage="A valid email is required."
  />
);

export const requiredPhone = (
  <FormattedMessage
    id="home_improvement_required_phone"
    defaultMessage="A valid phone number is required"
  />
);

export const activeApplicationByAnotherContact = (
  <FormattedMessage
    id="home_improvement_active_application_by_another_contact"
    defaultMessage="Another contact already has an active application for this parcel"
  />
);

export const activeApplication = (
  <FormattedMessage
    id="home_improvement_active_application"
    defaultMessage="There is already an active application for this parcel"
  />
);

export const exemption5Years = (
  <FormattedMessage
    id="home_improvement_exemption_5_years"
    defaultMessage="There is an exemption with 5 or fewer years of approval for this parcel"
  />
);