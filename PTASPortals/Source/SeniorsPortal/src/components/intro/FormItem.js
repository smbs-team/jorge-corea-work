import React from 'react';
import './FormItem.css';
import { FormattedMessage } from 'react-intl';

const FormItem = props => {
  return (
    <div className="form-item">
      <p className="body-large">{props.titleId}</p>
      <p className="body-large">{props.message}</p>
      {props.children}
    </div>
  );
};

export default FormItem;
