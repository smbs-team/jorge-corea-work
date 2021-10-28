import React from 'react';
import './FinancialInfo.css';
import * as fm from './FormatTexts';
import Grid from '@material-ui/core/Grid';
import Collapse from '@material-ui/core/Collapse';
import HelpText from '../common/HelpText';
import SelectInput from '../common/SelectInput';
import CurrencyInput from '../common/CurrencyInput';
import UploadFile from '../common/UploadFile/UploadFile';
import { withStyles } from '@material-ui/core/styles';
import { FormattedMessage } from 'react-intl';
import { getUserType } from './financialHelper';
import CardSide from '../common/CardSide';
import VimeoSlider from '../common/vimeo-player/VimeoSlider';
import { Hidden } from '@material-ui/core';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';

const Expenses = props => {
  if (!props.object) {
    props.object = {};
    props.object.year = 0;
    props.object.assistedLiving = null;
    props.object.inHomeCare = null;
    props.object.nonReimbursedPrescriptions = null;
    props.object.approvedMedicare = null;
    props.object.medicareProvider = '';
    props.object.medicalPlan = '';
    props.object.expenseDocuments = [];
  } else {
  }

  const medicarePlans = props.medicarePlans.filter(
    m =>
      m.organizationName === props.object.medicareProvider ||
      m.defaultLocString === 'None'
  );

  return (
    <div className="center-panel form-content-max" style={{ paddingTop: 0 }}>
      <Card style={{ marginBottom: '20px' }} className="form-card">
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
                <Grid sm={12}>
                  <strong
                    style={{
                      paddingBottom: 5,
                      textAlign: 'left',
                    }}
                  >
                    <FormattedMessage
                      id="finances_qualifiedExpenses"
                      defaultMessage="Qualified expenses"
                    />
                  </strong>
                </Grid>
              </Grid>
            </div>
          </div>
        </Hidden>
        <div>
          <CardSide
            header={
              <FormattedMessage
                id="finances_qualifiedExpenses"
                defaultMessage="Qualified expenses"
              />
            }
            content={[
              <FormattedMessage
                id="finances_PleaseProvideDocuments"
                defaultMessage="Please provide documents for each qualified expense."
              />,
              <div className="space-bottom-16"></div>,
              <b>
                <FormattedMessage
                  id="finances_AssistedLivingOrAdultFamilyHome"
                  defaultMessage="Assisted living or adult family home"
                />
              </b>,
              <FormattedMessage
                id="finances_AttachAnAnnualStatement"
                defaultMessage="Attach an annual statement, monthly receipts, canceled checks, or bank statements showing payments."
              />,
              <div className="space-bottom-16"></div>,
              <b>
                <FormattedMessage
                  id="finances_InHomeCare"
                  defaultMessage="In-home care and nursing home"
                />
              </b>,
              <FormattedMessage
                id="finances_AttachAnAnnualStatement"
                defaultMessage="Attach an annual statement, monthly receipts, canceled checks, or bank statements showing payments."
              />,
              <div className="space-bottom-16"></div>,
              <b>
                <FormattedMessage
                  id="finances_NonReimbursed"
                  defaultMessage="Non-reimbursed prescriptions"
                />
              </b>,
              <FormattedMessage
                id="finances_AttachAnInsurance"
                defaultMessage="Attach an end-of-year statement from your insurance provider or pharmacy or payment receipts."
              />,
              <div className="space-bottom-16"></div>,
              <b>
                <FormattedMessage
                  id="finances_ApprovedMedicare"
                  defaultMessage="Medicare insurance premiums"
                />
              </b>,
              <FormattedMessage
                id="finances_AttachAnBank"
                defaultMessage="Attach an end-of-year statement, bank statements showing payments, or receipts."
              />,
              <FormattedMessage
                id="finances_DentalPlans"
                defaultMessage="Dental plans, supplemental insurance plans, optical plans, and company insurance plans aren’t considered expenses for this program."
              />,
            ]}
          ></CardSide>
        </div>
        <CardContent className="center-panel form-content-max">
          <Grid
            container
            spacing={5}
            style={{ marginTop: '20px' }}
            className="grid-container forms-content-margin"
          >
            <Grid item xs={12} sm={6} className="grid">
              <CurrencyInput
                name="assistedLiving"
                label={fm.assistedLiving}
                information={fm.assistedLiving_ht}
                wrapLabel="true"
                value={props.object.assistedLiving}
                onChange={props.handleCurrencyInputChange}
                readOnly={props.readOnlyMode}
              />
            </Grid>
            <Grid item xs={12} sm={6} className="grid">
              <CurrencyInput
                name="inHomeCare"
                label={fm.inHomeCare}
                information={fm.inHomeCare_ht}
                value={props.object.inHomeCare}
                onChange={props.handleCurrencyInputChange}
                readOnly={props.readOnlyMode}
              />
            </Grid>
            <Grid
              item
              xs={12}
              sm={6}
              className="grid"
              style={{ marginTop: '2px' }}
            >
              <CurrencyInput
                name="nonReimbursedPrescriptions"
                label={fm.nonReimbursedPrescriptions}
                information={fm.nonReimbursedPrescriptions_ht}
                value={props.object.nonReimbursedPrescriptions}
                onChange={props.handleCurrencyInputChange}
                readOnly={props.readOnlyMode}
              />
            </Grid>
            <Grid item xs={12} sm={6} className="grid">
              <CurrencyInput
                name="approvedMedicare"
                label={fm.approvedMedicare}
                information={fm.approvedMedicare_ht}
                wrapLabel="true"
                value={props.object.approvedMedicare}
                onChange={props.handleCurrencyInputChange}
                readOnly={props.readOnlyMode}
              />
            </Grid>
            <Grid item xs={12} sm={12} style={{ marginTop: '2px' }}>
              <SelectInput
                name="medicareProvider"
                label={fm.medicareProvider}
                source={props.medicareOrganizations}
                value={props.object.medicareProvider}
                onChange={props.handleSelectChangeValue}
                sourceLabel="defaultLocString"
                sourceValue="name"
                readOnly={props.readOnlyMode}
                style={{ margin: '40px' }}
                classToApply={'select-input select-medical'}
              />

              <Collapse in={props.object.medicareProvider !== ''}>
                <SelectInput
                  name="medicalPlan"
                  label={fm.medicalPlan}
                  source={medicarePlans}
                  value={props.object.medicalPlan}
                  onChange={props.handleSelectChangeValue}
                  sourceLabel="defaultLocString"
                  sourceValue="id"
                  readOnly={props.readOnlyMode}
                  style={{ marginTop: '40px' }}
                  classToApply={'select-input select-medical'}
                />
              </Collapse>
            </Grid>
          </Grid>
          <div style={{ marginTop: '50px' }}>
            <div className="forms-content-margin-uploads">
              <p className="forms-uploaders-title">
                {fm.YearExpenseDoc(props.detail.yearValue - 1)}
              </p>
              <UploadFile
                id={props.uploadFileId}
                hideIcon={true}
                obscureInfoMessage={true}
                fileArray={props.fileArray}
                section={props.section}
                document={props.document}
                appId={props.seniorApp.seapplicationid}
                detailsId={props.detail.detailId}
                helpText={props.helpText}
                onCreate={props.onCreate}
                onDelete={props.onDelete}
                currentData={props.currentData}
                currDataName={props.currDataName}
                setParentState={props.setParentState}
              />
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default Expenses;
