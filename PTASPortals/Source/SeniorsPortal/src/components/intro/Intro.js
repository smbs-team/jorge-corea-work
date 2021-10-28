/*
//-----------------------------------------------------------------------
// <copyright file="Intro.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
*/
/* eslint-disable no-useless-constructor */

import React from 'react';
import './Intro.css';
import * as fm from './FormatTexts';
import { Link } from 'react-router-dom';
import Grid from '@material-ui/core/Grid';
import VimeoPlayer from '../common/vimeo-player/VimeoPlayer';
import iconDocuments from '../../assets/icon-documents.svg';
import ContentViewer from '../common/content-viewer/ContentViewer';
import CustomContentDialog from '../common/dialog-component/CustomContentDialog';
import Faq from '../faq/Faq';
import Income from '../income-help/Income';
import Banner from '../common/Banner';
import { renderIf } from '../../lib/helpers/util';

class Intro extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showContentDialog: false,
      showContentDialogOption: false,
      pdfUrl:
        'https://www.kingcounty.gov/depts/assessor/~/media/depts/assessor/documents/Forms/ExemptionForms/Senior/SrCitDisApp.pdf',
      siteUrl: 'https://www.kingcounty.gov/depts/assessor/Forms.aspx',
      showBanner: false,
    };
  }

  componentDidMount() {
    this.showBanner();
  }

  showBanner = () => {
    if (process.env.REACT_APP_SHOW_INTRO_BANNER === 'true') {
      const showUntil = process.env.REACT_APP_SHOW_INTRO_BANNER_UNTIL;
      // if property empty, assume there is not expiration date for the banner.
      const showUntilDate =
        showUntil && showUntil !== '' ? new Date(showUntil) : null;
      const showBanner = showUntilDate ? showUntilDate > new Date() : true;
      this.setState({ showBanner });
    }
  };

  toggleDialog = option => {
    this.setState({ showContentDialogOption: option });
    if (this.state.showContentDialog) {
      this.setState({ showContentDialog: false });
    } else {
      this.setState({ showContentDialog: true });
    }
  };

  render() {
    return (
      <div className="main-area">
        <CustomContentDialog
          showContentDialog={this.state.showContentDialog}
          toggleDialog={this.toggleDialog}
          hidePrint={
            this.state.showContentDialogOption === 'www' ? true : false
          }
        >
          {this.state.showContentDialogOption === 'faq' && (
            <Faq ref={el => (this.componentRef = el)} />
          )}
          {this.state.showContentDialogOption === 'incomehelp' && (
            <Income ref={el => (this.componentRef = el)} />
          )}
          {this.state.showContentDialogOption === 'pdf' && (
            <ContentViewer
              ref={el => (this.componentRef = el)}
              pdfUrl={this.state.pdfUrl}
            ></ContentViewer>
          )}
          {this.state.showContentDialogOption === 'www' && (
            <ContentViewer siteUrl={this.state.siteUrl}></ContentViewer>
          )}
        </CustomContentDialog>

        <div className="nav-in-page body-large">
          <div className="nav-in-page-item">
            <b>{fm.nipHeader}</b>
          </div>
          <div className="nav-in-page-item">
            <a href="#overview">{fm.nipOverview}</a>
          </div>
          <div className="nav-in-page-item">
            <a href="#videos">{fm.nipHelpVideos}</a>
          </div>
          <div className="nav-in-page-item">
            <a href="#help">{fm.nipGettingHelp}</a>
          </div>
          <div className="nav-in-page-item">
            <a href="#qualification">{fm.nipQualDetails}</a>
          </div>
          <div className="nav-in-page-item">
            <a href="#application">{fm.nipApplicationDetails}</a>
          </div>
          <div className="nav-in-page-item">
            <a href="#more">{fm.nipMoreInfo}</a>
          </div>
          <div className="nav-in-page-item">
            <Link to="/home" className="heavy">
              {fm.nipApplyNow}
            </Link>
          </div>
        </div>

        <div className="page">
          {/* <SideMenu /> */}
          <div className="content-wrapper">
            <div className="content writing">
              {renderIf(
                this.state.showBanner,
                <Banner
                  level={process.env.REACT_APP_INTRO_BANNER_LEVEL}
                  message={fm.introBannerContent}
                />
              )}

              <h1 className="header1">
                <a id="overview"></a>
                {fm.mainHeading}
                <div className="subhead space-top-8">{fm.mainHeadingSub}</div>
              </h1>
              <div className="quick-info-wrapper space-top-32">
                <div className="quick-info-item">
                  <div className="quick-info-number center-block">1</div>
                  <div className="quick-info-title">{fm.quickInfo1}</div>
                  <div className="body-large space-top-8">
                    {fm.bqHeader}
                    <ul>
                      <li>{fm.bqItem1}</li>
                      <li>{fm.bqItem2}</li>
                      <li>{fm.bqItem3}</li>
                    </ul>
                    <div>{fm.readQualDetail}</div>
                  </div>
                </div>
                <div className="quick-info-item line-sides">
                  <div className="quick-info-number center-block">2</div>
                  <div className="quick-info-title">{fm.quickInfo2}</div>
                  <div className="body-large space-top-8 flex">
                    <img
                      src={iconDocuments}
                      alt="Documents"
                      className="quick-info-icon"
                    ></img>
                    {fm.quickInfo2Content1}
                  </div>
                  <a
                    style={{ cursor: 'pointer' }}
                    onClick={() => this.toggleDialog('incomehelp')}
                    href="#documents_popup"
                    className="button-link"
                  >
                    <button className="button secondary interactive">
                      {fm.quickInfo2BtnText}
                    </button>
                  </a>
                  <div className="body-large center-text space-top-16">
                    {fm.quickInfo2Content2}
                  </div>
                  <div className="body-large center-text space-top-8">
                    {fm.quickInfo2Content3}
                    <br /> 206-296-3920.
                  </div>
                </div>
                <div className="quick-info-item">
                  <div className="quick-info-number center-block">3</div>
                  <div className="quick-info-title">{fm.quickInfo3}</div>
                  <div className="body-large center-text space-top-8">
                    {fm.quickInfo3Content1}
                  </div>
                  <Link
                    to="/home"
                    href="#signin"
                    className="button-link space-top-16"
                  >
                    <button
                      className="button primary interactive"
                      data-testid="signIn"
                    >
                      {fm.applyButton}
                    </button>
                  </Link>
                  <div className="body-large center-text nogap space-top-32">
                    {fm.quickInfo3Content2}
                  </div>
                  <div className="body-large center-text">
                    <Link to="/home" href="#signin">
                      {fm.applyLink}
                    </Link>
                  </div>
                </div>
              </div>
              <div id="area4">
                <a id="videos"></a>
                <h2 className="header3">{fm.helpFullVideos}</h2>
                <div id="areaVimeo" className="space-top-16">
                  <Grid
                    container
                    spacing={20}
                    style={{
                      border: 'solid 1px',
                    }}
                  >
                    <Grid xs={12} sm={12} md={12}>
                      <VimeoPlayer
                        height={'360'}
                        videos={[
                          { url: '382824142' },
                          { url: '382824153' },
                          { url: '382824157' },
                          { url: '385316103' },
                          { url: '382824179' },
                          { url: '382824186' },
                          { url: '382824199' },
                          { url: '382824211' },
                          { url: '382824222' },
                          { url: '382824232' },
                          { url: '382824246' },
                          { url: '382824273' },
                        ]}
                        //isPopup
                        showList
                        //autoplay
                      />
                    </Grid>
                  </Grid>
                </div>
                <a id="help"></a>
                <h2 className="header3 tall">{fm.gettingHelp}</h2>
                <p className="body-large">{fm.gettingHelpContent}</p>
                <div className="help-contact-block inverse header5 nogap">
                  {fm.questions}
                </div>
                <a id="qualification"></a>
                <h2 className="header3 tall">{fm.qualDetailsHeader}</h2>
                <div className="body-large">
                  <p>{fm.qualDetailsContent}</p>
                  <p>
                    <b>{fm.ownershipOccHeader}</b>
                    <ul>
                      <li>{fm.ownershipOccItem1}</li>
                      <li>{fm.ownershipOccItem2}</li>
                    </ul>
                  </p>
                  <p>
                    <b>{fm.ageOrDis}</b>
                    <ul>
                      <li>{fm.ageOrDisItem1}</li>
                      <li>{fm.ageOrDisItem2}</li>
                      <li>{fm.ageOrDisItem3}</li>
                    </ul>
                  </p>
                  <p>
                    <b>{fm.incomeHeader}</b>
                    <ul>
                      <li>{fm.incomeItem1}</li>
                      <li>{fm.incomeItem2}</li>
                    </ul>
                  </p>
                  <p>{fm.householdIncome}</p>
                </div>
                <a id="application"></a>
                <h2 className="header3 tall">{fm.appDetail}</h2>
                <h3 className="header5">{fm.changes20202021Header}</h3>
                <div className="body-large">{fm.changes20202021Content}</div>
                <h3 className="header5 tall">{fm.applyingMoreOneYearHeader}</h3>
                <p className="body-large">
                  {fm.applyingMoreOneYearContent}
                  <ul>
                    <li>{fm.applyingMoreOneYearItem1}</li>
                    <li>{fm.applyingMoreOneYearItem2}</li>
                    <li>{fm.applyingMoreOneYearItem3}</li>
                  </ul>
                </p>
                {/*                 <h3 className="header5 tall">Applying by mail</h3>
                <p className="body-large">
                  If you don't want to apply online, go to the{' '}
                  <a
                    to="https://www.kingcounty.gov/depts/assessor/Forms.aspx"
                    href="#paperforms_popup"
                    className="link-popup"
                    style={{ cursor: 'pointer' }}
                    onClick={() => this.toggleDialog('www')}
                  >
                    King County Forms &amp; Exemptions
                  </a>
                  &nbsp; page, find and select the "senior citizen and disabled
                  exemption" form for the year you want to apply for, and print
                  it. Then complete the application form and mail it with the
                  required documents to:
                  <br />
                  <br />
                  {fm.departmentAssessments}
                  <br />
                  {fm.building}
                  <br />
                  {fm.address1}
                  <br />
                  {fm.address2}
                </p>
 */}{' '}
                <h3 className="header5 tall">{fm.whatExpect}</h3>
                <div className="body-large">{fm.whatExpectBody}</div>
                <a id="more"></a>
                <h2 className="header3 tall">{fm.stHaveQuest}</h2>
                <p className="body-large">
                  <ul>
                    <li>{fm.stHaveQuestItem1}</li>
                    <li>{fm.stHaveQuestItem2}</li>
                  </ul>
                  {fm.stHaveQuestContent(() => this.toggleDialog('faq'))}
                </p>
              </div>
              {/*  <a
                style={{ cursor: 'pointer' }}
                onClick={() => this.toggleDialog('www')}
              >
                MoreInfo
              </a>{' '}
              */}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Intro;
