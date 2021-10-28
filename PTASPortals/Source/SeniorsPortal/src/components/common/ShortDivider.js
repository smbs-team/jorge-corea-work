import React, { label, value } from 'react';
import './ShortDivider.css';
import { withStyles } from '@material-ui/core/styles';
import Divider from '@material-ui/core/Divider';
import PropTypes from 'prop-types';

const styles = theme => ({
  root: {
    width: '920px',
    maxWidth: '920px',
    alignContent: 'center',
    display: 'inline-block',
    backgroundColor: 'black',
    border: 'solid',
    borderWidth: '0.1px',
  },
});

class ShortDivider extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { classes } = this.props;
    return <Divider className={classes.root} />;
  }
}

ShortDivider.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(ShortDivider);
