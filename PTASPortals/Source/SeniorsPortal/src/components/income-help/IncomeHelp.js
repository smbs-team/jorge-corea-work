//-----------------------------------------------------------------------
// <copyright file="IncomeHelp.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React from 'react';
import './IncomeHelp.css';
import Income from './Income';
import * as fm from './FormatTexts';
import ReactToPrint from 'react-to-print';
import CustomButton from '../common/CustomButton';
import { withStyles } from '@material-ui/core/styles';

const CustomButtonCss = withStyles({
  button: {
    marginTop: '26px',
    backgroundColor: 'white',
    color: '#6f1f62',
    borderStyle: 'solid',
    borderWidth: '2px',
    borderColor: '#6f1f62',
    fontSize: '16px',
  },
})(CustomButton);

export default class IncomeHelp extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <Income ref={el => (this.componentRef = el)} />
      </div>
    );
  }
}
