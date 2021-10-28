// formatText.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';
import { Link } from 'react-router-dom';

// titles
export const businessPersonalProperty = (
  <FormattedMessage
    id="webinar_business_personal_property"
    defaultMessage="How to manage your business personal property listing"
  />
);

export const whenYouFinishEditing = (
  <FormattedMessage
    id="new_business_finish_editing"
    defaultMessage="When you finish editing "
  />
);

export const saveBusiness = (
  <FormattedMessage
    id="new_business_save_business"
    defaultMessage="Save business"
  />
);

export const newBusiness = (
  <FormattedMessage id="new_business" defaultMessage="New business" />
);

///
export const basicInfo = (
  <FormattedMessage id="new_business_basic_info" defaultMessage="Basic info" />
);

export const location = (
  <FormattedMessage id="new_business_location" defaultMessage="Location" />
);

export const contact = (
  <FormattedMessage id="new_business_contact" defaultMessage="Contact" />
);

export const attach = (
  <FormattedMessage id="new_business_attach" defaultMessage="Attach" />
);

export const assets = (
  <FormattedMessage id="new_business_assets" defaultMessage="Assets" />
);

export const exemptions = (
  <FormattedMessage id="new_business_exemptions" defaultMessage="Exemptions" />
);

export const supplies = (
  <FormattedMessage id="new_business_supplies" defaultMessage="Supplies" />
);

export const improvements = (
  <FormattedMessage
    id="new_business_improvements"
    defaultMessage="Improvements"
  />
);

// basic info
export const businessName = (
  <FormattedMessage
    id="new_business_business_name"
    defaultMessage="Business name"
  />
);

export const dateOpened = (
  <FormattedMessage
    id="new_business_date_opened"
    defaultMessage="Date opened"
  />
);

export const startupBusiness = (
  <FormattedMessage
    id="new_business_startup_business"
    defaultMessage="This is a startup business."
  />
);

export const purchasedOrRemovedThisBusiness = (
  <FormattedMessage
    id="new_business_purchased_or_removed"
    defaultMessage="It is new because I’ve purchased or moved this business."
  />
);

export const UBINumber = (
  <FormattedMessage id="new_business_ubi_number" defaultMessage="UBI number" />
);

export const NAICSNumber = (
  <FormattedMessage
    id="new_business_NAICS_number"
    defaultMessage="NAICS number"
  />
);

export const previousOwnerName = (
  <FormattedMessage
    id="new_business_previous_owner_name"
    defaultMessage="Previous owner name"
  />
);
export const previousBusinessName = (
  <FormattedMessage
    id="new_business_previous_business_name"
    defaultMessage="Previous business name"
  />
);
export const previousLocationAddress = (
  <FormattedMessage
    id="new_business_previous_location_address"
    defaultMessage="Previous location address"
  />
);

export const businessType = (
  <FormattedMessage
    id="new_business_business_type"
    defaultMessage="Business type"
  />
);

// location
export const addressTitle = (
  <FormattedMessage
    id="new_business_address_title"
    defaultMessage="Address title"
  />
);

export const addressRequired = (
  <FormattedMessage
    id="new_business_address_required"
    defaultMessage="Address (required)"
  />
);

export const addressCont = (
  <FormattedMessage
    id="new_business_address_cont"
    defaultMessage="Address (cont.)"
  />
);

export const stateOfIncorporation = (
  <FormattedMessage
    id="new_business_state_of_incorporation"
    defaultMessage="State of incorporation"
  />
);

// contact
export const ownerTaxpayer = (
  <FormattedMessage
    id="new_business_owner_taxpayer"
    defaultMessage="Owner/Taxpayer name"
  />
);
export const contactName = (
  <FormattedMessage
    id="new_business_contact_name"
    defaultMessage="Contact name"
  />
);
export const useOtherEmail = (
  <FormattedMessage
    id="new_business_user_other_email"
    defaultMessage="Use other email"
  />
);
export const useOtherPhone = (
  <FormattedMessage
    id="new_business_use_other_phone"
    defaultMessage="Use other phone"
  />
);
export const acceptsTextMessages = (
  <FormattedMessage
    id="new_business_accept_text_messages"
    defaultMessage="Accepts text messages"
  />
);
export const useOtherAddress = (
  <FormattedMessage
    id="new_business_use_other_address"
    defaultMessage="Use other address"
  />
);
export const auburnStore = (
  <FormattedMessage
    id="new_business_auburn_store"
    defaultMessage="Auburn store"
  />
);
export const howardRd = (
  <FormattedMessage
    id="new_business_howard_rd"
    defaultMessage="1801 Howard Rd"
  />
);
export const auburnWA = (
  <FormattedMessage
    id="new_business_auburn_WA"
    defaultMessage="Auburn, WA 98002"
  />
);

export const attention = (
  <FormattedMessage id="new_business_attention" defaultMessage="Attention" />
);

// attach
export const haveTheNextFew = (
  <FormattedMessage
    id="new_business_already_have_the_next_few"
    defaultMessage="Already have the next few steps in a spreadsheet?"
  />
);

export const importYourInfo = (
  <FormattedMessage
    id="new_business_import_your_info"
    defaultMessage="You can import your info into our {link} template and attach."
    values={{
      link: <Link to="/">eListing.xlsx</Link>,
    }}
  />
);

export const attachListingInfo = (
  <FormattedMessage
    id="new_business_attach_listing_info"
    defaultMessage="Attach listing file(s):"
  />
);

export const PTMSToExcel = (
  <FormattedMessage
    id="new_business_attention"
    defaultMessage="PTMS to Excel"
  />
);

export const instructions = (
  <FormattedMessage
    id="new_business_instructions"
    defaultMessage="Instructions"
  />
);

// assets
export const newAssets = (
  <FormattedMessage id="new_business_new_assets" defaultMessage="New asset" />
);

export const newItem = (
  <FormattedMessage id="new_business_new_item" defaultMessage="New item" />
);

export const sortBy = (
  <FormattedMessage id="new_business_sort_by" defaultMessage="Sort by" />
);

export const year = (
  <FormattedMessage id="new_business_year" defaultMessage="Year" />
);

export const cost = (
  <FormattedMessage id="new_business_cost" defaultMessage="Cost" />
);

export const category = (
  <FormattedMessage id="new_business_category" defaultMessage="Category" />
);

export const reason = (
  <FormattedMessage id="new_business_reason" defaultMessage="Reason" />
);

export const addOneOrMoreAssets = (
  <FormattedMessage
    id="new_business_add_one_or_more_assets"
    defaultMessage="You’ll need to add one or more assets to complete this step."
  />
);

// exemptions

export const headOfFamily = (
  <FormattedMessage
    id="new_business_head_of_family"
    defaultMessage="Head of family exemption"
  />
);

export const rcwExemptions = (
  <FormattedMessage
    id="new_business_rwc_exemptions"
    defaultMessage="RCW 84.36.110"
  />
);

export const exemptionsApplies = (
  <FormattedMessage
    id="new_business_exemptions_applies"
    defaultMessage="This $15,000 exemption applies only to sole proprietors registered with the Washington State Department of Revenue."
  />
);

export const mustBeAppliedForAnnually = (
  <FormattedMessage
    id="new_business_must_be_applied_for_annually"
    defaultMessage="Must be applied for annually."
  />
);

export const onlyOneExemption = (
  <FormattedMessage
    id="new_business_only_one_exemption"
    defaultMessage="Only one exemption per year is allowed."
  />
);

export const exemptionIsOnlyAllowedForOneProperty = (
  <FormattedMessage
    id="new_business_exemptions_is_only_allowed_for_one_property"
    defaultMessage="Exemption is only allowed for one property per year."
  />
);

export const followingQualification = (
  <FormattedMessage
    id="new_business_following_qualification"
    defaultMessage="You must meet one of the following qualifications:"
  />
);

export const livingWithASpouseOrDependent = (
  <FormattedMessage
    id="new_business_living_with_a_spouse_or_dependent"
    defaultMessage="Living with a spouse or dependent"
  />
);

export const residingContinually = (
  <FormattedMessage
    id="new_business_resident_continually"
    defaultMessage="A US citizen over the age of 65 residing continually in Washington state for 10 years"
  />
);

export const survivingSpouse = (
  <FormattedMessage
    id="new_business_surviving_spouse"
    defaultMessage="Surviving spouse, not married"
  />
);

export const qualifyForThisExemption = (
  <FormattedMessage
    id="new_business_qualify_for_this_exemption"
    defaultMessage="I qualify for this exemption."
  />
);

export const farmMachinery = (
  <FormattedMessage
    id="new_business_farmMachinery"
    defaultMessage="Farm machinery and equipment exemption"
  />
);

export const rcwExemptions2 = (
  <FormattedMessage
    id="new_business_rcw_exemption_2"
    defaultMessage="RCW 84.36.630"
  />
);

export const exemptsQualifyingFarmingMachinery = (
  <FormattedMessage
    id="new_business_exempts_qualifying_faming_machinery"
    defaultMessage="Exempts qualifying farming machinery and equipment from state property tax. It declares that all machinery and equipment owned by a farmer that is personal property is exempt from property taxes levied for any state purpose if the items are used exclusively in growing and producing agricultural products during the calendar year for which the claim for exemption is made."
  />
);

export const likeToApply = (
  <FormattedMessage
    id="new_business_like_to_apply"
    defaultMessage="I’d like to apply for this exemption."
  />
);

// supplies
export const suppliesMaterials = (
  <FormattedMessage
    id="new_business_supplies_materials_other_items"
    defaultMessage="Supplies, materials, and other expensed items"
  />
);

export const provideTheCostOfItem = (
  <FormattedMessage
    id="new_business_provide_the_cost_of_item"
    defaultMessage="Provide the cost of items that don’t become ingredients or components of articles for sale."
  />
);

export const examples = (
  <FormattedMessage id="new_business_examples" defaultMessage="Examples:" />
);

export const officeShopJanitorial = (
  <FormattedMessage
    id="new_business_office_shop_janitorial"
    defaultMessage="Office, shop, janitorial, or medical supplies"
  />
);

export const brochuresPromotionalItems = (
  <FormattedMessage
    id="new_business_brochures_promotional_items"
    defaultMessage="Brochures and promotional items"
  />
);

export const fuelSparePart = (
  <FormattedMessage
    id="new_business_fuel_spare_part"
    defaultMessage="Fuel, spare parts, and expensed small tools"
  />
);

export const researchCompanies = (
  <FormattedMessage
    id="new_business_research_companies"
    defaultMessage="For research companies, this would include all raw materials and supplies used in your research. Software, furniture, fixtures, and other items you may have expensed but have a life of more than one year are to be reported under the business property section of the return."
  />
);

export const yearlyAverage = (
  <FormattedMessage
    id="new_business_yearly-average"
    defaultMessage="Yearly average"
  />
);

export const monthlyAverage = (
  <FormattedMessage
    id="new_business_monthly_average"
    defaultMessage="Monthly average"
  />
);

// improvements
export const recordAnyLeaseHold = (
  <FormattedMessage
    id="new_business_reason"
    defaultMessage="Record any leasehold or tenant improvements made to leased space for the purpose of conducting business. They can be immobile in nature but cannot be real property."
  />
);

export const personalPropertyTaxpayer = (
  <FormattedMessage
    id="new_business_reason"
    defaultMessage="The personal property taxpayer owns the building where the leasehold improvements are located."
  />
);

export const type = (
  <FormattedMessage id="new_business_type" defaultMessage="Type" />
);

export const modified = (
  <FormattedMessage id="new_business_modified" defaultMessage="Modified" />
);

export const uploadDragReject = (
  <FormattedMessage
    id="new_business_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);
