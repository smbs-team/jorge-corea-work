import { FormattedMessage } from 'react-intl';
import React from 'react';

export const print = <FormattedMessage id="print" defaultMessage="Print" />;

export const mainHeader = (
  <FormattedMessage
    id="incomeHelp_mainHeader"
    defaultMessage="Documents and information you’ll need"
  />
);
export const mainHeaderDesc = (
  <FormattedMessage
    id="incomeHelp_mainHeaderDesc"
    defaultMessage="King County property tax relief programs for seniors, disabled
    persons, and disabled veterans"
  />
);

export const aboutYou = (
  <FormattedMessage
    id="incomeHelp_aboutYou"
    defaultMessage="About you and any co-owners"
  />
);

export const proofOfIdentity = (
  <FormattedMessage
    id="incomeHelp_proofOfIdentity"
    defaultMessage="Proof of identity"
  />
);
export const proofOfIdentityDesc = (
  <FormattedMessage
    id="incomeHelp_proofOfIdentityDesc"
    defaultMessage=" An official document that proves your name and date of
    birth, such as a driver's license"
  />
);

export const domesticPartnerAgreement = (
  <FormattedMessage
    id="incomeHelp_domesticPartnerAgreement"
    defaultMessage="Domestic partnership agreement"
  />
);
export const domesticPartnerAgreementDesc = (
  <FormattedMessage
    id="incomeHelp_domesticPartnerAgreementDesc"
    defaultMessage="If you have a registered domestic partner"
  />
);

export const proofOfDisability = (
  <FormattedMessage
    id="incomeHelp_proofOfDisability"
    defaultMessage="Proof of disability"
  />
);
export const proofOfDisabilityTitle = (
  <FormattedMessage
    id="incomeHelp_proofOfDisabilityTitle"
    defaultMessage="If you’re disabled, such as:"
  />
);
export const proofOfDisabilityDesc = (
  <FormattedMessage
    id="incomeHelp_proofOfDisabilityDesc"
    defaultMessage="A current King County Department of Assessments
    physician-signed disability form"
  />
);
export const proofOfDisabilityDesc2 = (
  <FormattedMessage
    id="incomeHelp_proofOfDisabilityDesc2"
    defaultMessage="A Supplemental Security Income (SSI) determination letter"
  />
);
export const proofOfDisabilityDesc3 = (
  <FormattedMessage
    id="incomeHelp_proofOfDisabilityDesc3"
    defaultMessage="A Veterans Affairs (VA) award letter indicating a
    service-connected disability"
  />
);

export const aboutYourProperty = (
  <FormattedMessage
    id="incomeHelp_aboutYourProperty"
    defaultMessage="About your property"
  />
);
export const moveInYear1 = (
  <FormattedMessage id="incomeHelp_moveInYear1" defaultMessage="Move-in year" />
);
export const moveInYear1Desc = (
  <FormattedMessage
    id="incomeHelp_moveInYear1Desc"
    defaultMessage="Approximate year you purchased your residence and
    began to live there"
  />
);
export const trustDocuments1 = (
  <FormattedMessage
    id="incomeHelp_trustDocuments1"
    defaultMessage="Trust documents"
  />
);

export const trustDocuments2 = (
  <FormattedMessage
    id="incomeHelp_trustDocuments2"
    defaultMessage="If your property is in a trust"
  />
);
export const addresses = (
  <FormattedMessage id="incomeHelp_addresses" defaultMessage="Addresses" />
);

export const addressesDesc = (
  <FormattedMessage
    id="incomeHelp_addressesDesc"
    defaultMessage="All property addresses you own in any state or country"
  />
);

export const aboutYourFinances = (
  <FormattedMessage
    id="incomeHelp_aboutYourFinances"
    defaultMessage="About your finances"
  />
);

export const aboutYourFinancesDesc = (
  <FormattedMessage
    id="incomeHelp_protectingPersonalInfoDesc"
    id="incomeHelp_aboutYourFinancesDesc"
    defaultMessage="Your qualification for a property tax exemption for a specific
    year is based on your income and expenses for the <b>previous year</b>."
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const prevYear = (
  <FormattedMessage id="incomeHelp_prevYearA" defaultMessage="previous year" />
);

export const applicationYear = (
  <FormattedMessage
    id="incomeHelp_applicationYear"
    defaultMessage="Application year:"
  />
);
export const useInfoFrom = (
  <FormattedMessage id="incomeHelp_prevYearB" defaultMessage="Use info from:" />
);

export const incomeDocuments = (
  <FormattedMessage
    id="incomeHelp_incomeDocuments"
    defaultMessage="Income documents"
  />
);
export const incomeDocumentsTitle = (
  <FormattedMessage
    id="incomeHelp_incomeDocumentsTitle"
    defaultMessage="You'll need to provide these documents from:"
  />
);
export const incomeDocumentsWho1 = (
  <FormattedMessage
    id="incomeHelp_incomeDocumentsWho1"
    defaultMessage="You and your spouse or registered domestic partner"
  />
);
export const incomeDocumentsWho2 = (
  <FormattedMessage
    id="incomeHelp_incomeDocumentsWho2"
    defaultMessage="Co-owners who live with you"
  />
);
export const generalIncome0 = (
  <FormattedMessage
    id="incomeHelp_generalIncome0"
    defaultMessage="General income"
  />
);

export const generalIncome1 = (
  <FormattedMessage
    id="incomeHelp_generalIncome1"
    defaultMessage="All pages of your complete <b>federal tax return</b>. Include all schedules (such as Schedule C or Schedule D) and supporting documents (such as W-2s and 1099s)."
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const generalIncome2 = (
  <FormattedMessage
    id="incomeHelp_generalIncome2"
    defaultMessage="If you don’t file a tax return, provide  
    <b>all other documents</b>
    (such as 1099s, W-2s, and statements from annuities and IRAs) and information about help from family, friends, or public assistance (such as gift letters or annual statements)."
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const otherIncome = (
  <FormattedMessage id="incomeHelp_otherIncome" defaultMessage="Other income" />
);

export const otherIncomeDesc = (
  <FormattedMessage
    id="incomeHelp_otherIncomeDesc"
    defaultMessage=" <b>Documentation of income</b> earned by a spouse, a
    registered domestic partner, or a co-owner who lives with
    you (such as a complete copy of a federal tax return and all
    attachments and schedules)"
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const incomeFromOtherCountries = (
  <FormattedMessage
    id="incomeHelp_incomeFromOtherCountries"
    defaultMessage="Income from other countries"
  />
);

export const incomeFromOtherCountriesDesc = (
  <FormattedMessage
    id="incomeHelp_incomeFromOtherCountriesDesc"
    defaultMessage=" <b>Annual statements</b> or bank statements showing
    deposits, or receipts"
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const expenseDocuments = (
  <FormattedMessage
    id="incomeHelp_expenseDocuments"
    defaultMessage="Expense documents"
  />
);
export const qualifiedExpenses = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpenses"
    defaultMessage="Qualified expenses"
  />
);
export const qualifiedExpenses1 = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpenses1"
    defaultMessage="Annual statements are preferred for:"
  />
);

export const qualifiedExpensesDesc = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpensesDesc"
    defaultMessage="Assisted living or adult family home"
  />
);

export const qualifiedExpensesDesc2 = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpensesDesc2"
    defaultMessage="In-home care or nursing home"
  />
);

export const qualifiedExpensesDesc3 = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpensesDesc3"
    defaultMessage="Nonreimbursed prescriptions"
  />
);

export const qualifiedExpensesDesc4 = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpensesDesc4"
    defaultMessage="Social Security–approved Medicare Advantage plan"
  />
);

export const qualifiedExpensesDesc5 = (
  <FormattedMessage
    id="incomeHelp_qualifiedExpensesDesc5"
    defaultMessage="(Dental plans, supplemental insurance plans, optical
      plans, and company insurance plans don’t qualify.)"
  />
);

export const protectingPersonalInfo = (
  <FormattedMessage
    id="incomeHelp_protectingPersonalInfo"
    defaultMessage="Protecting personal info"
  />
);

export const protectingPersonalInfoDesc1 = (
  <FormattedMessage
    id="incomeHelp_protectingPersonalInfoDesc1"
    defaultMessage="If you’re <b>applying online</b>, cover up all Social Security and
    account numbers as you attach the images of your documents. If you
    <b>mail copies</b> of your documents, you can cover up these
    numbers with a thick felt pen or tape."
    values={{
      b: (...chunks) => <b>{chunks}</b>,

      br: <br />,
    }}
  />
);

export const coverUpPersonalInfo = (
  <FormattedMessage
    id="incomeHelp_coverUpPersonalInfo"
    defaultMessage="Cover up Social Security and account numbers example"
  />
);
