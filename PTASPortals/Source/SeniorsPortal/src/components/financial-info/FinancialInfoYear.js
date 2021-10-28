import React from 'react';
import './FinancialInfo.css';
import * as fm from './FormatTexts';
import ReactToPrint from 'react-to-print';
import TabContainer from '../common/TabContainer';
import IncomeHelp from '../income-help/IncomeHelp';
import CustomButton from '../common/CustomButton';
import SelectInput from '../common/SelectInputW';
import RadioInput from '../common/RadioInput';
import UploadFile from '../common/UploadFile/UploadFile';
import HelpText from '../common/HelpText';
import TotalIncome from '../common/TotalIncome';
import Grid from '@material-ui/core/Grid';
import Collapse from '@material-ui/core/Collapse';
import SummaryButton from '../common/SummaryButton';
import SwitchInput from '../common/SwitchInput';

import ExpandLink from '../common/ExpandLink';
import KeyboardArrowRightIcon from '@material-ui/icons/KeyboardArrowRight';
import KeyboardArrowLeftIcon from '@material-ui/icons/KeyboardArrowLeft';
import { FormattedMessage } from 'react-intl';
import { withStyles } from '@material-ui/core/styles';
import ShortDivider from '../common/ShortDivider';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import DialogContentText from '@material-ui/core/DialogContentText';
import { cloneDeep } from 'lodash';

import Form2020 from './Form2020';
import {
  CollectionConsumer,
  CollectionContext,
} from '../../contexts/CollectionContext';
import ToolbarItem from './ToolbarItem';
import {
  arrayNullOrEmpty,
  renderIf,
  sumOnFinancialForms,
} from '../../lib/helpers/util';
import deepEqual from 'deep-equal';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { getParcelDetail } from '../../services/dynamics-service';
import Button from '@material-ui/core/Button';

import {
  deleteSeniorAppDetail,
  deleteFileMetadata,
  deleteSeniorApplicationsFinancial,
  updateSeniorAppFinancial,
  createSeniorAppFinancial,
} from '../../services/dataServiceProvider';

import {
  createOrUpdateFileMetadataEntities,
  createFileArraysFromMetadata,
} from '../common/UploadFile/uploaderHelper';

import { createDocuSignHtml } from '../../services/blobService';
import uuid from 'uuid';
import CardSide from '../common/CardSide';
import CheckIcon from '@material-ui/icons/Check';
import { getAppInsightsInstance } from '../../services/telemetryService';

const SwitchInputCss = withStyles({
  root: {
    marginTop: '19px',
  },
  label: {
    maxWidth: '310px',
    fontSize: '14px',
  },
})(SwitchInput);

const RadioInputCss = withStyles({
  root: {
    marginTop: '19px',
  },
  label: {
    maxWidth: '310px',
    fontSize: '14px',
  },
})(RadioInput);

const SelectInputCss = withStyles({
  '& .MuiInput-root': {
    width: '180px',
  },
  root: {
    marginTop: '19px',
    '.MuiInputBase-root': {
      width: '250px',
    },
    '& .MuiInput-root': {
      width: '100%',
    },
    '& .MuiCardContent-root': {
      width: '180px',
      padding: 0,
      paddingTop: '15px',
    },
  },
  select: {
    width: '250px',
  },
})(SelectInput);

const CustomButtonCss = withStyles({
  button: {
    marginTop: '14px',
    backgroundColor: 'white',
    color: '#6f1f62',
    borderStyle: 'solid',
    borderWidth: '2px',
    borderColor: '#6f1f62',
    fontSize: '16px',
  },
})(CustomButton);

let totalHouseIncome = null;

const appInsights = getAppInsightsInstance();

export default class FinancialInfoYear extends React.Component {
  constructor() {
    super();

    this.state = {
      currentSavedFormReference: '',
      formsToBeDeleted: [],
      formsOptions: [
        ['F1040', ``, false],
        ['F1040EZ', ``, false],
        ['F1040A', ``, false],
        ['1099', ``, false],
      ],
      // new states
      formTypes: [
        {
          value: 'F1040',
          label: 'IRS Form 1040',
          id: 668020000,
        },
        { value: 'F1040EZ', label: 'IRS Form 1040EZ', id: 668020002 },
        { value: 'F1040A', label: 'IRS Form 1040A', id: 668020001 },
      ],
      selectedUser: null,
      appliedYear: false,
      username: '',
      wantToApply: false,
      //current
      formTypeId: 0,
      formId: null,
      financesMain: true,
      Forms: false,
      Form1040EZ: false,
      Form1040A: false,
      Form1040: false,
      Form20181040: false,
      Form1099: false,
      Form2020: false,

      iFilledTaxReturn: false,
      occupantFilledTaxReturn: false,
      spouseFilledTaxReturn: false,

      users: [],

      years: [],

      spouse: {
        name: 'Michelle Brooks',
      },

      occupants: [
        {
          name: 'Richard Thomas',
        },
        {
          name: 'John Case',
        },
      ],

      myForm1040: {
        year: 2017,
        f1040YourIRS1040: [],
        f1040YourSSA1099: [],
        ScheduleC: [],
        ScheduleD: [],
        ScheduleE: [],
        ScheduleF: [],
        expenses: {
          key: '123',
          expenseDocuments: [],
        },
      },

      myForm20181040: {
        year: 2017,
        f20181040YourIRS1040: [],
        f20181040YourSSA1099: [],
        ScheduleC: [],
        ScheduleD: [],
        ScheduleE: [],
        ScheduleF: [],
        expenses: {
          key: '123',
          expenseDocuments: [],
        },
      },

      myForm1099: {
        year: 2017,
        f1099_IncomeFileArray: [],
        expenses: {
          key: '123',
          expenseDocuments: [],
        },
      },

      myForm1040EZ: {
        year: 2017,
        fez_IRSFileArray: [],
        expenses: {
          key: '123',
          expenseDocuments: [],
        },
      },

      myForm1040A: {
        year: 2017,
        fez_IRSFileArray: [],
        expenses: {
          key: '123',
          expenseDocuments: [],
        },
      },
      baseSchedules: {
        scheduleC: {
          id: null,
          f1040Line13Depreciation: null,
          f1040Line30FootageForBusiness: null,
          f1040Line30ExpenseForBusiness: null,
          f1040Line31NetProfitOrLoss: null,
          f1040ScheduleC: [],
          f1040Line3PercentageUsed: null,
          f1040YourForm8829: [],
          form8829Id: null,
        },
        scheduleD: {
          id: null,
          f1040ScheduleDRequired: null,
          f1040Line1ShortTerm8949BoxA: null,
          f1040Line2ShortTerm8949BoxB: null,
          f1040Line3ShortTerm8949BoxC: null,
          f1040Line4ShortTermGains: null,
          f1040Line5NetShortTermGains: null,
          f1040Line8LongTerm8949BoxA: null,
          f1040Line9LongTerm8949BoxB: null,
          f1040Line10LongTerm8949BoxC: null,
          f1040Line13CapitalGainDistributions: null,
          f1040YourScheduleD: [],
          f1040YourConsolidated1099: [],
          f1040YourForm8949: [],
          f1040TotalGainsFromColumnH: null,
          form8949Id: null,
        },
        scheduleE: {
          id: null,
          f1040Line1PhysicalAddress: null,
          f1040Line2bTypeOfProperty: null,
          f1040Line18DepreciationExpense: null,
          f1040YourScheduleE: [],
        },
        scheduleF: {
          id: null,
          f1040Line14Depreciation: null,
          f1040YourScheduleF: [],
        },
      },
      showBackButton: false,
      totalHouseHoldIncome: null,
    };
  }

  componentDidMount() {
    window.scrollTo(0, 0);
    if (this.props.detail) {
      this.setSeniorAppDetail(this.props.detail);
    }

    this.setOccupantsAndOtherUsers(
      this.props.contact,
      this.props.seniorApp,
      this.props.occupants
    );

    if (this.state.users.length === 1) {
      this.addFinancialInfoButton(this.state.users[0]);
      this.setState({ showBackButton: false });
    }

    if (this.props.financials && this.props.detail) {
      totalHouseIncome = this.props.calculateFormsAmounts(
        this.props.financials[this.props.detail.detailId]
      );
    }
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.detail && !deepEqual(this.props.detail, prevProps.detail)) {
      //this.backButtonForm();
      this.setSeniorAppDetail(this.props.detail);
      this.setFinancialsAndOcupants();
      this.addFinancialInfoButton(this.state.users[0]);
    }

    if (
      this.props.financials &&
      !deepEqual(this.props.financials, prevProps.financials)
    ) {
      this.setFinancialsAndOcupants();
    }
    /*if (
      this.props.detail &&
      //!arrayNullOrEmpty(this.props.occupants) &&
      !deepEqual(this.props.detail, prevProps.detail)
    ) {
      this.backButtonForm();
      this.setOccupantsAndOtherUsers(
        this.props.contact,
        this.props.seniorApp,
        this.props.occupants
      );
    }*/
    if (
      this.props.occupants &&
      !deepEqual(this.props.occupants, prevProps.occupants)
    ) {
      this.setFinancialsAndOcupants();
    }

    if (this.state.users && !deepEqual(this.state.users, prevState.users)) {
      if (this.state.users.length === 1) {
        this.addFinancialInfoButton(this.state.users[0]);
        this.setState({ showBackButton: false });
      } else {
        this.backButtonForm();
      }
    }
  }

  setFinancialsAndOcupants = () => {
    this.setFinancialsInfo(this.props.financials);
    this.setOccupantsAndOtherUsers(
      this.props.contact,
      this.props.seniorApp,
      this.props.occupants
    );
  };

  deleteExtraData = async () => {
    if (this.props.financials) {
      let promisesMetadata = [];
      let promisesDetails = [];
      Object.keys(this.props.financials).forEach((key, index) => {
        // if financial do not contain even one element, remove key/seniorDetail's file metadata
        if (arrayNullOrEmpty(this.props.financials[key])) {
          let matchingMetadata = this.props.filesMetadata.filter(
            f =>
              f.seniorExemptionApplicationId &&
              f.seniorExemptionApplicationDetailId === key &&
              f.isBlob
          );

          matchingMetadata = arrayNullOrEmpty(matchingMetadata)
            ? null
            : matchingMetadata[0];
          if (matchingMetadata) {
            promisesMetadata.push(deleteFileMetadata(matchingMetadata.id));
          }
          promisesDetails.push(deleteSeniorAppDetail(key));
        }
      });

      await Promise.all(promisesMetadata);
      await Promise.all(promisesDetails);
    }
  };

  deleteNotNeededItems = async () => {};

  updateSeniorDetail = () => {};

  setSeniorAppDetail = detail => {
    if (detail) {
    }
  };

  setFinancialsInfo = financials => {
    if (financials) {
      let currentFinancials = financials[this.props.detail.detailId];
      let users = cloneDeep(this.state.users);
      if (!arrayNullOrEmpty(currentFinancials)) {
        currentFinancials.map(f => {
          users.map(u => {
            if (
              f.filertype === u.filterTypeValue &&
              ((f.filertype === 668020003 && f.occupantid === u.id) ||
                f.filertype === 668020000)
            ) {
              u.forms.push(f);
              u.willApply = !arrayNullOrEmpty(u.forms);
              this.setState({ appliedYear: true });
              //Form Type 1099 or No Return Filed = 668020010
              if (f.financialformtype === 668020010) {
                u.is1099 = true;
                u.is1040 = false;
                u.formTypeId = f.financialformtype;
              } else {
                //            1040 = 668020000                   1040A = 668020001                  1040EZ =  668020002
                if (
                  f.financialformtype === 668020000 ||
                  f.financialformtype === 668020001 ||
                  f.financialformtype === 668020002
                ) {
                  u.formTypeId = f.financialformtype;
                  u.is1099 = false;
                  u.is1040 = true;
                }
              }
            }
          });
        });
        if (!deepEqual(users, this.state.users)) {
          this.setState({ users });
          // this.setState({
          //   wantToApply: this.state.users.some(u => u.is1040 === true),
          // });
        }
      } else {
        this.updateAfterDelete();
      }
    }
    if (this.props.detail && this.props.financials) {
      totalHouseIncome = this.props.calculateFormsAmounts(
        this.props.financials[this.props.detail.detailId]
      );
    }
  };

  setOccupantsAndOtherUsers = (taxpayer, seniorApp, occupants) => {
    let users = [];

    users.push({
      // filterType TaxPayer = 668020000
      filterTypeValue: 668020000,
      isOccupant: false,
      isTaxpayer: true,
      isSpouse: false,
      id: taxpayer.contactid,
      fullName: `${taxpayer.firstname ? taxpayer.firstname : ''} ${
        taxpayer.middlename ? taxpayer.middlename : ''
      } ${taxpayer.lastname ? taxpayer.lastname : ''}`,
      forms: [],
      is1040: false,
      is1099: false,
      willApply: false,
    });

    if (!arrayNullOrEmpty(occupants)) {
      occupants.map(o => {
        if (o.occupanttype && o.occupanttype === 668020001) {
          users.push({
            // filterType Filed By Co-owner / Co-signer = 668020003
            filterTypeValue: 668020003,
            isOccupant: true,
            isTaxpayer: false,
            isSpouse: false,
            id: o.seappoccupantId,
            fullName: `${o.occupantfirstName ? o.occupantfirstName : ''} ${
              o.occupantmiddlename ? o.occupantmiddlename + '' : ''
            } ${o.occupantlastname ? o.occupantlastname : ''}`,
            forms: [],
            is1040: false,
            is1099: false,
            willApply: false,
          });
        }
      });
    }

    if (!deepEqual(users, this.state.users)) {
      this.setState({ users }, () => {
        this.setFinancialsInfo(this.props.financials);
      });
      // this.setState({
      //   wantToApply: this.state.users.some(u => u.is1040 === true),
      // });
    }
  };

  set2020Option = () => {
    let formOptions = cloneDeep(this.state.formsOptions);
    this.setState({
      Form2020: true,
      Form1040EZ: false,
      Form1040A: false,
      Form1040: false,
      Form1099: false,
      Form20181040: false,
      formTypeId: 668020002,
      formId: 'F2020',
    });
    formOptions[0][2] = false;
    formOptions[1][2] = false;
    formOptions[2][2] = false;
    formOptions[3][2] = false;
    this.setState({ formsOptions: formOptions });
  };

  handleSwitchInputChange = (checked, e, id) => {
    this.setState({ [id]: checked });
  };

  handleWantToApply = (checked, e) => {
    this.props.handleWantToApply(this.props.detail.detailId, checked);
  };

  handleWillApply = (checked, e, userId) => {
    let users = cloneDeep(this.state.users);
    let user = users.filter(u => u.id === userId)[0];

    //user.is1040 = checked;
    //user.is1099 = !checked;
    user.willApply = checked;

    //let users = this.state.users.map(u => (u.id === userId ? user : u));
    this.setState({ users });

    // this.setState({
    //   wantToApply: this.state.users.some(u => u.is1040 === true),
    // });

    // let yearsCopy = cloneDeep(this.state.years);

    // for (var i = 0; i < yearsCopy[this.state.selectedIndex].forms.length; i++) {
    //   let user = yearsCopy[this.state.selectedIndex].forms[i];
    //   if (user.userId == userId) {
    //     yearsCopy[this.state.selectedIndex].forms[i].iFilledTaxReturn = checked;
    //   }
    // }
    // this.setState({ years: yearsCopy });
  };

  addFinancialInfoButton = user => {
    //this.props.hideToolbar(true);
    // this.setState({ selectedUser: user, username: user.fullName })
    if (this.props.selectedYear - 1 > 2017) {
      this.setState({
        Form20181040: true,
        Form1040EZ: false,
        Form1040A: false,
        Form1040: false,
        Form1099: false,
        Forms: true,
        formSelection: true,
        financesMain: false,
        selectedUser: user,
        username: user.fullName,
      });
    } else {
      this.setState({
        Form20181040: false,
        Form1040EZ: false,
        Form1040A: false,
        Form1040: true,
        Form1099: false,
        Forms: true,
        formSelection: true,
        financesMain: false,
        selectedUser: user,
        username: user.fullName,
        formId: 'F1040',
      });

      this.props.handleHideEnoughMonths(true);

      // if (!formId) {
      //   this.setState({
      //     Forms: true,
      //     formSelection: true,
      //     financesMain: false,
      //     Form1040: true,
      //     formId: 'F1040',
      //   });
      // } else {
      //   if (formId == 'form1040A') {
      //     this.setState({
      //       Forms: true,
      //       formSelection: true,
      //       financesMain: false,
      //       Form1040A: true,
      //       formId: 'F1040A',
      //     });
      //   } else if (formId == 'form1040EZ') {
      //     this.setState({
      //       Forms: true,
      //       formSelection: true,
      //       financesMain: false,
      //       Form1040EZ: true,
      //       formId: 'F1040EZ',
      //     });
      //   } else {
      //     this.setState({
      //       Forms: true,
      //       formSelection: true,
      //       financesMain: false,
      //       Form1040: true,
      //       formId: 'F1040',
      //     });
      //   }
      // }
    }
    // this.setState({ financesMain: false });
    this.getAndSetSelectedForm(user);
    window.scrollTo(0, 0);
    this.setState({ showBackButton: true });
  };

  backButtonForm = () => {
    // this.props.hideToolbar(false);
    this.setState({
      Form20181040: false,
      Form1040EZ: false,
      Form1040A: false,
      Form1040: false,
      Form1099: false,
      Form2020: false,
      Forms: false,
      formSelection: false,
      financesMain: true,
      selectedUser: null,
      username: '',
      formId: null,
      showBackButton: false,
    });

    this.props.handleHideEnoughMonths(false);
    window.scrollTo(0, 0);
  };

  getAndSetSelectedForm = user => {
    let detailId = this.props.detail.detailId;
    let currentSavedForm = '';
    let formsArray = [];

    if (this.props.financials) {
      let yearFormsArray = this.props.financials[detailId];

      if (!arrayNullOrEmpty(yearFormsArray)) {
        if (user.filterTypeValue === 668020000) {
          let userForms = user.forms;
          formsArray = yearFormsArray.filter(item =>
            userForms.some(
              i => i.sefinancialformsid === item.sefinancialformsid
            )
          );
        } else {
          formsArray = yearFormsArray.filter(item => {
            return item.occupantid === user.id;
          });
        }
      }

      this.set2020Option();
      currentSavedForm = '2020';

      if (!arrayNullOrEmpty(formsArray)) {
        formsArray.map(item => {
          switch (item.financialformtype) {
            case 668020000:
              this.set2020Option();
              currentSavedForm = '2020';
              break;
            case 668020002:
              this.set2020Option();
              currentSavedForm = '2020';
              break;
            case 668020001:
              this.set2020Option();
              currentSavedForm = '2020';
              break;
            case 668020010:
              this.set2020Option();
              currentSavedForm = '2020';
              break;
            default:
              break;
          }
        });
      } else {
        this.set2020Option();
        currentSavedForm = '2020';
      }
      this.setState({ currentSavedFormReference: currentSavedForm });
    } else {
      this.set2020Option();
      currentSavedForm = '2020';
    }
  };

  handleYearClick = index => {
    this.backButtonForm();
    var len = this.state.yearsTab.length;
    var selectedYear = [];
    for (var i; i < len; i++) selectedYear.push(false);

    selectedYear[index] = true;
    this.setState({
      yearsTab: selectedYear,
      selectedIndex: index,
      selectedYear: this.state.years[index].year,
    });
  };

  canContinue = () => {
    let existForm = false;
    if (this.props.financials) {
      Object.keys(this.props.financials).forEach(key => {
        if (!arrayNullOrEmpty(this.props.financials[key])) {
          existForm = true;
        }
      });
    }

    return existForm;
  };

  cleanObjectProperties = object => {
    Object.keys(object).forEach(key => {
      if (
        object[key] === undefined ||
        object[key] === null ||
        object[key] === NaN
      ) {
        object[key] = '';
      } else if (!(object[key] && object[key].length)) {
        object[key] = (object[key] + '').trim();
      }
    });

    return object;
  };

  putParcelIntoSeniorApp = async seniorApp => {
    const parcel = await getParcelDetail(seniorApp.parcelid);
    if (parcel) {
      seniorApp.namesonaccount = parcel.namesonaccount;
      seniorApp.address = parcel.address;
      seniorApp.district = parcel.district;
      seniorApp.zipcode = parcel.zipcode;
      seniorApp.major = parcel.major;
      seniorApp.minor = parcel.minor;
    }

    if (seniorApp.addrchange) {
      seniorApp.correspondencename =
        seniorApp.correspondencename !== ''
          ? seniorApp.correspondencename
          : ' ';

      seniorApp.addrstreet1 =
        seniorApp.addrstreet1 !== '' ? `${seniorApp.addrstreet1}` : ' ';

      seniorApp.addrcity =
        seniorApp.addrcity !== '' ? `${seniorApp.addrcity}` : ' ';

      seniorApp.correspondenceStreetCity = `${seniorApp.addrstreet1} ${seniorApp.addrcity}`;

      seniorApp.addrstate =
        seniorApp.addrstate !== '' ? `${seniorApp.addrstate}` : ' ';

      seniorApp.addrpostal =
        seniorApp.addrpostal !== '' ? `${seniorApp.addrpostal}` : ' ';

      seniorApp.correspondenceStateZip = `${seniorApp.addrstate} ${seniorApp.addrpostal}`;
    } else {
      seniorApp.correspondencename = 'No';
      seniorApp.correspondenceStreetCity = ' ';
      seniorApp.correspondenceStateZip = ' ';
    }

    if (seniorApp.differentcheckaddress) {
      seniorApp.checkaddressname =
        seniorApp.checkaddressname !== '' ? seniorApp.checkaddressname : ' ';

      seniorApp.checkaddressstreet =
        seniorApp.checkaddressstreet !== ''
          ? `${seniorApp.checkaddressstreet}`
          : ' ';

      seniorApp.checkaddresscity =
        seniorApp.checkaddresscity !== ''
          ? `${seniorApp.checkaddresscity}`
          : ' ';

      seniorApp.checkStreetCity = `${seniorApp.checkaddressstreet} ${seniorApp.checkaddresscity}`;

      seniorApp.checkaddressstate =
        seniorApp.checkaddressstate !== ''
          ? `${seniorApp.checkaddressstate}`
          : ' ';

      seniorApp.checkaddresspostalcode =
        seniorApp.checkaddresspostalcode !== ''
          ? `${seniorApp.checkaddresspostalcode}`
          : ' ';

      seniorApp.checkStateZip = `${seniorApp.checkaddressstate} ${seniorApp.checkaddresspostalcode}`;
    } else {
      seniorApp.checkaddressname = 'No';
      seniorApp.checkStreetCity = ' ';
      seniorApp.checkStateZip = ' ';
    }

    return seniorApp;
  };

  addFinancialForms = seniorApp => {
    let summaryForms = [];
    const forms = this.props.financials[this.props.details[0].seappdetailid];
    let usersForms = [];
    let ownerForm = [];
    let currentForms = [];

    this.state.users.map(user => {
      user.forms.map(form => {
        usersForms.push({
          ownerUser: user.fullName,
          formId: form.sefinancialformsid,
        });
      });
    });

    usersForms.map(userForm => {
      forms.map(form => {
        if (userForm.formId === form.sefinancialformsid) {
          currentForms.push(form);
        }
      });
    });

    if (!arrayNullOrEmpty(currentForms)) {
      currentForms.map(d => {
        let form = {};
        // set year value
        let year = this.props.years.filter(y => y.yearid === d.yearid)[0];

        form.year = year.name;

        ownerForm = usersForms.filter(item => {
          return item.formId == d.sefinancialformsid;
        });

        if (!arrayNullOrEmpty(ownerForm)) {
          form.occupantFullName = ownerForm[0].ownerUser;
        }

        form = { ...form, ...d };
        form.template = '1099 or no return filed';

        let medicarePlan = '';
        if (form.financialformtype == 668020010) {
          if (form.medicareplanid !== '') {
            medicarePlan = this.props.medicarePlans.filter(item => {
              return item.id === form.medicareplanid;
            });
            form.medicareplanid = !arrayNullOrEmpty(medicarePlan)
              ? medicarePlan[0].name
              : 'NA';
            form.medicareprovidername = !arrayNullOrEmpty(medicarePlan)
              ? medicarePlan[0].organizationName
              : 'NA';
          }

          let disabilitySource = this.props.disabilitySources.filter(item => {
            return item.attributeValue == form.incomedisabilitysrc;
          })[0];
          if (disabilitySource) {
            form.incomedisabilitysrc = disabilitySource.value;
          }
        }
        form = this.cleanObjectProperties(form);
        summaryForms.push(form);
      });
    }
    seniorApp.forms = summaryForms;
    return seniorApp;
  };

  setDatesFormatUS = seniorApp => {
    Object.keys(seniorApp).forEach(key => {
      let value = seniorApp[key].toString();
      if (value.includes(':')) {
        let dateWithOutTime = value.slice(0, value.search('T'));
        let datePositions = dateWithOutTime.split('-');
        seniorApp[key] = `${datePositions[1]}-${
          datePositions[2]
        }-${datePositions[0].trim()}`;
      }
    });
    return seniorApp;
  };

  putContactIntoSeniorApp = (seniorApp, contact) => {
    seniorApp = Object.assign(seniorApp, contact);

    return seniorApp;
  };

  determinateProvidedOrNotYesOrNo = seniorApp => {
    Object.keys(seniorApp).forEach(key => {
      if (key.slice(0, 3).includes('doc')) {
        seniorApp[key] = seniorApp[key] ? 'Provided' : 'Not provided';
      } else {
        if (typeof seniorApp[key] === 'boolean') {
          seniorApp[key] = seniorApp[key] ? 'Yes' : 'No';
        }
      }
    });

    return seniorApp;
  };

  getDocuSignHtml = async () => {
    let seniorApp = cloneDeep(this.props.seniorApp);
    seniorApp.template = 'summary';
    seniorApp.spousefullname = `${
      seniorApp.spousefirstname ? seniorApp.spousefirstname : ''
    } ${seniorApp.spousemiddlename ? seniorApp.spousemiddlename : ''} ${
      seniorApp.spouselastname ? seniorApp.spouselastname : ''
    }`;

    seniorApp.occupieddate = seniorApp.occupieddate.substring(0, 4);

    // it adds properties of contact to seniorApp
    seniorApp.seniorApp = this.putContactIntoSeniorApp(
      seniorApp,
      this.props.contact
    );
    // add parcel information to seniorApp
    seniorApp = await this.putParcelIntoSeniorApp(seniorApp);

    // it changes properties that starts with doc to provided or not.
    seniorApp = this.determinateProvidedOrNotYesOrNo(seniorApp);

    // add forms

    seniorApp = this.addFinancialForms(seniorApp);

    //clean object invalid values and convert everything to string
    seniorApp = this.cleanObjectProperties(seniorApp);

    // format the dates for US format
    seniorApp = this.setDatesFormatUS(seniorApp);

    // get html from formed json

    const html = await createDocuSignHtml(seniorApp);

    //set html on auth context
    this.props.setHtmlSummary(html.htmlResponse);

    //save html on the local storage
    localStorage.setItem(
      `${this.props.seniorApp.seapplicationid}_DocuSignHTML`,
      JSON.stringify(html.htmlResponse)
    );
  };

  handleContinueClick = async () => {
    //Navigate to next year, until last year, jump to next tab
    window.scrollTo(0, 0);
    this.props.showSavingOverlay();
    if (!this.props.readOnlyMode) {
      await this.deleteExtraData();
      this.updateSeniorDetail();
      await Promise.all([
        this.props.getSeniorDetails(this.props.seniorApp.seapplicationid),
        this.props.getFilesMetadata(this.props.seniorApp.seapplicationid),
      ]);
      await this.getDocuSignHtml();
    }
    this.props.nextTab();
    /*appInsights.trackEvent({
      name: 'continue',
      event: 'click',
      section: 'PropertyInfo',
    });*/
    this.props.hideSavingOverlay();
  };

  onSaveExpenses = (expenses, user) => {
    let expensesForm = user.forms.filter(
      f =>
        f.financialformtype === 668020011 &&
        f.yearid === this.props.detail.yearid
    );

    expensesForm = arrayNullOrEmpty(expensesForm) ? {} : expensesForm[0];
    expensesForm.financialformtype = 668020011;
    expensesForm.filertype = user.filterTypeValue;
    expensesForm.yearid = this.props.detail.yearid;

    // Expenses:
    expensesForm.expenseassistedliving = expenses.assistedLiving;
    expensesForm.expensesinhome = expenses.inHomeCare;
    expensesForm.expensepres = expenses.nonReimbursedPrescriptions;
    expensesForm.expensemedicareplan = expenses.approvedMedicare;
    expensesForm.medicareplanid = expenses.medicalPlan;

    return expensesForm;
  };

  onSetExpenses = (expenses, user) => {
    let expensesForm = user.forms.filter(
      f =>
        f.financialformtype === 668020011 &&
        f.yearid === this.props.detail.yearid
    );

    expensesForm = arrayNullOrEmpty(expensesForm) ? {} : expensesForm[0];

    // Expenses:

    let newExpenses = cloneDeep(expenses);
    newExpenses.assistedLiving = expensesForm.expenseassistedliving;
    newExpenses.inHomeCare = expensesForm.expensesinhome;
    newExpenses.nonReimbursedPrescriptions = expensesForm.expensepres;
    newExpenses.approvedMedicare = expensesForm.expensemedicareplan;

    newExpenses.medicalPlan = expensesForm.medicareplanid
      ? expensesForm.medicareplanid
      : '';

    if (newExpenses.medicalPlan) {
      let currentPlan = this.props.medicarePlans.filter(
        m => m.id === newExpenses.medicalPlan
      )[0];

      if (currentPlan) {
        newExpenses.medicareProvider = currentPlan.organizationName;
      }
    }

    return newExpenses;
  };

  onSaveForm8829 = (schedule, user) => {
    let form8829 = user.forms.filter(
      f =>
        f.financialformtype === 668020008 &&
        f.yearid === this.props.detail.yearid &&
        f.sefinancialformsid === schedule.form8829Id
    );

    form8829 = arrayNullOrEmpty(form8829) ? {} : form8829[0];
    form8829.sefinancialformsid = schedule.form8829Id;
    form8829.financialformtype = 668020008;
    form8829.filertype = user.filterTypeValue;
    form8829.yearid = this.props.detail.yearid;
    form8829['8829percentusedforbusiness'] = schedule.f1040Line3PercentageUsed;

    return form8829;
  };

  onSaveScheduleC = (schedules, user) => {
    let scheduleForms = [];
    if (!arrayNullOrEmpty(schedules)) {
      schedules.map(schedule => {
        let scheduleForm = user.forms.filter(
          f =>
            f.financialformtype === 668020004 &&
            f.yearid === this.props.detail.yearid &&
            f.sefinancialformsid === schedule.id
        );

        scheduleForm = arrayNullOrEmpty(scheduleForm) ? {} : scheduleForm[0];
        scheduleForm.sefinancialformsid = schedule.id;
        scheduleForm.financialformtype = 668020004;
        scheduleForm.filertype = user.filterTypeValue;
        scheduleForm.yearid = this.props.detail.yearid;

        scheduleForm.schdCdepreciation = schedule.f1040Line13Depreciation;
        scheduleForm.schdCsqfthome = schedule.f1040Line30HomeFootage;
        scheduleForm.schdCsqftbusiness = schedule.f1040Line30FootageForBusiness;
        scheduleForm.schdCexbusinesshome =
          schedule.f1040Line30ExpenseForBusiness;
        scheduleForm.schdCnetprofitloss = schedule.f1040Line31NetProfitOrLoss;
        let form8829 = this.onSaveForm8829(schedule, user);
        scheduleForms.push(scheduleForm);
        scheduleForms.push(form8829);
      });
    }

    return scheduleForms;
  };

  returnSchedule1 = user => {
    let schedule1 = user.forms.filter(
      f =>
        f.financialformtype === 668020003 &&
        f.yearid === this.props.detail.yearid
    );
    return schedule1;
  };

  returnScheduleC = user => {
    let scheduleForms = user.forms.filter(
      f =>
        f.financialformtype === 668020004 &&
        f.yearid === this.props.detail.yearid
    );

    return scheduleForms;
  };

  returnScheduleD = user => {
    let scheduleForms = user.forms.filter(
      f =>
        f.financialformtype === 668020005 &&
        f.yearid === this.props.detail.yearid
    );

    return scheduleForms;
  };

  returnScheduleE = user => {
    let scheduleForms = user.forms.filter(
      f =>
        f.financialformtype === 668020006 &&
        f.yearid === this.props.detail.yearid
    );

    return scheduleForms;
  };

  returnScheduleF = user => {
    let scheduleForms = user.forms.filter(
      f =>
        f.financialformtype === 668020007 &&
        f.yearid === this.props.detail.yearid
    );

    return scheduleForms;
  };

  onSetScheduleC = user => {
    let schedules = [];
    let forms8829 = user.forms.filter(
      f =>
        f.financialformtype === 668020008 &&
        f.yearid === this.props.detail.yearid
    );

    let scheduleForms = this.returnScheduleC(user);

    if (!arrayNullOrEmpty(scheduleForms)) {
      scheduleForms.map((scheduleForm, index) => {
        let newSchedule = {};
        newSchedule.id = scheduleForm.sefinancialformsid;
        newSchedule.f1040Line13Depreciation = scheduleForm.schdCdepreciation;
        newSchedule.f1040Line30FootageForBusiness =
          scheduleForm.schdCsqftbusiness;
        newSchedule.f1040Line30ExpenseForBusiness =
          scheduleForm.schdCexbusinesshome;
        newSchedule.f1040Line31NetProfitOrLoss =
          scheduleForm.schdCnetprofitloss;

        newSchedule.f1040Line30HomeFootage = scheduleForm.schdCsqfthome;

        // There is no way currently to associate a form to another form, so associate them by index
        if (index < forms8829.length) {
          newSchedule.f1040Line3PercentageUsed =
            forms8829[index]['8829percentusedforbusiness'];
          newSchedule.form8829Id = forms8829[index].sefinancialformsid;
        }

        schedules.push(newSchedule);
      });
    }

    return schedules;
  };

  onSaveForm8949 = (schedule, user) => {
    let form8949 = user.forms.filter(
      f =>
        f.financialformtype === 668020009 &&
        f.yearid === this.props.detail.yearid &&
        f.sefinancialformsid === schedule.form8949Id
    );

    form8949 = arrayNullOrEmpty(form8949) ? {} : form8949[0];
    form8949.sefinancialformsid = schedule.form8949Id;
    form8949.financialformtype = 668020009;
    form8949.filertype = user.filterTypeValue;
    form8949.yearid = this.props.detail.yearid;
    form8949['8949totalgains'] = schedule.f1040TotalGainsFromColumnH;

    return form8949;
  };

  onSaveScheduleD = (schedules, user) => {
    let scheduleForms = [];
    if (!arrayNullOrEmpty(schedules)) {
      schedules.map(schedule => {
        let scheduleForm = user.forms.filter(
          f =>
            f.financialformtype === 668020005 &&
            f.yearid === this.props.detail.yearid &&
            f.sefinancialformsid === schedule.id
        );

        scheduleForm = arrayNullOrEmpty(scheduleForm) ? {} : scheduleForm[0];
        scheduleForm.sefinancialformsid = schedule.id;
        scheduleForm.financialformtype = 668020005;
        scheduleForm.filertype = user.filterTypeValue;
        scheduleForm.yearid = this.props.detail.yearid;

        scheduleForm.schdDrequired = schedule.f1040ScheduleDRequired;
        scheduleForm.schdDshorttermboxA = schedule.f1040Line1ShortTerm8949BoxA;
        scheduleForm.schdDshorttermboxB = schedule.f1040Line2ShortTerm8949BoxB;
        scheduleForm.schdDshorttermboxC = schedule.f1040Line3ShortTerm8949BoxC;
        scheduleForm.schdDshortterm6252 = schedule.f1040Line4ShortTermGains;

        scheduleForm.schdDnetshorttermgains =
          schedule.f1040Line5NetShortTermGains;
        scheduleForm.schdDlongtermboxA = schedule.f1040Line8LongTerm8949BoxA;
        scheduleForm.schdDlongtermboxB = schedule.f1040Line9LongTerm8949BoxB;
        scheduleForm.schdDlongtermboxC = schedule.f1040Line10LongTerm8949BoxC;
        scheduleForm.schdDcapgains =
          schedule.f1040Line13CapitalGainDistributions;

        let form8949 = this.onSaveForm8949(schedule, user);
        scheduleForms.push(scheduleForm);
        scheduleForms.push(form8949);
      });
    }

    return scheduleForms;
  };

  onSetScheduleD = user => {
    let schedules = [];
    let forms8949 = user.forms.filter(
      f =>
        f.financialformtype === 668020009 &&
        f.yearid === this.props.detail.yearid
    );

    let scheduleForms = this.returnScheduleD(user);

    if (!arrayNullOrEmpty(scheduleForms)) {
      scheduleForms.map((scheduleForm, index) => {
        let newSchedule = {};

        newSchedule.id = scheduleForm.sefinancialformsid;
        newSchedule.f1040ScheduleDRequired = scheduleForm.schdDrequired;
        newSchedule.f1040Line1ShortTerm8949BoxA =
          scheduleForm.schdDshorttermboxA;
        newSchedule.f1040Line2ShortTerm8949BoxB =
          scheduleForm.schdDshorttermboxB;
        newSchedule.f1040Line3ShortTerm8949BoxC =
          scheduleForm.schdDshorttermboxC;
        newSchedule.f1040Line4ShortTermGains = scheduleForm.schdDshortterm6252;
        newSchedule.f1040Line5NetShortTermGains =
          scheduleForm.schdDnetshorttermgains;
        newSchedule.f1040Line8LongTerm8949BoxA = scheduleForm.schdDlongtermboxA;
        newSchedule.f1040Line9LongTerm8949BoxB = scheduleForm.schdDlongtermboxB;
        newSchedule.f1040Line10LongTerm8949BoxC =
          scheduleForm.schdDlongtermboxC;
        newSchedule.f1040Line13CapitalGainDistributions =
          scheduleForm.schdDcapgains;

        // There is no way currently to associate a form to another form, so associate them by index
        if (index < forms8949.length) {
          newSchedule.f1040TotalGainsFromColumnH =
            forms8949[index]['8949totalgains'];
          newSchedule.form8949Id = forms8949[index].sefinancialformsid;
        }

        schedules.push(newSchedule);
      });
    }

    return schedules;
  };

  onSaveScheduleE = (schedules, user) => {
    let scheduleForms = [];
    if (!arrayNullOrEmpty(schedules)) {
      schedules.map(schedule => {
        let scheduleForm = user.forms.filter(
          f =>
            f.financialformtype === 668020006 &&
            f.yearid === this.props.detail.yearid &&
            f.sefinancialformsid === schedule.id
        );

        scheduleForm = arrayNullOrEmpty(scheduleForm) ? {} : scheduleForm[0];
        scheduleForm.sefinancialformsid = schedule.id;
        scheduleForm.financialformtype = 668020006;
        scheduleForm.filertype = user.filterTypeValue;
        scheduleForm.yearid = this.props.detail.yearid;

        scheduleForm.schdEphysaddress = schedule.f1040Line1PhysicalAddress;
        scheduleForm.schdEproptype = schedule.f1040Line2bTypeOfProperty;
        scheduleForm.schdEdepreciation =
          schedule.f1040Line18DepreciationExpense;

        scheduleForms.push(scheduleForm);
      });
    }

    return scheduleForms;
  };

  onSetScheduleE = user => {
    let schedules = [];

    let scheduleForms = this.returnScheduleE(user);

    if (!arrayNullOrEmpty(scheduleForms)) {
      scheduleForms.map(scheduleForm => {
        let newSchedule = {};
        newSchedule.id = scheduleForm.sefinancialformsid;
        newSchedule.f1040Line1PhysicalAddress = scheduleForm.schdEphysaddress;
        newSchedule.f1040Line2bTypeOfProperty = scheduleForm.schdEproptype;
        newSchedule.f1040Line18DepreciationExpense =
          scheduleForm.schdEdepreciation;

        schedules.push(newSchedule);
      });
    }

    return schedules;
  };

  onSaveScheduleF = (schedules, user) => {
    let scheduleForms = [];
    if (!arrayNullOrEmpty(schedules)) {
      schedules.map(schedule => {
        let scheduleForm = user.forms.filter(
          f =>
            f.financialformtype === 668020007 &&
            f.yearid === this.props.detail.yearid &&
            f.sefinancialformsid === schedule.id
        );

        scheduleForm = arrayNullOrEmpty(scheduleForm) ? {} : scheduleForm[0];
        scheduleForm.sefinancialformsid = schedule.id;
        scheduleForm.financialformtype = 668020007;
        scheduleForm.filertype = user.filterTypeValue;
        scheduleForm.yearid = this.props.detail.yearid;

        scheduleForm.schdFdepreciation = schedule.f1040Line14Depreciation;

        scheduleForms.push(scheduleForm);
      });
    }

    return scheduleForms;
  };

  onSetScheduleF = user => {
    let schedules = [];
    let scheduleForms = user.forms.filter(
      f =>
        f.financialformtype === 668020007 &&
        f.yearid === this.props.detail.yearid
    );

    if (!arrayNullOrEmpty(scheduleForms)) {
      scheduleForms.map(scheduleForm => {
        let newSchedule = {};
        newSchedule.id = scheduleForm.sefinancialformsid;
        newSchedule.f1040Line14Depreciation = scheduleForm.schdFdepreciation;

        schedules.push(newSchedule);
      });
    }

    return schedules;
  };

  returnCurrentForm = () => {
    return cloneDeep(this.state.currentSavedFormReference);
  };

  returnFormAndChildrenIdsToBeDeleted = () => {
    let formsToBeDeleted = [];
    let detailId = this.props.detail.detailId;
    if (this.props.financials) {
      let formsArray = this.props.financials[detailId];
      if (!arrayNullOrEmpty(formsArray)) {
        formsArray.map(item => {
          if (item.financialformtype !== 668020011) {
            formsToBeDeleted.push(item.sefinancialformsid);
          }
        });
      }
    }
    return formsToBeDeleted;
  };

  updateAfterDelete = () => {
    let users = cloneDeep(this.state.users);
    users.map(u => {
      u.forms = [];
      u.is1040 = false;
      u.is1099 = false;
      u.willApply = false;
    });
    this.setState({ users });
  };

  updateFormsToBeDeleted = array => {
    //It updates the formsToBeDeleted that contains all ids that will be deleted
    this.setState(prevState => {
      return { formsToBeDeleted: [...prevState.formsToBeDeleted, ...array] };
    });
  };

  getFormsOptions = (form, year) => {
    form[0][1] = `${this.props.selectedYear - 1} IRS Form 1040`;
    form[1][1] = `${this.props.selectedYear - 1} IRS Form 1040EZ`;
    form[2][1] = `${this.props.selectedYear - 1} IRS Form 1040A`;
    form[3][1] = `Did not file taxes in ${this.props.selectedYear - 1}`;
    if (year - 1 > 2017) {
      form.splice(1, 2);
    }
    return form;
  };

  deleteFinancialForms = async formsToBeDeletedArray => {
    //It will get all Id on state formsToBeDeleted and delete for each iteration.
    if (this.state.selectedUser) {
      let userForms = this.state.selectedUser.forms;
      if (!arrayNullOrEmpty(formsToBeDeletedArray)) {
        let promises = formsToBeDeletedArray.map(async (o, i) => {
          let form = userForms.filter(item => {
            return item.sefinancialformsid === o;
          })[0];
          if (form) {
            await deleteSeniorApplicationsFinancial(
              o,
              this.props.detail.detailId
            );
          }
        });
        await Promise.all(promises);
        this.updateAfterDelete();
      }
    }
  };

  createOrUpdateFinancialForm = async financialForm => {
    if (financialForm.sefinancialformsid) {
      financialForm.seappdetailid = this.props.detail.detailId;
      financialForm.yearid = this.props.detail.yearid;
      //adding occupantid property
      if (this.state.selectedUser.filterTypeValue === 668020003) {
        financialForm.occupantid = this.state.selectedUser.id;
      }
      await updateSeniorAppFinancial(financialForm);
    } else {
      financialForm.seappdetailid = this.props.detail.detailId;
      financialForm.yearid = this.props.detail.yearid;
      financialForm.sefinancialformsid = uuid.v4();
      //adding occupantid property
      if (this.state.selectedUser.filterTypeValue === 668020003) {
        financialForm.occupantid = this.state.selectedUser.id;
      }
      await createSeniorAppFinancial(financialForm);
    }
  };

  render() {
    let forms = cloneDeep(this.state.formsOptions);
    forms = this.getFormsOptions(forms, this.props.selectedYear);

    return (
      <CollectionConsumer>
        {value => (
          // <TabContainer>

          <div>
            <Collapse in={this.state.financesMain}>
              {renderIf(
                this.props.detail,
                <React.Fragment>
                  {renderIf(
                    false,
                    <Card className="cardStyle">
                      <CardContent>
                        <div style={{ marginTop: '42px' }}>
                          <span
                            style={{ fontSize: '20px', fontWeight: 'bold' }}
                          >
                            <FormattedMessage
                              id="household_income"
                              defaultMessage="${income} household income"
                              values={{ income: this.props.detail.totalIncome }}
                            />
                          </span>
                        </div>
                      </CardContent>
                    </Card>
                  )}
                  <Card>
                    <div>
                      <CardSide
                        header={fm.individualFinancialInfoHeader}
                        content={[
                          <FormattedMessage
                            id="selectFinancialInfo"
                            defaultMessage="Select the Add financial info button after your name to add income and expense information for yourself."
                          />,
                          <FormattedMessage
                            id="ifAnotherPersonFinancialInfo"
                            defaultMessage="If you did not file a joint tax return for the people listed here, select the Add financial info button after their name to add income and expense information for them."
                            values={{
                              previousYear: this.props.selectedYear,
                            }}
                          />,
                        ]}
                      ></CardSide>
                    </div>
                    <CardContent className="cardStyleFinancialInfo">
                      <div className="financialHeader">
                        <div
                          style={{
                            font: 16,
                            marginLeft: '20px',
                            textAlign: 'left',
                          }}
                        >
                          <Grid container spacing={20}>
                            <Grid sm={12}>
                              <Grid
                                container
                                direction="row"
                                justify="flex-start"
                              >
                                <Grid sm={2}>
                                  <span
                                    style={{
                                      fontSize: '1.5em',
                                      fontWeight: '600',
                                    }}
                                  >
                                    {this.props.selectedYear}
                                  </span>
                                </Grid>
                                <Grid sm={10}>
                                  <FormattedMessage
                                    id="infoExemption"
                                    defaultMessage="For the {currentYear} exemption, please provide information about your finances and expenses from {lastYear}."
                                    values={{
                                      lastYear: this.props.selectedYear,
                                      currentYear: this.props.selectedYear,
                                    }}
                                  />
                                </Grid>
                              </Grid>
                            </Grid>
                          </Grid>
                        </div>
                      </div>
                      {renderIf(
                        totalHouseIncome !== null,
                        <Grid>
                          <TotalIncome
                            globalIncome={true}
                            totalFormsIncome={totalHouseIncome}
                          />
                        </Grid>
                      )}
                      <div>
                        {renderIf(
                          this.state.users.length > 1,
                          <Grid container direction="row">
                            <Grid sm={12}>
                              <div style={{ margin: '30px 0 50px 0' }}>
                                {this.state.users.map((u, index) => {
                                  return (
                                    <React.Fragment>
                                      <div
                                        key={u.id}
                                        style={{ padding: '0 10px' }}
                                      >
                                        <Grid
                                          container
                                          direction="row"
                                          spacing={0}
                                          justify="flex-start"
                                        >
                                          <Grid sm={2}>
                                            {renderIf(
                                              u.willApply,
                                              <p
                                                style={{
                                                  color: '#a5c727',
                                                  marginRight: '20px',
                                                }}
                                              >
                                                <CheckIcon />
                                              </p>
                                            )}
                                          </Grid>
                                          <Grid sm={5}>
                                            <p style={{ marginRight: '40px' }}>
                                              {u.fullName}
                                            </p>
                                          </Grid>
                                          <Grid sm={2}>
                                            {renderIf(
                                              u.willApply,
                                              <CustomButton
                                                testid={'button' + index}
                                                secondaryThin={true}
                                                btnBigLabel={false}
                                                onClick={() =>
                                                  this.addFinancialInfoButton(u)
                                                }
                                                label={
                                                  <FormattedMessage
                                                    id="edit_financial_info"
                                                    defaultMessage="Edit financial info"
                                                  />
                                                }
                                              />
                                            )}
                                            {renderIf(
                                              !u.willApply,
                                              <CustomButton
                                                testid={'button' + index}
                                                secondaryThin={true}
                                                btnBigLabel={false}
                                                onClick={() =>
                                                  this.addFinancialInfoButton(u)
                                                }
                                                label={
                                                  <FormattedMessage
                                                    id="add_financial_info"
                                                    defaultMessage="Add financial info"
                                                  />
                                                }
                                              />
                                            )}
                                          </Grid>
                                          <Grid sm={12}>
                                            {renderIf(
                                              this.state.users.length > 1 &&
                                                index <
                                                  this.state.users.length - 1,
                                              <div
                                                style={{
                                                  borderTop:
                                                    'solid #7a7a7a 2px',
                                                  margin: '30px 0 30px 0',
                                                  width: '100%',
                                                }}
                                              ></div>
                                            )}
                                          </Grid>
                                        </Grid>
                                      </div>
                                    </React.Fragment>
                                  );
                                })}
                              </div>
                            </Grid>
                          </Grid>
                        )}
                      </div>
                    </CardContent>
                  </Card>
                </React.Fragment>
              )}
              <div className="continue-summary-panel">
                {renderIf(
                  this.canContinue(),
                  <CustomButton
                    testid={'mainContinue'}
                    style={{ marginBottom: 30 }}
                    disabled={!this.canContinue()}
                    onClick={this.handleContinueClick}
                    label={fm.continueLabel}
                  />
                )}
              </div>
            </Collapse>

            <Collapse in={this.state.Forms}>
              <CardSide
                header={`${this.props.selectedYear - 1} info for ${
                  this.props.selectedYear
                }?`}
                content={[
                  <FormattedMessage
                    id="infoExemption"
                    defaultMessage="For the {currentYear} exemption, please provide information about your finances and expenses from {lastYear}."
                    values={{
                      lastYear: this.props.selectedYear - 1,
                      currentYear: this.props.selectedYear,
                    }}
                  />,
                ]}
              ></CardSide>
              {renderIf(
                this.state.Form2020,
                <Form2020
                  uploadsData={this.props.uploadsData}
                  setUpload={this.props.setUpload}
                  signalRMessages={this.props.signalRMessages}
                  sendMessage={this.props.sendMessage}
                  calculateFormsAmounts={this.props.calculateFormsAmounts}
                  userName={this.state.username}
                  totalHouseHoldIncome={this.state.totalHouseHoldIncome}
                  createOrUpdateFinancialForm={this.createOrUpdateFinancialForm}
                  selectedUser={this.state.selectedUser}
                  users={this.state.users}
                  handleContinueClickFinancialInfoYear={
                    this.handleContinueClick
                  }
                  contact={this.props.contact}
                  deleteFinancialForms={this.deleteFinancialForms}
                  updateAfterDelete={this.updateAfterDelete}
                  selectedYear={this.props.selectedYear}
                  object={this.state.myForm1099}
                  handleBack={this.backButtonForm}
                  getFilesMetadata={this.props.getFilesMetadata}
                  getSeniorDetails={this.props.getSeniorDetails}
                  getSeniorFinancials={this.props.getSeniorFinancials}
                  detail={this.props.detail}
                  seniorApp={this.props.seniorApp}
                  user={this.state.selectedUser}
                  financialFormTypes={value.financialFormTypes}
                  medicareOrganizations={value.medicareOrganizations}
                  medicarePlans={this.props.medicarePlans}
                  onSaveExpenses={this.onSaveExpenses}
                  onSetExpenses={this.onSetExpenses}
                  filesMetadata={this.props.filesMetadata}
                  showSavingOverlay={this.props.showSavingOverlay}
                  hideSavingOverlay={this.props.hideSavingOverlay}
                  details={this.props.details}
                  disabilitySources={value.disabilityIncomeSources}
                  returnCurrentForm={this.returnCurrentForm}
                  returnFormAndChildrenIdsToBeDeleted={
                    this.returnFormAndChildrenIdsToBeDeleted
                  }
                  currentForm={this.state.currentSavedFormReference}
                />
              )}
            </Collapse>
            <div style={{ margin: '0 0 15% 0' }}>
              {renderIf(
                this.state.showBackButton,
                <div style={{ marginTop: '30px' }}>
                  <CustomButton
                    style={{
                      margin: '10px 0',
                    }}
                    secondary={true}
                    btnBigLabel={true}
                    onClick={this.backButtonForm}
                    label={
                      <FormattedMessage
                        id="back_financial_info"
                        defaultMessage="Back"
                      />
                    }
                  />
                </div>
              )}
            </div>
          </div>
        )}
      </CollectionConsumer>
    );
  }
}
