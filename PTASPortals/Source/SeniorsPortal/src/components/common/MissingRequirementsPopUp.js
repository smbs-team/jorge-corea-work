import './MissingRequirementsPopUp.css';
import React from 'react';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Collapse from '@material-ui/core/Collapse';
import MissingRequirementsContent from './MissingRequirementsContent';

const MissingRequirementsPopUp = props => {
  return (
    <Collapse
      className="missing-requirements-popup"
      in={props.show}
      style={{
        top:
          props.missingFields.length > 0 &&
          props.refsFields &&
          props.refsFields[props.missingFields[0]]
            ? props.refsFields[props.missingFields[0]].offsetTop - 200
            : 0,
      }}
    >
      <IconButton
        onClick={props.onClose}
        className="missing-requirements-close"
      >
        <ClearIcon className="missing-requirements-close-icon" />
      </IconButton>
      <MissingRequirementsContent {...props} />
    </Collapse>
  );
};

export default MissingRequirementsPopUp;
