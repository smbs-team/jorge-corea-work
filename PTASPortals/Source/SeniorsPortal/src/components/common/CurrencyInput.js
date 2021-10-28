import React, {
  name,
  value,
  onChange,
  style,
  wrapLabel,
  label,
  information,
  readOnly,
} from 'react';
import './CurrencyInput.css';
import { withStyles, makeStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import InputAdornment from '@material-ui/core/InputAdornment';
import HelpOutline from '@material-ui/icons/HelpOutline';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';

import PropTypes from 'prop-types';
import MaskedInput from 'react-text-mask';
import NumberFormat from 'react-number-format';
import TextField from '@material-ui/core/TextField';
import CurrencyFormat from 'react-currency-format';

const NumberFormatCss = withStyles({
  root: {
    '& label': {
      fontSize: '1.25em',
      //width: '500px',
      width: '250px',
      marginTop: '-10px',
    },
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
    '& .MuiInput-formControl': {
      width: '128px',
    },
  },
})(NumberFormat);

export default class CurrencyInput extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      infoOpen: false,
    };

    this.handleClickOpen = this.handleClickOpen.bind(this);
    this.handleClose = this.handleClose.bind(this);
  }

  handleClickOpen() {
    this.setState({ infoOpen: true });
  }

  handleClose() {
    this.setState({ infoOpen: false });
  }

  InformationAvailable() {
    if (this.props.information != null) {
      return (
        <div className="info-icon">
          <IconButton onClick={this.handleClickOpen}>
            <HelpOutline />
          </IconButton>
          <Dialog
            open={this.state.infoOpen}
            onClose={this.handleClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogActions>
              <IconButton onClick={this.handleClose}>
                <ClearIcon style={{ width: '2em', height: '2em' }} />
              </IconButton>
            </DialogActions>
            <DialogContent style={{ marginTop: '-50px', marginBottom: '20px' }}>
              <div className="info-icon-alert" style={{ marginRight: '27px' }}>
                <HelpOutline style={{ width: '2em', height: '2em' }} />
              </div>
              <div className="info-icon-alert" style={{ width: '75%' }}>
                <DialogContentText
                  id="alert-dialog-description"
                  style={{ color: 'black' }}
                >
                  {this.props.information}
                </DialogContentText>
              </div>
            </DialogContent>
          </Dialog>
        </div>
      );
    }
  }

  displayInformation(e) {
    alert(this.props.information);
  }

  render() {
    if (this.props.wrapLabel == 'true') {
      return (
        <div style={{ textAlign: 'left' }}>
          <NumberFormatCss
            customInput={TextField}
            name={this.props.name}
            label={this.props.label}
            value={this.props.value}
            onChange={this.props.onChange}
            thousandSeparator={true}
            fixedDecimalScale={true}
            allowNegative={this.props.allowNegative || false}
            decimalScale={2}
            prefix={'$'}
            placeholder={'$'}
            InputLabelProps={{
              shrink: true,
              style: {
                width: '300px',
              },
            }}
            InputProps={{
              style: {
                marginTop: '30px',
              },
              readOnly: this.props.readOnly,
            }}
            inputProps={{ 'data-testid': this.props.testid }}
          />
          {this.InformationAvailable()}
        </div>
      );
    } else {
      return (
        <div style={{ textAlign: 'left' }}>
          <NumberFormatCss
            customInput={TextField}
            name={this.props.name}
            label={this.props.label}
            value={this.props.value}
            onChange={this.props.onChange}
            thousandSeparator={true}
            fixedDecimalScale={true}
            decimalScale={2}
            allowNegative={this.props.allowNegative || false}
            prefix={'$'}
            placeholder={'$'}
            InputLabelProps={{
              shrink: true,
            }}
            InputProps={{
              readOnly: this.props.readOnly,
            }}
            inputProps={{ 'data-testid': this.props.testid }}
          />
          {this.InformationAvailable()}
        </div>
      );
    }
  }
}
