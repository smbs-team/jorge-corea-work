import React, { label, value } from 'react';
import { withStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import { FormattedMessage } from 'react-intl';
import PropTypes from 'prop-types';

const styles = theme => ({
  root: {
    marginTop: '17px',
    textAlign: 'left',
  },
  leftLabel: {
    textAlign: 'left',
    fontSize: '12px',
    display: 'inline-block',
    width: '98px',
  },
  rightLabel: {
    textAlign: 'left',
    fontSize: '16px',
    display: 'inline-block',
    marginLeft: '17px',
    verticalAlign: 'top',
  },
});

class CurrencyLabel extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { classes } = this.props;
    return (
      <div className={classes.root}>
        <div className={classes.leftLabel}>
          <FormattedMessage id={this.props.label} />
        </div>
        <div className={classes.rightLabel}>{this.props.value}</div>
      </div>
    );
  }
}

CurrencyLabel.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(CurrencyLabel);
