import React, { label, value, className } from 'react';

import { withStyles } from '@material-ui/core/styles';
import { FormattedMessage } from 'react-intl';
import PropTypes from 'prop-types';

const styles = theme => ({
  boolValue: {
    display: 'inline-block',
    verticalAlign: 'top',
    fontSize: '12px',
  },
  label: {
    fontSize: '12px',
    marginLeft: '19px',
    display: 'inline-block',
    maxWidth: '290px',
  },
});

class BoolLabel extends React.Component {
  constructor(props) {
    super(props);
  }

  getBoolString(value) {
    if (value) {
      return <FormattedMessage id="yes" defaultMessage="Yes" />;
    } else {
      return <FormattedMessage id="no" defaultMessage="No" />;
    }
  }

  render() {
    const { classes } = this.props;
    return (
      <div className={this.props.className}>
        <div className={classes.boolValue}>
          {this.getBoolString(this.props.value)}
        </div>
        <div className={classes.label}>{this.props.label}</div>
      </div>
    );
  }
}

BoolLabel.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(BoolLabel);
