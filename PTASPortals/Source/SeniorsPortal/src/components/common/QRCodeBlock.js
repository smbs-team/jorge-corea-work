//-----------------------------------------------------------------------
// <copyright file="QRCodeBlock.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';
import { renderIf } from '../../lib/helpers/util';
import './QRCodeBlock.css';
import PropTypes from 'prop-types';
import { FormattedMessage } from 'react-intl';

/**
 * Stateless component to display the QR Code on the page.
 * Shows a loading indicator when no QR image available.
 *
 * @param {*} props
 * @returns component to render
 */
const QRCodeBlock = props => {
  return (
    <div className="qrcodeblock">
      {renderIf(
        props.QRCode,
        <img className="qrcodeblock-img" src={props.QRCode} />,
        <CircularProgress size={100} />
      )}
      <p className="qrcodeblock_description">
        <FormattedMessage
          id="QRCode_description"
          defaultMessage="Scan this code with the SEMS app."
        />
      </p>
    </div>
  );
};

QRCodeBlock.propTypes = {
  QRCode: PropTypes.string,
};

export default QRCodeBlock;
