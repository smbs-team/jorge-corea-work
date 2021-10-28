import { FormattedMessage } from 'react-intl';
import React from 'react';

export const signApplication = (
  <FormattedMessage
    id="summary_signApplication"
    defaultMessage="Add signature"
  />
);
export const addApplication = (
  <FormattedMessage
    id="summary_addApplication"
    defaultMessage="Add signature"
  />
);

export const finishApplication = (
  <FormattedMessage
    id="summary_finishApplication"
    defaultMessage="Finish application"
  />
);

export const proofOfAge = (
  <FormattedMessage id="summary_proofOfAge" defaultMessage="Proof of age" />
);

export const proofOfDisability = (
  <FormattedMessage
    id="summary_proofOfDisability"
    defaultMessage="Proof of disability"
  />
);

// Commented because of the Change Request to remove the witnesses.
// export const dialogHeaderText1 = (
//   <FormattedMessage
//     id="summary_dialogHeaderText1"
//     defaultMessage="Witnesses can't be a co-owner, spouse, or registered domestic partner. Any exemption granted through willfully providing erroneous information shall be subject to correct tax assessed for the last three (3) years, plus 100% penalty, (RCW 84.40.130). I declare, under the penalties of perjury, that all of the foregoing statements are true.
//     "
//     values={{
//       b: (...chunks) => <b>{chunks}</b>,

//       br: <br />,
//     }}
//   />
// );

// Removed content after feedback on review for 2021 changes.
//export const dialogHeaderText1 = (
//  <FormattedMessage
//    id="summary_dialogHeaderText1"
//    defaultMessage="Any exemption granted through willfully providing erroneous information shall be subject to correct tax assessed for the last three (3) years, plus 100% penalty, (RCW 84.40.130). I declare, under the penalties of perjury, that all of the foregoing statements are true."
//    values={{
//      b: (...chunks) => <b>{chunks}</b>,
//
//      br: <br />,
//    }}
//  />
//);

export const dialogWitnessText = (
  <FormattedMessage
    id="summary_dialogWitnessText"
    defaultMessage="Witnesses can't be a co-owner, spouse, or registered domestic partner."
  />
);

export const dialogFooterText = (
  <FormattedMessage
    id="summary_dialogFooterText"
    defaultMessage="You won't be able to edit this application after signing."
  />
);

export const spousePartnerInfo = (
  <FormattedMessage
    id="summary_spousePartnerInfo"
    defaultMessage="Spouse or registered domestic partner info"
  />
);

export const veteran = (
  <FormattedMessage id="summary_veteran" defaultMessage="Veteran" />
);

export const disabledVeteran = (
  <FormattedMessage
    id="summary_disabledVeteran"
    defaultMessage="Disabled veteran"
  />
);

export const propertyInfo = (
  <FormattedMessage id="summary_propertyInfo" defaultMessage="Property info" />
);

export const yourProperty = (
  <FormattedMessage id="summary_yourProperty" defaultMessage="Your property" />
);

export const witness1 = (
  <FormattedMessage id="summary_witness1" defaultMessage="Witness 1" />
);
export const witness2 = (
  <FormattedMessage id="summary_witness2" defaultMessage="Witness 2" />
);
export const witnessSignature1 = (
  <FormattedMessage
    id="summary_witnessSignature1"
    defaultMessage="Enter witness full name"
  />
);
export const dateAcquired = (
  <FormattedMessage id="summary_dateAcquired" defaultMessage="Date acquired" />
);

export const firstDatePrimaryResidence = (
  <FormattedMessage
    id="summary_firstDatePrimaryResidence"
    defaultMessage="First date as primary residence"
  />
);

export const proofOwnership = (
  <FormattedMessage
    id="summary_proofOwnership"
    defaultMessage="Proof of ownership"
  />
);

export const mailingAddress = (
  <FormattedMessage
    id="summary_mailingAddress"
    defaultMessage="Mailing address"
  />
);

export const financialInfo = (
  <FormattedMessage
    id="summary_financialInfo"
    defaultMessage="Financial info"
  />
);

export const signingMessage = (
  <FormattedMessage
    id="summary_signingMessage"
    defaultMessage="By signing this application, you confirm that the information provided is true and correct. You will not be able to edit this application after submitting it, unless asked by King County for further information."
  />
);

export const yourInfo = (
  <FormattedMessage id="summary_yourInfo" defaultMessage="Your info" />
);

export const correspondenceAddress = (
  <FormattedMessage
    id="summary_correspondenceAddress"
    defaultMessage="Correspondence address"
  />
);

export const isPrimaryResidence = (
  <FormattedMessage
    id="summary_isPrimaryResidence"
    defaultMessage="This property has been my primary residence since I acquired it."
  />
);

export const othersLiveInProperty = (
  <FormattedMessage
    id="summary_othersLiveInProperty"
    defaultMessage="I share ownership of this property with someone other than my spouse or registered domestic partner."
  />
);

export const hasExperiencedMajorLifeChanges = (
  <FormattedMessage
    id="summary_hasExperiencedMajorLifeChanges"
    defaultMessage="Major life changes"
  />
);

export const ownOtherProperties = (
  <FormattedMessage
    id="summary_ownOtherProperties"
    defaultMessage="Other properties"
  />
);

export const isPropertyCoop = (
  <FormattedMessage
    id="summary_isPropertyCoop"
    defaultMessage="Property is a co-op"
  />
);

export const isTransferringExemption = (
  <FormattedMessage
    id="summary_isTransferringExemption"
    defaultMessage="Transferring from a previous residence"
  />
);

export const applyingForThisYear = (
  <FormattedMessage
    id="summary_applyingForThisYear"
    defaultMessage="Applying for this year"
  />
);

export const filedTaxReturn = (
  <FormattedMessage
    id="summary_filedTaxReturn"
    defaultMessage="Filed a tax return"
  />
);

export const filedForm = (
  <FormattedMessage id="summary_filedForm" defaultMessage="Filed form" />
);

export const dialogEnterYourSignature1 = (
  <FormattedMessage
    id="summary_dialogEnterYourSignature1"
    defaultMessage="To sign, enter your full name"
  />
);

export const whatToDoOnThisPageTitle = (
  <FormattedMessage
    id="summary_whatToDoOnThisPageTitle"
    defaultMessage="What to do on this page"
  />
);
export const whatToDoOnThisPageText1 = (
  <FormattedMessage
    id="summary_whatToDoOnThisPageText1"
    defaultMessage="Please <i>carefully</i> review all of your information shown in the following sections. If you need to update or correct anything, 
    select one of section titles (<b>Your info</b>, <b>Property info</b>, or <b>Financial info</b>) at the top of this page to return to that section and make updates.
    If you need to gather updated information, you can end this session by closing your browser, gather your information, and sign in again later
    to update and sign the application. You won't lose any information you've already entered. {br}{br}
    When everything looks good, to sign and submit your application, at the end of this page, select <b>Add Signature</b>.{br}
    <b>Important</b>: Sign your application <i>only when it's complete</i>, because after signing you can't change or update it."
    values={{
      b: (...chunks) => <b>{chunks}</b>,
      i: (...chunks) => <i>{chunks}</i>,
      br: <br />,
    }}
  />
);

export const witnessObservedSigning = (
  <FormattedMessage
    id="summary_witnessObservedSigning"
    defaultMessage="Witnesses observed the signing"
  />
);

export const thankYouFiling = (
  <FormattedMessage
    id="summary_thankYouFiling"
    defaultMessage="Thank you for filing your Senior Exemption application. Within six to eight weeks, we'll let you know whether your application has been approved, or we'll ask for additional information from you."
  />
);

export const confirmInformation = (
  <FormattedMessage
    id="summary_confirmInformation"
    defaultMessage="By finishing this application, you confirm that the information provided is true and correct. You will not be able to edit this application after submitting it, unless asked by King County for further information."
  />
);

export const backToHome = (
  <FormattedMessage id="summary_backToHome" defaultMessage="Back to home" />
);

export const applicationFiled = (
  <FormattedMessage
    id="summary_applicationFiled"
    defaultMessage="Application filed"
  />
);

export const fezHouseholdIncome = (
  <FormattedMessage
    id="summary_fezHouseholdIncome"
    defaultMessage="Household Income"
  />
);

export const edit = (
  <FormattedMessage id="summary_edit" defaultMessage="Edit" />
);

export const fullName1 = (
  <FormattedMessage id="summary_fullName1" defaultMessage="Your signature" />
);

export const parcelNumber = (
  <FormattedMessage id="summary_parcelNumber" defaultMessage="Parcel #" />
);

export const review = (
  <FormattedMessage id="summary_review" defaultMessage="Review" />
);

export const appStatus = (
  <FormattedMessage
    id="summary_appStatus"
    defaultMessage="Application status: "
  />
);

export const medicareProvider = (
  <FormattedMessage
    id="summary_medicareProvider"
    defaultMessage="Medicare provider"
  />
);

export const submissionSuccessful = (
  <FormattedMessage
    id="summary_submissionSuccessfulA"
    defaultMessage="Submission Successful"
  />
);

export const submissionSuccessfulDesc = (
  <FormattedMessage
    id="summary_submissionSuccessfulB"
    defaultMessage="Your submission has been successful, please click the button below to go home"
  />
);

/**By signing this form, I confirm that I: */
export const signInfoHeader = (
  <FormattedMessage
    id="summary_signInfoHeader"
    defaultMessage="By signing this form, I confirm that I:"
  />
);

/**Have completed the income section of this form and all proof of income is included. */
export const signInfoItem1 = (
  <FormattedMessage
    id="summary_signInfoItem1"
    defaultMessage="Have completed the income section of this form and all proof of income is included."
  />
);

/**Understand it is my responsibility to notify the King County Assessor’s office if I have a change in income or circumstances and that any exemption granted through erroneous information is subject to the correct tax being assessed for the last five years, plus a 100% penalty.*/
export const signInfoItem2 = (
  <FormattedMessage
    id="summary_signInfoItem2"
    defaultMessage="Understand it is my responsibility to notify the King County Assessor’s office if I have a change in income or circumstances and that any exemption granted through erroneous information is subject to the correct tax being assessed for the last five years, plus a 100% penalty."
  />
);

/**Declare under penalty of perjury that the information in this application packet is true and complete. */
export const signInfoItem3 = (
  <FormattedMessage
    id="summary_signInfoItem3"
    defaultMessage="Declare under penalty of perjury that the information in this application packet is true and complete."
  />
);

/**Request a refund under the provisions of RCW 84-69-020 for taxes paid or overpaid as a result of mistake, inadvertence, or lack of knowledge regarding exemption from paying real property taxes pursuant to RCW 84.36.381 through 389. */
export const signInfoItem4 = (
  <FormattedMessage
    id="summary_signInfoItem4"
    defaultMessage="Request a refund under the provisions of RCW 84-69-020 for taxes paid or overpaid as a result of mistake, inadvertence, or lack of knowledge regarding exemption from paying real property taxes pursuant to RCW 84.36.381 through 389."
  />
);
