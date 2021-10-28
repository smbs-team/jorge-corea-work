//-----------------------------------------------------------------------
// <copyright file="Home.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { Component } from 'react';
import './Home.css';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { AuthConsumer, AuthContext } from '../../contexts/AuthContext';
import CustomButton from '../common/CustomButton';

import rightArrow from '../../assets/downArrowWhite.png';
import Grid from '@material-ui/core/Grid';
import { FormattedMessage } from 'react-intl';

import VimeoCard from '../common/vimeo-player/VimeoCard';
import {
  arrayNullOrEmpty,
  renderIf,
  checkClientBrowser,
} from '../../lib/helpers/util';
import {
  CollectionProvider,
  CollectionConsumer,
} from '../../contexts/CollectionContext';
import { Redirect } from 'react-router-dom';

import SeniorAppCard from './SeniorAppCard';
import LoadingOverlay from 'react-loading-overlay';
import { createImageFromCurrentPage } from '../../lib/helpers/file-manipulation';
import IconButton from '@material-ui/core/IconButton';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import ClearIcon from '@material-ui/icons/Clear';
import MessageBox from '../common/MessageBox';
import * as fm from './FormatTexts';
import { Container } from '@material-ui/core';

class Home extends Component {
  constructor(props) {
    super(props);
    this.state = {
      seniorApplication: null,
      seniorApp: null,
      allowRedirect: false,
    };
  }

  componentDidMount = async () => {
    await this.context.currentUser();
    this.determinateRedirection();
  };

  determinateRedirection = async () => {
    let incomingResponse = this.props.location.search;

    if (incomingResponse.includes('signing_complete')) {
      //this.afterRedirectedFromDocuSign(incomingResponse);
    } else if (this.context.contact && this.context.seniorApps.length === 0) {
      // Allow automatic redirect to My Info when there is no existing senior application
      await this.context.createSeniorApp(this.context.contact.contactid);
      this.setState({ allowRedirect: true });
    } else if (
      // Allow automatic redirect to My Info when there is only one Undefined Application.
      this.context.contact &&
      this.context.seniorApps.length === 1 &&
      this.context.seniorApps[0].statuscode === 668020012
    ) {
      this.setState({ allowRedirect: true });
    }
  };

  render = () => {
    if (this.context.seniorApps.length === 1 && this.state.allowRedirect) {
      localStorage.setItem(
        `${this.context.seniorApps[0].seapplicationid}_seapplicationid`,
        this.context.seniorApps[0].seapplicationid
      );
    }

    return (
      <AuthConsumer>
        {({
          seniorApps,
          setSeniorApp,
          showLoading,
          loadingFormatter,
          contact,
          createSeniorApp,
          seniorAppsDetails,
          redirectToSeniors,
          showSubmitDoneConfirmation,
          onShowSubmitDoneConfirmation,
          showSubmitFailedConfirmation,
          onShowSubmitFailedConfirmation,
        }) => (
          <CollectionConsumer>
            {value => (
              <React.Fragment>
                {/* Automatically set the senior App if only one app is on the array */}

                {renderIf(
                  redirectToSeniors || this.state.allowRedirect,
                  <React.Fragment>
                    {this.state.allowRedirect && value.collectionsLoaded
                      ? setSeniorApp(seniorApps[0])
                      : null}
                    {/* {redirectToSeniors
                      ? setSeniorApp(
                          seniorApps.filter(s => s.statuscode === 668020012)[0]
                        )
                      : null} */}

                    <Redirect to="/seniors/myInfo" />
                  </React.Fragment>
                )}

                <LoadingOverlay
                  active={showLoading && !value.collectionsLoaded}
                  spinner
                  text={loadingFormatter}
                  styles={{
                    overlay: base => ({
                      ...base,
                      position: 'fixed',
                    }),
                    content: base => ({
                      ...base,
                      margin: 0,
                      position: 'absolute',
                      top: '50%',
                      left: '50%',
                      transform: 'translate(-50%, -50%)',
                    }),
                  }}
                />

                {/* <Grid style={{ minHeight: '800px' }} container> */}

                {/*Submit confirmation*/}
                {renderIf(
                  showSubmitDoneConfirmation,
                  <MessageBox
                    testid={'appFiledBox'}
                    title={fm.filledApplication}
                    line1={fm.thankYou}
                    line2={fm.letYouKnow}
                    onClick={() => {
                      onShowSubmitDoneConfirmation(false);
                    }}
                  />
                )}
                {renderIf(
                  showSubmitFailedConfirmation,
                  <MessageBox
                    title={fm.filledApplicationError}
                    line1={fm.failedSubmission}
                    line2={fm.failedSubmissionTryAgain}
                    onClick={() => {
                      onShowSubmitFailedConfirmation(false);
                    }}
                  />
                )}
                {/*Submit confirmation*/}
                <div className="content-wrapper home-wrapper">
                  <Grid container className="apps-header space-top-16">
                    <Grid
                      item
                      md={12}
                      style={{ textAlign: 'left', maxHeight: '50px' }}
                    >
                      <h1 className="header3">
                        {arrayNullOrEmpty(seniorApps)
                          ? fm.filingInfo
                          : fm.accountHome}
                      </h1>{' '}
                    </Grid>
                  </Grid>
                  <Grid container className="apps">
                    <Grid item xs={12}>
                      <hr />
                      {renderIf(
                        arrayNullOrEmpty(seniorApps),
                        <React.Fragment>
                          <Card md={12}>
                            <CardContent>
                              <h5>{fm.noProgress}</h5>
                            </CardContent>
                          </Card>
                        </React.Fragment>,

                        <React.Fragment>
                          <Grid container>
                            {seniorApps.map((app, index) => {
                              return (
                                <Grid item>
                                  <SeniorAppCard
                                    key={app.seapplicationid}
                                    index={index}
                                    seniorApp={app}
                                    seniorAppDetails={
                                      seniorAppsDetails[app.seapplicationid]
                                    }
                                    appStatusCodes={value.seniorAppStatuses}
                                    statusCodes={value.seniorAppDetailsStatuses}
                                    setSeniorApp={setSeniorApp}
                                    years={value.years}
                                  />
                                </Grid>
                              );
                            })}
                          </Grid>
                          <Grid style={{ paddingTop: 12 }}>
                            <VimeoCard
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
                            />
                          </Grid>
                        </React.Fragment>
                      )}

                      {renderIf(
                        arrayNullOrEmpty(seniorApps) ||
                          !seniorApps.some(s => s.statuscode === 668020012),
                        <React.Fragment>
                          <div
                            md={4}
                            style={{
                              marginRight: '20px',
                              marginTop: '20px',
                              textAlign: 'left',
                            }}
                          >
                            {fm.areYouElegible} <br />
                            {fm.applyNow} <br />
                            <div
                              className="edit-link"
                              style={{ marginLeft: '0px', fontSize: '20px' }}
                              onClick={() => createSeniorApp(contact.contactid)}
                            >
                              {fm.seniorExemption}{' '}
                              <img src={rightArrow} alt="King County" />
                            </div>
                          </div>
                          <div md={4}></div>
                          <div md={4}></div>
                        </React.Fragment>
                      )}
                    </Grid>
                    {/*
                    <Grid xs={0} md={1}></Grid>
                      <Grid xs={12} md={4} xl={3}>
                        <Card className="app-card-complete">
                          <CardContent className="app-card-content">
                            <span>
                              <FormattedMessage
                                id="you_profile_is"
                                defaultMessage="Your profile is"
                              />{' '}
                              85%{' '}
                              <FormattedMessage
                                id="complete"
                                defaultMessage="complete"
                              />{' '}
                            </span>
                            <a
                              href={`${process.env.REACT_APP_B2C_INSTANCE}${process.env.REACT_APP_B2C_TENANT}/${process.env.REACT_APP_B2C_PROFEDITPOLICY}/oauth2/v2.0/authorize`}
                              target=""
                            >
                              <CustomButton
                                style={{
                                  background: 'white',
                                  color: '#6f1f62',
                                  borderColor: '#6f1f62',
                                  marginTop: '15px',
                                }}
                                label="Update profile info"
                              />
                            </a>
                          </CardContent>
                        </Card>
                      </Grid>
                              */}
                  </Grid>
                </div>
              </React.Fragment>
            )}
          </CollectionConsumer>
        )}
      </AuthConsumer>
    );
  };
}

Home.contextType = AuthContext;
export default Home;
