//-----------------------------------------------------------------------
// <copyright file="Footer.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React from 'react';
import { FormattedMessage } from 'react-intl';

function Footer() {
  return (
    <footer className="content-wrapper footer inverse ">
      <div className="content space-top-42 space-bottom-32">
        <div className="footer-links body-small">
          <ul>
            <li>
              <a href="https://www.kingcounty.gov/tools/contact-us">
                <FormattedMessage
                  id="contactUs"
                  defaultMessage="Contact us"
                  description="Contact us "
                />
              </a>
            </li>
            <li>
              <a href="https://www.kingcounty.gov/elected/executive/customer-service">
                <FormattedMessage
                  id="customerService"
                  defaultMessage="Customer service"
                  description="Customer service"
                />
              </a>
            </li>
            <li>206-296-3920</li>
          </ul>
          <ul>
            <li>
              <a href="https://www.kingcounty.gov/about/website/privacy">
                <FormattedMessage
                  id="privacyPolicy"
                  defaultMessage="Privacy policy"
                  description="Privacy policy"
                />
              </a>{' '}
            </li>
            <li>
              <a href="https://www.kingcounty.gov/about/website/termsofuse">
                <FormattedMessage
                  id="termsOfUse"
                  defaultMessage="Terms of use"
                  description="Terms of use"
                />
              </a>{' '}
            </li>
            <li style={{ fontSize: '14px' }}>
              © {new Date().getFullYear()} King County, WA
            </li>
          </ul>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
