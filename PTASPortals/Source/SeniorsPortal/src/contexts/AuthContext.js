//-----------------------------------------------------------------------
// <copyright file="AuthContext.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import decodeJWT from 'jwt-decode';
import {
  getContactByEmail,
  createContact,
  updateContact,
  getFileMetadataListBySeniorAppId,
  deleteSeniorAppDetail,
} from '../services/dynamics-service';
import { getAccessToken, signOut } from '../contexts/MagicLinkContext';

import * as dataServiceProvider from '../services/dataServiceProvider';

import { getBaseFileMetadataObject } from '../lib/data-mappings/fileMetadataToFileArray';
import {
  arrayNullOrEmpty,
  removeRepeatedObjectFromArray,
} from '../lib/helpers/util';
import { yearsToApplyMyInfo } from '../lib/helpers/age';
import { FormattedMessage } from 'react-intl';
import uuid from 'uuid';

import {
  applyJsonAndMoveToPermanentStorage,
  deleteBlob,
} from '../services/blobService';

import { createOrUpdateFileMetadataEntities } from '../components/common/UploadFile/uploaderHelper';
import {
  getValue,
  setValue,
  deleteValue,
  postValue,
} from '../services/jsonStoreService';
import { cloneDeep } from 'lodash';

import { generateRadioButtomSource } from '../lib/helpers/age';
import { sumOnFinancialForms, replaceDotBySymbol } from '../lib/helpers/util';
import {
  deleteSeniorApplicationsFinancial,
  updateSeniorAppFinancial,
} from '../services/dataServiceProvider';
import deepEqual from 'deep-equal';

const AuthContext = React.createContext();

class AuthProvider extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuth: false,
      qrToken: '',
      token: '',
      user: null,
      decoded: null,
      contact: null,
      seniorApp: null,
      seniorApps: [],
      filesMetadata: [],
      otherProperties: [],
      occupants: [],
      seniorAppDetails: [],
      seniorAppsDetails: {},
      financials: null,
      readOnlyMode: false,
      showLoading: null,
      loadingFormatter: null,
      showSubmitDoneConfirmation: false,
      showSubmitFailedConfirmation: false,
      htmlSummary: null,
      yearsToApplyArray: [],
      displayYearsPopUp: false,
      globalYearOptionsArray: null,
      selectedYear: null,
      recommendedYear: null,
      earlierYear: null,
      displayWarningPopUp: false,
      newSelectedYear: null,
      prevSelectedYear: null,
      sectionsVideoPlayedArray: [],
      openMaxIncomeDialog: false,
      maxIncomeDialogMsg: {},
    };
  }

  componentDidMount = () => {
    const token = getAccessToken();

    if (token) {
      this.setState({ isAuth: true, token });
    } else {
      this.setState({ isAuth: false });
    }
  };

  setHtmlSummary = htmlString => {
    this.setState({ htmlSummary: htmlString });
  };

  checkClientBrowser = async => {
    let nVer = navigator.appVersion;
    let nAgt = navigator.userAgent;
    let browserName = navigator.appName;
    let fullVersion = '' + parseFloat(navigator.appVersion);
    let majorVersion = parseInt(navigator.appVersion, 10);
    let nameOffset, verOffset, ix;
    // In MSIE, the true version is after "MSIE" in userAgent
    if ((verOffset = nAgt.indexOf('Trident')) != -1) {
      browserName = 'Microsoft Internet Explorer';
      fullVersion = nAgt.substring(verOffset + 5);
    } // In Chrome, the true version is after "Chrome"
    else if ((verOffset = nAgt.indexOf('Chrome')) != -1) {
      browserName = 'Chrome';
      fullVersion = nAgt.substring(verOffset + 7);
    }
    return browserName == 'Microsoft Internet Explorer';
  };

  /**
   *Iterates all the file attachment values and correct data properties to latest application values
   *
   * @memberof AuthProvider
   */
  rectifyFileAttachmentValues = async seniorApp => {
    let filesMetadata = cloneDeep(this.state.filesMetadata);
    let promises = filesMetadata.map(async fileMeta => {
      //fileMeta.originalFilename = '';
      fileMeta.accountNumber = seniorApp.accountnumber;
      await dataServiceProvider.updateFileMetadata(fileMeta);
    });

    await Promise.all(promises);
    this.setState({ filesMetadata });
  };

  /**
   * Corrects parcel and contact info for the detail entity before.
   *
   * @memberof AuthProvider
   */
  rectifyDetailValues = async seniorApp => {
    let detail = cloneDeep(this.state.seniorAppDetails[0]);
    detail = this.state.seniorAppDetails[0];
    detail.parcelid = seniorApp.parcelid;
    detail.contactid = this.state.contact.contactid;
    this.setState({ seniorAppDetails: [detail] });
    await dataServiceProvider.updateSeniorAppDetail(detail);
  };

  /**
   *Corrects parcel and contact info for the detail entity before.
   *
   * @memberof AuthProvider
   */
  rectifyFinancialFormValues = async seniorApp => {
    let financialForms = cloneDeep(
      this.state.financials[this.state.seniorAppDetails[0].seappdetailid]
    );
    let promises = financialForms.map(async f => {
      f.parcelid = seniorApp.parcelid;
      f.contactid = this.state.contact.contactid;
      await dataServiceProvider.updateSeniorAppFinancial(f);
    });

    await Promise.all(promises);
  };

  submitSeniorApp = async currentSeniorApp => {
    if (currentSeniorApp) {
      let seniorApp = { ...currentSeniorApp };
      dataServiceProvider.setRootIds(seniorApp, this.state.contact);

      if (seniorApp.statuscode === 668020012) {
        this.showLoadingOverLay(
          true,
          <FormattedMessage
            id="auth_submit_message"
            defaultMessage="Submitting application..."
          />
        );

        // update years object
        let yearObjectArray = cloneDeep(this.state.yearsToApplyArray);

        let selectedYear = yearObjectArray.filter(
          y => y.seniorAppId === seniorApp.seapplicationid
        )[0];

        // Bug #113837 fix:
        await Promise.all([
          this.rectifyFileAttachmentValues(seniorApp),
          this.rectifyDetailValues(seniorApp),
          this.rectifyFinancialFormValues(seniorApp),
        ]);

        // Will move the current senior application from the Json Store to the Data Services and then call the move to ilinx method.
        // No await to make the call async
        applyJsonAndMoveToPermanentStorage(
          this.state.contact.contactid,
          seniorApp.seapplicationid,
          seniorApp.accountnumber,
          `${selectedYear.name} Application Received`,
          selectedYear.name
        );

        selectedYear.applied = true;
        this.saveYearsObject(yearObjectArray);

        // Clean this application data from localStorage.
        localStorage.removeItem(`${seniorApp.seapplicationid}_seapplicationid`);
        localStorage.removeItem(`${seniorApp.seapplicationid}_lastTab`);
        localStorage.removeItem(`${seniorApp.seapplicationid}_DocuSignHTML`);

        // Change status to new, this start the workflows on Dynamics CE.
        seniorApp.statuscode = 668020013;

        // Bug #115830 fix:
        // Issue: Dynamics Date control shows date always to the day before as the data submitted.
        // Reason: It stores only the date part of the data and converts it to local time from UTC. For a correct UTC or local date like "Thu, 23 Jul 2020 14:52:58 GMT"
        // it will store only "2020-07-23T00:00:00Z" and displays "7/22/2020"
        // Fix: Always add 1 more day so whe subtracting (7) for the local time (PST/Seattle) it will always be the correct date.
        let receivedDate = new Date();
        receivedDate.setDate(receivedDate.getDate() + 1);

        // Set date to UTC date.
        seniorApp.datereceived = receivedDate.toUTCString();

        await this.updateSeniorApp(seniorApp);
        await this.getSeniorApps(this.state.contact.contactid);

        this.setState({
          showSubmitDoneConfirmation: true,
          selectedYear: null,
          newSelectedYear: null,
          prevSelectedYear: null,
        });

        // display new loading indicator to reflect pulling fresh data
        this.showLoadingOverLay(
          true,
          <FormattedMessage id="Auth_fetch_info" />
        );
        this.showLoadingOverLay(false, null);
      }
    }
  };

  isLoggedIn = () => {
    const token = getAccessToken();
    if (token) {
      if (!this.state.isAuth) {
        this.setState({ isAuth: true, token });
      }
      return true;
    } else {
      if (this.state.isAuth) {
        this.setState({ isAuth: false, token });
      }

      return false;
    }
  };

  logout = () => {
    signOut();
    this.setState({ isAuth: false, token: null, user: null, contact: null });
    if (this.state.seniorApp) {
      localStorage.removeItem(
        `${this.state.seniorApp.seapplicationid}_seapplicationid`
      );
      localStorage.removeItem(
        `${this.state.seniorApp.seapplicationid}_lastTab`
      );
    }
  };

  getToken = () => {
    return this.state.token;
  };

  /**
   *
   *
   * @memberof AuthProvider
   */
  showLoadingOverLay = (show, formatter) => {
    this.setState({ showLoading: show, loadingFormatter: formatter });
  };

  getFilesMetadata = async (seniorAppId, useDataServiceProvider = true) => {
    let filesMetadata = [];
    if (seniorAppId) {
      if (useDataServiceProvider) {
        filesMetadata = await dataServiceProvider.getFileMetadataListBySeniorAppId(
          seniorAppId
        );
      } else {
        filesMetadata = await getFileMetadataListBySeniorAppId(seniorAppId);
      }
      this.setState({ filesMetadata });
      return filesMetadata;
    }
  };

  getOtherProperties = async seniorAppId => {
    if (seniorAppId) {
      this.setState({
        otherProperties: await dataServiceProvider.getOtherPropertiesBySeniorAppId(
          seniorAppId
        ),
      });
    }
  };

  getOccupants = async seniorAppId => {
    if (seniorAppId) {
      const allOccupants = await dataServiceProvider.getSeniorOccupantsBySeniorAppId(
        seniorAppId
      );
      // Remove taxpayer occupant (automatically created on the Dynamics CE backend) from the state so that Portal flow works as normal, assuming an occupant is only the
      // occupants added by the taxpayer, use fullname + o.occupanttype === 668020001 to find the automatically created occupant.
      const taxPayerFullName = `${
        this.state.contact.firstname ? this.state.contact.firstname : ''
      } ${this.state.contact.middlename ? this.state.contact.middlename : ''} ${
        this.state.contact.lastname ? this.state.contact.lastname : ''
      }`;

      const filterOccupants = allOccupants.filter(o => {
        const coOwnerFullName = `${
          o.occupantfirstName ? o.occupantfirstName : ''
        } ${o.occupantmiddlename ? o.occupantmiddlename + '' : ''} ${
          o.occupantlastname ? o.occupantlastname : ''
        }`;

        return o.occupanttype &&
          o.occupanttype === 668020001 &&
          coOwnerFullName != taxPayerFullName
          ? true
          : false;
      });
      this.setState({
        occupants: filterOccupants,
      });
    }
  };

  getSeniorFinancials = async seniorAppDetails => {
    if (!arrayNullOrEmpty(seniorAppDetails)) {
      let financials = {};
      let promises = seniorAppDetails.map(async d => {
        financials[
          d.seappdetailid
        ] = await dataServiceProvider.getSeniorAppFinancialBySeniorAppDetailId(
          d.seappdetailid
        );
      });

      await Promise.all(promises);

      this.setState({ financials });
    }
  };

  getSeniorDetails = async seniorAppId => {
    if (seniorAppId) {
      let details = await dataServiceProvider.getSeniorAppDetailsBySeniorAppId(
        seniorAppId
      );
      if (this.state.seniorApp) {
        this.setState({ seniorAppDetails: details });
      }
      return details;
    }
  };

  setSelectedSeniorAppDetails = seniorAppId => {
    let object = this.state.seniorAppsDetails[seniorAppId];
    if (arrayNullOrEmpty(object)) {
      object = {
        seapplicationid: seniorAppId,
        seappdetailid: uuid.v4(),
        statuscode: 668020013,
        statecode: 0,
      };
      dataServiceProvider.createSeniorAppDetail(object);
      object = [object];
    }

    // Correct missing state code.
    object[0].statecode =
      object[0].statecode == null || object[0].statecode == undefined
        ? 0
        : object[0].statecode;

    this.setState({ seniorAppDetails: object }, () => {
      this.getSeniorFinancials(this.state.seniorAppDetails);
    });
  };

  updateFilesMetadataState = filesMetadata => {
    this.setState({ filesMetadata });
  };

  updateSeniorApp = async (seniorApp, index, dataServices = true) => {
    let updatedSeniorApp = await dataServiceProvider.updateSeniorApplication(
      seniorApp,
      dataServices
    );

    this.setState(prevState => {
      return {
        seniorApp: updatedSeniorApp ? updatedSeniorApp : prevState.seniorApp,
      };
    });
  };

  updateContact = async contact => {
    let updatedContact = await updateContact(contact);
    this.setState(prevState => {
      return { contact: updatedContact ? updatedContact : prevState.contact };
    });
  };

  getSeniorApps = async contactId => {
    let seniorApps = await dataServiceProvider.getSeniorApplications(contactId);
    if (!arrayNullOrEmpty(seniorApps)) {
      this.setState({ seniorApps });
      let seniorAppsDetails = {};
      let promises = seniorApps.map(async s => {
        seniorAppsDetails[s.seapplicationid] = await this.getSeniorDetails(
          s.seapplicationid
        );
      });

      await Promise.all(promises);
      if (seniorAppsDetails) {
        this.setState({ seniorAppsDetails });
      }
      this.showLoadingOverLay(false, null);
    } else {
      this.showLoadingOverLay(false, null);
    }
  };

  createSeniorApp = async contactId => {
    this.showLoadingOverLay(
      true,
      <FormattedMessage
        id="Auth_create-senior-app"
        defaultMessage="Creating new application..."
      />
    );

    let seniorApp = await dataServiceProvider.createSeniorApplication(
      {
        contactid: contactId,
        seapplicationid: uuid.v4(),
        statecode: 0,
        // Represents undefined status, all Senior Apps should be created with this status
        statuscode: 668020012,
        // Represents that the source is web
        source: 668020001,
        // Represents the the account type is Residential Property
        accounttype: 668020000,
        // Represents application type = New
        applicationtype: 668020000,
        applicationdate: new Date().toUTCString(),
        // New fields from first UAT round, should match values on contact:
        applicantfirstname: this.state.contact.firstname,
        applicantlastname: this.state.contact.lastname,
        applicantemailaddress: this.state.contact.emailaddress,
        applicantmiddlename: this.state.contact.middlename,
        applicantsuffix: this.state.contact.suffix,
        applicantdateofbirth: this.state.contact.birthdate,
        applicantmobilephone: this.state.contact.phone,
      },
      true
    );

    //postValue();

    this.setState(
      prevState => {
        return {
          seniorApps: [...prevState.seniorApps, seniorApp],
          seniorApp,
          readOnlyMode: false,
          redirectToSeniors: true,
        };
      },
      () => {
        this.showLoadingOverLay(false, null);
      }
    );

    this.getSeniorAppData(seniorApp.seapplicationid);
  };
  toggleRedirectToSeniors = () => {
    this.setState({ redirectToSeniors: false });
  };
  setSeniorApp = seniorApp => {
    let readOnlyMode = false;
    if (seniorApp.statuscode !== 668020012) {
      readOnlyMode = true;
    }
    dataServiceProvider.setRootIds(seniorApp, this.state.contact);
    this.setState({ seniorApp, readOnlyMode, redirectToSeniors: false });
    this.getSeniorAppData(seniorApp.seapplicationid);
  };

  getSeniorAppData = seniorAppId => {
    this.getFilesMetadata(seniorAppId);
    this.getOtherProperties(seniorAppId);
    this.getOccupants(seniorAppId);
    this.setSelectedSeniorAppDetails(seniorAppId);
  };

  onShowSubmitDoneConfirmation = show => {
    this.setState({ showSubmitDoneConfirmation: show });
  };

  onShowSubmitFailedConfirmation = show => {
    this.setState({ showSubmitFailedConfirmation: show });
  };

  getOrCreateContact = async user => {
    this.showLoadingOverLay(
      true,
      <FormattedMessage
        id="Auth_fetch_info"
        defaultMessage="Loading existing information..."
      />
    );

    let contact = await getContactByEmail(user.email);

    if (!contact) {
      contact = await createContact({
        contactid: uuid.v4(),
        firstname: user.firstName,
        lastname: user.lastName,
        emailaddress: user.email,
      });
    }
    this.setState({ contact });
    if (contact) {
      //await this.saveYearsObject([]);
      await this.getSeniorApps(contact.contactid);
      //gets and updates the years to apply object ftom the blob
      await this.getYearsObject();

      //getVideosToPlay List
      //this.getSectionsVideoPlayedObject();
    }
  };

  currentUser = async () => {
    let user = {
      firstName: '',
      lastName: '',
      emails: '',
    };
    const token = getAccessToken();
    let decoded = null;
    if (token) {
      decoded = decodeJWT(token);
      user = {
        firstName: '',
        lastName: '',
        email: decoded.sub,
      };
      this.setState({
        user: user,
        decoded,
        token,
        qrToken: decoded.qrToken,
      });
      await this.getOrCreateContact(user);
    }

    return user;
  };

  saveYearsObject = async value => {
    // cleans any repeated items from the array.
    //value = [...new Set(value)];
    const newValue = removeRepeatedObjectFromArray(
      value,
      'seniorAppId',
      'yearId'
    );

    await postValue(
      `${this.state.contact.contactid}/${'yearsToApplyArray'}`,
      newValue,
      null
    );
    this.setState({ yearsToApplyArray: newValue });
  };

  getYearsObject = async () => {
    let yearsToApply = await getValue(
      `${this.state.contact.contactid}/yearsToApplyArray`
    );
    if (arrayNullOrEmpty(yearsToApply)) return;

    const seApps = this.state.seniorApps || [];
    let seniorAppsByYearId = {};
    seApps.forEach(sad => {
      const seDetail = (this.state.seniorAppsDetails[sad.seapplicationid] ||
        [])[0];
      if (!seDetail) return;
      seniorAppsByYearId = {
        ...seniorAppsByYearId,
        [seDetail.yearid]: sad,
      };
    });
    const yearToApplyUpdated = (yearsToApply[0].json || []).map(ya => {
      const seniorAppFound = seniorAppsByYearId[ya.yearId];
      if (!seniorAppFound) {
        return {
          ...ya,
          applied: false,
        };
      }

      if (seniorAppFound.statuscode === 668020012 && ya.applied) {
        return {
          ...ya,
          applied: false,
        };
      } else if (
        seniorAppFound.statuscode !== 668020012 &&
        ya.applied === false
      ) {
        return {
          ...ya,
          applied: true,
          seniorAppId: seniorAppFound.seapplicationid,
        };
      }
      return ya;
    });
    this.setState({ yearsToApplyArray: yearToApplyUpdated });
  };

  getSectionsVideoPlayedObject = async () => {
    let sectionsVideoPlayed = await getValue(
      `${this.state.contact.contactid}/sectionsVideoPlayed`
    );
    let tempArray = [];
    sectionsVideoPlayed[0] && (tempArray = sectionsVideoPlayed[0].json);

    if (!arrayNullOrEmpty(tempArray)) {
      this.setState({ sectionsVideoPlayedArray: sectionsVideoPlayed.json });
    }

    return sectionsVideoPlayed.json;
  };

  setSectionsVideoPlayedObject = async value => {
    // cleans any repeated items from the array.
    //value = [...new Set(value)];
    if (this.state.contact) {
      let sectionsVideoPlayed = await getValue(
        `${this.state.contact.contactid}/sectionsVideoPlayed`
      );
      let tempArray = arrayNullOrEmpty(sectionsVideoPlayed)
        ? []
        : sectionsVideoPlayed[0].json
        ? sectionsVideoPlayed[0].json
        : [];

      tempArray.push(value);

      await postValue(
        `${this.state.contact.contactid}/${'sectionsVideoPlayed'}`,
        tempArray,
        null
      );
      this.setState({ sectionsVideoPlayedArray: tempArray });
    }
  };

  checkIfVideoPlayed = async section => {
    let found = false;
    let tempArray = [];
    if (this.state.contact) {
      let sectionsVideoPlayed = await getValue(
        `${this.state.contact.contactid}/sectionsVideoPlayed`
      );
      sectionsVideoPlayed[0] && (tempArray = sectionsVideoPlayed[0].json);
    }

    if (arrayNullOrEmpty(tempArray)) {
      //PlayVideo
      //this.setSectionsVideoPlayedObject(section);
    } else {
      found = tempArray.some(element => {
        return element === section;
      });
    }
    if (!found) {
      //this.setSectionsVideoPlayedObject(section);
    }

    return found;
  };

  deleteFinancialForms = async formsToBeDeletedArray => {
    this.showLoadingOverLay(
      true,
      <FormattedMessage
        id="deleting_financial_forms"
        defaultMessage="Deleting forms..."
      />
    );
    if (!arrayNullOrEmpty(formsToBeDeletedArray)) {
      let promises = formsToBeDeletedArray.map(async (o, i) => {
        await deleteSeniorApplicationsFinancial(
          o,
          this.state.seniorAppDetails[0].seappdetailid
        );
      });
      await Promise.all(promises);
    }

    this.setState({
      displayWarningPopUp: false,
      displayYearsPopUp: false,
    });

    await this.getSeniorFinancials(this.state.seniorAppDetails);

    this.showLoadingOverLay(false);
  };

  handleDeleteFinancialForms = async () => {
    this.showLoadingOverLay(
      true,
      <FormattedMessage
        id="deleting_financial_forms"
        defaultMessage="Deleting forms..."
      />
    );
    let currentSelectedYear = cloneDeep(this.state.newSelectedYear);
    this.selectYearOptions(currentSelectedYear);

    let formsToBeDeleted = [];
    let idsToBeDeleted = [];
    let detail = [];

    detail = this.state.seniorAppDetails.filter(item => {
      return item.seapplicationid === this.state.seniorApp.seapplicationid;
    });
    if (!arrayNullOrEmpty(detail)) {
      detail = detail[0];
      formsToBeDeleted = this.state.financials[detail.seappdetailid];
      formsToBeDeleted.map(item => {
        idsToBeDeleted.push(item.sefinancialformsid);
      });
      this.setState({ displayWarningPopUp: false });
      await this.deleteFinancialForms(idsToBeDeleted);
    }

    if (this.state.filesMetadata) {
      let promises = this.state.filesMetadata.map(async item => {
        const imgId = `${this.state.seniorApp.seapplicationid}${
          this.state.seniorAppDetails[0].seappdetailid
            ? '.' + this.state.seniorAppDetails[0].seappdetailid
            : ''
        }.${item.portalSection}.${item.portalDocument}.${replaceDotBySymbol(
          item.originalFilename.substring(2, item.originalFilename.length - 2),
          '_'
        )}`;

        if (item.portalSection.includes('Financial Info')) {
          await deleteBlob(imgId);

          await createOrUpdateFileMetadataEntities(
            this.state.filesMetadata,
            item.portalSection,
            item.portalDocument,
            this.state.seniorApp.seapplicationid,
            this.state.seniorAppDetails[0].seappdetailid,
            item.accountNumber,
            [],
            'form2020Files',
            this.getFilesMetadata
          );
        }
      });
      await Promise.all(promises);
    }

    this.showLoadingOverLay(false);

    if (this.state.filesMetadata) {
      let promises = this.state.filesMetadata.map(async item => {
        const imgId = `${this.state.seniorApp.seapplicationid}${
          this.state.seniorAppDetails[0].seappdetailid
            ? '.' + this.state.seniorAppDetails[0].seappdetailid
            : ''
        }.${item.portalSection}.${item.portalDocument}.${replaceDotBySymbol(
          item.originalFilename.substring(2, item.originalFilename.length - 2),
          '_'
        )}`;

        if (item.portalSection.includes('Financial Info')) {
          await deleteBlob(imgId);

          await createOrUpdateFileMetadataEntities(
            this.state.filesMetadata,
            item.portalSection,
            item.portalDocument,
            this.state.seniorApp.seapplicationid,
            this.state.seniorAppDetails[0].seappdetailid,
            item.accountNumber,
            [],
            'form2020Files',
            this.getFilesMetadata
          );
        }
      });
      await Promise.all(promises);
    }

    this.showLoadingOverLay(false);
    if (this.state.filesMetadata) {
      let promises = this.state.filesMetadata.map(async item => {
        const imgId = `${this.state.seniorApp.seapplicationid}${
          this.state.seniorAppDetails[0].seappdetailid
            ? '.' + this.state.seniorAppDetails[0].seappdetailid
            : ''
        }.${item.portalSection}.${item.portalDocument}.${replaceDotBySymbol(
          item.originalFilename.substring(2, item.originalFilename.length - 2),
          '_'
        )}`;

        if (item.portalSection.includes('Financial Info')) {
          await deleteBlob(imgId);

          await createOrUpdateFileMetadataEntities(
            this.state.filesMetadata,
            item.portalSection,
            item.portalDocument,
            this.state.seniorApp.seapplicationid,
            this.state.seniorAppDetails[0].seappdetailid,
            item.accountNumber,
            [],
            'form2020Files',
            this.getFilesMetadata
          );
        }
      });
      await Promise.all(promises);
    }

    this.showLoadingOverLay(false);
  };

  checkIfAlreadyExistFinancialForms(selectedYear, currentSelectedYear) {
    if (selectedYear !== currentSelectedYear) {
      if (!arrayNullOrEmpty(this.state.seniorAppDetails)) {
        if (this.state.financials) {
          let detail = [];
          detail = this.state.seniorAppDetails.filter(item => {
            return (
              item.seapplicationid === this.state.seniorApp.seapplicationid
            );
          });
          if (!arrayNullOrEmpty(detail)) {
            detail = detail[0];
            if (
              !arrayNullOrEmpty(this.state.financials[detail.seappdetailid])
            ) {
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  handleYearsPopUpDisplay = showPopUp => {
    this.setState({ displayYearsPopUp: showPopUp });
  };

  handleYearsPopUpClose = () => {
    this.setState({ displayYearsPopUp: false });
  };

  handleWarningPopUpClose = () => {
    this.setState({ displayWarningPopUp: false });
  };

  selectYearOptions = async currentSelected => {
    let currentYearsArrayOptions = cloneDeep(this.state.globalYearOptionsArray);
    let currentSelectedYear = currentSelected;
    let previousSelectedYear = {};

    let objectOnAuth = cloneDeep(this.state.yearsToApplyArray);

    objectOnAuth.map(item => {
      if (item.name == currentSelectedYear && !item.applied) {
        item.seniorAppId = this.state.seniorApp.seapplicationid;
      } else {
        if (item.seniorAppId === this.state.seniorApp.seapplicationid) {
          item.seniorAppId = null;
        }
      }
    });

    let jsonSavedArray = await this.createOrUpdateAuthYearsObject(objectOnAuth);
    currentYearsArrayOptions = this.returnToRadioButtomSource(jsonSavedArray);
    // max income msg on select year
    const msg = this.getMaxIncomeMsg(currentSelectedYear);
    if (this.state.displayYearsPopUp) {
      this.setState({
        selectedYear: currentSelectedYear,
        globalYearOptionsArray: currentYearsArrayOptions,
        yearsToApplyArray: objectOnAuth,
        maxIncomeDialogMsg: {
          [currentSelectedYear]: msg,
        },
      });
    } else {
      this.setState({
        selectedYear: currentSelectedYear,
        globalYearOptionsArray: currentYearsArrayOptions,
        yearsToApplyArray: objectOnAuth,
        openMaxIncomeDialog: true,
        maxIncomeDialogMsg: {
          [currentSelectedYear]: msg,
        },
      });
    }
  };

  returnToRadioButtomSource = jsonSavedArray => {
    //this.setState({ recommendedYear: this.props.recommendedYear });
    return generateRadioButtomSource(
      jsonSavedArray,
      this.state.seniorApp.seapplicationid
    );
  };

  handleYearsPopUpOptions = async event => {
    let currentSelectedYear = event.target.value;
    if (
      !this.checkIfAlreadyExistFinancialForms(
        this.state.selectedYear,
        currentSelectedYear
      )
    ) {
      this.selectYearOptions(currentSelectedYear);
    } else {
      const msg = this.getMaxIncomeMsg(currentSelectedYear);
      if (this.state.displayYearsPopUp) {
        this.setState({
          displayWarningPopUp: true,
          newSelectedYear: currentSelectedYear,
          maxIncomeDialogMsg: {
            [currentSelectedYear]: msg,
          },
        });
      } else {
        this.setState({
          displayWarningPopUp: true,
          newSelectedYear: currentSelectedYear,
          openMaxIncomeDialog: true,
          maxIncomeDialogMsg: {
            [currentSelectedYear]: msg,
          },
        });
      }
    }
  };

  setOptionsArray = yearOptionsArray => {
    this.setState({
      globalYearOptionsArray: yearOptionsArray,
    });
  };

  createOrUpdateSeniorDetails = async objectAuth => {
    let applicationsPending = [];
    applicationsPending = objectAuth.filter(item => {
      return item.seniorAppId === null;
    });

    if (applicationsPending.length > 0) {
      if (!arrayNullOrEmpty(objectAuth)) {
        if (!arrayNullOrEmpty(this.state.seniorAppDetails)) {
          let newDetail = [];
          newDetail = this.state.seniorAppDetails.filter(item => {
            return (
              item.seapplicationid === this.state.seniorApp.seapplicationid
            );
          });
          if (!arrayNullOrEmpty(newDetail)) {
            newDetail = newDetail[0];
            let year = objectAuth.filter(item => {
              return item.seniorAppId === this.state.seniorApp.seapplicationid;
            })[0];
            if (year) {
              newDetail.yearid = year.yearId;
              if (this.state.seniorApp.statuscode === 668020012) {
                await dataServiceProvider.updateSeniorAppDetail(newDetail);
              }
              await this.getSeniorDetails(this.state.seniorApp.seapplicationid);
            } else {
              let newYear = objectAuth.find(k => k.seniorAppId === null);
              if (newYear) {
                newDetail.yearid = newYear.yearId;
                await dataServiceProvider.updateSeniorAppDetail(newDetail);
                await this.getSeniorDetails(
                  this.state.seniorApp.seapplicationid
                );
                this.selectYearOptions(newYear.name);
              }
            }
          }
        }
      }
    }
  };

  compareCreateOrUpdateAuthYearsObject = (
    newCalculatedObject,
    yearsToApplyArrayObject,
    seniorAppId,
    fromPropertyInfo
  ) => {
    let objectOnAuth = yearsToApplyArrayObject;
    let toPreserveAuthObjects = [];
    let newArrayToBeSaved = cloneDeep(newCalculatedObject);
    if (newCalculatedObject && !deepEqual(newCalculatedObject, objectOnAuth)) {
      toPreserveAuthObjects = objectOnAuth.filter(item => {
        return item.seniorAppId !== null;
      });

      newArrayToBeSaved = [];

      if (!arrayNullOrEmpty(toPreserveAuthObjects)) {
        toPreserveAuthObjects.map(tp => {
          if (
            tp.seniorAppId === seniorAppId &&
            !newCalculatedObject.some(i => i.yearId === tp.yearId)
          ) {
            tp.valid = false;
            this.handleYearsPopUpDisplay(true);
          } else {
            tp.valid = true;
          }
          newArrayToBeSaved.push(tp);
        });

        newCalculatedObject = newCalculatedObject.filter(
          item => !toPreserveAuthObjects.some(i => i.yearId === item.yearId)
        );
      }
      newCalculatedObject.map(nc => {
        newArrayToBeSaved.push(nc);
      });

      //if still there are not a current senior application, it is set.
      if (!newArrayToBeSaved.some(item => item.seniorAppId === seniorAppId)) {
        for (let i = newArrayToBeSaved.length - 1; i >= 0; i--) {
          if (newArrayToBeSaved[i].seniorAppId === null) {
            newArrayToBeSaved[i].seniorAppId = seniorAppId;
            break;
          }
        }
      }
    } else if (
      newArrayToBeSaved &&
      newArrayToBeSaved.length > 0 &&
      !newArrayToBeSaved.some(obj => obj.seniorAppId !== null)
    ) {
      newArrayToBeSaved[0].seniorAppId = seniorAppId;
    }

    newArrayToBeSaved =
      newArrayToBeSaved && newArrayToBeSaved.length ? newArrayToBeSaved : [];

    newArrayToBeSaved.sort((a, b) =>
      a.name > b.name ? -1 : b.name > a.name ? 1 : 0
    );

    return newArrayToBeSaved;
  };

  calculateAuthYearsObject = newCalculatedObject => {
    let newArrayToBeSaved = [];
    newArrayToBeSaved = this.compareCreateOrUpdateAuthYearsObject(
      newCalculatedObject,
      this.state.yearsToApplyArray,
      this.state.seniorApp.seapplicationid
    );
    this.setRecommendedYear(newArrayToBeSaved);
    this.createOrUpdateAuthYearsObject(newArrayToBeSaved);
    return newArrayToBeSaved;
  };

  createOrUpdateAuthYearsObject = newArrayToBeSaved => {
    this.saveYearsObject(newArrayToBeSaved);
    this.setSelectedYear(newArrayToBeSaved);
    this.createOrUpdateSeniorDetails(newArrayToBeSaved);
    return newArrayToBeSaved;
  };

  setSelectedYear = arrayToBeSaved => {
    let selectedYear = '';
    if (!arrayNullOrEmpty(arrayToBeSaved)) {
      selectedYear = arrayToBeSaved.filter(
        item =>
          item.seniorAppId === this.state.seniorApp.seapplicationid &&
          item.applied === false
        //&& item.valid === false
      )[0];
      if (selectedYear) {
        this.setState({ selectedYear: selectedYear.name });
      }
    }
  };

  returnSelectedYear = arrayToBeSaved => {
    return arrayToBeSaved.filter(
      item =>
        item.seniorAppId === this.state.seniorApp.seapplicationid &&
        item.applied === false
    )[0];
  };

  setRecommendedYear = arrayToBeSaved => {
    let recommended = '';
    let earlierYear = '';
    let validYears = [];

    if (!arrayNullOrEmpty(arrayToBeSaved)) {
      validYears = arrayToBeSaved.filter(item => !item.applied && item.valid);
    }

    if (!arrayNullOrEmpty(validYears)) {
      recommended = validYears[0].name;
      earlierYear = validYears[validYears.length - 1].name;
    } else {
      recommended = 'none';
      earlierYear = 'none';
    }
    this.setState({
      recommendedYear: recommended.toString(),
      earlierYear: earlierYear.toString(),
    });
  };

  calculateFormsAmounts = financialForms => {
    let totalAmounts = 0,
      totalExpenses = 0;

    let formPropertiesToBeExcluded = {
      financialformtype: 'financialformtype',
      filertype: 'filertype',
      yearid: 'yearid',
      incomesocialsec: 'incomesocialsec',
      incomedividends: 'incomedividends',
      incomeretirement: 'incomeretirement',
      incomeirataxamt: 'incomeirataxamt',
      incomewages: 'incomewages',
      incomecapgains: 'incomecapgains',
      incomebusiness: 'incomebusiness',
      incomeother: 'incomeother',
      incomerental: 'incomerental',
      incomegifts: 'incomegifts',
      incomecountries: 'incomecountries',
      incomedshs: 'incomedshs',
      incomeunemployment: 'incomeunemployment',
      incomedisabilitysrc: 'incomedisabilitysrc',
      incomeservicedisability: 'incomeservicedisability',
      incomegambling: 'incomegambling',
      trustpartnershipestateincome: 'trustpartnershipestateincome',
      taxnontaxbonds: 'taxnontaxbonds',
      incomealimony: 'incomealimony',
      seappdetailid: 'seappdetailid',
      controller: 'controller',
      sefinancialformsid: 'sefinancialformsid',
      medicareplanid: 'medicareplanid',
      occupantid: 'occupantid',
    };

    let expensesPropertiesToBeExcluded = {
      sefinancialformsid: 'sefinancialformsid',
      financialformtype: 'financialformtype',
      filertype: 'filertype',
      yearid: 'yearid',
      seappdetailid: 'seappdetailid',
      medicareplanid: 'medicareplanid',
      controller: 'controller',
      incomedisabilitysrc: 'incomedisabilitysrc',
      occupantid: 'occupantid',
      incomeservicedisability: 'incomeservicedisability',
      incomedshs: 'incomedshs',
      expenseassistedliving: 'expenseassistedliving',
      expensesinhome: 'expensesinhome',
      expensepres: 'expensepres',
      expensemedicareplan: 'expensemedicareplan',
    };

    if (financialForms) {
      if (!arrayNullOrEmpty(financialForms)) {
        financialForms.map(item => {
          if (item.financialformtype !== 668020011) {
            totalExpenses =
              totalExpenses +
              sumOnFinancialForms(item, formPropertiesToBeExcluded);
            totalAmounts =
              totalAmounts +
              sumOnFinancialForms(item, expensesPropertiesToBeExcluded);
          }
        });
        return totalAmounts - totalExpenses;
      } else {
        return null;
      }
    }
  };

  // message on select year
  getMaxIncomeMsg = year => {
    try {
      const parsedYear = parseInt(year);
      const amount = year === '2018' || year === '2019' ? '40,000' : '58,423';
      return (
        <FormattedMessage
          id="myInfo_maxIncomeMsg"
          defaultMessage="Maximum income of ${amount} for {year}"
          description="myInfo_maxIncomeMsg"
          values={{
            amount,
            year: parsedYear - 1,
          }}
        />
      );
    } catch (error) {
      return null;
    }
  };

  handleCloseMaxIncomeDialog = () => {
    this.setState({
      openMaxIncomeDialog: false,
    });
  };

  render = () => {
    console.log('AuthContext', this.state);
    return (
      <AuthContext.Provider
        value={{
          qrToken: this.state.qrToken,
          showLoading: this.state.showLoading,
          loadingFormatter: this.state.loadingFormatter,
          showLoadingOverLay: this.showLoadingOverLay,
          readOnlyMode: this.state.readOnlyMode,
          redirectToSeniors: this.state.redirectToSeniors,
          isLoggedIn: this.isLoggedIn,
          login: this.login,
          logout: this.logout,
          currentUser: this.currentUser,
          user: this.state.user,
          contact: this.state.contact,
          updateContact: this.updateContact,
          setSeniorApp: this.setSeniorApp,
          seniorApp: this.state.seniorApp,
          seniorApps: this.state.seniorApps,
          updateSeniorApp: this.updateSeniorApp,
          filesMetadata: this.state.filesMetadata,
          getFilesMetadata: this.getFilesMetadata,
          createSeniorApp: this.createSeniorApp,
          getSeniorApp: this.getSeniorApp,
          updateFilesMetadataState: this.updateFilesMetadataState,
          otherProperties: this.state.otherProperties,
          occupants: this.state.occupants,
          getOtherProperties: this.getOtherProperties,
          getOccupants: this.getOccupants,
          seniorAppDetails: this.state.seniorAppDetails,
          seniorAppsDetails: this.state.seniorAppsDetails,
          getSeniorDetails: this.getSeniorDetails,
          financials: this.state.financials,
          sectionsVideoPlayedArray: this.state.sectionsVideoPlayedArray,
          getSeniorApps: this.getSeniorApps,
          setSectionsVideoPlayedObject: this.setSectionsVideoPlayedObject,
          getSeniorFinancials: this.getSeniorFinancials,
          checkIfVideoPlayed: this.checkIfVideoPlayed,
          submitSeniorApp: this.submitSeniorApp,
          showSubmitDoneConfirmation: this.state.showSubmitDoneConfirmation,
          showSubmitFailedConfirmation: this.state.showSubmitFailedConfirmation,
          onShowSubmitDoneConfirmation: this.onShowSubmitDoneConfirmation,
          onShowSubmitFailedConfirmation: this.onShowSubmitFailedConfirmation,
          htmlSummary: this.state.htmlSummary,
          setHtmlSummary: this.setHtmlSummary,
          toggleRedirectToSeniors: this.toggleRedirectToSeniors,
          getYearsObject: this.getYearsObject,
          yearsToApplyObject: this.state.yearsToApplyArray,
          handleYearsPopUpOptions: this.handleYearsPopUpOptions,
          handleYearsPopUpClose: this.handleYearsPopUpClose,
          handleWarningPopUpClose: this.handleWarningPopUpClose,
          displayYearsPopUp: this.state.displayYearsPopUp,
          handleYearsPopUpDisplay: this.handleYearsPopUpDisplay,
          globalYearOptionsArray: this.state.globalYearOptionsArray,
          setOptionsArray: this.setOptionsArray,
          createOrUpdateSeniorDetails: this.createOrUpdateSeniorDetails,
          createOrUpdateAuthYearsObject: this.createOrUpdateAuthYearsObject,
          selectedYear: this.state.selectedYear,
          recommendedYear: this.state.recommendedYear,
          earlierYear: this.state.earlierYear,
          displayWarningPopUp: this.state.displayWarningPopUp,
          handleDeleteFinancialForms: this.handleDeleteFinancialForms,
          returnToRadioButtomSource: this.returnToRadioButtomSource,
          calculateAuthYearsObject: this.calculateAuthYearsObject,
          calculateFormsAmounts: this.calculateFormsAmounts,
          checkClientBrowser: this.checkClientBrowser,
          openMaxIncomeDialog: this.state.openMaxIncomeDialog,
          handleCloseMaxIncomeDialog: this.handleCloseMaxIncomeDialog,
          maxIncomeDialogMsg: this.state.maxIncomeDialogMsg,
        }}
      >
        {this.props.children}
      </AuthContext.Provider>
    );
  };
}

const AuthConsumer = AuthContext.Consumer;

export { AuthProvider, AuthConsumer, AuthContext };
