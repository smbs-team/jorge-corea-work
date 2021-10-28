import React, {
  name,
  value,
  helperText,
  onChange,
  label,
  information,
  style,
  readOnly,
} from 'react';
import './TextInputML.css';
import TextField from '@material-ui/core/TextField';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';

const styles = theme => ({
  root: {
    marginTop: '8px',
    textAlign: 'left',
  },
});

const CssTextField = withStyles(theme => ({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiFormLabel-root': {
      [theme.breakpoints.up('sm')]: {
        fontSize: '20px',
      },
      [theme.breakpoints.down('xs')]: {
        fontSize: '16px',
      },
    },
    '& .MuiInputBase-input': {
      width: '320px',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
    '& .MuiInput-underline:before': {
      borderBottomColor: '#000000',
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
  },
}))(TextField);

class TextAreaML extends React.Component {
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

  render() {
    const { classes } = this.props;
    return (
      <div className={classes.root} style={this.props.style}>
        <CssTextField
          name={this.props.name}
          id="standard-dense"
          value={this.props.value}
          label={this.props.label}
          multiline={true}
          rows={2}
          margin="dense"
          helperText={this.props.helperText}
          onChange={this.props.onChange}
          InputProps={{
            readOnly: this.props.readOnly,
          }}
        ></CssTextField>

        {this.InformationAvailable()}
      </div>
    );
  }
}

TextAreaML.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(TextAreaML);
