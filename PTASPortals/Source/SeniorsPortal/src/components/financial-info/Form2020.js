//-----------------------------------------------------------------------
// <copyright file="Form2020.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React, { key, object, handleBack, readOnlyMode } from 'react';
import './FinancialInfo.css';
import * as fm from './FormatTexts';
import Grid from '@material-ui/core/Grid';
import { FormattedMessage } from 'react-intl';
import SelectInput from '../common/SelectInput';
import CustomButton from '../common/CustomButton';
import CurrencyInput from '../common/CurrencyInput';
import UploadFile from '../common/UploadFile/UploadFile';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Expenses from './Expenses';
import {
  arrayNullOrEmpty,
  convertToMoneyFormat,
  convertToNumberFloatFormat,
  renderIf,
  sumOnFinancialForms,
} from '../../lib/helpers/util';

import { cloneDeep } from 'lodash';
import { getUserType } from './financialHelper';

import { createOrUpdateFileMetadataEntities } from '../common/UploadFile/uploaderHelper';

import deepEqual from 'deep-equal';

import uuid from 'uuid';
import CardSide from '../common/CardSide';
import TotalIncome from '../common/TotalIncome';
import VimeoSlider from '../common/vimeo-player/VimeoSlider';

import { Hidden } from '@material-ui/core';

import { createFileArraysFromMetadata } from '../common/UploadFile/uploaderHelper';
import { getAppInsightsInstance } from '../../services/telemetryService';
import {
  updateUploadsData,
  setUploadsData,
} from '../common/UploadFile/signalRHelper';
import { getFileArraysFromFileMetadataList } from '../../lib/data-mappings/fileMetadataToFileArray';
import MissingRequirementsContent from '../common/MissingRequirementsContent';
import MissingRequirementsPopUp from '../common/MissingRequirementsPopUp';
import SwitchInput from '../common/SwitchInput';
import { withStyles } from '@material-ui/styles';
let totalHouseIncome = null;

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

export default class Form2020 extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      documents: [
        { document: 'Financial Forms', arrayName: 'form2020Files' },
        {
          document: 'Expenses',
          arrayName: 'expensesFiles',
        },
      ],
      section: 'Financial Info',
      year: 0,
      f2020_NetSocialSecurity: '',
      f2020_InterestDividends: '',
      f2020_RetirementPensionAnnuity: '',
      f2020_IraTaxableAmount: '',
      f2020_Wages: '',
      f2020_CapitaGains: '',
      f2020_BusinessIncome: '',
      f2020_OtherIncome: '',
      f2020_RentalIncome: '',
      f2020_GiftsFromFamily: '',
      f2020_IncomeOtherCountries: '',
      f2020_DshsPublicAssistance: '',
      f2020_Unemployment: '',
      f2020_DisabilityIncome: '',
      f2020_DisabilitySource: '',
      f2020_ServiceRelatedDisabilityIncome: null,
      //new fields.
      f2020_TrustOrRoyaltyIncome: null,
      f2020_TaxableAndNonTaxableBonds: null,
      f2020_AlimonyIncome: null,
      f2020_GamblingIncome: null,
      f2020_ReqFedFileInTax: false,
      expenses: {
        assistedLiving: null,
        inHomeCare: null,
        nonReimbursedPrescriptions: null,
        approvedMedicare: null,
        medicareProvider: '',
        medicalPlan: '',
      },
      form2020Files: [],
      expensesFiles: [],
      curr2020Data: [],
      currExpensesData: [],
      notifying: false,
      requiredFields: ['form2020Files'],
      requirePopUp: false,
      requireMessage: false,
    };

    this.financialFormUploadRef = React.createRef();
  }

  componentDidMount = async () => {
    this.setFinancialForms();

    if (!arrayNullOrEmpty(this.props.user.forms)) {
      totalHouseIncome = this.calculateHouseIncome();
    }

    await this.sendNotification(
      getFileArraysFromFileMetadataList(
        this.props.filesMetadata,
        'portalSection',
        this.state.section
      )
    );

    await this.setFileArrays();
  };

  sendNotification = async arrayContainer => {
    if (this.props.contact && this.props.seniorApp && arrayContainer) {
      let documents = cloneDeep(this.state.documents);

      documents.map(d => {
        d.isVisible = true;

        d.files = arrayContainer[d.arrayName]
          ? cloneDeep(arrayContainer[d.arrayName])
          : [];
      });

      const uploadsData = await updateUploadsData(
        this.props.contact,
        this.props.seniorApp.seapplicationid,
        this.props.detail.detailId,
        this.state.section,
        documents,
        this.props.uploadsData,
        this.props.sendMessage,
        this.props.setUpload
      );

      this.props.setUpload(uploadsData);
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

  componentDidUpdate = async (prevProps, prevState) => {
    // if (
    //   arrayNullOrEmpty(this.state.form2020Files) &&
    //   !arrayNullOrEmpty(this.props.filesMetadata) &&
    //   !deepEqual(prevProps.filesMetadata, this.props.filesMetadata)
    // ) {
    //   await this.setFileArrays();
    // }

    if (!deepEqual(this.props.user, prevProps.user)) {
      //this.setFileArrays();
      this.setFinancialForms();
      totalHouseIncome = this.calculateHouseIncome();
    }

    if (!deepEqual(this.props.user.forms, prevProps.user.forms)) {
      //this.setFileArrays();
      this.setFinancialForms();
      totalHouseIncome = this.calculateHouseIncome();
    }

    if (!deepEqual(prevState.form2020Files, this.state.form2020Files)) {
      if (this.state.form2020Files.length > 0) {
        const requiredFields = this.state.requiredFields.filter(
          f => f !== 'form2020Files'
        );

        this.setState({
          requiredFields,
          requirePopUp: false,
          requireMessage: false,
        });
      } else {
        this.setState({ requiredFields: ['form2020Files'] });
      }
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

    //END SIGNALR
  };

  calculateHouseIncome = () => {
    let totalAmounts = 0,
      totalExpenses = 0;

    let formPropertiesToBeExcluded = {
      documents: 'documents',
      uploadDocuments: 'uploadDocuments',
      section: 'section',
      year: 'year',
      expenses: 'expenses',
      form2020Files: 'form2020Files',
      f2020_ServiceRelatedDisabilityIncome:
        'f2020_ServiceRelatedDisabilityIncome',
      f2020_DshsPublicAssistance: 'f2020_DshsPublicAssistance',
      f2020_DisabilitySource: 'f2020_DisabilitySource',
    };

    let expensesPropertiesToBeExcluded = {
      medicalPlan: 'medicalPlan',
      medicareProvider: 'medicareProvider',
    };

    if (this.state) {
      totalExpenses =
        totalExpenses +
        sumOnFinancialForms(
          this.state.expenses,
          expensesPropertiesToBeExcluded
        );
      totalAmounts =
        totalAmounts +
        sumOnFinancialForms(this.state, formPropertiesToBeExcluded);
      return totalAmounts - totalExpenses;
    } else {
      return 0;
    }
  };

  setFileArrays = async () => {
    if (!arrayNullOrEmpty(this.props.filesMetadata)) {
      const arrays = await createFileArraysFromMetadata(
        false,
        this.props.filesMetadata,
        this.props.seniorApp.seapplicationid,
        this.props.detail.detailId,
        this.state.section,
        this.state.documents
      );
      this.setState(arrays.rootFileArrays);
    }
  };

  handleMetadataUpdate = async (array, documentObject, notify = true) => {
    if (notify && !this.state.notifying) {
      this.setState({ notifying: true }, async () => {
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
        this.setState({ notifying: false });
      });
    }
    const state = {};
    state[documentObject.arrayName] = array;
    this.setState(state);
    await createOrUpdateFileMetadataEntities(
      this.props.filesMetadata,
      this.state.section,
      documentObject.document,
      this.props.seniorApp.seapplicationid,
      this.props.detail.detailId,
      this.props.seniorApp.accountnumber,
      array,
      documentObject.arrayName,
      this.props.getFilesMetadata
    );
  };

  setParentState = (key, value) => {
    this.setState({ [key]: value });
  };

  setBaseState = value => {
    this.setState(value);
  };

  setFinancialForms = () => {
    let expenses = {};

    if (this.props.user) {
      let form2020 = this.props.user.forms.filter(
        f =>
          f.financialformtype === 668020010 &&
          f.yearid === this.props.detail.yearid
      );

      if (!arrayNullOrEmpty(form2020)) {
        form2020 = form2020[0];

        this.setState({
          f2020_NetSocialSecurity: form2020['incomesocialsec'],
          f2020_InterestDividends: form2020['incomedividends'],
          f2020_RetirementPensionAnnuity: form2020['incomeretirement'],
          f2020_IraTaxableAmount: form2020['incomeirataxamt'],
          f2020_Wages: form2020['incomewages'],
          f2020_CapitaGains: form2020['incomecapgains'],
          f2020_BusinessIncome: form2020['incomebusiness'],
          f2020_OtherIncome: form2020['incomeother'],
          f2020_RentalIncome: form2020['incomerental'],
          f2020_GiftsFromFamily: form2020['incomegifts'],
          f2020_IncomeOtherCountries: form2020['incomecountries'],
          f2020_DshsPublicAssistance: form2020['incomedshs'],
          f2020_Unemployment: form2020['incomeunemployment'],
          f2020_DisabilityIncome: form2020['incomedisability'],
          f2020_DisabilitySource: form2020['incomedisabilitysrc'],
          f2020_ServiceRelatedDisabilityIncome:
            form2020['incomeservicedisability'],
          f2020_GamblingIncome: form2020['incomegambling'],
          //new fields.
          f2020_TrustOrRoyaltyIncome: form2020['trustpartnershipestateincome'],
          f2020_TaxableAndNonTaxableBonds: form2020['taxnontaxbonds'],
          f2020_AlimonyIncome: form2020['incomealimony'],
          f2020_ReqFedFileInTax:
            form2020['requiredtofilefederalincometaxreturn'],
        });

        //expenses fields
        expenses.assistedLiving = form2020['expenseassistedliving'];
        expenses.inHomeCare = form2020['expensesinhome'];
        expenses.nonReimbursedPrescriptions = form2020['expensepres'];
        expenses.approvedMedicare = form2020['expensemedicareplan'];
        expenses.medicalPlan = form2020['medicareplanid']
          ? form2020['medicareplanid']
          : '';

        if (expenses.medicalPlan) {
          let currentPlan = this.props.medicarePlans.filter(
            m => m.id === expenses.medicalPlan
          )[0];

          if (currentPlan) {
            expenses.medicareProvider = currentPlan.organizationName;
          }
        }
      } else {
        this.setState({
          f2020_NetSocialSecurity: '',
          f2020_InterestDividends: '',
          f2020_RetirementPensionAnnuity: '',
          f2020_IraTaxableAmount: '',
          f2020_Wages: '',
          f2020_CapitaGains: '',
          f2020_BusinessIncome: '',
          f2020_OtherIncome: '',
          f2020_RentalIncome: '',
          f2020_GiftsFromFamily: '',
          f2020_IncomeOtherCountries: '',
          f2020_DshsPublicAssistance: '',
          f2020_Unemployment: '',
          f2020_DisabilityIncome: '',
          f2020_DisabilitySource: '',
          f2020_ServiceRelatedDisabilityIncome: '',
          f2020_GamblingIncome: '',
          //new fields.
          f2020_TrustOrRoyaltyIncome: '',
          f2020_TaxableAndNonTaxableBonds: '',
          f2020_AlimonyIncome: '',
          f2020_ReqFedFileInTax: false,
          form2020Files: [],
          expensesFiles: [],
        });

        //expenses fields
        expenses.assistedLiving = '';
        expenses.inHomeCare = '';
        expenses.nonReimbursedPrescriptions = '';
        expenses.approvedMedicare = '';
        expenses.medicalPlan = '';
      }
    }

    this.setState({ expenses });
  };

  save2020 = () => {
    let form2020 = this.props.user.forms.filter(
      f =>
        f.financialformtype === 668020010 &&
        f.yearid === this.props.detail.yearid
    );

    form2020 = arrayNullOrEmpty(form2020) ? {} : form2020[0];
    form2020.financialformtype = 668020010;
    form2020.filertype = this.props.user.filterTypeValue;
    form2020.yearid = this.props.detail.yearid;

    form2020['incomesocialsec'] = this.state.f2020_NetSocialSecurity;
    form2020['incomedividends'] = this.state.f2020_InterestDividends;
    form2020['incomeretirement'] = this.state.f2020_RetirementPensionAnnuity;
    form2020['incomeirataxamt'] = this.state.f2020_IraTaxableAmount;
    form2020['incomewages'] = this.state.f2020_Wages;
    form2020['incomecapgains'] = this.state.f2020_CapitaGains;
    form2020['incomebusiness'] = this.state.f2020_BusinessIncome;
    form2020['incomeother'] = this.state.f2020_OtherIncome;
    form2020['incomerental'] = this.state.f2020_RentalIncome;
    form2020['incomegifts'] = this.state.f2020_GiftsFromFamily;
    form2020['incomecountries'] = this.state.f2020_IncomeOtherCountries;
    form2020['incomedshs'] = this.state.f2020_DshsPublicAssistance;
    form2020['incomeunemployment'] = this.state.f2020_Unemployment;
    form2020['incomedisability'] = this.state.f2020_DisabilityIncome;
    form2020['incomedisabilitysrc'] = this.state.f2020_DisabilitySource;
    form2020[
      'incomeservicedisability'
    ] = this.state.f2020_ServiceRelatedDisabilityIncome;
    form2020['incomegambling'] = this.state.f2020_GamblingIncome;
    //new fields.
    form2020[
      'trustpartnershipestateincome'
    ] = this.state.f2020_TrustOrRoyaltyIncome;
    form2020['taxnontaxbonds'] = this.state.f2020_TaxableAndNonTaxableBonds;
    form2020['incomealimony'] = this.state.f2020_AlimonyIncome;
    form2020[
      'requiredtofilefederalincometaxreturn'
    ] = this.state.f2020_ReqFedFileInTax;

    let expenses = this.props.onSaveExpenses(
      this.state.expenses,
      this.props.user
    );
    //expenses fields.
    form2020['expenseassistedliving'] = expenses.expenseassistedliving;
    form2020['expensesinhome'] = expenses.expensesinhome;
    form2020['expensepres'] = expenses.expensepres;
    form2020['expensemedicareplan'] = expenses.expensemedicareplan;
    form2020['medicareplanid'] = expenses.medicareplanid;

    return form2020;
  };

  saveFinancialForms = async () => {
    let form2020 = this.save2020();
    let promises = [this.props.createOrUpdateFinancialForm(form2020)];
    await Promise.all(promises);
  };

  onSavingStarted = index => {
    let saveControl = [...this.state.saveControl];
    saveControl.push({ index, saving: true });
    this.setState({ saveControl });
  };

  onSavingFinished = index => {
    let saveControl = this.state.saveControl.map(s =>
      s.index === index ? { index, saving: false } : s
    );

    let goBack = true;
    this.setState({ saveControl }, () => {
      this.state.saveControl.map(s => {
        if (!s.saving) {
          goBack = !s.saving;
        }
      });

      if (goBack) {
        this.props.handleBack();
      }
    });
  };

  canContinue = () => {
    console.log('CAN CONTINUE:', this.state.requiredFields);
    if (this.state.requiredFields && this.state.requiredFields.length > 0) {
      this.setState({ requirePopUp: true, requireMessage: true });
      return false;
    } else {
      this.setState({ requirePopUp: false, requireMessage: false });
      return true;
    }
  };

  handleContinueClick = async () => {
    if (this.canContinue()) {
      window.scrollTo(0, 0);
      this.props.showSavingOverlay();
      if (!this.props.readOnlyMode) {
        await this.saveFinancialForms();
        if (this.props.currentForm !== '2020') {
          await this.props.deleteFinancialForms(
            this.props.returnFormAndChildrenIdsToBeDeleted()
          );
        }
        await this.props.getSeniorFinancials(this.props.details);
        if (this.props.users.length > 1) {
          this.props.handleBack();
        }
      } else {
        if (this.props.users.length > 1) {
          this.props.handleBack();
        }
      }

      if (this.props.users.length === 1) {
        //call FinancialInfoYear continue
        await this.props.handleContinueClickFinancialInfoYear();
      }

      this.props.hideSavingOverlay();
    }
  };

  handleArrayUpdate = array => {
    if (array.length <= 0) return;
  };

  handleCurrencyInputChange = e => {
    let value = convertToNumberFloatFormat(e.target.value);
    this.setState({ [e.target.name]: value });
  };

  handleSelectChangeValue = e => {
    this.setState({ [e.target.name]: e.target.value });
  };

  handleExpensesCurrencyInputChange = e => {
    let value = convertToMoneyFormat(e.target.value);
    let expenses = { ...this.state.expenses };
    expenses[e.target.name] = value;
    this.setState({ expenses });
  };

  handleExpensesSelectChangeValue = e => {
    let expenses = { ...this.state.expenses };
    if (e.target.name === 'medicareProvider') {
      expenses['medicalPlan'] = null;
    }
    expenses[e.target.name] = e.target.value;
    this.setState({ expenses });
  };

  handleSwitchInputChange = (checked, e, id) => {
    this.setState({ [id]: checked });
  };

  render() {
    let userType = getUserType(this.props.user);
    let userFullName = this.props.user ? this.props.user.fullName : '';
    totalHouseIncome = this.calculateHouseIncome();
    return (
      <React.Fragment>
        <Card style={{ marginBottom: '20px' }} className="form-card">
          <div className="formContainerHeader">
            <div
              style={{
                font: 16,
                textAlign: 'left',
                marginLeft: '25px',
              }}
            >
              <Grid container spacing={20}>
                <Grid sm={2}>
                  <span
                    style={{
                      fontSize: '30px',
                      fontWeight: '600',
                    }}
                  >
                    {this.props.selectedYear}
                  </span>
                </Grid>
                <Grid sm={7}>
                  <p
                    style={{
                      marginTop: '7px',
                      marginBottom: '7px',
                      fontSize: '20px',
                    }}
                    className="mouseflow-hide"
                  >
                    <FormattedMessage
                      id="username"
                      defaultMessage="{username}"
                      values={{
                        username: this.props.userName,
                      }}
                    />
                  </p>
                </Grid>
              </Grid>
            </div>
          </div>
          <TotalIncome
            globalIncome={false}
            totalFormsIncome={totalHouseIncome}
          />
          <CardContent className="center-panel form-content-max">
            <Hidden xsDown style={{ marginButton: '40px' }}>
              <div>
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
                    <Grid sm={12}></Grid>
                    <Grid
                      sm={12}
                      className="forms-content-margin"
                      style={{ margin: '0 0 40px 0' }}
                    >
                      <strong
                        style={{
                          paddingBottom: 5,
                          textAlign: 'left',
                        }}
                      >
                        <FormattedMessage
                          id="finances_headerForm2020"
                          defaultMessage="Enter your income sources from {previousYear} (may be from your 1040, W-2, 1099, etc.)"
                          values={{
                            previousYear: this.props.selectedYear - 1,
                          }}
                        />
                      </strong>
                    </Grid>
                  </Grid>
                </div>
              </div>
            </Hidden>
            <div className="center-panel form-content-max">
              <Grid
                container
                spacing={5}
                style={{ marginTop: '30px' }}
                className="grid-container forms-content-margin"
              >
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                >
                  <CurrencyInput
                    testid={'netSocial'}
                    name="f2020_NetSocialSecurity"
                    label={fm.f2020_NetSocialSecurity}
                    information={fm.f2020_NetSocialSecurity_ht}
                    value={this.state.f2020_NetSocialSecurity}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                >
                  <CurrencyInput
                    testid={'interestDividends'}
                    name="f2020_InterestDividends"
                    label={fm.f2020_InterestDividends}
                    wrapLabel="true"
                    information={fm.f2020_InterestDividends_ht}
                    value={this.state.f2020_InterestDividends}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'retirementPension'}
                    name="f2020_RetirementPensionAnnuity"
                    label={fm.f2020_RetirementPensionAnnuity}
                    wrapLabel="true"
                    information={fm.f2020_RetirementPensionAnnuity_ht}
                    value={this.state.f2020_RetirementPensionAnnuity}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'iraTaxableAmount'}
                    name="f2020_IraTaxableAmount"
                    label={fm.f2020_IraTaxableAmount}
                    information={fm.f2020_IraTaxableAmount_ht}
                    value={this.state.f2020_IraTaxableAmount}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'wages'}
                    name="f2020_Wages"
                    label={fm.f2020_Wages}
                    information={fm.f2020_Wages_ht}
                    value={this.state.f2020_Wages}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'capitalGains'}
                    name="f2020_CapitaGains"
                    label={fm.f2020_CapitaGains}
                    information={fm.f2020_CapitaGains_ht}
                    value={this.state.f2020_CapitaGains}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'businessIncome'}
                    name="f2020_BusinessIncome"
                    label={fm.f2020_BusinessIncome}
                    information={fm.f2020_BusinessIncome_ht}
                    value={this.state.f2020_BusinessIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'otherIncome'}
                    name="f2020_OtherIncome"
                    label={fm.f2020_OtherIncome}
                    information={fm.f2020_OtherIncome1_ht}
                    value={this.state.f2020_OtherIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'rentalIncome'}
                    name="f2020_RentalIncome"
                    label={fm.f2020_RentalIncome}
                    information={fm.f2020_RentalIncome_ht}
                    value={this.state.f2020_RentalIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'gifts'}
                    name="f2020_GiftsFromFamily"
                    label={fm.f2020_GiftsFromFamily}
                    information={fm.f2020_GiftsFromFamily1_ht}
                    value={this.state.f2020_GiftsFromFamily}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'otherCountriesIncome'}
                    name="f2020_IncomeOtherCountries"
                    label={fm.f2020_IncomeOtherCountries}
                    information={fm.f2020_IncomeOtherCountries1_ht}
                    value={this.state.f2020_IncomeOtherCountries}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'unemployment'}
                    name="f2020_Unemployment"
                    label={fm.f2020_Unemployment}
                    information={fm.f2020_Unemployment_ht}
                    value={this.state.f2020_Unemployment}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'publicAssistance'}
                    name="f2020_DshsPublicAssistance"
                    label={fm.f2020_DshsPublicAssistance}
                    information={fm.f2020_DshsPublicAssistance1_ht}
                    value={this.state.f2020_DshsPublicAssistance}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'gamblingIncome'}
                    name="f2020_GamblingIncome"
                    label={fm.f2020_GamblingIncome}
                    information={fm.f2020_GamblingIncome_ht}
                    value={this.state.f2020_GamblingIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'disabilityIncome'}
                    name="f2020_DisabilityIncome"
                    label={fm.f2020_DisabilityIncome}
                    information={fm.f2020_DisabilityIncome_ht}
                    value={this.state.f2020_DisabilityIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'serviceRelatedDisabilityIncome'}
                    name="f2020_ServiceRelatedDisabilityIncome"
                    label={fm.f2020_ServiceRelatedDisabilityIncome}
                    information={fm.f2020_ServiceRelatedDisabilityIncome_ht}
                    value={this.state.f2020_ServiceRelatedDisabilityIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <SelectInput
                    name="f2020_DisabilitySource"
                    label={fm.f2020_DisabilitySource}
                    source={this.props.disabilitySources}
                    value={this.state.f2020_DisabilitySource}
                    onChange={this.handleSelectChangeValue}
                    readOnly={this.props.readOnlyMode}
                    style={{ marginTop: '0px' }}
                    sourceValue="attributeValue"
                    sourceLabel="defaultLocString"
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'trustRoyaltyIncome'}
                    name="f2020_TrustOrRoyaltyIncome"
                    label={fm.f2020_TrustOrRoyaltyIncome}
                    information={fm.f2020_TrustOrRoyaltyIncome_ht}
                    value={this.state.f2020_TrustOrRoyaltyIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                    allowNegative={true}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'bonds'}
                    name="f2020_TaxableAndNonTaxableBonds"
                    label={fm.f2020_TaxableAndNonTaxableBonds}
                    information={fm.f2020_TaxableAndNonTaxableBonds_ht}
                    value={this.state.f2020_TaxableAndNonTaxableBonds}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                >
                  <CurrencyInput
                    testid={'alimony'}
                    name="f2020_AlimonyIncome"
                    label={fm.f2020_AlimonyIncome}
                    information={fm.f2020_AlimonyIncome_ht}
                    value={this.state.f2020_AlimonyIncome}
                    onChange={this.handleCurrencyInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </Grid>
                <Grid
                  item
                  xs={12}
                  sm={6}
                  className="grid grid-vertical-spacing"
                  style={{ marginTop: '2px' }}
                ></Grid>
              </Grid>
              <div style={{ marginTop: '50px' }}>
                <CardSide
                  header={fm.attachingDocuments}
                  content={[
                    fm.confirmAmounts,
                    <VimeoSlider videos={[{ url: '382824186' }]} />,
                    fm.selectPhotoOfDocument,
                    fm.hideSocialSecurity,
                  ]}
                ></CardSide>
                <div className="req-file-income-tax">
                  <SwitchInputCSS
                    id="f2020_ReqFedFileInTax"
                    label={fm.form2020ReqFileIncomeTax}
                    checked={this.state.f2020_ReqFedFileInTax}
                    onChange={this.handleSwitchInputChange}
                    readOnly={this.props.readOnlyMode}
                  />
                </div>
                <div className="forms-content-margin-uploads">
                  <p className="forms-uploaders-title">
                    {fm.IncomeDocs(this.props.detail.yearValue)}
                  </p>
                  <p>{fm.IncludeAllPages}</p>
                  <MissingRequirementsPopUp
                    missingFields={this.state.requiredFields}
                    refsFields={{
                      form2020Files: this.financialFormUploadRef?.current,
                    }}
                    formattedStrings={fm}
                    show={this.state.requirePopUp}
                    onClose={() => this.setState({ requirePopUp: false })}
                    onMessageClick={() => this.setState({ requirePopUp: true })}
                  />
                  <UploadFile
                    ref={this.financialFormUploadRef}
                    detailsId={this.props.detail.detailId}
                    id={'form2020UploadFile'}
                    rightMessage={this.rightMessage}
                    obscureInfoMessage={false}
                    currentData={this.state.curr2020Data}
                    currDataName={'curr2020Data'}
                    setParentState={this.setParentState}
                    fileArray={this.state.form2020Files}
                    appId={
                      this.props.seniorApp
                        ? this.props.seniorApp.seapplicationid
                        : ''
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
                    hideIcon
                  />
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
        <Expenses
          uploadFileId={'expenses1099'}
          helpText={fm.helpText}
          user={this.props.user}
          key={this.state.expenses.key}
          object={this.state.expenses}
          handleArrayUpdate={this.handleArrayUpdate}
          handleCurrencyInputChange={this.handleExpensesCurrencyInputChange}
          handleSelectChangeValue={this.handleExpensesSelectChangeValue}
          readOnlyMode={this.props.readOnlyMode}
          medicareOrganizations={this.props.medicareOrganizations}
          medicarePlans={this.props.medicarePlans}
          detail={this.props.detail}
          seniorApp={this.props.seniorApp}
          document={this.state.documents[1].document}
          section={this.state.section}
          fileArray={this.state.expensesFiles}
          currentData={this.state.currExpensesData}
          currDataName={'currExpensesData'}
          setParentState={this.setParentState}
          onCreate={array =>
            this.handleMetadataUpdate(array, this.state.documents[1])
          }
          onDelete={array =>
            this.handleMetadataUpdate(array, this.state.documents[1])
          }
        />
        {renderIf(
          this.state.requireMessage,

          <MissingRequirementsContent
            missingFields={this.state.requiredFields}
            refsFields={{ form2020Files: this.financialFormUploadRef?.current }}
            formattedStrings={fm}
            onMessageClick={() => this.setState({ requirePopUp: true })}
          />
        )}
        <div className="continue-summary-panel align-center">
          <CustomButton
            testid={'formContinue'}
            label={fm.continueButton}
            style={{ marginTop: '42px', marginBottom: '30px' }}
            onClick={this.handleContinueClick}
          />
        </div>
      </React.Fragment>
    );
  }
}
