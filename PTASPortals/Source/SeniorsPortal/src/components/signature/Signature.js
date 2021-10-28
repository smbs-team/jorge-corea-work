import React from 'react';
import './Signature.css';
import { FormattedMessage } from 'react-intl';
import TabContainer from '../common/TabContainer';
import TextInputML from '../common/TextInputML';
import CustomButton from '../common/CustomButton';
import SwitchInput from '../common/SwitchInput';
import TextField from '@material-ui/core/TextField';
import { withStyles } from '@material-ui/core/styles';
import deepEqual from 'deep-equal';
import * as fm from './FormatTexts';

const TextFieldCss = withStyles({
  root: {
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
      '&.Mui-focused fieldset': {
        borderColor: '#a5c727',
      },
      width: '299px',
    },
    '& .MuiInput-formControl': {
      width: '299px',
    },
  },
})(TextField);

const SwitchInputCss = withStyles({
  root: {
    marginTop: '28px',
  },
  label: {
    maxWidth: '310px',
    fontSize: '12px',
  },
})(SwitchInput);

const CustomButtonCss = withStyles({
  button: {
    marginTop: '49px',
  },
})(CustomButton);

class Signature extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      continueText: fm.continueLabel,
      isSaving: false,
      name: '',
      date: this.formatDateString(new Date()),
      twoWitnessesConfirmation: false,
    };
  }

  componentDidMount() {
    window.scrollTo(0, 0);

    this.setSeniorApp(this.props.seniorApp);
  }
  componentWillUnmount() {}

  componentDidUpdate(prevProps, prevState) {
    if (
      this.props.seniorApp &&
      !deepEqual(prevProps.seniorApp, this.props.seniorApp)
    ) {
      this.setSeniorApp(this.props.seniorApp);
    }
  }

  handleTextInputChange = e => {
    this.setState({ [e.target.name]: e.target.value });
  };

  handleSwitchInputChange = (checked, e, id) => {
    this.setState({ [id]: checked });
  };

  updateSeniorApp = async () => {
    let seniorApp = { ...this.props.seniorApp };

    seniorApp.signatureconfirmed = this.state.twoWitnessesConfirmation;
    seniorApp.signatureline = this.state.name;
    seniorApp.signaturedate = this.state.date;
    seniorApp.signaturesection = false;

    await this.props.updateSeniorApp(seniorApp);
  };

  setSeniorApp = seniorApp => {
    if (seniorApp) {
      this.setState({
        twoWitnessesConfirmation: seniorApp.signatureconfirmed,
        name: seniorApp.signatureline,
        date: this.formatDateString(seniorApp.signaturedate),
      });
    }
  };

  formatDateString = value => {
    let dateString = '';
    if (value) {
      let date = new Date(value);
      dateString = date.toLocaleDateString('en-US');
    } else {
      let date = new Date();
      dateString = date.toLocaleDateString('en-US');
    }
    return dateString;
  };

  handleContinueClick = async () => {
    window.scrollTo(0, 0);
    this.setState({
      isSaving: true,
      continueText: fm.savingLabel,
    });

    document.getElementById('saved').style.display = 'none';
    document.getElementById('saving').style.display = 'block';
    await this.updateSeniorApp();
    document.getElementById('saving').style.display = 'none';
    document.getElementById('saved').style.display = 'block';
    this.props.nextTab();
  };

  render() {
    return (
      <TabContainer>
        <div className="center-panel">
          <div className="align-left">
            <FormattedMessage
              id="signature_enterYourName"
              defaultMessage="To sign, enter your name"
            >
              {placeholder => (
                <TextFieldCss
                  name="name"
                  label={fm.yourSignature}
                  value={this.state.name}
                  placeholder={placeholder}
                  margin="normal"
                  variant="outlined"
                  onChange={this.handleTextInputChange}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              )}
            </FormattedMessage>
          </div>

          <TextInputML
            label={fm.date}
            value={this.state.date}
            style={{ width: '128px', marginTop: '22px' }}
          />

          <SwitchInputCss
            id="twoWitnessesConfirmation"
            label={fm.twoWitnessesConfirmation}
            checked={this.state.twoWitnessesConfirmation}
            onChange={this.handleSwitchInputChange}
            readOnly={this.props.readOnlyMode}
          />

          <CustomButtonCss
            disabled={
              !this.state.twoWitnessesConfirmation || this.state.isSaving
            }
            onClick={this.handleContinueClick}
            label={this.state.continueText}
          />
        </div>
      </TabContainer>
    );
  }
}

export default Signature;
