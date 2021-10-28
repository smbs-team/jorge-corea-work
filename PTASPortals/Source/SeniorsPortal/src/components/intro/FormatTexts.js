/*
//-----------------------------------------------------------------------
// <copyright file="FormatTexts.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
*/
import { FormattedMessage } from 'react-intl';
import React from 'react';

export const introBannerContent = (
  <FormattedMessage
    id="intro_topBannerContent"
    defaultMessage="2021 Portal applications expected to be available mid-March"
  />
);

export const mainHeading = (
  <FormattedMessage
    id="intro_mainHeading"
    defaultMessage="Property tax exemptions"
  />
);

export const mainHeadingSub = (
  <FormattedMessage
    id="intro_mainHeadingSub"
    defaultMessage="for senior and disabled homeowners"
  />
);

export const applyButton = (
  <FormattedMessage id="intro_applyButton" defaultMessage="Sign in to apply" />
);

export const applyLink = (
  <FormattedMessage id="intro_applyLink" defaultMessage="Sign in to continue" />
);

export const quickInfo1 = (
  <FormattedMessage id="intro_quickInfo1" defaultMessage="Will I qualify?" />
);

export const faq = (
  <FormattedMessage
    id="intro_faq"
    defaultMessage="Frequently asked questions"
  />
);

export const quickInfo01BodyList01 = (
  <FormattedMessage
    id="intro_quickInfo01BodyList01"
    defaultMessage="Basic qualifications:"
  />
);

export const doQualify = (
  <FormattedMessage
    id="intro_doQualify"
    defaultMessage="Do I qualify for a property tax exemption?"
  />
);

export const meetCriteria = (
  <FormattedMessage
    id="intro_meetCriteria"
    defaultMessage="You might, if you meet these criteria:"
  />
);

export const criteriaOwnership = (
  <FormattedMessage
    id="intro_criteriaOwnership"
    defaultMessage="{ownership} You owned and occupied a house, mobile
    home, condo, or co-op during the tax year you're applying
    for."
    values={{ ownership: <b>Ownership.</b> }}
  />
);

export const criteriaAge = (
  <FormattedMessage
    id="intro_criteriaAge"
    defaultMessage="{age} You were 61, a veteran with a
    service-connected disability, or retired because of a
    disability during the tax year you're applying for. Or
    your deceased spouse or state-registered domestic partner
    had an exemption at the time of death, and you were 57 or
    older during the tax year you're applying for."
    values={{ age: <b>Age or disability.</b> }}
  />
);

export const more = <FormattedMessage id="intro_more" defaultMessage="More" />;

export const moreOpen = (
  <FormattedMessage
    id="intro_moreOpen"
    defaultMessage="{income} Your annual household income was less
    than $40,000, including Social Security and other
    sources, for the tax years 2016 through 2018. (Maximum
    annual household income for 2019 will be determined in
    August 2019). Household income includes income received
    by you, by your spouse or domestic partner, and by a
    co-tenant who lives with you and has an ownership
    interest in the property."
    values={{ income: <b>Income.</b> }}
  />
);

export const less = <FormattedMessage id="intro_less" defaultMessage="Less" />;

export const requiredDocumentsHeading = (
  <FormattedMessage
    id="intro_requiredDocumentsHeading"
    defaultMessage="What documents will I need when I apply?"
  />
);

export const requiredDocumentsBody = (
  <FormattedMessage
    id="intro_requiredDocumentsBody"
    defaultMessage="You'll need to provide redacted documentation about you,
    your property, and your finances. Before you upload or
    mail your documents, use a heavy felt pen, tape, or a
    digital editing program to obscure all Social Security and
    account numbers."
  />
);

export const aboutYou = (
  <FormattedMessage
    id="intro_aboutYou"
    defaultMessage="{aboutYou} Proof of identity and date of birth (a
      driver's license, state ID card, birth certificate, or
      passport) for you and for your spouse or registered
      domestic partner, if applicable"
    values={{ aboutYou: <b>About you.</b> }}
  />
);

export const morePartnership = (
  <FormattedMessage
    id="intro_partnership"
    defaultMessage="State-registered domestic partnership agreement, if applicable"
  />
);

export const moreDisability = (
  <FormattedMessage
    id="intro_disability"
    defaultMessage="Proof of disability, if applicable, which can be any of the following:"
  />
);

export const moreSignedDisability = (
  <FormattedMessage
    id="intro_signedDisability"
    defaultMessage="A current, physician-signed disability form indicating
    the year the disability occurred, the type of
    disability, and whether the disability is temporary or
    permanent"
  />
);

export const moreSsi = (
  <FormattedMessage
    id="intro_Ssi"
    defaultMessage="A Supplemental Security Income (SSI) determination letter"
  />
);

export const veteranAffairs = (
  <FormattedMessage
    id="intro_veteranAffairs"
    defaultMessage="A copy of your Veterans Affairs (VA) award letter indicating a service-connected disability"
  />
);

export const aboutProperty = (
  <FormattedMessage
    id="intro_aboutProperty"
    defaultMessage="About your property"
  />
);

export const declarationTrust = (
  <FormattedMessage
    id="intro_declarationTrust"
    defaultMessage=" Date of purchase and occupancy of your residence, and
    the deed, trust, recorded lease, or completed <a>Declaration of Trust</a>"
    values={{
      a: (...chunks) => (
        <a href="https://www.kingcounty.gov/depts/assessor/~/media/depts/assessor/documents/Forms/ExemptionForms/Senior/DeclarationOfTrust.ashx">
          {chunks}
        </a>
      ),
    }}
  />
);

export const yourProperties = (
  <FormattedMessage
    id="intro_yourProperties"
    defaultMessage="Address and country of any other property you own"
  />
);

export const aboutFinances = (
  <FormattedMessage
    id="intro_aboutFinances"
    defaultMessage="About your finances"
  />
);

export const documentVerifyIncome = (
  <FormattedMessage
    id="intro_documentVerifyIncome"
    defaultMessage="Federal tax returns or other documents for each application year to verify your income"
  />
);

export const whatExpect = (
  <FormattedMessage
    id="intro_whatExpect"
    defaultMessage="What to expect when you apply"
  />
);

export const whatExpectBody = (
  <FormattedMessage
    id="intro_whatExpectBody"
    defaultMessage="We’ll let you know we’ve received your application. After we've reviewed it, we'll notify you of your exemption status or we’ll ask you for additional information."
  />
);

export const applyOnlineB = (
  <FormattedMessage
    id="intro_applyOnlineB"
    defaultMessage=", or <a>download a printable application</a>, complete it, and mail it to us."
    values={{
      a: (...chunks) => (
        <a
          href="https://www.kingcounty.gov/depts/assessor/~/media/depts/assessor/documents/Forms/ExemptionForms/Senior/SrCitDisApp.ashx"
          className="link-black"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

export const needMoreHelp = (
  <FormattedMessage id="intro_needMoreHelp" defaultMessage="Need more help?" />
);

export const callUs = (
  <FormattedMessage
    id="intro_callUs"
    defaultMessage="Call us at 206-296-3920 or visit our office."
  />
);

export const departmentAssessments = (
  <FormattedMessage
    id="intro_departmentAssessments"
    defaultMessage="Department of Assessments"
  />
);

export const building = (
  <FormattedMessage
    id="intro_building"
    defaultMessage="King County Administration Building"
  />
);

export const address1 = (
  <FormattedMessage
    id="intro_address1"
    defaultMessage="500 4th Ave., Room 740"
  />
);

export const address2 = (
  <FormattedMessage
    id="intro_address2"
    defaultMessage="Seattle, WA 98104-2384"
  />
);

export const print = (
  <FormattedMessage id="intro_Print" defaultMessage="Print" />
);

export const qualDetailsHeader = (
  <FormattedMessage
    id="intro_qualDetailsHeader"
    defaultMessage="Qualification details"
  />
);

export const qualDetailsContent = (
  <FormattedMessage
    id="intro_qualDetailsContent"
    defaultMessage="Your qualification for a property tax exemption is based on your ownership, occupancy, age, and income for the preceding year. For example, a property tax exemption for 2021 is based on 2020 information. You must meet four requirements to qualify:"
  />
);

/**
 * Ownership and Occupancy
 */
export const ownershipOccHeader = (
  <FormattedMessage
    id="intro_ownershipOccHeader"
    defaultMessage="Ownership and Occupancy"
  />
);

/**
 * You own and occupy your house, mobile home, condo, or co-op.
 */
export const ownershipOccItem1 = (
  <FormattedMessage
    id="intro_ownershipOccItem1"
    defaultMessage="You own and occupy your house, mobile home, condo, or co-op."
  />
);

/**
 * It is your principal residence and has been your principal residence for more than 6 months in 2020, 9 months in 2019, or more than 6 months in preceding years.
 */
export const ownershipOccItem2 = (
  <FormattedMessage
    id="intro_ownershipOccItem2"
    defaultMessage="It is your principal residence and has been your principal residence for more than 6 months in 2020, 9 months in 2019, or more than 6 months in preceding years."
  />
);

/**Income */
export const incomeHeader = (
  <FormattedMessage id="intro_incomeHeader" defaultMessage="Income" />
);

/**For an exemption on your 2021 property taxes, your household income for 2020 was $58,423 or less. */
export const incomeItem1 = (
  <FormattedMessage
    id="intro_incomeItem1"
    defaultMessage="For an exemption on your 2021 property taxes, your household income for 2020 was $58,423 or less."
  />
);

/**For an exemption on your 2018 or 2019 property taxes, your household income for 2017, or 2018 was $40,000 or less. */
export const incomeItem2 = (
  <FormattedMessage
    id="intro_incomeItem2"
    defaultMessage="For an exemption on your 2018 or 2019 property taxes, your household income for 2017, or 2018 was $40,000 or less."
  />
);

export const changes20202021Header = (
  <FormattedMessage
    id="intro_changes20202021Header"
    defaultMessage="Changes in 2020 and 2021"
  />
);

/**If you applied for a property tax exemption in the past but didn’t qualify, consider applying again in 2021 and for 2020. For property taxes payable in 2020 and 2021, you can apply now. Allowable income levels increased substantially for 2019 (for property taxes payable in 2020) and 2020 (for property taxes payable in 2021) to $58,423 for your income. */
export const changes20202021Content = (
  <FormattedMessage
    id="intro_changes20202021Content"
    defaultMessage="If you applied for a property tax exemption in the past but didn’t qualify, consider applying again in 2021 and for 2020. For property taxes payable in 2020 and 2021, you can apply now. Allowable income levels increased substantially for 2019 (for property taxes payable in 2020) and 2020 (for property taxes payable in 2021) to $58,423 for your income."
  />
);

/**Applying for more than one year */
export const applyingMoreOneYearHeader = (
  <FormattedMessage
    id="intro_applyingMoreOneYearHeader"
    defaultMessage="Applying for more than one year"
  />
);

/**You can apply for a property tax exemption for 2018, 2019, 2020 and/or 2021. */
export const applyingMoreOneYearContent = (
  <FormattedMessage
    id="intro_applyingMoreOneYearContent"
    defaultMessage="You can apply for a property tax exemption for 2018, 2019, 2020 and/or 2021."
  />
);

/**You'll need to fill out a separate application for each year. */
export const applyingMoreOneYearItem1 = (
  <FormattedMessage
    id="intro_applyingMoreOneYearItem1"
    defaultMessage="You'll need to fill out a separate application for each year."
  />
);

/**We recommend that you apply for the 2021 year first so we can place you on the exemption program moving forward. */
export const applyingMoreOneYearItem2 = (
  <FormattedMessage
    id="intro_applyingMoreOneYearItem2"
    defaultMessage="We recommend that you apply for the 2021 year first so we can place you on the exemption program moving forward."
  />
);

/**If you receive an exemption for a prior year and you’ve already paid taxes for that year, you might receive a refund. */
export const applyingMoreOneYearItem3 = (
  <FormattedMessage
    id="intro_applyingMoreOneYearItem3"
    defaultMessage="If you receive an exemption for a prior year and you’ve already paid taxes for that year, you might receive a refund."
  />
);

/**On this page */
export const nipHeader = (
  <FormattedMessage id="intro_nipHeader" defaultMessage="On this page" />
);

export const nipOverview = (
  <FormattedMessage id="intro_nipOverview" defaultMessage="Overview" />
);

export const nipHelpVideos = (
  <FormattedMessage id="intro_nipHelpVideos" defaultMessage="Helpful videos" />
);

export const nipGettingHelp = (
  <FormattedMessage id="intro_nipGettingHelp" defaultMessage="Getting help" />
);

export const nipQualDetails = (
  <FormattedMessage
    id="intro_nipQualDetails"
    defaultMessage="Qualification details"
  />
);

export const nipApplicationDetails = (
  <FormattedMessage
    id="intro_nipApplicationDetails"
    defaultMessage="Application details"
  />
);

export const nipMoreInfo = (
  <FormattedMessage id="intro_nipMoreInfo" defaultMessage="More info" />
);

export const nipApplyNow = (
  <FormattedMessage id="intro_nipApplyNow" defaultMessage="Apply now" />
);

/**Basic qualifications: */
export const bqHeader = (
  <FormattedMessage
    id="intro_bqHeader"
    defaultMessage="Basic qualifications:"
  />
);

/**Own the home you live in */
export const bqItem1 = (
  <FormattedMessage
    id="intro_bqItem1"
    defaultMessage="Own the home you live in"
  />
);

/**At least age 61 by December 31 of the preceding year or disabled */
export const bqItem2 = (
  <FormattedMessage
    id="intro_bqItem2"
    defaultMessage="At least age 61 by December 31 of the preceding year or disabled"
  />
);

/**Max income of $58,423 (2019 and 2020) or $40,000 (2017 and 2018) */
export const bqItem3 = (
  <FormattedMessage
    id="intro_bqItem3"
    defaultMessage="Max income of $58,423 (2019 and 2020) or $40,000 (2017 and 2018)"
  />
);

/**internal use */
const readQualDetailLinkText = (
  <FormattedMessage
    id="intro_readQualDetailLinkText"
    defaultMessage="Qualification details"
  />
);

/**Read the full {qualDetailLink} section on this page. */
export const readQualDetail = (
  <FormattedMessage
    id="intro_readQualDetail"
    defaultMessage="Read the full {qualDetailLink} section on this page."
    values={{
      qualDetailLink: <a href="#qualification">{readQualDetailLinkText}</a>,
    }}
  />
);

/**What will I need? */
export const quickInfo2 = (
  <FormattedMessage id="intro_quickInfo2" defaultMessage="What will I need?" />
);

/**Open this printable list of documents to gather. */
export const quickInfo2Content1 = (
  <FormattedMessage
    id="intro_quickInfo2Content1"
    defaultMessage="Open this printable list of documents to gather."
  />
);

/**Documents you'll need */
export const quickInfo2BtnText = (
  <FormattedMessage
    id="intro_quickInfo2BtnText"
    defaultMessage="Documents you'll need"
  />
);

const quickInfo2Content2Gmail = (
  <FormattedMessage id="intro_quickInfo2Content2Gmail" defaultMessage="Gmail" />
);

const quickInfo2Content2Outlook = (
  <FormattedMessage
    id="intro_quickInfo2Content2Outlook"
    defaultMessage="Outlook"
  />
);

/**You'll need an email account, such as one from {gmailLink} or {outlookLink}. */
export const quickInfo2Content2 = (
  <FormattedMessage
    id="intro_quickInfo2Content2"
    defaultMessage="You'll need an email account, such as one from {gmailLink} or {outlookLink}."
    values={{
      gmailLink: (
        <a href="https://www.google.com/gmail/">{quickInfo2Content2Gmail}</a>
      ),
      outlookLink: (
        <a href="https://outlook.live.com/">{quickInfo2Content2Outlook}</a>
      ),
    }}
  />
);

const quickInfo2Content3Email = (
  <FormattedMessage
    id="intro_quickInfo2Content3Email"
    defaultMessage="email us"
  />
);

/**For help, {emailLink} or call */
export const quickInfo2Content3 = (
  <FormattedMessage
    id="intro_quickInfo2Content3"
    defaultMessage="For help, {emailLink} or call"
    values={{
      emailLink: (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {quickInfo2Content3Email}
        </a>
      ),
    }}
  />
);

/**How do I apply? */
export const quickInfo3 = (
  <FormattedMessage id="intro_quickInfo3" defaultMessage="How do I apply?" />
);

const quickInfo3Content1Video = (
  <FormattedMessage
    id="intro_quickInfo3Content1Video"
    defaultMessage="Watch short videos"
  />
);

/**{videoLink} to learn exactly what you need to do. */
export const quickInfo3Content1 = (
  <FormattedMessage
    id="intro_quickInfo3Content1"
    defaultMessage="{videoLink} to learn exactly what you need to do."
    values={{
      videoLink: <a href="#videos">{quickInfo3Content1Video}</a>,
    }}
  />
);

/**Already started an application? */
export const quickInfo3Content2 = (
  <FormattedMessage
    id="intro_quickInfo3Content2"
    defaultMessage="Already started an application?"
  />
);

export const helpFullVideos = (
  <FormattedMessage id="intro_helpFullVideos" defaultMessage="Helpful videos" />
);

export const gettingHelp = (
  <FormattedMessage id="intro_gettingHelp" defaultMessage="Getting help" />
);

/**
 * Applying includes several steps, such as gathering information to enter and documents to attach. We encourage you to ask a friend or family member for help if you need it. Your local senior centers and AARP locations might hold special events to help you apply.
 */
export const gettingHelpContent = (
  <FormattedMessage
    id="intro_gettingHelpContent"
    defaultMessage="Applying includes several steps, such as gathering information to enter and documents to attach. We encourage you to ask a friend or family member for help if you need it. Your local senior centers and AARP locations might hold special events to help you apply."
  />
);

const questionsEmail = (
  <FormattedMessage
    id="intro_questionsEmail"
    defaultMessage="Exemptions.Assessments@kingcounty.gov"
  />
);

/**Questions? Email {emailLink} or call 206-296-3920. */
export const questions = (
  <FormattedMessage
    id="intro_questions"
    defaultMessage="Questions? Email {emailLink} or call 206-296-3920."
    values={{
      emailLink: (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {questionsEmail}
        </a>
      ),
    }}
  />
);

export const ageOrDis = (
  <FormattedMessage id="intro_ageOrDis" defaultMessage="Age or disability" />
);

/**You are at least age 61 by December 31 of the preceding year. */
export const ageOrDisItem1 = (
  <FormattedMessage
    id="intro_ageOrDisItem1"
    defaultMessage="You are at least age 61 by December 31 of the preceding year."
  />
);

/**Or you are veteran with a service-connected disability or disabled. */
export const ageOrDisItem2 = (
  <FormattedMessage
    id="intro_ageOrDisItem2"
    defaultMessage="Or you are veteran with a service-connected disability or disabled."
  />
);

/**You might also qualify if your deceased spouse or registered domestic partner had an exemption at the time of death and you were at least age 57 that year. */
export const ageOrDisItem3 = (
  <FormattedMessage
    id="intro_ageOrDisItem3"
    defaultMessage="You might also qualify if your deceased spouse or registered domestic partner had an exemption at the time of death and you were at least age 57 that year."
  />
);

const householdIncomeHI = (
  <FormattedMessage
    id="intro_householdIncomeHI"
    defaultMessage="Household income"
  />
);

/**{hi} includes income received by you, by your spouse or domestic partner, and by a co-owner who lives with you. Remember to subtract qualified expenses. */
export const householdIncome = (
  <FormattedMessage
    id="intro_householdIncome"
    defaultMessage="{hi} includes income received by you, by your spouse or domestic partner, and by a co-owner who lives with you. Remember to subtract qualified expenses."
    values={{
      hi: <i>{householdIncomeHI}</i>,
    }}
  />
);

/**Application details */
export const appDetail = (
  <FormattedMessage id="intro_appDetail" defaultMessage="Application details" />
);

/**Still have questions? */
export const stHaveQuest = (
  <FormattedMessage
    id="intro_stHaveQuest"
    defaultMessage="Still have questions?"
  />
);

const stHaveQuestItem1Email = (
  <FormattedMessage
    id="intro_stHaveQuestItem1Email"
    defaultMessage="Exemptions.Assessments@kingcounty.gov"
  />
);

/**Email us at {emailLink} */
export const stHaveQuestItem1 = (
  <FormattedMessage
    id="intro_stHaveQuestItem1"
    defaultMessage="Email us at {emailLink}"
    values={{
      emailLink: (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {stHaveQuestItem1Email}
        </a>
      ),
    }}
  />
);

/**Give us a call at 206-296-3920 */
export const stHaveQuestItem2 = (
  <FormattedMessage
    id="intro_stHaveQuestItem2"
    defaultMessage="Give us a call at 206-296-3920"
  />
);

const stHaveQuestContentFAQ = (
  <FormattedMessage
    id="intro_stHaveQuestContentFAQ"
    defaultMessage="Frequently Asked Questions"
  />
);

/**For detailed information about the Senior Exemption program, see our {faqLink} (FAQ) page. */
export const stHaveQuestContent = onClick => (
  <FormattedMessage
    id="intro_stHaveQuestContent"
    defaultMessage="For detailed information about the Senior Exemption program, see our {faqLink} (FAQ) page."
    values={{
      faqLink: (
        <a
          className="link-popup"
          href="#faq_popup"
          style={{ cursor: 'pointer' }}
          onClick={onClick}
        >
          {stHaveQuestContentFAQ}
        </a>
      ),
    }}
  />
);
