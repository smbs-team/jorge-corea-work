import React from 'react';
import * as fm from './FormatTexts';

import { renderIf, arrayNullOrEmpty } from '../../../lib/helpers/util';
import { hasAgeToQualify } from '../../../lib/helpers/age';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import CardHeader from '@material-ui/core/CardHeader';

import { FormattedMessage } from 'react-intl';

import CustomButton from '../CustomButton';

import SelectInput from '../SelectInput';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';

import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import Grid from '@material-ui/core/Grid';
import FormControl from '@material-ui/core/FormControl';

import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import WarningOutlinedIcon from '@material-ui/icons/WarningOutlined';
import ErrorOutlinedIcon from '@material-ui/icons/ErrorOutlined';

class CustomDialog extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      continueText: fm.continueLabel,
      isSaving: false,
      isErrorM: false,
      isWarning: false,
    };
  }
  renderSwitch = param => {
    switch (param) {
      case 1:
        return <WarningOutlinedIcon className="clear-icon" />;
      case 2:
        return (
          <WarningOutlinedIcon
            style={{ color: 'red' }}
            className="clear-icon"
          />
        );

      default:
        return <InfoOutlinedIcon className="clear-icon" />;
    }
  };

  render() {
    const ITEM_HEIGHT = 48;
    console.log('CustomDialog', this);

    const ITEM_PADDING_TOP = 8;
    const MenuProps = {
      PaperProps: {
        style: {
          maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
          width: 250,
        },
      },
    };

    return (
      <Dialog
        open={this.props.showDialog}
        onClose={this.props.handleCloseContinue}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        hideBackdrop
        scroll="body"
        PaperProps={{ style: { border: '1px solid #d20000' } }}
      >
        <DialogActions>
          <IconButton onClick={this.props.handleCloseContinue}>
            <ClearIcon
              className="clear-icon"
              style={{
                width: '1.5em',
                height: '1.5em',
                color: 'black',
              }}
            />
          </IconButton>
        </DialogActions>
        <DialogContent style={{ marginTop: '-60px', marginBottom: '20px' }}>
          <div
            className="info-icon-alert"
            style={{
              margin: '3% 27px 0 1%',
              transform: 'scale(2)',
            }}
          >
            {this.renderSwitch(this.props.showDialogType)}
          </div>
          <div className="info-icon-alert" style={{ width: '75%' }}>
            <DialogContentText
              id="alert-dialog-description"
              style={{ color: 'red' }}
            >
              <p>
                {this.props.showDialogType === 2 && (
                  <FormattedMessage
                    id="youllneedsomeofthefollowing"
                    defaultMessage="To continue, you need to enter your information:"
                  />
                )}
                {this.props.showDialogType === 1 && (
                  <FormattedMessage
                    id="youmightneedsomeofthefollowing"
                    defaultMessage="You might need some of the following information to complete the application: "
                  />
                )}
              </p>
              <ul>
                {/* {this.props.showDialogDOBMSG && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="canContiinue_MyInfo_Dialog_Text"
                        defaultMessage="Date Of Birth required"
                      />
                    </li>
                  </React.Fragment>
                )} */}

                {/* {this.props.showDialogPOD && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="canContiinue_POB_Text"
                        defaultMessage="Proof of age file is required legal proof of disability"
                      />
                    </li>
                  </React.Fragment>
                )} */}
                {this.props.dateOfBirth && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_dateOfBirth"
                        defaultMessage="Date of birth"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.legalProofDisability && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_proofOfDisabilityDocument"
                        defaultMessage="proofOfDisabilityFiles"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.firstName && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_firstGivenName"
                        defaultMessage="firstName"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.lastName && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_lastName"
                        defaultMessage="lastName"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.emailAddress && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_emailAddress"
                        defaultMessage="emailAddress"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.phoneNumber && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_phoneNumber"
                        defaultMessage="phoneNumber"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.proofOfAgeDocument && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="myInfo_proofOfAgeDocument"
                        defaultMessage="proofOfAgeDocument"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.propertyId && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="property_propertyIdHt"
                        defaultMessage="propertyId"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.acquiredDate && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="property_acquiredDateHt"
                        defaultMessage="acquiredDate"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.proofOfOwnership && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="property_proofOwnership"
                        defaultMessage="proofOfOwnership"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.isPrimaryResidence && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="property_firstDatePrimaryResidence"
                        defaultMessage="firstDatePrimaryResidence"
                      />
                    </li>
                  </React.Fragment>
                )}
                {this.props.hasExperiencedMajorLifeChanges && (
                  <React.Fragment>
                    <li>
                      <FormattedMessage
                        id="property_proofMajorLifeChange"
                        defaultMessage="proofMajorLifeChange"
                      />
                    </li>
                  </React.Fragment>
                )}
              </ul>
            </DialogContentText>
          </div>
        </DialogContent>
      </Dialog>
    );
  }
}

export default CustomDialog;
