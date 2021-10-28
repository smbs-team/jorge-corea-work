import { FormattedMessage } from 'react-intl';
import React from 'react';

export const question = (
  <FormattedMessage
    id="faq_question"
    defaultMessage="{question}"
    values={{ question: <span className="question">Q</span> }}
  />
);

export const answer = (
  <FormattedMessage
    id="faq_answer"
    defaultMessage="{answer}"
    values={{ answer: <span className="answer">A</span> }}
  />
);

export const frequentlyAsked = (
  <FormattedMessage
    id="faq_frequentlyAsked"
    defaultMessage="Frequently asked questions"
  />
);
export const description = (
  <FormattedMessage
    id="faq_description"
    defaultMessage="King County property tax relief programs for seniors, disabled persons, and disabled veterans"
  />
);
export const programBasics = (
  <FormattedMessage id="faq_programBasics" defaultMessage="Program basics" />
);
export const propertyTaxReliefAvailable = (
  <FormattedMessage
    id="faq_propertyTaxReliefAvailable"
    defaultMessage="What property tax relief is available for seniors, disabled persons, and disabled veterans?"
  />
);
export const propertyTaxReliefAvailableAnswer = (
  <FormattedMessage
    id="faq_propertyTaxReliefAvailableAnswer"
    defaultMessage="The following property tax relief is available for seniors, disabled persons, and disabled veterans:"
  />
);
export const propertyTaxesExemption = (
  <FormattedMessage
    id="faq_propertyTaxesExemption"
    defaultMessage="{strong} Your property taxes are reduced for as long as you meet the qualifications and continue to own and live in your home."
    values={{ strong: <strong>Property tax exemptions.</strong> }}
  />
);
export const propertyTaxesDeferral = (
  <FormattedMessage
    id="faq_propertyTaxesDeferral"
    defaultMessage="{strong} Your property tax is deferred for payment at a later time. Deferred taxes and interest become a lien (obligation to repay) on your property. The lien is due when you no longer qualify for the program."
    values={{ strong: <strong>Property tax deferrals.</strong> }}
  />
);
export const whatExemption = (
  <FormattedMessage
    id="faq_whatExemption"
    defaultMessage="What's an exemption?"
  />
);
export const whatExemptionAnswer1 = (
  <FormattedMessage
    id="faq_whatExemptionAnswer1"
    defaultMessage="A {em} {strong} as of January 1 of the first year you receive the exemption. (This value is called the {em2}.)"
    values={{
      em: <em>property tax exemption</em>,
      strong: <strong>freezes the taxable value of the residence</strong>,
      em2: <em>frozen value</em>,
    }}
  />
);
export const whatExemptionAnswer2 = (
  <FormattedMessage
    id="faq_whatExemptionAnswer2"
    defaultMessage="As long as you qualify for the exemption, your annual property taxes will be based on the lesser of your frozen value or the current taxable (market) value. The taxable (market) value of the property will still be assessed annually, and you'll be notified of changes."
  />
);
export const whatExemptionAnswer3 = (
  <FormattedMessage
    id="faq_whatExemptionAnswer3"
    defaultMessage="A property tax exemption renews automatically for up to three years following your initial application, but you need to reapply every four years. If your financial situation changes, it's your responsibility to notify the King County Department of Assessments to evaluate your new financial situation so you can avoid penalties."
  />
);
export const whatDeferral = (
  <FormattedMessage id="faq_whatDeferral" defaultMessage="What's a deferral?" />
);
export const whatDeferralAnswer = (
  <FormattedMessage
    id="faq_whatDeferralAnswer"
    defaultMessage="A {em} {strong}. Instead, the State of Washington pays your taxes and places a lien on your property for the taxes and interest paid. The state must be repaid when certain events occur, such as when you move or transfer ownership or if you don't maintain adequate fire and casualty insurance."
    values={{
      em: <em>property tax deferral</em>,
      strong: <strong>doesn't change the amount of your property taxes</strong>,
    }}
  />
);
export const whatDeferralAnswer2 = (
  <FormattedMessage
    id="faq_whatDeferralAnswer2"
    defaultMessage="Initially, you can apply for a deferral and an exemption at the same time, but you need to reapply for a property tax deferral each year."
  />
);

export const doQualify = (
  <FormattedMessage
    id="faq_doQualify"
    defaultMessage="Do I qualify for a property tax exemption?"
  />
);
export const doQualifyAnswer = (
  <FormattedMessage
    id="faq_doQualifyAnswer"
    defaultMessage="Your qualification for a property tax exemption is based on your ownership, age, and income for the preceding year. For example, a property tax exemption for 2020 is based on 2019
    information. You must meet three requirements to qualify:"
  />
);
export const qualifyPropertyTaxDeferral = (
  <FormattedMessage
    id="faq_qualifyPropertyTaxDeferral"
    defaultMessage="Do I qualify for a senior or disabled property tax deferral?"
  />
);
export const qualifyPropertyTaxDeferralAnswer = (
  <FormattedMessage
    id="faq_qualifyPropertyTaxDeferralAnswer"
    defaultMessage="If your taxes are delinquent or you need help paying your unpaid taxes in the current year, you may qualify for a property tax deferral. You must meet the following criteria:"
  />
);
export const qualifyPropertyTaxDeferralAnswer2 = (
  <FormattedMessage
    id="faq_qualifyPropertyTaxDeferralAnswer2"
    defaultMessage="{ownership} You owned and occupied a house, mobile home, condo, or co-op during the year prior to your application."
    values={{ ownership: <strong>Ownership.</strong> }}
  />
);
export const qualifyPropertyTaxDeferralAnswer3 = (
  <FormattedMessage
    id="faq_qualifyPropertyTaxDeferralAnswer3"
    defaultMessage="{age} You are 60 years old or retired because of a disability during the tax year you're applying for. Or your deceased spouse or state-registered domestic partner had an exemption at the time of death, and you were 57 or older when they passed away."
    values={{ age: <strong>Age or disability.</strong> }}
  />
);
export const qualifyPropertyTaxDeferralAnswer4 = (
  <FormattedMessage
    id="faq_qualifyPropertyTaxDeferralAnswer4"
    defaultMessage="{income} Your annual household income was less than $45,000, including Social Security and other sources. (Maximum annual household income for 2019 will be determined in August 2019.)"
    values={{ income: <strong>Income.</strong> }}
  />
);
export const exemptionPropertyTaxChange = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChange"
    defaultMessage="How do my property taxes change with an exemption?"
  />
);
export const exemptionPropertyTaxChangeAnswer1 = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChangeAnswer1"
    defaultMessage="Depending on your household income, you might qualify for an exemption at one of these levels in 2017, 2018, 2019, and/or 2020:"
  />
);
export const exemptionPropertyTaxChangeAnswer2 = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChangeAnswer2"
    defaultMessage="If your household income is between $35,001 and $40,000, you'll be exempt from voter-approved levies."
  />
);
export const exemptionPropertyTaxChangeAnswer3 = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChangeAnswer3"
    defaultMessage="If your household income is between $30,001 and $35,000, you'll be exempt from voter-approved levies and will receive a reduction in assessed values of 35 percent or $50,000 (whichever is greater, but not more than $70,000). In future tax years, your taxes will be calculated based on the lesser of the current market value or the reduced (frozen) value."
  />
);
export const exemptionPropertyTaxChangeAnswer4 = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChangeAnswer4"
    defaultMessage="If your household income is between $0 and $30,000, you'll be exempt from voter-approved levies and will receive a reduction in assessed value of 60 percent or $60,000 (whichever is greater). In future tax years, your taxes will be calculated based on the lesser of the current market value or the reduced (frozen) value."
  />
);
export const exemptionPropertyTaxChangeAnswer5 = (
  <FormattedMessage
    id="faq_exemptionPropertyTaxChangeAnswer5"
    defaultMessage="For the 2019 tax year, household income levels for each level of exemption will be determined in August 2019. "
  />
);
export const whatIsFrozenValue = (
  <FormattedMessage
    id="faq_whatIsFrozenValue"
    defaultMessage="What is a frozen value?"
  />
);
export const whatIsFrozenValueAnswer = (
  <FormattedMessage
    id="faq_whatIsFrozenValueAnswer"
    defaultMessage="When you qualify for a property tax exemption, your home's market value as of January 1 in the year you qualify becomes the {value}. As long as you qualify for the exemption, your property taxes will be based on the lesser of your frozen value or the current market value. "
    values={{ value: <em>frozen value</em> }}
  />
);
export const canPropertyValueIncrease = (
  <FormattedMessage
    id="faq_canPropertyValueIncrease"
    defaultMessage="Can my property value increase beyond the frozen value? "
  />
);
export const canPropertyValueIncreaseAnswer1 = (
  <FormattedMessage
    id="faq_canPropertyValueIncreaseAnswer1"
    defaultMessage="Yes. If you remodel your home (for example, you add a bathroom) the value of the remodel is added to your frozen value. This new frozen value becomes the basis for future property taxes."
  />
);
export const howPropertyTaxesChangeWithDeferral = (
  <FormattedMessage
    id="faq_howPropertyTaxesChangeWithDeferral"
    defaultMessage="How do my property taxes change with a deferral?"
  />
);
export const howPropertyTaxesChangeWithDeferralAnswer = (
  <FormattedMessage
    id="faq_howPropertyTaxesChangeWithDeferralAnswer"
    defaultMessage="There's no change to the amount of property taxes you owe with a deferral, but you defer payment until a later time."
  />
);
export const applying = (
  <FormattedMessage id="faq_applying" defaultMessage="Applying" />
);
export const howApplyExemption = (
  <FormattedMessage
    id="faq_howApplyPropertyTaxExemption"
    defaultMessage="How do I apply for a property tax exemption?"
  />
);
export const howApplyExemptionAnswer = (
  <FormattedMessage
    id="faq_howApplyExemptionAnswer"
    defaultMessage="To apply for a property tax exemption, use the <a>online Senior Exemption application</a>."
    values={{
      a: (...chunks) => (
        <a
          href="https://kingcounty.gov/depts/assessor/Forms.aspx"
          target="_blank"
          rel="noopener noreferrer"
          data-new-window="true"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

export const howApplyExemptionAnswer2 = (
  <FormattedMessage
    id="faq_howApplyExemptionAnswer2"
    defaultMessage="Or print the <a>Senior Citizen and Disabled Persons Reduction in Property Taxes
          Application</a> (PDF), complete it, and then mail it with the required documents to:"
    values={{
      a: (...chunks) => (
        <a
          href="https://www.kingcounty.gov/depts/assessor/~/media/depts/assessor/documents/Forms/ExemptionForms/Senior/SrCitDisApp.ashx"
          target="_blank"
          rel="noopener noreferrer"
          data-new-window="true"
        >
          {chunks}
        </a>
      ),
    }}
  />
);
export const mailingAddress = (
  <FormattedMessage
    id="faq_mailingAddress"
    defaultMessage="Department of Assessments{break}King County Administration Building{break}500 4th Ave., Room 740{break}Seattle, WA 98104-2384"
    values={{ break: <br></br> }}
  />
);
export const haveQuestions = (
  <FormattedMessage
    id="faq_haveQuestions"
    defaultMessage="If you have questions, email <a>Exemptions.Assessments@kingcounty.gov</a> or call 206-296-3920."
    values={{
      a: (...chunks) => (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {chunks}
        </a>
      ),
    }}
  />
);

export const howApplyPropertyTaxDeferral = (
  <FormattedMessage
    id="faq_howApplyPropertyTaxDeferral"
    defaultMessage="How do I apply for a property tax deferral?"
  />
);
export const howApplyPropertyTaxDeferralAnswer1 = (
  <FormattedMessage
    id="faq_howApplyPropertyTaxDeferralAnswer1"
    defaultMessage="To apply for a property tax deferral, print the <a>Deferral Application for Senior Citizens and Disabled Persons</a> (DOC), complete it, and then mail it with the required documents to:"
    values={{
      a: (...chunks) => (
        <a
          href="https://dor.wa.gov/sites/default/files/legacy/Docs/forms/proptx/forms/srcitdispersdeclaredefer.doc"
          target="_blank"
          rel="noopener noreferrer"
          data-new-window="true"
        >
          {chunks}
        </a>
      ),
    }}
  />
);
export const whatDocuments = (
  <FormattedMessage
    id="faq_whatDocuments"
    defaultMessage="What documents will I need when I apply?"
  />
);
export const whatDocsAnswer = (
  <FormattedMessage
    id="faq_whatDocsAnswer"
    defaultMessage="You'll need documents and information about your age, property, and income. For details, refer to the Documents you’ll need checklist. It’s a good idea to print this list to help you gather all the documents and information before you get started."
  />
);
export const whatDocsAnswer2 = (
  <FormattedMessage
    id="faq_whatDocsAnswer2"
    defaultMessage="If you’re applying online, cover up all Social Security and account numbers as you attach the images of the original documents. If you mail copies of your documents, you can cover up these numbers with a thick felt pen or tape."
  />
);
export const howToGetAnswers = (
  <FormattedMessage
    id="faq_howToGetAnswers"
    defaultMessage="What if I have additional questions about applying?"
  />
);
export const howToGetAnswersAns = (
  <FormattedMessage
    id="faq_howToGetAnswersAns"
    defaultMessage="Our experienced exemption staff can help you complete the forms and can answer additional questions about which documents you should provide. Email <a>Exemptions.Assessments@kingcounty.gov</a> or call 206-296-3920."
    values={{
      a: (...chunks) => (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {chunks}
        </a>
      ),
    }}
  />
);
export const happensAfterApply = (
  <FormattedMessage
    id="faq_happensAfterApply"
    defaultMessage="What happens after I apply? "
  />
);
export const happensAfterApplyAnswer = (
  <FormattedMessage
    id="faq_happensAfterApplyAnswer"
    defaultMessage="We’ll let you know we’ve received your application. After we've reviewed it, we'll notify you of your exemption status or we’ll ask you for additional information."
  />
);
export const applyTaxesDelinquent = (
  <FormattedMessage
    id="faq_applyTaxesDelinquent"
    defaultMessage="Can I apply if my property taxes are delinquent?"
  />
);
export const applyTaxesDelinquentAnswer = (
  <FormattedMessage
    id="faq_applyTaxesDelinquentAnswer"
    defaultMessage="Yes. Please contact our office so we can help you.  
    Email <a>Exemptions.Assessments@kingcounty.gov</a> or call 206-296-3920."
    values={{
      a: (...chunks) => (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {chunks}
        </a>
      ),
    }}
  />
);
export const financialInfo = (
  <FormattedMessage id="faq_financialInfo" defaultMessage="Financial info" />
);
export const whyNeedTaxReturn = (
  <FormattedMessage
    id="faq_whyNeedTaxReturn"
    defaultMessage="Why do you need my federal tax return?"
  />
);
export const whyNeedTaxReturnAnswer = (
  <FormattedMessage
    id="faq_whyNeedTaxReturnAnswer"
    defaultMessage="You'll use information from your federal tax return to complete your application. We'll use it as a starting point to verify your income and to help us determine your exemption level."
  />
);
export const whatDontFileFederalTaxReturn = (
  <FormattedMessage
    id="faq_whatDontFileFederalTaxReturn"
    defaultMessage="What if I don't file a federal tax return?"
  />
);
export const whatDontFileFederalTaxReturnAnswer = (
  <FormattedMessage
    id="faq_whatDontFileFederalTaxReturnAnswer"
    defaultMessage="We'll ask for other documents, such as bank statements, Form 1099s, reverse mortgage statements, Social Security statements, or other year-end statements, to verify your income and how you pay your household expenses."
  />
);
export const whatIncludedHouseholdIncomeProgram1 = (
  <FormattedMessage
    id="faq_whatIncludedHouseholdIncomeProgram1"
    defaultMessage="What is included in my household income for the program?"
  />
);
export const whatIncludedHouseholdIncomeProgramAnswer11 = (
  <FormattedMessage
    id="faq_whatIncludedHouseholdIncomeProgramAnswer11"
    defaultMessage="To calculate your household income for the program, we include income received by you, your spouse, a domestic partner, and/or a co-owner who lives with you."
  />
);
export const whatIncludedHouseholdIncomeProgramAnswer22 = (
  <FormattedMessage
    id="faq_whatIncludedHouseholdIncomeProgramAnswer22"
    defaultMessage="The Documents you’ll need checklist contains a more detailed, printable list of income that’s included in your household income, along with documents and information you’ll need to gather before you apply."
  />
);
export const whatIfLowIncome = (
  <FormattedMessage
    id="faq_whatIfLowIncome"
    defaultMessage="What if I have no income or extremely low income?"
  />
);
export const whatIfLowIncomeAnswer = (
  <FormattedMessage
    id="faq_whatIfLowIncomeAnswer"
    defaultMessage="Even people without income or with extremely low income have expenses and some means of covering the costs of daily living, such as food, utilities, and transportation. Applications that show no source of funding are denied."
  />
);
export const whatIfLowIncomeAnswer2 = (
  <FormattedMessage
    id="faq_whatIfLowIncomeAnswer2"
    defaultMessage="You might use funds from reverse mortgages, gifts from children, or savings accounts to cover your household expenses. Although we don't count these income sources toward your household income, you need to report the sources and the amounts on your application."
  />
);
export const whatIfCoOwnHome = (
  <FormattedMessage
    id="faq_whatIfCoOwnHome"
    defaultMessage="What if I co-own my home?"
  />
);
export const whatIfCoOwnHomeAnswer1 = (
  <FormattedMessage
    id="faq_whatIfCoOwnHomeAnswer1"
    defaultMessage="You might qualify for an exemption as a co-owner. However, if you share ownership of your home, your exemption might be adjusted to reflect your share of the ownership."
  />
);
export const whatIfCoOwnHomeAnswer2 = (
  <FormattedMessage
    id="faq_whatIfCoOwnHomeAnswer2"
    defaultMessage="For example, if you and your two children each own one-third of the home, your exemption will be applied to one-third of the value—the portion that you own. Your two children will continue to owe full property taxes on the percentage that they own."
  />
);
export const whatIfCoOwnHomeAnswer3 = (
  <FormattedMessage
    id="faq_whatIfCoOwnHomeAnswer3"
    defaultMessage="If a co-owner lives with you, their income is included in the total household income on the exemption application."
  />
);
export const whyIsCalculatedIncome = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncome"
    defaultMessage="Why is my calculated household income more than the adjusted gross income (AGI) on my federal tax return? "
  />
);
export const whyIsCalculatedIncomeAnswer = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncomeAnswer"
    defaultMessage="To calculate your household income for the exemption program, we count income sources that might not be considered taxable by the IRS. For example:"
  />
);
export const whyIsCalculatedIncomeAnswer2 = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncomeAnswer2"
    defaultMessage="The IRS might partially or completely exclude Social Security from your taxable income. For the exemption program, the entire amount you receive via Social Security check or direct deposit is counted as income."
  />
);
export const whyIsCalculatedIncomeAnswer3 = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncomeAnswer3"
    defaultMessage="The IRS allows you to deduct capital losses from capital gains. For the exemption program, all capital gains are counted as income, regardless of losses."
  />
);
export const whyIsCalculatedIncomeAnswer4 = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncomeAnswer4"
    defaultMessage="Income earned by a spouse, a registered domestic partner, or a co-owner who lives with you is counted toward your household income for the exemption program, although it might not be included on your tax return."
  />
);
export const whyIsCalculatedIncomeAnswer5 = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncome5"
    defaultMessage="The IRS allows you to deduct depreciation from rental and business income. For the exemption program, these income sources are counted toward your household income in their entirety."
  />
);
export const whyIsCalculatedIncomeAnswer6 = (
  <FormattedMessage
    id="faq_whyIsCalculatedIncomeAnswer6"
    defaultMessage="Any of these situations could cause your calculated household income to exceed your federal AGI."
  />
);
export const renewal = (
  <FormattedMessage id="faq_renewal" defaultMessage="Renewal" />
);
export const willNeedReapplyAfterReceiveExemption = (
  <FormattedMessage
    id="faq_willNeedReapplyAfterReceiveExemption"
    defaultMessage="Will I need to reapply after I receive an exemption? "
  />
);
export const willNeedReapplyAfterReceiveExemptionAnswer = (
  <FormattedMessage
    id="faq_willNeedReapplyAfterReceiveExemptionAnswer"
    defaultMessage="We'll send you a renewal application when it's time for you to reapply—at least every four years. You'll need to provide updated information and documentation with your renewal application."
  />
);
export const whatIfDontRespondRenewal = (
  <FormattedMessage
    id="faq_whatIfDontRespondRenewal"
    defaultMessage="What happens if I don't respond to a renewal request?"
  />
);
export const whatIfDontRespondRenewalAnswer = (
  <FormattedMessage
    id="faq_whatIfDontRespondRenewalAnswer"
    defaultMessage="If you don't return your renewal form and the necessary documents by the date requested, you'll be removed from the program and you'll begin paying full property taxes based on your property's current market value."
  />
);
export const whatIfDontSendDocumentsRenewal = (
  <FormattedMessage
    id="faq_whatIfDontSendDocumentsRenewal"
    defaultMessage="What if I don't send documents with my renewal request? "
  />
);
export const whatIfDontSendDocumentsRenewalAnswer = (
  <FormattedMessage
    id="faq_whatIfDontSendDocumentsRenewalAnswer"
    defaultMessage="If you don't send documents with your renewal request, we'll ask you to provide the missing information. This will delay processing of your application. If you don't respond, you'll be removed from the program. At that time, you'll begin paying full property taxes based on your property's current market value."
  />
);
export const changes = (
  <FormattedMessage id="faq_changes" defaultMessage="Changes" />
);
export const whatIncomeChanges = (
  <FormattedMessage
    id="faq_whatIncomeChanges"
    defaultMessage="What if my income or living situation changes?"
  />
);
export const whatIncomeChangesAnswer = (
  <FormattedMessage
    id="faq_whatIncomeChangesAnswer"
    defaultMessage="If your income or living situation changes in a way that might affect your qualification for a property tax exemption or your exemption level, it's your responsibility to contact us. Email <a>Exemptions.Assessments@kingcounty.gov</a> or call 206-296-3920."
    values={{
      a: (...chunks) => (
        <a
          href="mailto:Exemptions.Assessments@kingcounty.gov"
          data-rel="external"
        >
          {chunks}
        </a>
      ),
    }}
  />
);
export const whatIncomeChangesAnswer2 = (
  <FormattedMessage
    id="faq_whatIncomeChangesAnswer2"
    defaultMessage="Check the <a>exemption income table</a> earlier on this page for specific program requirements."
    values={{
      a: (...chunks) => <a href="#exemptionChange">{chunks}</a>,
    }}
  />
);
export const whatIncomeChangesAnswer3 = (
  <FormattedMessage
    id="faq_whatIncomeChangesAnswer3"
    defaultMessage="Common changes that might affect your exemption include reaching 70½ years of age and beginning to draw on your IRA or annuity, loss of a spouse or partner, or donation of some or all of your property to someone else."
  />
);
export const whatIncomeChangesAnswer4 = (
  <FormattedMessage
    id="faq_whatIncomeChangesAnswer4"
    defaultMessage="If your income increases beyond the property tax exemption maximum, you'll be removed from the program for the year in which you don't qualify. You'll pay taxes on your property's current market value for that year."
  />
);
export const whatIncomeChangesAnswer5 = (
  <FormattedMessage
    id="faq_whatIncomeChangesAnswer5"
    defaultMessage="If you are removed from the exemption program for one year, you might be reinstated at your original frozen value. In that event, you'll pay the reduced property taxes again the following year."
  />
);
export const whatIfOverIncome = (
  <FormattedMessage
    id="faq_whatIfOverIncome"
    defaultMessage="What if I'm over the income limit for more than one year? "
  />
);
export const whatIfOverIncomeAnswer = (
  <FormattedMessage
    id="faq_whatIfOverIncomeAnswer"
    defaultMessage="If your income exceeds the property tax exemption maximum for more than one year, you'll be removed from the program and will begin paying your full property taxes, based on your property's current market value."
  />
);
export const whatIfOverIncomeAnswer2 = (
  <FormattedMessage
    id="faq_whatIfOverIncomeAnswer2"
    defaultMessage="If your income drops below the exemption maximum in the future, you can reapply and requalify for the program. Your new property tax exemption will be based on a new frozen value."
  />
);
export const whatIfIMove = (
  <FormattedMessage id="faq_whatIfIMove" defaultMessage="What if I move? " />
);
export const whatIfIMoveAnswer = (
  <FormattedMessage
    id="faq_whatIfIMoveAnswer"
    defaultMessage="If you move to a new home, condo, or mobile home that you have purchased, you might be able to transfer your exemption to the new property. You'll need to apply for the exemption at the new property."
  />
);
export const ifPartnerHadExemptionTransfer = (
  <FormattedMessage
    id="faq_ifPartnerHadExemptionTransfer"
    defaultMessage="If my deceased spouse or registered domestic partner had an exemption, does it transfer to me?"
  />
);
export const ifPartnerHadExemptionTransferAnswer = (
  <FormattedMessage
    id="faq_ifPartnerHadExemptionTransferAnswer"
    defaultMessage="If your deceased spouse or registered domestic partner had a senior or disabled property tax exemption at the time of death, and you were 57 or older that year, you might be eligible for a property tax exemption."
  />
);
