import React, { Component, Fragment } from 'react';

import { Redirect } from 'react-router-dom';
import './SeniorsContainer.css';
import { renderIf, getDateDifferenceInDays } from '../lib/helpers/util';
import MyInfo from '../components/my-info/MyInfo';
import RequestInfo from '../components/request-info/RequestInfo';
import CustomTab from '../components/common/CustomTab';
import PropertyInfo from '../components/property-info/PropertyInfo';
import FinancialInfo from '../components/financial-info/FinancialInfo';
import Summary from '../components/summary/Summary';
import Signature from '../components/signature/Signature';
import Submitted from '../components/summary/Submitted';
import VimeoPlayer from '../components/common/vimeo-player/VimeoPlayer';
import * as fm from './FormatTexts';
import { AuthConsumer, AuthContext } from '../contexts/AuthContext';
import { CollectionConsumer } from '../contexts/CollectionContext';
import { SignalRConsumer } from '../contexts/SignalRContext';
import { FormattedMessage } from 'react-intl';
import deepEqual from 'deep-equal';
import FullPageSpinner from '../components/common/FullPageSpinner';
import Grid from '@material-ui/core/Grid';
import { nonReimbursedPrescriptions } from '../components/financial-info/FormatTexts';
import LoadingOverlay from 'react-loading-overlay';
import { setRootIds } from '../services/dataServiceProvider';
import { getQRCode } from '../services/blobService';
import Collapse from '@material-ui/core/Collapse';
import AlertPopUpYears from '../components/common/AlertPopUpYears';
import AlertWarningYear from '../components/common/AlertWarningYear';
import { withStyles } from '@material-ui/core/styles';
import IncomeHelp from '../components/income-help/IncomeHelp';
import { Clear as ClearIcon, InfoOutlined } from '@material-ui/icons';

import CustomContentDialog from '../components/common/dialog-component/CustomContentDialog';
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  IconButton,
} from '@material-ui/core';

class SeniorsContainer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      steps: [
        'myInfo',
        'propertyInfo',
        'financialInfo',
        'summary',
        'requestMoreInfo',
      ],
      myInfoStepSelected: true,
      displayReplacementTitle: '',
      propertyInfoStepSelected: false,
      financialInfoStepSelected: false,
      summaryStepSelected: false,
      submittedStepSelected: false,
      requestMoreInfoStepSelected: false,
      hideToolbar: false,
      showmoreinfotitle: false,
      isIE: false,
      defaultHelpVideoUrl: null,
      showHelpDialog: false,
      hideHeader: false,
      editMode: false,
      readOnlyMode: false,
      continueText: fm.continueLabel,
      isSaving: false,
      infoSaved: false,
      redirectHome: false,
      applicationSigned: false,
      currentTab: this.props.match.params.id,
      seniorApp: null,
      QRCode: '',
      showContentDialog: false,
      applicationDaysLeft: null,
    };
  }

  toggleDialog = () => {
    if (this.state.showContentDialog) {
      this.setState({ showContentDialog: false });
    } else {
      this.setState({ showContentDialog: true });
    }
  };

  componentDidMount = async () => {
    this.context.checkClientBrowser() && this.setState({ isIE: true });
    if (this.context.seniorApp) {
      setRootIds(this.context.seniorApp, this.context.contact);
    }

    if (this.context.seniorApp && !this.state.seniorApp) {
      // Calculate timeout for the application
      let daysDifference = process.env.REACT_APP_TimeOutDays;
      if (this.context.seniorApp.createdon) {
        let finalDate = new Date(this.context.seniorApp.createdon);
        finalDate.setDate(
          finalDate.getDate() + parseInt(process.env.REACT_APP_TimeOutDays)
        );
        daysDifference = getDateDifferenceInDays(new Date(), finalDate);
        daysDifference = daysDifference < 0 ? 0 : daysDifference;
      }

      this.setState(
        {
          seniorApp: this.context.seniorApp,
          applicationDaysLeft: daysDifference,
        },
        () => {
          this.getAppQRCode(this.context.user.email, this.context.qrToken);
        }
      );
      await this.navigateToSection();
    } else {
      this.setState({ redirectHome: true });
    }
  };

  componentDidUpdate = async () => {
    if (this.context.seniorApp && !this.state.seniorApp) {
      this.setState({ seniorApp: this.context.seniorApp });
      await this.navigateToSection();
    }
  };

  getAppQRCode = async (email, qrToken) => {
    const QRCode = await getQRCode(
      this.state.seniorApp.contactid,
      this.state.seniorApp.seapplicationid,
      email,
      qrToken
    );

    if (QRCode && QRCode !== '') {
      this.setState({ QRCode: QRCode.fileBytes });
    }
  };
  applicationSigned = async () => {
    this.setState({ applicationSigned: true, redirectHome: true });
    console.log('HomeRedirect SeniorContainer State', this.state);
  };
  navigateToSection = async () => {
    if (this.context.seniorApp) {
      let lastTab = localStorage.getItem(
        `${this.context.seniorApp.seapplicationid}_lastTab`
      );

      lastTab =
        lastTab && lastTab !== '' ? lastTab : this.props.match.params.id;

      switch (lastTab) {
        case 'propertyInfo':
          this.onHandleTabChange(null, 1);
          break;
        case 'financialInfo':
          await this.context.getOccupants(
            this.context.seniorApp.seapplicationid
          );
          this.onHandleTabChange(null, 2);
          break;
        case 'summary':
          this.onHandleTabChange(null, 3);
          break;
        case 'requestMoreInfo':
          this.setState({
            displayReplacementTitle: fm.additionalInfoRequested,
          });
          this.onHandleTabChange(null, 4);
          break;

        default:
          localStorage.removeItem(
            `${this.context.seniorApp.seapplicationid}_lastTab`
          );
          break;
      }
    }
  };

  getPreviousStep = seniorApp => {
    if (seniorApp.financialsection) {
      this.setState({
        summaryStepSelected: false,
        myInfoStepSelected: false,
        propertyInfoStepSelected: false,
        financialInfoStepSelected: false,
      });
    }
    if (seniorApp.propertysection) {
      this.setState({
        summaryStepSelected: false,
        myInfoStepSelected: false,
        propertyInfoStepSelected: false,
        financialInfoStepSelected: true,
      });
    }

    if (seniorApp.taxpayersection) {
      this.setState({
        summaryStepSelected: false,
        myInfoStepSelected: false,
        propertyInfoStepSelected: true,
        financialInfoStepSelected: false,
      });
    }
  };

  selectedStepToValue = () => {
    if (this.state.myInfoStepSelected) return 0;
    if (this.state.propertyInfoStepSelected) return 1;
    if (this.state.financialInfoStepSelected) return 2;
    if (this.state.summaryStepSelected) return 3;
    return 4;
  };

  onHandleTabChange = (event, newValue) => {
    window.scrollTo(0, 0);
    let state = { currentTab: this.state.steps[newValue] };

    let currentIndex = 0;
    this.state.steps.map((s, i) => {
      if (this.state[`${s}StepSelected`]) {
        currentIndex = i;
        return;
      }
    });

    // Allow to move from tab to tab when: App is running in dev, the method is call manually or if the tab being navigated to is a previous tab.
    if (
      process.env.NODE_ENV === 'development' ||
      !event ||
      newValue < currentIndex
    ) {
      this.state.steps.map((s, i) => {
        if (i === newValue) {
          state[`${s}StepSelected`] = true;
          let lastTab = this.state.steps[newValue]
            ? this.state.steps[newValue]
            : 'myInfo';
          localStorage.setItem(
            `${this.context.seniorApp.seapplicationid}_lastTab`,
            `${lastTab}`
          );
        } else {
          state[`${s}StepSelected`] = false;
        }
      });

      this.setState(state);
    }
  };
  setVideoCode = videoCode => {
    this.setState({ defaultHelpVideoUrl: videoCode });
  };
  onPopupOpen = e => {
    let state = {
      showHelpDialog: true,
    };

    this.setState(state);
    /* Push to mouseflow as another page state */
    window._mfq = window._mfq || [];
    window._mfq.push(['newPageView', 'leftmenu-popup']);
  };

  onPopupClose = () => {
    this.setState({ showHelpDialog: false });
  };

  tabUpdateInfo = async e => {
    window.scrollTo(0, 0);
    this.setState({ isSaving: true, continueText: fm.savingLabel });
    if (!this.props.readOnlyMode) {
      const promises = [
        this.child.updateContact(),
        this.child.updateFileMetadata(),
        this.child.updateSeniorApp(),
      ];

      await Promise.all(promises);
    }
  };

  tabUpdateProperty = async () => {
    window.scrollTo(0, 0);
    this.setState({ isSaving: true, continueText: fm.savingLabel });
    if (!this.props.readOnlyMode) {
      const promises = [
        this.child.updateFileMetadata(),
        this.child.updateSeniorApp(),
        this.child.createOrUpdateOtherOccupants(),
        this.child.createOrUpdateOtherProperties(),
        //this.child.createOrUpdateSeniorDetails(),
      ];

      await Promise.all(promises);
    }
  };

  hideToolbar = hide => {
    this.setState({ hideToolbar: hide });
  };

  showmoreinfotitle = show => {
    this.setState({ showmoreinfotitle: show });
  };

  navigateToEditTab = tab => {
    this.setState({ editMode: true }, () => {
      this.onHandleTabChange(null, tab);
    });
  };

  navigateToSummary = () => {
    this.onHandleTabChange(null, 4);
  };

  getYearToRequestInfo = (seniorApp, seniorAppDetails, yearsToApplyObject) => {
    if (seniorApp?.statuscode !== 668020014) return;

    const seniorAppDetFound = seniorAppDetails.find(
      sad => sad.seapplicationid === seniorApp.seapplicationid
    );

    if (!seniorAppDetFound) return;

    const yearFound =
      yearsToApplyObject.find(y => y.yearId === seniorAppDetFound.yearid) || {};
    return yearFound.name;
  };

  render = () => {
    let selectedTab = this.selectedStepToValue();
    let renderMoreInfoTitle = this.state.showmoreinfotitle;

    let renderToolBar =
      !this.state.submittedStepSelected && !this.state.hideToolbar;

    const yearApplying = 2018;
    return (
      <AuthConsumer>
        {({
          getSeniorApps,
          showLoading,
          loadingFormatter,
          showLoadingOverLay,
          readOnlyMode,
          applicationStatus,
          updateContact,
          contact,
          seniorApp,
          updateSeniorApp,
          submitSeniorApp,
          filesMetadata,
          getFilesMetadata,
          updateFilesMetadataState,
          otherProperties,
          occupants,
          getOtherProperties,
          getOccupants,
          seniorAppDetails,
          getSeniorDetails,
          financials,
          getSeniorFinancials,
          htmlSummary,
          setHtmlSummary,
          getYearsObject,
          yearsToApplyObject,
          handleYearsPopUpOptions,
          handleYearsPopUpClose,
          handleWarningPopUpClose,
          handleYearsPopUpDisplay,
          displayYearsPopUp,
          globalYearOptionsArray,
          setOptionsArray,
          createOrUpdateSeniorDetails,
          createOrUpdateAuthYearsObject,
          selectedYear,
          recommendedYear,
          earlierYear,
          displayWarningPopUp,
          handleDeleteFinancialForms,
          returnToRadioButtomSource,
          calculateAuthYearsObject,
          sectionsVideoPlayedArray,
          setSectionsVideoPlayedObject,
          checkIfVideoPlayed,
          toggleRedirectToSeniors,
          calculateFormsAmounts,
          checkClientBrowser,
          openMaxIncomeDialog,
          handleCloseMaxIncomeDialog,
          maxIncomeDialogMsg,
        }) => (
          <SignalRConsumer>
            {({ messages, uploadsData, sendMessage, setUpload }) => (
              <React.Fragment>
                <LoadingOverlay
                  active={showLoading}
                  spinner
                  text={loadingFormatter}
                  styles={{
                    overlay: base => ({
                      ...base,
                      position: 'fixed',
                      width: '100%',
                      height: '100%',
                      zIndex: 10,
                    }),
                    content: base => ({
                      margin: 0,
                      position: 'absolute',
                      top: '50%',
                      left: '50%',
                      transform: 'translate(-50%, -50%)',
                    }),
                  }}
                />
                <div className="seniors">
                  <CustomContentDialog
                    showContentDialog={this.state.showContentDialog}
                    toggleDialog={this.toggleDialog}
                    //dialogTitle={'Documents You will Need'}
                  >
                    <IncomeHelp ref={el => (this.componentRef = el)} />
                  </CustomContentDialog>

                  <CollectionConsumer>
                    {value => (
                      <React.Fragment>
                        {this.state.showHelpDialog ? (
                          <VimeoPlayer
                            width={'1000'}
                            height={'430'}
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
                            isPopup
                            defaultUrl={this.state.defaultHelpVideoUrl}
                            showList
                            autoplay
                            onPopupClose={this.onPopupClose}
                            //hasPlaybar
                            noTitle
                            show={this.state.showHelpDialog}
                          />
                        ) : (
                          ''
                        )}

                        {renderIf(
                          this.state.redirectHome,

                          <Redirect
                            push
                            to={{
                              pathname: '/home',
                              state: { id: this.props.match.params.id },
                            }}
                          />
                        )}

                        {renderIf(
                          !this.state.submittedStepSelected,

                          <Grid container spacing={12}>
                            <Grid item xs={12} sm={10}>
                              <h1
                                className="header2"
                                style={{
                                  fontWeight: 'bold !important',

                                  margin: '30px 47px',
                                }}
                              >
                                <FormattedMessage
                                  id="SeniorContainer_title"
                                  defaultMessage="Senior Exemption {year} application"
                                  values={{
                                    year:
                                      selectedYear ||
                                      this.getYearToRequestInfo(
                                        seniorApp,
                                        seniorAppDetails,
                                        yearsToApplyObject
                                      ),
                                  }}
                                />
                              </h1>
                            </Grid>

                            <Grid
                              item
                              sm={2}
                              style={{
                                lineHeight: 1.3,
                                textAlign: 'center',
                                marginTop: '23px',
                              }}
                            >
                              {renderIf(
                                showLoading,
                                <div
                                  id="saving"
                                  style={{
                                    display: 'block',
                                  }}
                                >
                                  <small>
                                    <strong>{fm.savingLabel}</strong>
                                  </small>
                                </div>,
                                renderIf(
                                  showLoading === false,
                                  <div id="saved">
                                    <small>
                                      <strong>
                                        <FormattedMessage
                                          id="SeniorContainer_infoSaved"
                                          defaultMessage="Your information is saved"
                                        />
                                      </strong>
                                    </small>
                                    <br></br>
                                    <small>
                                      <FormattedMessage
                                        id="SeniorContainer_infoSavedDetail"
                                        defaultMessage="You can continue your application now or return <strong>within {daysLeft} days</strong> to finish."
                                        values={{
                                          strong: (...chunks) => (
                                            <strong>{chunks}</strong>
                                          ),
                                          daysLeft: this.state
                                            .applicationDaysLeft,
                                        }}
                                      />
                                    </small>
                                  </div>
                                )
                              )}
                            </Grid>
                          </Grid>
                        )}

                        <div className="box-shadow">
                          <CustomTab
                            checkClientBrowser={checkClientBrowser}
                            isIE={this.state.isIE}
                            value={selectedTab}
                            handleTabChange={this.onHandleTabChange}
                            display={renderToolBar}
                            QRCode={
                              this.state.QRCode
                                ? `data:image/png;base64,${this.state.QRCode}`
                                : null
                            }
                            displayReplacementTitle={
                              this.state.displayReplacementTitle
                            }
                            toggleDialog={this.toggleDialog}
                            callHelpVideo={this.onPopupOpen}
                            handleYearsPopUpOptions={handleYearsPopUpOptions}
                            handleYearsPopUpClose={handleYearsPopUpClose}
                            handleYearsPopUpDisplay={handleYearsPopUpDisplay}
                            displayYearsPopUp={displayYearsPopUp}
                            selectedYear={selectedYear}
                          />
                        </div>

                        <div>
                          <AlertPopUpYears
                            text={
                              <FormattedMessage
                                style={{ fontWeight: 'bold !important' }}
                                id="YearsPopUp_title"
                                defaultMessage="We recommend you apply for the current year first, and then for later years that you may qualify for. If approved, a current year application will place you in the exemption program in future years. Based on you age, ability, and property, the earliest year you may qualify for is {year}."
                                values={{ year: earlierYear }}
                              />
                            }
                            isOpen={displayYearsPopUp}
                            yearOptionsArray={globalYearOptionsArray}
                            onChangeRadioButton={handleYearsPopUpOptions}
                            onClose={handleYearsPopUpDisplay}
                            itemMsg={maxIncomeDialogMsg}
                          />
                        </div>

                        <div>
                          <AlertWarningYear
                            text={
                              <FormattedMessage
                                style={{ fontWeight: 'bold !important' }}
                                id="WarningPopUp_title"
                                defaultMessage="You have financial forms for {year}. If you
                                change it, all financial forms filled for {year} will be
                                deleted. Do you want to continue?"
                                values={{
                                  year: selectedYear,
                                }}
                              />
                            }
                            isOpen={displayWarningPopUp}
                            onClose={handleWarningPopUpClose}
                            handleDeleteFinancialForms={
                              handleDeleteFinancialForms
                            }
                          />
                        </div>

                        <div
                          className="box-shadow"
                          style={{
                            visibility: `${
                              renderMoreInfoTitle ? 'visible' : 'collapse'
                            }`,
                          }}
                        ></div>

                        {renderIf(
                          !this.state.applicationSigned,
                          <div>
                            <Redirect
                              push
                              to={
                                '/seniors/' +
                                (this.state.currentTab
                                  ? this.state.currentTab
                                  : 'myInfo')
                              }
                            />
                          </div>
                        )}
                        {renderIf(
                          this.state.myInfoStepSelected,

                          <MyInfo
                            uploadsData={uploadsData}
                            setUpload={setUpload}
                            checkClientBrowser={checkClientBrowser}
                            calculateAuthYearsObject={calculateAuthYearsObject}
                            returnToRadioButtomSource={
                              returnToRadioButtomSource
                            }
                            setVideoCode={this.setVideoCode}
                            signalRMessages={messages}
                            sendMessage={sendMessage}
                            checkIfVideoPlayed={checkIfVideoPlayed}
                            callHelpVideo={this.onPopupOpen}
                            defaultHelpVideoUrl={382824222}
                            getYearsObject={getYearsObject}
                            years={value.years}
                            yearsToApplyArrayObject={yearsToApplyObject}
                            handleYearsPopUpOptions={handleYearsPopUpOptions}
                            handleYearsPopUpClose={handleYearsPopUpClose}
                            displayYearsPopUp={displayYearsPopUp}
                            handleYearsPopUpDisplay={handleYearsPopUpDisplay}
                            globalYearOptionsArray={globalYearOptionsArray}
                            setOptionsArray={setOptionsArray}
                            createOrUpdateSeniorDetails={
                              createOrUpdateSeniorDetails
                            }
                            createOrUpdateAuthYearsObject={
                              createOrUpdateAuthYearsObject
                            }
                            selectedYear={selectedYear}
                            recommendedYear={recommendedYear}
                            earlierYear={earlierYear}
                            seniorApp={seniorApp}
                            seniorAppDetails={seniorAppDetails}
                            getSeniorDetails={getSeniorDetails}
                            updateContact={updateContact}
                            updateSeniorApp={updateSeniorApp}
                            contact={contact}
                            filesMetadata={filesMetadata}
                            getFilesMetadata={getFilesMetadata}
                            updateFilesMetadataState={updateFilesMetadataState}
                            updateStep={this.getPreviousStep}
                            editMode={this.state.editMode}
                            readOnlyMode={readOnlyMode}
                            onNavigateToSummary={this.navigateToSummary}
                            nextTab={() => this.onHandleTabChange(null, 1)}
                            showSavingOverlay={() =>
                              showLoadingOverLay(
                                true,
                                <FormattedMessage
                                  id="Overlay_saving-message"
                                  defaultMessage="Saving information..."
                                  description="Saving information Message"
                                />
                              )
                            }
                            hideSavingOverlay={() =>
                              showLoadingOverLay(false, null)
                            }
                            sectionsVideoPlayedArray={sectionsVideoPlayedArray}
                            setSectionsVideoPlayedObject={
                              setSectionsVideoPlayedObject
                            }
                            itemMsg={maxIncomeDialogMsg}
                          />
                        )}
                        {renderIf(
                          this.state.propertyInfoStepSelected && seniorApp,

                          <PropertyInfo
                            uploadsData={uploadsData}
                            setUpload={setUpload}
                            calculateAuthYearsObject={calculateAuthYearsObject}
                            returnToRadioButtomSource={
                              returnToRadioButtomSource
                            }
                            signalRMessages={messages}
                            sendMessage={sendMessage}
                            checkIfVideoPlayed={checkIfVideoPlayed}
                            callHelpVideo={this.onPopupOpen}
                            defaultHelpVideoUrl={382824232}
                            setVideoCode={this.setVideoCode}
                            getYearsObject={getYearsObject}
                            years={value.years}
                            countries={value.countries}
                            relationships={value.relationships}
                            yearsToApplyArrayObject={yearsToApplyObject}
                            handleYearsPopUpOptions={handleYearsPopUpOptions}
                            handleYearsPopUpClose={handleYearsPopUpClose}
                            displayYearsPopUp={displayYearsPopUp}
                            handleYearsPopUpDisplay={handleYearsPopUpDisplay}
                            globalYearOptionsArray={globalYearOptionsArray}
                            selectedYear={selectedYear}
                            setOptionsArray={setOptionsArray}
                            createOrUpdateSeniorDetails={
                              createOrUpdateSeniorDetails
                            }
                            createOrUpdateAuthYearsObject={
                              createOrUpdateAuthYearsObject
                            }
                            recommendedYear={recommendedYear}
                            earlierYear={earlierYear}
                            seniorApp={seniorApp}
                            contact={contact}
                            updateSeniorApp={updateSeniorApp}
                            filesMetadata={filesMetadata}
                            getFilesMetadata={getFilesMetadata}
                            updateFilesMetadataState={updateFilesMetadataState}
                            otherProperties={otherProperties}
                            occupants={occupants}
                            getOtherProperties={getOtherProperties}
                            getOccupants={getOccupants}
                            seniorAppDetails={seniorAppDetails}
                            getSeniorDetails={getSeniorDetails}
                            editMode={this.state.editMode}
                            readOnlyMode={readOnlyMode}
                            onNavigateToSummary={this.navigateToSummary}
                            nextTab={() => this.onHandleTabChange(null, 2)}
                            showSavingOverlay={() =>
                              showLoadingOverLay(
                                true,
                                <FormattedMessage
                                  id="Overlay_saving-message"
                                  defaultMessage="Saving information..."
                                  description="Saving information Message"
                                />
                              )
                            }
                            hideSavingOverlay={() =>
                              showLoadingOverLay(false, null)
                            }
                          />
                        )}
                        {renderIf(
                          this.state.financialInfoStepSelected && seniorApp,

                          <FinancialInfo
                            uploadsData={uploadsData}
                            setUpload={setUpload}
                            calculateFormsAmounts={calculateFormsAmounts}
                            calculateAuthYearsObject={calculateAuthYearsObject}
                            returnToRadioButtomSource={
                              returnToRadioButtomSource
                            }
                            signalRMessages={messages}
                            sendMessage={sendMessage}
                            checkIfVideoPlayed={checkIfVideoPlayed}
                            callHelpVideo={this.onPopupOpen}
                            defaultHelpVideoUrl={382824246}
                            setVideoCode={this.setVideoCode}
                            getYearsObject={getYearsObject}
                            years={value.years}
                            yearsToApplyArrayObject={yearsToApplyObject}
                            handleYearsPopUpOptions={handleYearsPopUpOptions}
                            handleYearsPopUpClose={handleYearsPopUpClose}
                            displayYearsPopUp={displayYearsPopUp}
                            handleYearsPopUpDisplay={handleYearsPopUpDisplay}
                            globalYearOptionsArray={globalYearOptionsArray}
                            setOptionsArray={setOptionsArray}
                            createOrUpdateSeniorDetails={
                              createOrUpdateSeniorDetails
                            }
                            createOrUpdateAuthYearsObject={
                              createOrUpdateAuthYearsObject
                            }
                            selectedYear={selectedYear}
                            recommendedYear={recommendedYear}
                            earlierYear={earlierYear}
                            seniorApp={seniorApp}
                            contact={contact}
                            updateSeniorApp={updateSeniorApp}
                            filesMetadata={filesMetadata}
                            getFilesMetadata={getFilesMetadata}
                            updateFilesMetadataState={updateFilesMetadataState}
                            occupants={occupants}
                            seniorAppDetails={seniorAppDetails}
                            hideToolbar={this.hideToolbar}
                            financials={financials}
                            getSeniorFinancials={getSeniorFinancials}
                            getSeniorDetails={getSeniorDetails}
                            editMode={this.state.editMode}
                            readOnlyMode={readOnlyMode}
                            onNavigateToSummary={this.navigateToSummary}
                            years={value.years}
                            setHtmlSummary={setHtmlSummary}
                            nextTab={() => this.onHandleTabChange(null, 3)}
                            showSavingOverlay={() =>
                              showLoadingOverLay(
                                true,
                                <FormattedMessage
                                  id="Overlay_saving-message"
                                  defaultMessage="Saving information..."
                                  description="Saving information Message"
                                />
                              )
                            }
                            hideSavingOverlay={() =>
                              showLoadingOverLay(false, null)
                            }
                          />
                        )}

                        {renderIf(
                          this.state.summaryStepSelected && seniorApp,
                          <Summary
                            uploadsData={uploadsData}
                            setUpload={setUpload}
                            signalRMessages={messages}
                            sendMessage={sendMessage}
                            getFilesMetadata={getFilesMetadata}
                            getSeniorApps={getSeniorApps}
                            toggleRedirectToSeniors={toggleRedirectToSeniors}
                            callHelpVideo={this.onPopupOpen}
                            setVideoCode={this.setVideoCode}
                            applicationSigned={this.applicationSigned}
                            defaultHelpVideoUrl={382824273}
                            seniorApp={seniorApp}
                            years={value.years}
                            financialFormTypes={value.financialFormTypes}
                            updateSeniorApp={updateSeniorApp}
                            submitSeniorApp={submitSeniorApp}
                            contact={contact}
                            filesMetadata={filesMetadata}
                            otherProperties={otherProperties}
                            occupants={occupants}
                            seniorAppDetails={seniorAppDetails}
                            financials={financials}
                            readOnlyMode={readOnlyMode}
                            applicationStatus={applicationStatus}
                            htmlSummary={htmlSummary}
                            onEditMyInfo={() => this.navigateToEditTab(0)}
                            onEditPropertyInfo={() => this.navigateToEditTab(1)}
                            onEditFinancialInfo={() =>
                              this.navigateToEditTab(2)
                            }
                            onEditSignature={() => this.navigateToEditTab(3)}
                            nextTab={() => this.onHandleTabChange(null, 5)}
                            showSavingOverlay={() =>
                              showLoadingOverLay(
                                true,
                                <FormattedMessage
                                  id="Overlay_savingApplication"
                                  defaultMessage="Saving Application..."
                                />
                              )
                            }
                            hideSavingOverlay={() =>
                              showLoadingOverLay(false, null)
                            }
                          />
                        )}

                        {renderIf(
                          this.state.requestMoreInfoStepSelected && seniorApp,
                          <RequestInfo
                            portalAttachmentLocations={
                              value.portalAttachmentLocations
                            }
                            upload={value}
                            setUpload={setUpload}
                            years={value.years}
                            yearsToApplyArrayObject={yearsToApplyObject}
                            showSavingOverlay={() =>
                              showLoadingOverLay(
                                true,
                                <FormattedMessage
                                  id="Overlay_saving-message"
                                  defaultMessage="Saving information..."
                                  description="Saving information Message"
                                />
                              )
                            }
                            hideSavingOverlay={() =>
                              showLoadingOverLay(false, null)
                            }
                            seniorApp={seniorApp}
                            financialFormTypes={value.financialFormTypes}
                            updateSeniorApp={updateSeniorApp}
                            contact={contact}
                            filesMetadata={filesMetadata}
                            otherProperties={otherProperties}
                            occupants={occupants}
                            hideToolbar={this.hideToolbar}
                            showmoreinfotitle={this.showmoreinfotitle}
                            seniorAppDetails={seniorAppDetails}
                            financials={financials}
                            readOnlyMode={readOnlyMode}
                            getFilesMetadata={getFilesMetadata}
                            applicationStatus={applicationStatus}
                            htmlSummary={htmlSummary}
                            onEditMyInfo={() => this.navigateToEditTab(0)}
                            onEditPropertyInfo={() => this.navigateToEditTab(1)}
                            onEditFinancialInfo={() =>
                              this.navigateToEditTab(2)
                            }
                            onEditSignature={() => this.navigateToEditTab(3)}
                            nextTab={() => this.onHandleTabChange(null, 5)}
                          />
                        )}
                        {renderIf(
                          this.state.submittedStepSelected && seniorApp,
                          <Submitted />
                        )}
                      </React.Fragment>
                    )}
                  </CollectionConsumer>
                </div>
                {/* max income dialog  */}
                <Dialog open={openMaxIncomeDialog}>
                  <DialogActions>
                    <IconButton onClick={handleCloseMaxIncomeDialog}>
                      <ClearIcon style={{ width: '1em', height: '1em' }} />
                    </IconButton>
                  </DialogActions>
                  <DialogContent
                    style={{
                      display: 'flex',
                      flexDirection: 'row',
                      alignItems: 'center',
                      marginTop: '-25px',
                    }}
                  >
                    <div style={{ marginRight: 8 }}>
                      <InfoOutlined style={{ width: '2em', height: '2em' }} />
                    </div>
                    <div>
                      <DialogContentText
                        id="alert-dialog-description"
                        style={{ color: 'black' }}
                      >
                        {maxIncomeDialogMsg[selectedYear]}
                      </DialogContentText>
                    </div>
                  </DialogContent>
                </Dialog>
              </React.Fragment>
            )}
          </SignalRConsumer>
        )}
      </AuthConsumer>
    );
  };
}

SeniorsContainer.contextType = AuthContext;
export default SeniorsContainer;
