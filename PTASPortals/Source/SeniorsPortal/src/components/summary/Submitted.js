import React, { style } from 'react';
import './Summary.css';
import * as fm from './FormatTexts';
import CustomButton from '../common/CustomButton';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import { Link } from 'react-router-dom';

export default class Submitted extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <div style={{ marginTop: '20px' }}> {fm.thankYouFiling} </div>
        <Link to="/home">
          <CustomButton label={fm.backToHome} />
        </Link>
      </div>
    );
  }
}
