//-----------------------------------------------------------------------
// <copyright file="Symmary.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React from 'react';
import './Summary.css';
import * as fm from './FormatTexts';
import TabContainer from '../common/TabContainer';
import UploadFile from '../common/UploadFile/UploadFile';
import ShortDivider from '../common/ShortDivider';
import CurrencyLabel from '../common/CurrencyLabel';
import { FormattedMessage } from 'react-intl';
import Collapse from '@material-ui/core/Collapse';
import { Redirect } from 'react-router-dom';
import { AuthConsumer, AuthContext } from '../../contexts/AuthContext';
import CustomButton from '../common/CustomButton';
import { withRouter } from 'react-router-dom';
import BoolLabel from '../common/BoolLabel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import CustomContentDialog from '../common/dialog-component/CustomContentDialog';
import Grid from '@material-ui/core/Grid';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ArrowLeftIcon from '@material-ui/icons/ArrowLeft';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import HelpText from '../common/HelpText';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import SummaryLabel from '../common/SummaryLabel';
import {
  moveFilesToiLinx,
  getRedirectedDocuSignUrl,
  getRedirectedDocuSignUrlHtml,
} from '../../services/blobService';
import BorderColorIcon from '@material-ui/icons/BorderColor';

import { textAlign } from '@material-ui/system';
import { yourInfo } from './FormatTexts';
import { getParcelDetail } from '../../services/dynamics-service';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContentText from '@material-ui/core/DialogContentText';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import {
  updateFileMetadata,
  createFileMetadata,
} from '../../services/dataServiceProvider';
import deepEqual from 'deep-equal';
import Submitted from './Submitted';
import { cloneDeep } from 'lodash';
import TextField from '@material-ui/core/TextField';
import {
  generateBlobUriObjectFromAFileArray,
  getFileArraysFromFileMetadataList,
  getFileArrayFromBlobUrl,
} from '../../lib/data-mappings/fileMetadataToFileArray';
import {
  CollectionConsumer,
  CollectionContext,
} from '../../contexts/CollectionContext';
import { arrayNullOrEmpty, renderIf } from '../../lib/helpers/util';
import Maps from './propertyMappings';
import { transformToType } from '../../lib/data-mappings/financialFormsDataTypes';
import { get } from 'https';
import { createImageFromCurrentPage } from '../../lib/helpers/file-manipulation';
import LoadingSection from '../common/LoadingSection';
import LoadingSectionButtom from '../common/LoadingSectionButtom';
import {
  createOrUpdateFileMetadataEntities,
  createFileArraysFromMetadata,
} from '../common/UploadFile/uploaderHelper';
import { getAppInsightsInstance } from '../../services/telemetryService';

import CustomIFrame from '../common/CustomIFrame';
import {
  updateUploadsData,
  setUploadsData,
} from '../common/UploadFile/signalRHelper';
import { trimSecure } from '../../lib/helpers/trimText';

const CssTextField = withStyles(theme => ({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiFormLabel-root, ': {
      fontSize: '16px',
    },
    '& .MuiInputBase-input': {
      fontSize: '16px',
      [theme.breakpoints.up('sm')]: {
        width: '286px',
      },
      [theme.breakpoints.down('xs')]: {
        width: '210px',
      },
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
    '& .MuiInput-underline:before': {
      borderBottomColor: '#000000',
    },
    '& .MuiInput-underline:before': {
      borderBottomColor: '#000000',
    },
    '& .MuiOutlinedInput-root': {
      '& fieldset': {
        borderColor: 'red',
      },
      '&:hover fieldset': {
        borderColor: 'yellow',
      },
      '&.Mui-focused fieldset': {
        borderColor: '#a5c727',
      },
    },
  },
}))(TextField);

const DialogSmall = withStyles(theme => ({
  root: {
    [theme.breakpoints.up('sm')]: {
      width: '88%',
    },
    [theme.breakpoints.down('xs')]: {
      width: '100%',
    },
    margin: 'auto',
    marginTop: 40,
    '& .MuiDialog-paper': {
      border: '3px solid black',
      width: '630px !important',
      [theme.breakpoints.down('xs')]: {
        margin: '5%',
      },
    },
    '& .MuiDialog-container': {
      height: 'auto',
    },
  },
}))(Dialog);
class Summary extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      continueTextSum: fm.addApplication,
      showContentDialog: false,
      showSigDialog: false,
      signatureName: '',
      witness1: '',
      witness2: '',
      section: 'Summary',
      arr1: [],
      arr2: [],
      documents: [
        { document: 'DocName', arrayName: 'arrName' },

        // { document: 'Spouse Proof of Age', arrayName: 'spouseProofOfAgeFiles' },

        // {
        //   document: 'Spouse Proof of Disability',
        //   arrayName: 'spouseProofOfDisabilityFiles',
        // },
      ],
      unListenHistoryHandler: null,
    };
    this.handleClose = this.handleClose.bind(this);
  }
  componentDidMount = async () => {
    //  window.scrollTo(0, 0);
    this.props.setVideoCode(this.props.defaultHelpVideoUrl);
    // this.props.callHelpVideo(null, '380143715');
    const { history } = this.props;

    if (history) {
      const unListenHistoryHandler = history.listen((newLocation, action) => {
        //Get lastTab to prevent app from going /home on back button press
        // if (action === 'POP');
        // {
        //   this.toggleDialog();
        //   history && history.block();
        // }
      });
      this.setState({ unListenHistoryHandler });
    }

    this.sendNotification(
      getFileArraysFromFileMetadataList(
        this.props.filesMetadata,
        'portalSection',
        this.state.section
      )
    );
  };

  componentWillUnmount() {
    this.toggleDialog();
    if (this.state.unListenHistoryHandler) this.state.unListenHistoryHandler();
  }

  componentDidUpdate = async (prevProps, prevState) => {
    if (this.props.readOnlyMode && !this.props.editMode) {
      this.props.onNavigateToSummary();
    }
  };

  sendNotification = async arrayContainer => {
    if (this.props.contact && this.props.seniorApp) {
      const uploadsData = await updateUploadsData(
        this.props.contact,
        this.props.seniorApp.seapplicationid,
        null,
        this.state.section,
        [],
        this.props.uploadsData,
        this.props.sendMessage,
        this.props.setUpload
      );

      this.props.setUpload(uploadsData);
    }
  };

  handleContinueClick = async () => {
    window.scrollTo(0, 0);
    await this.getRedirectedDocuSign();
  };

  handleBackClick = async () => {
    window.scrollTo(0, 0);
    await this.getRedirectedDocuSign();
  };

  updateSeniorApp = async () => {
    let seniorApp = { ...this.props.seniorApp };
    seniorApp.signatureconfirmed = true;
    seniorApp.signatureline = this.state.signatureName;
    var d = new Date();
    seniorApp.signaturedate = d.toUTCString();

    await this.props.updateSeniorApp(seniorApp);
  };

  getRedirectedDocuSign = async () => {
    const seniorAppIdUrl = `${process.env.REACT_APP_DOCUSIGN_REDIRECT}/${this.props.seniorApp.seapplicationid}`;
    //const htmlToBeSent = this.props.htmlSummary;

    //getting it directly from the localStorage
    const htmlToBeSent = JSON.parse(
      localStorage.getItem(
        `${this.props.seniorApp.seapplicationid}_DocuSignHTML`
      )
    );

    this.props.showSavingOverlay();

    this.setState({ enableSubmitButton: false }, async () => {
      const result = await getRedirectedDocuSignUrlHtml(
        htmlToBeSent,
        `${this.props.contact.firstname} ${this.props.contact.lastname}`,
        this.props.contact.emailaddress,
        seniorAppIdUrl,
        this.props.contact.contactid
      );

      if (result) {
        if (result.error) {
          this.setState({
            errorMessage: result.errorMessage,
            enableSubmitButton: true,
          });

          this.props.hideSavingOverlay();
        } else {
          window.location = result.redirectUrl;
        }
      } else {
        this.props.hideSavingOverlay();
        this.setState({
          errorMessage: 'There was a problem connecting to DocuSign',
          enableSubmitButton: true,
        });
      }
    });
  };

  handleClose() {
    this.setState({
      infoOpen: false,
      continueTextSum: fm.finishApplication,
    });
  }

  openDialog = option => {
    this.setState({ showSigDialog: true });
    /* Push to mouseflow as another page state */
    window._mfq = window._mfq || [];
    window._mfq.push(['newPageView', 'signature-popup']);
  };
  toggleDialog = option => {
    this.setState({ showSigDialog: false });
  };

  handleSuccess = async e => {
    //this.setState({ redirectHome: true });

    this.toggleDialog();
    this.props.showSavingOverlay();

    await this.updateSeniorApp();

    await this.props.submitSeniorApp(this.props.seniorApp);

    this.props.toggleRedirectToSeniors();

    //await this.props.getSeniorApps();
    this.props.hideSavingOverlay();
    this.props.applicationSigned();

    this.setState({
      showSuccessDialog: true,
      showSigDialog: false,
      redirectHome: true,
    });
  };
  handleClickDialog = e => {
    this.setState({ showSigDialog: !this.state.showSigDialog });
  };
  onChange = e => {
    const signatureName = trimSecure(e.target.value);
    this.setState({ signatureName });
  };
  onChangeWit1 = e => {
    this.setState({ witness1: e.target.value });
  };
  onChangeWit2 = e => {
    this.setState({ witness2: e.target.value });
  };
  render() {
    return (
      <React.Fragment>
        {this.state.showSigDialog ? (
          <DialogSmall
            maxWidth={true}
            onClose={this.toggleDialog}
            aria-labelledby="customized-dialog-title"
            scroll={'paper'}
            open={this.state.showSigDialog}
            style={{ top: -20, overflow: 'auto' }}
            className={'no-focus'}
          >
            <IconButton
              onClick={this.toggleDialog}
              style={{ right: '0%', position: 'absolute' }}
            >
              <ClearIcon
                style={{ color: 'black', width: '2em', height: '2em' }}
              />
            </IconButton>

            <DialogContent className="no-focus">
              <DialogContentText id="scroll-dialog-description" tabIndex={-1}>
                <div className="signature-terms">
                  <h3>{fm.signInfoHeader}</h3>
                  <ul>
                    <li>{fm.signInfoItem1}</li>
                    <li>{fm.signInfoItem2}</li>
                    <li>{fm.signInfoItem3}</li>
                    <li>{fm.signInfoItem4}</li>
                  </ul>
                </div>
                <div style={{ textAlign: 'center', marginBottom: '32px' }}>
                  <BorderColorIcon className="signature-icon" />
                  <CssTextField
                    value={this.state.signatureName1}
                    label={fm.fullName1}
                    helperText={fm.dialogEnterYourSignature1}
                    onChange={this.onChange}
                    className="signature-input-text"
                    inputProps={{
                      style: { fontSize: 24 },
                      'data-testid': 'fullName',
                    }}
                    InputLabelProps={{
                      style: { fontSize: 24 },
                    }}
                    /* InputProps={{
                      readOnly: this.props.readOnly,
                    }}*/
                  ></CssTextField>
                </div>
                {/* <div style={{ textAlign: 'center', marginBottom: '32px' }}>
                  <TextField
                    value={this.state.witness1}
                    label={fm.witness1}
                    helperText={fm.witnessSignature1}
                    onChange={this.onChangeWit1}
                    className="signature-input-text"
                    inputProps={{
                      style: { fontSize: 24 },
                      'data-testid': 'witness1',
                    }}
                    InputLabelProps={{
                      style: { fontSize: 24 },
                    }}
                  
                  ></TextField>
                </div>
                <div style={{ textAlign: 'center', marginBottom: '32px' }}>
                  <TextField
                    value={this.state.witness2}
                    label={fm.witness2}
                    helperText={fm.witnessSignature1}
                    onChange={this.onChangeWit2}
                    className="signature-input-text"
                    inputProps={{
                      style: { fontSize: 24 },
                      'data-testid': 'witness2',
                    }}
                    InputLabelProps={{
                      style: { fontSize: 24 },
                    }}
                   
                  ></TextField>
                </div> */}
                <div style={{ textAlign: 'center' }}>
                  <CustomButton
                    testid="signFinal"
                    style={{
                      margin: '10px 0',
                    }}
                    onClick={this.handleSuccess}
                    label={'Sign and finalize'}
                    disabled={
                      this.state.signatureName.length == 0
                      // this.state.signatureName.length == 0 ||
                      // this.state.witness1.length == 0 ||
                      // this.state.witness2.length == 0
                    }
                  />
                </div>
                <p className="signature-info-text text-center">
                  {fm.dialogFooterText}
                </p>
              </DialogContentText>
            </DialogContent>
          </DialogSmall>
        ) : (
          ''
        )}
        {/*<CustomContentDialog
          showContentDialog={this.state.showSuccessDialog}
          toggleDialog={this.handleSuccess}
          hidePrint
          small={true}
          dialogTitle={fm.submissionSuccessful}
        >
          <DialogContent>
            <p style={{ color: 'black' }}>{fm.submissionSuccessfulDesc}</p>
          </DialogContent>
          <DialogActions>
            <CustomButton
              testid={'goHome'}
              label={'Go Home'}
              onClick={this.handleSuccess}
            />
          </DialogActions>
        </CustomContentDialog>*/}
        {renderIf(
          this.state.redirectHome,
          <div>
            <Redirect
              push
              to={{
                pathname: '/home',
              }}
            />
          </div>
        )}
        <TabContainer style={{ fontSize: '12px' }} nomargin>
          <div className="summary-top-container">
            <b>{fm.whatToDoOnThisPageTitle}</b>
            <p>{fm.whatToDoOnThisPageText1}</p>
          </div>
          <CustomIFrame
            summaryHtml={this.props.htmlSummary}
            id="summary"
            seniorApp={this.props.seniorApp}
          />
        </TabContainer>
        {!this.state.showSigDialog && !this.state.showSuccessDialog ? (
          <div className="signing-message">
            <div>
              <p style={{ color: 'red', margin: '5px' }}>
                {this.state.errorMessage}
              </p>
              <div
                style={{
                  paddingTop: '4px',
                }}
              >
                <CustomButton
                  testid={'signApp'}
                  secondary={true}
                  btnBigLabel={true}
                  onClick={this.openDialog}
                  label={this.state.continueTextSum}
                />
              </div>
            </div>
          </div>
        ) : (
          ''
        )}
      </React.Fragment>
    );
  }
}

export default withRouter(Summary);
