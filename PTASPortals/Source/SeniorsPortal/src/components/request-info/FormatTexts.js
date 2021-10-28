//-----------------------------------------------------------------------
// <copyright file="FormatText.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const hideSocialSecurity = (
  <FormattedMessage
    id="requestInfo_hideSocialSecurity"
    defaultMessage="Hide Social Security and account numbers, either on paper copies or after attaching with the privacy marker."
  />
);

export const helpTextObscure = (
  <FormattedMessage
    id="requestInfo_helpTextObscure"
    defaultMessage="Before you begin, copy all documents and obscure the Social Security 
            numbers and account numbers. You can do this using paper or tape, a felt pen, or an online image-editing program. Then, upload the redacted copies."
  />
);
export const proofOfAge = (
  <FormattedMessage
    id="requestInfo_helpText"
    defaultMessage="Copy a valid document that verifies your identity and date of birth, such as a driver's license, state identification card, birth certificate, or passport. Obscure the Social Security numbers, account numbers, and any other identification numbers, and then upload the redacted copy."
  />
);
export const selectPhotoOfDocument = (
  <FormattedMessage
    id="requestInfo_selectPhotoOfDocument"
    defaultMessage="Select a photo of your document or the document file"
  />
);
export const attachingDocuments = (
  <FormattedMessage
    id="requestInfo_attachingDocuments"
    defaultMessage="Attaching Documents"
  />
);

export const WeNeedToConfirmAmounts = (
  <FormattedMessage
    id="request_infoWeNeedToConfirmAmounts"
    defaultMessage="We need to confirm that the amounts entered match your financial forms."
  />
);

export const allDocsAreNecessary = (
  <FormattedMessage
    id="requestInfo_allDocsAreNecessary"
    defaultMessage="All requested documents are required to continue and complete your application."
  />
);

export const beforeContinueText = (
  <FormattedMessage
    id="requestInfo_beforeContinueText"
    defaultMessage="If you need more time to find and upload documents, don’t click <strong>Continue</strong>.{break}{break}You can return to your application later to upload more documents. Those you’ve uploaded already are saved but not submitted.{break}{break}When you’re finished uploading documents, click <strong>Continue</strong> to submit them. After you click <strong>Continue</strong>, you’ll no longer be able to edit your application."
    values={{
      strong: (...chunks) => <strong>{chunks}</strong>,
      break: <br></br>,
    }}
  />
);

export const youMayObscureText = (
  <FormattedMessage
    id="requestInfo_youMayObscureText"
    defaultMessage="Please hide personal information in your documents—either before or as you upload them."
  />
);

export const whatShouldIObscureText = (
  <FormattedMessage
    id="requestInfo_whatShouldIObscureText"
    defaultMessage="What should I hide?"
  />
);

export const documentsNotSubmittedText = (
  <FormattedMessage
    id="requestInfo_documentsNotSubmittedText"
    defaultMessage="The following documents weren’t submitted with your application but would help us process your application. Please upload these documents if you have them."
  />
);

export const continueLabel = (
  <FormattedMessage id="requestInfo_continueLabel" defaultMessage="Continue" />
);
export const savingLabel = (
  <FormattedMessage id="requestInfo_savingLabel" defaultMessage="Saving..." />
);

export const documentsToComplete = (
  <FormattedMessage
    id="requestInfo_documentsToComplete"
    defaultMessage="To continue, you need to enter your information:"
  />
);

export const proofOfAgeDocument = (
  <FormattedMessage
    id="requestInfo_proofOfAgeDocument"
    defaultMessage="Proof of age (such as state ID or passport)"
  />
);
export const proofOfAgeDocumentSpouse = (
  <FormattedMessage
    id="requestInfo_proofOfAgeDocumentSpouse"
    defaultMessage="Spouse proof of age (such as state ID or passport)"
  />
);

export const proofOfDisabilityDocument = (
  <FormattedMessage
    id="requestInfo_proofOfDisabilityDocument"
    defaultMessage="If disabled, proof of disability (such as an SSI or VA award letter)"
  />
);
export const proof1040Document = (
  <FormattedMessage
    id="requestInfo_proof1040Document"
    defaultMessage="Form 1040"
  />
);
export const proof1040EZDocument = (
  <FormattedMessage
    id="requestInfo_proof1040EZDocument"
    defaultMessage="Form 1040EZ"
  />
);
export const proof1099Document = (
  <FormattedMessage
    id="requestInfo_proof1099Document"
    defaultMessage="Form 1099"
  />
);
export const proof8829Document = (
  <FormattedMessage
    id="requestInfo_proof8829Document"
    defaultMessage="Form 8829"
  />
);
export const proof8949Document = (
  <FormattedMessage
    id="requestInfo_proof8949Document"
    defaultMessage="Form 8949"
  />
);
export const proofSch1Document = (
  <FormattedMessage
    id="requestInfo_proofSch1Document"
    defaultMessage="Schedule 1"
  />
);
export const proofSchCDocument = (
  <FormattedMessage
    id="requestInfo_proofSchCDocument"
    defaultMessage="Schedule C"
  />
);
export const proofSchDDocument = (
  <FormattedMessage
    id="requestInfo_proofSchDDocument"
    defaultMessage="Schedule D"
  />
);
export const proofSchEDocument = (
  <FormattedMessage
    id="requestInfo_proofSchEDocument"
    defaultMessage="Schedule E"
  />
);
export const proofSchFDocument = (
  <FormattedMessage
    id="requestInfo_proofSchFDocument"
    defaultMessage="Schedule F"
  />
);
export const proofExpensesDocument = (
  <FormattedMessage
    id="requestInfo_proofExpensesDocument"
    defaultMessage="Expenses"
  />
);
export const proofCoopDocument = (
  <FormattedMessage
    id="requestInfo_proofCoopDocument"
    defaultMessage="Proof of Co-Op Shares"
  />
);
export const proofLifeChangeDocument = (
  <FormattedMessage
    id="requestInfo_proofLifeChangeDocument"
    defaultMessage="Proof of Life Change"
  />
);
export const proofOwnershipDocument = (
  <FormattedMessage
    id="requestInfo_proofOwnershipDocument"
    defaultMessage="Proof of Ownership"
  />
);
export const proofOfDisabilityDocumentSpouse = (
  <FormattedMessage
    id="requestInfo_proofOfDisabilityDocumentSpouse"
    defaultMessage="If Spouse is disabled, proof of disability (such as an SSI or VA award letter)"
  />
);

export const helpText1 = (
  <FormattedMessage
    id="requestInfo_obscureMessage"
    defaultMessage="It's important to obscure Social Security and account numbers on your documents, which become part of the public record."
  />
);
export const helpText2 = (
  <FormattedMessage
    id="requestInfo_obscureMessage2"
    defaultMessage="You can obscure them with our online tool as you upload your documents."
  />
);
export const helpText3 = (
  <FormattedMessage
    id="requestInfo_obscureMessage3"
    defaultMessage="Or, before you upload, copy your documents and obscure this information using paper or tape, a felt pen, or an online image-editing program. Then upload the redacted copies."
  />
);
