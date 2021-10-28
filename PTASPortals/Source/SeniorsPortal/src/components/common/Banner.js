/*
//-----------------------------------------------------------------------
// <copyright file="Banner.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
*/

import React from 'react';
import PropTypes from 'prop-types';
import './Banner.css';

/**
 * Stateless component to display a banner inside a page.
 * @param {*} props
 * @returns component to render
 */
const Banner = props => {
  return (
    <div
      className={`banner ${
        props.level
          ? props.level === 'error'
            ? 'banner-error'
            : props.level === 'warning'
            ? 'banner-warning'
            : 'banner-information'
          : 'banner-information'
      }`}
    >
      {props.message}
    </div>
  );
};

Banner.propTypes = {
  message: PropTypes.oneOfType([PropTypes.string, PropTypes.object]),
  level: PropTypes.oneOf(['error', 'warning', 'information']),
};

export default Banner;
