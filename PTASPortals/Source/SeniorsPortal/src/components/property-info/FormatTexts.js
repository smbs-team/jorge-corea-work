import { FormattedMessage } from 'react-intl';
import React from 'react';

export const percentageValue = (
  <FormattedMessage
    id="property_percentageValue"
    defaultMessage="Value is percentage."
  />
);

export const otherPropertyInCurrentTaxYear = year => {
  return (
    <FormattedMessage
      id="property_otherPropertyIn2020"
      defaultMessage="I have sold other property in the year {year}."
      values={{
        year: year - 1,
      }}
    />
  );
};

export const receivedExemptionBefore = (
  <FormattedMessage
    id="property_receivedExemptionBefore"
    defaultMessage="I have received an exemption before."
  />
);

export const rentPortionResidence = (
  <FormattedMessage
    id="property_rentPortionResidence"
    defaultMessage="I rent our a portion of my residence."
  />
);

export const provideAmount = (
  <FormattedMessage
    id="property_provideAmount"
    defaultMessage="Provide # of square feet."
  />
);

export const providePercentage = (
  <FormattedMessage
    id="property_providePercentage"
    defaultMessage="Provide % of your home."
  />
);

export const residenceForBusiness = (
  <FormattedMessage
    id="property_residenceForBusiness"
    defaultMessage="I use my residence for business use."
  />
);

export const soldFormerResidence = (
  <FormattedMessage
    id="property_soldFormerResidence"
    defaultMessage="I have sold my former residence."
  />
);

export const propertyPurchaseDate = (
  <FormattedMessage
    id="property_propertyPurchaseDate"
    defaultMessage="Date property purchased"
  />
);

export const ownerDoc = (
  <FormattedMessage
    id="property_ownerDoc"
    defaultMessage="Lease or Life Estate"
  />
);

export const owner = (
  <FormattedMessage
    id="property_owner"
    defaultMessage="I am the owner, hold a lease for life, or hold a life estate for this residence."
  />
);

export const primaryResidence = (
  <FormattedMessage
    id="property_primaryResidence"
    defaultMessage="This property has been my primary residence."
    description="Primary residence label"
  />
);

export const liveNineMonthsOfYear = year => {
  return (
    <FormattedMessage
      id="property_liveNineMonthsOfYear"
      defaultMessage="I lived at this property for at least {months} months in {year}."
      description="nine months residence label"
      values={{
        months:
          year === process.env.REACT_APP_REQUIRED_MONTHS_YEAR_LIMIT ? 9 : 6,
        year: year - 1,
      }}
    />
  );
};

export const householdChanges = (
  <FormattedMessage
    id="property_householdChanges"
    defaultMessage="My household has experienced major life changes (such as a marriage, divorce, or death) since I acquired the property."
  />
);

export const otherProperties = (
  <FormattedMessage
    id="property_otherProperties"
    defaultMessage="I own other properties."
    description="Household other properties label"
  />
);

export const enterFullAddress = (
  <FormattedMessage
    id="property_enterFullAddress"
    defaultMessage="Enter full address"
    description="Enter full address label"
  />
);

export const addAnotherProperty = (
  <FormattedMessage
    id="property_addAnotherProperty"
    defaultMessage="Add another property"
    description="Add another property label"
  />
);

export const property = (
  <FormattedMessage
    id="property"
    defaultMessage="1. Property"
    description="1. Property"
  />
);

export const mailingAddressf = (
  <FormattedMessage
    id="mailing_address"
    defaultMessage="2. Mailing address"
    description="2. Mailing address"
  />
);

export const mailingAddressFullName = (
  <FormattedMessage
    id="mailing_address_full_name"
    defaultMessage="Full name (if different)"
    description="Description for the full name on mailing address input field."
  />
);

export const mailingAddressChangeOfficialAddress = (
  <FormattedMessage
    id="mailing_address_change_official_address"
    defaultMessage="Change my official mailing address"
    description="Description of the change my official address switch."
  />
);

export const residence = (
  <FormattedMessage
    id="residence"
    defaultMessage="3. Residence"
    description="3. Residence"
  />
);

export const additionalInfo = (
  <FormattedMessage
    id="additional_info"
    defaultMessage="4. Additional info"
    description="4. Additional info"
  />
);

export const otherLiveProperty = (
  <FormattedMessage
    id="property_othersLiveProperty"
    defaultMessage="I share ownership of this property with someone other than my spouse or registered domestic partner."
    description="Others live in property label"
  />
);

export const addAnotherOccupant = (
  <FormattedMessage
    id="property_addAnotherOccupant"
    defaultMessage="Add another occupant"
    description="Add another occupant label"
  />
);

export const treasurerPhoneNumber = (
  <FormattedMessage
    id="property_treasurerPhoneNumber"
    defaultMessage="Treasurer's phone number"
    description="Treasurer's phone number label"
  />
);

export const transferringExemption = (
  <FormattedMessage
    id="property_transferringExemption"
    defaultMessage="I'm transferring my exemption from a previous residence in Washington state."
    description="Transferring exemption label"
  />
);

export const firstDatePrimaryResidence = (
  <FormattedMessage
    id="property_firstDatePrimaryResidence"
    defaultMessage="First year as primary residence"
  />
);

export const differentAddress = (
  <FormattedMessage
    id="property_differentAddress"
    defaultMessage="I want to add a mailing address just for this application."
    description="Different address"
  />
);

export const differentCheckAddress = (
  <FormattedMessage
    id="property_differentCheckAddress"
    defaultMessage="If I qualify for a refund, send the check to a different address."
    description="Refund address"
  />
);

export const DatePlaceHolder = props => {
  return (
    <FormattedMessage
      id="property_datePlaceholder"
      defaultMessage="mm/dd/yyyy"
      description="date acquired property placeholder"
    >
      {props.children}
    </FormattedMessage>
  );
};

export const dateAcquired = (
  <FormattedMessage
    id="property_dateAcquiredProperty"
    defaultMessage="Date I acquired this property"
    description="date acquired property label"
  />
);

export const propertyFormYearsToApply = (
  <FormattedMessage
    id="propertyInfo_formYearToApply"
    defaultMessage="Based on your age ability, and property, the earliest year you might qualify for is "
  />
);

export const propertyRecommendedYearToApply = (
  <FormattedMessage
    id="propertyInfo_recommendedYearToApply"
    defaultMessage=". We recommend you apply for that year"
  />
);

export const propertyId = (
  <FormattedMessage
    id="property_yourProperty"
    defaultMessage="Your property"
    description="your property label"
  />
);

export const addressExample = (
  <FormattedMessage
    id="property_addressExample"
    defaultMessage="123 Street Name... or 000000-0000"
    description="Address Example label"
  />
);

export const enterAddress = (
  <FormattedMessage
    id="property_enterAddress"
    defaultMessage="Enter your address, parcel number, or tax account number. Then select it from the generated list."
    description="Description for search element to look for parcel, address or tax account number."
  />
);

export const propertyAddress = (
  <FormattedMessage
    id="property_address"
    defaultMessage="Property address"
    description="Property address label"
  />
);

export const propertyPurpose = (
  <FormattedMessage
    id="property_purpose"
    defaultMessage="Property purpose"
    description="Property purpose label"
  />
);

export const firstName = (
  <FormattedMessage
    id="property_firstGivenName"
    defaultMessage="First (given) name"
    description="first name label"
  />
);

export const middleName = (
  <FormattedMessage
    id="property_middleName"
    defaultMessage="Middle name"
    description="middle name label"
  />
);

export const lastName = (
  <FormattedMessage
    id="property_lastName"
    defaultMessage="Last (family) name"
    description="last name label"
  />
);

export const suffix = (
  <FormattedMessage
    id="property_suffix"
    defaultMessage="Suffix"
    description="Suffix label"
  />
);

export const relationship = (
  <FormattedMessage
    id="property_relationship"
    defaultMessage="Relationship"
    description="Relationship label"
  />
);

export const nameOfCoop = (
  <FormattedMessage
    id="property_nameCoOp"
    defaultMessage="Name of co-op"
    description="Name of co-op label"
  />
);

export const treasurer = (
  <FormattedMessage
    id="property_treasurer"
    defaultMessage="Treasurer"
    description="Treasurer label"
  />
);

export const numberOfSharesYouOwn = (
  <FormattedMessage
    id="property_numSharesYouOwn"
    defaultMessage="Number of shares you own"
    description="Number of shares you own label"
  />
);

export const totalCoopShares = (
  <FormattedMessage
    id="property_totalCoOpShares"
    defaultMessage="Total co-op shares"
    description="Total co-op shares label"
  />
);

export const countyPreviousResidence = (
  <FormattedMessage
    id="property_countyPreviousResidence"
    defaultMessage="County of previous residence"
    description="County of previous residence label"
  />
);

export const mailingAddressAssigned = (
  <FormattedMessage
    id="property_mailingAddressAssigned"
    defaultMessage="Mailing address assigned to parcel number"
    description="Mailing address"
  />
);

export const country = (
  <FormattedMessage
    id="property_country"
    defaultMessage="Country"
    description="Country label"
  />
);

export const mailingAddress = (
  <FormattedMessage
    id="property_mailingAddress"
    defaultMessage="Mailing address"
    description="Mailing address label"
  />
);

export const city = (
  <FormattedMessage
    id="property_city"
    defaultMessage="City"
    description="City label"
  />
);

export const state = (
  <FormattedMessage
    id="property_state"
    defaultMessage="State"
    description="State label"
  />
);

export const zip = (
  <FormattedMessage
    id="property_zip"
    defaultMessage="Zip"
    description="Zip label"
  />
);

export const postalCode = (
  <FormattedMessage
    id="property_postalCode"
    defaultMessage="Postal code"
    description="Postal code label"
  />
);

export const town = (
  <FormattedMessage
    id="property_town"
    defaultMessage="Town"
    description="Town label"
  />
);

export const provinceAddress = (
  <FormattedMessage
    id="property_provinceAddress"
    defaultMessage="Province address"
    description="Province address label"
  />
);

export const propertyId_ht = (
  <FormattedMessage
    id="property_propertyIdHt"
    defaultMessage="Enter the address or parcel number of the property, which must be your primary residence and owned or co-owned by you."
  />
);

export const property_cardSide_primaryResidence = (
  <FormattedMessage
    id="property_cardSide_content"
    defaultMessage="Must be your owned or co-owned primary residence."
  />
);

export const property_cardSide_condo = (
  <FormattedMessage
    id="property_cardSide_condo"
    defaultMessage='{value} owners must enter their 10-digit parcel number: "000000-0000"'
    values={{ value: <strong>Condo</strong> }}
  />
);

export const property_cardSide_mobileHome = (
  <FormattedMessage
    id="property_cardSide_mobileHome"
    defaultMessage='{value} owners on leased land must enter their 8-digit tax account number: "0000-0000"'
    values={{ value: <strong>Mobile home</strong> }}
  />
);

export const property_cardSide_addressFormat = (
  <FormattedMessage
    id="property_cardSide_addressFormat"
    defaultMessage="Address format: '123 Street...'"
  />
);

export const property_cardSide_parcelFormat = (
  <FormattedMessage
    id="property_cardSide_parcelFormat"
    defaultMessage="Parcel format: '000000-0000'"
  />
);

export const acquiredDate_ht = (
  <FormattedMessage
    id="property_acquiredDateHt"
    defaultMessage="Enter the date that the title was transferred to you in a recorded transaction."
  />
);

export const infoOnCountyRecord = (
  <FormattedMessage
    id="property_infoOnCountyRecord"
    defaultMessage="Info on county record:"
  />
);

export const proofOwnership = (
  <FormattedMessage
    id="property_proofOwnership"
    defaultMessage="Proof of ownership"
    description="Property info UploadFile label"
  />
);

export const proofChange = (
  <FormattedMessage
    id="property_proofChange"
    defaultMessage="Proof of change"
    description="Property info UploadFile label"
  />
);

export const shareCertificate = (
  <FormattedMessage
    id="property_shareCertificate"
    defaultMessage="Share certificate"
    description="Property info UploadFile label"
  />
);

export const attachCertificate = (
  <FormattedMessage
    id="property_attachCertificate"
    defaultMessage="Attach your share certificate for this property."
    description="Property info UploadFile side text"
  />
);

export const parcelNumber = (
  <FormattedMessage id="property_parcelNumber" defaultMessage="Parcel number" />
);

export const otherParcelNumber = (
  <FormattedMessage
    id="property_otherParcelNumber"
    defaultMessage="Parcel number of previous property"
  />
);

export const isDelinquent = (
  <FormattedMessage
    id="property_isDelinquent"
    defaultMessage="My property taxes are delinquent."
  />
);

export const unsureIfTaxesAreDelinquent = (
  <FormattedMessage
    id="property_unsureIfTaxesAreDelinquent"
    defaultMessage="Unsure if your taxes are delinquent?"
  />
);

export const checkPropertyStatus = (
  <FormattedMessage
    id="property_checkPropertyStatus"
    defaultMessage="Check property status"
  />
);

export const notTheRightAddress = (
  <FormattedMessage
    id="property_notTheRightAddress"
    defaultMessage="Not the right address?"
  />
);

export const chageProperty = (
  <FormattedMessage
    id="property_chageProperty"
    defaultMessage="Re-enter property address"
  />
);

const ownership1 = (
  <FormattedMessage
    id="property_proofOwnershipHelpText1"
    defaultMessage="If you purchased the property before 1995, upload a copy of the deed or other document that verifies the date of purchase."
  />
);

const ownership2 = (
  <FormattedMessage
    id="property_proofOwnershipHelpText2"
    defaultMessage="If your property is in the name of a trust, upload a copy of the entire trust or a completed declaration of trust, which you can download from the King County website <a>https://www.kingcounty.gov/depts/assessor/Forms.aspx</a>."
    values={{
      a: (...chunks) => (
        <a
          href="https://www.kingcounty.gov/depts/assessor/Forms.aspx"
          target="_blank"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

const ownership3 = (
  <FormattedMessage
    id="property_proofOwnershipHelpText3"
    defaultMessage="If you have a lease for life retaining ownership, upload a copy of the recorded lease."
  />
);

const ownership4 = (
  <FormattedMessage
    id="property_proofOwnershipHelpText4"
    defaultMessage="If you inherited the property or acquired it as part of a divorce settlement or court order, upload documentation of the acquisition."
  />
);

const ownership5 = (
  <FormattedMessage
    id="property_proofOwnershipHelpText5"
    defaultMessage="To find documents dated on or after August 1, 1991, use the King County Official Records Search <a>https://www.kingcounty.gov/depts/assessor/Forms.aspx</a>. To find documents recorded prior to August 1, 1991, contact the King County Archives <b>https://www.kingcounty.gov/depts/records-licensing/archives/research-guides/recordings.aspx</b>."
    values={{
      a: (...chunks) => (
        <a
          href="https://www.kingcounty.gov/depts/assessor/Forms.aspx"
          target="_blank"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
      b: (...chunks) => (
        <a
          href="https://www.kingcounty.gov/depts/records-licensing/archives/research-guides/recordings.aspx"
          target="_blank"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

export const helpTextOwnership = (
  <React.Fragment>
    <p>{ownership1}</p>
    <p>{ownership2}</p>
    <p>{ownership3}</p>
    <p>{ownership4}</p>
    <p>{ownership5}</p>
  </React.Fragment>
);

export const helpTextChange = (
  <FormattedMessage
    id="property_proofChangeHelpText"
    defaultMessage="Before you begin, copy all required documents, such as legal documents pertaining to a marriage, divorce, death, or change in domestic partnership. Obscure the Social Security and account numbers, and then upload the redacted copies."
  />
);

export const helpTextShare = (
  <FormattedMessage
    id="property_shareCertificateHelpText"
    defaultMessage="Before you begin, copy your share certificate. Obscure the Social Security numbers and account numbers, and then upload the redacted copies."
  />
);

export const documentsToComplete = (
  <FormattedMessage
    id="property_documentsToComplete"
    defaultMessage="You might need the following documents to complete this section:"
  />
);

export const proofResidenceOwnership = (
  <FormattedMessage
    id="property_proofResidenceOwnership"
    defaultMessage="Proof of residence ownership (such as a deed)"
  />
);

export const proofMajorLifeChange = (
  <FormattedMessage
    id="property_proofMajorLifeChange"
    defaultMessage="Proof of major life change (such as a marriage, divorce, or death certificate)"
  />
);

export const coopShares = (
  <FormattedMessage
    id="property_coOpShares"
    defaultMessage="Co-op shares certificate"
  />
);

export const savingLabel = (
  <FormattedMessage id="property_savingLabel" defaultMessage="Saving..." />
);
export const continueLabel = (
  <FormattedMessage id="property_continueLabel" defaultMessage="Continue" />
);
export const moreInfoLabel = (
  <FormattedMessage
    id="property_moreInfoRequested"
    defaultMessage="More info requested"
  />
);

export const removeLabel = (
  <FormattedMessage id="property_removeLabel" defaultMessage="Remove" />
);

export const removeOccupantLabel = (
  <FormattedMessage
    id="property_removeOccupantLabel"
    defaultMessage="Remove occupant"
  />
);

export const removePropertyLabel = (
  <FormattedMessage
    id="property_removePropertyLabel"
    defaultMessage="Remove property"
  />
);

export const basedOnInformation = (
  <FormattedMessage
    id="property_basedOnInformation"
    defaultMessage="Based on the information you entered so far:"
  />
);

// export const youMightQualifySingleYear = year => {
//   return (
//     <FormattedMessage
//       id="youMightQualifySingleYear"
//       defaultMessage="You might qualify for {year}."
//       values={{ year: year }}
//     />
//   );
// };

export const youMightQualifyMultipleYears = (years, year) => {
  return (
    <FormattedMessage
      id="property_youMightQualifyMultipleYears"
      defaultMessage="You might qualify for {years} and {year} ."
      values={{ year: year, years: years }}
    />
  );
};

// export const youMightNotQualifySingleYear = year => {
//   return (
//     <FormattedMessage
//       id="youMightNotQualifySingleYear"
//       defaultMessage="You might not qualify for {year}."
//       values={{ year: year }}
//     />
//   );
// };

// export const youMightNotQualifyMultipleYears = (years, year) => {
//   return (
//     <FormattedMessage
//       id="youMightQualifyMultipleYears"
//       defaultMessage="You might not qualify for {years} or {year} ."
//       values={{ year: year, years: years }}
//     />
//   );
//};

export const whyNot = (
  <FormattedMessage id="property_whyNot" defaultMessage="Why not?" />
);

export const whyNot_ht = (
  <FormattedMessage
    id="property_whyNotHt"
    defaultMessage="To qualify, you must have lived in the residence and have been 61, a veteran with a service-connected disability, or retired because of a disability during the tax year you're applying for. You might also qualify if your deceased spouse or state-registered domestic partner had an exemption at the time of death, and you were 57 or older during the tax year you're applying for."
  />
);

export const returnToSummary = (
  <FormattedMessage
    id="property_returnToSummary"
    defaultMessage="Return to summary"
  />
);

export const required = (
  <FormattedMessage id="property_required" defaultMessage="(required)" />
);

export const mustOccupy9months = year => (
  <FormattedMessage
    id="property_mustOccupy9months"
    defaultMessage="You must own and occupy this property as your primary residence for at least {months} months of the year you are applying for."
    values={{
      months: year === process.env.REACT_APP_REQUIRED_MONTHS_YEAR_LIMIT ? 9 : 6,
    }}
  />
);

export const mustOwn9months = year => (
  <FormattedMessage
    id="property_mustOwn9months"
    defaultMessage="To qualify, you must own and occupy this property as your primary residence for at least {months} months of the year."
    values={{
      months: year === process.env.REACT_APP_REQUIRED_MONTHS_YEAR_LIMIT ? 9 : 6,
    }}
  />
);

export const youllneedsomeofthefollowing = (
  <FormattedMessage
    id="youllneedsomeofthefollowing"
    defaultMessage="To continue, you need to enter your information:"
  />
);

export const trustDocument = (
  <FormattedMessage id="trustDocument" defaultMessage="Trust Document" />
);

export const attachTrustDocument = (
  <FormattedMessage
    id="attachTrustDocument"
    defaultMessage="Attach your complete trust document for this property."
  />
);

export const hideSocialSecurity = (
  <FormattedMessage
    id="myInfo_hideSocialSecurity"
    defaultMessage="Hide Social Security and account numbers, either with a marker or tape on paper copies or with the document editor after attaching."
  />
);

export const selectPhotoOfDocument = (
  <FormattedMessage
    id="myInfo_selectPhotoOfDocument"
    defaultMessage="Select a photo or digital copy of your document. JPG or PDF formats are preferred."
  />
);

export const singleFamily = (
  <FormattedMessage id="property_singleFamily" defaultMessage="Single family" />
);

export const housingCoOp = (
  <FormattedMessage id="property_housingCoOp" defaultMessage="Housing Co-op" />
);

export const singleUnitMultiFamDwell = (
  <FormattedMessage
    id="property_singleUnitMultiFamDwell"
    defaultMessage="Single unit in Multi-Family Dwelling (duplex or condo)"
  />
);

export const mobileHome = (
  <FormattedMessage id="property_mobileHome" defaultMessage="Mobile Home" />
);

export const mobileHomeYear = (
  <FormattedMessage id="property_mobileHomeYear" defaultMessage="Year" />
);

export const mobileHomeMake = (
  <FormattedMessage id="property_mobileHomeMake" defaultMessage="Make" />
);

export const mobileHomeModel = (
  <FormattedMessage id="property_mobileHomeModel" defaultMessage="Model" />
);

export const mobileHomeSelectInfo = (
  <FormattedMessage
    id="property_mobileHomeSelectInfo"
    defaultMessage='Mobile home owners on leased land must enter their 8-digit account number: "0000-0000"'
  />
);

export const singleUnitSelectInfo = (
  <FormattedMessage
    id="property_singleUnitSelectInfo"
    defaultMessage="Condo owners must enter their 10-digit parcel number rather than their property address."
  />
);
