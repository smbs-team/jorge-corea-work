import './MissingRequirementsContent.css';
import React from 'react';
import WarningIcon from '@material-ui/icons/Warning';
import { FormattedMessage } from 'react-intl';

const MissingRequirementsContent = props => {
  return (
    <div className="missing-requirements">
      <WarningIcon className="required-icon" />
      <ul>
        <p>
          <FormattedMessage
            id="myInfo_toContinue"
            defaultMessage="To continue, you need to enter your information:"
          />
        </p>
        {props.missingFields.map((field, index) => {
          return (
            <li key={index}>
              <a
                onClick={() => {
                  props.onMessageClick();
                  if (field !== 'yearsOptions') {
                    props.refsFields[field].scrollIntoView({
                      block: 'center',
                      behavior: 'smooth',
                    });
                  } else if (props.displayYearsOptions) {
                    props.refsFields[field].scrollIntoView({
                      block: 'center',
                      behavior: 'smooth',
                    });
                  }
                }}
                href={'#' + field}
              >
                {props.formattedStrings[field]
                  ? props.formattedStrings[field]
                  : props.formattedStrings[`${field}Missing`]}
              </a>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default MissingRequirementsContent;
