//-----------------------------------------------------------------------
// <copyright file="FinancialInfo.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React from 'react';
import './FinancialInfo.css';
import * as fm from './FormatTexts';
import ReactToPrint from 'react-to-print';
import TabContainer from '../common/TabContainer';
import IncomeHelp from '../income-help/IncomeHelp';
import CustomButton from '../common/CustomButton';
import SelectInput from '../common/SelectInput';
import UploadFile from '../common/UploadFile/UploadFile';
import HelpText from '../common/HelpText';
import Grid from '@material-ui/core/Grid';
import Collapse from '@material-ui/core/Collapse';
import SummaryButton from '../common/SummaryButton';
import SwitchInput from '../common/SwitchInput';
import FootSquareInput from '../common/FootSquareInput';
import ExpandLink from '../common/ExpandLink';
import KeyboardArrowRightIcon from '@material-ui/icons/KeyboardArrowRight';
import KeyboardArrowLeftIcon from '@material-ui/icons/KeyboardArrowLeft';
import { FormattedMessage } from 'react-intl';
import { withStyles } from '@material-ui/core/styles';
import VimeoPlayer from '../common/vimeo-player/VimeoPlayer';
import ShortDivider from '../common/ShortDivider';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import DialogContentText from '@material-ui/core/DialogContentText';
import { cloneDeep } from 'lodash';

import {
  CollectionConsumer,
  CollectionContext,
} from '../../contexts/CollectionContext';
import ToolbarItem from './ToolbarItem';
import { arrayNullOrEmpty, renderIf } from '../../lib/helpers/util';
import deepEqual from 'deep-equal';
import FinancialInfoYear from './FinancialInfoYear';
import deleteSeniorApplicationsFinancial from '../../services/dataServiceProvider';
import { Tabs, Card, Hidden } from '@material-ui/core';
import AlertPopUpYears from '../common/AlertPopUpYears';

const DialogCSS = withStyles({
  root: {
    '& .MuiDialog-paperWidthSm': {
      maxWidth: '1078px',
      width: '100%',
      height: 'auto',
    },
  },
})(Dialog);

let filteredYear = '';

class FinancialInfo extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      details: [],
      section: 'Financial Info',
      selectedDetail: null,
      infoOpen: true,
      hideToolbar: false,
      hideEnoughMonths: false,
    };
  }

  handleClickOpen = () => {
    this.setState({ infoOpen: true });
  };

  handleClose = () => {
    this.setState({ infoOpen: false });
  };

  componentDidMount = () => {
    window.scrollTo(0, 0);

    if (!arrayNullOrEmpty(this.props.seniorAppDetails)) {
      this.setSeniorAppDetails(this.props.seniorAppDetails);
    }

    this.props.setVideoCode(this.props.defaultHelpVideoUrl);
    //this.props.callHelpVideo(null, '380143715');
    //this.checkIfVideoPlayed();
  };

  checkIfVideoPlayed = () => {
    this.props.checkIfVideoPlayed(this.state.section).then(data => {
      if (!data) {
        this.props.callHelpVideo();
      }
    });
  };
  componentWillUnmount() {}
  componentDidUpdate = (prevProps, prevState) => {
    if (
      this.props.seniorAppDetails &&
      !deepEqual(this.props.seniorAppDetails, prevProps.seniorAppDetails)
    ) {
      this.setSeniorAppDetails(this.props.seniorAppDetails);
    }
  };

  setSeniorAppDetails = details => {
    if (!arrayNullOrEmpty(details)) {
      let yearArray = [];
      let year = details[0].yearid;

      if (year) {
        yearArray = this.props.years.filter(y => y.yearid === year);

        let yearObject = yearArray[0];
        let detailObject = details[0];

        let detailsArray = [];
        if (yearObject) {
          detailsArray.push({
            detailId: detailObject.seappdetailid,
            yearid: detailObject.yearid,
            year: yearObject.name,
            yearLoc: yearObject.locId,
            yearValue: yearObject.name,
            totalIncome: detailObject.totalincome,
            totalExpenses: detailObject.totalexpenses,
            netTotalIncome: detailObject.nettotalincome,
            wantToApply: detailObject.docirs1040 || details[0].docirs1099,
            accountNumber: detailObject.accountnumber,
            parcelId: detailObject.parcelid,
            isSelected: false,
            hasEnoughMonthsResidency: true,
          });
        }

        // By default first date is selected
        if (!this.state.selectedDetail) {
          if (detailsArray[0]) {
            detailsArray[0].isSelected = true;
          }
        }

        this.setState({
          details: detailsArray,
          selectedDetail: detailsArray[0],
        });
      }
    }

    /*let financialPreviousYear = this.props.selectedYear;
    let financialYearId = '';

    if (financialPreviousYear && financialPreviousYear > 1) {
      financialYearId = this.props.years.filter(item => {
        return item.name == financialPreviousYear;
      })[0].yearid;

      filteredYear = financialYearId;
    }*/
  };

  /*setSeniorAppDetails = details => {   
    if (!arrayNullOrEmpty(details)) {
      let detailsArray = [];
      details.map(d => {
        let year = this.props.years.filter(y => y.yearid === d.yearid)[0];
        detailsArray.push({
          detailId: d.seappdetailid,
          yearid: d.yearid,
          year: year,
          //yearLoc: year.locId,
          //yearValue: parseInt(year.name),
          yearLoc: '2016',
          yearValue: 2016,
          totalIncome: d.totalincome,
          totalExpenses: d.totalexpenses,
          netTotalIncome: d.nettotalincome,
          wantToApply: d.docirs1040 || d.docirs1099,
          accountNumber: d.accountnumber,
          parcelId: d.parcelid,
          isSelected: false,
          hasEnoughMonthsResidency: true,
        });
      });

      // Sort ascending by yearValue
      detailsArray.sort((a, b) => a.yearValue - b.yearValue);

      // By default first date is selected
      if (!this.state.selectedDetail) {
        detailsArray[0].isSelected = true;
        this.setState({
          details: detailsArray,
          selectedDetail: detailsArray[0],
        });
      }
    }
  };*/

  hideToolbar = hide => {
    this.setState({ hideToolbar: hide });
    this.props.hideToolbar(hide);
  };

  handleWantToApply = (detailId, checked) => {
    let details = [...this.state.details];
    details.map(d => {
      if (d.detailId === detailId) {
        d.wantToApply = checked;
      }
    });

    this.setState({ details });
  };

  handleYearClick = id => {
    if (!arrayNullOrEmpty(this.state.details)) {
      let selectedDetail = null;
      let details = cloneDeep(this.state.details);
      details.map(d => {
        if (d.detailId === id) {
          d.isSelected = true;
          selectedDetail = d;
        } else {
          d.isSelected = false;
        }
      });

      this.setState({ details, selectedDetail });
    }

    window.scrollTo(0, 0);
  };

  getSelectedDetail = () => {
    return this.state.details.filter(d => d.isSelected)[0];
  };

  handleHasEnoughMonthsResidencyChange = (checked, e, id) => {
    let selectedDetail = this.state.selectedDetail;
    selectedDetail.hasEnoughMonthsResidency = checked;
    this.setState({ selectedDetail });
  };

  handleHideEnoughMonths = hide => {
    this.setState({
      hideEnoughMonths: hide,
    });
  };

  deleteScheduleObjects = () => {};

  render() {
    /* added output array for filter, must check final values*/
    let flags = [];
    let filteredArray = [];
    let i;
    let array = this.state.details;
    let l = array.length > 4 ? 4 : array.length;
    for (i = 0; i < l; i++) {
      if (flags[array[i].yearValue]) continue;
      flags[array[i].yearValue] = true;

      filteredArray.push(array[i]);
    }
    /* END OF FILTERED ARRAY */
    return (
      <CollectionConsumer>
        {value => (
          <TabContainer>
            <div style={{ maxWidth: '631px', margin: 'auto' }}>
              <div>
                {renderIf(
                  !arrayNullOrEmpty(this.state.details),
                  <div style={{ margin: '0 auto', maxWidth: '631px' }}>
                    <Collapse
                      in={
                        this.state.selectedDetail &&
                        this.state.selectedDetail.hasEnoughMonthsResidency
                      }
                    >
                      <Hidden smUp>
                        <div className="card-separator"></div>
                      </Hidden>
                      <FinancialInfoYear
                        uploadsData={this.props.uploadsData}
                        setUpload={this.props.setUpload}
                        signalRMessages={this.props.signalRMessages}
                        sendMessage={this.props.sendMessage}
                        disabilitySources={value.disabilityIncomeSources}
                        calculateFormsAmounts={this.props.calculateFormsAmounts}
                        medicarePlans={
                          this.state.selectedDetail
                            ? value.medicarePlans.filter(item => {
                                return (
                                  item.yearId ===
                                    this.state.selectedDetail.yearid ||
                                  item.defaultLocString === 'None'
                                );
                              })
                            : []
                        }
                        handleHideEnoughMonths={this.handleHideEnoughMonths}
                        details={this.props.seniorAppDetails}
                        detail={this.state.selectedDetail}
                        handleWantToApply={this.handleWantToApply}
                        seniorApp={this.props.seniorApp}
                        contact={this.props.contact}
                        filesMetadata={this.props.filesMetadata}
                        getFilesMetadata={this.props.getFilesMetadata}
                        updateFilesMetadataState={
                          this.props.updateFilesMetadataState
                        }
                        selectedYear={this.props.selectedYear}
                        occupants={this.props.occupants}
                        hideToolbar={this.props.hideToolbar}
                        financials={this.props.financials}
                        getSeniorFinancials={this.props.getSeniorFinancials}
                        getSeniorDetails={this.props.getSeniorDetails}
                        hideToolbar={this.hideToolbar}
                        editMode={this.props.editMode}
                        readOnlyMode={this.props.readOnlyMode}
                        navigateToSummary={this.props.navigateToSummary}
                        nextTab={this.props.nextTab}
                        showSavingOverlay={this.props.showSavingOverlay}
                        hideSavingOverlay={this.props.hideSavingOverlay}
                        formTypes={value.financialFormTypes}
                        years={this.props.years}
                        setHtmlSummary={this.props.setHtmlSummary}
                      />
                    </Collapse>
                  </div>
                )}
              </div>
            </div>
          </TabContainer>
        )}
      </CollectionConsumer>
    );
  }
}

export default FinancialInfo;
