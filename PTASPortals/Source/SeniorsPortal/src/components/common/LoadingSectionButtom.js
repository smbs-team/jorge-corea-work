import React from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import './LoadingSectionButtom.css';

const LoadingSectionButtom = props => {
  return (
    <div className="loading-button-section">
      <h1>{props.label}</h1>
      <CircularProgress />
    </div>
  );
};

export default LoadingSectionButtom;
