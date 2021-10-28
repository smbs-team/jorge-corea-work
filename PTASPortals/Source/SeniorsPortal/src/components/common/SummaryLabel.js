import React, { label, value } from 'react';
import { withStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import { FormattedMessage } from 'react-intl';
import PropTypes from 'prop-types';

const styles = theme => ({
  root: {
    marginTop: '25px',
    textAlign: 'left',
  },
  leftLabel: {
    textAlign: 'left',
    fontSize: '12px',
    minWidth: '120px',
    display: 'inline-block',
  },
  rightLabel: {
    textAlign: 'left',
    fontSize: '13px',
    display: 'inline-block',
  },
});

class SummaryLabel extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { classes } = this.props;
    return (
      <div className={classes.root}>
        <div className={classes.leftLabel}>
          <label>
            {this.props.label && <FormattedMessage id={this.props.label} />}{' '}
          </label>
        </div>
        <div className={classes.rightLabel}>
          <label>{this.props.value}</label>
        </div>
      </div>
    );
  }
}

SummaryLabel.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(SummaryLabel);
