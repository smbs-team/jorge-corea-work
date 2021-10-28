import React, {
  id,
  label,
  value,
  placeholder,
  onchange,
  style,
  information,
  shrinkLabel,
  readOnly,
} from 'react';
import './DateInput.css';
import 'date-fns';
import {
  makeStyles,
  withStyles,
  MuiThemeProvider,
  createMuiTheme,
} from '@material-ui/core/styles';
import DateFnsUtils from '@date-io/date-fns';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import {
  MuiPickersUtilsProvider,
  KeyboardTimePicker,
  KeyboardDatePicker,
} from '@material-ui/pickers';
import { renderIf } from '../../lib/helpers/util';

const CssKeyboardDatePicker = withStyles(theme => ({
  root: {
    '& label': {
      fontFamily: 'Open Sans',
      [theme.breakpoints.up('sm')]: {
        fontSize: '1.25rem',
      },
      [theme.breakpoints.down('xs')]: {
        fontSize: '1rem',
      },
    },
    '& input': {
      fontFamily: 'Open Sans',
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
      width: '150px',
      [theme.breakpoints.up('sm')]: {
        fontSize: '20px',
      },
      [theme.breakpoints.down('xs')]: {
        fontSize: '16px',
      },
    },
    '& .MuiSvgIcon-root': {
      fontSize: '20px',
      top: '5px',
      position: 'relative',
    },
    '& .MuiIconButton-root': {
      padding: 0,
    },
    '& .MuiFormLabel-root': {
      width: '320px',
      fontSize: '20px',
    },
    display: 'inline-block',
    fontFamily: 'Open Sans',
    day: {
      width: 4,
      height: 4,
      fontSize: 8,
      margin: '0 2px',
      color: 'green',
    },
  },
}))(KeyboardDatePicker);

export default class DateInput extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      infoOpen: false,
    };

    this.handleClickOpen = this.handleClickOpen.bind(this);
    this.handleClose = this.handleClose.bind(this);
  }

  handleDateChange(date) {
    this.props.onChange(date, this.props.id);
  }

  InformationAvailable() {
    const { classes } = this.props;
    if (this.props.information != null) {
      return (
        <div className="date-info-icon">
          <IconButton onClick={this.handleClickOpen} style={{ zoom: 0.8 }}>
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
    alert(this.props.information);
  }

  handleClickOpen() {
    this.setState({ infoOpen: true });
  }

  handleClose() {
    this.setState({ infoOpen: false });
  }

  render() {
    let readOnlyInput = this.props.readOnly ? { readOnly: true } : '';
    let calendarProps = this.props.readOnly
      ? {
          'aria-label': 'change date',
          disabled: 'true',
        }
      : {
          'aria-label': 'change date',
        };
    const defaultMaterialTheme = createMuiTheme({
      palette: {
        primary: { main: '#6f1f62' },
      },
      overrides: {
        MuiPickersDay: {
          daySelected: {
            backgroundColor: '#a5c727',
          },
        },
      },
    });

    return (
      <div className="date-input ">
        <MuiPickersUtilsProvider
          utils={DateFnsUtils}
          style={{ display: 'inline-block', width: 100 }}
        >
          <MuiThemeProvider theme={defaultMaterialTheme}>
            <CssKeyboardDatePicker
              id={this.props.id}
              name={this.props.name}
              margin="normal"
              label={this.props.label}
              placeholder={this.props.placeholder}
              onChange={this.handleDateChange.bind(this)}
              KeyboardButtonProps={calendarProps}
              value={this.props.value ? this.props.value : null}
              format={this.props.format ? this.props.format : 'MM/dd/yyyy'}
              InputLabelProps={{
                //Show label and placeholder at the same time
                shrink: this.props.shrinkLabel,
              }}
              style={this.props.style}
              readOnly={this.props.readOnly}
              inputProps={readOnlyInput}
              openTo={this.props.openTo ? this.props.openTo : 'date'}
              className="date-picker"
              views={this.props.yearOnly ? ['year'] : ['year', 'date', 'month']}
            />
          </MuiThemeProvider>
        </MuiPickersUtilsProvider>
        {this.InformationAvailable()}
      </div>
    );
  }
}
