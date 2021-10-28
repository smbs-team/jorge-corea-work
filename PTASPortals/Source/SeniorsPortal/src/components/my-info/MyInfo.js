//-----------------------------------------------------------------------
// <copyright file="MyInfo.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import * as fm from './FormatTexts';
import './MyInfo.css';
import {
  renderIf,
  arrayNullOrEmpty,
  isValidDate,
} from '../../lib/helpers/util';
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
import VimeoSlider from '../common/vimeo-player/VimeoSlider';
import CustomWarning from '../common/CustomWarning';
import { FormattedMessage } from 'react-intl';
import Collapse from '@material-ui/core/Collapse';
import CustomButton from '../common/CustomButton';
import SummaryButton from '../common/SummaryButton';
import CustomDialog from '../common/dialog-component/CustomDialog';
import SelectInput from '../common/SelectInput';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import Grid from '@material-ui/core/Grid';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import Input from '@material-ui/core/Input';
import MenuItem from '@material-ui/core/MenuItem';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import WarningIcon from '@material-ui/icons/Warning';
import { CollectionConsumer } from '../../contexts/CollectionContext';
import RadioInput from '../common/RadioInput';
import { AuthContext, AuthConsumer } from '../../contexts/AuthContext';
import { getFileArraysFromFileMetadataList } from '../../lib/data-mappings/fileMetadataToFileArray';
import deepEqual from 'deep-equal';
import { cloneDeep } from 'lodash';
import { trimSecure } from '../../lib/helpers/trimText';

import {
  createOrUpdateFileMetadataEntities,
  createFileArraysFromMetadata,
} from '../common/UploadFile/uploaderHelper';
import {
  updateUploadsData,
  setUploadsData,
  setDocumentArray,
} from '../common/UploadFile/signalRHelper';
import { createImageFromCurrentPage } from '../../lib/helpers/file-manipulation';
import { Hidden } from '@material-ui/core';
import {
  determinateYearsToApply,
  generateRadioButtomSource,
} from '../../lib/helpers/age';
import {
  updateSeniorAppDetail,
  getSeniorAppDetailsBySeniorAppId,
} from '../../services/dataServiceProvider';
import CardSide from '../common/CardSide';
import { postValue, getValue } from '../../services/jsonStoreService';
import AlertPopUpYears from '../common/AlertPopUpYears';
import { getAppInsightsInstance } from '../../services/telemetryService';
import { array } from 'prop-types';

//About placeholder translations issue
//https://github.com/formatjs/react-intl/issues/908

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

const RadioInputCss = withStyles({
  root: {
    marginTop: '19px',
  },
  label: {
    maxWidth: '310px',
    fontSize: '14px',
  },
})(RadioInput);

const appInsights = getAppInsightsInstance();

// This component will have all the UI components that will show under the My Info tab
class MyInfo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      section: 'My Info',
      currentUser: {},
      firstName: '',
      middleName: '',
      lastFamilyName: '',
      continueText: fm.continueLabel,
      isSaving: false,
      suffix: '',
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
      // spouseDateOfBirth: null,
      // spouseProofOfAgeFiles: [],
      spouseBelowAge: false,
      spouseDisabledVeteran: false,
      canSpouseApplyLegalProofDisability: false, //true if age < 61
      spouseLegalProofDisability: false,
      spouseProofOfDisabilityFiles: [],
      spouseVeteranSpouse: false,
      checkedCanContinue: false,
      emailAddress: null,
      phoneNumber: null,
      textSmsCapable: true,
      infoOpen: true,
      infoSaved: false,
      dialogAlreadyShowed: false,
      showDialog: false,
      showDialogType: 0,
      showDialogDOBMSG: false,
      showDialogPOD: false,
      showDialogWAMSG: false,
      helperText: '',
      warned: false,
      // link between the document and the array to extract the files and match the Upload control accordingly.
      documents: [
        { document: 'Proof of Age', arrayName: 'proofOfAgeFiles' },
        {
          document: 'Proof of Disability',
          arrayName: 'proofOfDisabilityFiles',
        },
        {
          document: 'Decree',
          arrayName: 'decreeFiles',
        },
        {
          document: 'VA compensation or Award letter',
          arrayName: 'VACompensationLetterFiles',
        },
        {
          document: 'SSA Award Letter or Disability form',
          arrayName: 'SSAAwardLetterFormFiles',
        },
      ],
      displayYearsOptions: false,
      requiredMissing: [],
      requiredPopup: true,
      uploading: false,
      inputTop: 0,
      inputLeft: 0,
      sectionsVideoPlayedArray: [],
      notifying: false,
      survivingSpouse: false,
      married: false,
      single: false,
      widowed: false,
      divorced: false,
      marriedLiving: false,
      veteranSpouseDisability: false,
      not61WithDisabilityNotice: false,
      disabilityEffectiveDate: null,
      decreeFiles: [],
      VACompensationLetterFiles: [],
      SSAAwardLetterFormFiles: [],
    };

    this.handleClickOpen = this.handleClickOpen.bind(this);
    this.handleCloseContinue = this.handleCloseContinue.bind(this);
    this.handleClose = this.handleClose.bind(this);
    this.closeDialog = this.closeDialog.bind(this);
  }

  handleCloseContinue() {
    this.setState({ checkedCanContinue: false });
    this.setState({ showDialog: false });
    this.props.hideSavingOverlay();
  }
  handleClickOpen() {
    this.setState({ infoOpen: true });
  }
  handleClose() {
    this.setState({ infoOpen: false });
  }

  componentDidMount = async () => {
    window.scrollTo(0, 0);
    // this.sendNotification();

    // Birth date should be srt by default to a valid date, so current - min allowed date
    let currentDate = new Date();
    let birthDate = new Date(
      currentDate.getFullYear() - process.env.REACT_APP_AGE_LIMIT,
      currentDate.getMonth(),
      currentDate.getDate(),
      0,
      0,
      0,
      0
    );

    this.props.setVideoCode(this.props.defaultHelpVideoUrl);
    this.setContactInfo(this.props.contact);
    this.setSeniorAppInfo(this.props.seniorApp);
    this.sendNotification(
      getFileArraysFromFileMetadataList(
        this.props.filesMetadata,
        'portalSection',
        this.state.section
      )
    );
    this.setFileArrays();
    // this.props.callHelpVideo(null, '380143715');
    //this.props.checkIfVideoPlayed('MyInfo');
    //removeProduction
    //this.checkIfVideoPlayed();
  };

  sendNotification = async arrayContainer => {
    if (this.props.contact && this.props.seniorApp) {
      let documents = cloneDeep(this.state.documents);
      let isDisable = false;
      if (
        isValidDate(this.state.dateOfBirth) &&
        !hasAgeToQualify(this.state.dateOfBirth) &&
        this.state.legalProofDisability
      ) {
        isDisable = true;
      }
      documents.map(d => {
        if (d.document === 'Proof of Disability') {
          d.isVisible = isDisable;
        } else {
          d.isVisible = true;
        }

        d.files = arrayContainer[d.arrayName]
          ? cloneDeep(arrayContainer[d.arrayName])
          : [];
      });

      const uploadsData = await updateUploadsData(
        this.props.contact,
        this.props.seniorApp.seapplicationid,
        null,
        this.state.section,
        documents,
        this.props.uploadsData,
        this.props.sendMessage,
        this.props.setUpload
      );

      this.props.setUpload(uploadsData);
    }
  };
  // sendNotification = async () => {
  //   if (this.props.seniorApp) {
  //     let fileMeta = await this.props.getFilesMetadata(
  //       this.props.seniorApp.seapplicationid
  //     );

  //     if (fileMeta.length === 0) {
  //       let updatedFiles = this.state.proofOfDisabilityFiles;
  //       let document = this.state.documents[1];
  //       let updatedFilesPOA = this.state.proofOfAgeFiles;
  //       let documentPOA = this.state.documents[0];
  //       // alert(4);
  //       this.notifyChangeOnFilesMulti(
  //         [updatedFilesPOA],
  //         [documentPOA],
  //         [true, true],
  //         true
  //       );
  //     } else {
  //       let toUpdateArray = [
  //         //this.state.legalProofDisability ? 'Proof of Disability' : '',
  //         //!this.state.belowAge ? this.state.documents[0].document : '',
  //         // this.state.belowAge ? this.state.documents[1].document : '',
  //       ];
  //       if (!this.state.belowAge) {
  //         toUpdateArray.push(this.state.documents[0].document);
  //       }
  //       if (this.props.seniorApp.docdisability) {
  //         toUpdateArray.push(this.state.documents[1].document);
  //       }

  //       await updateUploadsDataMultinoB64(
  //         fileMeta,
  //         this.props.contact,
  //         this.props.seniorApp,
  //         this.state.section,
  //         '',
  //         toUpdateArray
  //       );

  //       this.props.sendMessage('documentUpdated');
  //     }
  //   }
  // };
  checkIfVideoPlayed = () => {
    this.props.checkIfVideoPlayed(this.state.section).then(data => {
      if (!data) {
        this.props.callHelpVideo();
      }
    });
  };
  componentDidUpdate = async (prevProps, prevState) => {
    if (this.props.readOnlyMode && !this.props.editMode) {
      this.props.onNavigateToSummary();
    }

    if (
      this.props.contact &&
      !deepEqual(prevProps.contact, this.props.contact)
    ) {
      this.setContactInfo(this.props.contact);
    }

    if (
      this.props.seniorApp &&
      !deepEqual(prevProps.seniorApp, this.props.seniorApp)
    ) {
      this.setSeniorAppInfo(this.props.seniorApp);
    }

    if (
      arrayNullOrEmpty(this.state.proofOfAgeFiles) &&
      !arrayNullOrEmpty(this.props.filesMetadata) &&
      //!deepEqual(prevProps.filesMetadata, this.props.filesMetadata)
      arrayNullOrEmpty(prevProps.filesMetadata) &&
      !arrayNullOrEmpty(this.props.filesMetadata)
    ) {
      // await this.setFileArraysPlaceHolder();
      this.setFileArrays();
    }

    if (!deepEqual(prevProps.signalRMessages, this.props.signalRMessages)) {
      // currently only messages to indicate new files are being added, so changes on the collection means new file messages.

      await setUploadsData(
        this.props.contact.contactid,
        this.props.seniorApp.seapplicationid,
        this.state.section,
        this.state.documents,
        this.handleMetadataUpdate,
        this.props.setUpload,
        this.setFileArrayPlaceHolder
      );
    }

    if (!deepEqual(prevProps.filesMetadata, this.props.filesMetadata)) {
      await this.sendNotification(
        getFileArraysFromFileMetadataList(
          this.props.filesMetadata,
          'portalSection',
          this.state.section
        )
      );

      await this.setFileArrays();
    }

    if (prevState.legalProofDisability !== this.state.legalProofDisability) {
      await this.sendNotification(
        getFileArraysFromFileMetadataList(
          this.props.filesMetadata,
          'portalSection',
          this.state.section
        )
      );
    }
  };

  setFileArrayPlaceHolder = (document, files) => {
    const currentDoc = this.state.documents.filter(
      d => d.document === document
    )[0];
    const currentArrayFileNames = this.state[currentDoc.arrayName].map(
      f => f.imageName
    );

    const filteredResults = files.filter(
      f => !currentArrayFileNames.includes(f.imageName)
    );

    if (!arrayNullOrEmpty(filteredResults)) {
      this.setState(prevState => {
        return {
          [currentDoc.arrayName]: [
            ...prevState[currentDoc.arrayName],
            ...filteredResults,
          ],
        };
      });
    }
  };

  traceFileArray = () => {
    this.setState({ uploading: true });
  };

  /**
   * Updates the respective file array with the data
   * shared on the cloud.
   * @memberof MyInfo
   */
  /* setUploadsData = async () => {
    const currentUpload = await getValue(
      `${this.props.contact.contactid}/${this.props.seniorApp.seapplicationid}/uploads/uploads`
    );
    if (currentUpload && currentUpload.uploads.length > 0) {
      currentUpload.uploads.map(u => {
        let document = this.state.documents.filter(
          d => d.document === u.document
        )[0];

        // Only update if files are different
        if (!deepEqual(u.files, this.state[document.arrayName])) {
          if (!arrayNullOrEmpty(u.files)) {
            u.files.map(f => {
              f.isDirty = false;
              f.isLoading = false;
              f.isUploaded = true;
              f.isUploading = false;
              f.isValid = true;
            });
          }
          const state = { [document.arrayName]: u.files };
          this.handleMetadataUpdate(u.files, document, false);
          this.setState(state);
        }
      });
    }
  };
*/
  /**
   * Updates the data on the cloud to reflect changes on the
   * exiting file arrays
   * @memberof MyInfo
   */
  /*updateUploadsData = async (updatedFiles, document) => {
    if (this.props.contact) {
      let uploads = {
        fullName: `${
          this.props.contact.firstname ? this.props.contact.firstname : ' '
        } ${
          this.props.contact.middlename ? this.props.contact.middlename : ' '
        } ${this.props.contact.lastname ? this.props.contact.lastname : ' '}`,
        email: this.props.contact.emailaddress,
        contactId: this.props.contact.contactid,
        seniorAppId: this.props.seniorApp.seapplicationid,
        uploads: [],
      };

      if (updatedFiles && document) {
        let upload = {
          name: document.document,
          section: 'My Info',
          document: document.document,
          files: [],
        };
        updatedFiles.map(f => {
          let file = cloneDeep(f);
          file.isDirty = false;
          file.isLoading = false;
          file.isUploaded = true;
          file.isUploading = false;
          file.isValid = true;

          upload.files.push(file);
        });
        uploads.uploads.push(upload);
      } else {
        /*
        if (this.state.belowAge) {
          let upload = {
            name: 'Proof of Disability',
            section: 'My Info',
            document: 'Proof of Disability',
            files: [],
          };

          this.state.proofOfDisabilityFiles.map(f => {
            let file = cloneDeep(f);
            file.isDirty = false;
            file.isLoading = false;
            file.isUploaded = true;
            file.isUploading = false;
            file.isValid = true;

            upload.files.push(file);
          });
          uploads.uploads.push(upload);
        } else {
          let upload = {
            name: 'Proof of Age',
            section: 'My Info',
            document: 'Proof of Age',
            files: [],
          };

          this.state.proofOfAgeFiles.map(f => {
            upload.files.push(f);
          });
          uploads.uploads.push(upload);
        //}OLD END OF COMMENT
      }

      await postValue(
        `${this.props.contact.contactid}/${this.props.seniorApp.seapplicationid}/uploads`,
        uploads,
        null
      );
    }
  };
*/
  setFileArraysPlaceHolder = async () => {
    if (!arrayNullOrEmpty(this.props.filesMetadata)) {
      const arrays = await createFileArraysFromMetadata(
        true,
        this.props.filesMetadata,
        this.props.seniorApp.seapplicationid,
        null,
        this.state.section,
        this.state.documents
      );

      /*this.setState(arrays.rootFileArrays, () => {
        this.setState({
          legalProofDisability:
            !arrayNullOrEmpty(this.state.proofOfDisabilityFiles) ||
            this.state.disabledVeteran,
        });
      });*/
      //console.log('MyInfo setFileArraysPlaceHolder arrays', arrays);
      //return arrays;
      // this.setState({ proofOfAgeFiles: [] });
    }
  };

  setFileArrays = async () => {
    if (!arrayNullOrEmpty(this.props.filesMetadata)) {
      const arrays = await createFileArraysFromMetadata(
        false,
        this.props.filesMetadata,
        this.props.seniorApp.seapplicationid,
        null,
        this.state.section,
        this.state.documents
      );

      this.setState(arrays.rootFileArrays, () => {
        this.setState({
          legalProofDisability:
            !arrayNullOrEmpty(this.state.proofOfDisabilityFiles) ||
            this.state.disabledVeteran,
        });
      });
    }
  };

  setContactInfo = contact => {
    if (contact) {
      this.setState({
        firstName: contact.firstname ? contact.firstname : '',
        lastFamilyName: contact.lastname ? contact.lastname : '',
        emailAddress: contact.emailaddress,
        middleName: contact.middlename ? contact.middlename : '',
        suffix: contact.suffix,
        phoneNumber: contact.phone,
        textSmsCapable: contact.smscapable,
      });

      this.updateBirthDate(contact.birthdate ? contact.birthdate : null);
    }
  };

  setSeniorAppInfo = seniorApp => {
    if (seniorApp) {
      this.setState({
        disabledVeteran: seniorApp.vadisabled,
        spouseDisabledVeteran: seniorApp.disabled,
        veteranSpouse: seniorApp.veteran,
        spouseFirstName: seniorApp.spousefirstname,
        spouseMiddleName: seniorApp.spousemiddlename,
        spouseLastFamilyName: seniorApp.spouselastname,
        spouseSuffix: seniorApp.spousesuffix,
        spouseInformation: seniorApp.hasspouseorpartner,
        survivingSpouse: seniorApp.isasurvivingspouse,
        married: seniorApp.married,
        single: seniorApp.single,
        widowed: seniorApp.widowed,
        divorced: seniorApp.divorcedlegallyseparated,
        marriedLiving: seniorApp.marriedlivingapart,
        veteranSpouseDisability:
          seniorApp.veteranwithserviceevaluationordisability,
        not61WithDisabilityNotice: seniorApp.under61withdisabilitynotice,
        disabilityEffectiveDate: seniorApp.effectivedateofdisability,
      });
      // this.handleSpouseDateChange(seniorApp.spousedob);
      // this.showSpouseSectionOnLoad(seniorApp);
    }
  };

  updateContact = async () => {
    let contact = { ...this.props.contact };
    contact.firstname = this.state.firstName;
    contact.lastname = this.state.lastFamilyName;
    contact.emailaddress = this.state.emailAddress;
    contact.middlename = this.state.middleName;
    contact.suffix = this.state.suffix;
    contact.birthdate = this.state.dateOfBirth;
    contact.phone = this.state.phoneNumber;
    contact.smscapable = this.state.textSmsCapable;

    await this.props.updateContact(contact);
  };

  updateSeniorApp = async () => {
    let seniorApp = { ...this.props.seniorApp };
    seniorApp.vadisabled = this.state.disabledVeteran;
    seniorApp.disabled = this.state.spouseDisabledVeteran;
    seniorApp.veteran = this.state.veteranSpouse;
    seniorApp.taxpayersection = false;
    seniorApp.docageapplicant = !arrayNullOrEmpty(this.state.proofOfAgeFiles);
    seniorApp.docdisability = !arrayNullOrEmpty(
      this.state.proofOfDisabilityFiles
    );

    seniorApp.spousefirstname = this.state.spouseFirstName;
    seniorApp.spousemiddlename = this.state.spouseMiddleName;
    seniorApp.spouselastname = this.state.spouseLastFamilyName;
    seniorApp.spousesuffix = this.state.spouseSuffix;
    seniorApp.hasspouseorpartner = this.state.spouseInformation;

    // Removed by usability changes
    //seniorApp.spousedob = this.state.spouseDateOfBirth;
    // seniorApp.docspouseage = !arrayNullOrEmpty(
    //   this.state.spouseProofOfAgeFiles
    // );

    // New fields from first UAT round, should match values on contact:
    seniorApp.applicantfirstname = this.state.firstName;
    seniorApp.applicantlastname = this.state.lastFamilyName;
    seniorApp.applicantemailaddress = this.state.emailAddress;
    seniorApp.applicantmiddlename = this.state.middleName;
    seniorApp.applicantsuffix = this.state.suffix;
    seniorApp.applicantdateofbirth = this.state.dateOfBirth;
    seniorApp.applicantmobilephone = this.state.phoneNumber;
    seniorApp.isasurvivingspouse = this.state.survivingSpouse;
    seniorApp.married = this.state.spouseInformation; // married use spouseInformation status
    seniorApp.single = this.state.single;
    seniorApp.widowed = this.state.widowed;
    seniorApp.divorcedlegallyseparated = this.state.divorced;
    seniorApp.marriedlivingapart = this.state.marriedLiving;
    seniorApp.veteranwithserviceevaluationordisability = this.state.veteranSpouseDisability;
    seniorApp.under61withdisabilitynotice = this.state.not61WithDisabilityNotice;
    seniorApp.effectivedateofdisability = this.state.disabilityEffectiveDate;
    await this.props.updateSeniorApp(seniorApp);
  };

  showSpouseSectionOnLoad = seniorApp => {
    if (
      seniorApp &&
      (seniorApp.ptas_spousefirstname ||
        seniorApp.ptas_spousefirstname ||
        seniorApp.ptas_spousemiddlename ||
        seniorApp.ptas_spousemiddlename ||
        seniorApp.ptas_spouselastname ||
        seniorApp.ptas_spousesuffix ||
        seniorApp.spousedob ||
        seniorApp.ptas_docspouseage)
    )
      this.setState({ spouseInformation: true });
  };

  handleSelectChangeValue = e => {
    this.setState({ [e.target.name]: e.target.value });
  };

  calculateAndGenerate = (date, disability, propertyDate) => {
    let source = [];
    let yearsObjectArray = [];
    let newDate = '';
    let jsonSavedArray = [];

    newDate =
      typeof date === 'string'
        ? date.slice(0, 10)
        : new Date(date.getFullYear(), date.getMonth(), date.getDate())
            .toISOString()
            .slice(0, 10);
    console.log('MyInfo savedDate', savedDate);
    let savedDate = this.props.seniorApp.applicantdateofbirth
      ? typeof this.props.seniorApp.applicantdateofbirth === 'string'
        ? this.props.seniorApp.applicantdateofbirth.slice(0, 10)
        : new Date(
            this.props.seniorApp.applicantdateofbirth.getFullYear(),
            this.props.seniorApp.applicantdateofbirth.getMonth(),
            this.props.seniorApp.applicantdateofbirth.getDate()
          )
            .toISOString()
            .slice(0, 10)
      : '';

    //if (newDate !== savedDate) {
    yearsObjectArray = determinateYearsToApply(
      date,
      disability,
      propertyDate,
      this.props.years
    );
    if (arrayNullOrEmpty(yearsObjectArray)) {
      this.activateYearsToApplyCard(false);
    } else {
      jsonSavedArray = this.props.calculateAuthYearsObject(
        yearsObjectArray,
        false
      );
      source = this.props.returnToRadioButtomSource(jsonSavedArray);
      this.activateYearsToApplyCard(true);
    }
    // } else {
    //   jsonSavedArray = this.props.calculateAuthYearsObject(
    //     this.props.yearsToApplyArrayObject,
    //     false
    //   );
    //   source = this.props.returnToRadioButtomSource(jsonSavedArray);
    // }
    this.props.setOptionsArray(source);
  };

  activateYearsToApplyCard = value => {
    this.setState({ displayYearsOptions: value });
  };

  updateBirthDate = date => {
    if (date && this.props.seniorApp) {
      let requiredMissing = this.state.requiredMissing;
      let textIndex = requiredMissing.findIndex(missing => {
        return missing == 'dateOfBirth';
      });
      if (textIndex > -1) requiredMissing.splice(textIndex, 1);

      let canApplyLegalProofDisability = this.state
        .canApplyLegalProofDisability;
      let d = new Date(date);
      if (isValidDate(date)) {
        if (!hasAgeToQualify(d)) {
          canApplyLegalProofDisability = true;
        } else {
          canApplyLegalProofDisability = false;
        }

        this.setState({
          dateOfBirth: date,
          belowAge: canApplyLegalProofDisability,
          canApplyLegalProofDisability: canApplyLegalProofDisability,
          requiredMissing: requiredMissing,
        });

        this.activateYearsToApplyCard(true);
        this.calculateAndGenerate(
          date,
          canApplyLegalProofDisability,
          this.props.seniorApp.firstdateprimaryres
        );
      }
    } else {
      this.setState({ dateOfBirth: '', belowAge: false });
      this.activateYearsToApplyCard(false);
    }
  };

  handleDateChange = (date, id) => {
    if (id === 'dateOfBirth') {
      this.updateBirthDate(date);
    } else {
      this.setState({
        [id]: date,
      });
    }
  };

  handleGroupCivilStatusSwitch = (checked, e, id) => {
    const groupOff = {
      single: false,
      widowed: false,
      divorced: false,
      marriedLiving: false,
    };
    if (id === 'addSpouse' && checked) {
      this.setState(groupOff, this.showSpouseSection);
    } else if (id === 'addSpouse' && !checked) {
      this.showSpouseSection();
    } else {
      this.setState(
        {
          ...groupOff,
          [id]: checked,
        },
        () => {
          if (this.state.spouseInformation) {
            this.showSpouseSection(); // handle spouseInformation status
          }
        }
      );
    }
  };

  handleSwitchInputChange = (checked, e, id) => {
    this.setState({
      [id]: checked,
    });

    if (id === 'legalProofDisability' || id === 'disabledVeteran') {
      if (this.state.dateOfBirth) {
        if (isValidDate(this.state.dateOfBirth)) {
          this.calculateAndGenerate(
            this.state.dateOfBirth,
            checked,
            this.props.seniorApp.firstdateprimaryres
          );
        }
      }
    }
    if (!checked) {
      // console.log('MyInfo checked', checked);
      // let updatedFiles = this.state.proofOfDisabilityFiles;
      // let document = this.state.documents[1];
      // alert(4);
      // this.notifyChangeOnFilesMulti(
      //   [updatedFiles],
      //   [document],
      //   [false, false],
      //   false
      // );

      if (id == 'legalProofDisability') {
        this.setState({ proofOfDisabilityFiles: [], disabledVeteran: false });
      }
      if (id == 'spouseLegalProofDisability') {
        this.setState({
          spouseProofOfDisabilityFiles: [],
          spouseDisabledVeteran: false,
        });
      }
      // } else {
      //   if (id == 'legalProofDisability') {
      //     //this.setState({ checkedproofOfDisabilityFiles: true });
      //     let updatedFiles = this.state.proofOfDisabilityFiles;
      //     let document = this.state.documents[1];
      //     this.notifyChangeOnFiles(updatedFiles, document, true);
      //   }
    }
  };

  cleanSpouseSection = () => {
    this.setState({
      spouseFirstName: '',
      spouseMiddleName: '',
      spouseLastFamilyName: '',
      spouseSuffix: '',
      //spouseDateOfBirth: null,
      spouseBelowAge: false,
      spouseDisabledVeteran: false,
      canSpouseApplyLegalProofDisability: false,
      spouseLegalProofDisability: false,
      spouseProofOfDisabilityFiles: [],
      spouseProofOfAgeFiles: [],
    });
  };

  showSpouseSection = () => {
    if (this.state.spouseInformation) {
      //Clean information
      this.cleanSpouseSection();
    }
    this.setState(prevState => {
      return { spouseInformation: !prevState.spouseInformation };
    });
  };

  /**
   * Updates the file array information on the cloud and notifies
   * of the file update process.
   * @memberof MyInfo
   */
  // notifyChangeOnFilesMulti = async (
  //   updatedFiles,
  //   document,
  //   shouldUpdateArray,
  //   isVisible = false
  // ) => {
  //   if (this.state.isSaving) {
  //     this.props.hideSavingOverlay();
  //     this.setState(
  //       {
  //         isSaving: false,
  //         uploading: false,
  //       },
  //       this.props.nextTab()
  //     );
  //   }

  //   await updateUploadsDataMulti(
  //     updatedFiles,
  //     document,
  //     this.props.contact,
  //     this.props.seniorApp,
  //     this.state.section,
  //     shouldUpdateArray,
  //     isVisible
  //   );
  //   await this.props.sendMessage('documentUpdated');
  // };
  handleMetadataUpdate = async (array, documentObject, notify = true) => {
    if (notify /*&& !this.state.notifying*/) {
      //   this.setState({ notifying: true }, async () => {
      // Get others file Array list from file metadata
      let fileArrayList = getFileArraysFromFileMetadataList(
        // filter current document as the array value will come from the parameter
        this.props.filesMetadata.filter(
          f => f.document != documentObject.document
        ),
        'portalSection',
        this.state.section
      );

      // Add current array as it is the latest value.
      fileArrayList[documentObject.arrayName] = array;
      await this.sendNotification(fileArrayList);
      //    this.setState({ notifying: false });
      //  });
    }
    const state = {};
    state[documentObject.arrayName] = array;
    this.setState(state);
    await createOrUpdateFileMetadataEntities(
      this.props.filesMetadata,
      this.state.section,
      documentObject.document,
      this.props.seniorApp.seapplicationid,
      null,
      this.props.seniorApp.accountnumber,
      array,
      documentObject.arrayName,
      this.props.getFilesMetadata
    );
  };

  handleTextInputChange = e => {
    let requiredMissing = this.state.requiredMissing;
    let textIndex = requiredMissing.findIndex(missing => {
      return missing == e.target.name;
    });
    const value = trimSecure(e.target.value);
    if (value.length > 0) {
      if (textIndex > -1) requiredMissing.splice(textIndex, 1);
      this.setState({ helperText: '', requiredMissing: requiredMissing });
    } else {
      this.setState({ helperText: 'Required' });
    }
    this.setState({ [e.target.name]: value });
  };

  canContinue = () => {
    let canContinue = true;
    let requiredMissing = [];
    if (!this.state.firstName || this.state.firstName == '') {
      requiredMissing.push('firstName');
      canContinue = false;
    }
    if (!this.state.lastFamilyName || this.state.lastFamilyName == '') {
      requiredMissing.push('lastFamilyName');
      canContinue = false;
    }
    if (!this.state.dateOfBirth || this.state.dateOfBirth == '') {
      requiredMissing.push('dateOfBirth');
      canContinue = false;
    }
    if (arrayNullOrEmpty(this.state.proofOfAgeFiles)) {
      if (!this.state.canApplyLegalProofDisability) {
        requiredMissing.push('proofOfAgeFiles');
        canContinue = false;
      }
    }
    if (!this.props.selectedYear) {
      requiredMissing.push('yearsOptions');
      canContinue = false;
    }

    if (!canContinue)
      this.setState({
        requiredMissing: requiredMissing,
        requiredPopup: true,
      });

    return canContinue;
    /*
    if (!this.state.firstName || this.state.firstName == '') {
      this.setState({ firstRequired: 'firstName' }, () => {
        return false;
      });
    } else if (!this.state.lastFamilyName || this.state.lastFamilyName == '') {
      this.setState({ firstRequired: 'lastFamilyName' }, () => {
        return false;
      });
    } else if (!this.state.dateOfBirth || this.state.dateOfBirth == '') {
      this.setState({ firstRequired: 'dateOfBirth' }, () => {
        return false;
      });
    } else if (arrayNullOrEmpty(this.state.proofOfAgeFiles)) {
      this.setState({ firstRequired: 'proofOfAge' }, () => {
        return false;
      });
    } else {
      return true;
    }*/
    /*return (
      this.state.dateOfBirth &&
      this.state.dateOfBirth !== '' &&
      this.state.firstName &&
      this.state.firstName !== '' &&
      this.state.lastFamilyName &&
      this.state.LastFamilyName !== '' &&
      this.state.emailAddress &&
      this.state.emailAddress !== ''
      //this.state.phoneNumber &&
      //this.state.phoneNumber !== '' &&
    );*/
  };

  handleContinueClick = async e => {
    if (this.canContinue()) {
      window.scrollTo(0, 0);
      this.continueAndSave();
      /*appInsights.trackEvent({
        name: 'continue',
        event: 'click',
        section: 'MyInfo',
      });*/
    }
  };

  defineBlocks = () => {
    let result = false;
    if (!this.canContinue()) {
      //console.log('result is', 1);
      result = true;
      this.setState({
        //showDialogDOBMSG: true,
        //showDialog: true,
        dialogAlreadyShowed: true,
        showDialogType: 2, // 0 info 1 warning 2 error
        handleCloseContinue: this.closeDialog,
      });
    } else {
    }
    return result;
  };

  closeDialog = (event, reason) => {
    if (reason !== 'backdropClick') {
      this.setState({
        showDialog: false,
        dialogAlreadyShowed: true,
      });
      /*if (this.state.firstRequired && this.refs[this.state.firstRequired]) {
        setTimeout(() => {
          this.refs[this.state.firstRequired].scrollIntoView({
            block: 'center',
            behavior: 'smooth',
          });
        }, 100);
      }*/
    }
  };

  continueAndSave = async e => {
    //this.closeDialog();
    this.setState({
      // continueText: fm.savingLabel,
      isSaving: true,
    });
    this.props.showSavingOverlay();

    if (!this.props.readOnlyMode) {
      await this.updateContact();
      await this.updateSeniorApp();
    }
    // if (!this.state.uploading) {
    this.props.hideSavingOverlay();
    this.setState(
      {
        isSaving: false,
      },
      this.props.nextTab()
    );
    // }
  };

  informationDialog = () => {
    if (!(this.props.editMode || this.props.readOnlyMode)) {
      return (
        <DialogCSS
          open={this.state.infoOpen}
          onClose={this.handleClose}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
        >
          <DialogContent style={{ marginTop: '20px', marginBottom: '20px' }}>
            <div className="info-icon-alert" style={{ marginRight: '27px' }}>
              <InfoOutlinedIcon style={{ width: '2em', height: '2em' }} />
            </div>
            <div className="info-icon-alert" style={{ width: '75%' }}>
              <DialogContentText
                id="alert-dialog-description"
                style={{ color: 'black' }}
              >
                <div>{fm.documentsToComplete}</div>
                <div>
                  <ul>
                    <li>{fm.proofOfAgeDocument}</li>
                    <li>{fm.proofOfDisabilityDocument}</li>
                  </ul>
                </div>
              </DialogContentText>
            </div>
            <div className="align-center">
              <CustomButton
                onClick={this.handleClose}
                label={fm.continueLabel}
                btnBigLabel={true}
              />
            </div>
          </DialogContent>
        </DialogCSS>
      );
    }
  };

  render() {
    const ITEM_HEIGHT = 48;
    const ITEM_PADDING_TOP = 8;
    const MenuProps = {
      PaperProps: {
        style: {
          maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
          width: 250,
        },
      },
    };

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
              <li key={field}>
                <a
                  onClick={() => {
                    if (field !== 'yearsOptions') {
                      this.refs[field].scrollIntoView({
                        block: 'center',
                        behavior: 'smooth',
                      });
                    } else {
                      if (this.state.displayYearsOptions) {
                        this.refs[field].scrollIntoView({
                          block: 'center',
                          behavior: 'smooth',
                        });
                      }
                    }
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

    const firstNameInput = (
      <div ref="firstName">
        <TextInputML
          name="firstName"
          label={fm.firstName}
          value={this.state.firstName}
          onChange={this.handleTextInputChange}
          readOnly={this.props.readOnlyMode}
          helperText={
            this.state.dialogAlreadyShowed && this.state.firstName.length < 1
              ? fm.required
              : ''
          }
          error={
            this.state.dialogAlreadyShowed && this.state.firstName.length < 1
          }
          required
        />
      </div>
    );

    const lastNameInput = (
      <div ref="lastFamilyName">
        <TextInputML
          name="lastFamilyName"
          label={fm.lastFamilyName}
          value={this.state.lastFamilyName}
          onChange={this.handleTextInputChange}
          readOnly={this.props.readOnlyMode}
          helperText={
            this.state.dialogAlreadyShowed &&
            this.state.lastFamilyName.length < 1
              ? fm.required
              : ''
          }
          error={
            this.state.dialogAlreadyShowed &&
            this.state.lastFamilyName.length < 1
          }
          required
        />
      </div>
    );

    const dateOfBirth = (
      <div className="center-myage-panel card-item" ref="dateOfBirth">
        <FormattedMessage
          id="myInfo_dateOfBirthPlaceholder"
          defaultMessage="mm/dd/yyyy"
          description="date of birth placeholder"
        >
          {placeholder => (
            <DateInput
              id="dateOfBirth"
              name="dateOfBirthInput"
              label={fm.dateOfBirth}
              placeholder={placeholder}
              value={this.state.dateOfBirth}
              onChange={this.handleDateChange}
              readOnly={this.props.readOnlyMode}
              helperText={fm.required}
              error={
                this.state.dialogAlreadyShowed && this.state.dateOfBirth < 1
              }
              required
              openTo="year"
            />
          )}
        </FormattedMessage>
      </div>
    );

    const proofOfAge = (
      <div
        className="center-myage-panel card-item"
        style={{ marginBottom: 18 }}
        ref="proofOfAgeFiles"
      >
        <CardSide
          header={fm.proofOfAgeFiles}
          content={[
            fm.validDocumentThatVerifies,
            <VimeoSlider videos={[{ url: '382824186' }]} />,
            fm.hideSocialSecurity,
            fm.selectPhotoOfDocument,
          ]}
        ></CardSide>

        <UploadFile
          id={'myageUploadFile'}
          leftMessage={fm.leftUploadFileMessage}
          rightMessage={fm.required}
          obscureInfoMessage={false}
          fileArray={this.state.proofOfAgeFiles}
          appId={
            this.props.seniorApp ? this.props.seniorApp.seapplicationid : ''
          }
          section={this.state.section}
          document={this.state.documents[0].document}
          helpText={fm.helpText}
          onCreate={array =>
            this.handleMetadataUpdate(array, this.state.documents[0])
          }
          onDelete={array =>
            this.handleMetadataUpdate(array, this.state.documents[0])
          }
          required={
            this.state.dialogAlreadyShowed &&
            arrayNullOrEmpty(this.state.proofOfAgeFiles)
          }
          traceFileArray={this.traceFileArray}
          hideIcon
        />
      </div>
    );

    const yearsOptions = (
      <div ref="yearsOptions">
        <Hidden smUp>
          <div className="card-separator"></div>
        </Hidden>
        <Card className="cardStyle">
          <Hidden xsDown>
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
                  <FormattedMessage
                    sm={12}
                    style={{ fontWeight: 'bold !important' }}
                    id="YearsPopUp_title"
                    values={{ year: this.props.earlierYear }}
                  />
                </Grid>
              </div>
            </div>
          </Hidden>
          <CardSide
            header={fm.cardSide_applicationYear_title}
            content={[fm.cardSide_applicationYear_content]}
          ></CardSide>
          <CardContent className="center-myinfo-panel">
            <div
              style={{ textAlign: 'left', fontWeight: 'bold' }}
              className="card-item"
            >
              <FormattedMessage
                id="myInfo_formApplyForExemptionYear"
                defaultMessage="Apply for exemption year"
              ></FormattedMessage>
            </div>
          </CardContent>
          <Grid container spacing={20} style={{ padding: '0 0px 0 27%' }}>
            <Grid item sm={12}>
              <RadioInputCss
                source={this.props.globalYearOptionsArray}
                onChange={this.props.handleYearsPopUpOptions}
                itemMsg={this.props.itemMsg}
              />
            </Grid>
          </Grid>
        </Card>
      </div>
    );

    return (
      <CollectionConsumer>
        {value => (
          <TabContainer>
            <div className="center-panel center-content center">
              <Collapse
                className="required-popup"
                in={
                  this.state.requiredMissing.length > 0 &&
                  this.state.requiredPopup
                }
                style={{
                  top: this.state.requiredMissing
                    ? this.state.requiredMissing.length > 0
                      ? ''
                      : 0
                    : 0,
                }}
              >
                <IconButton
                  onClick={() => this.setState({ requiredPopup: false })}
                  className="required-close"
                >
                  <ClearIcon className="required-close-icon" />
                </IconButton>
                {requiredMissing}
              </Collapse>
              <Card className="cardStyle">
                <Hidden xsDown>
                  <div className="financialHeader">
                    <div
                      style={{
                        font: 14,
                        fontWeight: 'bold',
                        margin: '0 8px',
                        padding: '8px',
                        textAlign: 'left',
                      }}
                    >
                      <Grid container spacing={20}>
                        <div item={true} sm={12}>
                          {fm.formName}
                        </div>
                      </Grid>
                    </div>
                  </div>
                </Hidden>

                <CardContent className="center-myname-panel card-item">
                  {firstNameInput}
                  <TextInputML
                    name="middleName"
                    label={fm.middleName}
                    value={this.state.middleName}
                    onChange={this.handleTextInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                  {lastNameInput}
                  <div style={{ marginTop: '16px' }}>
                    <SelectInput
                      name="suffix"
                      label={fm.suffix}
                      source={value.suffixes}
                      value={this.state.suffix}
                      onChange={this.handleSelectChangeValue}
                      readOnly={this.props.readOnlyMode}
                      sourceValue="defaultLocString"
                      sourceLabel="name"
                    />
                  </div>
                </CardContent>
              </Card>
              <Hidden smUp>
                <div className="card-separator"></div>
              </Hidden>
              <Card className="cardStyle">
                <Hidden xsDown>
                  <div className="financialHeader">
                    <div
                      style={{
                        font: 14,
                        fontWeight: 'bold',
                        marginLeft: '10px',
                        padding: '8px',
                        textAlign: 'left',
                      }}
                      className="card-item"
                    >
                      <Grid container spacing={20}>
                        <div item={true} sm={12}>
                          {fm.formAge}
                        </div>
                      </Grid>
                    </div>
                  </div>
                </Hidden>
                <CardContent className="center-myinfo-panel">
                  {dateOfBirth}
                  <div className="center-myageWarning-panel">
                    {renderIf(
                      this.state.belowAge,

                      <CustomWarning
                        className="margin-top-14"
                        label={fm.belowAge}
                      />
                    )}
                  </div>

                  {proofOfAge}

                  <Collapse
                    in={this.state.belowAge}
                    className="center-myage2-panel greyCollapse"
                  >
                    <Collapse in={this.state.canApplyLegalProofDisability}>
                      <div className="margin-top-14">
                        <SwitchInputCSS
                          id="legalProofDisability"
                          label={fm.legalProofDisability}
                          information={fm.legalProofDisability_ht}
                          checked={this.state.legalProofDisability}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                        />
                      </div>
                      <Collapse
                        className="center-myage-panel"
                        in={this.state.legalProofDisability}
                      >
                        <div className="margin-top-14">
                          <UploadFile
                            obscureInfoMessage={false}
                            fileArray={this.state.proofOfDisabilityFiles}
                            hideIcon
                            appId={
                              this.props.seniorApp
                                ? this.props.seniorApp.seapplicationid
                                : ''
                            }
                            section={this.state.section}
                            document={this.state.documents[1].document}
                            helpText={fm.helpText}
                            onCreate={array =>
                              this.handleMetadataUpdate(
                                array,
                                this.state.documents[1]
                              )
                            }
                            onDelete={array =>
                              this.handleMetadataUpdate(
                                array,
                                this.state.documents[1]
                              )
                            }
                            hideIcon
                          />
                        </div>
                        <div className="margin-top-14">
                          <SwitchInput
                            id="disabledVeteran"
                            label={fm.disableVeteran}
                            checked={this.state.disabledVeteran}
                            onChange={this.handleSwitchInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                        </div>
                      </Collapse>
                    </Collapse>
                  </Collapse>
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="survivingSpouse"
                      label={fm.survivingSpouse}
                      checked={this.state.survivingSpouse}
                      onChange={this.handleSwitchInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                </CardContent>
              </Card>
              {renderIf(
                this.state.displayYearsOptions,
                <div>{yearsOptions}</div>
              )}
              <Hidden smUp>
                <div className="card-separator"></div>
              </Hidden>
              <Card className="cardStyle">
                <Hidden xsDown>
                  <div className="financialHeader">
                    <div
                      style={{
                        font: 14,
                        fontWeight: 'bold',
                        margin: '0 10px',
                        padding: '8px',
                        textAlign: 'left',
                      }}
                    >
                      <Grid container spacing={20}>
                        <div item={true} sm={12}>
                          {fm.formContactInfo}
                        </div>
                      </Grid>
                    </div>
                  </div>
                </Hidden>
                <CardContent className="center-myinfo-panel">
                  <div className="card-item">
                    <div style={{ marginLeft: 20 }}>
                      <FormattedMessage
                        id="myInfo_emailAddressPlaceholder"
                        defaultMessage="username@domain.com"
                      >
                        {placeholder => (
                          <EmailInput
                            name="emailAddress"
                            label={fm.emailAddress}
                            placeholder={placeholder}
                            value={this.state.emailAddress}
                            onChange={this.handleTextInputChange}
                            readOnly={true}
                          />
                        )}
                      </FormattedMessage>

                      <FormattedMessage
                        id="myInfo_phoneNumberPlaceholder"
                        defaultMessage="000-000-0000"
                      >
                        {placeholder => (
                          <PhoneInput
                            name="phoneNumber"
                            label={fm.phoneNumber}
                            placeholder={placeholder}
                            value={this.state.phoneNumber}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                        )}
                      </FormattedMessage>
                    </div>

                    <SwitchInput
                      id="textSmsCapable"
                      label={fm.smsCapable}
                      checked={this.state.textSmsCapable}
                      onChange={this.handleSwitchInputChange}
                      readOnly={this.props.readOnlyMode}
                      style={{ marginTop: '31px' }}
                    />
                  </div>
                </CardContent>
              </Card>
              <Hidden smUp>
                <div className="card-separator"></div>
              </Hidden>
              <Card className="cardStyle">
                <Hidden xsDown>
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
                        <div item={true} sm={12}>
                          {fm.formAdditionalInfo}
                        </div>
                      </Grid>
                    </div>
                  </div>
                </Hidden>
                <CardContent className="center-myinfo-panel">
                  <div className="card-item">
                    <SwitchInput
                      id="addSpouse"
                      label={fm.addSpouse}
                      checked={this.state.spouseInformation}
                      onChange={this.handleGroupCivilStatusSwitch}
                    />
                  </div>

                  <Collapse
                    className="center-myinfo2-panel greyCollapse"
                    in={this.state.spouseInformation}
                  >
                    <TextInputML
                      name="spouseFirstName"
                      label={fm.firstName}
                      value={this.state.spouseFirstName}
                      onChange={this.handleTextInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                    <TextInputML
                      name="spouseMiddleName"
                      label={fm.middleName}
                      value={this.state.spouseMiddleName}
                      onChange={this.handleTextInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                    <TextInputML
                      name="spouseLastFamilyName"
                      label={fm.lastFamilyName}
                      value={this.state.spouseLastFamilyName}
                      onChange={this.handleTextInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                    <div style={{ marginTop: '16px' }}>
                      <SelectInput
                        name="spouseSuffix"
                        label={fm.suffix}
                        source={value.suffixes}
                        value={this.state.spouseSuffix}
                        onChange={this.handleSelectChangeValue}
                        readOnly={this.props.readOnlyMode}
                        sourceValue="defaultLocString"
                        sourceLabel="name"
                      />
                    </div>
                  </Collapse>
                  {/* <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="married"
                      label={fm.married}
                      checked={this.state.married}
                      onChange={this.handleSwitchInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div> */}
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="single"
                      label={fm.single}
                      checked={this.state.single}
                      onChange={this.handleGroupCivilStatusSwitch}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="widowed"
                      label={fm.widowed}
                      checked={this.state.widowed}
                      onChange={this.handleGroupCivilStatusSwitch}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="divorced"
                      label={fm.divorced}
                      checked={this.state.divorced}
                      onChange={this.handleGroupCivilStatusSwitch}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                  <Collapse
                    in={this.state.divorced}
                    className="center-myage-panel greyCollapse"
                  >
                    <p className="forms-uploaders-title">
                      {fm.divorcedDecreeDocDescription}
                    </p>
                    <div className="margin-top-14">
                      <UploadFile
                        leftMessage={fm.divorcedDecreeDoc}
                        obscureInfoMessage={false}
                        fileArray={this.state.decreeFiles}
                        hideIcon
                        appId={
                          this.props.seniorApp
                            ? this.props.seniorApp.seapplicationid
                            : ''
                        }
                        section={this.state.section}
                        document={this.state.documents[2].document}
                        helpText={fm.helpText}
                        onCreate={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[2]
                          )
                        }
                        onDelete={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[2]
                          )
                        }
                      />
                    </div>
                  </Collapse>
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    <SwitchInput
                      id="marriedLiving"
                      label={fm.marriedLiving}
                      checked={this.state.marriedLiving}
                      onChange={this.handleGroupCivilStatusSwitch}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                  <div
                    style={{
                      paddingTop: '20px',
                    }}
                    className="card-item"
                  >
                    {/* Replaced text but also property to save value on 
                    PBI #148405
                    <SwitchInput
                      id="veteranSpouse"
                      label={fm.veteranSpouse}
                      checked={this.state.veteranSpouse}
                      onChange={this.handleSwitchInputChange}
                      readOnly={this.props.readOnlyMode}
                    /> */}
                    <SwitchInput
                      id="veteranSpouseDisability"
                      label={fm.veteranSpouseDisability}
                      checked={this.state.veteranSpouseDisability}
                      onChange={this.handleSwitchInputChange}
                      readOnly={this.props.readOnlyMode}
                    />
                  </div>
                  <Collapse
                    in={this.state.veteranSpouseDisability}
                    className="center-myage-panel greyCollapse"
                  >
                    <p className="forms-uploaders-title">
                      {fm.VACompensationDocDescription}
                    </p>
                    <div className="margin-top-14">
                      <UploadFile
                        leftMessage={fm.VACompensationDoc}
                        obscureInfoMessage={false}
                        fileArray={this.state.VACompensationLetterFiles}
                        hideIcon
                        appId={
                          this.props.seniorApp
                            ? this.props.seniorApp.seapplicationid
                            : ''
                        }
                        section={this.state.section}
                        document={this.state.documents[3].document}
                        helpText={fm.helpText}
                        onCreate={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[3]
                          )
                        }
                        onDelete={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[3]
                          )
                        }
                      />
                    </div>
                  </Collapse>
                  {renderIf(
                    this.state.belowAge,
                    <div
                      style={{
                        paddingTop: '20px',
                      }}
                      className="card-item"
                    >
                      <SwitchInput
                        id="not61WithDisabilityNotice"
                        label={fm.not61WithDisabilityNotice}
                        checked={this.state.not61WithDisabilityNotice}
                        onChange={this.handleSwitchInputChange}
                        readOnly={this.props.readOnlyMode}
                      />
                    </div>
                  )}

                  <Collapse
                    in={
                      this.state.not61WithDisabilityNotice &&
                      this.state.belowAge
                    }
                    className="center-myage-panel greyCollapse"
                  >
                    <FormattedMessage
                      id="myInfo_disabilityEffectiveDatePlaceHolder"
                      defaultMessage="mm/dd/yyyy"
                      description="Effective date of disability placeholder"
                    >
                      {placeholder => (
                        <DateInput
                          id="disabilityEffectiveDate"
                          name="disabilityEffectiveDateInput"
                          label={fm.disabilityEffectiveDate}
                          placeholder={placeholder}
                          value={this.state.disabilityEffectiveDate}
                          onChange={this.handleDateChange}
                          readOnly={this.props.readOnlyMode}
                          shrinkLabel={true}
                          openTo="year"
                        />
                      )}
                    </FormattedMessage>

                    <p
                      className="forms-uploaders-title"
                      style={{
                        paddingTop: '20px',
                      }}
                    >
                      {fm.SSAAwardLetterDocDescription}
                    </p>
                    <div className="margin-top-14">
                      <UploadFile
                        leftMessage={fm.SSAAwardLetterDoc}
                        obscureInfoMessage={false}
                        fileArray={this.state.SSAAwardLetterFormFiles}
                        hideIcon
                        appId={
                          this.props.seniorApp
                            ? this.props.seniorApp.seapplicationid
                            : ''
                        }
                        section={this.state.section}
                        document={this.state.documents[4].document}
                        helpText={fm.helpText}
                        onCreate={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[4]
                          )
                        }
                        onDelete={array =>
                          this.handleMetadataUpdate(
                            array,
                            this.state.documents[4]
                          )
                        }
                        hideIcon
                      />
                    </div>
                  </Collapse>
                </CardContent>
              </Card>
              {/* <CustomDialog
                showDialog={this.state.showDialog}
                showDialogType={this.state.showDialogType}
                handleCloseContinue={this.state.handleCloseContinue}
                legalProofDisability={
                  this.state.legalProofDisability &&
                  arrayNullOrEmpty(this.state.proofOfDisabilityFiles)
                }
                dateOfBirth={
                  !(this.state.dateOfBirth && this.state.dateOfBirth !== '')
                }
                firstName={
                  !(this.state.firstName && this.state.firstName !== '')
                }
                lastName={
                  !(
                    this.state.lastFamilyName &&
                    this.state.lastFamilyName !== ''
                  )
                }
                emailAddress={
                  !(this.state.emailAddress && this.state.emailAddress !== '')
                }
                //phoneNumber={!this.state.phoneNumber}
                proofOfAgeDocument={
                  !this.state.belowAge &&
                  arrayNullOrEmpty(this.state.proofOfAgeFiles)
                }
              ></CustomDialog> */}
              <div className="card-end">
                {renderIf(
                  this.state.requiredMissing.length > 0,
                  requiredMissing
                )}
              </div>

              <div className="continue-summary-panel">
                <CustomButton
                  style={{ marginTop: 10, marginBottom: 90 }}
                  className="display-inline"
                  onClick={this.handleContinueClick}
                  label={this.state.continueText}
                  btnBigLabel={true}
                />
                {(this.props.editMode || this.props.readOnlyMode) && (
                  <SummaryButton
                    onClick={this.props.onNavigateToSummary}
                    label={fm.returnToSummary}
                  />
                )}
              </div>
            </div>
          </TabContainer>
        )}
      </CollectionConsumer>
    );
  }
}

export default MyInfo;
