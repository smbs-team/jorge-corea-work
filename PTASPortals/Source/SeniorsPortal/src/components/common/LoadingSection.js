import React from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import './LoadingSection.css';

const LoadingSection = props => {
  return (
    <div className="loading-section">
      <h1>{props.label}</h1>
      <CircularProgress />
    </div>
  );
};

export default LoadingSection;
