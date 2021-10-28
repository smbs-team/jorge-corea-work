//-----------------------------------------------------------------------
// <copyright file="income.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import './IncomeHelp.css';
import redactionHelpImage from '../../assets/redaction-help-example.jpg';
import * as fm from './FormatTexts';

export default class Income extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="main no-focus writing">
        <div id="copyDiv" style={{ textAlign: 'left' }}>
          <h1 className="header2">
            {fm.mainHeader}
            <div className="body-large">{fm.mainHeaderDesc}</div>
          </h1>
          <div className="body">
            <h3 className="header3 tall">{fm.aboutYou}</h3>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.proofOfIdentity}</div>
                <div className="area-1">{fm.proofOfIdentityDesc}</div>
              </div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.domesticPartnerAgreement}</div>
                <div className="area-1">{fm.domesticPartnerAgreementDesc}</div>
              </div>{' '}
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.proofOfDisability}</div>
                <div className="area-1">
                  {fm.proofOfDisabilityTitle}
                  <ul className="space-top-8">
                    <li>{fm.proofOfDisabilityDesc}</li>
                    <li>{fm.proofOfDisabilityDesc2}</li>
                    <li>{fm.proofOfDisabilityDesc3}</li>
                  </ul>
                </div>
              </div>
            </div>
            <h3 className="header3 tall">{fm.aboutYourProperty}</h3>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.moveInYear1}</div>
                <div className="area-1">{fm.moveInYear1Desc}</div>
              </div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.trustDocuments1}</div>
                <div className="area-1">{fm.trustDocuments2}</div>
              </div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.addresses}</div>
                <div className="area-1">{fm.addressesDesc}</div>
              </div>
            </div>
            <h3 className="header3 tall print-break">{fm.aboutYourFinances}</h3>
            <div className="body">{fm.aboutYourFinancesDesc}</div>
            <div className="div-table-side">
              <div className="div-table-row">
                <div className="div-table-cell div-table-heading table-side-head">
                  {fm.applicationYear}
                </div>
                <div className="div-table-cell">{new Date().getFullYear()}</div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 1}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 2}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 3}
                </div>
              </div>
              <div className="div-table-row">
                <div className="div-table-cell div-table-heading table-side-head">
                  {fm.useInfoFrom}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 1}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 2}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 3}
                </div>
                <div className="div-table-cell">
                  {new Date().getFullYear() - 4}
                </div>
              </div>
            </div>
            <div className="body tall">{fm.incomeDocumentsTitle}</div>
            <div className="checklist">
              <div className="checkbox"></div>

              <div className="item-title">{fm.incomeDocumentsWho1}</div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div className="item-title">{fm.incomeDocumentsWho2}</div>
            </div>
            <h3 className="header4 tall">{fm.incomeDocuments}</h3>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.generalIncome0}</div>
                <div className="area-1">
                  <p>{fm.generalIncome1}</p>
                  <p>{fm.generalIncome2}</p>
                </div>
              </div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.otherIncome}</div>
                <div className="area-1">{fm.otherIncomeDesc}</div>
              </div>
            </div>
            <div className="checklist">
              <div className="checkbox"></div>
              <div>
                <div className="item-title">{fm.incomeFromOtherCountries}</div>
                <div className="area-1">{fm.incomeFromOtherCountriesDesc}</div>
              </div>
            </div>
          </div>
          <h3 className="header4 tall print-break">{fm.expenseDocuments}</h3>
          <div className="checklist">
            <div className="checkbox"></div>
            <div>
              <div className="item-title">{fm.qualifiedExpenses}</div>
              <div className="area-1">
                {fm.qualifiedExpenses1}
                <ul>
                  <li>{fm.qualifiedExpensesDesc}</li>
                  <li>{fm.qualifiedExpensesDesc2}</li>
                  <li>{fm.qualifiedExpensesDesc3}</li>
                  <li>
                    {fm.qualifiedExpensesDesc4}
                    <br />
                    {fm.qualifiedExpensesDesc5}
                  </li>
                </ul>
              </div>
            </div>
          </div>
          <h3 className="header4 tall"> {fm.protectingPersonalInfo}</h3>
          <div className="body"> {fm.protectingPersonalInfoDesc1}</div>
          <img
            src={redactionHelpImage}
            alt={fm.coverUpPersonalInfo}
            className="space-top-4"
          ></img>
        </div>
      </div>
    );
  }
}
