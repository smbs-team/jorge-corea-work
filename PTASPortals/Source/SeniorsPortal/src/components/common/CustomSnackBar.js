import React, { label } from 'react';
import './CustomWarning.css';
import WarningIcon from '@material-ui/icons/Warning';

class CustomWarning extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="custom-warning">
        <WarningIcon className="warning-icon" />
        <div className="warning-text">{this.props.label}</div>
      </div>
    );
  }
}

export default CustomWarning;
