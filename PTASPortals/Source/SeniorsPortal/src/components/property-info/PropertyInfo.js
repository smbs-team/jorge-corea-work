import React, { Fragment } from 'react';
import * as fm from './FormatTexts';
import * as gm from '../../GeneralFormatTexts';
import './PropertyInfo.css';
import states from '../../assets/states_hash.json';
import theme from './PropertyInfo.css';
import TabContainer from '../common/TabContainer';
import TextInputML from '../common/TextInputML';
import TextAreaML from '../common/TextAreaML';
import PhoneInput from '../common/PhoneInput';
import DateInput from '../common/DateInput';
import UploadFile from '../common/UploadFile/UploadFile';
import ExpandLink from '../common/ExpandLink';
import ShortDivider from '../common/ShortDivider';
import SwitchInput from '../common/SwitchInput';
import SelectInput from '../common/SelectInput';
import Information from '../common/Information';
import { FormattedMessage } from 'react-intl';
import Collapse from '@material-ui/core/Collapse';
import CustomButton from '../common/CustomButton';
import SummaryButton from '../common/SummaryButton';
import WarningIcon from '@material-ui/icons/Warning';
import Grid from '@material-ui/core/Grid';
import NativeSelect from '@material-ui/core/NativeSelect';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import HelpText from '../common/HelpText';
import AlertPopUpYears from '../common/AlertPopUpYears';
import { AuthConsumer } from '../../contexts/AuthContext';
import { withStyles, makeStyles } from '@material-ui/core/styles';
import AwesomeDebouncePromise from 'awesome-debounce-promise';
import {
  mightQualifyConsideringMyInfo,
  yearsToApply,
} from '../../lib/helpers/age';
import {
  CollectionConsumer,
  CollectionContext,
} from '../../contexts/CollectionContext';
import PropTypes from 'prop-types';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Draggable from 'react-draggable';
import DialogContentText from '@material-ui/core/DialogContentText';
import Autosuggest from 'react-autosuggest';
import match from 'autosuggest-highlight/match';
import parse from 'autosuggest-highlight/parse';
import TextField from '@material-ui/core/TextField';
import Paper from '@material-ui/core/Paper';
import MenuItem from '@material-ui/core/MenuItem';
import Popper from '@material-ui/core/Popper';
import CustomDialog from '../common/dialog-component/CustomDialog';
import { Hidden, CircularProgress } from '@material-ui/core';

import {
  generateBlobUriObjectFromAFileArray,
  getFileArraysFromFileMetadataList,
  getFileArrayFromBlobUrl,
} from '../../lib/data-mappings/fileMetadataToFileArray';
import deepEqual from 'deep-equal';
import { cloneDeep } from 'lodash';
import {
  renderIf,
  arrayNullOrEmpty,
  getYearDifferentToNow,
  isValidDate,
  convertToNumberFloatFormat,
  convertToNumberIntFormat,
} from '../../lib/helpers/util';
import {
  updateFileMetadata,
  createFileMetadata,
  deleteOtherProperty,
  deleteSeniorOccupant,
  createSeniorOccupant,
  updateSeniorOccupant,
  createOtherProperty,
  updateOtherProperty,
  createSeniorAppDetail,
  updateSeniorAppDetail,
  getSeniorAppDetailsBySeniorAppId,
} from '../../services/dataServiceProvider';
import {
  getParcelDetail,
  addressLookup,
  parcelLookup,
} from '../../services/dynamics-service';
import { getBaseFileMetadataObject } from '../../lib/data-mappings/fileMetadataToFileArray';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardHeader from '@material-ui/core/CardHeader';
import {
  createOrUpdateFileMetadataEntities,
  createFileArraysFromMetadata,
} from '../common/UploadFile/uploaderHelper';
import uuid from 'uuid';
import {
  determinateYearsToApply,
  generateRadioButtomSource,
} from '../../lib/helpers/age';
import RadioInput from '../common/RadioInput';
import CardSide from '../common/CardSide';
import CustomWarning from '../common/CustomWarning';
import VimeoSlider from '../common/vimeo-player/VimeoSlider';
import { getAppInsightsInstance } from '../../services/telemetryService';
import {
  updateUploadsData,
  setUploadsData,
} from '../common/UploadFile/signalRHelper';
import AlertDialog from '../common/AlertDialog';
import FootSquareInput from '../common/FootSquareInput';
import PercentageInput from '../common/PercentageInput';
import CustomTextField from '../common/CustomTextField';
import { trimSecure } from '../../lib/helpers/trimText';

const TextFieldCss = withStyles(theme => ({
  root: {
    width: 298,
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
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
    '& .MuiFormControl-root': {
      [theme.breakpoints.up('sm')]: {
        fontSize: '20px',
      },
      [theme.breakpoints.down('xs')]: {
        fontSize: '16px',
      },
    },
  },
}))(TextField);

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

const MenuItemCss = withStyles({
  selected: {
    backgroundColor: '#d4e693 !important',
  },
})(MenuItem);

const DialogCSS = withStyles({
  root: {
    '& .MuiDialog-paperWidthSm': {
      maxWidth: '1078px',
      width: '100%',
      height: 'auto',
    },
  },
})(Dialog);

const QDialogCSS = withStyles({
  root: {
    '& .MuiDialog-paperWidthSm': {
      maxWidth: '800px',
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

function renderInputComponentProp(inputProps) {
  const { classes, inputRef = () => {}, ref, ...other } = inputProps;
  return (
    <TextFieldCss
      fullWidth
      InputProps={{
        inputRef: node => {
          ref(node);
          inputRef(node);
        },
        // classes: {
        //   input: classes.input,
        // },
      }}
      {...other}
      multiline
      rows="2"
    />
  );
}
function renderInputComponent(inputProps) {
  const { classes, inputRef = () => {}, ref, ...other } = inputProps;

  return (
    <TextFieldCss
      fullWidth
      InputProps={{
        inputRef: node => {
          ref(node);
          inputRef(node);
        },
        // classes: {
        //   input: classes.input,
        // },
      }}
      {...other}
    />
  );
}

function renderSuggestion(suggestion, { query, isHighlighted }) {
  const matches = match(
    suggestion.ptas_name + ' ' + suggestion.ptas_address,
    query
  );

  const parts = parse(
    [
      suggestion.ptas_parceldetailid,
      suggestion.ptas_name,
      suggestion.ptas_district,
      suggestion.ptas_zipcode,
      suggestion.ptas_address,
      // ? suggestion.ptas_address.slice(0, suggestion.ptas_address.indexOf(','))
      // : suggestion.ptas_address,
    ],
    matches
  );

  return (
    <MenuItemCss
      selected={isHighlighted}
      component="div"
      style={{ paddingBottom: '0px' }}
    >
      <div>{renderAddress(parts)}</div>
    </MenuItemCss>
  );
}
function renderSuggestionMailing(suggestion, { query, isHighlighted }) {
  return (
    <MenuItemCss
      selected={isHighlighted}
      component="div"
      style={{ paddingBottom: '0px' }}
    >
      <div>{suggestion.formattedaddr ? suggestion.formattedaddr : ''}</div>
    </MenuItemCss>
  );
}

function renderAddress(parts) {
  console.log('parts' + parts[0].text);
  let info = [];
  for (var i = 0; i < parts[0].text.length; i++) {
    info.push(parts[0].text[i]);
  }

  if (parts.length > 1) {
    console.log('parts' + parts[1].text);
    for (var i = 0; i < parts[1].text.length; i++) {
      info.push(parts[1].text[i]);
    }
  }

  let parcelId = info[0];

  let parcelNum = info.length > 1 && info[1] !== undefined ? info[1] : '';
  let city = info.length > 2 && info[2] !== undefined ? info[2] : '';
  let zip = info.length > 3 && info[3] !== undefined ? info[3] : '';
  let parcelAddress = info.length > 4 && info[4] !== undefined ? info[4] : '';

  let address = (
    <div>
      <span
        key={parcelId}
        style={{
          fontWeight: parts[0].highlight ? 500 : 400,
          fontWeight: 'bold',
        }}
      >
        {parcelAddress}
      </span>
    </div>
  );

  let parcelNumber = (
    <div className="margin-bottom-10">
      {fm.parcelNumber}
      &nbsp;
      {parcelNum}
    </div>
  );

  let cityStateZip = (
    <div>
      {city}
      {', WA '}
      {zip}
    </div>
  );

  return (
    <div className="option-parcel-box">
      {address}
      {cityStateZip}
      {parcelNumber}
      <div className="lookup-divider" />
    </div>
  );
}

function renderInput(inputProps) {
  const { InputProps, classes, ref, ...other } = inputProps;

  return (
    <TextField
      InputProps={{
        inputRef: ref,
        ...InputProps,
      }}
      {...other}
    />
  );
}

renderInput.propTypes = {
  /**
   * Override or extend the styles applied to the component.
   */
  classes: PropTypes.object.isRequired,
  InputProps: PropTypes.object,
};

const ShortDividerCSS = withStyles({
  root: {
    width: '631px',
    marginTop: '30px',
    marginBottom: '30px',
  },
})(ShortDivider);

const TextInputMLCss117 = withStyles({
  root: {
    display: 'inline-block',
    marginRight: '10px',
    '& .MuiInputBase-input': {
      width: '117px',
    },
  },
})(TextInputML);

const TextInputMLCssW117 = withStyles({
  root: {
    marginRight: '10px',
    '& .MuiInputBase-input': {
      width: '117px',
    },
  },
})(TextInputML);

const TextInputMLCss40 = withStyles({
  root: {
    display: 'inline-block',
    marginRight: '10px',
    '& .MuiInputBase-input': {
      width: '40px',
    },
  },
})(TextInputML);

const TextInputMLCss74 = withStyles({
  root: {
    display: 'inline-block',
    '& .MuiInputBase-input': {
      width: '74px',
    },
  },
})(TextInputML);

const parcelLookupAPIDebounced = AwesomeDebouncePromise(parcelLookup, 500);
const addressLookupDebounced = AwesomeDebouncePromise(addressLookup, 500);

function PaperComponent(props) {
  return (
    <Draggable cancel={'[class*="MuiDialogContent-root"]'}>
      <Paper {...props} />
    </Draggable>
  );
}

class PropertyInfo extends React.Component {
  constructor() {
    super();
    this.state = {
      section: 'Property Info',
      continueText: fm.continueLabel,
      isSaving: false,
      usaCountryId: 'b850ac6e-60b4-e911-a980-001dd800cc2e',
      parcel: null,

      propertyId: '',
      propertyFound: false,
      property: '',
      properties: [],
      firstNameProperty: '',
      streetProperty: '',
      cityProperty: '',
      parcelNumber: '',
      accountNumber: '',

      acquiredDate: null,

      singleFamilyResidence: false,
      housingCoop: false,
      singleUnitOfMultiDwellingCondoOrDuplex: false,
      mobileHome: false,
      mobileHomeYear: '',
      mobileHomeMake: '',
      mobileHomeModel: '',
      showMobileHomeInfoModal: false,
      showSingleUnitInfoModal: false,

      //Primary residence section
      isPrimaryResidence: true,
      liveNineMonthsOfYear: true,
      firstDateAsPrimaryResidence: null,

      //Major changes section
      hasExperiencedMajorLifeChanges: false,
      //has upload file

      //Own other properties section
      ownOtherProperties: false,
      otherProperties: [],

      //Others live in property
      othersLiveInProperty: false,
      occupants: [],

      //Property co-op fields
      isPropertyCoop: false,
      nameOfCoop: '',
      treasurer: null,
      treasurerPhoneNumber: '',
      numberOfSharesYouOwn: '',
      totalCoopShares: '',
      //Transferring exemption from previous residence fields
      isTransferringExemption: false,
      firstDatePreviousResidence: null,
      countyPreviousResidence: '',
      //Mailing address
      mailingAddressStreet: '',
      mailingAddressCity: '',
      mailingAddresses: [],
      mailingAddressFullName: '',
      mailingAddressChangeOfficialAddress: false,
      mailingAddressState: 'WA',
      mailingAddress: '',
      mailingAddressZip: '',

      exemptionAddress: '',
      exemptionStreetAddress: '',
      exemptionAddressCity: '',
      exemptionAddressState: '',
      exemptionAddressZip: '',
      country: 'b850ac6e-60b4-e911-a980-001dd800cc2e',
      isUSA: true,

      propertyAddresses: [],
      //different address section
      addDifferentAddress: false,

      city: '',
      state: '',
      zip: '',

      checkAddress: '',
      checkAddresses: [],
      checkAddressStreet: '',
      checkAddressCountryId: 'b850ac6e-60b4-e911-a980-001dd800cc2e',
      isCheckUSA: true,

      //Files to upload
      proofOfOwnershipFileArray: [],
      shareCertificateFileArray: [],
      infoOpen: false,
      informationOpen: true,
      qualifyOpen: true,
      infoOpenWhyNot: false,
      warned: false,
      currentProofOfOwnershipFileArray: [],
      currentShareCertificateFileArray: [],
      // link between the document and the array to extract the files and match the Upload control accordingly.
      documents: [
        {
          document: 'Share Certificate',
          arrayName: 'shareCertificateFileArray',
        },
        {
          document: 'Title of Trust',
          arrayName: 'proofOfOwnershipFileArray',
        },
        {
          document: 'Lease or Life Estate',
          arrayName: 'leaseOrLifeEstateFileArray',
        },
      ],
      yearOptionsArray: null,
      displayYearsOptions: false,
      recommendedYear: null,
      requiredMissing: [],
      requiredPopup: true,
      owned9months: true,
      ownThroughTrust: false,
      isDelinquent: false,
      parcelBlurred: true,
      notifying: false,
      owner: false,
      leaseOrLifeEstateFileArray: [],
      propertyPurchaseDate: null,
      soldFormerResidence: false,
      whenSoldFormerResidence: null,

      residenceForBusiness: false,
      isResidenceForBusinessPercentageValue: false,
      percentageUsedForBusiness: 0,
      squareFootageUsedForBusiness: 0,
      rentPortionResidence: false,
      isRentPortionPercentageValue: false,
      percentageRentedOut: 0,
      squareFootageRentedOut: 0,
      receivedExemptionBefore: false,
      whenReceivedExemptionBefore: null,
      whereReceivedExemptionBefore: '',
      otherPropertyInCurrentTaxYear: false,
      whenOtherPropertyIn2020: 0,
      whereOtherPropertyIn2020: 0,
      exclusiveHomeTypes: [
        'singleFamilyResidence',
        'singleUnitOfMultiDwellingCondoOrDuplex',
        'mobileHome',
      ],

      isPropertiesTimeout: false,
    };
  }

  handleCloseMobileHomeInfoModal = () => {
    this.setState({ showMobileHomeInfoModal: false });
  };

  handleCloseSingleUnitInfoModal = () => {
    this.setState({ showSingleUnitInfoModal: false });
  };

  verifyIfUserMightQualify = () => {
    let mightQualifyTexts = mightQualifyConsideringMyInfo(
      this.props.contact.birthdate,
      this.props.seniorApp.vadisabled,
      this.props.seniorApp.disabled,
      this.props.seniorApp.docdisability
    );

    let youMightQualify = '';
    let youMightNotQualify = '';

    if (mightQualifyTexts[0] != '') {
      youMightQualify = <li>{mightQualifyTexts[0]}</li>;
    }
    if (mightQualifyTexts[1] != '') {
      youMightNotQualify = (
        <li>
          {mightQualifyTexts[1]}
          <div className="why-not-div" onClick={this.handleClickOpenWhyNot}>
            <div className="link display-inline">{fm.whyNot}</div>
            <IconButton className="why-not-icon">
              <InfoOutlinedIcon />
            </IconButton>
          </div>
        </li>
      );
    }

    return (
      <div>
        <ul>
          {youMightQualify}
          {youMightNotQualify}
        </ul>
      </div>
    );
  };

  handleCloseInformation = () => {
    this.setState({ informationOpen: false });
  };

  handleCloseQualifyDialog = () => {
    this.setState({ qualifyOpen: false });
  };

  handleClickOpen = () => {
    this.setState({ infoOpen: true });
  };

  handleClose = () => {
    this.setState({ infoOpen: false });
  };

  componentDidMount = async () => {
    window.scrollTo(0, 0);
    //await this.props.getFilesMetadata(this.props.seniorApp.seapplicationid);

    if (!arrayNullOrEmpty(this.props.countries)) {
      const USACountryId = this.props.countries.filter(
        c => c.name === 'United States'
      )[0].countryid;
      this.setState({
        checkAddressCountryId: USACountryId,
        usaCountryId: USACountryId,
        country: USACountryId,
      });
    }

    if (this.props.seniorApp) {
      this.setSeniorAppInfo(this.props.seniorApp);
    }

    this.sendNotification(
      getFileArraysFromFileMetadataList(
        this.props.filesMetadata,
        'portalSection',
        this.state.section
      )
    );

    // if (
    //   arrayNullOrEmpty(this.state.proofOfOwnershipFileArray) &&
    //   !arrayNullOrEmpty(this.props.filesMetadata)
    // ) {
    //   await this.setFileArrays();
    // }

    if (this.props.occupants) {
      this.setOtherOccupants(this.props.occupants);
    }

    if (this.props.otherProperties) {
      this.setOtherProperties(this.props.otherProperties);
    }

    await this.props.setVideoCode(this.props.defaultHelpVideoUrl);

    this.setFileArrays();
  };

  checkIfVideoPlayed = () => {
    this.props.checkIfVideoPlayed(this.state.section).then(data => {
      if (!data) {
        this.props.callHelpVideo();
      }
    });
  };

  componentDidUpdate = async (prevProps, prevState) => {
    // if (
    //   arrayNullOrEmpty(this.state.proofOfOwnershipFileArray) &&
    //   !arrayNullOrEmpty(this.props.filesMetadata) &&
    //   !deepEqual(prevProps.filesMetadata, this.props.filesMetadata)
    // ) {
    //   this.setFileArrays();
    // }

    if (
      this.props.seniorApp &&
      !deepEqual(prevProps.seniorApp, this.props.seniorApp)
    ) {
      this.setSeniorAppInfo(this.props.seniorApp);
    }

    if (
      this.props.occupants &&
      !deepEqual(prevProps.occupants, this.props.occupants)
    ) {
      this.setOtherOccupants(this.props.occupants);
    }

    if (
      this.props.otherProperties &&
      !deepEqual(prevProps.otherProperties, this.props.otherProperties)
    ) {
      this.setOtherProperties(this.props.otherProperties);
    }
    //SIGNALR
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

    if (
      prevState.isPropertyCoop !== this.state.isPropertyCoop ||
      prevState.ownThroughTrust !== this.state.ownThroughTrust
    ) {
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

  getFormatedParcelId = parcelId => {
    if (parcelId) {
      return parcelId.trim().replace('-', '');
    }
    return '';
  };

  setParcelDetails = async parcelId => {
    if (parcelId) {
      const parcel = await getParcelDetail(parcelId);
      console.log('parcel', parcel);
      if (parcel) {
        this.setState({
          parcel,
          propertyFound: true,
          firstNameProperty: parcel.namesonaccount
            ? parcel.namesonaccount.trim()
            : '',
          streetProperty: parcel.address
            ? parcel.address.trim() == ','
              ? ''
              : parcel.address.trim()
            : '',
          cityProperty:
            (parcel.district ? parcel.district + ', WA ' : ' WA') +
            (parcel.zipcode ? parcel.zipcode : ''),
          parcelNumber: parcel.name,
          propertyId: parcelId,
          accountNumber: parcel.acctnbr,
          city: parcel.district,
          zip: parcel.zipcode,
          state: 'WA',
        });
      }
    }
  };

  updateSeniorApp = async () => {
    let seniorApp = { ...this.props.seniorApp };

    seniorApp.accountnumber = this.state.accountNumber;
    seniorApp.parcelid = this.state.parcelId;
    seniorApp.occupieddate = this.state.firstDateAsPrimaryResidence;
    seniorApp.currentlyownoccupy = this.state.isPrimaryResidence;
    // seniorApp.firstdateprimaryres = this.state.firstDateAsPrimaryResidence;
    seniorApp.majorlifechange = this.state.hasExperiencedMajorLifeChanges;
    seniorApp.ownmultipleproperties = this.state.ownOtherProperties;

    //Fields ready for when the object is ready in dynamics
    seniorApp.othercountyaddress = this.state.exemptionStreetAddress;
    seniorApp.othercountycity = this.state.exemptionAddressCity;
    seniorApp.othercountystate = this.state.exemptionAddressState;
    seniorApp.othercountypostal = this.state.exemptionAddressZip;

    //seniorApp.  = this.state.firstDatePreviousResidence

    seniorApp.otherparcelnumber = this.state.otherParcelNumber;
    seniorApp.coopproperty = this.state.isPropertyCoop;
    seniorApp.coopname = this.state.nameOfCoop;
    seniorApp.cooptreasurer = this.state.treasurer;
    seniorApp.cooptreasurerphone = this.state.treasurerPhoneNumber;
    seniorApp.coopownedshares = this.state.numberOfSharesYouOwn;
    seniorApp.cooptotalshares = this.state.totalCoopShares;
    seniorApp.otheroccupants = this.state.othersLiveInProperty;
    seniorApp.addrchange = this.state.addDifferentAddress;
    seniorApp.addrstreet1 = this.state.mailingAddressStreet;

    seniorApp.addrcity = this.state.mailingAddressCity;
    seniorApp.addrstate = this.state.mailingAddressState;
    seniorApp.addrpostal = this.state.mailingAddressZip;
    seniorApp.addrcountryid =
      !this.state.country || this.state.country === ''
        ? null
        : this.state.country;

    seniorApp.differentcheckaddress = this.state.differentCheckAddress;
    seniorApp.checkaddressname = this.state.checkAddressName;
    seniorApp.checkaddresscountryid =
      !this.state.checkAddressCountryId ||
      this.state.checkAddressCountryId === ''
        ? null
        : this.state.checkAddressCountryId;
    seniorApp.checkaddressstreet = this.state.checkAddressStreet;
    seniorApp.checkaddresscity = this.state.checkAddressCity;
    seniorApp.checkaddressstate = this.state.checkAddressState;
    seniorApp.checkaddresspostalcode = this.state.checkAddressPostalCode;

    seniorApp.residingfor9months = this.state.liveNineMonthsOfYear;
    seniorApp.correspondencename = this.state.mailingAddressFullName;
    //seniorApp. = this.state.mailingAddressChangeOfficialAddress;
    seniorApp.docownership = !arrayNullOrEmpty(
      this.state.proofOfOwnershipFileArray
    );
    seniorApp.docmajorlifechange = !arrayNullOrEmpty(
      this.state.proofOfChangeFileArray
    );
    seniorApp.doccoopshares = !arrayNullOrEmpty(
      this.state.shareCertificateFileArray
    );

    seniorApp.propertysection = false;
    seniorApp.ownthroughtrust = this.state.ownThroughTrust;
    seniorApp.hadexinanothercounty = this.state.isTransferringExemption;

    seniorApp.propertytaxesdelinquent = this.state.isDelinquent;

    seniorApp.lifeestate = this.state.owner;
    seniorApp.datepropertypurchased = this.state.propertyPurchaseDate;
    seniorApp.soldformerresidence = this.state.soldFormerResidence;
    seniorApp.dateofformerpropertysale = this.state.whenSoldFormerResidence;

    seniorApp.propertyusedforbusiness = this.state.residenceForBusiness;

    seniorApp.percentageusedforbusiness = this.state
      .isResidenceForBusinessPercentageValue
      ? this.state.percentageUsedForBusiness
      : 0;
    seniorApp.squarefootageusedforbusiness = !this.state
      .isResidenceForBusinessPercentageValue
      ? this.state.squareFootageUsedForBusiness
      : 0;
    seniorApp.rentoutaportionofproperty = this.state.rentPortionResidence;
    seniorApp.percentagerentedout = this.state.isRentPortionPercentageValue
      ? this.state.percentageRentedOut
      : 0;
    seniorApp.squarefootagerentedout = !this.state.isRentPortionPercentageValue
      ? this.state.squareFootageRentedOut
      : 0;
    seniorApp.receivedexemptionbefore = this.state.receivedExemptionBefore;
    seniorApp.whenwasthepreviousexemption = this.state.whenReceivedExemptionBefore;
    seniorApp.wherewasthepreviousexemption = this.state.whereReceivedExemptionBefore;
    seniorApp.soldotherpropertyin2020 = this.state.otherPropertyInCurrentTaxYear;
    seniorApp.datepropertysoldin2020 = this.state.whenOtherPropertyIn2020;
    seniorApp.wherepropertywassoldin2020 = this.state.whereOtherPropertyIn2020;

    seniorApp.singlefamilyresidence = this.state.singleFamilyResidence;
    seniorApp.housingcoop = this.state.isPropertyCoop;
    seniorApp.singleunitofmultidwellingcondoorduplex = this.state.singleUnitOfMultiDwellingCondoOrDuplex;
    seniorApp.mobilehome = this.state.mobileHome;
    seniorApp.mobilehomeyear = this.state.mobileHomeYear;
    seniorApp.mobilehomemake = this.state.mobileHomeMake;
    seniorApp.mobilehomemodel = this.state.mobileHomeModel;

    await this.props.updateSeniorApp(seniorApp);
  };

  setSeniorAppInfo = seniorApp => {
    if (seniorApp) {
      this.setParcelDetails(seniorApp.parcelid);

      this.setState({
        //firstDatePreviousResidence : seniorApp. ,
        parcelId: seniorApp.parcelid,
        firstDateAsPrimaryResidence: seniorApp.occupieddate,
        isPrimaryResidence:
          seniorApp.currentlyownoccupy == undefined
            ? true
            : seniorApp.currentlyownoccupy,
        // firstDateAsPrimaryResidence: seniorApp.firstdateprimaryres,
        hasExperiencedMajorLifeChanges: seniorApp.majorlifechange,
        ownOtherProperties: seniorApp.ownmultipleproperties,
        countyPreviousResidence: seniorApp.transferredfrcounty,
        isTransferringExemption: seniorApp.hadexinanothercounty,
        otherParcelNumber: seniorApp.otherparcelnumber,
        isPropertyCoop: seniorApp.coopproperty,
        nameOfCoop: seniorApp.coopname,
        treasurer: seniorApp.cooptreasurer,
        treasurerPhoneNumber: seniorApp.cooptreasurerphone,
        numberOfSharesYouOwn: seniorApp.coopownedshares,
        totalCoopShares: seniorApp.cooptotalshares,
        othersLiveInProperty: seniorApp.otheroccupants,
        addDifferentAddress: seniorApp.addrchange,
        mailingAddress: seniorApp.addrstreet1 ? seniorApp.addrstreet1 : '',
        mailingAddressStreet: seniorApp.addrstreet1
          ? seniorApp.addrstreet1
          : '',
        mailingAddressFullName: seniorApp.correspondencename,
        liveNineMonthsOfYear:
          seniorApp.residingfor9months == undefined
            ? true
            : seniorApp.residingfor9months,
        //mailingAddressChangeOfficialAddress : seniorApp. ,

        //Fields ready for when the object is ready in dynamics
        exemptionStreetAddress: seniorApp.othercountyaddress
          ? seniorApp.othercountyaddress
          : '',
        exemptionAddress: seniorApp.othercountyaddress
          ? seniorApp.othercountyaddress
          : '',
        exemptionAddressCity: seniorApp.othercountycity
          ? seniorApp.othercountycity
          : '',
        exemptionAddressState: seniorApp.othercountystate
          ? seniorApp.othercountystate
          : '',
        exemptionAddressZip: seniorApp.othercountypostal
          ? seniorApp.othercountypostal
          : '',

        differentCheckAddress: seniorApp.differentcheckaddress,
        checkAddressName: seniorApp.checkaddressname,
        checkAddressCountryId: seniorApp.checkaddresscountryid,
        checkAddressStreet: seniorApp.checkaddressstreet
          ? seniorApp.checkaddressstreet
          : '',
        checkAddress: seniorApp.checkaddressstreet
          ? seniorApp.checkaddressstreet
          : '',
        checkAddressCity: seniorApp.checkaddresscity
          ? seniorApp.checkaddresscity
          : '',
        checkAddressState: seniorApp.checkaddressstate
          ? seniorApp.checkaddressstate
          : '',
        checkAddressPostalCode: seniorApp.checkaddresspostalcode
          ? seniorApp.checkaddresspostalcode
          : '',

        ownThroughTrust: seniorApp.ownthroughtrust,
        mailingAddressCity: seniorApp.addrcity,
        mailingAddressZip: seniorApp.addrpostal,
        mailingAddressState: seniorApp.addrstate,
        city: seniorApp.addrcity,
        state: seniorApp.addrstate,
        zip: seniorApp.addrpostal,
        country: seniorApp.addrcountryid
          ? seniorApp.addrcountryid
          : this.state.country,

        isDelinquent: seniorApp.propertytaxesdelinquent,
        owner: seniorApp.lifeestate,
        propertyPurchaseDate: seniorApp.datepropertypurchased,
        soldFormerResidence: seniorApp.soldformerresidence,
        whenSoldFormerResidence: seniorApp.dateofformerpropertysale,

        residenceForBusiness: seniorApp.propertyusedforbusiness,
        percentageUsedForBusiness: seniorApp.percentageusedforbusiness,
        squareFootageUsedForBusiness: seniorApp.squarefootageusedforbusiness,
        isResidenceForBusinessPercentageValue:
          seniorApp.percentageusedforbusiness > 0 ? true : false,
        rentPortionResidence: seniorApp.rentoutaportionofproperty,
        percentageRentedOut: seniorApp.percentagerentedout,
        squareFootageRentedOut: seniorApp.squarefootagerentedout,
        isRentPortionPercentageValue:
          seniorApp.percentagerentedout > 0 ? true : false,
        receivedExemptionBefore: seniorApp.receivedexemptionbefore,
        whenReceivedExemptionBefore: seniorApp.whenwasthepreviousexemption,
        whereReceivedExemptionBefore: seniorApp.wherewasthepreviousexemption,
        otherPropertyIn2020: seniorApp.soldotherpropertyin2020,
        whenOtherPropertyIn2020: seniorApp.datepropertysoldin2020,
        whereOtherPropertyIn2020: seniorApp.wherepropertywassoldin2020,
        singleFamilyResidence: seniorApp.singlefamilyresidence,
        housingCoop: seniorApp.coopproperty,
        singleUnitOfMultiDwellingCondoOrDuplex:
          seniorApp.singleunitofmultidwellingcondoorduplex,
        mobileHome: seniorApp.mobilehome,
        mobileHomeYear: seniorApp.mobilehomeyear,
        mobileHomeMake: seniorApp.mobilehomemake,
        mobileHomeModel: seniorApp.mobilehomemodel,
      });
      if (seniorApp.addrcountryid)
        this.handleCountryChangeValue(seniorApp.addrcountryid);
      if (seniorApp.checkaddresscountryid)
        this.handleCheckCountryChangeValue(seniorApp.checkaddresscountryid);
    }
  };

  createOrUpdateOtherOccupants = async () => {
    if (!this.state.othersLiveInProperty) {
      //If switch is not checked, it will delete all occupants.
      await this.deleteInvalidCollectionElements(
        this.props.occupants,
        'occupants'
      );
    } else {
      if (!arrayNullOrEmpty(this.state.occupants)) {
        let promises = this.state.occupants.map(async (o, i) => {
          if (o.occupantId) {
            let occupant = {
              ...this.props.occupants.filter(
                oc => oc.seappoccupantId === o.occupantId
              )[0],
            };
            occupant.occupanttype = o.relationship;
            occupant.occupantlastname = o.occupantLastName;
            occupant.occupantfirstName = o.occupantFirstName;
            occupant.occupantmiddlename = o.occupantMiddleName;
            occupant.occupantsuffix = o.suffix;
            await updateSeniorOccupant(occupant);
          } else {
            let occupant = {
              seappoccupantId: uuid.v4(),
              occupanttype: o.relationship,
              occupantlastname: o.occupantLastName,
              occupantfirstName: o.occupantFirstName,
              occupantmiddlename: o.occupantMiddleName,
              occupantsuffix: o.suffix,
              seapplicationid: this.props.seniorApp.seapplicationid,
            };

            await createSeniorOccupant(occupant);
          }
        });

        promises.push(
          this.deleteInvalidCollectionElements(
            this.determinateCollectionElementsToBeDeleted(
              this.state.occupants,
              this.props.occupants,
              'occupants'
            ),
            'occupants'
          )
        );

        await Promise.all(promises);
      }
    }
    await this.props.getOccupants(this.props.seniorApp.seapplicationid);
  };

  setOtherOccupants = async otherOccupants => {
    if (!arrayNullOrEmpty(otherOccupants)) {
      let array = [];
      otherOccupants.map(o => {
        array.push({
          // If occupanttype is 668020001 it means owner living on property
          livesHere: o.occupanttype == 668020001 ? true : false,
          occupantId: o.seappoccupantId,
          relationship: o.occupanttype,
          occupantLastName: o.occupantlastname,
          occupantFirstName: o.occupantfirstName,
          occupantMiddleName: o.occupantmiddlename,
          suffix: o.occupantsuffix,
        });
      });

      this.setState({ occupants: array });
    }
  };

  determinateCollectionElementsToBeDeleted = (
    stateCollection,
    propCollection,
    objectType
  ) => {
    let elementsToBeDeleted = [];
    switch (objectType) {
      case 'occupants':
        elementsToBeDeleted = propCollection.filter(
          p => !stateCollection.find(s => s.occupantId == p.seappoccupantId)
        );
        break;
      case 'properties':
        elementsToBeDeleted = propCollection.filter(
          p => !stateCollection.find(s => s.propertyId == p.seappotherpropid)
        );
        break;
      default:
        break;
    }
    return elementsToBeDeleted;
  };

  deleteInvalidCollectionElements = async (
    collectionToBeDeleted,
    objectType
  ) => {
    if (!arrayNullOrEmpty(collectionToBeDeleted)) {
      let promises = collectionToBeDeleted.map(async (o, i) => {
        switch (objectType) {
          case 'occupants':
            await deleteSeniorOccupant(o);
            break;
          case 'properties':
            await deleteOtherProperty(o);
            break;
          default:
            break;
        }
      });
      await Promise.all(promises);
    }
  };

  createOrUpdateOtherProperties = async () => {
    if (!this.state.ownOtherProperties) {
      //If switch is not checked, it will delete all properties.
      await this.deleteInvalidCollectionElements(
        this.props.otherProperties,
        'properties'
      );
    } else {
      if (!arrayNullOrEmpty(this.state.otherProperties)) {
        let promises = this.state.otherProperties.map(async (o, i) => {
          if (o.propertyId) {
            let property = {
              ...this.props.otherProperties.filter(
                ot => ot.seappotherpropid === o.propertyId
              )[0],
            };
            property.address =
              !o.propertyStreetAddress || o.propertyStreetAddress == ''
                ? o.propertyAddress
                : o.propertyStreetAddress;
            property.purpose = o.propertyPurpose;
            property.countyname = o.propertyCounty;
            property.statename = o.propertyState;
            await updateOtherProperty(property);
          } else {
            let property = {
              seappotherpropid: uuid.v4(),
              address:
                !o.propertyStreetAddress || o.propertyStreetAddress == ''
                  ? o.propertyAddress
                  : o.propertyStreetAddress,
              countyname: o.propertyCounty,
              statename: o.propertyState,
              purpose: o.propertyPurpose,
              seapplicationid: this.props.seniorApp.seapplicationid,
            };

            await createOtherProperty(property);
          }
        });

        promises.push(
          this.deleteInvalidCollectionElements(
            this.determinateCollectionElementsToBeDeleted(
              this.state.otherProperties,
              this.props.otherProperties,
              'properties'
            ),
            'properties'
          )
        );
        await Promise.all(promises);
      }
    }
    this.props.getOtherProperties(this.props.seniorApp.seapplicationid);
  };

  setOtherProperties = async otherProperties => {
    if (!arrayNullOrEmpty(otherProperties)) {
      let array = [];
      otherProperties.map(o => {
        let splittedAddress = o.address.split(',');
        let completeAddress = isNaN(splittedAddress[1])
          ? `${splittedAddress[0] ? splittedAddress[0] : ''}${
              o.countyname ? ', ' + o.countyname : ''
            }${o.statename ? ', ' + o.statename : ''}${
              splittedAddress[1] ? ', ' + splittedAddress[1] : ''
            }`
          : `${splittedAddress[0] ? splittedAddress[0] : ''}${
              o.countyname ? ', ' + o.countyname : ''
            }${o.statename ? ', ' + o.statename : ''}${
              splittedAddress[1] ? ', ' + splittedAddress[1] : ''
            }`;

        array.push({
          propertyId: o.seappotherpropid,
          //propertyAddress: o.address,
          propertyAddress: completeAddress,
          propertyPurpose: o.purpose,
          //propertyStreetAddress: o.address,
          propertyStreetAddress: completeAddress,
          propertyCounty: o.countyname,
          propertyState: o.statename,
        });
      });

      this.setState({ otherProperties: array });
    }
  };

  createOrUpdateSeniorDetails = async () => {
    if (!arrayNullOrEmpty(this.props.seniorAppDetails)) {
      let newDetail = this.props.seniorAppDetails.filter(item => {
        return item.seapplicationid === this.props.seniorApp.seapplicationid;
      });
      if (!arrayNullOrEmpty(newDetail)) {
        newDetail = newDetail[0];
        if (newDetail) {
          newDetail.parcelid = this.state.parcelId;
          newDetail.contactid = this.props.contact.contactid;
          newDetail.accountnumber = this.state.accountNumber;
          newDetail.yearid = this.props.yearsToApplyArrayObject.filter(item => {
            return item.seniorAppId === this.props.seniorApp.seapplicationid;
          })[0].yearId;
          await updateSeniorAppDetail(newDetail);
          await this.props.getSeniorDetails(
            this.props.seniorApp.seapplicationid
          );
        }
      }
    }
  };

  /**File manipulation methods*/

  updateFileMetadata = async fileMetadata => {
    if (fileMetadata && fileMetadata.length > 0) {
      let currentFileMetadata = fileMetadata.filter(
        f =>
          f.seniorExemptionApplicationId &&
          !f.seniorExemptionApplicationDetailId &&
          f.isBlob
      );
      currentFileMetadata =
        currentFileMetadata && currentFileMetadata.length > 0
          ? currentFileMetadata[0]
          : null;
      if (currentFileMetadata) {
        currentFileMetadata.accountNumber = this.state.accountNumber;
        await updateFileMetadata(currentFileMetadata);
        this.props.getFilesMetadata(this.props.seniorApp.seapplicationid);
      }
    }
  };

  sendNotification = async arrayContainer => {
    if (this.props.contact && this.props.seniorApp) {
      let documents = cloneDeep(this.state.documents);

      documents.map(d => {
        if (d.document === 'Title of Trust') {
          d.isVisible = this.state.ownThroughTrust;
        }

        if (d.document === 'Share Certificate') {
          d.isVisible = this.state.isPropertyCoop;
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

  //     let toUpdateArray = [];
  //     this.props.seniorApp.coopproperty &&
  //       toUpdateArray.push(this.state.documents[0].document);
  //     this.props.seniorApp.ownthroughtrust &&
  //       toUpdateArray.push(this.state.documents[1].document);

  //     await updateUploadsDataMultinoB64(
  //       fileMeta,
  //       this.props.contact,
  //       this.props.seniorApp,
  //       this.state.section,
  //       '',
  //       toUpdateArray
  //     );

  //     this.props.sendMessage('documentUpdated');
  //   }
  // };

  setParentState = (key, value) => {
    this.setState({ [key]: value });
  };
  handleMetadataUpdate = async (array, documentObject, notify = true) => {
    if (notify /*&& !this.state.notifying*/) {
      // this.setState({ notifying: true }, async () => {
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
      //  this.setState({ notifying: false });
      //  });
    }

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

      this.setState(arrays.rootFileArrays);
    }
  };

  /**END of file  manipulation methods*/

  createOrEmptyOtherPropertyObject = create => {
    let otherProperties = [];
    if (create) {
      otherProperties = [...this.state.otherProperties];
      console.log(otherProperties);
      otherProperties.push({
        propertyId: null,
        propertyAddress: '',
        propertyPurpose: '',
        propertyStreetAddress: '',
        propertyCounty: '',
        propertyState: '',
      });
    }

    this.setState({ otherProperties });
  };

  createOrEmptyOccupantsObject = create => {
    let occupants = [...this.state.occupants];
    if (create || occupants.length < 1) {
      occupants.push({
        occupantId: null,
        occupantFirstName: '',
        occupantMiddleName: '',
        occupantLastName: '',
        suffix: '',
        // set livesHere in false, 668020003 represents owner not living here as default relationship value
        relationship: 668020003,
        livesHere: false,
      });
    }

    this.setState({ occupants });
  };

  handleNumericFloatInputChange = (e, id) => {
    this.setState({ [id]: convertToNumberFloatFormat(e.target.value) });
  };

  handleNumericIntInputChange = (e, id) => {
    this.setState({ [id]: convertToNumberIntFormat(e.target.value) });
  };

  handleSelectedParcel = value => {
    this.setState({ property: value, propertyFound: true });
  };
  handleTextInputChange = (e, name, array, index) => {
    //if (e && e.target) {
    const value = trimSecure(e.target.value);
    if (array) {
      let newArray = [...array];
      newArray[index][e.target.name] = value;
      this.setState({ [name]: array });
    } else {
      this.setState({ [e.target.name]: value });
    }
    // }
  };

  handleOccupantsSwitchInputChange = (checked, e, id) => {
    this.createOrEmptyOccupantsObject(false);
    this.setState({ [id]: checked });
  };

  handleIOwnOtherPropertiesInputChange = (checked, e, id) => {
    this.createOrEmptyOtherPropertyObject(checked);
    this.setState({ [id]: checked });
  };

  handleTrustInputChange = (checked, e, id) => {
    if (checked) {
      this.setState({ [id]: checked });
    } else {
      this.setState({ proofOfOwnershipFileArray: [], [id]: checked });
    }
  };

  handleExclusiveSwitchInputChange = (checked, e, id, exclusiveList) => {
    let state = {};
    exclusiveList.map(item => {
      if (item === id && checked) {
        state[item] = checked;
      } else {
        state[item] = false;
      }
    });
    if (checked) {
      // popup info handle
      state = {
        ...state,
        showMobileHomeInfoModal: id === 'mobileHome',
        showSingleUnitInfoModal:
          id === 'singleUnitOfMultiDwellingCondoOrDuplex',
      };
    }
    this.setState(state);
  };

  updateAddressFromParcelSelected = () => {
    this.setState(prev => {
      return {
        mailingAddressFullName: prev.firstNameProperty,
        mailingAddressStreet: prev.streetProperty,
        mailingAddressCity: prev.city,
        mailingAddressState: prev.state,
        mailingAddressZip: prev.zip,
        mailingAddress: prev.streetProperty,
        country: this.state.usaCountryId,
      };
    });
  };

  handleSwitchInputChange = (checked, e, id) => {
    this.setState({ [id]: checked }, () => {
      if (!checked) {
        switch (id) {
          case 'addDifferentAddress':
            this.updateAddressFromParcelSelected();
            break;
          case 'differentCheckAddress':
            this.clearCheckAddress();
            break;
          case 'isPropertyCoop':
            this.clearCoop();
            break;
          case 'isTransferringExemption':
            this.clearTransferringExemption();
            break;
          default:
            break;
        }
      } else {
        switch (id) {
          case 'addDifferentAddress':
            this.clearDifferentAddress();
            break;
          default:
            break;
        }
      }
    });
  };

  handleSwitchMobileHomeChange = (checked, e, id) => {
    this.setState({
      [id]: checked,
      showMobileHomeInfoModal: checked,
    });
  };

  handleSwitchSingleUnitChange = (checked, e, id) => {
    this.setState({
      [id]: checked,
      showSingleUnitInfoModal: checked,
    });
  };

  handleInputChange = async e => {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  };

  handleCustomInputChange = ({ value, name }) => {
    this.setState({ [name]: value });
  };

  clearDifferentAddress = () => {
    this.setState({
      mailingAddressFullName: '',
      country: this.state.usaCountryId,
      mailingAddressStreet: '',
      mailingAddressCity: '',
      mailingAddressState: '',
      mailingAddressZip: '',
      mailingAddress: '',
    });
  };

  clearCheckAddress = () => {
    this.setState({
      checkAddressName: '',
      checkAddressCountryId: '',
      checkAddressStreet: '',
      checkAddressCity: '',
      checkAddressState: '',
      checkAddressPostalCode: '',
      checkAddress: '',
    });
  };

  clearCoop = () => {
    this.setState({
      nameOfCoop: '',
      treasurer: '',
      treasurerPhoneNumber: '',
      numberOfSharesYouOwn: '',
      totalCoopShares: '',
      shareCertificateFileArray: [],
    });
  };

  clearTransferringExemption = () => {
    this.setState({
      otherParcelNumber: '',
      exemptionAddress: '',
      exemptionStreetAddress: '',
      exemptionAddressCity: '',
      exemptionAddressState: '',
      exemptionAddressZip: '',
    });
  };

  handleOtherPropertyPurpouseSelectChangeValue = (value, index) => {
    let otherProperties = [...this.state.otherProperties];
    otherProperties[index].propertyPurpose = value;
    this.setState({ otherProperties });
    //this.setState({ [e.target.name]: e.target.value });
  };

  handleOtherPropertyPurpouseSelectChangeValue = (value, index) => {
    let otherProperties = [...this.state.otherProperties];
    otherProperties[index].propertyPurpose = value;
    this.setState({ otherProperties });
    //this.setState({ [e.target.name]: e.target.value });
  };

  handleSelectChangeValue = (e, name, array, index) => {
    if (array) {
      let newArray = [...array];
      newArray[index][e.target.name] = e.target.value;
      this.setState({ [name]: newArray });
    } else {
      this.setState({ [e.target.name]: e.target.value });
    }
  };

  handleOtherOccupantsSwitchInputValue = (e, name, array, index) => {
    if (array) {
      let newArray = [...array];
      newArray[index].livesHere = e;

      // If value is true, set relationship to owner lives here (668020001), if not to owner not lives here (668020003)
      newArray[index].relationship = e ? 668020001 : 668020003;
      this.setState({ [name]: newArray });
    }
  };

  handleDeleteArray = (e, name, array, index) => {
    if (array) {
      var filtered = array.filter(function(value, ix, arr) {
        return ix != index;
      });
      this.setState({ [name]: filtered });
    }
  };

  calculateAndGenerate = (date, disability, propertyDate) => {
    let source = [];
    let yearsObjectArray = [];

    yearsObjectArray = determinateYearsToApply(
      date,
      disability,
      propertyDate,
      this.props.years
    );
    if (!arrayNullOrEmpty(yearsObjectArray)) {
      let jsonSavedArray = this.props.calculateAuthYearsObject(
        yearsObjectArray,
        true
      );

      source = this.props.returnToRadioButtomSource(jsonSavedArray);
      this.props.setOptionsArray(source);
    }
  };

  handleDateChange = async (date, id) => {
    if (isValidDate(date)) {
      this.setState({ [id]: date });
      if (id === 'firstDateAsPrimaryResidence') {
        let entered = new Date(date);
        let today = new Date();
        let requiredMissing = this.state.requiredMissing;
        let textIndex = requiredMissing.findIndex(missing => {
          return missing == 'firstDatePrimaryResidence';
        });
        if (textIndex > -1) requiredMissing.splice(textIndex, 1);
        this.setState({
          owned9months:
            today.getFullYear() * 12 +
              today.getMonth() -
              (entered.getFullYear() * 12 + entered.getMonth()) >
            8,
          requiredMissing: requiredMissing,
        });

        this.calculateAndGenerate(
          this.props.seniorApp.applicantdateofbirth,
          this.props.seniorApp.docdisability,
          date
        );
      }
    }
  };

  showAddAnotherProperty = () => {
    this.createOrEmptyOtherPropertyObject(true);
  };

  showAddAnotherOccupant = () => {
    this.createOrEmptyOccupantsObject(true);
  };

  handleCountryChangeValue = value => {
    this.setState({
      country: value,
      isUSA: value === this.state.usaCountryId,
    });
  };

  handleCheckCountryChangeValue = value => {
    this.setState({
      checkAddressCountryId: value,
      isCheckUSA: value === this.state.usaCountryId,
    });
  };

  getSuggestions = async (value, { showEmpty = false } = {}) => {
    let parcels = [];
    parcels = await parcelLookupAPIDebounced(value);
    return parcels && parcels.length > 0 ? parcels : [];
  };

  handleSuggestionsFetchRequested = async ({ value }) => {
    const parcelResponse = await parcelLookupAPIDebounced(value);
    console.log('handleSuggestionsFetchRequested', properties);
    let isPropertiesTimeout = false;
    let properties = [];
    if (typeof parcelResponse === 'object' && parcelResponse.isTimeout) {
      isPropertiesTimeout = true;
    } else {
      properties = parcelResponse;
    }
    this.setState(prevState => {
      return {
        isPropertiesTimeout,
        properties: !arrayNullOrEmpty(properties)
          ? properties.filter(a => a.ptas_name)
          : prevState.properties.filter(a => a.ptas_name),
      };
    });
  };
  handleSuggestionsClearRequested = () => {
    this.setState(
      {
        defaultParcel:
          this.state.properties.length == 1
            ? this.state.properties[0].ptas_parceldetailid
            : '',
        properties: [],
      },
      () => {
        if (this.state.parcelId == '') {
          this.setState({ parcelId: this.state.defaultParcel });
        }
      }
    );
  };

  handleSuggestionsMailingFetchRequested = async ({ value }) => {
    const mailingAddresses = await addressLookupDebounced(value);
    this.setState(prevState => {
      return {
        mailingAddresses: !arrayNullOrEmpty(mailingAddresses)
          ? mailingAddresses
          : prevState.mailingAddresses,
      };
    });
  };
  handleSuggestionsMailingClearRequested = () => {
    this.setState({ mailingAddresses: [] });
  };
  handleSuggestionsPropertyFetchRequested = async ({ value }) => {
    const propertyAddresses = await addressLookupDebounced(value);
    this.setState(prevState => {
      return {
        propertyAddresses: !arrayNullOrEmpty(propertyAddresses)
          ? propertyAddresses
          : prevState.propertyAddresses,
      };
    });
  };

  handleSuggestionsPropertyClearRequested = () => {
    this.setState({ propertyAddresses: [] });
  };

  handleChange = (event, { newValue }) => {
    let requiredMissing = this.state.requiredMissing;
    let textIndex = requiredMissing.findIndex(missing => {
      return missing == 'propertyId';
    });
    if (textIndex > -1) requiredMissing.splice(textIndex, 1);
    this.setState({ propertyId: newValue, requiredMissing: requiredMissing });
  };

  handleChangeMailing = (event, { newValue }) => {
    if (event.type == 'change') {
      this.setState({
        mailingAddress: newValue,
        mailingAddressStreet: newValue,
      });
    }
  };
  handleChangeCheckMailing = (event, { newValue }) => {
    if (event.type == 'change') {
      this.setState({
        checkAddress: newValue,
        checkAddressStreet: newValue,
      });
    }
  };

  handleChangeProperty = (event, newValue) => {
    this.setState({ propertyAddress: newValue });
  };
  handleChangeExemption = (event, { newValue }) => {
    if (event.type == 'change') {
      this.setState({
        exemptionAddress: newValue,
        exemptionStreetAddress: newValue,
      });
    }
  };

  handleChangeOtherProperty = o => {
    console.log('handleChangeOtherProperty', o);
  };

  changeProperty = () => {
    this.setState({
      propertyFound: false,
      firstNameProperty: '',
      streetProperty: '',
      cityProperty: '',
      parcelNumber: '',
      propertyId: '',
      parcelId: '',
      mailingAddressStreet: '',
      mailingAddressCity: '',
      mailingAddress: '',
      city: '',
      state: '',
      zip: '',
    });
  };

  getSuggestionValue = suggestion => {
    this.setState({
      propertyFound: true,
      firstNameProperty: suggestion.ptas_namesonaccount
        ? suggestion.ptas_namesonaccount
        : '',
      streetProperty: suggestion.ptas_address ? suggestion.ptas_address : '',
      cityProperty:
        (suggestion.ptas_district ? suggestion.ptas_district : '') +
        ', WA ' +
        (suggestion.ptas_zipcode ? suggestion.ptas_zipcode : ''),
      parcelNumber: suggestion.ptas_name ? suggestion.ptas_name : '',

      parcelId: suggestion.ptas_parceldetailid,
      accountNumber: suggestion.ptas_acctnbr,

      state: 'WA',
      city: suggestion.ptas_district,
      zip: suggestion.ptas_zipcode,
    });

    // If user selected to use a different mailing addres than the parcel's
    // don't replace the fiels.
    if (!this.state.addDifferentAddress) {
      this.setState({
        mailingAddressFullName: suggestion.ptas_namesonaccount
          ? suggestion.ptas_namesonaccount
          : '',
        mailingAddressStreet: suggestion.ptas_address,
        mailingAddressCity: suggestion.ptas_district,
        mailingAddressState: 'WA',
        mailingAddressZip: suggestion.ptas_zipcode
          ? suggestion.ptas_zipcode
          : '',
        mailingAddress: suggestion.ptas_address ? suggestion.ptas_address : '',
      });
    }

    return suggestion.ptas_name + ', ' + suggestion.ptas_address;
  };
  getSuggestionPropertyValue = (suggestion, name, array, index) => {
    /*this.setState({
      propertyAddress: suggestion.formattedaddr,
    });*/
    if (suggestion) {
      if (array) {
        let newArray = [...array];
        newArray[index].propertyAddress = suggestion.formattedaddr;
        newArray[index].propertyStreetAddress = suggestion.hasOwnProperty('zip')
          ? `${suggestion.streetname}, ${suggestion.zip}`
          : `${suggestion.streetname}, ${suggestion.country}`;
        newArray[index].propertyCounty = suggestion.city;
        newArray[index].propertyState = suggestion.state;
        this.setState({ [name]: newArray });
      }
    }
    return suggestion.formattedaddr;
  };

  getSuggestionExemptionValue = suggestion => {
    this.setState({
      exemptionAddressCity: suggestion.city,
      exemptionStreetAddress: suggestion.address
        ? `${suggestion.address} ${suggestion.streetname}`
        : `${suggestion.streetname}`,
      exemptionAddress: suggestion.formattedaddr
        ? suggestion.formattedaddr
        : '',
      exemptionAddressState: suggestion.state ? states[suggestion.state] : '',
      exemptionAddressZip: suggestion.zip ? suggestion.zip : '',
    });
    return suggestion.formattedaddr;
  };
  getSuggestionMailingValue = suggestion => {
    let selectedCountry = this.props.countries.find(c => {
      return c.defaultLocString == suggestion.country;
    });
    this.handleCountryChangeValue(selectedCountry.countryid);

    this.setState({
      mailingAddressStreet: suggestion.address
        ? `${suggestion.address} ${suggestion.streetname}`
        : `${suggestion.streetname}`,
      mailingAddressCity: suggestion.city,
      mailingAddress: suggestion.formattedaddr ? suggestion.formattedaddr : '',
      mailingAddressState: suggestion.state
        ? states[suggestion.state] || suggestion.state
        : '',
      mailingAddressZip: suggestion.zip ? suggestion.zip : '',
      country: selectedCountry.countryid,
    });
    return suggestion.formattedaddr;
  };
  getSuggestionCheckValue = suggestion => {
    let selectedCountry = this.props.countries.find(c => {
      return c.defaultLocString == suggestion.country;
    });
    this.handleCheckCountryChangeValue(selectedCountry.countryid);
    this.setState({
      checkAddressStreet: suggestion.address
        ? `${suggestion.address} ${suggestion.streetname}`
        : `${suggestion.streetname}`,
      checkAddressCity: suggestion.city,
      checkAddressState: suggestion.state ? states[suggestion.state] : '',
      checkAddress: suggestion.formattedaddr ? suggestion.formattedaddr : '',
      checkAddressPostalCode: suggestion.zip ? suggestion.zip : '',
      checkAddressCountryId: selectedCountry.countryid,
    });
    return suggestion.formattedaddr;
  };
  handleClickOpen = () => {
    this.setState({ infoOpen: true });
  };

  handleClose = () => {
    this.setState({ infoOpen: false });
  };

  handleClickOpenWhyNot = () => {
    this.setState({ infoOpenWhyNot: true });
  };

  handleCloseWhyNot = () => {
    this.setState({ infoOpenWhyNot: false });
  };

  handleContinueClick = async () => {
    if (this.canContinue()) {
      this.props.showSavingOverlay();
      window.scrollTo(0, 0);
      this.setState({ isSaving: true, continueText: fm.savingLabel });

      if (!this.props.readOnlyMode) {
        const promises = [
          this.handleMetadataUpdate(
            this.state.proofOfOwnershipFileArray,
            this.state.documents[1],
            false
          ),
          this.updateFileMetadata(),
          //this.updateSeniorApp(),
          this.createOrUpdateOtherOccupants(),
          this.createOrUpdateOtherProperties(),
          this.createOrUpdateSeniorDetails(),
        ];

        await Promise.all(promises);
        await this.updateSeniorApp();
      }

      this.props.nextTab();
      /*appInsights.trackEvent({
        name: 'continue',
        event: 'click',
        section: 'PropertyInfo',
      });*/
      this.props.hideSavingOverlay();
    } else {
      this.setState({
        showDialogDOBMSG: true,
        //showDialog: true,
        dialogAlreadyShowed: true,
        showDialogType: 2, // 0 info 1 warning 2 error
      });
    }
  };

  informationDialog = () => {
    if (!(this.props.editMode || this.props.readOnlyMode)) {
      return (
        <DialogCSS
          open={this.state.informationOpen}
          onClose={this.handleCloseInformation}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
        >
          <DialogContent style={{ marginTop: '20px', marginBottom: '20px' }}>
            <div className="info-icon-alert" style={{ marginRight: '27px' }}>
              <InfoOutlinedIcon className="clear-icon" />
            </div>
            <div className="info-icon-alert" style={{ width: '75%' }}>
              <DialogContentText
                id="alert-dialog-description"
                style={{ color: 'black' }}
              >
                <div>{fm.documentsToComplete}</div>
                <div>
                  <ul>
                    <li>{fm.proofResidenceOwnership}</li>
                    <li>{fm.proofMajorLifeChange}</li>
                    <li>{fm.coopShares}</li>
                  </ul>
                </div>
              </DialogContentText>
            </div>
            <div className="align-center">
              <CustomButton
                onClick={this.handleCloseInformation}
                label={fm.continueLabel}
              />
            </div>
          </DialogContent>
        </DialogCSS>
      );
    }
  };

  qualifyDialog = () => {
    if (!(this.props.editMode || this.props.readOnlyMode)) {
      return (
        <div>
          <QDialogCSS
            open={this.state.qualifyOpen}
            onClose={this.handleCloseQualifyDialog}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogActions>
              <IconButton onClick={this.handleCloseQualifyDialog}>
                <ClearIcon className="clear-icon" />
              </IconButton>
            </DialogActions>
            <DialogContent style={{ marginTop: '-50px', marginBottom: '20px' }}>
              <div className="info-icon-alert" style={{ marginRight: '27px' }}>
                <InfoOutlinedIcon className="clear-icon" />
              </div>
              <div className="info-icon-alert" style={{ width: '75%' }}>
                <DialogContentText
                  id="alert-dialog-description"
                  style={{ color: 'black' }}
                >
                  <div>{fm.basedOnInformation}</div>
                  <div className="display-inline">
                    {this.verifyIfUserMightQualify()}
                  </div>
                </DialogContentText>
              </div>
            </DialogContent>
          </QDialogCSS>
          <Dialog
            open={this.state.infoOpenWhyNot}
            onClose={this.handleCloseWhyNot}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogActions>
              <IconButton onClick={this.handleCloseWhyNot}>
                <ClearIcon className="clear-icon" />
              </IconButton>
            </DialogActions>
            <DialogContent style={{ marginTop: '-50px', marginBottom: '20px' }}>
              <div className="info-icon-alert" style={{ marginRight: '27px' }}>
                <InfoOutlinedIcon className="clear-icon" />
              </div>
              <div className="info-icon-alert" style={{ width: '75%' }}>
                <DialogContentText
                  id="alert-dialog-description"
                  style={{ color: 'black' }}
                >
                  {fm.whyNot_ht}
                </DialogContentText>
              </div>
            </DialogContent>
          </Dialog>
        </div>
      );
    }
  };

  canContinue = () => {
    this.setState({ warned: true });

    let canContinue = true;
    let requiredMissing = [];
    if (!this.state.parcelId || this.state.parcelId == '') {
      requiredMissing.push('propertyId');
      canContinue = false;
    }
    if (!this.state.firstDateAsPrimaryResidence) {
      requiredMissing.push('firstDatePrimaryResidence');
      canContinue = false;
    }

    if (!canContinue)
      this.setState({
        requiredMissing: requiredMissing,
        requiredPopup: true,
      });

    return canContinue;
    /*if (!this.state.propertyId || this.state.propertyId == '') {
      this.setState({ firstRequired: 'yourProperty' }, () => {
        return false;
      });
    } else if (!this.state.firstDateAsPrimaryResidence) {
      this.setState({ firstRequired: 'firstDateAsPrimaryResidence' }, () => {
        return false;
      });
    } else {
      return true;
    }*/
    /*let canContinue = true;

    if (
      !(this.state.propertyId && this.state.propertyId != '') ||
      !this.state.firstDateAsPrimaryResidence ||
      (!this.state.warned &&
        (this.state.hasExperiencedMajorLifeChanges &&
          this.state.proofOfChangeFileArray &&
          this.state.proofOfChangeFileArray.length < 1))
    )
      canContinue = false;

    this.setState({ warned: true });
    return canContinue;*/
  };

  closeDialog = () => {
    this.props.hideSavingOverlay();
    this.setState({
      showDialog: false,
      dialogAlreadyShowed: true,
      continueText: fm.continueLabel,
    });
    /*if (this.state.firstRequired && this.refs[this.state.firstRequired]) {
      setTimeout(() => {
        this.refs[this.state.firstRequired].scrollIntoView({
          block: 'center',
          behavior: 'smooth',
        });
        this.setState({ firstRequired: null });
      }, 100);
    }*/
  };

  lastQuery = '';

  renderParcelError = () => {
    if (this.state.isPropertiesTimeout) {
      return (
        <div className="parcel-error">
          <FormattedMessage
            id="property_timeoutMsg"
            defaultMessage="Too many results found."
          />
        </div>
      );
    }
    return (
      <div className="parcel-error">
        <FormattedMessage
          id="property_parcelNotFound"
          defaultMessage="No parcels or addresses found"
        />
      </div>
    );
  };

  render = () => {
    const autosuggestProps = {
      renderInputComponent,
      suggestions: this.state.properties,
      onSuggestionsFetchRequested: this.handleSuggestionsFetchRequested,
      onSuggestionsClearRequested: this.handleSuggestionsClearRequested,
      getSuggestionValue: this.getSuggestionValue,
      renderSuggestion,
    };
    const requiredMissing = (
      <div className="required-missing">
        <WarningIcon className="required-icon" />
        <ul>
          <p>{fm.youllneedsomeofthefollowing}</p>
          {this.state.requiredMissing.map((field, index) => {
            return (
              <li>
                <a
                  onClick={() =>
                    this.refs[field].scrollIntoView({
                      block: 'center',
                      behavior: 'smooth',
                    })
                  }
                  //href={'#' + field}
                >
                  {fm[field]}
                </a>
              </li>
            );
          })}
        </ul>
      </div>
    );

    const ColorCircularProgress = withStyles({
      root: {
        margin: 8,
        marginLeft: 'calc(50% - 20px)',
      },
      barColorPrimary: {
        backgroundColor: '#a5c727',
      },
      colorPrimary: {
        color: '#a5c727',
      },
    })(CircularProgress);

    const yourProperty = (
      <div
        ref="propertyId"
        style={{ display: 'inline-block' }}
        onBlur={() => {
          this.setState({ parcelBlurred: true });
          this.lastQuery = '';
        }}
        onFocus={() => {
          this.setState({ parcelBlurred: false });
          this.lastQuery = '';
        }}
      >
        <Autosuggest
          {...autosuggestProps}
          inputProps={{
            id: 'react-autosuggest-simple',
            label: 'Your property',
            placeholder: 'Your property',
            value: this.state.propertyId,
            onChange: this.handleChange,
            error:
              this.state.dialogAlreadyShowed &&
              this.state.propertyId.length < 1,
            helperText: fm.required,
          }}
          renderSuggestionsContainer={options => (
            <Paper {...options.containerProps} square>
              {options.children || this.state.propertyId == '' ? (
                options.children
              ) : this.lastQuery == options.query ? (
                this.state.parcelBlurred ? null : (
                  this.renderParcelError()
                )
              ) : this.state.parcelBlurred ? null : (
                <ColorCircularProgress>
                  {(this.lastQuery = options.query)}
                </ColorCircularProgress>
              )}
            </Paper>
          )}
          theme={theme}
        />
      </div>
    );

    const firstDateAsPrimaryResidence = (
      <div className="greyCollapse" ref="firstDatePrimaryResidence">
        <FormattedMessage
          id="property_firstDatePrimaryPlaceholder"
          defaultMessage="yyyy"
          description="first date placeholder"
        >
          {placeholder => (
            <DateInput
              id="firstDateAsPrimaryResidence"
              name="firstDateAsPrimaryResidenceInput"
              label={fm.firstDatePrimaryResidence}
              placeholder={placeholder}
              value={this.state.firstDateAsPrimaryResidence}
              onChange={this.handleDateChange}
              readOnly={this.props.readOnlyMode}
              shrinkLabel={true}
              yearOnly={true}
              required
              format="yyyy"
            />
          )}
        </FormattedMessage>
      </div>
    );

    const autosuggestCheckMailingProps = {
      renderInputComponent,
      suggestions: this.state.mailingAddresses,
      onSuggestionsFetchRequested: this.handleSuggestionsMailingFetchRequested,
      onSuggestionsClearRequested: this.handleSuggestionsMailingClearRequested,
      getSuggestionValue: this.getSuggestionCheckValue,
      renderSuggestion: renderSuggestionMailing,
    };
    const autosuggestMailingProps = {
      renderInputComponent,
      suggestions: this.state.mailingAddresses,
      onSuggestionsFetchRequested: this.handleSuggestionsMailingFetchRequested,
      onSuggestionsClearRequested: this.handleSuggestionsMailingClearRequested,
      getSuggestionValue: this.getSuggestionMailingValue,
      renderSuggestion: renderSuggestionMailing,
    };
    const autosuggestExemptionProps = {
      renderInputComponent,
      suggestions: this.state.mailingAddresses,
      onSuggestionsFetchRequested: this.handleSuggestionsMailingFetchRequested,
      onSuggestionsClearRequested: this.handleSuggestionsMailingClearRequested,
      getSuggestionValue: this.getSuggestionExemptionValue,
      renderSuggestion: renderSuggestionMailing,
    };

    const autosuggestPropertyProps = {
      renderInputComponent,
      suggestions: this.state.propertyAddresses,
      onSuggestionsFetchRequested: this.handleSuggestionsPropertyFetchRequested,
      onSuggestionsClearRequested: this.handleSuggestionsPropertyClearRequested,
      renderSuggestion: renderSuggestionMailing,
    };
    return (
      <AuthConsumer>
        {valueConsumer => (
          <CollectionConsumer>
            {value => (
              <TabContainer>
                {/* {this.qualifyDialog()}
            {this.informationDialog()} */}
                <div className="center-panel center-content center">
                  <Collapse
                    className="required-popup"
                    in={
                      this.state.requiredMissing.length > 0 &&
                      this.state.requiredPopup
                    }
                    style={{
                      top:
                        this.state.requiredMissing.length > 0
                          ? this.refs[this.state.requiredMissing[0]].offsetTop -
                            200
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
                  <div
                    className="center-myage-panel card-item"
                    style={{ marginBottom: 18 }}
                  ></div>
                  <Card className="cardStyle myProperty-center-panel">
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
                            <Grid sm={12}>{fm.property}</Grid>
                          </Grid>
                        </div>
                      </div>
                    </Hidden>
                    {!this.state.propertyFound && (
                      <CardSide
                        header={fm.propertyId}
                        content={[
                          fm.property_cardSide_primaryResidence,
                          fm.property_cardSide_condo,
                          fm.property_cardSide_mobileHome,
                        ]}
                      />
                    )}
                    <CardContent className="myProperty-subpanel">
                      <Collapse in={!this.state.propertyFound}>
                        <Fragment>
                          {yourProperty}
                          <HelpText
                            className="property-help-text"
                            label={fm.enterAddress}
                          />
                        </Fragment>
                      </Collapse>
                      <Collapse in={this.state.propertyFound}>
                        <div className="property-address-title">
                          {fm.propertyId}
                        </div>
                        {/*<div>{fm.infoOnCountyRecord}</div>*/}
                        <Card className="cardStyleBlack ">
                          <div style={{ margin: 15 }}>
                            {/* <div className="property-address-bold">
                              {this.state.firstNameProperty}
                            </div> */}
                            <div
                              className="property-address-bold"
                              data-testid="propAddress"
                            >
                              {this.state.streetProperty}
                              <br />
                              {this.state.cityProperty}
                            </div>
                            <div
                              className="property-address-white
                            "
                            >
                              {fm.parcelNumber}
                              &nbsp;
                              <span style={{ fontWeight: 'bold' }}>
                                {this.state.parcelNumber}
                              </span>
                            </div>
                          </div>
                          <CardContent className="mouseflow-hide">
                            {this.state.firstNameProperty}
                            {/* {valueConsumer.user
                              ? `${valueConsumer.user.lastName.toUpperCase()}, ${valueConsumer.user.firstName.toUpperCase()}`
                              : ''} */}
                          </CardContent>
                        </Card>
                        {!this.props.readOnlyMode && (
                          <div>
                            <span className="property-address">
                              {fm.notTheRightAddress}
                            </span>{' '}
                            <span
                              className="property-address-link"
                              onClick={this.changeProperty}
                            >
                              {fm.chageProperty}
                            </span>
                          </div>
                        )}
                      </Collapse>
                      <div className="property-form-switch">
                        <SwitchInputCSS
                          id="singleFamilyResidence"
                          label={fm.singleFamily}
                          checked={this.state.singleFamilyResidence}
                          onChange={(checked, e, id) =>
                            this.handleExclusiveSwitchInputChange(
                              checked,
                              e,
                              id,
                              this.state.exclusiveHomeTypes
                            )
                          }
                          readOnly={this.props.readOnlyMode}
                        />
                      </div>
                      <div className="property-form-switch">
                        <SwitchInputCSS
                          id="singleUnitOfMultiDwellingCondoOrDuplex"
                          label={fm.singleUnitMultiFamDwell}
                          checked={
                            this.state.singleUnitOfMultiDwellingCondoOrDuplex
                          }
                          onChange={(checked, e, id) =>
                            this.handleExclusiveSwitchInputChange(
                              checked,
                              e,
                              id,
                              this.state.exclusiveHomeTypes
                            )
                          }
                          readOnly={this.props.readOnlyMode}
                        />
                      </div>
                      <AlertDialog
                        text={fm.singleUnitSelectInfo}
                        isOpen={this.state.showSingleUnitInfoModal}
                        onClose={this.handleCloseSingleUnitInfoModal}
                      />
                      <div className="property-form-switch">
                        <SwitchInputCSS
                          id="mobileHome"
                          label={fm.mobileHome}
                          checked={this.state.mobileHome}
                          onChange={(checked, e, id) =>
                            this.handleExclusiveSwitchInputChange(
                              checked,
                              e,
                              id,
                              this.state.exclusiveHomeTypes
                            )
                          }
                          readOnly={this.props.readOnlyMode}
                        />
                      </div>
                      <AlertDialog
                        text={fm.mobileHomeSelectInfo}
                        isOpen={this.state.showMobileHomeInfoModal}
                        onClose={this.handleCloseMobileHomeInfoModal}
                      />
                      <Collapse
                        className="greyCollapse"
                        in={this.state.mobileHome}
                      >
                        <div className="property-form-input">
                          <CustomTextField
                            fullWidth
                            type="number"
                            label={fm.mobileHomeYear}
                            name="mobileHomeYear"
                            value={this.state.mobileHomeYear}
                            onChange={this.handleCustomInputChange}
                            changeDelay={800}
                          />
                        </div>
                        <div className="property-form-input">
                          <CustomTextField
                            fullWidth
                            label={fm.mobileHomeMake}
                            name="mobileHomeMake"
                            value={this.state.mobileHomeMake}
                            onChange={this.handleCustomInputChange}
                            changeDelay={800}
                          />
                        </div>
                        <div className="property-form-input margin-bottom-10">
                          <CustomTextField
                            fullWidth
                            label={fm.mobileHomeModel}
                            name="mobileHomeModel"
                            value={this.state.mobileHomeModel}
                            onChange={this.handleCustomInputChange}
                            changeDelay={800}
                          />
                        </div>
                      </Collapse>
                    </CardContent>
                  </Card>
                  <div>
                    <Hidden smUp>
                      <div className="card-separator"></div>
                    </Hidden>
                    <Card className="cardStyle myProperty-center-panel">
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
                              <Grid sm={12}>{fm.mailingAddressf}</Grid>
                            </Grid>
                          </div>
                        </div>
                      </Hidden>
                      <CardContent className="center-propinfo-panel">
                        <HelpText
                          label={fm.mailingAddressAssigned}
                          style={{ textAlign: 'left' }}
                        />
                        <div className="mailing-address">
                          {this.state.streetProperty}
                        </div>
                        <div className="mailing-address">
                          {this.state.cityProperty}
                        </div>

                        <SwitchInput
                          id="addDifferentAddress"
                          label={fm.differentAddress}
                          checked={this.state.addDifferentAddress}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '15px' }}
                        />

                        <Collapse
                          className="greyCollapse"
                          in={this.state.addDifferentAddress}
                        >
                          <TextInputML
                            name="mailingAddressFullName"
                            label={fm.mailingAddressFullName}
                            value={this.state.mailingAddressFullName}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                          <SelectInput
                            name="country"
                            label={fm.country}
                            source={value.countries}
                            value={this.state.country}
                            onChange={e =>
                              this.handleCountryChangeValue(e.target.value)
                            }
                            readOnly={this.props.readOnlyMode}
                            sourceValue="countryid"
                            sourceLabel="name"
                          />
                          <div
                            style={{
                              display: 'inline-block',
                              marginTop: '20px',
                            }}
                          >
                            <Autosuggest
                              {...autosuggestMailingProps}
                              inputProps={{
                                //classes,
                                id: 'react-autosuggest-simple-2',
                                label: 'Your mailing address',
                                placeholder: 'Your mailing address',
                                value: this.state.mailingAddress,
                                onChange: this.handleChangeMailing,
                              }}
                              renderSuggestionsContainer={options => (
                                <Paper {...options.containerProps} square>
                                  {options.children}
                                </Paper>
                              )}
                            />
                          </div>

                          <Collapse in={this.state.isUSA}>
                            <div style={{ textAlign: 'left' }}>
                              <div
                                style={{
                                  display: 'inline-block',
                                  marginLeft: '0px',
                                  width: '117px',
                                  marginRight: '10px',
                                }}
                              >
                                <TextInputMLCss117
                                  name="mailingAddressCity"
                                  label={fm.city}
                                  value={this.state.mailingAddressCity}
                                  onChange={this.handleTextInputChange}
                                  readOnly={this.props.readOnlyMode}
                                />
                              </div>
                              <TextInputMLCss40
                                name="mailingAddressState"
                                label={fm.state}
                                value={this.state.mailingAddressState}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />

                              <TextInputMLCss74
                                name="mailingAddressZip"
                                label={fm.zip}
                                value={this.state.mailingAddressZip}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                            </div>
                          </Collapse>

                          <Collapse in={!this.state.isUSA}>
                            <TextInputMLCssW117
                              name="mailingAddressZip"
                              label={fm.postalCode}
                              value={this.state.mailingAddressZip}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                            <TextInputML
                              name="mailingAddressCity"
                              label={fm.town}
                              value={this.state.mailingAddressCity}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                            <TextInputML
                              name="mailingAddressState"
                              label={fm.provinceAddress}
                              value={this.state.mailingAddressState}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                          </Collapse>
                        </Collapse>

                        <SwitchInput
                          id="differentCheckAddress"
                          label={fm.differentCheckAddress}
                          checked={this.state.differentCheckAddress}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '15px' }}
                        />

                        <Collapse
                          className="greyCollapse"
                          in={this.state.differentCheckAddress}
                        >
                          <TextInputML
                            name="checkAddressName"
                            label={fm.mailingAddressFullName}
                            value={this.state.checkAddressName}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                          <SelectInput
                            name="checkAddressCountryId"
                            label={fm.country}
                            source={value.countries}
                            value={this.state.checkAddressCountryId}
                            onChange={e =>
                              this.handleCheckCountryChangeValue(e.target.value)
                            }
                            readOnly={this.props.readOnlyMode}
                            sourceValue="countryid"
                            sourceLabel="name"
                          />
                          <div
                            style={{
                              display: 'inline-block',
                              marginTop: '20px',
                            }}
                          >
                            <Autosuggest
                              {...autosuggestCheckMailingProps}
                              inputProps={{
                                //classes,
                                id: 'checkAddressStreet',
                                label: 'Your mailing address',
                                placeholder: 'Your mailing address',
                                value: this.state.checkAddress,
                                onChange: this.handleChangeCheckMailing,
                              }}
                              renderSuggestionsContainer={options => (
                                <Paper {...options.containerProps} square>
                                  {options.children}
                                </Paper>
                              )}
                            />
                          </div>

                          <Collapse in={this.state.isCheckUSA}>
                            <div style={{ textAlign: 'left' }}>
                              <div
                                style={{
                                  display: 'inline-block',
                                  marginLeft: '0px',
                                  width: '117px',
                                  marginRight: '10px',
                                }}
                              >
                                <TextInputMLCss117
                                  name="checkAddressCity"
                                  label={fm.city}
                                  value={this.state.checkAddressCity}
                                  onChange={this.handleTextInputChange}
                                  readOnly={this.props.readOnlyMode}
                                />
                              </div>
                              <TextInputMLCss40
                                name="checkAddressState"
                                label={fm.state}
                                value={this.state.checkAddressState}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />

                              <TextInputMLCss74
                                name="checkAddressPostalCode"
                                label={fm.zip}
                                value={this.state.checkAddressPostalCode}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                            </div>
                          </Collapse>

                          <Collapse in={!this.state.isCheckUSA}>
                            <TextInputMLCssW117
                              name="checkAddressPostalCode"
                              label={fm.postalCode}
                              value={this.state.checkAddressPostalCode}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                            <TextInputML
                              name="checkAddressCity"
                              label={fm.town}
                              value={this.state.checkAddressCity}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                            <TextInputML
                              name="checkAddressState"
                              label={fm.provinceAddress}
                              value={this.state.checkAddressState}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                          </Collapse>
                        </Collapse>
                      </CardContent>
                    </Card>
                  </div>

                  <div>
                    <Hidden smUp>
                      <div className="card-separator"></div>
                    </Hidden>
                    <Card className="cardStyle myProperty-center-panel">
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
                              <Grid sm={12}> {fm.residence}</Grid>
                            </Grid>
                          </div>
                        </div>
                      </Hidden>

                      <CardContent className="center-propinfo-panel">
                        <SwitchInput
                          id="owner"
                          label={fm.owner}
                          checked={this.state.owner}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          className="greyCollapse"
                          in={this.state.owner}
                        >
                          <UploadFile
                            id={'ownerDoc'}
                            leftMessage={fm.ownerDoc}
                            obscureInfoMessage={this.obscureInfoMessage}
                            fileArray={this.state.leaseOrLifeEstateFileArray}
                            section={this.state.section}
                            document={this.state.documents[2].document}
                            appId={this.props.seniorApp.seapplicationid}
                            helpText={fm.helpTextShare}
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
                            hideIcon
                          />
                        </Collapse>
                        <SwitchInput
                          id="isPrimaryResidence"
                          label={fm.primaryResidence}
                          checked={this.state.isPrimaryResidence}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '40px' }}
                        />
                        {firstDateAsPrimaryResidence}
                        <div
                          className="myProperty-center-panel"
                          style={{ margin: '40px 0 0 15px' }}
                        >
                          {gm.fullDatePlaceholder(placeholder => (
                            <DateInput
                              id="propertyPurchaseDate"
                              name="propertyPurchaseDate"
                              label={fm.propertyPurchaseDate}
                              placeholder={placeholder}
                              value={this.state.propertyPurchaseDate}
                              onChange={this.handleDateChange}
                              readOnly={this.props.readOnlyMode}
                              shrinkLabel={true}
                            />
                          ))}
                        </div>

                        <Collapse in={!this.state.owned9months}>
                          <CustomWarning
                            className="occupy-9-months"
                            label={fm.mustOccupy9months(
                              this.props.selectedYear
                            )}
                          />
                        </Collapse>

                        <SwitchInput
                          id="liveNineMonthsOfYear"
                          label={fm.liveNineMonthsOfYear(
                            this.props.selectedYear
                          )}
                          checked={this.state.liveNineMonthsOfYear}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '40px' }}
                        />
                        <Collapse in={!this.state.liveNineMonthsOfYear}>
                          <CustomWarning
                            className="occupy-9-months"
                            label={fm.mustOwn9months(this.props.selectedYear)}
                          />
                        </Collapse>

                        <SwitchInput
                          id="soldFormerResidence"
                          label={fm.soldFormerResidence}
                          checked={this.state.soldFormerResidence}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '32px' }}
                        />
                        <Collapse
                          className="greyCollapse"
                          in={this.state.soldFormerResidence}
                        >
                          {gm.fullDatePlaceholder(placeholder => (
                            <DateInput
                              id="whenSoldFormerResidence"
                              name="whenSoldFormerResidence"
                              label={gm.when}
                              placeholder={placeholder}
                              value={this.state.whenSoldFormerResidence}
                              onChange={this.handleDateChange}
                              readOnly={this.props.readOnlyMode}
                              shrinkLabel={true}
                            />
                          ))}
                        </Collapse>
                        <SwitchInput
                          id="othersLiveInProperty"
                          label={fm.otherLiveProperty}
                          checked={this.state.othersLiveInProperty}
                          onChange={this.handleOccupantsSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '32px' }}
                        />
                        <Collapse
                          className="greyCollapse"
                          in={this.state.othersLiveInProperty}
                        >
                          {this.state.occupants &&
                            this.state.occupants.map((o, i) => {
                              return (
                                <React.Fragment>
                                  {renderIf(
                                    this.state.occupants.length > 1 && i > 0,
                                    <div
                                      style={{
                                        width: 'calc(100% + 18px)',
                                        minHeight: '2px',
                                        background: 'rgba(0, 0, 0, 0.1)',
                                        top: '23px',
                                        position: 'relative',
                                        right: '9px',
                                        marginTop: 20,
                                      }}
                                    ></div>
                                  )}
                                  <TextInputML
                                    testid={'firstName' + i}
                                    name="occupantFirstName"
                                    label={fm.firstName}
                                    style={{ marginTop: 30 }}
                                    value={o.occupantFirstName}
                                    onChange={e =>
                                      this.handleTextInputChange(
                                        e,
                                        'occupants',
                                        this.state.occupants,
                                        i
                                      )
                                    }
                                    readOnly={this.props.readOnlyMode}
                                  />
                                  <TextInputML
                                    testid={'middleName' + i}
                                    name="occupantMiddleName"
                                    label={fm.middleName}
                                    value={o.occupantMiddleName}
                                    onChange={e =>
                                      this.handleTextInputChange(
                                        e,
                                        'occupants',
                                        this.state.occupants,
                                        i
                                      )
                                    }
                                    readOnly={this.props.readOnlyMode}
                                  />
                                  <TextInputML
                                    name="occupantLastName"
                                    testid={'lastName' + i}
                                    label={fm.lastName}
                                    value={o.occupantLastName}
                                    onChange={e =>
                                      this.handleTextInputChange(
                                        e,
                                        'occupants',
                                        this.state.occupants,
                                        i
                                      )
                                    }
                                    readOnly={this.props.readOnlyMode}
                                  />
                                  <div style={{ marginTop: '30px' }}>
                                    <SelectInput
                                      inputid={'inputSuffix' + i}
                                      name="suffix"
                                      testid={'suffix' + i}
                                      label={fm.suffix}
                                      source={value.suffixes}
                                      value={o.suffix}
                                      onChange={e =>
                                        this.handleSelectChangeValue(
                                          e,
                                          'occupants',
                                          this.state.occupants,
                                          i
                                        )
                                      }
                                      readOnly={this.props.readOnlyMode}
                                      sourceValue="defaultLocString"
                                      sourceLabel="name"
                                    />
                                  </div>
                                  <div style={{ marginTop: '30px' }}>
                                    <SwitchInput
                                      testid={'livesHere' + i}
                                      name="livesHere"
                                      label={
                                        <FormattedMessage
                                          id="property_livesHere"
                                          defaultMessage="Lives at this property"
                                        />
                                      }
                                      checked={o.livesHere}
                                      onChange={e =>
                                        this.handleOtherOccupantsSwitchInputValue(
                                          e,
                                          'occupants',
                                          this.state.occupants,
                                          i
                                        )
                                      }
                                      readOnly={this.props.readOnlyMode}
                                    />
                                  </div>

                                  {renderIf(
                                    this.state.occupants.length > 1,
                                    <div
                                      className="property-address-link"
                                      style={{
                                        position: 'relative',
                                        float: 'right',
                                      }}
                                      onClick={e =>
                                        this.handleDeleteArray(
                                          e,
                                          'occupants',
                                          this.state.occupants,
                                          i
                                        )
                                      }
                                    >
                                      <br />
                                      {fm.removeOccupantLabel}
                                    </div>
                                  )}
                                  <br />
                                </React.Fragment>
                              );
                            })}
                        </Collapse>
                        {renderIf(
                          this.state.othersLiveInProperty,
                          <div
                            style={{
                              marginTop: '30px',
                              marginBottom: '30px',
                            }}
                          >
                            <ExpandLink
                              testid={'addOccupant'}
                              label={fm.addAnotherOccupant}
                              onclick={this.showAddAnotherOccupant}
                            />
                          </div>
                        )}
                        {/* Replaced text but also property to save value on  PBI #148407 */}
                        {/* <SwitchInput
                          id="isPropertyCoop"
                          label={fm.coOpName}
                          checked={this.state.isPropertyCoop}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        /> */}
                        <SwitchInput
                          id="isPropertyCoop"
                          label={fm.housingCoOp}
                          checked={this.state.isPropertyCoop}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.isPropertyCoop}
                          className="greyCollapse"
                        >
                          <TextInputML
                            name="nameOfCoop"
                            label={fm.nameOfCoop}
                            value={this.state.nameOfCoop}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                          <TextInputML
                            name="treasurer"
                            label={fm.treasurer}
                            value={this.state.treasurer}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />

                          <FormattedMessage
                            id="property_phoneNumberPlaceholder"
                            defaultMessage="000-000-0000"
                          >
                            {placeholder => (
                              <PhoneInput
                                name="treasurerPhoneNumber"
                                label={fm.treasurerPhoneNumber}
                                placeholder={placeholder}
                                value={this.state.treasurerPhoneNumber}
                                onChange={this.handleTextInputChange}
                              />
                            )}
                          </FormattedMessage>

                          <TextInputML
                            name="numberOfSharesYouOwn"
                            label={fm.numberOfSharesYouOwn}
                            value={this.state.numberOfSharesYouOwn}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                          <TextInputML
                            name="totalCoopShares"
                            label={fm.totalCoopShares}
                            value={this.state.totalCoopShares}
                            onChange={this.handleTextInputChange}
                            readOnly={this.props.readOnlyMode}
                          />
                          <div
                            style={{
                              margin: '40px 0 30px',
                              minWidth: '220px',
                              width: '232px',
                            }}
                          >
                            <CardSide
                              header={fm.shareCertificate}
                              content={[
                                fm.attachCertificate,
                                <VimeoSlider videos={[{ url: '382824186' }]} />,
                                fm.selectPhotoOfDocument,
                                fm.hideSocialSecurity,
                              ]}
                            />
                            <UploadFile
                              id={'shareCertificate'}
                              leftMessage={fm.shareCertificate}
                              obscureInfoMessage={this.obscureInfoMessage}
                              fileArray={this.state.shareCertificateFileArray}
                              section={this.state.section}
                              document={this.state.documents[0].document}
                              appId={this.props.seniorApp.seapplicationid}
                              helpText={fm.helpTextShare}
                              onCreate={array =>
                                this.handleMetadataUpdate(
                                  array,
                                  this.state.documents[0]
                                )
                              }
                              currentData={
                                this.state.currentShareCertificateFileArray
                              }
                              currDataName={'currentShareCertificateFileArray'}
                              setParentState={this.setParentState}
                              onDelete={array =>
                                this.handleMetadataUpdate(
                                  array,
                                  this.state.documents[0]
                                )
                              }
                              hideIcon
                            />
                          </div>
                        </Collapse>
                      </CardContent>
                    </Card>
                  </div>

                  <div>
                    <Hidden smUp>
                      <div className="card-separator"></div>
                    </Hidden>
                    <Card className="cardStyle myProperty-center-panel">
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
                              <Grid sm={12}>{fm.additionalInfo}</Grid>
                            </Grid>
                          </div>
                        </div>
                      </Hidden>
                      <CardContent className="center-propinfo-panel">
                        {/* Will be taken out of new application, and added back for renewals */}
                        {/* <SwitchInput
                          id="hasExperiencedMajorLifeChanges"
                          label={fm.householdChanges}
                          checked={this.state.hasExperiencedMajorLifeChanges}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '32px' }}
                        />
                        <Collapse
                          className="greyCollapse"
                          in={this.state.hasExperiencedMajorLifeChanges}
                        >
                          <div
                            style={{
                              margin: 'auto',
                            }}
                          >
                            <UploadFile
                              id={'proofChange'}
                              leftMessage={fm.proofChange}
                              obscureInfoMessage={this.obscureInfoMessage}
                              fileArray={this.state.proofOfChangeFileArray}
                              section={this.state.section}
                              document={this.state.documents[1].document}
                              appId={this.props.seniorApp.seapplicationid}
                              helpText={fm.helpTextChange}
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
                            />
                          </div>
                        </Collapse> */}
                        <SwitchInput
                          id="residenceForBusiness"
                          testid={'residenceForBusiness'}
                          label={fm.residenceForBusiness}
                          checked={this.state.residenceForBusiness}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.residenceForBusiness}
                          className="greyCollapse"
                        >
                          <SwitchInput
                            id="isResidenceForBusinessPercentageValue"
                            testid={'isResidenceForBusinessPercentageValue'}
                            label={fm.percentageValue}
                            checked={
                              this.state.isResidenceForBusinessPercentageValue
                            }
                            onChange={this.handleSwitchInputChange}
                            readOnly={this.props.readOnlyMode}
                            style={{ marginTop: '8px' }}
                          />
                          <div style={{ marginTop: '16px' }}>
                            {renderIf(
                              this.state.isResidenceForBusinessPercentageValue,
                              <PercentageInput
                                id="percentageUsedForBusiness"
                                name="percentageUsedForBusiness"
                                label={fm.providePercentage}
                                wrapLabel="true"
                                decimalScale={2}
                                allowNegative={false}
                                value={this.state.percentageUsedForBusiness}
                                onChange={this.handleNumericFloatInputChange}
                                readOnly={this.props.readOnlyMode}
                              />,
                              <FootSquareInput
                                id="squareFootageUsedForBusiness"
                                name="squareFootageUsedForBusiness"
                                label={fm.provideAmount}
                                wrapLabel="true"
                                decimalScale={0}
                                allowNegative={false}
                                value={this.state.squareFootageUsedForBusiness}
                                onChange={this.handleNumericIntInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                            )}
                          </div>
                        </Collapse>
                        <SwitchInput
                          id="rentPortionResidence"
                          testid={'rentPortionResidence'}
                          label={fm.rentPortionResidence}
                          checked={this.state.rentPortionResidence}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.rentPortionResidence}
                          className="greyCollapse"
                        >
                          <SwitchInput
                            id="isRentPortionPercentageValue"
                            testid={'isRentPortionPercentageValue'}
                            label={fm.percentageValue}
                            checked={this.state.isRentPortionPercentageValue}
                            onChange={this.handleSwitchInputChange}
                            readOnly={this.props.readOnlyMode}
                            style={{ marginTop: '8px' }}
                          />
                          <div style={{ marginTop: '16px' }}>
                            {renderIf(
                              this.state.isRentPortionPercentageValue,
                              <PercentageInput
                                id="percentageRentedOut"
                                name="percentageRentedOut"
                                label={fm.providePercentage}
                                wrapLabel="true"
                                decimalScale={2}
                                allowNegative={false}
                                value={this.state.percentageRentedOut}
                                onChange={this.handleNumericFloatInputChange}
                                readOnly={this.props.readOnlyMode}
                              />,
                              <FootSquareInput
                                id="squareFootageRentedOut"
                                name="squareFootageRentedOut"
                                label={fm.provideAmount}
                                wrapLabel="true"
                                decimalScale={0}
                                allowNegative={false}
                                value={this.state.squareFootageRentedOut}
                                onChange={this.handleNumericIntInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                            )}
                          </div>
                        </Collapse>
                        <SwitchInput
                          id="ownThroughTrust"
                          testid={'ownThroughTrust'}
                          label={
                            <FormattedMessage
                              id="property_ownThroughTrust"
                              defaultMessage="I own this property through a trust."
                            />
                          }
                          checked={this.state.ownThroughTrust}
                          onChange={this.handleTrustInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.ownThroughTrust}
                          className="greyCollapse"
                        >
                          <CardSide
                            header={fm.trustDocument}
                            content={[
                              fm.attachTrustDocument,
                              <VimeoSlider videos={[{ url: '382824186' }]} />,
                              fm.selectPhotoOfDocument,
                              fm.hideSocialSecurity,
                            ]}
                          />
                          <div style={{ marginTop: 32 }} />
                          <UploadFile
                            id="trustDocument"
                            leftMessage={
                              <FormattedMessage
                                id="property_trustDocument"
                                defaultMessage="Title of trust document"
                              />
                            }
                            obscureInfoMessage={this.obscureInfoMessage}
                            fileArray={this.state.proofOfOwnershipFileArray}
                            section={this.state.section}
                            document={this.state.documents[1].document}
                            appId={this.props.seniorApp.seapplicationid}
                            helpText={fm.helpTextShare}
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
                            currentData={
                              this.state.currentProofOfOwnershipFileArray
                            }
                            currDataName={'currentProofOfOwnershipFileArray'}
                            setParentState={this.setParentState}
                            hideIcon
                          />
                        </Collapse>

                        <SwitchInput
                          id="ownOtherProperties"
                          label={fm.otherProperties}
                          checked={this.state.ownOtherProperties}
                          onChange={this.handleIOwnOtherPropertiesInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />

                        <Collapse
                          in={this.state.ownOtherProperties}
                          className="greyCollapse"
                        >
                          <div
                            style={{
                              marginTop: '20px',
                              marginBottom: '20px',
                            }}
                          >
                            {this.state.otherProperties &&
                              this.state.otherProperties.map((op, i) => {
                                return (
                                  <React.Fragment>
                                    {renderIf(
                                      this.state.otherProperties.length > 1 &&
                                        i > 0,
                                      <div
                                        style={{
                                          width: 'calc(100% + 18px)',
                                          minHeight: '2px',
                                          background: 'rgba(0, 0, 0, 0.1)',
                                          position: 'relative',
                                          right: '9px',
                                          marginTop: 28,
                                        }}
                                      ></div>
                                    )}
                                    <div
                                      id="otherPropertiesDiv"
                                      style={{
                                        width: '100%',
                                      }}
                                    >
                                      <Autosuggest
                                        {...autosuggestPropertyProps}
                                        renderInputComponent={
                                          renderInputComponentProp
                                        }
                                        getSuggestionValue={e => {
                                          this.getSuggestionPropertyValue(
                                            e,
                                            'otherProperties',
                                            this.state.otherProperties,
                                            i,
                                            op
                                          );
                                        }}
                                        inputProps={{
                                          //classes,
                                          id: 'autosuggest-property',
                                          name: 'propertyAddress',
                                          label: 'Your property address',
                                          placeholder: 'Your property address',
                                          value: op.propertyAddress,
                                          onChange: e => {
                                            this.handleTextInputChange(
                                              e,
                                              'otherProperties',
                                              this.state.otherProperties,
                                              i,
                                              op
                                            );
                                          },
                                        }}
                                        renderSuggestionsContainer={options => (
                                          <Paper
                                            {...options.containerProps}
                                            square
                                          >
                                            {options.children}
                                          </Paper>
                                        )}
                                      />
                                      {/*<TextAreaML
                                      id="standard-multiline-flexible"
                                      multiline={true}
                                      row={2}
                                      key={op.propertyId || i}
                                      name="propertyAddress"
                                      label={fm.propertyAddress}
                                      style={{
                                        marginTop: '15px',
                                        display: 'flex',
                                      }}
                                      value={op.propertyAddress}
                                      onChange={e =>
                                        this.handleTextInputChange(
                                          e,
                                          'otherProperties',
                                          this.state.otherProperties,
                                          i
                                        )
                                      }
                                      readOnly={this.props.readOnlyMode}
                                      helperText={fm.enterFullAddress}
                                      //style={{ width: '319px' }}
                                    />*/}
                                      <div style={{ marginTop: '30px' }}>
                                        <SelectInput
                                          name="propertyPurpose"
                                          label={fm.propertyPurpose}
                                          source={value.purposes}
                                          value={op.propertyPurpose}
                                          onChange={e =>
                                            this.handleSelectChangeValue(
                                              e,
                                              'otherProperties',
                                              this.state.otherProperties,
                                              i
                                            )
                                          }
                                          readOnly={this.props.readOnlyMode}
                                          sourceValue="attributeValue"
                                          sourceLabel="value"
                                        />
                                      </div>
                                      {renderIf(
                                        this.state.otherProperties.length > 1,
                                        <div
                                          className="property-address-link"
                                          style={{
                                            position: 'relative',
                                            float: 'right',
                                            marginBottom: '15px',
                                          }}
                                          onClick={e =>
                                            this.handleDeleteArray(
                                              e,
                                              'otherProperties',
                                              this.state.otherProperties,
                                              i
                                            )
                                          }
                                        >
                                          {fm.removePropertyLabel}
                                        </div>
                                      )}
                                    </div>
                                  </React.Fragment>
                                );
                              })}
                          </div>
                        </Collapse>
                        {renderIf(
                          this.state.ownOtherProperties,
                          <ExpandLink
                            label={fm.addAnotherProperty}
                            onclick={this.showAddAnotherProperty}
                          />
                        )}
                        <SwitchInput
                          id="otherPropertyInCurrentTaxYear"
                          label={fm.otherPropertyInCurrentTaxYear(
                            this.props.selectedYear
                          )}
                          checked={this.state.otherPropertyInCurrentTaxYear}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.otherPropertyInCurrentTaxYear}
                          className="greyCollapse"
                        >
                          <div
                            className="myProperty-center-panel"
                            style={{ margin: '8px 0 0 15px' }}
                          >
                            {gm.fullDatePlaceholder(placeholder => (
                              <DateInput
                                id="whenOtherPropertyIn2020"
                                name="whenOtherPropertyIn2020"
                                label={gm.when}
                                placeholder={placeholder}
                                value={this.state.whenOtherPropertyIn2020}
                                onChange={this.handleDateChange}
                                readOnly={this.props.readOnlyMode}
                                shrinkLabel={true}
                              />
                            ))}

                            <TextInputML
                              name="whereOtherPropertyIn2020"
                              label={gm.where}
                              value={this.state.whereOtherPropertyIn2020}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                          </div>
                        </Collapse>
                        <SwitchInput
                          id="receivedExemptionBefore"
                          label={fm.receivedExemptionBefore}
                          checked={this.state.receivedExemptionBefore}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <Collapse
                          in={this.state.receivedExemptionBefore}
                          className="greyCollapse"
                        >
                          <div
                            className="myProperty-center-panel"
                            style={{ margin: '8px 0 0 15px' }}
                          >
                            {gm.fullDatePlaceholder(placeholder => (
                              <DateInput
                                id="whenReceivedExemptionBefore"
                                name="whenReceivedExemptionBefore"
                                label={gm.when}
                                placeholder={placeholder}
                                value={this.state.whenReceivedExemptionBefore}
                                onChange={this.handleDateChange}
                                readOnly={this.props.readOnlyMode}
                                shrinkLabel={true}
                              />
                            ))}

                            <TextInputML
                              name="whereReceivedExemptionBefore"
                              label={gm.where}
                              value={this.state.whereReceivedExemptionBefore}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                            />
                          </div>
                        </Collapse>
                        <SwitchInput
                          id="isTransferringExemption"
                          label={fm.transferringExemption}
                          checked={this.state.isTransferringExemption}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />

                        <Collapse
                          in={this.state.isTransferringExemption}
                          className="greyCollapse"
                        >
                          <div
                            style={{
                              display: 'inline-block',
                              marginTop: '4px',
                              marginBottom: '20px',
                            }}
                          >
                            <TextInputML
                              name="otherParcelNumber"
                              testid="otherParcelNumber"
                              label={fm.parcelNumber}
                              helperText={fm.otherParcelNumber}
                              value={this.state.otherParcelNumber}
                              onChange={this.handleTextInputChange}
                              readOnly={this.props.readOnlyMode}
                              type="text"
                            />
                            <Autosuggest
                              {...autosuggestExemptionProps}
                              inputProps={{
                                //classes,
                                id: 'react-autosuggest-simple-3',
                                label: 'Previous Exemption Address',
                                placeholder: 'Previous Exemption Address',
                                value: this.state.exemptionAddress,
                                onChange: this.handleChangeExemption,
                              }}
                              renderSuggestionsContainer={options => (
                                <Paper {...options.containerProps} square>
                                  {options.children}
                                </Paper>
                              )}
                            />

                            <Collapse in={this.state.isUSA}>
                              <div style={{ textAlign: 'left' }}>
                                <div
                                  style={{
                                    display: 'inline-block',
                                    marginLeft: '0px',
                                    width: '117px',
                                    marginRight: '10px',
                                  }}
                                >
                                  <TextInputMLCss117
                                    name="exemptionAddressCity"
                                    label={fm.city}
                                    value={this.state.exemptionAddressCity}
                                    onChange={this.handleTextInputChange}
                                    readOnly={this.props.readOnlyMode}
                                  />
                                </div>
                                <TextInputMLCss40
                                  name="exemptionAddressState"
                                  label={fm.state}
                                  value={this.state.exemptionAddressState}
                                  onChange={this.handleTextInputChange}
                                  readOnly={this.props.readOnlyMode}
                                />

                                <TextInputMLCss74
                                  name="exemptionAddressZip"
                                  label={fm.zip}
                                  value={this.state.exemptionAddressZip}
                                  onChange={this.handleTextInputChange}
                                  readOnly={this.props.readOnlyMode}
                                />
                              </div>
                            </Collapse>

                            <Collapse in={!this.state.isUSA}>
                              <TextInputMLCssW117
                                name="exemptionAddressZip"
                                label={fm.postalCode}
                                value={this.state.exemptionAddressZip}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                              <TextInputML
                                name="exemptionAddressCity"
                                label={fm.town}
                                value={this.state.exemptionAddressCity}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                              <TextInputML
                                name="exemptionAddressState"
                                label={fm.provinceAddress}
                                value={this.state.exemptionAddressState}
                                onChange={this.handleTextInputChange}
                                readOnly={this.props.readOnlyMode}
                              />
                            </Collapse>
                          </div>

                          {/*<FormattedMessage
                            id="property_datePlaceholder"
                            defaultMessage="mm/dd/yyyy"
                            description="date placeholder"
                          >
                            {placeholder => (
                              <DateInput
                                id="firstDatePreviousResidence"
                                name="firstDatePreviousResidenceInput"
                                label={fm.firstDatePrimaryResidence}
                                placeholder={placeholder}
                                value={this.state.firstDatePreviousResidence}
                                onChange={this.handleDateChange}
                                readOnly={this.props.readOnlyMode}
                                shrinkLabel="true"
                              />
                            )}
                          </FormattedMessage>
                          <div
                            style={{
                              marginTop: '30px',
                              marginBottom: '30px',
                            }}
                          >
                            <SelectInput
                              name="countyPreviousResidence"
                              label={fm.countyPreviousResidence}
                              source={value.counties}
                              value={this.state.countyPreviousResidence}
                              onChange={this.handleSelectChangeValue}
                              readOnly={this.props.readOnlyMode}
                              sourceValue="countyid"
                              sourceLabel="locId"
                            />
                          </div>
                         */}
                        </Collapse>

                        <SwitchInput
                          id="isDelinquent"
                          label={fm.isDelinquent}
                          checked={this.state.isDelinquent}
                          onChange={this.handleSwitchInputChange}
                          readOnly={this.props.readOnlyMode}
                          style={{ marginTop: '8px' }}
                        />
                        <div style={{ marginLeft: 94 }}>
                          <span className="property-address">
                            {fm.unsureIfTaxesAreDelinquent}
                          </span>
                          <br />
                          <a
                            className="property-address-link"
                            href={`${process.env.REACT_APP_REALPROPERTYLOOKUP +
                              this.getFormatedParcelId(
                                this.state.parcelNumber.trim().replace('-', '')
                              )}`}
                            target="_blank"
                          >
                            {fm.checkPropertyStatus}
                          </a>
                        </div>
                      </CardContent>
                    </Card>
                  </div>
                  <CustomDialog
                    showDialog={this.state.showDialog}
                    showDialogType={this.state.showDialogType}
                    handleCloseContinue={this.closeDialog}
                    propertyId={
                      !(
                        this.state.parcelNumber &&
                        this.state.parcelNumber !== ''
                      )
                    }
                    acquiredDate={!this.state.acquiredDate}
                    proofOfOwnership={arrayNullOrEmpty(
                      this.state.proofOfOwnershipFileArray
                    )}
                    isPrimaryResidence={
                      this.state.isPrimaryResidence
                        ? false
                        : !this.state.firstDateAsPrimaryResidence
                    }
                    hasExperiencedMajorLifeChanges={
                      this.state.hasExperiencedMajorLifeChanges &&
                      arrayNullOrEmpty(this.state.proofOfChangeFileArray)
                    }
                  ></CustomDialog>
                  <div className="card-end">
                    {renderIf(
                      this.state.requiredMissing.length > 0,
                      requiredMissing
                    )}
                  </div>
                  <div className="continue-summary-panel">
                    <CustomButton
                      style={{
                        margin: '10px 0',
                      }}
                      btnBigLabel={true}
                      onClick={this.handleContinueClick}
                      label={this.state.continueText}
                    />
                    {(this.props.editMode || this.props.readOnlyMode) && (
                      <SummaryButton
                        className="display-inline"
                        onClick={this.props.onNavigateToSummary}
                        label={fm.returnToSummary}
                      />
                    )}
                  </div>
                </div>
              </TabContainer>
            )}
          </CollectionConsumer>
        )}
      </AuthConsumer>
    );
  };
}

PropertyInfo.contextType = CollectionContext;
export default PropertyInfo;
