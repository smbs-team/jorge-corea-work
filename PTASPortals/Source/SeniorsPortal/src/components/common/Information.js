import React, { style } from 'react';
import './Information.css';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';

export default class Information extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="info-icon" style={this.props.style}>
        <InfoOutlinedIcon />
      </div>
    );
  }
}
