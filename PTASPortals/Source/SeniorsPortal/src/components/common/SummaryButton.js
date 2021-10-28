import React, { label, style, disabled } from 'react';
import './CustomButton.css';
import Button from '@material-ui/core/Button';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/styles';

const styles = theme => ({
  button: {
    display: 'inline-block',
    marginLeft: '15px',
    backgroundColor: 'white',
    color: '#6f1f62',
    borderStyle: 'solid',
    borderWidth: '2px',
    borderColor: '#6f1f62',
    fontSize: '16px',

    textTransform: 'none',
    width: '220px',
    height: '48px',
    marginTop: '40px',
    '&:disabled': {
      color: '#ffffff',
      backgroundColor: '#9b6692',
    },
  },
  input: {
    display: 'none',
  },
});

class SummaryButton extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { classes } = this.props;
    if (this.props.disabled) {
      return (
        <Button
          disabled
          variant="outlined"
          className={classes.button}
          onClick={this.props.onClick}
          style={this.props.style}
        >
          {this.props.label}
        </Button>
      );
    } else {
      return (
        <Button
          variant="outlined"
          className={classes.button}
          onClick={this.props.onClick}
          style={this.props.style}
        >
          {this.props.label}
        </Button>
      );
    }
  }
}

SummaryButton.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(SummaryButton);
