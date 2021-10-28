import React, {
  name,
  value,
  onChange,
  style,
  wrapLabel,
  label,
  information,
  placeholder,
  readOnly,
} from 'react';
import './CurrencyInput.css';
import { withStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import InputAdornment from '@material-ui/core/InputAdornment';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import { FormattedMessage } from 'react-intl';

const CssFormControl = withStyles({
  root: {
    '& label': {
      fontSize: '1.25em',
      width: '500px',
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
    '&.MuiFormControl-root': {
      display: 'inline',
    },

    display: 'inline',
  },
})(FormControl);

const CssInputAdornment = withStyles({
  root: {
    '& .MuiTypography-colorTextSecondary': {
      color: '#000000',
    },
  },
})(InputAdornment);

export default class CurrencyInput extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      infoOpen: false,
    };

    this.placeholder = this.props.placeholder;
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
    if (this.props.helperTextId != null) {
      return (
        <div className="info-icon">
          <IconButton onClick={this.handleClickOpen}>
            <InfoOutlinedIcon />
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
                <InfoOutlinedIcon style={{ width: '2em', height: '2em' }} />
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
    alert(this.props.helperText);
  }

  render() {
    if (this.props.wrapLabel == 'true') {
      return (
        <div style={{ textAlign: 'left' }}>
          <CssFormControl>
            <InputLabel
              htmlFor="adornment-amount"
              style={{ width: '310px' }}
              InputLabelProps={{
                //Show label and placeholder at the same time
                shrink: true,
              }}
            >
              {this.props.label}
            </InputLabel>
            <Input
              id="adornment-amount"
              name={this.props.name}
              value={this.props.value}
              onChange={this.props.onChange}
              style={{ marginTop: '30px' }}
              placeholder={this.props.placeholder}
              InputProps={{
                readOnly: this.props.readOnly,
              }}
            />
          </CssFormControl>
          {this.InformationAvailable()}
        </div>
      );
    } else {
      return (
        <div style={{ textAlign: 'left' }}>
          <CssFormControl>
            <InputLabel htmlFor="adornment-amount" shrink="true">
              {this.props.label}
            </InputLabel>

            <Input
              id="adornment-amount"
              name={this.props.name}
              value={this.props.value}
              onChange={this.props.onChange}
              placeholder="square feet"
              InputProps={{
                readOnly: this.props.readOnly,
              }}
            />
          </CssFormControl>
          {this.InformationAvailable()}
        </div>
      );
    }
  }
}
