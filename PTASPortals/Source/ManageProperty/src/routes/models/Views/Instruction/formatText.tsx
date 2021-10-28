// Instruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';
import { Link } from 'react-router-dom';

// titles
export const title = (
  <FormattedMessage
    id="instruction_principal_title"
    defaultMessage="How to manage your property"
  />
);

export const signedInTitle = (
  <FormattedMessage
    id="instruction_signedIn_title"
    defaultMessage="Signing in"
  />
);

export const addPropertyTitle = (
  <FormattedMessage
    id="instruction_add_property_title"
    defaultMessage="Add property"
  />
);

export const uptMailingAddressTitle = (
  <FormattedMessage
    id="instruction_upt_mailing_title"
    defaultMessage="Update mailing address"
  />
);

export const paperlessTitle = (
  <FormattedMessage
    id="instruction_paperless_title"
    defaultMessage="Switch to paperless value notices"
  />
);

export const exemptionsTitle = (
  <FormattedMessage
    id="instruction_paperless_title"
    defaultMessage="Manage and apply for tax exemptions "
  />
);

export const requestAdjustmentsTitle = (
  <FormattedMessage
    id="instruction_request_adjustments_title"
    defaultMessage="Request adjustments to assessed value"
  />
);

export const bussinessPersonalProperty = (
  <FormattedMessage
    id="instruction_bussiness_personal_property_title"
    defaultMessage="Update business personal property"
  />
);

export const signingInContent = (
  <FormattedMessage
    id="instruction_signing_in_content"
    defaultMessage={`
      {description}
      {content}
      {instruction}
    `}
    values={{
      description: (
        <span>
          You have a few options for signing in, whether you’re new or returning
          to a King County web account.
        </span>
      ),
      content: (
        <ul>
          <li>
            Sign in with email
            <ul>
              <li>
                With a password. If you’re new or you’ve migrated from a legacy
                account, we’ll ask you to create a password.
              </li>
              <li>
                With an email we send you each time you sign in. With this
                option, you don’t need to create or remember a password. It’s
                also useful if you’ve forgotten your password.
              </li>
            </ul>
          </li>
          <li>
            Sign in with an account provider
            <ul>
              <li>
                This is an easy option if you’re already signed in with Google,
                Apple, or Microsoft using the email address you’d like to use
                with King County.
              </li>
              <li>
                Your provider email address should be the one you want to use
                for King County communications
              </li>
              <li>
                That provider handles additional security options, depending on
                your settings.
              </li>
            </ul>
          </li>
        </ul>
      ),
      instruction: (
        <span>
          Regardless of your preferred option, be sure to use your correct email
          address.
        </span>
      ),
    }}
  />
);

export const addPropertyContent = (
  <FormattedMessage
    id="instruction_add_property_content"
    defaultMessage={`
      {description}
      {content}
    `}
    values={{
      description: <span>You can find your property by:</span>,
      content: (
        <ul>
          <li>Address</li>
          <li>Parcel number</li>
          <li>Condo name</li>
          <li>Mobile park name</li>
          <li>Marina name</li>
          <li>Commercial project name</li>
          <li>
            Current location (if you allow location services in your browser)
          </li>
        </ul>
      ),
    }}
  />
);

export const uptMailingAddressDesc = (
  <FormattedMessage
    id="instruction_upt_mailing_address_desc"
    defaultMessage="You can update your mailing address and the name(s) of the recipient(s). This doesn’t affect the owner property owner name."
  />
);

export const paperlessContent = (
  <FormattedMessage
    id="instruction_paperless_content_content"
    defaultMessage={`
      {description}
      {content}
    `}
    values={{
      description: (
        <span>
          You can get official property value notices delivered to your email
          inbox instead of your physical mailbox.
        </span>
      ),
      content: (
        <ul>
          <li>
            To confirm ownership, you need the account number and EVN code on
            your value notice.
          </li>
          <li>
            To receive paper notices again, you can turn off this service.
          </li>
        </ul>
      ),
    }}
  />
);

export const exemptionContent = (
  <FormattedMessage
    id="instruction_exemption_content"
    defaultMessage={`
      {description}
      {content}
      {descriptionToHome}
      {homeImprovementList}
      {currentUseDesc}
      {currentUseList}
    `}
    values={{
      description: <span>Senior Exemption</span>,
      descriptionToHome: <span>Home improvement</span>,
      currentUseDesc: <span>Current use</span>,
      currentUseList: (
        <ul>
          <li>
            Exemptions for land being used for farming, agriculture, and
            forestry.
          </li>
        </ul>
      ),
      homeImprovementList: (
        <ul>
          <li>
            Exemptions based on your ownership, occupancy, age, disability, and
            income.
          </li>
          <li>
            For more info, go to{' '}
            <Link to="/">Senior Exemption instructions.</Link>
          </li>
        </ul>
      ),
      content: (
        <ul>
          <li>
            Exemptions for land being used for farming, agriculture, and
            forestry.
          </li>
        </ul>
      ),
    }}
  />
);

export const requestAdjustmentsContent = (
  <FormattedMessage
    id="instruction_exemption_content"
    defaultMessage={`
      {description}
      {content}
      {destroyedDesc}
      {destroyedList}
    `}
    values={{
      description: <span>Property value appeal</span>,
      destroyedDesc: <span>Destroyed property</span>,
      destroyedList: (
        <ul>
          <li>Request a reduced assessed value due to damage or loss.</li>
        </ul>
      ),
      content: (
        <ul>
          <li>Request a new value with supporting evidence.</li>
          <li>
            For more info, go to{' '}
            <Link to="/">Property value appeal instructions.</Link>
          </li>
        </ul>
      ),
    }}
  />
);

export const bussinessPersonalPropertyContent = (
  <FormattedMessage
    id="instruction_exemption_content"
    defaultMessage={`
      {description}
      {content}
    `}
    values={{
      description: (
        <span>Manage your business listing info and current assets.</span>
      ),
      content: (
        <ul>
          <li>Request a new value with supporting evidence.</li>
          <li>
            For more info, go to{' '}
            <Link to="/">Business personal property instructions.</Link>
          </li>
        </ul>
      ),
    }}
  />
);
