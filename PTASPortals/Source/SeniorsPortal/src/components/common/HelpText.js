import React from 'react';
import './HelpText.css';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';

const styles = theme => ({
  root: {
    textAlign: 'left',
    fontSize: '0.8em',
  },
});

function HelpText({ label, style, ...rest }) {
  return (
    <div className={styles.root} style={style} {...rest}>
      {label}
    </div>
  );
}

HelpText.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(HelpText);
