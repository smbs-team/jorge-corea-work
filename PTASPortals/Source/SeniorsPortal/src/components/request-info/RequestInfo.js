//-----------------------------------------------------------------------
// <copyright file="RequestInfo.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import * as fm from './FormatTexts';
import './RequestInfo.css';
import { renderIf, arrayNullOrEmpty } from '../../lib/helpers/util';
import { hasAgeToQualify } from '../../lib/helpers/age';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import CardHeader from '@material-ui/core/CardHeader';
import TabContainer from '../common/TabContainer';
import TextInputML from '../common/TextInputML';
import EmailInput from '../common/EmailInput';
import PhoneInput from '../common/PhoneInput';
import DateInput from '../common/DateInput';
import UploadFile from '../common/UploadFile/UploadFile';
import ExpandLink from '../common/ExpandLink';
import ShortDivider from '../common/ShortDivider';
import SwitchInput from '../common/SwitchInput';
import CustomWarning from '../common/CustomWarning';
import { FormattedMessage } from 'react-intl';
import Collapse from '@material-ui/core/Collapse';
import CustomButton from '../common/CustomButton';
import SummaryButton from '../common/SummaryButton';
import CustomDialog from '../common/dialog-component/CustomDialog';
import { Redirect } from 'react-router-dom';
import { withRouter } from 'react-router-dom';
import SelectInput from '../common/SelectInput';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';

import CardSide from '../common/CardSide';
import {
  moveFilesToiLinx,
  uploadDocuSignDocuments,
} from '../../services/blobService';

import VimeoSlider from '../common/vimeo-player/VimeoSlider';
//import { updateSeniorAppDetail } from '../../services/dataServiceProvider';
import { updateSeniorAppDetail } from '../../services/dynamics-service';

import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import Grid from '@material-ui/core/Grid';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import Input from '@material-ui/core/Input';
import MenuItem from '@material-ui/core/MenuItem';
import Select from '@material-ui/core/Select';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import WarningIcon from '@material-ui/icons/Warning';

import { CollectionConsumer } from '../../contexts/CollectionContext';

import {
  generateBlobUriObjectFromAFileArray,
  getFileArraysFromFileMetadataList,
  getFileArrayFromBlobUrl,
} from '../../lib/data-mappings/fileMetadataToFileArray';
import deepEqual from 'deep-equal';
import { cloneDeep, groupBy } from 'lodash';

import {
  createOrUpdateFileMetadataEntities,
  createFileArraysFromMetadata,
} from '../common/UploadFile/uploaderHelper';
import { createImageFromCurrentPage } from '../../lib/helpers/file-manipulation';

import AlertDialog from '../common/AlertDialog';

import CloseIcon from '@material-ui/icons/Close';
import IconButton from '@material-ui/core/IconButton';
//About placeholder translations issue
//https://github.com/formatjs/react-intl/issues/908

import { applyJsonAndMoveToPermanentStorage } from '../../services/blobService';

const FormControlCSS = withStyles({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
  },
})(FormControl);

const ShortDividerCSS = withStyles({
  root: {
    width: '631px',
    marginTop: '30px',
    marginBottom: '30px',
  },
})(ShortDivider);
const SwitchInputCSS = withStyles({
  label: {
    width: '140px',
  },
  infoIcon: {
    bottom: '35px',
    left: '15px',
    position: 'relative',
  },
})(SwitchInput);

const DialogCSS = withStyles({
  root: {
    '& .MuiDialog-paperWidthSm': {
      maxWidth: '1078px',
      width: '100%',
      height: 'auto',
    },
  },
})(Dialog);

// This component will have all the UI components that will show under the My Info tab
class RequestInfo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      section: '',
      currentUser: {},
      firstName: '',
      requiredMissing: [],
      middleName: '',
      lastFamilyName: '',
      continueText: fm.continueLabel,
      isSaving: false,
      redirect: false,
      suffix: '',
      firstRequired: null,
      dateOfBirth: null,
      proofOfAgeFiles: [],
      belowAge: false,
      disabledVeteran: false,
      canApplyLegalProofDisability: false, //true if age < 61
      legalProofDisability: false,
      proofOfDisabilityFiles: [],
      veteranSpouse: false,
      spouseInformation: false,
      spouseFirstName: '',
      spouseMiddleName: '',
      spouseLastFamilyName: '',
      spouseSuffix: '',
      spouseDateOfBirth: null,
      spouseProofOfAgeFiles: [],
      spouseBelowAge: false,
      f1040YourIRS1040: [],
      fa_IRSFileArray: [],
      fez_IRSFileArray: [],
      f1099_IncomeFileArray: [],
      f8829FileArray: [],
      f8949FileArray: [],
      expensesFileArray: [],
      shareCertificateFileArray: [],
      proofOfChangeFileArray: [],
      proofOfOwnershipFileArray: [],
      missingDocumentsRequired: [],
      spouseDisabledVeteran: false,
      canSpouseApplyLegalProofDisability: false, //true if age < 61
      spouseLegalProofDisability: false,
      spouseProofOfDisabilityFiles: [],
      spouseProofO8949Files: [],

      pendingSchedule1Files: [],
      pendingScheduleCFiles: [],
      pendingScheduleDFiles: [],
      pendingScheduleEFiles: [],
      pendingScheduleFFiles: [],

      spouseVeteranSpouse: false,
      pendingDocumentsArr: false,
      ckdCanContinue: true,

      pendingproofofAge: false,
      pendingSpouseproofofAge: false,
      pendingproofofDisability: false,
      pendingSpouseproofofDisability: false,
      pendingproof1040: false,
      pendingproof1040A: false,
      pendingproof1040EZ: false,
      pendingproof1099: false,
      pendingproof8829: false,
      pendingproof8949: false,
      pendingproofCoOp: false,
      pendingExpenses: false,
      pendingproofLiveChange: false,
      pendingproofOwnership: false,

      pendingSchedule1: false,
      pendingScheduleC: false,
      pendingScheduleD: false,
      pendingScheduleE: false,
      pendingScheduleF: false,
      continueClicked: false,
      emailAddress: null,
      phoneNumber: null,
      textSmsCapable: true,
      infoOpen: false,
      infoSaved: false,
      dialogAlreadyShowed: false,
      showDialog: false,
      showDialogType: 0,
      showDialogDOBMSG: false,
      showDialogPOD: false,
      showDialogWAMSG: false,
      helperText: '',
      warned: false,
      open: false,
      alertText: false,

      // link between the document and the array to extract the files and match the Upload control accordingly.
      documents: [],
      fileMetaDataUpdated: false,
      canContinue: false,
    };

    this.handleClickOpen = this.handleClickOpen.bind(this);
    this.handleCloseContinue = this.handleCloseContinue.bind(this);
    this.handleClose = this.handleClose.bind(this);
    this.closeDialog = this.closeDialog.bind(this);
  }

  handleCloseContinue() {
    this.setState({ showDialog: false });
  }
  handleClickOpen() {
    this.setState({ infoOpen: true });
  }

  handleClose() {
    this.setState({ infoOpen: false });
  }

  componentDidMount = async () => {
    this.props.hideToolbar(true);
    this.props.showmoreinfotitle(true);
    window.scrollTo(0, 0);
    if (!arrayNullOrEmpty(this.props.filesMetadata)) {
      this.setState({ fileMetaDataUpdated: true }, () => {
        this.setFileArrays(this.createDocumentList());
      });
    } else {
      this.createDocumentList();
    }
  };

  componentDidUpdate(prevProps, prevState) {
    if (
      !arrayNullOrEmpty(this.props.filesMetadata) &&
      !deepEqual(prevProps.filesMetadata, this.props.filesMetadata) &&
      !this.state.fileMetaDataUpdated
    ) {
      this.setState({ fileMetaDataUpdated: true }, () => {
        if (this.state.documents && this.state.documents.length === 0) {
          this.setFileArrays(this.createDocumentList());
        } else {
          this.setFileArrays(this.state.documents);
        }
      });
    }
  }

  createDocumentList = () => {
    const documentList = [];
    let arraysOnState = {};
    this.props.seniorApp.seApplicationPredefNotes &&
      this.props.seniorApp.seApplicationPredefNotes.forEach(n => {
        n.ptas_showonportal &&
          documentList.push({
            id: n.ptas_seapppredefnotesid,
            document: n.ptas_description,
            array: [],
            arrayName: n.ptas_seapppredefnotesid,
            section: n.ptas_portalattachmentlocation
              ? this.props.portalAttachmentLocations.filter(
                  p => p.attributeValue === n.ptas_portalattachmentlocation
                )[0].value
              : 'My Info',
          });
        arraysOnState[n.ptas_seapppredefnotesid] = [];
      });

    arraysOnState.documents = documentList;
    this.setState(arraysOnState);
    return documentList;
  };

  checkIfUserCanContinue = () => {
    let canContinue = true;
    if (!(this.state.documents && this.state.documents.length > 0)) {
      return false;
    }

    this.state.documents.forEach(d => {
      if (this.state[d.arrayName] && this.state[d.arrayName].length === 0) {
        canContinue = false;
      }
    });

    return canContinue;
  };

  handleMetadataUpdate = async (array, documentObject) => {
    await createOrUpdateFileMetadataEntities(
      this.props.filesMetadata,
      documentObject.section,
      documentObject.document,
      this.props.seniorApp.seapplicationid,
      this.props.seniorAppDetails[0].seappdetailid,
      this.props.seniorApp.accountnumber,
      array,
      documentObject.arrayName,
      this.props.getFilesMetadata
    );

    let state = {};
    state[documentObject.arrayName] = array;
    this.setState(state);
  };

  setFileArrays = async documentList => {
    const groupedDocuments = groupBy(documentList, n => {
      return n.section;
    });

    const promises = Object.keys(groupedDocuments).map(async sectionGroup => {
      const fileArray = await createFileArraysFromMetadata(
        false,
        this.props.filesMetadata,
        this.props.seniorApp.seapplicationid,
        this.props.seniorAppDetails[0].seappdetailid,
        sectionGroup,
        groupedDocuments[sectionGroup]
      );
      if (fileArray && fileArray.rootFileArrays) {
        this.setState(fileArray.rootFileArrays);
      }
    });

    await Promise.all(promises);
  };

  renderRedirect = () => {
    if (this.state.redirect) {
      return <Redirect to="/home" />;
    }
  };

  updateSeniorApp = async seniorApp => {
    await this.props.updateSeniorApp(seniorApp, null, true);
  };

  handleContinueClick = async e => {
    this.props.showSavingOverlay();
    window.scrollTo(0, 0);
    // Update to Taxpayer submitted documents status.
    let documentsSubmittedStatus = 668020028;

    for (var i = 0; i < this.props.seniorAppDetails.length; i++) {
      if (this.props.seniorAppDetails[i]) {
        let newDetail = { ...this.props.seniorAppDetails[i] };
        newDetail.statuscode = documentsSubmittedStatus;

        await updateSeniorAppDetail(newDetail);
      }
    }

    let seniorApp = cloneDeep(this.props.seniorApp);
    seniorApp.statuscode = documentsSubmittedStatus;

    await this.updateSeniorApp(seniorApp);

    this.setState({ redirect: true });

    this.props.hideSavingOverlay();
    this.renderRedirect();
    this.props.history.push('/home');
    let yearObjectArray = this.props.yearsToApplyArrayObject;

    let selectedYear = yearObjectArray.filter(
      y => y.seniorAppId === this.props.seniorApp.seapplicationid
    )[0];
    applyJsonAndMoveToPermanentStorage(
      this.props.contact.contactid,
      this.props.seniorApp.seapplicationid,
      this.props.seniorApp.accountnumber,
      `${selectedYear.name} Requested Documents`,
      selectedYear.name
    );
    this.props.hideSavingOverlay();
  };

  closeDialog() {
    this.setState({
      infoOpen: false,
      requiredMissing: [],

      dialogAlreadyShowed: true,
      //continueText: fm.continueLabel,
    });
    if (this.state.firstRequired && this.refs[this.state.firstRequired]) {
      setTimeout(() => {
        this.refs[this.state.firstRequired].scrollIntoView({
          block: 'center',
          behavior: 'smooth',
        });
      }, 100);
    }
  }

  render() {
    // removed because of PBI #130877
    // const isContinueButtonDisabled = !this.checkIfUserCanContinue();
    const requiredMissing = (
      <div className="required-missing">
        <WarningIcon className="required-icon" />
        <ul>
          <p>
            <FormattedMessage
              id="myInfo_toContinue"
              defaultMessage="To continue, you need to enter your information:"
            />
          </p>

          {this.state.requiredMissing.map((field, index) => {
            return (
              <li>
                <a
                  onClick={() => {
                    this.refs[field] &&
                      this.refs[field].scrollIntoView({
                        block: 'center',
                        behavior: 'smooth',
                      });
                  }}
                  href={'#' + field}
                >
                  {fm[field]}
                </a>
              </li>
            );
          })}
        </ul>
      </div>
    );
    return (
      <React.Fragment>
        <Collapse
          className="required-popup"
          in={this.state.infoOpen}
          style={{
            top:
              this.state.requiredMissing.length > 0 &&
              this.refs[this.state.requiredMissing[0]]
                ? this.refs[this.state.requiredMissing[0]].offsetTop - 200
                : 0,
          }}
        >
          <IconButton onClick={this.closeDialog} className="required-close">
            <ClearIcon className="required-close-icon" />
          </IconButton>
          {requiredMissing}
        </Collapse>
        <AlertDialog
          text={this.state.alertText}
          isOpen={this.state.open}
          onClose={() => this.setState({ open: false })}
        />
        <TabContainer>
          <CardSide
            header={fm.attachingDocuments}
            content={[
              fm.WeNeedToConfirmAmounts,
              <VimeoSlider videos={[{ url: '382824186' }]} />,
              fm.selectPhotoOfDocument,
              fm.hideSocialSecurity,
            ]}
          ></CardSide>

          <div className="center-panel center-content center">
            <Card className="cardStyle">
              <div className="financialHeader">
                <div
                  style={{
                    font: 14,
                    fontWeight: 'bold',
                    marginLeft: '10px',
                    padding: '8px',
                    textAlign: 'left',
                  }}
                >
                  <Grid container spacing={20}>
                    <Grid sm={12}>
                      <strong style={{ paddingBottom: 5, textAlign: 'left' }}>
                        <p>Documents to attach</p>
                      </strong>
                    </Grid>
                  </Grid>
                </div>
              </div>
              <div
                style={{
                  marginLeft: 0,
                  textAlign: 'justify',
                  padding: 50,
                  fontSize: 14,
                }}
              >
                <p>{fm.youMayObscureText}</p>
                <a
                  onClick={() => {
                    this.setState({
                      open: true,
                      alertText: helpTextObscure,
                    });
                  }}
                  style={{ cursor: 'pointer' }}
                >
                  {fm.whatShouldIObscureText}
                </a>
              </div>
              <div
                style={{
                  marginLeft: 0,
                  textAlign: 'justify',
                  padding: '0 50px 0 50px',
                  fontSize: 14,
                }}
              >
                <p>{fm.documentsNotSubmittedText}</p>
              </div>

              <CardContent className="center-myname-panel">
                {renderIf(
                  this.props.seniorApp.seApplicationPredefNotes &&
                    this.props.seniorApp.seApplicationPredefNotes.length > 0 &&
                    this.state.documents.length > 0,
                  this.props.seniorApp.seApplicationPredefNotes.map(n => {
                    let document = this.state.documents.filter(
                      d => d.id === n.ptas_seapppredefnotesid
                    )[0];
                    if (n.ptas_showonportal)
                      return (
                        <div
                          style={{ paddingBottom: 30 }}
                          key={n.ptas_seapppredefnotesid}
                        >
                          <UploadFile
                            detailsId={
                              this.props.seniorAppDetails[0].seappdetailid
                            }
                            leftMessage={document ? document.document : ''}
                            customClass={
                              this.state.continueClicked && document
                                ? document.array.length === 0
                                : false
                                ? 'red-border'
                                : null
                            }
                            obscureInfoMessage={false}
                            fileArray={
                              document ? this.state[document.arrayName] : []
                            }
                            section={document ? document.section : ''}
                            document={document ? document.document : ''}
                            appId={
                              this.props.seniorApp
                                ? this.props.seniorApp.seapplicationid
                                : ''
                            }
                            onCreate={array =>
                              this.handleMetadataUpdate(array, document)
                            }
                            onDelete={array =>
                              this.handleMetadataUpdate(array, document)
                            }
                          />
                        </div>
                      );
                  })
                )}
              </CardContent>
            </Card>
            <div
              style={{
                width: 900,
                marginLeft: -130,
              }}
              className="continue-summary-panel  text-center"
            >
              <p className="text-center necesarry-docs-text">
                {fm.beforeContinueText}
                <br></br>
              </p>
            </div>
            <div className="card-end">
              {renderIf(this.state.requiredMissing.length > 0, requiredMissing)}
            </div>
            <div
              style={{
                width: 900,
                marginLeft: -130,
              }}
              className="continue-summary-panel  text-center"
            >
              <CustomButton
                style={{ marginTop: 10, marginBottom: 90 }}
                className="display-inline"
                btnBigLabel={true}
                onClick={this.handleContinueClick}
                label={this.state.continueText}
                // remove because of PBI #130877
                // disabled={isContinueButtonDisabled}
              />
            </div>
          </div>
        </TabContainer>
      </React.Fragment>
    );
  }
}

const helpTextObscure = (
  <React.Fragment>
    <p>{fm.helpText1}</p>
    <p>{fm.helpText2}</p>
    <p>{fm.helpText3}</p>
  </React.Fragment>
);
export default withRouter(RequestInfo);
