import React from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import './FullPageSpinner.css';
import { makeStyles, withStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
  progress: {
    //margin: theme.spacing(2),

    width: '100px !important',
    height: '100px !important',
  },
}));

const ColorCircularProgress = withStyles({
  barColorPrimary: {
    backgroundColor: '#a5c727',
  },
  colorPrimary: {
    color: '#a5c727',
  },
})(CircularProgress);

export default function FullPageSpinner() {
  const classes = useStyles();

  return (
    <div className="full-spinner">
      <div className="full-spinner_center">
        <ColorCircularProgress className={classes.progress} />
      </div>
    </div>
  );
}
