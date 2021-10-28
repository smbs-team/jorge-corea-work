import React, {
  id,
  label,
  checked,
  onChange,
  style,
  information,
  readOnly,
} from 'react';
import './SwitchInput.css';
import Switch from 'react-switch';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import { Grid } from '@material-ui/core';
import { FormattedMessage } from 'react-intl';

const styles = theme => ({
  root: {
    marginTop: '0px',
    textAlign: 'left',

    [theme.breakpoints.up('sm')]: {
      fontSize: '1rem',
    },
    [theme.breakpoints.down('xs')]: {
      fontSize: '0.75rem',
      maxWidth: '400px',
    },
  },
  label: {
    display: 'inline-block',
    textAlign: 'left',
    verticalAlign: 'top',
  },
  slider: {
    textAlign: 'right',
    paddingRight: 16,
    verticalAlign: 'top',
  },
  infoIcon: {
    display: 'inline-block',
    verticalAlign: 'bottom',
    marginBottom: '-12px',
    marginLeft: '-8px',
  },
});

class SwitchInput extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      infoOpen: false,
    };

    this.handleClickOpen = this.handleClickOpen.bind(this);
    this.handleClose = this.handleClose.bind(this);
  }

  render() {
    const { classes } = this.props;

    return (
      <Grid
        container
        justify="flex-end"
        alignItems="top"
        className={classes.root}
        style={this.props.style}
      >
        <Grid item xs={4} sm={3} className={classes.slider}>
          <Switch
            id={this.props.id}
            data-ischecked={this.props.checked}
            data-testid={this.props.testid}
            checked={this.props.checked}
            onChange={this.props.onChange}
            onColor="#d4e693"
            onHandleColor="#a5c727"
            handleDiameter={20}
            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
            height={28}
            width={63}
            className="react-switch switch-input-icon"
            disabled={this.props.readOnly}
            uncheckedIcon={
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center',
                  height: '100%',
                  fontSize: 14,
                  color: 'white',
                  paddingRight: 2,
                }}
              ></div>
            }
            checkedIcon={
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center',
                  height: '100%',
                  fontSize: 14,
                  color: 'black',
                  paddingLeft: 3,
                }}
              ></div>
            }
          />
          <div className="switch-states">
            <FormattedMessage id="no" defaultMessage="No" />
            <span className="switch-yes">
              <FormattedMessage id="yes" defaultMessage="Yes" />
            </span>
          </div>
        </Grid>
        <Grid item xs={8} sm={9} className={classes.label}>
          {this.props.label}
        </Grid>
        {this.InformationAvailable()}
      </Grid>
    );
  }

  InformationAvailable() {
    const { classes } = this.props;
    if (this.props.information != null) {
      return (
        <div className={classes.infoIcon}>
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

  handleClickOpen() {
    this.setState({ infoOpen: true });
  }

  handleClose() {
    this.setState({ infoOpen: false });
  }
}

SwitchInput.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(SwitchInput);
