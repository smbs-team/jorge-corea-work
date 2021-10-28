import { FormattedMessage } from 'react-intl';
import React from 'react';

export const firstGivenName = (
  <FormattedMessage
    id="first_given_name"
    defaultMessage="First (given) name"
    description="first name label"
  />
);

export const leftUploadFileMessage = (
  <FormattedMessage
    id="myInfo_leftUploadFileMessage"
    defaultMessage="Proof of age"
  />
);

export const rightUploadFileMessage = (
  <FormattedMessage
    id="myInfo_RightUploadFileMessage"
    defaultMessage="(required)"
  />
);

export const print = (
  <FormattedMessage
    id="customDialog_printThisPage"
    defaultMessage="Print This Page"
  />
);

export const printPDF = (
  <FormattedMessage
    id="customDialog_daveToPDF"
    defaultMessage="Save page to PDF file"
  />
);
export const helpText = (
  <FormattedMessage
    id="myInfo_helpText"
    defaultMessage="Copy a valid document that verifies your identity and date of birth, such as a driver's license, state identification card, birth certificate, or passport. Obscure the Social Security numbers, account numbers, and any other identification numbers, and then upload the redacted copy."
  />
);

export const belowAge = (
  <FormattedMessage
    id="belowAge"
    defaultMessage="You're below the age limit but might still qualify with proof of disability."
    description="Below age label"
  />
);

export const legalProofDisability = (
  <FormattedMessage
    id="legalProofDisability"
    defaultMessage="I have legal proof of disability (such as an SSI or VA award letter)."
    description="legal proof disability label"
  />
);

export const legalProofDisability_ht = (
  <FormattedMessage
    id="legalProofDisability_ht"
    defaultMessage="Copy one or more of these documents, obscure the Social Security numbers and account numbers, and then upload the redacted copies: Your Supplemental Security Income (SSI) determination letter. Your Veterans Administration (VA) award letter indicating a service-related disability. A current disability form, signed by a physician, that includes the year the disability occurred, the type of disability, and whether the disability is temporary or permanent"
    description="legal proof disability label"
  />
);

export const phoneNumberPlaceholder = (
  <FormattedMessage id="phoneNumberPlaceholder" defaultMessage="000-000-0000" />
);

export const datePlaceHolder = (
  <FormattedMessage id="datePlaceHolder" defaultMessage="mm/dd/yyyy" />
);

export const suffix = <FormattedMessage id="suffix" defaultMessage="Suffix" />;

export const continueLabel = (
  <FormattedMessage id="continueLabel" defaultMessage="Continue" />
);
export const savingLabel = (
  <FormattedMessage id="savingLabel" defaultMessage="Saving..." />
);

export const returnToSummary = (
  <FormattedMessage id="returnToSummary" defaultMessage="Return to summary" />
);

export const documentsToComplete = (
  <FormattedMessage
    id="documentsToComplete"
    defaultMessage="You'll need the following documents to complete this section:"
  />
);

export const proofOfAgeDocument = (
  <FormattedMessage
    id="proofOfAgeDocument"
    defaultMessage="Proof of age (such as state ID or passport)"
  />
);

export const proofOfDisabilityDocument = (
  <FormattedMessage
    id="proofOfDisabilityDocument"
    defaultMessage="If disabled, proof of disability (such as an SSI or VA award letter)"
  />
);

export const updateBrowser = (
  <FormattedMessage
    id="updateBrowser"
    defaultMessage="Update your browser for the best experience with this website. <a>Google Chrome</a>, <b>Mozilla Firefox</b>, or <c>Microsoft Edge</c>"
    values={{
      a: (...chunks) => (
        <a
          href="https://www.google.com/chrome/"
          target="_self"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
      b: (...chunks) => (
        <a
          href="https://www.mozilla.org/en-US/firefox/new/"
          target="_self"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
      c: (...chunks) => (
        <a
          href="https://www.microsoft.com/en-us/edge"
          target="_self"
          rel="noopener noreferrer"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

export const useDesktop = (
  <FormattedMessage
    defaultMessage="For the best experience on this site, we suggest using a desktop or laptop computer or a tablet."
    id="useDesktop"
  />
);
