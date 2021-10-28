import React from 'react';
import './Faq.css';
import * as fm from './FormatTexts';

export default class Faq extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="main writing">
        <div style={{ textAlign: 'left' }}>
          <div id="copyDiv">
            <h1 className="header2">{fm.frequentlyAsked}</h1>
            <h2 className="body-large">{fm.description}</h2>
          </div>
          <h3 className="header3 tall">{fm.programBasics}</h3>
          <div className="qa-unit">
            <h4 className="qa header4">
              {fm.question}
              <div className="question-text">
                {fm.propertyTaxReliefAvailable}
              </div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">
                  {fm.propertyTaxReliefAvailableAnswer}
                </p>
                <p className="body-large">{fm.propertyTaxesExemption}</p>
                <p className="body-large">{fm.propertyTaxesDeferral}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatExemption}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatExemptionAnswer1}</p>
                <p className="body-large">{fm.whatExemptionAnswer2}</p>
                <p className="body-large">{fm.whatExemptionAnswer3}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatDeferral}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatDeferralAnswer}</p>
                <p className="body-large">{fm.whatDeferralAnswer2}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.doQualify}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.doQualifyAnswer}</p>
                <p className="body-large">
                  <b>Ownership</b>
                  <ul>
                    <li>
                      You own and occupy your house, mobile home, condo, or
                      co-op.
                    </li>
                  </ul>
                </p>
                <p className="body-large">
                  <b>Age or disability</b>
                  <ul>
                    <li>You are at least age 62.</li>
                    <li>
                      Or you are veteran with a service-connected disability or
                      disabled.
                    </li>
                    <li>
                      You might also qualify if your deceased spouse or
                      registered domestic partner had an exemption at the time
                      of death and you were at least age 57 that year.
                    </li>
                  </ul>
                </p>
                <p className="body-large">
                  <b>Income</b>
                  <ul>
                    <li>
                      For an exemption on your 2020 property taxes, your
                      household income for 2019 was $58,423 or less.
                    </li>
                    <li>
                      For an exemption on your 2017, 2018, or 2019 property
                      taxes, your household income for 2016, 2017, or 2018 was
                      $40,000 or less.
                    </li>
                  </ul>
                </p>
                <p className="body-large">
                  <i>Household income</i> includes income received by you, by
                  your spouse or domestic partner, and by a co-owner who lives
                  with you. Remember to subtract qualified expenses.
                </p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <a id="exemptionChange"></a>
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.exemptionPropertyTaxChange}
              </div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">
                  {fm.exemptionPropertyTaxChangeAnswer1}
                </p>
                <div class="div-table space-top-16">
                  <div class="div-table-body">
                    <div class="div-table-row div-table-heading">
                      <div class="div-table-cell table-cell-number">
                        2019
                        <br />
                        income
                      </div>
                      <div class="div-table-cell table-cell-number">
                        2016-2018
                        <br />
                        income
                      </div>
                      <div class="div-table-cell">You might qualify for</div>
                    </div>
                    <div class="div-table-row">
                      <div class="div-table-cell table-cell-number">
                        $49,436 to $58,423
                      </div>
                      <div class="div-table-cell table-cell-number">
                        $35,001 to $40,000
                      </div>
                      <div class="div-table-cell">
                        For all exemption levels, your future property taxes
                        will be based on the market value or the frozen value
                        (whichever is lesser) and you’ll be exempt from
                        voter-approved levies (for example, school or bond
                        levies)
                      </div>
                    </div>
                    <div class="div-table-row">
                      <div class="div-table-cell table-cell-number">
                        $40,448 to $49,435
                      </div>
                      <div class="div-table-cell table-cell-number">
                        $30,001 to $35,000
                      </div>
                      <div class="div-table-cell">
                        At this level, you’ll also get a reduction in value of
                        35% or $50,000 (whichever is greater, but not more than
                        $70,000)
                      </div>
                    </div>
                    <div class="div-table-row">
                      <div class="div-table-cell table-cell-number">
                        Up to $40,447
                      </div>
                      <div class="div-table-cell table-cell-number">
                        Up to $30,000
                      </div>
                      <div class="div-table-cell">
                        At this level, you’ll also get a reduction in value of
                        60% or $60,000 (whichever is greater)
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.canPropertyValueIncrease}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>{fm.canPropertyValueIncreaseAnswer1}</div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.qualifyPropertyTaxDeferral}
              </div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <div>
                  <p className="body-large">
                    {fm.qualifyPropertyTaxDeferralAnswer}
                  </p>
                  <p className="body-large">
                    <b>Ownership</b>
                    <ul>
                      <li>
                        You owned and occupied your house, mobile home, condo,
                        or co-op for at least nine months last year.
                      </li>
                    </ul>
                  </p>
                  <p className="body-large">
                    <b>Age or disability</b>
                    <ul>
                      <li>
                        You’ll be at least 60 years old by December 31 of this
                        year or you’re disabled.
                      </li>
                      <li>
                        You might also qualify if your deceased spouse or
                        registered domestic partner had a deferral at the time
                        of death and you were 57 or older when they passed away.
                      </li>
                    </ul>
                  </p>
                  <p className="body-large">
                    <b>Income</b>
                    <ul>
                      <li>
                        For a deferral on your 2020 property taxes, your
                        household income in 2019 was $67,411 or less.
                      </li>
                    </ul>
                  </p>
                  <p className="body-large">
                    <i>Household income</i> includes income received by you, by
                    your spouse or domestic partner, and by a co-owner who lives
                    with you. Remember to subtract qualified expenses.
                  </p>
                </div>
              </div>
            </div>
          </div>
          <h3 className="header3 tall">{fm.applying}</h3>
          <div className="qa-unit">
            <h4 className="qa header4">
              {fm.question}
              <div className="question-text">{fm.howApplyExemption}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <div>
                  <p className="body-large">{fm.howApplyExemptionAnswer}</p>
                  {/*                   <p className="body-large">{fm.howApplyExemptionAnswer2}</p>
                  <p className="body-large">{fm.mailingAddress}</p>
 */}{' '}
                  <p className="body-large">{fm.haveQuestions}</p>
                </div>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatDocuments}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <div>
                  <p className="body-large">{fm.whatDocsAnswer}</p>
                  <p className="body-large">{fm.whatDocsAnswer2}</p>
                </div>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.howApplyPropertyTaxDeferral}
              </div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <div>
                  <p className="body-large">
                    {fm.howApplyPropertyTaxDeferralAnswer1}
                  </p>
                  <p className="body-large">{fm.mailingAddress}</p>
                </div>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.howToGetAnswers}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <p className="body-large">{fm.howToGetAnswersAns}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.happensAfterApply}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <p className="body-large">{fm.happensAfterApplyAnswer}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.applyTaxesDelinquent}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <p className="body-large">{fm.applyTaxesDelinquentAnswer}</p>
              </div>
            </div>
          </div>
          <h3 className="header3 tall">{fm.financialInfo}</h3>
          <div className="qa-unit">
            <h4 className="qa header4">
              {fm.question}
              <div className="question-text">{fm.whyNeedTaxReturn}</div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <p className="body-large">{fm.whyNeedTaxReturnAnswer}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.whatDontFileFederalTaxReturn}
              </div>
            </h4>
            <div>
              <div className="qa">
                {fm.answer}
                <p className="body-large">
                  {fm.whatDontFileFederalTaxReturnAnswer}
                </p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.whatIncludedHouseholdIncomeProgram1}
              </div>
            </h4>

            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">
                  {fm.whatIncludedHouseholdIncomeProgramAnswer11}
                </p>
                <p className="body-large">
                  {fm.whatIncludedHouseholdIncomeProgramAnswer22}
                </p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatIfLowIncome}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatIfLowIncomeAnswer}</p>
                <p className="body-large">{fm.whatIfLowIncomeAnswer2}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatIfCoOwnHome}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatIfCoOwnHomeAnswer1}</p>
                <p className="body-large">{fm.whatIfCoOwnHomeAnswer2}</p>
                <p className="body-large">{fm.whatIfCoOwnHomeAnswer3}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whyIsCalculatedIncome}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whyIsCalculatedIncomeAnswer}</p>
                <ul className="body-large">
                  <li>{fm.whyIsCalculatedIncomeAnswer2}</li>
                  <li>{fm.whyIsCalculatedIncomeAnswer3}</li>
                  <li>{fm.whyIsCalculatedIncomeAnswer4}</li>
                  <li>{fm.whyIsCalculatedIncomeAnswer5}</li>
                </ul>
                <p className="body-large">{fm.whyIsCalculatedIncomeAnswer6}</p>
              </div>
            </div>
          </div>
          <h3 className="header3 tall">{fm.renewal}</h3>
          <div className="qa-unit">
            <h4 className="qa header4">
              {fm.question}
              <div className="question-text">
                {fm.willNeedReapplyAfterReceiveExemption}
              </div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">
                  {fm.willNeedReapplyAfterReceiveExemptionAnswer}
                </p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatIfDontRespondRenewal}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <p className="body-large">{fm.whatIfDontRespondRenewalAnswer}</p>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.whatIfDontSendDocumentsRenewal}
              </div>
            </h4>
            <div className="qa">
              {fm.answer}
              <p className="body-large">
                {fm.whatIfDontSendDocumentsRenewalAnswer}
              </p>
            </div>
          </div>
          <h3 className="header3 tall">{fm.changes}</h3>
          <div className="qa-unit">
            <h4 className="qa header4">
              {fm.question}
              <div className="question-text">{fm.whatIncomeChanges}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatIncomeChangesAnswer}</p>
                <p className="body-large">{fm.whatIncomeChangesAnswer2}</p>
                <p className="body-large">{fm.whatIncomeChangesAnswer3}</p>
                <p className="body-large">{fm.whatIncomeChangesAnswer4}</p>
                <p className="body-large">{fm.whatIncomeChangesAnswer5}</p>
                <p className="body-large">For example:</p>
                <ul>
                  <li>
                    <b>
                      If you apply and qualify for an exemption in 2020 (based
                      on 2019 income)
                    </b>
                    , your property value is frozen, and you receive an
                    exemption on the taxes you pay in 2020.
                  </li>
                  <li>
                    <b>
                      If your income in 2020 exceeds the exemption maximum for
                      that year (for example, due to a one-time IRA
                      disbursement)
                    </b>
                    , you don’t qualify for an exemption in 2021, and you pay
                    your full property taxes.
                  </li>
                  <li>
                    <b>
                      If your 2021 income again falls below the exemption
                      maximum and you reapply in 2022
                    </b>
                    , you receive an exemption on the taxes you pay in 2022,
                    based on your 2020 frozen value.
                  </li>
                </ul>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatIfOverIncome}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <div>
                <p className="body-large">{fm.whatIfOverIncomeAnswer}</p>
                <p className="body-large">{fm.whatIfOverIncomeAnswer2}</p>
              </div>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">{fm.whatIfIMove}</div>
            </h4>
            <div className="qa">
              {fm.answer}
              <p className="body-large">{fm.whatIfIMoveAnswer}</p>
            </div>
          </div>
          <div className="qa-unit">
            <h4 className="qa header4 tall">
              {fm.question}
              <div className="question-text">
                {fm.ifPartnerHadExemptionTransfer}
              </div>
            </h4>
            <div className="qa">
              {fm.answer}
              <p className="body-large">
                {fm.ifPartnerHadExemptionTransferAnswer}
              </p>
            </div>
          </div>
          <div className="qa-unit">
            <h3 className="header3">If you still have questions:</h3>
            <ul>
              <li>
                Email us at{' '}
                <a
                  href="mailto:Exemptions.Assessments@kingcounty.gov"
                  data-rel="external"
                >
                  Exemptions.Assessments@kingcounty.gov
                </a>
              </li>
              <li>Call us at 206-296-3920</li>
              <li>
                Review the related Washington state code in{' '}
                <a
                  href="https://apps.leg.wa.gov/WAC/default.aspx?cite=458-16A"
                  target="blank"
                >
                  Chapter 458-16A WAC
                </a>
                : Property tax—exemptions—homes for the aging, senior citizens
                and disabled persons
              </li>
            </ul>
          </div>
        </div>
      </div>
    );
  }
}
